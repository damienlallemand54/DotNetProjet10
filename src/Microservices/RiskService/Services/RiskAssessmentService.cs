using RiskService.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace RiskService.Services
{
    public class RiskAssessmentService : IRiskAssessmentService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private static readonly string[] Triggers =
        {
            "hémoglobine a1c", "microalbumine", "taille", "poids",
            "fumeur", "fumeuse", "anormal", "cholestérol",
            "vertige", "rechute", "réaction", "anticorps"
        };

        public RiskAssessmentService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<RiskLevel> AssessRiskAsync(int patientId, string token)
        {
            var gatewayClient = _httpClientFactory.CreateClient("GatewayClient");

            if (!string.IsNullOrEmpty(token))
            {
                gatewayClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var patientResponse = await gatewayClient.GetAsync($"/api/patient/{patientId}");
            if (!patientResponse.IsSuccessStatusCode) throw new Exception($"Patient {patientId} introuvable.");

            var patientJson = await patientResponse.Content.ReadAsStringAsync();
            var patient = JsonSerializer.Deserialize<PatientData>(patientJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;

            var notesResponse = await gatewayClient.GetAsync($"/api/note/patient/{patientId}");
            var notesJson = await notesResponse.Content.ReadAsStringAsync();
            var notes = JsonSerializer.Deserialize<List<NoteData>>(notesJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<NoteData>();

            // Compter les déclencheurs, ToLowerInvariant pour couvrir toutes les possibilités d'écriture, Contains pour couvrir singulier et pluriel, masculin et féminin
            var allNotes = string.Join(" ", notes.Select(n => n.Content)).ToLowerInvariant();
            var triggerCount = Triggers.Count(t => allNotes.Contains(t.ToLowerInvariant()));

            return CalculateRiskLevel(patient, triggerCount);
        }

        private static RiskLevel CalculateRiskLevel(PatientData patient, int triggerCount)
        {
            if (triggerCount == 0) return RiskLevel.None;

            bool isUnder30 = patient.Age < 30;
            bool isMale = patient.Gender.Equals("M", StringComparison.OrdinalIgnoreCase);

            // Borderline
            if (!isUnder30 && triggerCount >= 2 && triggerCount <= 5) return RiskLevel.Borderline;

            // In Danger
            if (isUnder30 && isMale && triggerCount >= 3 && triggerCount <= 4) return RiskLevel.InDanger;
            if (isUnder30 && !isMale && triggerCount >= 4 && triggerCount <= 6) return RiskLevel.InDanger;
            if (!isUnder30 && triggerCount >= 6 && triggerCount <= 7) return RiskLevel.InDanger;

            // Early Onset
            if (isUnder30 && isMale && triggerCount >= 5) return RiskLevel.EarlyOnset;
            if (isUnder30 && !isMale && triggerCount >= 7) return RiskLevel.EarlyOnset;
            if (!isUnder30 && triggerCount >= 8) return RiskLevel.EarlyOnset;

            return RiskLevel.None;
        }
    }
}