using GenericFileService.Files;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TasChatAPI.Context;
using TasChatAPI.DTO_s;
using TasChatAPI.Entities;

namespace TasChatAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController(ApplicationDbContext context) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto request, CancellationToken cancellationToken)
        {
            bool isNameUniq = await context.Users.AnyAsync(user => user.Name == request.Name, cancellationToken: cancellationToken);
            if (!isNameUniq)
            {
                return BadRequest(new {Message = "Bu kullanıcı adı kullanılmakta."});
            }

            string userImage = FileService.FileSaveToServer(request.File, "wwwroot/avatars");
            User user = new User()
            {
                Name = request.Name,
                UserImage = userImage
            };
            await context.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Login( string name, CancellationToken cancellationToken)
        {
            User? user = await context.Users.FirstOrDefaultAsync(u => u.Name == name, cancellationToken);
            if (user is null)
            {
                return BadRequest(new { Message = "Oturum açmaya çalışılan kullanıcı bulunamamıştır." });
            }

            user.Status = "online";
            await context.SaveChangesAsync(cancellationToken);
            return Ok(user);
        }
    }
}
