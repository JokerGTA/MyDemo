using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ken_test.Dtos
{
    public class UserModifyDto
    {
        /// <summary>
        /// 用户昵称
        /// </summary>        
        public string NickName { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>        
        public string IPAddress { get; set; }

        /// <summary>
        /// 头像图片
        /// </summary>        
        public string HeadPicture { get; set; }

    }
}
