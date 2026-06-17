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

            var patientResponse = await client.GetAsync($"/api/patient/{id}");
            if (patientResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Auth");
            if (patientResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                return NotFound();

            var patientJson = await patientResponse.Content.ReadAsStringAsync();
            var patient = JsonSerializer.Deserialize<PatientViewModel>(patientJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            var notesResponse = await client.GetAsync($"/api/note/patient/{id}");
            var notesJson = await notesResponse.Content.ReadAsStringAsync();
            var notes = JsonSerializer.Deserialize<List<NoteViewModel>>(notesJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<NoteViewModel>();

            var viewModel = new PatientDetailsViewModel
            {
                Patient = patient!,
                Notes = notes
            };

            return View(viewModel);
        }
    }
}