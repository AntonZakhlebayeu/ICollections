using ICollections.Data;

namespace ICollections.ServiceAdditing;

public static class DbContext
{
    public static void AddDbContext(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddDbContext<CollectionDbContext>()
            .AddDatabaseDeveloperPageExceptionFilter();
    }
}