using FluentValidation;
using MangoFinancialApi.Dto;



namespace MangoFinancialApi.Validations;



public class CreateActorDtoValidator: AbstractValidator<CreateActorDto>
{

    public CreateActorDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage(Utility.FieldRequiredMessage)
                            .MaximumLength(150).WithMessage(Utility.MaxLenghtMessage);    



        var  minDate = new DateTime(1900, 1, 1);   

        RuleFor(x => x.BirthDate).GreaterThanOrEqualTo(minDate)
                                .WithMessage(Utility.GreaterThanOrEqualToMessage(minDate));    
    }

}
