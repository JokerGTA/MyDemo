using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ken_test.Models
{
    public class UserInfo : BaseModel
    {
        /// <summary>
        /// 用户昵称
        /// </summary>
        [StringLength(50)]
        public string NickName { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [StringLength(50)]
        public string RealName { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        [StringLength(50)]
        public string Password { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        [StringLength(50)]
        public string IPAddress { get; set; }

        /// <summary>
        /// 头像图片
        /// </summary>
        [StringLength(50)]
        public string HeadPicture { get; set; }

        public virtual List<MessageLog> MessageLogs { get; set; }
    }
}
