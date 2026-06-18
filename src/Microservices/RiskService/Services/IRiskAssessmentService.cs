using RiskService.Models;

namespace RiskService.Services
{
    public interface IRiskAssessmentService
    {
        Task<RiskLevel> AssessRiskAsync(int patientId, string token);
    }
}