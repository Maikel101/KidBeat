namespace KidBeat.Api.Models;

public class Imagen
{
    public int Id { get; set; }

    public int ParqueId { get; set; }

    public int UsuarioId { get; set; }

    public string StorageUrl { get; set; } = string.Empty;

    public DateTime FechaSubida { get; set; }

    public bool EsPrincipal { get; set; }
}
