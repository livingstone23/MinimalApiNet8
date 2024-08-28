using AutoMapper;
using MangoDomain.EntititesTest;
using MangoFinancialApi.Dto;
using MangoFinancialApi.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace MangoFinancialApi.Enpoints;

public static class CommentEndpoints
{


    public static RouteGroupBuilder MapComments(this RouteGroupBuilder endpoints)
    {

        endpoints.MapGet("/", GetAllComments).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("comments-get").SetVaryByRouteValue(new string[] { "movieId" }));
        endpoints.MapGet("/{id:int}",GetCommentById);
        endpoints.MapPost("/", CreateComment).DisableAntiforgery();    
        endpoints.MapPut("/{id:int}", UpdateComment).DisableAntiforgery();
        endpoints.MapDelete("/{id:int}",DeleteComment);

        return endpoints;

    }


    static async Task<Results<Created<CommentDto>, NotFound>> CreateComment (int movieId, 
                                                            CommentCreateDto commentCreateDto, 
                                                            IRepositoryComment repositoryComment, 
                                                            IRepositoryMovie repositoryMovie, 
                                                            IOutputCacheStore outputCacheStore, 
                                                            IMapper mapper) 
    {
        if(! await repositoryMovie.Exist(movieId))
        {
            return TypedResults.NotFound();
        }


        var comment = mapper.Map<Comment>(commentCreateDto);
        comment.MovieId = movieId;

        var id = await repositoryComment.Create(comment);
       
        await outputCacheStore.EvictByTagAsync("comments-get",default);

        var commentDto = mapper.Map<CommentDto>(comment);

        return TypedResults.Created($"/comments/{id}", commentDto);

    }


    static async Task<Results<Ok<List<CommentDto>>,NotFound>> GetAllComments(int movieId,
                                                                          IRepositoryComment repositoryComment, 
                                                                          IRepositoryMovie repositoryMovie, 
                                                                          IMapper mapper)
    {

        if(! await repositoryMovie.Exist(movieId))
        {
            return TypedResults.NotFound();
        }

        var comments = await repositoryComment.GetAll(movieId);
        
        var commentsDto = mapper.Map<List<CommentDto>>(comments);

        return TypedResults.Ok(commentsDto);
    }


    static async Task<Results<Ok<CommentDto>,NotFound>> GetCommentById(int movieId,
                                                                          IRepositoryComment repositoryComment, 
                                                                          IRepositoryMovie repositoryMovie, 
                                                                          IMapper mapper)
    {

        var comment = await repositoryComment.GetById(movieId);

        if(comment is null)
        {
            return TypedResults.NotFound();
        }
        
        var commentsDto = mapper.Map<CommentDto>(comment);

        return TypedResults.Ok(commentsDto);
    }


    static async Task<Results<NoContent,NotFound>> UpdateComment(int movieId, int id,
                                                                CommentCreateDto commentCreateDto, 
                                                                IRepositoryComment repositoryComment, 
                                                                IRepositoryMovie repositoryMovie, 
                                                                IOutputCacheStore outputCacheStore, 
                                                                IMapper mapper)
    {

        if(! await repositoryMovie.Exist(movieId))
        {
            return TypedResults.NotFound();
        }

        if(! await repositoryComment.Exist(id))
        {
            return TypedResults.NotFound();
        }

        var comment = mapper.Map<Comment>(commentCreateDto);
        comment.Id = id;
        comment.MovieId = movieId;


        await repositoryComment.Update(comment);

        await outputCacheStore.EvictByTagAsync("comments-get",default);

        return TypedResults.NoContent();

    }

    static async Task<Results<NoContent, NotFound>>  DeleteComment(int movieId, int id, 
                                                                IRepositoryComment repositoryComment, 
                                                                IOutputCacheStore outputCacheStore) 
    {
        if(! await repositoryComment.Exist(id))
        {
            return TypedResults.NotFound();
        }

        await repositoryComment.Delete(id);

        await outputCacheStore.EvictByTagAsync("comments-get",default);

        return TypedResults.NoContent();
            
    }





}
