using HigLabo.Core;
using ICollections.Data.Interfaces;
using ICollections.Models;
using ICollections.Services.Interfaces;
using ICollections.ViewModels;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

namespace ICollections.Controllers;

public class SearchController : Controller
{
    private readonly IItemService _itemService;
    public SearchController(IItemService itemService)
    {
        _itemService = itemService;
    }
    
    [Route("/Home/Search/")]
    public async Task<IActionResult> SearchResultViewEmpty(string searchString, SearchViewModel searchViewModel)
    {
        searchViewModel.resultItems = new List<Item>();

        return await Task.Run(() => View("Search", searchViewModel));
    }
    
    [Route("/Home/Search/{searchString}")]
    public async Task<IActionResult> SearchResultView(string searchString, SearchViewModel searchViewModel)
    {
        searchViewModel.resultItems = _itemService.FullTextSearch(searchString);
        
        return await Task.Run(() => View("Search", searchViewModel));
    }
    
    [Route("/Home/Tags/{tagString}")]
    public async Task<IActionResult> SearchByTagView(string tagString, SearchViewModel searchViewModel)
    {
        searchViewModel.resultItems = _itemService.GetItemsByTag(tagString);
        
        return await Task.Run(() => View("Search", searchViewModel));
    }
}