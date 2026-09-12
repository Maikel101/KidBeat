using KidBeat.Api.DTOs.Usuario;
using KidBeat.Api.Services.Usuario;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace KidBeat.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UsuarioDto>> Register(
        RegisterUsuarioDto dto)
    {
        try
        {
            var usuario = await _usuarioService.RegisterAsync(dto);

            return CreatedAtAction(
                nameof(Register),
                new { id = usuario.Id },
                usuario);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(
        LoginUsuarioDto dto)
    {
        var token = await _usuarioService.LoginAsync(dto);

        if (token is null)
        {
            return Unauthorized();
        }

        return Ok(token);
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult GetCurrentUser()
    {
        var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        return Ok(new
        {
            UsuarioId = usuarioId
        });
    }
}
