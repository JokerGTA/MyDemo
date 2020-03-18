using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ken_test.Models
{
    public class MessageLog : BaseModel
    {
        [StringLength(500)]
        public string MsgContext { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual UserInfo UserInfo { get; set; }
    }
}

