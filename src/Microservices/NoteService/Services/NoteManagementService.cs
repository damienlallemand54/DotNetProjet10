using NoteService.DTOs;
using NoteService.Entities;
using NoteService.Repositories;

namespace NoteService.Services
{
    public class NoteManagementService : INoteManagementService
    {
        private readonly INoteRepository _repository;

        public NoteManagementService(INoteRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<NoteReadDTO>> GetByPatientIdAsync(int patientId)
        {
            var notes = await _repository.GetByPatientIdAsync(patientId);
            return notes.Select(MapToReadDTO).ToList();
        }

        public async Task<NoteReadDTO?> GetByIdAsync(string id)
        {
            var note = await _repository.GetByIdAsync(id);
            return note == null ? null : MapToReadDTO(note);
        }

        public async Task<NoteReadDTO> CreateNoteAsync(NoteCreateDTO createDto)
        {
            var note = new Note
            {
                PatientId = createDto.PatientId,
                PatientName = createDto.PatientName,
                Content = createDto.Content
            };
            var created = await _repository.CreateAsync(note);
            return MapToReadDTO(created);
        }

        public async Task<bool> UpdateNoteAsync(string id, NoteUpdateDTO updateDto)
        {
            var note = new Note
            {
                Id = id,
                PatientId = updateDto.PatientId,
                PatientName = updateDto.PatientName,
                Content = updateDto.Content
            };
            return await _repository.UpdateAsync(id, note);
        }

        public async Task<bool> DeleteNoteAsync(string id)
        {
            return await _repository.DeleteAsync(id);
        }

        private static NoteReadDTO MapToReadDTO(Note note) => new()
        {
            Id = note.Id!,
            PatientId = note.PatientId,
            PatientName = note.PatientName,
            Content = note.Content
        };
    }
}