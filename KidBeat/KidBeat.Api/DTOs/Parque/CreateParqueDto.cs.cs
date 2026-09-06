using System.ComponentModel.DataAnnotations;

namespace KidBeat.Api.DTOs.Parque;

public class CreateParqueDto
{
    [Required]
    [MaxLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Descripcion { get; set; } = string.Empty;

    [Required]
    [MaxLength(250)]
    public string Direccion { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Ciudad { get; set; } = string.Empty;

    [Range(-90, 90)]
    public double? Latitud { get; set; }

    [Range(-180, 180)]
    public double? Longitud { get; set; }

    public bool TieneBanios { get; set; }

    public bool TieneZonaInfantil { get; set; }

    public bool EsAccesible { get; set; }

    public bool TieneZonasSombra { get; set; }
}