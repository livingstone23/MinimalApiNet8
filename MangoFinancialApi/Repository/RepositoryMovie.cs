using AutoMapper;
using MangoDomain.EntititesTest;
using MangoFinancialApi.Data;
using MangoFinancialApi.Dto;
using MangoFinancialApi.Utility;
using Microsoft.EntityFrameworkCore;



namespace MangoFinancialApi.Repository;



public class RepositoryMovie : IRepositoryMovie
{

    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly HttpContext httpContext;


    public RepositoryMovie(ApplicationDbContext context, 
                            IMapper mapper, 
                            IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _mapper = mapper;
        httpContext = httpContextAccessor.HttpContext!;
    }

    public async Task<List<Movie>> GetAll(PaginationDTO pagination)
    {

        var queryable = _context.Movies.AsQueryable();
        await httpContext.InsertPaginationParametersInResponse(queryable);
        var result = await queryable.OrderBy(a => a.Title).Paginate(pagination).ToListAsync();
        return result;

    }

    public async Task<Movie?> GetById(int id)
    {

        return await _context.Movies
                                .Include(c=>c.Comments)
                                .Include(a=>a.GenderMovies)
                                    .ThenInclude(gp => gp.Gender)
                                .Include(a=>a.ActorMovies.OrderBy(a => a.Order))
                                    .ThenInclude(am => am.Actor)
                                .AsNoTracking()
                                .FirstOrDefaultAsync(a => a.Id == id);
    
    }

    public async Task<List<Movie>> GetByTitle(string title)
    {
        
        var result = await _context.Movies
                                .Where(a => a.Title.Contains(title))
                                .OrderBy(x=>x.Title)
                                .ToListAsync();

        return result;

    }

    public async Task<int> Create(Movie movie)
    {
        
        _context.Add(movie);
        await _context.SaveChangesAsync();
        return movie.Id;

    }

    public async Task<bool> Delete(int id)
    {
        await _context.Movies.Where(a => a.Id == id).ExecuteDeleteAsync();
        return true;
    }

    public Task<bool> Exist(int id)
    {
        return _context.Movies.AnyAsync(a => a.Id == id);
    }


    public async Task<Movie> Update(Movie movie)
    {

         _context.Update(movie);
        await _context.SaveChangesAsync();
        return movie;

    }

    /// <summary>
    /// Editing the Genders of a movie
    /// </summary>
    /// <param name="id"></param>
    /// <param name="gendersIds"></param>
    /// <returns></returns>
    public async Task AsignGender(int id, List<int> gendersIds)
    {

        var movie = await _context.Movies.Include(x => x.GenderMovies).FirstOrDefaultAsync(x => x.Id == id);

        if (movie == null)
        {
            throw new ArgumentException($"Movie not found with the id {id}");
        }

        var genderMovies = gendersIds.Select(genderId => new GenderMovie() { GenderId = genderId });

        movie.GenderMovies = _mapper.Map(genderMovies, movie.GenderMovies);

        await _context.SaveChangesAsync();

    }


    public async Task AsignActors(int id, List<ActorMovie> actors)
    {

        for(int i = 1; i < actors.Count; i++)
        {
            actors[i-1].Order = i;
        }

        var movie = await _context.Movies.Include(x => x.ActorMovies).FirstOrDefaultAsync(x => x.Id == id);

        if (movie is null)
        {
            throw new ArgumentException($"Movie not found with the id {id}");
        }

        movie.ActorMovies = _mapper.Map(actors, movie.ActorMovies);

        await _context.SaveChangesAsync();

    }


}
