using KidBeat.Api.DTOs.Valoracion;
using KidBeat.Api.Enums;

namespace KidBeat.Api.Services.Valoracion;

public interface IValoracionService
{
    Task<IEnumerable<ValoracionDto>> GetAllAsync();
    Task<ValoracionDto?> GetByIdAsync(int id);
    Task<(ResultadoOperacion Resultado, ValoracionDto? Valoracion)> CreateAsync(int usuarioId, CreateValoracionDto dto);
    Task<ResultadoOperacion> DeleteAsync(int id, int usuarioId);
}