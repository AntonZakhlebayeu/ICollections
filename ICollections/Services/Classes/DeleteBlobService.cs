using Azure.Storage.Blobs;
using ICollections.Services.Interfaces;

namespace ICollections.Services.Classes;

public class DeleteBlobService : IDeleteBlob
{
    private readonly IConfiguration _configuration;

    public DeleteBlobService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async void DeleteBlob(string? fileName)
    {
        var serverClient = new BlobServiceClient(_configuration.GetConnectionString("BlobStorageConnection"));
        var containerClient = serverClient.GetBlobContainerClient("images");
        
        var blobClient = containerClient.GetBlobClient(fileName);
        
        await blobClient.DeleteAsync();
    }
}