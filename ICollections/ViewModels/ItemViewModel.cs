using System.ComponentModel.DataAnnotations;

namespace ICollections.ViewModels;

public class ItemViewModel
{
    //TODO
    //Add ErrorMessage
    //Translate ErrorMessage
    [Required]
    public int? CollectionId { get; set; }

    [Required]
    public string? Title { get; init; }
         
    [Required]
    public string? Description { get; init; }

    public string? Date { get; init; }
    public string? Brand { get; init; }
}