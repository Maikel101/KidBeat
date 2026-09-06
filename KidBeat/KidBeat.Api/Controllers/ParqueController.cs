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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ParqueDto>>> GetAll()
    {
        var parques = await _parqueService.GetAllAsync();

        return Ok(parques);
    }

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

    [HttpPost]
    public async Task<ActionResult<ParqueDto>> Create(CreateParqueDto dto)
    {
        var parque = await _parqueService.CreateAsync(dto);

        return CreatedAtAction(nameof(GetById),
            new { id = parque.Id },
            parque);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateParqueDto dto)
    {
        var updated = await _parqueService.UpdateAsync(id, dto);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

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