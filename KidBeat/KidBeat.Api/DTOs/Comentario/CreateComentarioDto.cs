using System.ComponentModel.DataAnnotations;

namespace KidBeat.Api.DTOs.Comentario;

public class CreateComentarioDto
{
    [Required]
    public int ParqueId { get; set; }

    [Required]
    [MaxLength(2000)]
    public string Texto { get; set; } = string.Empty;
}