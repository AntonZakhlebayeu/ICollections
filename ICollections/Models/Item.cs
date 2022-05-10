using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ICollections.Models;

public class Item
{
    public Item()
    {
        Likes = new List<Like>();
    }
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string? Title { get; set; }
    
    public string? Description { get; set; }
    
    public string? LastEditDate { get; set; }
    
    public string? Date { get; set; }
    public string? Brand { get; set; }
    
    public string? FileName { get; set; }

    //TODO
    //Add comments
    
    public int CollectionId { get; set; }

    public List<Like> Likes { get; set; }
}