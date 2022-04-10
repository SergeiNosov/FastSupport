using System.Collections.Generic;
using System.Diagnostics;
using FastSupportFixed.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using Newtonsoft.Json;

namespace FastSupportFixed.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _configuration = configuration;

            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult AdminPanel()
        {
            //Костыль.

            Dictionary<string, int> popularedCases = new Dictionary<string, int>();
            Dictionary<string, Dictionary<string, object>> requests = new Dictionary<string, Dictionary<string, object>>();

            using (MySqlConnection conn = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {

                conn.Open();

                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM UserRequests", conn);


                int counts = 0;

                int negativesCount = 0;
                int normalCounts = 0;

                using (var reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        requests[$"val{counts}"] = new Dictionary<string, object>();
                        if (reader["ShortDescription"] != null && reader["ShortDescription"] != "")
                        {
                            requests[$"val{counts}"]["txt"] = reader["ShortDescription"].ToString();
                        }
                        else
                        {
                            requests[$"val{counts}"]["txt"] = reader["Message"].ToString();

                        }

                        requests[$"val{counts}"]["Cases"] = JsonConvert.DeserializeObject <List<string>> (reader["Cases"].ToString());
                        requests[$"val{counts}"]["Category"] = reader["Category"].ToString();

                        if(requests[$"val{counts}"]["Category"] == "agressive")
                        {
                            negativesCount = negativesCount + 1;
                        }
                        else
                        {
                            normalCounts = normalCounts + 1;
                        }

                        counts = counts + 1;
                    }

                    requests[$"negatives"] = new Dictionary<string, object>();
                    requests[$"negatives"]["count"] = negativesCount;

                    requests[$"normals"] = new Dictionary<string, object>();
                    requests[$"normals"]["count"] = normalCounts;
                   
                    reader.Close();
                    reader.Dispose();
                }
            }


         
            return View(requests);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
