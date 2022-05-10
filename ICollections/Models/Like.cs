namespace ICollections.Models;

public class Like
{
    public int ItemId { get; set; }
    public Item Item { get; set; }

    public string UserId { get; set; }
    public User User { get; set; }
}