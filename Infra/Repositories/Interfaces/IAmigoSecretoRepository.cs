using Presentes.Entities;
using Presentes.Models.DTOs;

namespace Presentes.Infra.Repositories.Interfaces;

public interface IAmigoSecretoRepository
{
    Task<IEnumerable<AmigoSecreto>> BuscarTodosAmigosSecretosAsync();
    Task<List<AmigoSecreto>> ObterParesAtivoAsync();
    Task<AmigoSecreto> BuscarAmigoSecretoPorIdAsync(Guid id);
    Task<AmigoSecreto> BuscarUsuarioPorIdAsync(Guid id);
    Task AdicionarAmigoSecretoAsync(AmigoSecreto amigoSecreto);
    Task AtualizarAmigoSecretoAsync(Guid id, AmigoSecreto amigoSecreto);
    Task DeletarAmigoSecretoAsync(Guid id);
}