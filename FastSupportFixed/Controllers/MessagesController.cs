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
using System.Text;

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



        [HttpGet, Route("[controller]/Message")]
        public IActionResult Message(string fm, string lm)
        {
            if (fm != null || fm != "" && lm != null)
            {
                return View(new Dictionary<string, int>() {
                     { lm , 1},
                    { fm,0 },

                });
            }

            return View(new Dictionary<string, int>()
            {
            });
        }



        [HttpGet, Route("[controller]/SendMessage")]
        public IActionResult SendMessage(string mail,string message)
        {
            //Костыль на проверку вопроса.

            DataAnalyzer dataAnalyzer = new DataAnalyzer();

            string messageCategory = "";

            int emotionValue = dataAnalyzer.GetEmotionMessage(message);

            string fastMessage = "none";

            if (emotionValue == 3 && !message.Contains("?"))
            {
                //Попросим пользователя болле конкретизировать своё обращение.
                fastMessage = "Спасибо за обращение! Пожалуйста уточните детали в обращении.";
                var messageUnicode2 = String.Join("/", fastMessage.Split("/").Select(s => WebUtility.UrlEncode(s)));

                return new RedirectResult(url: $"/Messages/Message?fm={messageUnicode2}", permanent: true,
                          preserveMethod: true);
            }

            if (emotionValue == 1)
            {
                //Попросим пользователя болле конкретизировать своё обращение.
                fastMessage = "Мы приняли вашу жалобу и рассмотрим её как можно скорее. Заранее приносим свои извинения.";
                messageCategory = "agressive";
            }

            if(emotionValue == 0)
            {
                fastMessage = "Спасибо за обращение! Мы как можно скорее ответим Вам!";
                messageCategory = "normal";
            }


            KeywordsAnalizeInfo keywordsAnalizeInfo = dataAnalyzer.GetKeyWords(message);
            SemanticAnalyzeInfo semanticAnalyzeInfo = dataAnalyzer.SemanticAnalyze(message);


            using (MySqlConnection conn = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {

                conn.Open();

                MySqlCommand cmd = new MySqlCommand();


                if (semanticAnalyzeInfo.entities.Count == 0)
                {
                    string searchValue = "";
                    StringBuilder stringBuilder = new StringBuilder();


                    foreach (var s in semanticAnalyzeInfo.cases)
                    {
                        searchValue = searchValue + $"{s} ";
                    }

                    Console.WriteLine($"SEARCH LINE: {searchValue}");

                    searchValue = stringBuilder.ToString();

                    cmd = new MySqlCommand($"SELECT * FROM UserRequests WHERE MATCH (Cases) AGAINST ('{searchValue}');", conn);

                    

                    using (var reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {

                            List<string> casesLoaded = JsonConvert.DeserializeObject<List<string>>(reader["Cases"].ToString());

                            int matchedCases = 0;
                            int bigCountCases = casesLoaded.Count > semanticAnalyzeInfo.cases.Count ? casesLoaded.Count : semanticAnalyzeInfo.cases.Count;


                            Console.WriteLine($"Success FIND BY Search Line: {casesLoaded.Count}");

                            foreach (string mCase in semanticAnalyzeInfo.cases)
                            {
                                foreach(string childCase in casesLoaded)
                                {
                                    if(mCase.Equals(matchedCases))
                                    {
                                        matchedCases = matchedCases + 1;
                                    }
                                }
                            }

                           
                            float percent = matchedCases / bigCountCases;

                            Console.WriteLine($"Percent BY Search Line: {percent}");

                            if (percent > 0.75f)
                            {

                                if (reader["Answer"] != null)
                                {
                                    fastMessage = reader["Answer"].ToString();

                                    var messageUnicode2 = String.Join("/", fastMessage.Split("/").Select(s => WebUtility.UrlEncode(s)));

                                    return new RedirectResult(url: $"/Messages/Message?fm={messageUnicode2}", permanent: true,
                                              preserveMethod: true);
                                }
                            }

                        }






                        reader.Close();
                        reader.Dispose();
                    }
                }


                cmd = new MySqlCommand($"INSERT INTO `Chats` (`User`,`Messages`) VALUES ('freeToken','{message}')", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    /*
                    while (reader.Read())
                    {
                    }*/


                    reader.Close();
                    reader.Dispose();
                }



                cmd = new MySqlCommand($"INSERT INTO `UserRequests` (`Message`,`Keywords`, `Category`,`SubCategory`, `ShortDescription`, `Entities`, `Cases`) VALUES ('{message}','{JsonConvert.SerializeObject(keywordsAnalizeInfo.keywords)}','{messageCategory}','subCategory','{keywordsAnalizeInfo.shortDescription}', '{JsonConvert.SerializeObject(semanticAnalyzeInfo.entities)}', '{JsonConvert.SerializeObject(semanticAnalyzeInfo.cases)}')", conn);


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


            var messageUnicode = String.Join("/", fastMessage.Split("/").Select(s => WebUtility.UrlEncode(s)));
            var lmUnicode = String.Join("/", message.Split("/").Select(s => WebUtility.UrlEncode(s)));


            string urlOrigin = $"/Messages/Message?fm={messageUnicode}&lm={lmUnicode}";
            //var urlEncode = String.Join("/", urlOrigin.Split("/").Select(s => WebUtility.UrlEncode(s)));

            return new RedirectResult(url: urlOrigin, permanent: true,
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