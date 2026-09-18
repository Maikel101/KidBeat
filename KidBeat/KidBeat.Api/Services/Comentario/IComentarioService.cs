using KidBeat.Api.DTOs.Comentario;
using KidBeat.Api.Enums;

namespace KidBeat.Api.Services.Comentario;

public interface IComentarioService
{
    Task<IEnumerable<ComentarioDto>> GetAllAsync();

    Task<ComentarioDto?> GetByIdAsync(int id);

    Task<ComentarioDto> CreateAsync(int usuarioId, CreateComentarioDto dto);

    Task<ResultadoOperacion> UpdateAsync(int id, int usuarioId, UpdateComentarioDto dto);

    Task<ResultadoOperacion> DeleteAsync(int id, int usuarioId);
}