using ICollections.Services;
using ICollections.Services.Classes;
using ICollections.Services.Interfaces;

namespace ICollections.ServiceAdditing;

public static class BlobsManager
{
    public static void AddBlobs(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ISaveFileAsync, SaveFileToCloudService>()
            .AddSingleton<IDeleteBlob, DeleteBlobService>();
    }
}