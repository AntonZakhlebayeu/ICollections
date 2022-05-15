using ICollections.Models;

namespace ICollections.Services.Interfaces;

public interface ITagService
{
    Task AddTag(string tag);
    List<string> GetAll();
}