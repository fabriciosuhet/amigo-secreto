using Microsoft.AspNetCore.Http.HttpResults;
using Presentes.Entities;
using Presentes.Infra.Repositories.Interfaces;
using Presentes.Models.DTOs;
using Presentes.Services.Interfaces;
using Presentes.Validators;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace Presentes.Services.Implementations;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly UsuarioValidator _usuarioValidator;
    private readonly PasswordHasher _passwordHasher;
    private readonly IAuthService _authService;

    public UsuarioService(IUsuarioRepository usuarioRepository, UsuarioValidator usuarioValidator,
        PasswordHasher passwordHasher, IAuthService authService)
    {
        _usuarioRepository = usuarioRepository;
        _usuarioValidator = usuarioValidator;
        _passwordHasher = passwordHasher;
        _authService = authService;
    }

    public async Task<Usuario> BuscarUsuarioPorIdAsync(Guid id)
    {
        return await _usuarioRepository.BuscarUsuarioPorIdAsync(id);
    }

    public async Task<IEnumerable<Usuario>> BuscarTodosUsuariosAsync()
    {
        var usuarios = await _usuarioRepository.BuscarTodosUsuariosAsync();
        if (usuarios == null || !usuarios.Any())
        {
            throw new Exception("Usuários não encontrados");
        }
        return usuarios;
    }

    public async Task<Usuario?> BuscarUsuarioPorQueryAsync(string query)
    {
        return await _usuarioRepository.BuscarUsuarioPorQueryAsync(query);
    }

    public async Task<Usuario?> BuscarUsuarioPorEmailAsync(string email)
    {
        var usuarioEmail = await _usuarioRepository.BuscarUsuarioPorEmailAsync(email);
        if (usuarioEmail == null)
        {
            throw new Exception("Email incorreto ou inexistente");
        }
        return usuarioEmail;
    }
    
    public async Task<string?> LoginAsync(string email, string password)
    {
        var user = await _usuarioRepository.BuscarUsuarioPorEmailAsync(email);
        if (user == null || !_passwordHasher.VerifyPassword(password, user.PassWordHash, user.PasswordSalt))
        {
            return null;
        }
        var token = _authService.GenerateJwtToken(user.Email, user.Role.ToString());
        return token;
    }

    public async Task AdicionarUsuarioAsync(CadastrarUsuarioDTO dto)
    {
        var (hash, salt) = _passwordHasher.HashPassword(dto.Password);
        
        var usuario = new Usuario(dto.Nome, dto.Email, hash, salt, dto.Role);
        
        var validationResult = await _usuarioValidator.ValidateAsync(usuario);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors.ToString());
        }
        
        await _usuarioRepository.AdicionarUsuarioAsync(usuario);
    }
    

    public async Task AtualizarUsuarioAsync(Guid id, CadastrarUsuarioDTO atualizarUsuarioDto)
    {
        var usuario = await _usuarioRepository.BuscarUsuarioPorIdAsync(id);
        if (usuario == null)
        {
            throw new Exception("Usuário não encontrado");
        }
        
        usuario.AlterarNome(atualizarUsuarioDto.Nome);
        usuario.AlterarEmail(atualizarUsuarioDto.Email);

        if (!string.IsNullOrEmpty(atualizarUsuarioDto.Password))
        {
            var (hash, salt) = _passwordHasher.HashPassword(atualizarUsuarioDto.Password);
            usuario.AlterarPassWord(hash, salt);
        }
        
        var validationResult = await _usuarioValidator.ValidateAsync(usuario);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors.ToString());
        }
        
        await _usuarioRepository.AtualizarUsuarioAsync(id, usuario);
    }

    public async Task DeletarUsuarioAsync(Guid id)
    {
        var usuario = await _usuarioRepository.BuscarUsuarioPorIdAsync(id);
        if (usuario == null)
        {
            throw new Exception("Usuário não encontrado");
        }

        await _usuarioRepository.DeletarUsuarioAsync(id);
    }
}