using FrontendService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace FrontendService.Controllers
{
    public class PatientController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PatientController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient CreateAuthenticatedClient()
        {
            var client = _httpClientFactory.CreateClient("GatewayClient");
            var token = HttpContext.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        public async Task<IActionResult> Index()
        {
            var client = CreateAuthenticatedClient();
            var response = await client.GetAsync("/api/patient");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Auth");

            var json = await response.Content.ReadAsStringAsync();
            var patients = JsonSerializer.Deserialize<List<PatientViewModel>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(patients);
        }

        public async Task<IActionResult> Details(int id)
        {
            var client = CreateAuthenticatedClient();
            var response = await client.GetAsync($"/api/patient/{id}");

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Auth");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return NotFound();

            var json = await response.Content.ReadAsStringAsync();
            var patient = JsonSerializer.Deserialize<PatientViewModel>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(patient);
        }
    }
}