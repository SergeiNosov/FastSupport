using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FastSupportFixed.Models;

namespace FastSupportFixed.Controllers
{
    public class MessagesController : Controller
    {
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(ILogger<MessagesController> logger)
        {
            _logger = logger;
        }


        [Route("[controller]/Message")]
        public IActionResult Message()
        {
            return View(GetLastMessages("testToken"));
        }


        public Dictionary<string, int> GetLastMessages(string token)
        {
            return new Dictionary<string, int>()
            {
                {"Hello!", 1},
                {"Where are my money!!!", 0},
                {"Stop! Wait please 5 minutes!", 1}
            };
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
