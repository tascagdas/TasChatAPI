using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TasChatAPI.Context;
using TasChatAPI.DTO_s;
using TasChatAPI.Entities;

namespace TasChatAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChatsController(ApplicationDbContext context) : ControllerBase
    {
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
            return Ok();
        }
    }
}
