using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TasChatAPI.Context;
using TasChatAPI.DTO_s;
using TasChatAPI.Entities;
using TasChatAPI.Hubs;

namespace TasChatAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChatsController(ApplicationDbContext context, IHubContext<ChatHub> chatHubContext) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            List<User> users = await context.Users.OrderBy(u => u.UserName).ToListAsync();
            return Ok(users);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserChats(Guid fromUserId, Guid toUserId, CancellationToken cancellationToken)
        {
            List<Chat> chats = await context.Chats.Where(
                    c => c.FromUserId == fromUserId && c.ToUserId == toUserId ||
                         c.ToUserId == fromUserId && c.FromUserId == toUserId)
                .OrderBy(c=>c.SendDate)
                .ToListAsync();
            return Ok(chats);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(SendMessageDto request, CancellationToken cancellationToken)
        {
            Chat chat = new Chat()
            {
                FromUserId = request.FromUserId,
                ToUserId = request.ToUserId,
                Message = request.Message,
                SendDate = DateTime.Now
            };
            await context.AddAsync(chat, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            var connectionId = ChatHub.Users.First(x => x.Value == chat.ToUserId).Key;
            await chatHubContext.Clients.Client(connectionId).SendAsync("Messages", chat);
            
            return Ok(chat);
        }
    }
}
