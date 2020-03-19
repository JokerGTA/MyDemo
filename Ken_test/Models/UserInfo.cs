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

        public virtual List<MessageLog> MessageLogs { get; set; }
    }
}
