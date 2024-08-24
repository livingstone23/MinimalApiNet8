using AutoMapper;
using MangoDomain.EntititesTest;
using MangoFinancialApi.Dto;
using MangoFinancialApi.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;



namespace MangoFinancialApi.Enpoints;



public static class GenderEnpoints
{


    public static RouteGroupBuilder MapGenders(this RouteGroupBuilder endpoints)
    {

        //From expression lamda to method
        endpoints.MapGet("/", GetAllGenders).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("genders-get"));
        endpoints.MapGet("/{id:int}",GetById);
        endpoints.MapPost("/", CreateGender);    
        endpoints.MapPut("/{id:int}", UpdateGender);
        endpoints.MapDelete("/{id:int}",DeleteGender);

        return endpoints;
    }


    //From expression lamda to method
    static async Task<Ok<List<GenderDto>>> GetAllGenders(IRepositoryGender repository, IMapper mapper)
    {
        var genders = await repository.GetAll();
        
        //var gendersDto = genders.Select(g => new GenderDto { Id = g.Id, Name = g.Name }).ToList();
        var gendersDto = mapper.Map<List<GenderDto>>(genders);

        return TypedResults.Ok(gendersDto);
    }

    static async Task<Results<Ok<GenderDto>, NotFound>> GetById(IRepositoryGender repository, int id, IMapper mapper)
    {
        var gender = await repository.GetById(id);

        if(gender is null)
        {
            return TypedResults.NotFound();
        }

        var genderDto = mapper.Map<GenderDto>(gender);

        //var genderDto = new GenderDto { Id = gender.Id, Name  = gender.Name };

        return TypedResults.Ok(genderDto);
    }


    static async Task<Created<GenderDto>> CreateGender (CreateGenderDto createGenderDto, IRepositoryGender repository, IOutputCacheStore outputCacheStore, IMapper mapper) 
    {

        //var gender = new Gender { Name = createGenderDto.Name };

        var gender = mapper.Map<Gender>(createGenderDto);

        var id = await repository.Create(gender);

        //Clear the cache
        await outputCacheStore.EvictByTagAsync("genders-get",default);

        //var genderDto = new GenderDto { Id = id, Name = gender.Name };

        var genderDto = mapper.Map<GenderDto>(gender);

        return TypedResults.Created($"/genders/{id}",genderDto);
    }

    static async Task<Results<NoContent,NotFound>> UpdateGender(int id,CreateGenderDto genderDto, IRepositoryGender repository, IOutputCacheStore outputCacheStore, IMapper mapper)
    {
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

