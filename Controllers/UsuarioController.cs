using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentes.Models.DTOs;
using Presentes.Services;
using Presentes.Services.Interfaces;

namespace Presentes.Controllers;

[ApiController]
[Route("usuario")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly PasswordHasher _passwordHasher;


    public UsuarioController(IUsuarioService usuarioService, PasswordHasher passwordHasher)
    {
        _usuarioService = usuarioService;
        _passwordHasher = passwordHasher;
    }

    [HttpGet]
    public async Task<IActionResult> BuscarTodos(string? query)
    {
        if (!string.IsNullOrEmpty(query))
        {
            var usuarioEspecifico = await _usuarioService.BuscarUsuarioPorQueryAsync(query);
            if (usuarioEspecifico != null)
            {
                return Ok(usuarioEspecifico);
            }

            return NotFound($"Nenhum usuario encontrado para a consulta: {query}");
        }

        var usuarios = await _usuarioService.BuscarTodosUsuariosAsync();
        return Ok(usuarios);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPorId(Guid id)
    {
        var usuario = await _usuarioService.BuscarUsuarioPorIdAsync(id);
        return Ok(usuario);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody]LoginDTO loginDto)
    {
        try
        {
            var token = await _usuarioService.LoginAsync(loginDto.Email, loginDto.Password);
            return Ok(new { Token = token });
        }
        catch (Exception ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CadastrarUsuario([FromBody] CadastrarUsuarioDTO usuarioDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            await _usuarioService.AdicionarUsuarioAsync(usuarioDto);
        }
        catch (ValidationException ex)
        {
            return BadRequest($"Erro de validação: {ex.Message}");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
        return Ok("Usuário cadastrado com sucesso");
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AtualizarUsuario(Guid id, [FromBody] CadastrarUsuarioDTO atualizarUsuarioDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            await _usuarioService.AtualizarUsuarioAsync(id, atualizarUsuarioDto);
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro de validação: {ex.Message}");
        }

        return Ok("Usuário atualizado com sucesso");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ExcluirUsuario(Guid id)
    {
        await _usuarioService.DeletarUsuarioAsync(id);
        return Ok("Usuário excluido com sucesso");
    }
}