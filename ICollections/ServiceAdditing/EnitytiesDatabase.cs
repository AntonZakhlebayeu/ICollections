using ICollections.Services.Classes;
using ICollections.Services.Interfaces;

namespace ICollections.ServiceAdding;

public static class EntitiesDatabase
{
    public static void AddDatabaseServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUserDatabase, UserDatabaseService>()
            .AddScoped<ICollectionDatabase, CollectionDatabaseService>()
            .AddScoped<IItemDatabase, ItemDatabaseService>()
            .AddScoped<ILikeDatabase, LikeDatabaseService>();
    }
}