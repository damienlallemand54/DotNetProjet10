using FrontendService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

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


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PatientCreateViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var client = CreateAuthenticatedClient();
            var content = new StringContent(
                JsonSerializer.Serialize(new
                {
                    firstName = model.FirstName,
                    lastName = model.LastName,
                    birthDate = model.BirthDate.ToString("yyyy-MM-dd"),
                    gender = model.Gender,
                    address = model.Address,
                    phoneNumber = model.PhoneNumber
                }),
                Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/patient", content);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Auth");

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
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

            var model = new PatientEditViewModel
            {
                Id = patient!.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                BirthDate = patient.BirthDate,
                Gender = patient.Gender,
                Address = patient.Address,
                PhoneNumber = patient.PhoneNumber
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, PatientEditViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var client = CreateAuthenticatedClient();
            var content = new StringContent(
                JsonSerializer.Serialize(new
                {
                    firstName = model.FirstName,
                    lastName = model.LastName,
                    birthDate = model.BirthDate.ToString("yyyy-MM-dd"),
                    gender = model.Gender,
                    address = model.Address,
                    phoneNumber = model.PhoneNumber
                }),
                Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/patient/{id}", content);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Auth");

            return RedirectToAction("Details", new { id });
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

        [HttpPost]
        public async Task<IActionResult> AddNote(NoteCreateViewModel model)
        {
            if (!ModelState.IsValid) return RedirectToAction("Details", new { id = model.PatientId });

            var client = CreateAuthenticatedClient();
            var content = new StringContent(
                JsonSerializer.Serialize(new
                {
                    patientId = model.PatientId,
                    patientName = model.PatientName,
                    content = model.Content
                }),
                Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/note", content);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return RedirectToAction("Login", "Auth");

            return RedirectToAction("Details", new { id = model.PatientId });
        }
    }
}