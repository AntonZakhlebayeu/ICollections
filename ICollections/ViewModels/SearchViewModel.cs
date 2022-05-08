using ICollections.Models;

namespace ICollections.ViewModels;

public class SearchViewModel
{
    public List<Item> resultItems { get; set; } = new List<Item>();

    public string? Text { get; set; }
}