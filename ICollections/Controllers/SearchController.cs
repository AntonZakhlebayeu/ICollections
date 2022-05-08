using HigLabo.Core;
using ICollections.Data;
using ICollections.ViewModels;
using Korzh.EasyQuery.Linq;
using Microsoft.AspNetCore.Mvc;

namespace ICollections.Controllers;

public class SearchController : Controller
{
    private readonly ApplicationDbContext _db;

    public SearchController(ApplicationDbContext db)
    {
        _db = db;
    }
    
    [Route("/Home/Search/{searchString}")]
    public async Task<IActionResult> SearchResultView(string searchString, SearchViewModel searchViewModel)
    {
        if (!searchString.IsNullOrEmpty())
        {
            searchViewModel.resultItems = _db.Items.FullTextSearchQuery(searchString).ToList();
            var itemsInCollection = _db.Collections.FullTextSearchQuery(searchString);

            foreach (var collection in itemsInCollection)
            {
                collection.CollectionItems = _db.Items.Where(c => c.CollectionId == collection.CollectionId).ToList();

                foreach (var item in collection!.CollectionItems!.Where(item => !searchViewModel.resultItems.Contains(item)))
                {
                    searchViewModel.resultItems.Add(item);
                }
            }
        }
        else
        {
            searchViewModel.resultItems = _db.Items.ToList();
        }

        return await Task.Run(() => View("Search", searchViewModel));
    }
}