namespace MangoFinancialApi.Services;



public interface IStoreFiles
{
    
    //Task<string> SaveFileAsync(byte[] file);
    //Task<byte[]> GetFileAsync(string route);
    Task DeleteFileAsync(string route, string container);
    Task<string> SaveFileAsync(string container, IFormFile file);
    
    //Implementacion por defecto de editar, esta funcionalidad es reciente en .net8 
    async Task<string> Edit(string route, string container, IFormFile file)
    {
        await DeleteFileAsync(route, container);
        return await SaveFileAsync(container, file);
    }

}
