namespace ICollections.Models;

public class Like
{
    public string ItemId { get; set; }
    public Item Item { get; set; }

    public string UserId { get; set; }
    public User User { get; set; }
}