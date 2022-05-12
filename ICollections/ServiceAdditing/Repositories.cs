using ICollections.Data.Interfaces;
using ICollections.Data.Repositories;

namespace ICollections.ServiceAdding;

public static class Repositories
{
    public static void AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUserRepository, UserRepository>()
            .AddScoped<ICollectionRepository, CollectionRepository>()
            .AddScoped<ILikeRepository, LikeRepository>()
            .AddScoped<IItemRepository, ItemRepository>();
    }
}