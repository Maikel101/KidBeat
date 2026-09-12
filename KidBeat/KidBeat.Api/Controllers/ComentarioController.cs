using KidBeat.Api.DTOs.Comentario;
using KidBeat.Api.Services.Comentario;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using KidBeat.Api.Enums;

namespace KidBeat.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ComentarioController : ControllerBase
{
    private readonly IComentarioService _comentarioService;

    public ComentarioController(IComentarioService comentarioService)
    {
        _comentarioService = comentarioService;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ComentarioDto>>> GetAll()
    {
        var comentarios = await _comentarioService.GetAllAsync();

        return Ok(comentarios);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<ComentarioDto>> GetById(int id)
    {
        var comentario = await _comentarioService.GetByIdAsync(id);

        if (comentario is null)
        {
            return NotFound();
        }

        return Ok(comentario);
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ComentarioDto>> Create(
        CreateComentarioDto dto)
    {
        var usuarioIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(usuarioIdClaim, out var usuarioId))
        {
            return Unauthorized();
        }

        var comentario = await _comentarioService.CreateAsync(usuarioId, dto);

        return CreatedAtAction(nameof(GetById), new { id = comentario.Id }, comentario);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(
        int id,
        UpdateComentarioDto dto)
    {
        var usuarioIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(usuarioIdClaim, out var usuarioId))
        {
            return Unauthorized();
        }

        var resultado = await _comentarioService.UpdateAsync(id, usuarioId, dto);

        if (resultado == ResultadoOperacionComentario.NoEncontrado)
        {
            return NotFound();
        }

        if (resultado == ResultadoOperacionComentario.NoAutorizado)
        {
            return Forbid();
        }

        return NoContent();
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

        var resultado = await _comentarioService.DeleteAsync(
            id,
            usuarioId);

        if (resultado == ResultadoOperacionComentario.NoEncontrado)
        {
            return NotFound();
        }

        if (resultado == ResultadoOperacionComentario.NoAutorizado)
        {
            return Forbid();
        }

        return NoContent();
    }
}