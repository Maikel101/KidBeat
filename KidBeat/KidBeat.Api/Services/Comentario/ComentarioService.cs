using KidBeat.Api.Data;
using KidBeat.Api.DTOs.Comentario;
using Microsoft.EntityFrameworkCore;
using KidBeat.Api.Enums;
using ComentarioModel = KidBeat.Api.Models.Comentario;

namespace KidBeat.Api.Services.Comentario;

public class ComentarioService : IComentarioService
{
    private readonly KidBeatDbContext _context;

    public ComentarioService(KidBeatDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ComentarioDto>> GetAllAsync()
    {
        var comentarios = await _context.Comentarios
            .ToListAsync();

        return comentarios.Select(MapToDto);
    }

    public async Task<ComentarioDto?> GetByIdAsync(int id)
    {
        var comentario = await _context.Comentarios
            .FirstOrDefaultAsync(c => c.Id == id);

        return comentario is null ? null : MapToDto(comentario);
    }

    public async Task<ComentarioDto> CreateAsync(
        int usuarioId,
        CreateComentarioDto dto)
    {
        var comentario = new ComentarioModel
        {
            ParqueId = dto.ParqueId,
            UsuarioId = usuarioId,
            Texto = dto.Texto,
            FechaPublicacion = DateTime.UtcNow
        };

        _context.Comentarios.Add(comentario);

        await _context.SaveChangesAsync();

        return MapToDto(comentario);
    }

    public async Task<ResultadoOperacionComentario> UpdateAsync(
        int id,
        int usuarioId,
        UpdateComentarioDto dto)
    {
        var comentario = await _context.Comentarios
            .FirstOrDefaultAsync(c => c.Id == id);

        if (comentario is null)
        {
            return ResultadoOperacionComentario.NoEncontrado;
        }

        if (comentario.UsuarioId != usuarioId)
        {
            return ResultadoOperacionComentario.NoAutorizado;
        }

        comentario.Texto = dto.Texto;
        comentario.FechaModificacion = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return ResultadoOperacionComentario.Correcto;
    }

    public async Task<ResultadoOperacionComentario> DeleteAsync(int id, int usuarioId)
    {
        var comentario = await _context.Comentarios
            .FirstOrDefaultAsync(c => c.Id == id);

        if (comentario is null)
        {
            return ResultadoOperacionComentario.NoEncontrado;
        }

        if (comentario.UsuarioId != usuarioId)
        {
            return ResultadoOperacionComentario.NoAutorizado;
        }

        _context.Comentarios.Remove(comentario);

        await _context.SaveChangesAsync();

        return ResultadoOperacionComentario.Correcto;
    }

    private static ComentarioDto MapToDto(ComentarioModel comentario)
    {
        return new ComentarioDto
        {
            Id = comentario.Id,
            ParqueId = comentario.ParqueId,
            UsuarioId = comentario.UsuarioId,
            Texto = comentario.Texto,
            FechaPublicacion = comentario.FechaPublicacion,
            FechaModificacion = comentario.FechaModificacion
        };
    }
}