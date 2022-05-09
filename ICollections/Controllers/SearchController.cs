using HigLabo.Core;
using ICollections.Data.Interfaces;
using ICollections.Models;
using ICollections.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ICollections.Controllers;

public class SearchController : Controller
{
    private readonly ICollectionRepository _collectionRepository;
    private readonly IItemRepository _itemRepository;

    public SearchController(IItemRepository itemRepository, ICollectionRepository collectionRepository)
    {
        _itemRepository = itemRepository;
        _collectionRepository = collectionRepository;
    }
    
    [Route("/Home/Search/")]
    public async Task<IActionResult> SearchResultView(SearchViewModel searchViewModel)
    {
        if (!searchViewModel.Text.IsNullOrEmpty())
        {
            searchViewModel.resultItems = _itemRepository.FullTextSearch(searchViewModel.Text).ToList();
            
            var itemsInCollection = _collectionRepository.FullTextSearch(searchViewModel.Text);

            foreach (var collection in itemsInCollection)
            {
                collection.CollectionItems = _itemRepository.FindBy(c => c.CollectionId == collection.Id).ToList();

                foreach (var item in collection!.CollectionItems!.Where(item => !searchViewModel.resultItems.Contains(item)))
                {
                    searchViewModel.resultItems.Add(item);
                }
            }
        }
        else
        {
            searchViewModel.resultItems = new List<Item>();
        }

        return await Task.Run(() => View("Search", searchViewModel));
    }
}