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
        endpoints.MapGet("/{id:int}",GetById);//.AddEndpointFilter<TestFilter>();
        //endpoints.MapPost("/", CreateGender).AddEndpointFilter<GenderFilterValidation>();    
        endpoints.MapPost("/", CreateGender).AddEndpointFilter<FilterValidation<CreateGenderDto>>();   //Using generic filter 
        //endpoints.MapPut("/{id:int}", UpdateGender).AddEndpointFilter<GenderFilterValidation>(); 
        endpoints.MapPut("/{id:int}", UpdateGender).AddEndpointFilter<FilterValidation<CreateGenderDto>>();  //Using generic filter 
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
    static async Task<Ok<List<GenderDto>>> GetAllGenders(IRepositoryGender repository, IMapper mapper)
    {
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

