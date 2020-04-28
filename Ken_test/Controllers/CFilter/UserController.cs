using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ken_test.Bos;
using Ken_test.Dtos;
using Ken_test.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ken_test.Controllers.CFilter
{
    [ApiController]
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly BoPriver _boProvider;
        private NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public UserController(BoPriver boPriver )
        {
            _boProvider = boPriver;
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("save")]
        public IActionResult SaveUserInfo([FromForm] UserModifyDto input)
        {            
            var user = _boProvider._userInfoRepo.GetByName(input.NickName);
            UserBo userBo = _boProvider.NewBo<UserBo, UserInfo>(user);
            userBo.Save(input);
            return Ok();
        }

    }
}