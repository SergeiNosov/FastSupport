using System.Collections.Generic;
using System.Diagnostics;
using FastSupportFixed.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySqlConnector;

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
            List<string> requests = new List<string>();

            using (MySqlConnection conn = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {

                conn.Open();

                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM UserRequests", conn);

                

                using (var reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {

                        if (reader["ShortDescription"] != null && reader["ShortDescription"] != "")
                        {
                            requests.Add(reader["ShortDescription"].ToString());
                        }
                        else
                        {
                            requests.Add(reader["Message"].ToString());

                        }

                    }

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
