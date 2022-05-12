using ICollections.Data;
using ICollections.Services.Classes;
using ICollections.Services.Interfaces;

namespace ICollections.ServiceAdditing;

public static class EntitiesDatabase
{
    public static void AddDatabaseServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUserDatabase, UserDatabaseService>()
            .AddScoped<ICollectionDatabase, CollectionDatabaseService>()
            .AddScoped<IItemDatabase, ItemDatabaseService>();
    }
}