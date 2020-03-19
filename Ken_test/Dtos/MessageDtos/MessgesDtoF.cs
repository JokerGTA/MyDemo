using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ken_test.Dtos
{
    public class MessgesDtoF
    {
        public int Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 编辑时间
        /// </summary>
        public string EditTime { get; set; }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string MsgContext { get; set; }

        /// <summary>
        /// 发送人主键
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 发送人姓名
        /// </summary>
        public string UserName { get; set; }

        public bool IsMine { get; set; } = false;
    }
}
