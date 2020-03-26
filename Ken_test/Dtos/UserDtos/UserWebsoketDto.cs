using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ken_test.Dtos
{
    public class UserWebsoketDto
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户头像图片
        /// </summary>
        public string UserHeadPic { get; set; }
    }
}
