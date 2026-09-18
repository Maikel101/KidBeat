using KidBeat.Api.DTOs.Parque;
using KidBeat.Api.Services.Parque;
using Microsoft.AspNetCore.Mvc;

namespace KidBeat.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParqueController : ControllerBase
{
    private readonly IParqueService _parqueService;

    public ParqueController(IParqueService parqueService)
    {
        _parqueService = parqueService;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ParqueDto>>> GetAll()
    {
        var parques = await _parqueService.GetAllAsync();

        return Ok(parques);
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}")]
    public async Task<ActionResult<ParqueDto>> GetById(int id)
    {
        var parque = await _parqueService.GetByIdAsync(id);

        if (parque is null)
        {
            return NotFound();
        }

        return Ok(parque);
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [HttpPost]
    public async Task<ActionResult<ParqueDto>> Create(
        CreateParqueDto dto)
    {
        var parque = await _parqueService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = parque.Id },
            parque);
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateParqueDto dto)
    {
        var updated = await _parqueService.UpdateAsync(id, dto);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _parqueService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}