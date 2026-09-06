using KidBeat.Api.DTOs.Parque;

namespace KidBeat.Api.Services.Parque;

public interface IParqueService
{
    Task<IEnumerable<ParqueDto>> GetAllAsync();

    Task<ParqueDto?> GetByIdAsync(int id);

    Task<ParqueDto> CreateAsync(CreateParqueDto dto);

    Task<bool> UpdateAsync(int id, UpdateParqueDto dto);

    Task<bool> DeleteAsync(int id);
}
