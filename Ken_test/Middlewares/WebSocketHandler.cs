using Ken_test.Bos;
using Ken_test.Dtos;
using Ken_test.Models;
using Ken_test.Models.Enums;
using Ken_test.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ken_test.Middlewares
{
    public static class WebSocketHandler
    {
        static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        static Dictionary<string, SocketObject> dicWebsockets { get; set; } = new Dictionary<string, SocketObject>();
        static BoPriver _boProvider;
        /// <summary>
        /// 路由绑定处理
        /// </summary>
        /// <param name="app"></param>
        public static void Map(IApplicationBuilder app)
        {
            app.Use(Acceptor);
        }

        /// <summary>
        /// 获取在线用户信息
        /// </summary>
        /// <returns></returns>
        public static List<UserWebsoketDto> GetOnlineUserData()
        {
            List<UserWebsoketDto> userWebsoketDtos = new List<UserWebsoketDto>();
            foreach (var item in dicWebsockets)
            {
                UserWebsoketDto userWebsoketDto = new UserWebsoketDto
                {
                    UserName = item.Value.UserWebsoket.NickName,
                    UserHeadPic = item.Value.UserWebsoket.HeadPicture,
                };
                userWebsoketDtos.Add(userWebsoketDto);
            }
            return userWebsoketDtos;
        }

        private static void SaveChange()
        {
            _boProvider._context.SaveChanges();
        }

        /// <summary>
        /// 创建链接
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        private async static Task Acceptor(HttpContext httpContext, Func<Task> next)
        {
            _boProvider = (BoPriver)httpContext.RequestServices.GetService(typeof(BoPriver)); // 这句可太NB了
            try
            {
                if (!httpContext.WebSockets.IsWebSocketRequest)
                    return;
                UserInfo userInfo = new UserInfo();
                userInfo.IPAddress = httpContext.Request.Query["userId"].ToArray()[0]; // userId暂时用ip替代
                userInfo.NickName = httpContext.Request.Query["userName"].ToArray()[0];
                userInfo.HeadPicture = httpContext.Request.Query["userHeadPic"].ToArray()[0];
                var socket = await httpContext.WebSockets.AcceptWebSocketAsync();
                if (dicWebsockets.ContainsKey(userInfo.NickName))
                    dicWebsockets.Remove(userInfo.NickName);
                dicWebsockets.Add(userInfo.NickName, new SocketObject { UserWebsoket = userInfo, WebSocket = socket });
                UserInfo user = _boProvider._userInfoRepo.GetByName(userInfo.NickName);
                if (user == null)
                {
                    user = userInfo;
                    user.MessageLogs = new List<MessageLog>() { };
                    _boProvider._context.UserInfos.Add(user);
                    SaveChange();
                }

                _logger.Debug($"服务端建立websockect链接{userInfo.IPAddress}||{userInfo.NickName}");
                // 建立连接后发送该用户进入聊天室提示信息 
                await SendMessage(socket, new SocketMessage
                {
                    WsType = WsMessageTypeEnum.Enter,
                    UserName = userInfo.NickName,
                    UserHeadPic = userInfo.HeadPicture,                    
                }, userInfo.NickName);
               
                await RecvAsync(user, socket, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logger.Debug($"又特么异常了啊啊啊啊啊啊：{ex.Message.ToString()}");
            }
        }

        /// <summary>
        /// 接收客户端数据
        /// </summary>
        /// <param name="userInfo"></param>       
        /// <param name="webSocket">webSocket 对象</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private static async Task RecvAsync(UserInfo userInfo, WebSocket webSocket, CancellationToken cancellationToken)
        {
            List<string> oldRequestParam = new List<string>();
            WebSocketReceiveResult result;
            try
            {
                do
                {
                    var ms = new MemoryStream();
                    var buffer = new ArraySegment<byte>(new byte[1024 * 8]);
                    _logger.Debug($"等待发送信息...");
                    result = await webSocket.ReceiveAsync(buffer, cancellationToken);
                    if (result.MessageType.ToString() == "Close")
                    {
                        break;
                    }
                    ms.Write(buffer.Array, buffer.Offset, result.Count - buffer.Offset);
                    ms.Seek(0, SeekOrigin.Begin);
                    var reader = new StreamReader(ms);
                    string s = reader.ReadToEnd();
                    reader.Dispose();
                    ms.Dispose();
                    if (!string.IsNullOrEmpty(s))
                    {
                        SocketMessage socketMessage = new SocketMessage
                        {
                            WsType = WsMessageTypeEnum.Chat,
                            UserName = userInfo.NickName,
                            UserHeadPic = userInfo.HeadPicture,
                            Msg = s
                        };
                        MessageLog messageLog = new MessageLog { MsgContext = s };
                        userInfo.MessageLogs.Add(messageLog);
                        _boProvider._context.UserInfos.Update(userInfo);
                        await SendMessage(webSocket, socketMessage, userInfo.NickName);
                        SaveChange();
                    }
                    oldRequestParam.Add(s);

                } while (result.EndOfMessage);

            }
            catch (Exception ex)
            {
                _logger.Debug($"发消息后的异常：{ex.Message.ToString()}");
            }
            finally
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Close", default(CancellationToken));
            }
        }

        private class SocketMessage
        {
            public WsMessageTypeEnum WsType { get; set; }
            public string UserName { get; set; }
            public string UserHeadPic { get; set; }
            public string Msg { get; set; }
        }

        private class SocketObject
        {
            public UserInfo UserWebsoket { get; set; }
            public WebSocket WebSocket { get; set; }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="webSocket"></param>
        /// <param name="entity"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        private static async Task SendMessage<TEntity>(WebSocket webSocket, TEntity entity, string userName)
        {
            var Json = JsonConvert.SerializeObject(entity);
            var bytes = Encoding.UTF8.GetBytes(Json);

            foreach (var item in dicWebsockets)
            {
                if (item.Key != userName)
                {
                    _logger.Debug($"{userName}发送消息给{item.Key}");
                    try
                    {
                        if (item.Value.WebSocket != null && item.Value.WebSocket.State.ToString() != "Closed")
                        {
                            await item.Value.WebSocket.SendAsync(
                            new ArraySegment<byte>(bytes),
                            WebSocketMessageType.Text,
                            true,
                            CancellationToken.None);
                        }
                        else
                        {
                            _logger.Debug($"{item.Key}不存在或已关闭");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Debug($"发消息时的异常：{item.Value.WebSocket.State.ToString()}||{ex.Message.ToString()}");
                    }
                }
            }
        }

        /// <summary>
        /// 后台群发
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static async void AdminSendMessage<TEntity>(TEntity entity)

        {
            var Json = JsonConvert.SerializeObject(entity);
            var bytes = Encoding.UTF8.GetBytes(Json);

            //foreach (var webSocket in webSockets)
            //{
            //    await webSocket.SendAsync(
            //    new ArraySegment<byte>(bytes),
            //    WebSocketMessageType.Text,
            //    true,
            //    CancellationToken.None);
            //}
        }



    }
}
