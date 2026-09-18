using KidBeat.Api.DTOs.Imagen;
using KidBeat.Api.Enums;
using KidBeat.Api.Services.Imagen;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KidBeat.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ImagenController : ControllerBase
{
    private readonly IImagenService _imagenService;

    public ImagenController(IImagenService imagenService)
    {
        _imagenService = imagenService;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ImagenDto>>> GetAll()
    {
        var imagenes = await _imagenService.GetAllAsync();

        return Ok(imagenes);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<ImagenDto>> GetById(int id)
    {
        var imagen = await _imagenService.GetByIdAsync(id);

        if (imagen is null)
        {
            return NotFound();
        }

        return Ok(imagen);
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ImagenDto>> Create(
     CreateImagenDto dto)
    {
        var usuarioIdClaim = User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(usuarioIdClaim, out var usuarioId))
        {
            return Unauthorized();
        }

        var resultado = await _imagenService.CreateAsync(
            usuarioId,
            dto);

        if (resultado.Resultado == ResultadoOperacion.YaExiste)
        {
            return Conflict("El parque ya tiene una imagen principal.");
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = resultado.Imagen!.Id },
            resultado.Imagen);
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

        var resultado = await _imagenService.DeleteAsync(
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