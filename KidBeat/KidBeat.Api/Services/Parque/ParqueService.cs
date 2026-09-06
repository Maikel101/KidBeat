using KidBeat.Api.Data;
using KidBeat.Api.DTOs.Parque;
using KidBeat.Api.Enums;
using Microsoft.EntityFrameworkCore;
using ParqueModel = KidBeat.Api.Models.Parque;

namespace KidBeat.Api.Services.Parque;

public class ParqueService : IParqueService
{
    private readonly KidBeatDbContext _context;

    public ParqueService(KidBeatDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ParqueDto>> GetAllAsync()
    {
        var parques = await _context.Parques
            .ToListAsync();

        return parques.Select(MapToDto);
    }

    public async Task<ParqueDto?> GetByIdAsync(int id)
    {
        var parque = await _context.Parques
            .FirstOrDefaultAsync(p => p.Id == id);

        return parque is null ? null : MapToDto(parque);
    }

    public async Task<ParqueDto> CreateAsync(CreateParqueDto dto)
    {
        var parque = new ParqueModel
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            Direccion = dto.Direccion,
            Ciudad = dto.Ciudad,
            Latitud = dto.Latitud,
            Longitud = dto.Longitud,
            TieneBanios = dto.TieneBanios,
            TieneZonaInfantil = dto.TieneZonaInfantil,
            EsAccesible = dto.EsAccesible,
            TieneZonasSombra = dto.TieneZonasSombra,
            EstadoParque = EstadoParque.Pendiente,
            FechaAlta = DateTime.UtcNow
        };

        _context.Parques.Add(parque);

        await _context.SaveChangesAsync();

        return MapToDto(parque);
    }

    public async Task<bool> UpdateAsync(int id, UpdateParqueDto dto)
    {
        var parque = await _context.Parques
            .FirstOrDefaultAsync(p => p.Id == id);

        if (parque is null)
        {
            return false;
        }

        parque.Nombre = dto.Nombre;
        parque.Descripcion = dto.Descripcion;
        parque.Direccion = dto.Direccion;
        parque.Ciudad = dto.Ciudad;
        parque.Latitud = dto.Latitud;
        parque.Longitud = dto.Longitud;
        parque.TieneBanios = dto.TieneBanios;
        parque.TieneZonaInfantil = dto.TieneZonaInfantil;
        parque.EsAccesible = dto.EsAccesible;
        parque.TieneZonasSombra = dto.TieneZonasSombra;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var parque = await _context.Parques
            .FirstOrDefaultAsync(p => p.Id == id);

        if (parque is null)
        {
            return false;
        }

        _context.Parques.Remove(parque);

        await _context.SaveChangesAsync();

        return true;
    }

    private static ParqueDto MapToDto(ParqueModel parque)
    {
        return new ParqueDto
        {
            Id = parque.Id,
            Nombre = parque.Nombre,
            Descripcion = parque.Descripcion,
            Direccion = parque.Direccion,
            Ciudad = parque.Ciudad,
            Latitud = parque.Latitud,
            Longitud = parque.Longitud,
            TieneBanios = parque.TieneBanios,
            TieneZonaInfantil = parque.TieneZonaInfantil,
            EsAccesible = parque.EsAccesible,
            TieneZonasSombra = parque.TieneZonasSombra,
            EstadoParque = parque.EstadoParque,
            FechaAlta = parque.FechaAlta
        };
    }
}