using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Presentes.Entities;
using Presentes.Services.Interfaces;

namespace Presentes.Controllers;
[ApiController]
[Route("amigo-secreto")]
public class AmigoSecretoController : ControllerBase
{
    private readonly IAmigoSecretoService _amigoSecretoService;

    public AmigoSecretoController(IAmigoSecretoService amigoSecretoService)
    {
        _amigoSecretoService = amigoSecretoService;
    }

    [HttpGet]
    public async Task<IActionResult> BuscarAmigosSecretos()
    {
        var amigoSecretos = await _amigoSecretoService.BuscarTodosAmigosSecretosAsync();
        return Ok(amigoSecretos);
    }

    [HttpGet("/buscar-amigo-secreto/{id}")]
    public async Task<IActionResult> BuscarAmigoSecretoPorId(Guid id)
    {
        var amigoSecretoId = await _amigoSecretoService.BuscarAmigoSecretoPorIdAsync(id);
        return Ok(amigoSecretoId);
    }

    [HttpGet("/buscar-usuario/{id}")]
    public async Task<IActionResult> BuscarUsuarioPorId(Guid id)
    {
        var usuarioId = await _amigoSecretoService.BuscarUsuarioPorIdAsync(id);
        return Ok(usuarioId);
    }

    [HttpPut("/atualizar-amigo-secreto/{id}")]
    public async Task<IActionResult> AtualizarAmigoSecreto(Guid id, [FromBody] AmigoSecreto amigoSecreto)
    {
        if (amigoSecreto == null)
        {
            return BadRequest("Os dados fornecidos são inválidos");
        }
        try
        {
            await _amigoSecretoService.AtualizarAmigoSecretoAsync(id, amigoSecreto);
            return Ok("Amigo secreto atualizado com sucesso");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AdicionarAmigoSecreto([FromBody] AmigoSecreto amigoSecreto)
    {
        try
        {
            await _amigoSecretoService.AdicionarAmigoSecretoAsync(amigoSecreto);
            return CreatedAtAction(nameof(BuscarAmigoSecretoPorId), new { id = amigoSecreto.Id }, amigoSecreto);
        }
        catch (ValidationException ex)
        {
            return BadRequest($"Erro de validação: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }

    [HttpPost("/sortear-amigo-secreto")]
    public async Task<IActionResult> SortearAmigoSecreto()
    {
        try
        {
            await _amigoSecretoService.SortearAmigosSecrestos();
            return Ok("Sorteio realizado com sucesso");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest($"Erro: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro inesperado: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarAmigoSecreto(Guid id)
    {
        await _amigoSecretoService.DeletarAmigoSecretoAsync(id);
        return Ok("Amigo secreto deletado com sucesso");
    }
    
}