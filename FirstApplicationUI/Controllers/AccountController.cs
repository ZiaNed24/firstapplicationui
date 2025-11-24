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

    // ================= LOGIN =================
    [HttpGet]
    public IActionResult Login() => View(new LoginVm());

    [HttpPost]
    public async Task<IActionResult> Login(LoginVm model)
    {
        if (!ModelState.IsValid) return View(model);

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

            return data.User.RoleId switch
            {
                1 => RedirectToAction("Index", "Admin"),
                2 => RedirectToAction("Index", "HR"),
                3 => RedirectToAction("Index", "Finance"),
                4 => RedirectToAction("Index", "Employee"),
                _ => RedirectToAction("Login")
            };
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

    // ================= FORGOT PASSWORD =================
    [HttpGet]
    public IActionResult ForgotPassword() => View(new ForgotPasswordVm());

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordVm model)
    {
        if (!ModelState.IsValid) return View(model);

        try
        {
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var apiUrl = _baseUrl + "Auth/forgot-password";
            var response = await _httpClient.PostAsync(apiUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Failed to send reset link");
                return View(model);
            }

            ViewBag.Message = "Reset link sent to your email (if account exists).";
            return View();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Error: " + ex.Message);
            return View(model);
        }
    }

    // ================= RESET PASSWORD =================
    [HttpGet]
    public IActionResult ResetPassword(string token, string email)
    {
        var model = new ResetPasswordVm { Token = token, Email = email };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordVm model)
    {
        if (!ModelState.IsValid) return View(model);

        try
        {
            var json = JsonConvert.SerializeObject(new
            {
                Email = model.Email,
                Token = model.Token,
                NewPassword = model.NewPassword
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var apiUrl = _baseUrl + "Auth/reset-password";
            var response = await _httpClient.PostAsync(apiUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                var err = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", "Reset failed: " + err);
                return View(model);
            }

            TempData["Message"] = "Password reset successful. You can now login.";
            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Error: " + ex.Message);
            return View(model);
        }
    }
}
