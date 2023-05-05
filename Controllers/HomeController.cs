using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RestApi.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Text.Json;

namespace RestApi.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    // public ActionResult Index()
    //     {
    //         return View();
    //     }

        List<TickerData> list = new List<TickerData>();
         public async Task<JsonResult> TickersData()
{
    using (var client = new HttpClient())
    {
        var response = await client.GetAsync("https://api.binance.com/api/v3/ticker/24hr");
        var content = await response.Content.ReadAsStringAsync();
        var tickerData = JsonConvert.DeserializeObject<List<TickerData>>(content);
        var firstElement = tickerData.First();
        return Json(firstElement.PriceChange);
       
    }
}

        private class TickerData
        {
            [JsonProperty("symbol")]
            public string? Symbol { get; set; }

            [JsonProperty("lastPrice")]
            public decimal LastPrice { get; set; }

            [JsonProperty("priceChange")]
            public decimal PriceChange { get; set; }

            [JsonProperty("highPrice")]
            public decimal HighPrice { get; set; }

            [JsonProperty("lowPrice")]
            public decimal LowPrice { get; set; }
        }

        // private class DataPoint
        // {
        //     [JsonProperty("x")]
        //     public DateTime X { get; set; }

        //     [JsonProperty("y")]
        //     public decimal Y { get; set; }
        // }
    // public ActionResult Index()
    //     {
    //         return View();
    //     }

    //     public async Task<JsonResult> TickerData()
    //     {
    //         using (var client = new HttpClient())
    //         {
    //             var response = await client.GetAsync("https://api.binance.com/api/v3/ticker/24hr");
    //             var content = await response.Content.ReadAsStringAsync();
    //             return Json(content);
    //         }
    //     }

    // public ActionResult Index()
    //     {
    //         return View();
    //     }

    //     // Action method that returns real-time data as JSON
    //     public async Task<IActionResult> TickerData()
    //     {
    //         using (var client = new HttpClient())
    //         {
    //             var response = await client.GetAsync("https://api.binance.com/api/v3/ticker/24hr");
    //             var content = await response.Content.ReadAsStringAsync();
    //             var options = new JsonSerializerOptions
    //             {
    //                 PropertyNameCaseInsensitive = true,
    //             };
    //             var data = System.Text.Json.JsonSerializer.Deserialize<dynamic>(content, options);
    //             return Json(data);
    //         }
    //     }
    public ActionResult Index(string symbol)
    {
        // Define the API endpoint and parameters
        string endpoint = "https://api.binance.com/api/v3/ticker/24hr";

        // Create a new HTTP client to make the API request
        HttpClient client = new HttpClient();

        // Create an empty list to store the retrieved data
        List<CryptoData> firstlist = new List<CryptoData>();

        // Initialize a variable to hold the query parameter string
        var queryparam = "";

        // Create a new HttpResponseMessage object to store the API response
        HttpResponseMessage response = new HttpResponseMessage();

        // Check if a symbol has been provided in the URL parameters
        if (symbol != null)
        {
            // If a symbol is provided, add it to the endpoint URL as a query parameter
            queryparam = endpoint + "?symbol=" + symbol;

            // Send the HTTP GET request to the API and store the response
            response = client.GetAsync(queryparam).Result;

            // Read the response content as a string and deserialize the JSON data into a CryptoData object
            string json = response.Content.ReadAsStringAsync().Result;
            var data = JsonConvert.DeserializeObject<CryptoData>(json);

            // Add the retrieved data to the list
            firstlist.Add(data);
        }
        else
        {
            // If no symbol is provided, send the HTTP GET request to the API without any query parameters
            response = client.GetAsync(endpoint).Result;

            // Read the response content as a string and deserialize the JSON data into a list of CryptoData objects
            string json = response.Content.ReadAsStringAsync().Result;
            var data1 = JsonConvert.DeserializeObject<List<CryptoData>>(json);

            // Assign the retrieved data to the list
            firstlist = data1;
        }
        // Pass the retrieved data to the view as a ViewBag variable so we can use it to show the data in the View 
        ViewBag.data = firstlist;

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
