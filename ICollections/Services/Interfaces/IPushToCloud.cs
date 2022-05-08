namespace ICollections.Services.Interfaces;

public interface IPushToCloud
{
    public Task PushToCloud(string fileName, string path);
}