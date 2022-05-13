using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ICollections.Models;

public class Comment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; init; }
    
    public string? UserNickName { get; init; }
    public int ItemId { get; init; }
    public string? CommentText { get; init; }
    public string? CommentWhen { get; init; }
}