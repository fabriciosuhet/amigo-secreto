using FluentValidation;
using Presentes.Entities;

namespace Presentes.Validators;

public class AmigoSecretoValidator : AbstractValidator<AmigoSecreto>
{
    public AmigoSecretoValidator()
    {
        RuleFor(a => a.Id)
            .NotEmpty().WithMessage("O usuário não pode estar vázio.");
        
        RuleFor(a => a.UsuarioId)
            .NotEmpty().WithMessage("O ID do usuário é obrigatório");
        
        RuleFor(amigoSecreto => amigoSecreto)
            .Must(amigoSecreto => amigoSecreto.UsuarioId != amigoSecreto.UsuarioSorteadoId)
            .WithMessage("Um usuário não pode ser sorteado para si mesmo.");

        RuleFor(a => a.UsuarioSorteado)
            .NotNull()
            .WithMessage("O usuário sorteado deve ser definido");
    }
}