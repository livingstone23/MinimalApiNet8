using System.Net.Mime;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Path = System.IO.Path;


namespace MangoFinancialApi.Services;



public class StoreFilesAzure : IStoreFiles
{

    private string? connectionString;
    private string basecontainerName = "imsa-account-statement-dev";

    public StoreFilesAzure(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("AzureStorage")!;   
    }

    public async Task<string> SaveFileAsync(string container, IFormFile file)
    {
        var client = new BlobContainerClient(connectionString, $"{basecontainerName}");
        await client.CreateIfNotExistsAsync();
        client.SetAccessPolicy(PublicAccessType.Blob);

        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{container}/{Guid.NewGuid()}{extension}";
        var blobClient = client.GetBlobClient(fileName);
        var blobHttpHeaders = new BlobHttpHeaders();
        blobHttpHeaders.ContentType = file.ContentType;
        await   blobClient.UploadAsync(file.OpenReadStream(), blobHttpHeaders);
        return blobClient.Uri.ToString();
    }

    public async Task DeleteFileAsync(string route, string container)
    {
        if(string.IsNullOrEmpty(route))
        {
            return;
        }

        var client = new BlobContainerClient(connectionString, $"{basecontainerName}{container}");
        await client.CreateIfNotExistsAsync();
        var fileName = Path.GetFileName(route);
        var blob = client.GetBlobClient(fileName);
        await blob.DeleteIfExistsAsync();
    }

    
}








