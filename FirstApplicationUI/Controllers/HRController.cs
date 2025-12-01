using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using FirstApplicationUI.Models;
using System.Net.Http.Headers;

namespace FirstApplicationUI.Controllers
{
    public class HRController : BaseController
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public HRController(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClient = httpClientFactory.CreateClient();
            _baseUrl = config["ApiSettings:BaseUrl"];
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }

        public async Task<IActionResult> Index()
        {
            // HR ka role = 2
            var redirect = CheckAccess(2);
            if (redirect != null) return redirect;

            var username = HttpContext.Session.GetString("Username");
            var token = HttpContext.Session.GetString("Token");

            if (!string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("Employee");

            List<EmployeeResponseDto> employees = new();
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                employees = JsonConvert.DeserializeObject<List<EmployeeResponseDto>>(json);
            }

            var vm = new HRDashboardViewModel
            {
                Username = username,
                Employees = employees
            };


            return View(vm);
        }
    }
}
