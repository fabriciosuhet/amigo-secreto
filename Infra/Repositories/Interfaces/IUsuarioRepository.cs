using Presentes.Entities;
using Presentes.Models.DTOs;
using Presentes.Models.Enums;

namespace Presentes.Infra.Repositories.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario> BuscarUsuarioPorIdAsync(Guid id);
    Task<IEnumerable<Usuario>> BuscarTodosUsuariosAsync();
    Task<Usuario?> BuscarUsuarioPorQueryAsync(string query);
    Task<Usuario?> BuscarUsuarioPorEmailAsync(string email);
    Task<List<Usuario?>> BuscarUsuarioPorRoleAsync(Role role);
    Task AdicionarUsuarioAsync(Usuario usuario);
    Task AtualizarUsuarioAsync(Guid id, Usuario usuario);
    Task DeletarUsuarioAsync(Guid id);
}