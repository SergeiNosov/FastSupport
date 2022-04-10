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
using FastSupportFixed.DataAnalysis;
using Newtonsoft.Json;

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
                {"Stop! Wait please 5 minutes!", 0},
                {"answer" , -1}
            };
        }


        [HttpGet, Route("[controller]/Send")]
        public IActionResult SendMessage(string message)
        {
            //Костыль на проверку вопроса.

            DataAnalyzer dataAnalyzer = new DataAnalyzer();

            string messageCategory = "";

            int emotionValue = dataAnalyzer.GetEmotionMessage(message);

            string fastMessage = "";

            if (emotionValue == 3 && !message.Contains("?"))
            {
                //Попросим пользователя болле конкретизировать своё обращение.
                fastMessage = "Спасибо за обращение! Пожалуйста уточните детали в обращении";

                return new RedirectResult(url: $"/Messages/Message?fm={fastMessage}", permanent: true,
                          preserveMethod: true);
            }

            if (emotionValue == 0)
            {
                //Попросим пользователя болле конкретизировать своё обращение.
                fastMessage = "Мы приняли вашу жалобу и рассмотрим её как можно скорее. Заранее приносим свои извинения.";
                messageCategory = "report";
            }

            if(emotionValue == 1)
            {
                fastMessage = "Спасибо за обращение! Мы как можно скорее ответим на ваше обращение!";
                messageCategory = "question";
            }


            KeywordsAnalizeInfo keywordsAnalizeInfo = dataAnalyzer.GetKeyWords(message);

            using (MySqlConnection conn = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand($"INSERT INTO `Chats` (`User`,`Messages`) VALUES ('freeToken','{message}')", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    /*
                    while (reader.Read())
                    {
                    }*/


                    reader.Close();
                    reader.Dispose();
                }



                cmd = new MySqlCommand($"INSERT INTO `UserRequests` (`Message`,`Keywords`, 'Category','SubCategory', 'ShortDescription') VALUES ('{message}','{JsonConvert.SerializeObject(keywordsAnalizeInfo.keywords)}','{messageCategory}','','','{keywordsAnalizeInfo.shortDescription}')", conn);


                using (var reader = cmd.ExecuteReader())
                {
                    /*
                    while (reader.Read())
                    {
                    }*/




                    reader.Close();
                    reader.Dispose();
                }

            }

            return new RedirectResult(url: $"/Messages/Message?fm={fastMessage}", permanent: true,
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
