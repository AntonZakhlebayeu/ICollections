namespace ICollections.Models;

public class Like
{
    public int ItemId { get; init; }
    public Item? Item { get; set; }

    public string UserId { get; init; } = null!;
    public User? User { get; set; }
}