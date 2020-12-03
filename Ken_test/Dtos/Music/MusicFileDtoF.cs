using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ken_test.Dtos
{
    public class MusicFileDtoF
    {
        /// <summary>
        /// 文件夹名称
        /// </summary>        
        public string Name { get; set; }

        /// <summary>
        /// 文件夹封面图片
        /// </summary>        
        public string CoverUrl { get; set; }

        /// <summary>
        /// 所含数量
        /// </summary>
        public int Count { get; set; }

        public int Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
