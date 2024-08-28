
using AutoMapper;
using MangoFinancialApi.Repository;

namespace MangoFinancialApi.Filters;



public class TestFilter : IEndpointFilter
{
    public  async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        //This code execute before the endpoint

            var paramRepositoryGender = context.Arguments.OfType<IRepositoryGender>().FirstOrDefault();
            //var param1 = (IRepositoryGender)context.Arguments[0]!;
            var paramInteger = context.Arguments.OfType<int>().FirstOrDefault();
            
            var paramMapper = context.Arguments.OfType<IMapper>().FirstOrDefault();
            //var param3 = (IMapper)context.Arguments[2]!;



           var result = await next(context);

            //This code execute after the endpoint
           return result;
    }

}
