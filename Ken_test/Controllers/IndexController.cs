﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ken_test.Bos;
using Microsoft.AspNetCore.Mvc;

namespace Ken_test.Controllers
{
    [ApiController]
    [Route("api/index")]
    public class IndexController : Controller
    {
        private readonly BoPriver _boProvider;
        private NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public IndexController(BoPriver boPriver)
        {
            _boProvider = boPriver;
        }

        /// <summary>
        /// 获取首页
        /// </summary>
        [HttpGet()]
        public IActionResult HomeIndex()
        {
            return View("index");
        }

    }
}