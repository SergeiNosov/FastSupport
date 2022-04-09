using System;
using System.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FastSupportFixed.Models;
using MySqlConnector;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace FastSupportFixed.Controllers
{
    public class MessagesController : Controller
    {
        private IConfiguration _configuration;
        private readonly ILogger<MessagesController> _logger;

        public MessagesController(ILogger<MessagesController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }



        [Route("[controller]/Message")]
        public IActionResult Message()
        {
            return View(GetLastMessages("testToken"));
        }


        public Dictionary<string, int> GetLastMessages(string userToken)
        {
            return new Dictionary<string, int>()
            {
                {"Hello!", 1},
                {"Where are my money!!!", 1},
                {"Stop! Wait please 5 minutes!", 0}
            };
        }


        [HttpGet, Route("[controller]/Send")]
        public IActionResult SendMessage(string data)
        {
            
            using (MySqlConnection conn = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand($"INSERT INTO `Chats` (`User`,`Messages`) VALUES ('freeToken','{data}')", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    /*
                    while (reader.Read())
                    {
                    }*/

                    
                }
            }

            return new RedirectResult(url: "/Messages/Message", permanent: true,
                            preserveMethod: true);
        }

        public ActionResult GetMessage()
        {
            string message = "Welcome";
            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
