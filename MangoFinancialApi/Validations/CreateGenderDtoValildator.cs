using FluentValidation;
using MangoFinancialApi.Dto;
using MangoFinancialApi.Repository;



namespace MangoFinancialApi.Validations;



public class CreateGenderDtoValildator : AbstractValidator<CreateGenderDto>
{
    public CreateGenderDtoValildator(IRepositoryGender repositoryGender, IHttpContextAccessor httpContextAccessor)
    {

        var valueRuteId = httpContextAccessor.HttpContext?.Request.RouteValues["id"];
        var id = 0;

        if(valueRuteId is string valueString)
        {
            int.TryParse(valueString, out id);
        }

        RuleFor(x => x.Name).NotEmpty().WithMessage(Utility.FieldRequiredMessage)
                            .NotNull().WithMessage(Utility.FieldRequiredMessage)
                            .MaximumLength(50).WithMessage(Utility.MaxLenghtMessage)
                            .Must(Utility.FirstLetterUpperCase).WithMessage(Utility.FirstLetterUpperCaseMessage)
                            .MustAsync(async (name, _) =>
                            {
                                var exist = await repositoryGender.Exist(id, name);
                                return !exist;
                            }).WithMessage(m => $"The property {m.Name} already exists");
    }

    



}



