using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchangePopularTags.Models;

namespace StackExchangePopularTags.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {

            return View();
        }

        public async Task<IActionResult> PopulateTagTable()
        {
            var tagsModel = new TagsListViewModel();

            for (var i = 0; i < 10; i++)
            {
                var handler = new HttpClientHandler();
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                using (var httpClient = new HttpClient(handler))
                {
                    var url = $"2.2/tags?page={i + 1}&pagesize=100&order=desc&sort=popular&site=stackoverflow";
                    var apiUrl = ("https://api.stackexchange.com/" + url);

                    //setup HttpClient
                    httpClient.BaseAddress = new Uri(apiUrl);
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //make request
                    var jsonString = await httpClient.GetStringAsync(apiUrl);
                    var tags = JsonConvert.DeserializeObject<TagsListViewModel>(jsonString);
                    tags.SetPopularity();
                    tagsModel.Tags.AddRange(tags.Tags);
                }
            }

            var data = new
            {
                data = tagsModel.Tags
            };

            var jsonDt = JsonConvert.SerializeObject(data);

            return Ok(jsonDt);
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
