using ICollections.Services.Classes;
using ICollections.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace ICollections.ServiceAdding;

public static class EntitiesDatabase
{
    public static void AddDatabaseServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUserService, UserService>()
            .AddScoped<ICollectionService, CollectionService>()
            .AddScoped<IItemService, ItemService>()
            .AddScoped<ILikeService, LikeService>()
            .AddScoped<ICommentService, CommentService>()
            .AddSingleton<IUserIdProvider, CustomUserIdProvider>();;
    }
}