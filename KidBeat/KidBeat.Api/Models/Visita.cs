namespace KidBeat.Api.Models;

public class Visita
{
    public int Id { get; set; }

    public DateTime FechaHora { get; set; }

    public int? UsuarioId { get; set; }
}
