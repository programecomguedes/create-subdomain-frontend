using CreateCustomSubdomain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CreateCustomSubdomain.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult CreateSubdomain(string subdomain)
        {
            try
            {
                if (!string.IsNullOrEmpty(subdomain))
                {
                    var client = new RestClient("{{URL_HERE}}");

                    var request = new RestRequest();
                    request.RequestFormat = DataFormat.Json;
                    request.AddJsonBody(new
                    {
                        subdomain = subdomain
                    });

                    var response = client.Post(request);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return View("Index");
                    }
                }
            }
            catch { }

            return View("Error");

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
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
