using FluentValidation;
using MangoFinancialApi.Dto;



namespace MangoFinancialApi.Validations;



public class EditClaimDTOValidator: AbstractValidator<EditClaimDTO>
{
    public EditClaimDTOValidator()
    {

        RuleFor(x => x.Email).NotEmpty().WithMessage(Utility.FieldRequiredMessage)
        .MaximumLength(256).WithMessage(Utility.MaxLenghtMessage)
        .EmailAddress().WithMessage(Utility.EmailMessage);
        
    }


}

