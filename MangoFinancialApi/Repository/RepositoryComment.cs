using MangoDomain.EntititesTest;
using MangoFinancialApi.Data;
using Microsoft.EntityFrameworkCore;



namespace MangoFinancialApi.Repository;



public class RepositoryComment: IRepositoryComment
{


    private readonly ApplicationDbContext _context;
    private readonly HttpContext httpContext;


    public RepositoryComment(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        httpContext = httpContextAccessor.HttpContext!;
    }


    public async Task<List<Comment>> GetAll(int movieId)
    {

        var result = await _context.Comments.Where(c => c.MovieId == movieId).ToListAsync();
    
        return result;

    }

    public async Task<Comment?> GetById(int id)
    {

        return await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
    
    }

    public async Task<int> Create(Comment comment)
    {
        
        _context.Add(comment);
        await _context.SaveChangesAsync();
        return comment.Id;
    }

    public async Task Update(Comment comment)
    {
        
        _context.Update(comment);
        await _context.SaveChangesAsync();
        
    }

    public async Task<bool> Delete(int id)
    {
        
        var comment = await GetById(id);
        if(comment is null)
        {
            return false;
        }

        _context.Remove(comment);
        await _context.SaveChangesAsync();
        return true;
    }

    public Task<bool> Exist(int id)
    {
        return _context.Comments.AnyAsync(c => c.Id == id);
    }



}
