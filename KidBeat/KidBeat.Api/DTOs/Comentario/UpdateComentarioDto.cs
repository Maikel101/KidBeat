using System.ComponentModel.DataAnnotations;

namespace KidBeat.Api.DTOs.Comentario;

public class UpdateComentarioDto
{
    [Required]
    [MaxLength(2000)]
    public string Texto { get; set; } = string.Empty;
}