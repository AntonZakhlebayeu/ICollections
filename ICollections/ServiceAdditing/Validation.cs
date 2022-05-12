using ICollections.Services.Classes;
using ICollections.Services.Interfaces;

namespace ICollections.ServiceAdding;

public static class Validation
{
    public static void AddValidation(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUserValidation, UserValidationService>()
            .AddScoped<ICollectionValidation, CollectionValidationService>()
            .AddScoped<IItemValidation, ItemValidationService>()
            .AddScoped<ILikeValidation, LikeValidationService>();

    }
}