namespace KidBeat.Api.DTOs.Imagen;

public class ImagenDto
{
    public int Id { get; set; }
    public int ParqueId { get; set; }
    public int UsuarioId { get; set; }
    public string StorageUrl { get; set; } = string.Empty;
    public DateTime FechaSubida { get; set; }
    public bool EsPrincipal { get; set; }
}