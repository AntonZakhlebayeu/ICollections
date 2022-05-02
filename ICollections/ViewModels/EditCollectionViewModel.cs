using System.ComponentModel.DataAnnotations;

namespace ICollections.ViewModels;

public class EditCollectionViewModel
{
    [Required]
    public int? CollectionId { get; init; }
    
    [Required]
    public string? Title { get; init; }
         
    [Required]
    public string? Description { get; init; }
}