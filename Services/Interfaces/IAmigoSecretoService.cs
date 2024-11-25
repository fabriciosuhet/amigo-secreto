using Presentes.Entities;

namespace Presentes.Services.Interfaces;

public interface IAmigoSecretoService
{
    Task SortearAmigosSecrestos();
    Task<IEnumerable<AmigoSecreto>> BuscarTodosAmigosSecretosAsync();
    Task<AmigoSecreto> BuscarAmigoSecretoPorIdAsync(Guid id);
    Task<AmigoSecreto> BuscarUsuarioPorIdAsync(Guid id);
    Task AdicionarAmigoSecretoAsync(AmigoSecreto amigoSecreto);
    Task AtualizarAmigoSecretoAsync(Guid id, AmigoSecreto amigoSecreto);
    Task DeletarAmigoSecretoAsync(Guid id);
}