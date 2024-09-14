using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;



namespace MangoFinancialApi.Utility;


/// <summary>
/// Class for extending the Swagger functionality
/// </summary>
public static class SwaggerExtensions
{


    /// <summary>
    /// Method to add the pagination parameters to the OpenAPI documentation
    /// </summary>
    /// <param name="builder"></param>
    /// <typeparam name="TBuilder"></typeparam>
    /// <returns></returns>
    public static TBuilder AgregateParametersPaginationOpenAPI<TBuilder>(this TBuilder builder) where TBuilder : IEndpointConventionBuilder
    {
        
        return builder.WithOpenApi(opt => 
        {
            
            opt.Parameters.Add(new OpenApiParameter
            {
                Name = "page",
                In = ParameterLocation.Query,
                Required = false,
                Schema = new OpenApiSchema { Type = "integer", Default = new OpenApiInteger(1) }
            });

            opt.Parameters.Add(new OpenApiParameter
            {
                Name = "recordsPerPage",
                In = ParameterLocation.Query,
                Required = false,
                Schema = new OpenApiSchema { Type = "integer", Default = new OpenApiInteger(10) }
            });

            return opt;

        });
    }

    public static TBuilder AgregateParametersMovieFilterOpenAPI<TBuilder>(this TBuilder builder) where TBuilder : IEndpointConventionBuilder
    {
        
        return builder.WithOpenApi(opt => 
        {
            
            opt.Parameters.Add(new OpenApiParameter
            {
                Name = "page",
                In = ParameterLocation.Query,
                Required = false,
                Schema = new OpenApiSchema { Type = "integer", Default = new OpenApiInteger(1) }
            });

            opt.Parameters.Add(new OpenApiParameter
            {
                Name = "recordsPerPage",
                In = ParameterLocation.Query,
                Required = false,
                Schema = new OpenApiSchema { Type = "integer", Default = new OpenApiInteger(10) }
            });

            opt.Parameters.Add(new OpenApiParameter
            {
                Name = "title",
                In = ParameterLocation.Query,
                Required = false,
                Schema = new OpenApiSchema { Type = "string" }
            });

            opt.Parameters.Add(new OpenApiParameter
            {
                Name = "inTheaters",
                In = ParameterLocation.Query,
                Required = false,
                Schema = new OpenApiSchema { Type = "boolean" }
            });

            opt.Parameters.Add(new OpenApiParameter
            {
                Name = "upcomingReleases",
                In = ParameterLocation.Query,
                Required = false,
                Schema = new OpenApiSchema { Type = "boolean" }
            });

            opt.Parameters.Add(new OpenApiParameter
            {
                Name = "genderId",
                In = ParameterLocation.Query,
                Required = false,
                Schema = new OpenApiSchema { Type = "integer" }
            });

            opt.Parameters.Add(new OpenApiParameter
            {
                Name = "fieldOrder",
                In = ParameterLocation.Query,
                Required = false,
                Schema = new OpenApiSchema 
                    { Type = "string", 
                        Enum = new List<IOpenApiAny>
                            {
                                new OpenApiString("Title"),
                                new OpenApiString("ReleaseDate"),
                            } 
                    }
            });

            opt.Parameters.Add(new OpenApiParameter
            {
                Name = "orderAscending",
                In = ParameterLocation.Query,
                Required = false,
                Schema = new OpenApiSchema { Type = "boolean", Default = new OpenApiBoolean(true) }
            });

            return opt;

        });
    }

}
