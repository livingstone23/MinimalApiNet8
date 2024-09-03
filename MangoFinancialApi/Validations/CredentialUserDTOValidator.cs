using FluentValidation;
using MangoFinancialApi.Dto;

namespace MangoFinancialApi.Validations;



public class CredentialUserDTOValidator:AbstractValidator<CredentialUserDTO>
{
    public CredentialUserDTOValidator()
    {
        RuleFor(x => x.Email)
        .NotEmpty().WithMessage(Utility.FieldRequiredMessage)
        .MaximumLength(256).WithMessage(Utility.MaxLenghtMessage)
        .EmailAddress().WithMessage(Utility.EmailMessage);

        RuleFor(x => x.Password)
        .NotEmpty().WithMessage(Utility.FieldRequiredMessage)
        .MaximumLength(50).WithMessage(Utility.MaxLenghtMessage);

    }
}
