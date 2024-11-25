using FluentValidation;
using Presentes.Entities;

namespace Presentes.Validators;

public class UsuarioValidator : AbstractValidator<Usuario>
{
    public UsuarioValidator()
    {
        RuleFor(usuario => usuario.Id)
            .NotEmpty().WithMessage("O ID não pode ser vázio");
        
        RuleFor(usuario => usuario.Name)
            .NotNull()
            .WithMessage("O nome não pode ser nulo.")
            .NotEmpty().WithMessage("O nome não pode ser vazio.")
            .MaximumLength(250).WithMessage("O nome não pode ter mais de 250 caracteres.");

        RuleFor(usuario => usuario.Email)
            .NotNull().WithMessage("O e-mail não pode ser nulo.")
            .NotEmpty().WithMessage("O e-mail não pode ser vazio.")
            .EmailAddress().WithMessage("O e-mail deve ter um formato válido.")
            .MaximumLength(250).WithMessage("O e-mail não pode ter mais de 250 caracteres.");

        RuleFor(usuario => usuario.PassWordHash)
            .NotEmpty().WithMessage("A senha não pode ser vazia.")
            .NotNull().WithMessage("A senha não pode ser nula!")
            .MinimumLength(8).WithMessage("A senha deve ter pelo menos 8 digitos.");
    }
}