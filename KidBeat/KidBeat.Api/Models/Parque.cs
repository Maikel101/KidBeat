using KidBeat.Api.Enums;
namespace KidBeat.Api.Models;

public class Parque
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Descripcion { get; set; } = string.Empty;

    public string Direccion { get; set; } = string.Empty;

    public string Ciudad { get; set; } = string.Empty;

    public double? Latitud { get; set; }

    public double? Longitud { get; set; }

    public bool TieneBanios { get; set; }

    public bool TieneZonaInfantil { get; set; }

    public bool EsAccesible { get; set; }

    public bool TieneZonasSombra { get; set; }

    public EstadoParque EstadoParque { get; set; }

    public int? UsuarioCreadorId { get; set; }

    public DateTime FechaAlta { get; set; }
}
