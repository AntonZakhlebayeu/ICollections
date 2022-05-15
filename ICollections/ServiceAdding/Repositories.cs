using ICollections.Data.Interfaces;
using ICollections.Data.Repositories;
using ICollections.Services.Classes;

namespace ICollections.ServiceAdding;

public static class Repositories
{
    public static void AddRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUserRepository, UserRepository>()
            .AddScoped<ICollectionRepository, CollectionRepository>()
            .AddScoped<ILikeRepository, LikeRepository>()
            .AddScoped<IItemRepository, ItemRepository>()
            .AddScoped<ICommentRepository, CommentRepository>()
            .AddScoped<IRoleRepository, RoleRepository>()
            .AddScoped<ITagRepository, TagRepository>();
    }
}