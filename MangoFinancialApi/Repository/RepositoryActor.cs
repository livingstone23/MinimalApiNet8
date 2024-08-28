using AutoMapper;
using MangoDomain.EntititesTest;
using MangoFinancialApi.Data;
using MangoFinancialApi.Dto;
using MangoFinancialApi.Utility;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;



namespace MangoFinancialApi.Repository;



public class RepositoryActor : IRepositoryActor
{

    
    private readonly ApplicationDbContext _context;

    private readonly HttpContext httpContext;

    public RepositoryActor(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        httpContext = httpContextAccessor.HttpContext!;

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

    public async Task<List<Actor>> GetAll(PaginationDTO pagination)
    {
        var queryable = _context.Actors.AsQueryable();
        await httpContext.InsertPaginationParametersInResponse(queryable);
        var result = await queryable.OrderBy(a => a.Name).Paginate(pagination).ToListAsync();
        return result;
    }

    public async Task<Actor?> GetById(int id)
    {
        return await _context.Actors.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<Actor>> GetByName(string name)
    {
        var result = await _context.Actors
                                .Where(a => a.Name.Contains(name))
                                .OrderBy(x=>x.Name)
                                .ToListAsync();

        return result;
    }

    public Task<bool> Exist(int id)
    {
        return _context.Actors.AnyAsync(a => a.Id == id);
    }

    public async Task<List<int>> Exists(List<int> ids)
    {
        return await _context.Actors.Where(a => ids.Contains(a.Id)).Select(a => a.Id).ToListAsync();
    }

 

    public async Task<Actor> Update(Actor actor)
    {
        _context.Update(actor);
        await _context.SaveChangesAsync();
        return actor;
    }


    




}
