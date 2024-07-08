using Microsoft.AspNetCore.SignalR;
using TasChatAPI.Context;
using TasChatAPI.Entities;

namespace TasChatAPI.Hubs;

public class ChatHub(ApplicationDbContext dbContext) : Hub
{
    public static Dictionary<string, Guid> Users = new();
    public async Task Connect(Guid userId)
    {
        Users.Add(Context.ConnectionId,userId);
        User? user = await dbContext.Users.FindAsync(userId);
        if (user is not null)
        {
            user.Status = "online";
            await dbContext.SaveChangesAsync();

            await Clients.All.SendAsync("Users", user);
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Users.TryGetValue(Context.ConnectionId, out var userId); 
        User? user = await dbContext.Users.FindAsync(userId);
            if (user is not null)
            {
                user.Status = "offline";
                await dbContext.SaveChangesAsync();
                await Clients.All.SendAsync("Users", user);
            }
    }
}