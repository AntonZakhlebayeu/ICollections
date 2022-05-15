using System.ComponentModel.DataAnnotations;

namespace ICollections.ViewModels;

public class EditItemViewModel
{
    //TODO
    //Add ErrorMessage
    //Translate ErrorMessage
    [Required]
    public int ItemId { get; set; }

    [Required]
    public string? Title { get; init; }
         
    [Required]
    public string? Description { get; init; }
    
    public string? Tags { get; init; }

    public string? Date { get; init; }
    public string? Brand { get; init; }
    
    public string? DeleteImage { get; init; }
}