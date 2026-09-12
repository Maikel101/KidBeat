using KidBeat.Api.Data;
using KidBeat.Api.DTOs.Usuario;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KidBeat.Api.Services.Usuario;

public class UsuarioService : IUsuarioService
{
    private readonly KidBeatDbContext _context;
    private readonly IPasswordHasher<KidBeat.Api.Models.Usuario> _passwordHasher;
    private readonly IConfiguration _configuration;

    public UsuarioService(
      KidBeatDbContext context,
      IPasswordHasher<KidBeat.Api.Models.Usuario> passwordHasher,
      IConfiguration configuration)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
    }

    public async Task<UsuarioDto?> GetByIdAsync(int id)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Id == id);

        return usuario is null ? null : MapToDto(usuario);
    }

    public async Task<UsuarioDto?> GetByEmailAsync(string email)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email);

        return usuario is null ? null : MapToDto(usuario);
    }

    public async Task<UsuarioDto> RegisterAsync(RegisterUsuarioDto dto)
    {
        var usuarioExistente = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (usuarioExistente is not null)
        {
            throw new InvalidOperationException(
                "Ya existe un usuario con ese email.");
        }

        var usuario = new KidBeat.Api.Models.Usuario
        {
            Nombre = dto.Nombre,
            Email = dto.Email,
            FechaRegistro = DateTime.UtcNow
        };

        usuario.PasswordHash = _passwordHasher.HashPassword(
            usuario,
            dto.Password);

        _context.Usuarios.Add(usuario);

        await _context.SaveChangesAsync();

        return MapToDto(usuario);
    }

    public async Task<string?> LoginAsync(LoginUsuarioDto dto)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (usuario is null)
        {
            return null;
        }

        var resultado = _passwordHasher.VerifyHashedPassword(
            usuario,
            usuario.PasswordHash,
            dto.Password);

        if (resultado == PasswordVerificationResult.Failed)
        {
            return null;
        }

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
        new Claim(ClaimTypes.Name, usuario.Nombre),
        new Claim(ClaimTypes.Email, usuario.Email)
    };

        var jwtKey = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException(
                "No se ha configurado la clave JWT.");

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static UsuarioDto MapToDto(KidBeat.Api.Models.Usuario usuario)
    {
        return new UsuarioDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Email = usuario.Email,
            FechaRegistro = usuario.FechaRegistro
        };
    }
}
