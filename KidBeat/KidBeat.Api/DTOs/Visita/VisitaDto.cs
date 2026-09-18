namespace KidBeat.Api.DTOs.Visita;

public class VisitaDto
{
    public int Id { get; set; }
    public DateTime FechaHora { get; set; }
    public int? UsuarioId { get; set; }
}