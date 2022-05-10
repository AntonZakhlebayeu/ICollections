namespace ICollections.Services.Interfaces;

public interface ILikeValidation
{
    bool IsUserOwner(string userEmail, int collectionId);
}