using HigLabo.Core;
using ICollections.Models;
using ICollections.Services.Interfaces;
using Korzh.EasyQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ICollections.Hub;

public class CommentsHub : Microsoft.AspNetCore.SignalR.Hub
{
    private readonly IServiceProvider _serviceProvider;

    public CommentsHub(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    [Authorize]
    public async Task SendComment(string comment, string itemId)
    {
        var id = itemId.ToInt();
        
        using var scope = _serviceProvider.CreateScope();
        
        var commentService = scope.ServiceProvider.GetRequiredService<ICommentService>();
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

        var user = await userService.GetUserByEmail(Context!.User!.Identity!.Name!).ConfigureAwait(false);

        var commentTime = DateTime.UtcNow.AddHours(3).ToString("MM/dd/yyyy H:mm");
        
        var newComment = new Comment{ CommentText = comment, CommentWhen = commentTime, UserNickName = user.NickName, ItemId = id};
        
        await commentService.AddComment(newComment);

        await Clients.All.SendAsync("ReceiveComment", user.NickName,comment, commentTime);
    }
}