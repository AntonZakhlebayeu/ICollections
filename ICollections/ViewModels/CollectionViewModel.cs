using System.ComponentModel.DataAnnotations;

namespace ICollections.ViewModels;

public class CollectionViewModel
{
    //TODO
    //Add ErrorMessage
    //Translate ErrorMessage
    
    [Required]
    public string? AuthorId { get; init; }
    
    [Required]
    public string? Title { get; init; }
         
    [Required]
    public string? Description { get; init; }
    
    [Required]
    public string? Theme { get; init; }

    public string? IncludeDate { get; init; }
    public string? IncludeBrand { get; init; }
    public string? IncludeComments { get; init; }
}