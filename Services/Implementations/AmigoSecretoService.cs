using FluentValidation;
using Presentes.Entities;
using Presentes.Infra;
using Presentes.Infra.Repositories.Interfaces;
using Presentes.Services.Interfaces;
using Presentes.Validators;

namespace Presentes.Services.Implementations;

public class AmigoSecretoService : IAmigoSecretoService
{
    private readonly IAmigoSecretoRepository _amigoSecretoRepository;
    private readonly AmigoSecretoValidator _amigoSecretoValidator;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ApplicationDbContext _context;

    public AmigoSecretoService(IAmigoSecretoRepository amigoSecretoRepository,
        AmigoSecretoValidator amigoSecretoValidator, IUsuarioRepository usuarioRepository, ApplicationDbContext context)
    {
        _amigoSecretoRepository = amigoSecretoRepository;
        _amigoSecretoValidator = amigoSecretoValidator;
        _usuarioRepository = usuarioRepository;
        _context = context;
    }

    public async Task SortearAmigosSecrestos()
    {
        var usuarios = await _usuarioRepository.BuscarTodosUsuariosAsync();
        if (usuarios.Count() < 2)
        {
            throw new InvalidOperationException("É necessário pelo menos dois participantes para o sorteio");
        }

        var todosOsPares = await _amigoSecretoRepository.ObterParesAtivoAsync();
        var usuariosJaSorteados = todosOsPares.Select(p => p.UsuarioId).ToHashSet();
        var usuariosJaSorteadores = todosOsPares.Select(p => p.UsuarioSorteadoId).ToHashSet();

        var participantesDisponiveis = usuarios
            .Where(u => !usuariosJaSorteados.Contains(u.Id) && !usuariosJaSorteadores.Contains(u.Id))
            .OrderBy(_ => Guid.NewGuid())
            .ToList();

        if (participantesDisponiveis.Count < 2)
        {
            throw new InvalidOperationException("Não há participantes suficientes disponíveis para um novo sorteio.");
        }
        
        var novosPares = new List<AmigoSecreto>();
        for (int i = 0; i < participantesDisponiveis.Count; i++)
        {
            var usuario = participantesDisponiveis[i];
            var sorteado = participantesDisponiveis[(i + 1) % participantesDisponiveis.Count];
            novosPares.Add(new AmigoSecreto(
                Guid.NewGuid(), 
                usuario.Id,
                sorteado.Id,
                DateTime.Now, 
                usuario,
                sorteado
                ));
        }

        var paresAtivos = todosOsPares.Where(p => p.Ativo).ToList();
        foreach (var par in paresAtivos)
        {
            par.Inativar();
            await _amigoSecretoRepository.AtualizarAmigoSecretoAsync(par.Id, par);
        }

        foreach (var novoPar in novosPares)
        {
            await _amigoSecretoRepository.AdicionarAmigoSecretoAsync(novoPar);
            
        }

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<AmigoSecreto>> BuscarTodosAmigosSecretosAsync()
    {
        var amigos = await _amigoSecretoRepository.BuscarTodosAmigosSecretosAsync();
        if (amigos == null)
        {
            throw new Exception("Amigos secretos não encontrados");
        }
        return amigos;
    }

    public async Task<AmigoSecreto> BuscarAmigoSecretoPorIdAsync(Guid id)
    {
        var amigoSecretoId = await _amigoSecretoRepository.BuscarAmigoSecretoPorIdAsync(id);
        if (amigoSecretoId == null)
        {
            throw new ArgumentNullException("O usuário amigo secreto não pode ser nulo");
        }
        return amigoSecretoId;
    }

    public async Task<AmigoSecreto> BuscarUsuarioPorIdAsync(Guid id)
    {
        var usuarioAmigoId = await _amigoSecretoRepository.BuscarUsuarioPorIdAsync(id);
        if (usuarioAmigoId == null)
        {
            throw new ArgumentNullException("O usuário não pode ser nulo");
        }

        return usuarioAmigoId;
    }

    public async Task AdicionarAmigoSecretoAsync(AmigoSecreto amigoSecreto)
    {
        var validationFriend = await _amigoSecretoValidator.ValidateAsync(amigoSecreto);
        if (!validationFriend.IsValid)
        {
            throw new ValidationException(validationFriend.Errors.ToString());
        }
        await _amigoSecretoRepository.AdicionarAmigoSecretoAsync(amigoSecreto);
    }

    public async Task AtualizarAmigoSecretoAsync(Guid id, AmigoSecreto amigoSecreto)
    {
        var amigoExistente = await _amigoSecretoRepository.BuscarAmigoSecretoPorIdAsync(id);
        if (amigoExistente == null)
        {
            throw new Exception("Amigo secreto não encontrado");
        }
        
        amigoExistente.atualizarUsuarioSorteadoId(amigoSecreto.UsuarioSorteadoId);
        amigoExistente.AtualizarCreatedAt(amigoExistente.CreatedAt);
        
        var validationFriend = await _amigoSecretoValidator.ValidateAsync(amigoSecreto);
        if (!validationFriend.IsValid)
        {
            throw new ValidationException(validationFriend.Errors.ToString());
        }
        await _amigoSecretoRepository.AtualizarAmigoSecretoAsync(id, amigoSecreto);
    }

    public async Task DeletarAmigoSecretoAsync(Guid id)
    {
        var usuario = await _amigoSecretoRepository.BuscarUsuarioPorIdAsync(id);
        if (usuario == null)
        {
            throw new NullReferenceException("Usuário não encontrado");
        }
        await _amigoSecretoRepository.DeletarAmigoSecretoAsync(usuario.Id);
    }
}