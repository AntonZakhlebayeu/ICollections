using ICollections.Data;

namespace ICollections.ServiceAdding;

public static class DbContext
{
    public static void AddDbContext(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContext<CollectionDbContext>()
            .AddDatabaseDeveloperPageExceptionFilter();
    }
}