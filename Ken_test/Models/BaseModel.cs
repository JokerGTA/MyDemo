using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Ken_test.Models
{
    public class BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        [Column(TypeName = "DateTime")] //指定为解决自动生成时,为DateTime(0)
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 编辑时间
        /// </summary>
        [Column(TypeName = "DateTime")] //指定为解决自动生成时,为DateTime(0)
        public DateTime? EditTime { get; set; }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        [Required]
        public bool IsDeleted { get; set; }
    }
}
