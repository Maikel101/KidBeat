namespace KidBeat.Api.DTOs.Comentario;

public class ComentarioDto
{
    public int Id { get; set; }

    public int ParqueId { get; set; }

    public int UsuarioId { get; set; }

    public string Texto { get; set; } = string.Empty;

    public DateTime FechaPublicacion { get; set; }

    public DateTime? FechaModificacion { get; set; }
}
