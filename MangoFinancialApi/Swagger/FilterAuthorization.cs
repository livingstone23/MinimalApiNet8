using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;



namespace MangoFinancialApi.Swagger;



/// <summary>
/// This class is used to extend the OpenAPI documentation
/// this make if the endpoint has the Authorize attribute, it will add the security schema to the endpoint
/// if this is not added, the endpoint will be public 
/// </summary>
public class FilterAuthorization : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if(!context.ApiDescription.ActionDescriptor.EndpointMetadata.OfType<AuthorizeAttribute>().Any())
        {
            return;
        }

        operation.Security = new List<OpenApiSecurityRequirement> 
        {   
            new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            }
        };
    }
}
