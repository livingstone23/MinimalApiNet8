using MangoDomain.EntititesTest;



namespace MangoFinancialApi.Repository;



public interface IRepositoryActor
{
    Task<List<Actor>> GetAll();

    Task<bool> IsExists(int id);
    Task<Actor?> GetById(int id);
    Task<int> Create(Actor actor);
    Task<Actor> Update(Actor actor);
    Task<bool> Delete(int id);

}
