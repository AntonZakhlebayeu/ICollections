using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;

namespace ICollections.Services.Classes;

public class SaveFileToCloudService : IGetFileName, IPushToCloud, ISaveFileAsync
{
    private readonly IConfiguration _configuration;

    public SaveFileToCloudService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetFileName()
    {
        var fileName = Guid.NewGuid().ToString();
        return fileName;
    }

    public async Task PushToCloud(string fileName, string path)
    {
        var connectionString = _configuration.GetConnectionString("BlobStorageConnection");
        
        var serverClient = new BlobServiceClient(connectionString);
        var containerClient = serverClient.GetBlobContainerClient("images");
        var blobClient = containerClient.GetBlobClient(fileName);
        await using var uploadFileStream = System.IO.File.OpenRead(path);
        
        await blobClient.UploadAsync(uploadFileStream, true);
        uploadFileStream.Close();

        System.IO.File.Delete(fileName);
    }

    public async Task<string> SaveFileAsync(IFormFile file)
    {
        var originalFileName = Path.GetFileName(file.FileName);
        var extension = originalFileName.Substring(originalFileName.LastIndexOf('.') + 1, originalFileName.Length - 1 - originalFileName.LastIndexOf('.'));
        var uniqueFileName = GetFileName();

        await using (var stream = System.IO.File.Create(uniqueFileName + '.' + extension))
        {
            await file.CopyToAsync(stream);
        }

        var resultingName = uniqueFileName + '.' + extension;
        await PushToCloud(resultingName, resultingName);

        return resultingName;
    }
}