using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ken_test.Models
{
    public class MusicFile : BaseModel
    {
        /// <summary>
        /// 文件夹名称
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 文件夹封面图片
        /// </summary>
        [StringLength(200)]
        public string CoverUrl { get; set; }

        /// <summary>
        /// 所含数量
        /// </summary>
        public int Count { get => Musics.Count; }

        /// <summary>
        /// 所含音乐
        /// </summary>
        public virtual List<Music> Musics { get; set; }
    }
}
