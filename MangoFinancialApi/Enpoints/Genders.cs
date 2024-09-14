using AutoMapper;
using FluentValidation;
using MangoDomain.EntititesTest;
using MangoFinancialApi.Dto;
using MangoFinancialApi.Filters;
using MangoFinancialApi.Repository;
using MangoFinancialApi.Validations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;



namespace MangoFinancialApi.Enpoints;



public static class GenderEnpoints
{


    public static RouteGroupBuilder MapGenders(this RouteGroupBuilder endpoints)
    {

        //From expression lamda to method
        endpoints.MapGet("/", GetAllGenders)
            .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("genders-get"))
            .RequireAuthorization();
            
        //endpoints.MapGet("/{id:int}",GetById);//.AddEndpointFilter<TestFilter>();

        endpoints.MapGet("/{id:int}",GetById2);

        //endpoints.MapPost("/", CreateGender).AddEndpointFilter<GenderFilterValidation>();    
        endpoints.MapPost("/", CreateGender).AddEndpointFilter<FilterValidation<CreateGenderDto>>();   //Using generic filter 
        //endpoints.MapPut("/{id:int}", UpdateGender).AddEndpointFilter<GenderFilterValidation>(); 
        
        endpoints.MapPut("/{id:int}", UpdateGender)
            .AddEndpointFilter<FilterValidation<CreateGenderDto>>()
            .WithOpenApi(opt =>
            {
                opt.Summary = "Update a Gender";
                opt.Description = "With this endpoint you can update the information of the Gender"; 
                opt.Parameters[0].Description = "The id of a Gender to update";
                opt.RequestBody.Description = "The information of Gender to update";
                return opt;
            });  //Using generic filter 
        
        
        endpoints.MapDelete("/{id:int}",DeleteGender);

        /*
        endpoints.MapGet("/{id:int}",GetById).AddEndpointFilter(async (context, next) => 
        {
            //This code execute before the endpoint
           var result = await next(context);

            //This code execute after the endpoint
           return result;
        });
        */

        return endpoints;
    }


    //From expression lamda to method
    static async Task<Ok<List<GenderDto>>> GetAllGenders(IRepositoryGender repository, IMapper mapper, ILoggerFactory loggerFactory)
    {
        //Enable the logger
        var type = typeof(GenderEnpoints);
        var logger = loggerFactory.CreateLogger(type.FullName!);
        logger.LogInformation("Getting the list of Genders");


        logger.LogInformation("Getting the list of");
        logger.LogTrace("This is a trace log");
        logger.LogDebug("This is a debug log");
        logger.LogWarning("This is a warning log");
        logger.LogError("This is a error log");
        logger.LogCritical("This is a critical log");



        var genders = await repository.GetAll();
        
        var gendersDto = mapper.Map<List<GenderDto>>(genders);

        return TypedResults.Ok(gendersDto);
    }

    static async Task<Results<Ok<GenderDto>, NotFound>> GetById(IRepositoryGender repository, 
                                                                int id, 
                                                                IMapper mapper)
    {
        var gender = await repository.GetById(id);

        if(gender is null)
        {
            return TypedResults.NotFound();
        }

        var genderDto = mapper.Map<GenderDto>(gender);

        return TypedResults.Ok(genderDto);
    }


    /// <summary>
    /// Method using the AsParameter
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="id"></param>
    /// <param name="mapper"></param>
    /// <returns></returns>
    static async Task<Results<Ok<GenderDto>, NotFound>> GetById2([AsParameters]GetGenderByIdDTO model)
    {
        var gender = await model.Repository.GetById(model.Id);

        if(gender is null)
        {
            return TypedResults.NotFound();
        }

        var genderDto = model.Mapper.Map<GenderDto>(gender);

        return TypedResults.Ok(genderDto);
    }


    static async Task<Results<Created<GenderDto>, ValidationProblem>> CreateGender (CreateGenderDto createGenderDto, 
                                                        IRepositoryGender repository, 
                                                        IOutputCacheStore outputCacheStore, 
                                                        IMapper mapper
                                                        //,IValidator<CreateGenderDto> validator
                                                        ) 
    {

        /*
        //Validate the data 
        var validationResult = await validator.ValidateAsync(createGenderDto);

        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary()); 
        }
        */

        var gender = mapper.Map<Gender>(createGenderDto);

        var id = await repository.Create(gender);

        //Clear the cache
        await outputCacheStore.EvictByTagAsync("genders-get",default);

        var genderDto = mapper.Map<GenderDto>(gender);

        return TypedResults.Created($"/genders/{id}",genderDto);
    }

    static async Task<Results<NoContent,NotFound, ValidationProblem>> UpdateGender(int id,
                                                                CreateGenderDto genderDto, 
                                                                IRepositoryGender repository, 
                                                                IOutputCacheStore outputCacheStore, 
                                                                IMapper mapper
                                                                //,IValidator<CreateGenderDto> validator
                                                                )
    {

        /*
        //Validate the data 
        var validationResult = await validator.ValidateAsync(genderDto);

        if (!validationResult.IsValid)
        {
            return TypedResults.ValidationProblem(validationResult.ToDictionary()); 
        }
        */


        var exist = await repository.Exist(id);

        if (!exist)
        {
            return TypedResults.NotFound();
        }

        var gender = mapper.Map<Gender>(genderDto);
        gender.Id = id;

        await repository.Update(gender);

            //Clear the cache
        await outputCacheStore.EvictByTagAsync("genders-get",default);

        return TypedResults.NoContent();

    }

    static async Task<Results<NoContent, NotFound>>  DeleteGender(int id, IRepositoryGender repository, IOutputCacheStore outputCacheStore) 
    {
            var exist = await repository.Exist(id);

            if (!exist)
            {
                return TypedResults.NotFound();
            }

            await repository.Delete(id);

            //Clear the cache
            await outputCacheStore.EvictByTagAsync("genders-get",default);

            return TypedResults.NoContent();
    }

}

