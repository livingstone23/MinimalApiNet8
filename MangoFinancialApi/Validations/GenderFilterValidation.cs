



using FluentValidation;
using MangoFinancialApi.Dto;

namespace MangoFinancialApi.Validations;



/// <summary>
/// Class that is use to Apply Filter in the Validator of Gender
/// using the interface IEndpointFilter
/// </summary>
public class GenderFilterValidation : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        
        //Get a service by Code
        var validador = context.HttpContext.RequestServices.GetService<IValidator<CreateGenderDto>>();

        //If Validator is null, continue with the next middleware
        if(validador is null)
        {
            return await next(context);
        } 

        var dataValidate = context.Arguments.OfType<CreateGenderDto>().FirstOrDefault();

        if(dataValidate is null)
        {
            return TypedResults.Problem("Can't found the data to validate");
        }

        var resultValidation = await validador.ValidateAsync(dataValidate);

        if(!resultValidation.IsValid)
        {
            return TypedResults.ValidationProblem(resultValidation.ToDictionary());
        }

        return await next(context);

    }
}
