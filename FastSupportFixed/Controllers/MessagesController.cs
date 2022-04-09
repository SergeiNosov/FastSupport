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


        /*public Dictionary<int, string> GetLastMessages(string token)
        {
            return new Dictionary<int, string>()
            {
                { 1, "Hello!"},
                { 1, "Where are my money!!!"},
                { 0, "Stop! Wait please 5 minutes!"}
            };
        }*/


        [Route("[controller]/Message")]
        public IActionResult Message()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
