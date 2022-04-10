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
            if (fm != null && fm != "" && lm != null && lm != "")
            {
                return View(new Dictionary<string, int>() {
                     { lm , 1},
                    { fm,0 },

                });
            }

            if(fm != null && fm != "")
            {
                return View(new Dictionary<string, int>() {
                    
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
            string fastMessage = "none";

            if (mail == null || mail == "" || message == null || message == "")
            {
                fastMessage = "Не корректное обращение, попробуйте снова.";
                var messageUnicode2 = String.Join("/", fastMessage.Split("/").Select(s => WebUtility.UrlEncode(s)));

                return new RedirectResult(url: $"/Messages/Message?fm={messageUnicode2}", permanent: true,
                          preserveMethod: true);
            }

            Console.WriteLine($"SEND MESSAGE");
            //Костыль на проверку вопроса.

            DataAnalyzer dataAnalyzer = new DataAnalyzer();

            string messageCategory = "";

            int emotionValue = dataAnalyzer.GetEmotionMessage(message);

            fastMessage = "Пожалуйста отправьте жалобу заново!";

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

                Console.WriteLine($"PRESEARCH");

                if (semanticAnalyzeInfo.entities.Count == 0)
                {
                    string searchValue = "";
                   


                    foreach (var s in semanticAnalyzeInfo.cases)
                    {
                        searchValue = searchValue + $"{s} ";
                    }

                    Console.WriteLine($"SEARCH LINE: {searchValue}");

             

                    cmd = new MySqlCommand($"SELECT * FROM UserRequests WHERE MATCH (Cases) AGAINST ('{searchValue}');", conn);

                    

                    using (var reader = cmd.ExecuteReader())
                    {
                        float bestPercent = 0f;
                        string bestAnswer = "";

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
                                    if(mCase.Equals(childCase))
                                    {
                                        matchedCases = matchedCases + 1;
                                    }
                                }
                            }

                           
                            float percent = matchedCases / bigCountCases;

                            Console.WriteLine($"Percent BY Search Line: {percent}");

                            if (percent > 0.49f)
                            {

                                if (percent > bestPercent)
                                {
                                    


                                    if (reader["Answer"] != null)
                                    {

                                        bestPercent = percent;
                                        bestAnswer = reader["Answer"].ToString();

                 
                                    }
                                }
                            }

                        }


                        if(bestAnswer != "")
                        {
                            Console.WriteLine($"Answer: {bestAnswer.ToString()}");

                            fastMessage = bestAnswer.ToString();

                            var messageUnicode2 = String.Join("/", fastMessage.Split("/").Select(s => WebUtility.UrlEncode(s)));

                            return new RedirectResult(url: $"/Messages/Message?fm={messageUnicode2}", permanent: true,
                                      preserveMethod: true);
                        }



                        reader.Close();
                        reader.Dispose();
                    }
                }


                cmd = new MySqlCommand($"INSERT INTO `Chats` (`User`,`Messages`) VALUES ('{mail}','{message}')", conn);

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


            var messageUnicode = fastMessage != "" ? string.Join("/", fastMessage.Split("/").Select(s => WebUtility.UrlEncode(s))) : "";
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