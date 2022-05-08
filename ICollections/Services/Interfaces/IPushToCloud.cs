namespace ICollections.Services;

public interface IPushToCloud
{
    public Task PushToCloud(string fileName, string path);
}