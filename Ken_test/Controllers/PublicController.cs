using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace Ken_test.Controllers
{
    [ApiController]
    [Route("api/public")]
    public class PublicController : Controller
    {
        /// <summary>
        /// 文件重命名
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ConvertFileName()
        {
            string path = @"D:\BaiduNetdiskDownload\新建文件夹";
            DirectoryInfo theFolder = new DirectoryInfo(path);

            //遍历文件
            foreach (FileInfo NextFile in theFolder.GetFiles())
            {
                string srcFileName = NextFile.Name;
                string destFileName = srcFileName.Replace("周杰伦 - ", "");
                if (System.IO.File.Exists($"{path}/{srcFileName}"))
                {
                    System.IO.File.Move($"{path}/{srcFileName}", $"{path}/{ destFileName}");
                }
            }

            //遍历文件夹
            foreach (DirectoryInfo NextFolder in theFolder.GetDirectories())
            {

            }

            return Ok();
        }

        /// <summary>
        /// 自动生成sql
        /// </summary>
        /// <returns></returns>
        [HttpGet("SQL")]
        public IActionResult AutoGenerationSQL()
        {
            string path = @"D:\BaiduNetdiskDownload\新建文件夹";
            DirectoryInfo theFolder = new DirectoryInfo(path);
            string[] sqls = new string[110];
            int i = 0;
            //遍历文件
            foreach (FileInfo NextFile in theFolder.GetFiles())
            {
                string srcFileName = NextFile.Name;
                string name = srcFileName.Substring(0, srcFileName.IndexOf("."));
                string sql = $"INSERT INTO ken_music (`CreateTime`,`Url`,`CoverImgUrl`,`Title`) VALUES (NOW(),\"https://www.93yz95rz.club/files/music_jay/{srcFileName}\"," +
                    $"\"https://ss0.bdstatic.com/94oJfD_bAAcT8t7mm9GUKT-xh_/timg?image&quality=100&size=b4000_4000&sec=1603962932&di=7cbcc37bc92c5e9f1517014e1b7f6492&src=http://file.digitaling.com/eImg/uimages/20180930/1538303931413950.jpg\"," +
                    $"\"{name}\")";

                sqls[i] = sql;
                i++;
            }
            return Ok(sqls);
        }



    }
}
