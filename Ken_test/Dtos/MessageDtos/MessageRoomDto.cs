using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ken_test.Dtos
{
    public class MessageRoomDto
    {
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string HeadPicture { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string MsgContext { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public string CreateTime { get; set; }
    }
}
