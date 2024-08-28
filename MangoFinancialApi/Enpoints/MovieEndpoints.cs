using AutoMapper;
using MangoDomain.EntititesTest;
using MangoFinancialApi.Dto;
using MangoFinancialApi.Repository;
using MangoFinancialApi.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;



namespace MangoFinancialApi.Enpoints;



public static class MovieEndpoints
{


    private static readonly string container = "movies";


    public static RouteGroupBuilder MapMovies(this RouteGroupBuilder endpoints)
    {

        endpoints.MapGet("/", GetAllMovies).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("movies-get"));
        endpoints.MapGet("/{id:int}",GetById);
        endpoints.MapGet("/getByTitle/{title}",GetByTitle);
        endpoints.MapPost("/", CreateMovie).DisableAntiforgery();    
        endpoints.MapPut("/{id:int}", UpdateMovie).DisableAntiforgery();
        endpoints.MapDelete("/{id:int}",DeleteMovie);
        endpoints.MapPost("/{id:int}/asignGenders",AsignGenders);
        endpoints.MapPost("/{id:int}/asignActors/",AsignActors);

        return endpoints;
    }


    static async Task<Ok<List<MovieDto>>> GetAllMovies(IRepositoryMovie repository, IMapper mapper, int page = 1, int recordsPerPage = 10)
    {

        var pagination = new PaginationDTO { Page = page, RecordsPerPage = recordsPerPage };

        var movies = await repository.GetAll(pagination);
        
        var moviesDtos = mapper.Map<List<MovieDto>>(movies);

        return TypedResults.Ok(moviesDtos);
    }


    static async Task<Results<Ok<MovieDto>, NotFound>> GetById(IRepositoryMovie repository, int id, IMapper mapper)
    {

        var movie = await repository.GetById(id);

        if(movie is null)
        {
            return TypedResults.NotFound();
        }

        var movieDto = mapper.Map<MovieDto>(movie);

        return TypedResults.Ok(movieDto);

    }


    static async Task<Results<Ok<List<MovieDto>>, NotFound>> GetByTitle(IRepositoryMovie repository, string name, IMapper mapper)
    {
        
        var movies = await repository.GetByTitle(name);

        if(!movies.Any())
        {
            return TypedResults.NotFound();
        }

        var moviesDto = mapper.Map<List<MovieDto>>(movies);

        return TypedResults.Ok(moviesDto);

    }


    static async Task<Created<MovieDto>> CreateMovie ([FromForm]MovieCreateDto movieCreateDto, IRepositoryMovie repository, IOutputCacheStore outputCacheStore, IMapper mapper, IStoreFiles storeFiles) 
    {

        var movie = mapper.Map<Movie>(movieCreateDto);

        if (movieCreateDto.Picture is not null)
        {
            var url = await storeFiles.SaveFileAsync(container,movieCreateDto.Picture);

            movie.PictureRoute = url;
        }

        var id = await repository.Create(movie);

        //Clear the cache
        await outputCacheStore.EvictByTagAsync("movies-get",default);

        //var genderDto = new GenderDto { Id = id, Name = gender.Name };

        var movieDto = mapper.Map<MovieDto>(movie);

        return TypedResults.Created($"/movies/{id}",movieDto);
    }


    static async Task<Results<NoContent,NotFound>> UpdateMovie(int id, 
                                                                [FromForm]MovieCreateDto movieCreateDto, 
                                                                IRepositoryMovie repository, 
                                                                IStoreFiles storeFiles,
                                                                IOutputCacheStore outputCacheStore, 
                                                                IMapper mapper)
    {

        var actorDB = await repository.GetById(id);

        if (actorDB is null)
        {
            return TypedResults.NotFound();
        }

        var movieToUpdate = mapper.Map<Movie>(movieCreateDto);
        movieToUpdate.Id = id;
        movieToUpdate.PictureRoute = actorDB.PictureRoute;

        if (movieCreateDto.Picture is not null)
        {
            var url = await storeFiles.Edit(movieToUpdate.PictureRoute, container, movieCreateDto.Picture);
            movieToUpdate.PictureRoute = url;
        }

        await repository.Update(movieToUpdate);

        //Clear the cache
        await outputCacheStore.EvictByTagAsync("movies-get",default);

        return TypedResults.NoContent();

    }


    static async Task<Results<NoContent, NotFound>>  DeleteMovie(int id, 
                                                                IRepositoryMovie repository, 
                                                                IOutputCacheStore outputCacheStore,
                                                                IStoreFiles storeFiles) 
    {
            var movieDB = await repository.GetById(id);

            if (movieDB is null)
            {
                return TypedResults.NotFound();
            }

            await repository.Delete(id);

            if(movieDB.PictureRoute is not null)
            {
                await storeFiles.DeleteFileAsync(movieDB.PictureRoute, container);
            }
            
            //Clear the cache
            await outputCacheStore.EvictByTagAsync("movies-get",default);

            return TypedResults.NoContent();
            
    }


    static async Task<Results<NoContent, NotFound, BadRequest<string>>> AsignGenders(int id, 
                                                                List<int> gendersId,
                                                                IRepositoryMovie repositoryMovie,
                                                                IRepositoryGender repositoryGender)
    {

        if(!await repositoryMovie.Exist(id))
        {
            return TypedResults.NotFound();
        }

        var genderExists = new List<int>();

        if(gendersId.Count != 0)
        {
            genderExists = await repositoryGender.Exists(gendersId);
        }

        if(genderExists.Count != gendersId.Count)
        {
            
            var gendersNoExists = gendersId.Except(genderExists);

            return TypedResults.BadRequest($"The Genders with Ids {string.Join(",",gendersNoExists)} no exists"); 

        }

        await repositoryMovie.AsignGender(id,gendersId);

        return TypedResults.NoContent();

    }


    static async Task<Results<NoContent, NotFound, BadRequest<string>>> AsignActors(int id,
                                                                                    List<AsignActorMovieDto> actorsDto, 
                                                                                    IRepositoryMovie repositoryMovie, 
                                                                                    IRepositoryActor repositoryActor,
                                                                                    IMapper mapper)        
    {

        if(! await repositoryMovie.Exist(id))
        {
            return TypedResults.NotFound();
        }

        var actorsExists = new List<int>();

        var actorsIds = actorsDto.Select(a => a.ActorId).ToList();

        if(actorsDto.Count != 0)
        {
            actorsExists = await repositoryActor.Exists(actorsIds);
        }   

        if(actorsExists.Count != actorsIds.Count)
        {

            var actorsNotExists = actorsIds.Except(actorsExists);

            return TypedResults.BadRequest($"Actors with Ids {string.Join(",",actorsNotExists)} not found");

        }

        var actors = mapper.Map<List<ActorMovie>>(actorsDto);

        await repositoryMovie.AsignActors(id, actors);

        return TypedResults.NoContent();

    }
    


}
