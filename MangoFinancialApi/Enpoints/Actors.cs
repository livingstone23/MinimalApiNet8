using AutoMapper;
using FluentValidation;
using MangoDomain.EntititesTest;
using MangoFinancialApi.Dto;
using MangoFinancialApi.Filters;
using MangoFinancialApi.Repository;
using MangoFinancialApi.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;



namespace MangoFinancialApi.Enpoints;



public static class ActorEnpoints
{

    private static readonly string container = "actors";

    public static RouteGroupBuilder MapActors(this RouteGroupBuilder endpoints)
    {

        endpoints.MapGet("/", GetAllActors).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("actors-get"));
        endpoints.MapGet("/{id:int}",GetById);
        endpoints.MapGet("/getByName/{name}",GetByName);
        endpoints.MapPost("/", CreateActor)
            .DisableAntiforgery()
            .AddEndpointFilter<FilterValidation<CreateActorDto>>()
            .RequireAuthorization("isadmin");       //Enable the security in endpoints #SEC2
        endpoints.MapPut("/{id:int}", UpdateActor)
            .DisableAntiforgery()
            .AddEndpointFilter<FilterValidation<CreateActorDto>>()
            .RequireAuthorization("isadmin");       //Enable the security in endpoints #SEC2
        endpoints.MapDelete("/{id:int}",DeleteActor)
            .RequireAuthorization("isadmin");       //Enable the security in endpoints #SEC2

        return endpoints;
    }


    static async Task<Ok<List<ActorDto>>> GetAllActors(IRepositoryActor repository, IMapper mapper, int page = 1, int recordsPerPage = 10)
    {

        var pagination = new PaginationDTO { Page = page, RecordsPerPage = recordsPerPage };

        var actors = await repository.GetAll(pagination);
        
        var actorsDtos = mapper.Map<List<ActorDto>>(actors);

        return TypedResults.Ok(actorsDtos);
    }

    static async Task<Results<Ok<ActorDto>, NotFound>> GetById(IRepositoryActor repository, int id, IMapper mapper)
    {

        var actor = await repository.GetById(id);

        if(actor is null)
        {
            return TypedResults.NotFound();
        }

        var actorDto = mapper.Map<ActorDto>(actor);

        return TypedResults.Ok(actorDto);

    }

    static async Task<Results<Ok<List<ActorDto>>, NotFound>> GetByName(IRepositoryActor repository, string name, IMapper mapper)
    {
        
        var actors = await repository.GetByName(name);

        if(!actors.Any())
        {
            return TypedResults.NotFound();
        }

        var actorsDto = mapper.Map<List<ActorDto>>(actors);

        return TypedResults.Ok(actorsDto);

    }


    static async Task<Results<Created<ActorDto>, ValidationProblem>> CreateActor ([FromForm]CreateActorDto createActorDto, 
                                                        IRepositoryActor repository, 
                                                        IOutputCacheStore outputCacheStore, 
                                                        IMapper mapper, 
                                                        IStoreFiles storeFiles
                                                        //,IValidator<CreateActorDto> validator
                                                        ) 
    {

        /*
        var resultValidation = await validator.ValidateAsync(createActorDto);

        if (!resultValidation.IsValid)
        {
            return TypedResults.ValidationProblem(resultValidation.ToDictionary());
        }
        */

        var actor = mapper.Map<Actor>(createActorDto);

        if (createActorDto.Picture is not null)
        {
            var url = await storeFiles.SaveFileAsync(container,createActorDto.Picture);
            actor.PictureRoute = url;
        }

        var id = await repository.Create(actor);

        //Clear the cache
        await outputCacheStore.EvictByTagAsync("actors-get",default);

        //var genderDto = new GenderDto { Id = id, Name = gender.Name };

        var actorDto = mapper.Map<ActorDto>(actor);

        return TypedResults.Created($"/actors/{id}",actorDto);
    }

    static async Task<Results<NoContent,NotFound>> UpdateActor(int id, 
                                                                [FromForm]CreateActorDto createActorDto, 
                                                                IRepositoryActor repository, 
                                                                IStoreFiles storeFiles,
                                                                IOutputCacheStore outputCacheStore, 
                                                                IMapper mapper)
    {

        var actorDB = await repository.GetById(id);

        if (actorDB is null)
        {
            return TypedResults.NotFound();
        }

        var actorToUpdate = mapper.Map<Actor>(createActorDto);
        actorToUpdate.Id = id;
        actorToUpdate.PictureRoute = actorDB.PictureRoute;

        if (createActorDto.Picture is not null)
        {
            var url = await storeFiles.Edit(actorToUpdate.PictureRoute, container, createActorDto.Picture);
            actorToUpdate.PictureRoute = url;
        }

        await repository.Update(actorToUpdate);

        //Clear the cache
        await outputCacheStore.EvictByTagAsync("actors-get",default);

        return TypedResults.NoContent();

    }

    static async Task<Results<NoContent, NotFound>>  DeleteActor(int id, 
                                                                IRepositoryActor repository, 
                                                                IOutputCacheStore outputCacheStore,
                                                                IStoreFiles storeFiles) 
    {
            var actorDB = await repository.GetById(id);

            if (actorDB is null)
            {
                return TypedResults.NotFound();
            }

            await repository.Delete(id);
            if(actorDB.PictureRoute is not null)
            {
                await storeFiles.DeleteFileAsync(actorDB.PictureRoute, container);
            }
            
            //Clear the cache
            await outputCacheStore.EvictByTagAsync("actors-get",default);

            return TypedResults.NoContent();

    }


}
