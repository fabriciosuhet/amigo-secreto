using FluentValidation;
using Presentes.Models.DTOs;

namespace Presentes.Validators;

public class CadastrarUsuarioDtoValidator : AbstractValidator<CadastrarUsuarioDTO>
{
    public CadastrarUsuarioDtoValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(250).WithMessage("O nome pode ter no máximo 250 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O email é obrigatório.")
            .EmailAddress().WithMessage("Formato de email inválido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(8).WithMessage("A senha deve ter pelo menos 6 caracteres.")
            .MaximumLength(20).WithMessage("A senha pode ter no máximo 20 caracteres.");
    }
}