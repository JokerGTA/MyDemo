﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ken_test.Bos;
using Ken_test.Common;
using Ken_test.Controllers.CFilter;
using Ken_test.Dtos;
using Ken_test.Middlewares;
using Ken_test.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ken_test.Controllers
{
    [ApiController]
    [Route("api/room")]
    public class RoomController : Controller
    {
        private readonly BoPriver _boProvider;
        private NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public RoomController(BoPriver boPriver)
        {
            _boProvider = boPriver;
        }

        /// <summary>
        /// 发消息
        /// </summary>
        /// <returns></returns>
        [HttpPost("submit")]
        //[ApiFilter(ApiType = ApiType.Admin)]
        public IActionResult SubmitMessage([FromForm]string context, [FromForm]string ip)
        {
            UserInfo user = _boProvider._userInfoRepo.GetByIp(ip);
            if (user == null)
                user = new UserInfo { NickName = ip };

            var userBo = _boProvider.NewBo<UserBo, UserInfo>(user);
            userBo.SendMsg(context);
            return Ok();
        }

        /// <summary>
        /// 获取聊天室数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("data")]
        //[ApiFilter(ApiType = ApiType.Admin)]
        public IActionResult GetMessage()
        {
            var messages = _boProvider._messageLogRepo.GetLastTwenty();
            var result = new RoomDataDto
            {
                OnlineUser = WebSocketHandler.GetOnlineUserData(),
                OldMessage = _boProvider._mapper.Map<List<MessageRoomDto>>(messages)
            };
            return Ok(result);
        }

        /// <summary>
        /// 获取消息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiFilter]
        //[ApiFilter(ApiType = ApiType.Admin)]
        public IActionResult GetMessage(string ip)
        {
            _logger.Debug($"{ip}：来了。");
            UserInfo user = _boProvider._userInfoRepo.GetByIp(ip);
            var messageLogs = _boProvider._messageLogRepo.GetAllList();
            var result = _boProvider._mapper.Map<List<MessgesDtoF>>(messageLogs);
            if (user != null)
            {
                result.ForEach(p =>
                {
                    p.IsMine = p.UserId == user.Id;
                });
            }

            return Ok(result);
        }


        /// <summary>
        /// 向前端发送消息
        /// </summary>
        /// <returns></returns>
        [HttpGet("receive")]
        [ApiFilter]
        //[ApiFilter(ApiType = ApiType.Admin)]
        public void ReceiveMessage(string msg)
        {
            WebSocketHandler.AdminSendMessage(msg);
        }
    }
}