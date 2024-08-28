using FluentValidation;
using MangoFinancialApi.Dto;

namespace MangoFinancialApi.Validations;

public class MovieDTOCreateValidation: AbstractValidator<MovieCreateDto>
{

    public MovieDTOCreateValidation()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage(Utility.FieldRequiredMessage)
        .MaximumLength(150).WithMessage(Utility.MaxLenghtMessage);
    }

}
