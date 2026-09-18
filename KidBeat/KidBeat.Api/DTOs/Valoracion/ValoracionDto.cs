namespace KidBeat.Api.DTOs.Valoracion;

public class ValoracionDto
{
    public int Id { get; set; }
    public int ParqueId { get; set; }
    public int UsuarioId { get; set; }
    public int Puntuacion { get; set; }
    public DateTime FechaValoracion { get; set; }
}