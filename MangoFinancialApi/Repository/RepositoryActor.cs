using MangoDomain.EntititesTest;
using MangoFinancialApi.Data;
using Microsoft.EntityFrameworkCore;



namespace MangoFinancialApi.Repository;



public class RepositoryActor : IRepositoryActor
{

    private readonly ApplicationDbContext _context;

    public RepositoryActor(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<int> Create(Actor actor)
    {
        _context.Add(actor);
        await _context.SaveChangesAsync();
        return actor.Id;
    }

    public async Task<bool> Delete(int id)
    {
        await _context.Actors.Where(a => a.Id == id).ExecuteDeleteAsync();
        return true;
    }

    public async Task<List<Actor>> GetAll()
    {
        return await _context.Actors.OrderBy(a => a.Name).ToListAsync();
    }

    public async Task<Actor?> GetById(int id)
    {
        return await _context.Actors.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
    }

    public Task<bool> IsExists(int id)
    {
        return _context.Actors.AnyAsync(a => a.Id == id);
    }

    public async Task<Actor> Update(Actor actor)
    {
        _context.Update(actor);
        await _context.SaveChangesAsync();
        return actor;
    }
}
