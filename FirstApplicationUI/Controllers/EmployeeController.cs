using Microsoft.AspNetCore.Mvc;
using FirstApplicationUI.Models;

namespace FirstApplicationUI.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public EmployeeController(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClient = httpClientFactory.CreateClient();
            _baseUrl = config["ApiSettings:BaseUrl"];
        }

        public async Task<IActionResult> Index()
        {
            var url = _baseUrl + "Employee/GetAll";   // final URL

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "API Error: " + response.StatusCode;
                return View(new List<EmployeeResponseDto>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var employees = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EmployeeResponseDto>>(json);

            return View(employees);
        }
    }
}
