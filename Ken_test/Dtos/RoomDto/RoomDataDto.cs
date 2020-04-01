using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ken_test.Dtos
{
    public class RoomDataDto
    {
        /// <summary>
        /// 在线用户
        /// </summary>
        public List<UserWebsoketDto> OnlineUser { get; set; }

        /// <summary>
        /// 历史消息
        /// </summary>
        public List<MessageRoomDto> OldMessage { get; set; }
    }
}
