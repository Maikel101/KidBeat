using KidBeat.Api.Data;
using KidBeat.Api.DTOs.Imagen;
using KidBeat.Api.Enums;
using Microsoft.EntityFrameworkCore;
using ImagenModel = KidBeat.Api.Models.Imagen;

namespace KidBeat.Api.Services.Imagen;

public class ImagenService : IImagenService
{
    private readonly KidBeatDbContext _context;

    public ImagenService(KidBeatDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ImagenDto>> GetAllAsync()
    {
        var imagenes = await _context.Imagenes
            .ToListAsync();

        return imagenes.Select(MapToDto);
    }

    public async Task<ImagenDto?> GetByIdAsync(int id)
    {
        var imagen = await _context.Imagenes
            .FirstOrDefaultAsync(i => i.Id == id);

        return imagen is null ? null : MapToDto(imagen);
    }

    public async Task<(ResultadoOperacion Resultado, ImagenDto? Imagen)> CreateAsync(
    int usuarioId,
    CreateImagenDto dto)
    {
        if (dto.EsPrincipal)
        {
            var yaExistePrincipal = await _context.Imagenes
                .AnyAsync(i =>
                    i.ParqueId == dto.ParqueId &&
                    i.EsPrincipal);

            if (yaExistePrincipal)
            {
                return (ResultadoOperacion.YaExiste, null);
            }
        }

        var imagen = new ImagenModel
        {
            ParqueId = dto.ParqueId,
            UsuarioId = usuarioId,
            StorageUrl = dto.StorageUrl,
            FechaSubida = DateTime.UtcNow,
            EsPrincipal = dto.EsPrincipal
        };

        _context.Imagenes.Add(imagen);

        await _context.SaveChangesAsync();

        return (ResultadoOperacion.Correcto, MapToDto(imagen));
    }

    public async Task<ResultadoOperacion> DeleteAsync(
      int id,
      int usuarioId)
    {
        var imagen = await _context.Imagenes
            .FirstOrDefaultAsync(i => i.Id == id);

        if (imagen is null)
        {
            return ResultadoOperacion.NoEncontrado;
        }

        if (imagen.UsuarioId != usuarioId)
        {
            return ResultadoOperacion.NoAutorizado;
        }

        _context.Imagenes.Remove(imagen);

        await _context.SaveChangesAsync();

        return ResultadoOperacion.Correcto;
    }

    private static ImagenDto MapToDto(ImagenModel imagen)
    {
        return new ImagenDto
        {
            Id = imagen.Id,
            ParqueId = imagen.ParqueId,
            UsuarioId = imagen.UsuarioId,
            StorageUrl = imagen.StorageUrl,
            FechaSubida = imagen.FechaSubida,
            EsPrincipal = imagen.EsPrincipal
        };
    }
}