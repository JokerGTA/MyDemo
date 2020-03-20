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
            var socket = await httpContext.WebSockets.AcceptWebSocketAsync();
            webSockets.Add(socket);
            var result = await RecvAsync(socket, CancellationToken.None);

        }

        /// <summary>
        /// 接收客户端数据
        /// </summary>
        /// <param name="webSocket">webSocket 对象</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private static async Task<string> RecvAsync(WebSocket webSocket, CancellationToken cancellationToken)
        {
            int isFirst = 0;
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
                    await SendMessage(webSocket, s);
                }
                oldRequestParam.Add(s);

            } while (result.EndOfMessage);

            return "";
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="webSocket"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static async Task SendMessage<TEntity>(WebSocket webSocket, TEntity entity)
        {
            var Json = JsonConvert.SerializeObject(entity);
            var bytes = Encoding.UTF8.GetBytes(Json);

            await webSocket.SendAsync(
                new ArraySegment<byte>(bytes),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None
            );
        }

        /// <summary>
        /// 后台群发
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static void AdminSendMessage<TEntity>(TEntity entity)
        {
            var Json = JsonConvert.SerializeObject(entity);
            var bytes = Encoding.UTF8.GetBytes(Json);

            webSockets.ForEach(webSocket =>
            {
                webSocket.SendAsync(
                new ArraySegment<byte>(bytes),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None
            );
            });

        }
    }
}
