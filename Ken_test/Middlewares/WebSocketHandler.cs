using Ken_test.Dtos;
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
        static List<WebSocket> webSockets { get; set; } = new List<WebSocket>();
        static Dictionary<string, SocketObject> dicWebsockets { get; set; } = new Dictionary<string, SocketObject>();
        /// <summary>
        /// 路由绑定处理
        /// </summary>
        /// <param name="app"></param>
        public static void Map(IApplicationBuilder app)
        {
            app.Use(Acceptor);
        }

        /// <summary>
        /// 创建链接
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        private static async Task Acceptor(HttpContext httpContext, Func<Task> next)
        {

            if (!httpContext.WebSockets.IsWebSocketRequest)
                return;
            UserWebsoketDto userInfo = new UserWebsoketDto();
            string userId = httpContext.Request.Query["userId"].ToArray()[0];
            string userName = httpContext.Request.Query["userName"].ToArray()[0];
            string userHeadPic = httpContext.Request.Query["userHeadPic"].ToArray()[0];
            userInfo.UserId = userId;
            userInfo.UserName = userName;
            userInfo.UserHeadPic = userHeadPic;
            var socket = await httpContext.WebSockets.AcceptWebSocketAsync();
            if (!dicWebsockets.ContainsKey(userId))
                dicWebsockets.Add(userId, new SocketObject { UserWebsoket = userInfo, WebSocket = socket });

            var result = await RecvAsync(userInfo, socket, CancellationToken.None);
        }

        /// <summary>
        /// 接收客户端数据
        /// </summary>
        /// <param name="userInfo"></param>       
        /// <param name="webSocket">webSocket 对象</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private static async Task<string> RecvAsync(UserWebsoketDto userInfo, WebSocket webSocket, CancellationToken cancellationToken)
        {
            string userid = userInfo.UserId;
            List<string> oldRequestParam = new List<string>();
            WebSocketReceiveResult result;
            do
            {
                var ms = new MemoryStream();
                var buffer = new ArraySegment<byte>(new byte[1024 * 8]);

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
                        UserName = userInfo.UserName,
                        UserHeadPic = userInfo.UserHeadPic,
                        Msg = s
                    };
                    await SendMessage(webSocket, socketMessage, userid);
                }
                oldRequestParam.Add(s);

            } while (result.EndOfMessage);

            return "";
        }

        private class SocketMessage
        {
            public string UserName { get; set; }
            public string UserHeadPic { get; set; }
            public string Msg { get; set; }
        }

        private class SocketObject
        {
            public UserWebsoketDto UserWebsoket { get; set; }
            public WebSocket WebSocket { get; set; }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="webSocket"></param>
        /// <param name="entity"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        private static async Task SendMessage<TEntity>(WebSocket webSocket, TEntity entity, string userid)
        {
            var Json = JsonConvert.SerializeObject(entity);
            var bytes = Encoding.UTF8.GetBytes(Json);

            foreach (var item in dicWebsockets)
            {
                //if (item.Key.UserId != userid)
                //{
                await webSocket.SendAsync(
                    new ArraySegment<byte>(bytes),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None
                );
                //}
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

            foreach (var webSocket in webSockets)
            {
                await webSocket.SendAsync(
                new ArraySegment<byte>(bytes),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None);
            }
        }



    }
}
