using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ken_test.Controllers.CFilter
{
    public class ApiFilter :Attribute, IAuthorizationFilter
    {
        public async void OnAuthorization(AuthorizationFilterContext context)
        {           
            if (context.HttpContext.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await context.HttpContext.WebSockets.AcceptWebSocketAsync();
               // var result =await  RecvAsync(webSocket, CancellationToken.None);
                //TODO
            }
        }

        ///// <summary>
        ///// 接收客户端数据
        ///// </summary>
        ///// <param name="webSocket">webSocket 对象</param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //public static async Task<string> RecvAsync(WebSocket webSocket, CancellationToken cancellationToken)
        //{
        //    int isFirst = 0;
        //    string oldRequestParam = "";
        //    WebSocketReceiveResult result;
        //    do
        //    {
        //        var ms = new MemoryStream();
        //        var buffer = new ArraySegment<byte>(new byte[1024 * 8]);
                
        //        result = await webSocket.ReceiveAsync(buffer, cancellationToken);
        //        if (result.MessageType.ToString() == "Close")
        //        {
        //            break;
        //        }
        //        ms.Write(buffer.Array, buffer.Offset, result.Count - buffer.Offset);
        //        ms.Seek(0, SeekOrigin.Begin);
        //        var reader = new StreamReader(ms);
        //        var s = reader.ReadToEnd();
        //        reader.Dispose();
        //        ms.Dispose();
        //        if (!string.IsNullOrEmpty(s))
        //        {                
        //            //await SendMessage(webSocket, s);
        //        }
        //        oldRequestParam = s;

        //    } while (result.EndOfMessage);

        //    return "";
        //}




        //private async Task SendMessage<TEntity>(WebSocket webSocket, TEntity entity)
        //{
        //    var Json = JsonConvert.SerializeObject(entity);
        //    var bytes = Encoding.UTF8.GetBytes(Json);

        //    await webSocket.SendAsync(
        //        new ArraySegment<byte>(bytes),
        //        WebSocketMessageType.Text,
        //        true,
        //        CancellationToken.None
        //    );
        //}
    }
}
