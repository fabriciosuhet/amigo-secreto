using Microsoft.EntityFrameworkCore;
using Presentes.Entities;
using Presentes.Infra.Repositories.Interfaces;
using Presentes.Models.DTOs;
using Presentes.Models.Enums;

namespace Presentes.Infra.Repositories.Implementation;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly ApplicationDbContext _context;

    public UsuarioRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Usuario>> BuscarTodosUsuariosAsync()
    {
        return await _context.Usuarios.ToListAsync();
    }

    public async Task<Usuario?> BuscarUsuarioPorQueryAsync(string query)
    {
        return await _context.Usuarios
            .Where(u => u.Name.Contains(query))
            .FirstOrDefaultAsync();
    }

    public async Task<Usuario?> BuscarUsuarioPorEmailAsync(string email)
    {
        return await _context.Usuarios
            .Where(u => u.Email.Equals(email))
            .FirstOrDefaultAsync();
    }

    public async Task<List<Usuario>> BuscarUsuarioPorRoleAsync(Role role)
    {
        return await _context.Usuarios
            .Where(u => u.Role == role)
            .ToListAsync();
    }

    public async Task<Usuario> BuscarUsuarioPorIdAsync(Guid id)
    {
        return await _context.Usuarios.FindAsync(id);
    }
    
    public async Task AdicionarUsuarioAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();
    }
    
    public async Task AtualizarUsuarioAsync(Guid id, Usuario usuario)
    {
         _context.Usuarios.Update(usuario);
         await _context.SaveChangesAsync();
    }

    public async Task DeletarUsuarioAsync(Guid id)
    {
        var usuario  = await _context.Usuarios.FindAsync(id);
        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
    }
}