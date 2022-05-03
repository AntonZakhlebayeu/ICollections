using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ICollections.Models;

public class Item
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string? Title { get; set; }
    
    public string? Description { get; set; }
    
    public string? LastEditDate { get; set; }
    
    public string? Date { get; set; }
    public string? Brand { get; set; }
    
    public string? FileName { get; set; }
    
    public string? FileUrl { get; set; }
    
    //TODO
    //Add comments
    
    public int? CollectionId { get; set; }
}