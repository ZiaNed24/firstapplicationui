using FirstApplicationUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

public class AccountController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public AccountController(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings)
    {
        _httpClient = httpClientFactory.CreateClient();
        _baseUrl = apiSettings.Value.BaseUrl;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginVm());
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVm model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var apiUrl = _baseUrl + "Auth/login";
            var response = await _httpClient.PostAsync(apiUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Invalid email or password");
                return View(model);
            }

            var resString = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<LoginResponse>(resString);

            HttpContext.Session.SetString("Token", data.Token);
            HttpContext.Session.SetString("Role", data.User.RoleId.ToString());
            HttpContext.Session.SetString("Username", data.User.Name);

            switch (data.User.RoleId)
            {
                case 1: return RedirectToAction("Index", "Admin");
                case 2: return RedirectToAction("Index", "HR");
                case 3: return RedirectToAction("Index", "Finance");
                case 4: return RedirectToAction("Index", "Employee");
                default: return RedirectToAction("Login");
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Login failed: " + ex.Message);
            return View(model);
        }
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
