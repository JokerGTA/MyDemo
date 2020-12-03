using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ken_test.Models
{
    public class Music : BaseModel
    {
        /// <summary>
        /// 音乐地址
        /// </summary>
        [StringLength(200)]
        public string Url { get; set; }

        /// <summary>
        /// 封面图片地址
        /// </summary>
        [StringLength(500)]
        public string CoverImgUrl { get; set; }

        /// <summary>
        /// 音乐标题
        /// </summary>
        [StringLength(100)]
        public string Title { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 关联音乐文件夹外键
        /// </summary>
        public int? MusicFileId { get; set; }

        public virtual MusicFile MusicFile { get; set; }
    }
}
