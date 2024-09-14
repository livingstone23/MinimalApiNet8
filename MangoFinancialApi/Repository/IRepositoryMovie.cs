using MangoDomain.EntititesTest;
using MangoFinancialApi.Dto;



namespace MangoFinancialApi.Repository;



public interface IRepositoryMovie
{

    Task<List<Movie>> GetAll(PaginationDTO pagination);

    Task<bool> Exist(int id);
    
    Task<Movie?> GetById(int id);
    
    Task<List<Movie>> GetByTitle(string title);
    
    Task<int> Create(Movie movie);
    
    Task<Movie> Update(Movie movie);

    Task<bool> Delete(int id);

    Task AsignGender(int id, List<int> gendersId);

    Task AsignActors(int id, List<ActorMovie> actors);


    /// <summary>
    /// Method to apply filter and order to the movies
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    Task<List<Movie>> Filter(MovieFilterDTO filter);
    
}
