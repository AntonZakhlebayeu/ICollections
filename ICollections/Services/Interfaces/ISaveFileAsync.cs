namespace ICollections.Services;

public interface ISaveFileAsync
{
    public Task<string> SaveFileAsync(IFormFile file);
}