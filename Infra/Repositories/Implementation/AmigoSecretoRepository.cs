using Microsoft.EntityFrameworkCore;
using Presentes.Entities;
using Presentes.Infra.Repositories.Interfaces;
using Presentes.Models.DTOs;

namespace Presentes.Infra.Repositories.Implementation;

public class AmigoSecretoRepository : IAmigoSecretoRepository
{
    private readonly ApplicationDbContext _context;

    public AmigoSecretoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AmigoSecreto>> BuscarTodosAmigosSecretosAsync()
    {
       return await  _context.AmigosSecretos
            .Include(a => a.Usuario)
            .Include(a => a.UsuarioSorteado)
            .ToListAsync();
    }

    public async Task<List<AmigoSecreto>> ObterParesAtivoAsync()
    {
        return await _context.AmigosSecretos
            .Where(a => a.Ativo)
            .ToListAsync();
    }

    public async Task<AmigoSecreto> BuscarAmigoSecretoPorIdAsync(Guid id)
    {
        var amigoSecreto = await _context.AmigosSecretos.FindAsync(id);
        if (amigoSecreto == null)
        {
            throw new ArgumentNullException("O usuário não existe ou não foi encontrado.");
        }
        return amigoSecreto;
    }

    public async Task<AmigoSecreto> BuscarUsuarioPorIdAsync(Guid id)
    {
        return await _context.AmigosSecretos.FindAsync(id);
    }

    public async Task AdicionarAmigoSecretoAsync(AmigoSecreto amigoSecreto)
    {
        await _context.AmigosSecretos.AddAsync(amigoSecreto);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAmigoSecretoAsync(Guid id, AmigoSecreto amigoSecreto)
    {
        _context.AmigosSecretos.Update(amigoSecreto);
        await _context.SaveChangesAsync();
    }

    public async Task DeletarAmigoSecretoAsync(Guid id)
    {
        var amigoSecreto = await _context.AmigosSecretos.FindAsync(id);
        _context.AmigosSecretos.Remove(amigoSecreto);
        await _context.SaveChangesAsync();
    }
}