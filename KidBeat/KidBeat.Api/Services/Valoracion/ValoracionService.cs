using KidBeat.Api.Data;
using KidBeat.Api.DTOs.Valoracion;
using Microsoft.EntityFrameworkCore;
using ValoracionModel = KidBeat.Api.Models.Valoracion;
using KidBeat.Api.Enums;

namespace KidBeat.Api.Services.Valoracion;

public class ValoracionService : IValoracionService
{
    private readonly KidBeatDbContext _context;

    public ValoracionService(KidBeatDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ValoracionDto>> GetAllAsync()
    {
        var valoraciones = await _context.Valoraciones
            .ToListAsync();

        return valoraciones.Select(MapToDto);
    }

    public async Task<ValoracionDto?> GetByIdAsync(int id)
    {
        var valoracion = await _context.Valoraciones
            .FirstOrDefaultAsync(v => v.Id == id);

        return valoracion is null ? null : MapToDto(valoracion);
    }

    public async Task<(ResultadoOperacion Resultado, ValoracionDto? Valoracion)> CreateAsync(
    int usuarioId,
    CreateValoracionDto dto)
    {
        var yaExiste = await _context.Valoraciones
            .AnyAsync(v =>
                v.UsuarioId == usuarioId &&
                v.ParqueId == dto.ParqueId);

        if (yaExiste)
        {
            return (ResultadoOperacion.YaExiste, null);
        }

        var valoracion = new ValoracionModel
        {
            ParqueId = dto.ParqueId,
            UsuarioId = usuarioId,
            Puntuacion = dto.Puntuacion,
            FechaValoracion = DateTime.UtcNow
        };

        _context.Valoraciones.Add(valoracion);

        await _context.SaveChangesAsync();

        return (ResultadoOperacion.Correcto, MapToDto(valoracion));
    }

    public async Task<ResultadoOperacion> DeleteAsync(
        int id,
        int usuarioId)
    {
        var valoracion = await _context.Valoraciones
            .FirstOrDefaultAsync(v => v.Id == id);

        if (valoracion is null)
        {
            return ResultadoOperacion.NoEncontrado;
        }

        if (valoracion.UsuarioId != usuarioId)
        {
            return ResultadoOperacion.NoAutorizado;
        }

        _context.Valoraciones.Remove(valoracion);

        await _context.SaveChangesAsync();

        return ResultadoOperacion.Correcto;
    }

    private static ValoracionDto MapToDto(
        ValoracionModel valoracion)
    {
        return new ValoracionDto
        {
            Id = valoracion.Id,
            ParqueId = valoracion.ParqueId,
            UsuarioId = valoracion.UsuarioId,
            Puntuacion = valoracion.Puntuacion,
            FechaValoracion = valoracion.FechaValoracion
        };
    }
}