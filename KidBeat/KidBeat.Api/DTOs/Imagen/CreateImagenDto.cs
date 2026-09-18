using System.ComponentModel.DataAnnotations;

namespace KidBeat.Api.DTOs.Imagen;

public class CreateImagenDto
{
    [Required]
    public int ParqueId { get; set; }

    [Required]
    [MaxLength(1000)]
    public string StorageUrl { get; set; } = string.Empty;

    public bool EsPrincipal { get; set; }
}