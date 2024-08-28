using MangoDomain.EntititesTest;
using MangoFinancialApi.Dto;



namespace MangoFinancialApi.Repository;



public interface IRepositoryActor
{
    Task<List<Actor>> GetAll(PaginationDTO pagination);

    Task<bool> Exist(int id);
    Task<List<int>> Exists(List<int> ids);
    Task<Actor?> GetById(int id);
    Task<List<Actor>> GetByName(string name);
    Task<int> Create(Actor actor);
    Task<Actor> Update(Actor actor);
    Task<bool> Delete(int id);

}
