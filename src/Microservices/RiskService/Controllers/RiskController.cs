using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RiskService.Services;

namespace RiskService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RiskController : ControllerBase
    {
        private readonly IRiskAssessmentService _riskService;

        public RiskController(IRiskAssessmentService riskService)
        {
            _riskService = riskService;
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetRisk(int patientId)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var risk = await _riskService.AssessRiskAsync(patientId, token);

            return Ok(new { riskLevel = risk.ToString() });
        }
    }
}