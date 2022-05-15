using System.Text;
using ICollections.Data.Interfaces;
using ICollections.Models;
using ICollections.Services.Interfaces;

namespace ICollections.Services.Classes;

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;

    public TagService(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task AddTag(string tag)
    {
        var checkTag = await _tagRepository.GetSingleAsync(t => t.TagText == tag);
        if(checkTag != null) return;
        
        await _tagRepository.AddAsync(new Tag{ TagText = tag});
        await _tagRepository.CommitAsync();
    }

    public List<string> GetAll()
    {
        var tags = _tagRepository.GetAll().ToList();

        return tags.Select(tag => tag.TagText!).ToList();
    }
}