using MangoDomain.EntititesTest;
using MangoFinancialApi.Data;



namespace MangoFinancialApi.Graphql;



public class Query
{


    [Serial]            // This is a custom attribute that I created to serialize the data, enable concurrency and optimistic concurrency
    [UsePaging]         // Enables paging
    [UseProjection]     // enables projection
    [UseFiltering]      // enables filtering
    [UseSorting]        // enables sorting
    public IQueryable<Gender> GetGenders([Service] ApplicationDbContext context) => context.Genders; 


    [Serial]            // This is a custom attribute that I created to serialize the data, enable concurrency and optimistic concurrency
    [UsePaging]         // Enables paging
    [UseProjection]     // enables projection
    [UseFiltering]      // enables filtering
    [UseSorting]        // enables sorting
    public IQueryable<Actor> GetActors([Service] ApplicationDbContext context) => context.Actors;


    [Serial]            // This is a custom attribute that I created to serialize the data, enable concurrency and optimistic concurrency
    [UsePaging]         // Enables paging
    [UseProjection]     // enables projection
    [UseFiltering]      // enables filtering
    [UseSorting]        // enables sorting
    public IQueryable<Movie> GetMovies([Service] ApplicationDbContext context) => context.Movies;

    
}
