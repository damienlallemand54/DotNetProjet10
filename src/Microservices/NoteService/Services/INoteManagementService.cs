using NoteService.DTOs;

namespace NoteService.Services
{
    public interface INoteManagementService
    {
        Task<List<NoteReadDTO>> GetByPatientIdAsync(int patientId);
        Task<NoteReadDTO?> GetByIdAsync(string id);
        Task<NoteReadDTO> CreateNoteAsync(NoteCreateDTO createDto);
        Task<bool> UpdateNoteAsync(string id, NoteUpdateDTO updateDto);
        Task<bool> DeleteNoteAsync(string id);
    }
}