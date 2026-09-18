using System.ComponentModel.DataAnnotations;

namespace KidBeat.Api.DTOs.Valoracion;

public class CreateValoracionDto
{
    [Required]
    public int ParqueId { get; set; }

    [Range(1, 5)]
    public int Puntuacion { get; set; }
}