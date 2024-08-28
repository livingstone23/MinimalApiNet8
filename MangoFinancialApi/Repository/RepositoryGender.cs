using MangoDomain.EntititesTest;
using MangoFinancialApi.Data;
using Microsoft.EntityFrameworkCore;



namespace MangoFinancialApi.Repository;



public class RepositoryGender : IRepositoryGender
{

    private readonly ApplicationDbContext _context;


    public RepositoryGender(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Create(Gender gender)
    {
        
        _context.Add(gender);
        await _context.SaveChangesAsync();
        return gender.Id;

    }

    public async Task<bool> Delete(int id)
    {
        await _context.Genders.Where(x => x.Id == id).ExecuteDeleteAsync();
        return true;
    }

    public  async Task<bool> Exist(int id)
    {
        
        return await _context.Genders.AnyAsync(x => x.Id == id);

    }

    /// <summary>
    /// Method for checking if the name already exists in the database
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public  async Task<bool> Exist(int id, string name)
    {
        
        return await _context.Genders.AnyAsync(x => x.Id != id && x.Name == name);

    }

    public async Task<List<int>> Exists(List<int> ids)
    {

        return await _context.Genders.Where(g => ids.Contains(g.Id)).Select(g => g.Id).ToListAsync();

    }


    public async Task<List<Gender>> GetAll()
    {
    
        return await _context.Genders.OrderBy(x=>x.Name).ToListAsync();
    
    }

    public async Task<Gender?> GetById(int id)
    {
        return await _context.Genders.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<bool> Update(Gender gender)
    {
        _context.Update(gender);
        await _context.SaveChangesAsync();
        return true;
    }
}