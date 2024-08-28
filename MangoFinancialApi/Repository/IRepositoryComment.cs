using MangoDomain.EntititesTest;



namespace MangoFinancialApi.Repository;



public interface IRepositoryComment
{



    Task<List<Comment>> GetAll(int movieId);

    Task<Comment?> GetById(int id);

    Task<bool> Exist(int id);

    Task<int> Create(Comment comment);
    
    Task Update(Comment comment);
    
    Task<bool> Delete(int id);

}
