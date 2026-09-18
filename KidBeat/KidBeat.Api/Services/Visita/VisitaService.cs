using KidBeat.Api.Data;
using KidBeat.Api.DTOs.Visita;
using Microsoft.EntityFrameworkCore;
using VisitaModel = KidBeat.Api.Models.Visita;

namespace KidBeat.Api.Services.Visita;

public class VisitaService : IVisitaService
{
    private readonly KidBeatDbContext _context;

    public VisitaService(KidBeatDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<VisitaDto>> GetAllAsync()
    {
        var visitas = await _context.Visitas
            .ToListAsync();

        return visitas.Select(MapToDto);
    }

    public async Task<VisitaDto?> GetByIdAsync(int id)
    {
        var visita = await _context.Visitas
            .FirstOrDefaultAsync(v => v.Id == id);

        return visita is null
            ? null
            : MapToDto(visita);
    }

    public async Task<VisitaDto> CreateAsync(int? usuarioId)
    {
        var visita = new VisitaModel
        {
            UsuarioId = usuarioId,
            FechaHora = DateTime.UtcNow
        };

        _context.Visitas.Add(visita);

        await _context.SaveChangesAsync();

        return MapToDto(visita);
    }

    private static VisitaDto MapToDto(VisitaModel visita)
    {
        return new VisitaDto
        {
            Id = visita.Id,
            FechaHora = visita.FechaHora,
            UsuarioId = visita.UsuarioId
        };
    }
}