namespace Presentes.Entities;

public class AmigoSecreto
{
    public Guid Id { get; init; }
    public Guid UsuarioId { get; private set; }
    public Guid UsuarioSorteadoId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool Ativo { get; private set; }

    public Usuario Usuario { get; private set; }
    public Usuario UsuarioSorteado { get; private set; }
    
    private AmigoSecreto(){}

    public AmigoSecreto(Guid id, Guid usuarioId, Guid usuarioSorteadoId, DateTime createdAt, Usuario usuario,
        Usuario usuarioSorteado)
    {
        Id = Guid.NewGuid();
        UsuarioId = usuarioId;
        UsuarioSorteadoId = usuarioSorteadoId;
        CreatedAt = DateTime.Now;
        Usuario = usuario;
        UsuarioSorteado = usuarioSorteado;
        Ativo = true;
    }

    public void Inativar()
    {
        Ativo = false;
    }

    public void atualizarUsuarioSorteadoId(Guid novoUsuarioSorteadoId)
    {
        UsuarioSorteadoId = novoUsuarioSorteadoId;
    }

    public void AtualizarCreatedAt(DateTime novoCreatedAt)
    {
        CreatedAt = novoCreatedAt;
    }
}