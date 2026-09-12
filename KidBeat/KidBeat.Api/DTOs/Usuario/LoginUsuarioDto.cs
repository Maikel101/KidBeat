using System.ComponentModel.DataAnnotations;

namespace KidBeat.Api.DTOs.Usuario;

public class LoginUsuarioDto
{
    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
