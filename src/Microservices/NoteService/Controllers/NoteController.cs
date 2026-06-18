using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteService.DTOs;
using NoteService.Services;

namespace NoteService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : ControllerBase
    {
        private readonly INoteManagementService _noteService;

        public NoteController(INoteManagementService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<List<NoteReadDTO>>> GetByPatientId(int patientId)
        {
            var notes = await _noteService.GetByPatientIdAsync(patientId);
            return Ok(notes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NoteReadDTO>> GetById(string id)
        {
            var note = await _noteService.GetByIdAsync(id);
            if (note == null) return NotFound($"La note avec l'ID {id} n'existe pas.");
            return Ok(note);
        }

        [HttpPost]
        public async Task<ActionResult<NoteReadDTO>> Create([FromBody] NoteCreateDTO createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _noteService.CreateNoteAsync(createDto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] NoteUpdateDTO updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var success = await _noteService.UpdateNoteAsync(id, updateDto);
            if (!success) return NotFound($"La note avec l'ID {id} n'existe pas.");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _noteService.DeleteNoteAsync(id);
            if (!success) return NotFound($"La note avec l'ID {id} n'existe pas.");
            return NoContent();
        }
    }
}