using FluentValidation;
using MangoFinancialApi.Dto;



namespace MangoFinancialApi.Validations;



public class CommentDTOCreateValidation: AbstractValidator<CommentCreateDto>
{

    public CommentDTOCreateValidation()
    {
        RuleFor(x => x.Content).NotEmpty().WithMessage(Utility.FieldRequiredMessage);
    }
    
}
