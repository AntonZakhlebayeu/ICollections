using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ICollections.Models;

public class Collection
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; set; }
    
    public string? AuthorId { get; set; }
    
    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Theme { get; set; }
    
    public string? LastEditDate { get; set; }
    
    public string? FileName { get; set; }

    public string? AddDates { get; set; }
    public string? AddBrands { get; set; }
    public string? AddComments { get; set; }

    public List<Item>? CollectionItems { get; set; } = new List<Item>();
}