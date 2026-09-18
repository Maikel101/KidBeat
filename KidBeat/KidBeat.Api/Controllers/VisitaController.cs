using KidBeat.Api.DTOs.Visita;
using KidBeat.Api.Services.Visita;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KidBeat.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VisitaController : ControllerBase
{
    private readonly IVisitaService _visitaService;

    public VisitaController(IVisitaService visitaService)
    {
        _visitaService = visitaService;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VisitaDto>>> GetAll()
    {
        var visitas = await _visitaService.GetAllAsync();

        return Ok(visitas);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<VisitaDto>> GetById(int id)
    {
        var visita = await _visitaService.GetByIdAsync(id);

        if (visita is null)
        {
            return NotFound();
        }

        return Ok(visita);
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [HttpPost]
    public async Task<ActionResult<VisitaDto>> Create()
    {
        int? usuarioId = null;

        var usuarioIdClaim = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (int.TryParse(usuarioIdClaim, out var id))
        {
            usuarioId = id;
        }

        var visita = await _visitaService.CreateAsync(
            usuarioId);

        return CreatedAtAction(
            nameof(GetById),
            new { id = visita.Id },
            visita);
    }
}