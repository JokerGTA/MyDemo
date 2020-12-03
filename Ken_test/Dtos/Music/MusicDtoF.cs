using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ken_test.Dtos
{
    public class MusicDtoF
    {
        /// <summary>
        /// 音乐地址
        /// </summary>        
        public string Url { get; set; }

        /// <summary>
        /// 封面图片地址
        /// </summary>        
        public string CoverImgUrl { get; set; }

        /// <summary>
        /// 音乐标题
        /// </summary>        
        public string Title { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        
        public int Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
