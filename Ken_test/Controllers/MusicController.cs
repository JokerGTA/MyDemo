using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ken_test.Bos;
using Ken_test.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Ken_test.Controllers
{
    [ApiController]
    [Route("api/music")]
    public class MusicController : Controller
    {
        private readonly BoPriver _boProvider;
        private NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public MusicController(BoPriver boPriver)
        {
            _boProvider = boPriver;
        }

        /// <summary>
        /// 获取音乐列表
        /// </summary>
        /// <param name="musicFileId"></param>
        /// <param name="token"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetMusicList(int musicFileId, string token = "", int pageSize = 10, int pageIndex = 1)
        {
            if (!token.Equals("470733450~RDrEQqBA.06ba3350f5040ba"))
                return new UnauthorizedResult();

            var query = _boProvider._context.Musics.Where(m=>1 == 1);
            if (musicFileId > 0)
            {
                query = query.Where(m => m.MusicFileId == musicFileId);
            }
            
            var lstMusic = query.OrderBy(m => m.Sort).ThenBy(m => m.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            var result = _boProvider._mapper.Map<List<MusicDtoF>>(lstMusic);
            return Ok(result);
        }

        /// <summary>
        /// 获取音乐文件夹
        /// </summary>
        /// <returns></returns>
        [HttpGet("files")]
        public IActionResult GetMusicFileList(string token = "")
        {
            if (!token.Equals("470733450~RDrEQqBA.06ba3350f5040ba"))
                return new UnauthorizedResult();

            var lstMusicFile = _boProvider._context.MusicFiles.OrderBy(m => m.Id).ToList();
            var result = _boProvider._mapper.Map<List<MusicFileDtoF>>(lstMusicFile);
            return Json(result);
        }


    }
}
