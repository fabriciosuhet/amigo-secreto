using Presentes.Entities;
using Presentes.Models.DTOs;
using Presentes.Models.Enums;

namespace Presentes.Services.Interfaces;

public interface IUsuarioService
{
    Task<Usuario> BuscarUsuarioPorIdAsync(Guid id);
    Task<IEnumerable<Usuario>> BuscarTodosUsuariosAsync();
    Task<Usuario?> BuscarUsuarioPorQueryAsync(string query);
    Task<string?> LoginAsync(string email, string password);
    Task<Usuario?> BuscarUsuarioPorEmailAsync(string email);
    Task AdicionarUsuarioAsync(CadastrarUsuarioDTO cadastrarUsuarioDto);
    Task AtualizarUsuarioAsync(Guid id, CadastrarUsuarioDTO atualizarUsuarioDto);
    Task DeletarUsuarioAsync(Guid id);
}