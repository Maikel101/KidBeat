using KidBeat.Api.DTOs.Visita;

namespace KidBeat.Api.Services.Visita;

public interface IVisitaService
{
    Task<IEnumerable<VisitaDto>> GetAllAsync();

    Task<VisitaDto?> GetByIdAsync(int id);

    Task<VisitaDto> CreateAsync(int? usuarioId);
}