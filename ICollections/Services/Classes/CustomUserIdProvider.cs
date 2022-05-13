using Microsoft.AspNetCore.SignalR;

namespace ICollections.Services.Classes
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.Identity!.Name!;
        }
    }
}