using Microsoft.AspNetCore.Mvc;
using PatientService.DTOs;
using PatientService.Services;

namespace PatientService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientManagementService _patientService;
        public PatientController(IPatientManagementService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientReadDTO>>> GetAll()
        {
            var patients = await _patientService.GetAllPatientsAsync();
            return Ok(patients); 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientReadDTO>> GetById(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            if (patient == null)
            {
                return NotFound($"Le patient avec l'ID {id} n'existe pas.");
            }
            return Ok(patient);
        }

        [HttpPost]
        public async Task<ActionResult<PatientReadDTO>> Create([FromBody] PatientCreateDTO createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdPatient = await _patientService.CreatePatientAsync(createDto);

            return CreatedAtAction(nameof(GetById), new { id = createdPatient.Id }, createdPatient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PatientUpdateDTO updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = await _patientService.UpdatePatientAsync(id, updateDto);
            if (!success)
            {
                return NotFound($"Impossible de mettre à jour : le patient avec l'ID {id} n'existe pas."); 
            }

            return NoContent();
        }
    }
}