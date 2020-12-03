using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ken_test.Bos;
using Microsoft.AspNetCore.Mvc;

namespace Ken_test.Controllers
{
    public class WeChatController : Controller
    {
        private readonly BoPriver _boProvider;
        private NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public WeChatController(BoPriver boPriver)
        {
            _boProvider = boPriver;
        }



    }
}