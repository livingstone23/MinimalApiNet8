using MangoDomain.EntititesTest;



namespace MangoFinancialApi.Repository;



public interface IRepositoryGender
{
    Task<int> Create(Gender gender);

    Task<List<Gender>> GetAll();

    Task<Gender?> GetById(int id);

    Task<bool> Exist(int id);

    Task<bool> Update(Gender gender);

    Task<bool> Delete(int id);


}

