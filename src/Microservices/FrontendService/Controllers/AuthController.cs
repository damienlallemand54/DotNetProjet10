using FrontendService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace FrontendService.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var client = _httpClientFactory.CreateClient("GatewayClient");
            var content = new StringContent(
                JsonSerializer.Serialize(new { email = model.Email, password = model.Password }),
                Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/auth/login", content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Email ou mot de passe incorrect.");
                return View(model);
            }

            var json = await response.Content.ReadAsStringAsync();
            var tokenObj = JsonSerializer.Deserialize<JsonElement>(json);
            var token = tokenObj.GetProperty("token").GetString();

            HttpContext.Session.SetString("JwtToken", token!);

            return RedirectToAction("Index", "Patient");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}