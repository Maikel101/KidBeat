using KidBeat.Api.DTOs.Valoracion;
using KidBeat.Api.Services.Valoracion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using KidBeat.Api.Enums;

namespace KidBeat.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ValoracionController : ControllerBase
{
    private readonly IValoracionService _valoracionService;

    public ValoracionController(
        IValoracionService valoracionService)
    {
        _valoracionService = valoracionService;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ValoracionDto>>> GetAll()
    {
        var valoraciones = await _valoracionService.GetAllAsync();

        return Ok(valoraciones);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<ValoracionDto>> GetById(int id)
    {
        var valoracion = await _valoracionService.GetByIdAsync(id);

        if (valoracion is null)
        {
            return NotFound();
        }

        return Ok(valoracion);
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ValoracionDto>> Create(
    CreateValoracionDto dto)
    {
        var usuarioIdClaim = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(usuarioIdClaim, out var usuarioId))
        {
            return Unauthorized();
        }

        var resultado = await _valoracionService.CreateAsync(usuarioId, dto);

        if (resultado.Resultado == ResultadoOperacion.YaExiste)
        {
            return Conflict(
                "El usuario ya ha valorado este parque.");
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = resultado.Valoracion!.Id },
            resultado.Valoracion);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var usuarioIdClaim = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(usuarioIdClaim, out var usuarioId))
        {
            return Unauthorized();
        }

        var resultado = await _valoracionService.DeleteAsync(
            id,
            usuarioId);

        if (resultado == ResultadoOperacion.NoEncontrado)
        {
            return NotFound();
        }

        if (resultado == ResultadoOperacion.NoAutorizado)
        {
            return Forbid();
        }

        return NoContent();
    }
}