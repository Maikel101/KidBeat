using KidBeat.Api.DTOs.Usuario;

namespace KidBeat.Api.Services.Usuario;

public interface IUsuarioService
{
    Task<UsuarioDto?> GetByIdAsync(int id);

    Task<UsuarioDto?> GetByEmailAsync(string email);

    Task<UsuarioDto> RegisterAsync(RegisterUsuarioDto dto);

    Task<string?> LoginAsync(LoginUsuarioDto dto);
}
