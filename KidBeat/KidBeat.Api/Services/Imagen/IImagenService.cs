using KidBeat.Api.DTOs.Imagen;
using KidBeat.Api.Enums;

namespace KidBeat.Api.Services.Imagen;

public interface IImagenService
{
    Task<IEnumerable<ImagenDto>> GetAllAsync();
    Task<ImagenDto?> GetByIdAsync(int id);
    Task<(ResultadoOperacion Resultado, ImagenDto? Imagen)> CreateAsync(int usuarioId, CreateImagenDto dto);
    Task<ResultadoOperacion> DeleteAsync(int id, int usuarioId);
}