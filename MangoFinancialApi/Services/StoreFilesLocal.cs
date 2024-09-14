
using Path = System.IO.Path;

namespace MangoFinancialApi.Services;

public class StoreFilesLocal : IStoreFiles
{

    private readonly IWebHostEnvironment env;
    private readonly IHttpContextAccessor httpContextAccessor;

    public StoreFilesLocal(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
    {
        this.env = env;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> SaveFileAsync(string container, IFormFile file)
    {
        
        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{Guid.NewGuid()}{extension}";
        string folder = Path.Combine(env.WebRootPath, container);
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        string route = Path.Combine(folder, fileName);
        using (var ms = new MemoryStream())
        {
            await file.CopyToAsync(ms);
            var fileBytes = ms.ToArray();
            await File.WriteAllBytesAsync(route, fileBytes);
        }
        
        var url = $"{httpContextAccessor.HttpContext!.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}/{container}/{fileName}";
        var urlFile = url.Replace("\\", "/");
        return urlFile;
    }

    public  Task DeleteFileAsync(string? route, string container)
    {
        
        if(string.IsNullOrEmpty(route))
        {
            return Task.CompletedTask;
        }

        var fileName = Path.GetFileName(route);
        var directoryFile = Path.Combine(env.WebRootPath, container, fileName);
        
        if (File.Exists(directoryFile))
        {
            File.Delete(directoryFile);
        }
        
        return Task.CompletedTask;

    }

    

}
