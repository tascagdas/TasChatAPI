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
    public class AuthController(
        ApplicationDbContext context) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Register([FromForm]RegisterDto request, CancellationToken cancellationToken)
        {
            bool isNameExist = await context.Users.AnyAsync(user => user.UserName == request.UserName, cancellationToken: cancellationToken);
            if (isNameExist)
            {
                return BadRequest(new {Message = "Bu kullanıcı adı kullanılmakta."});
            }

            string userImage = FileService.FileSaveToServer(request.File, "wwwroot/avatars");
            User user = new User()
            {
                UserName = request.UserName,
                UserImage = userImage
            };
            await context.AddAsync(user, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> Login( string userName, CancellationToken cancellationToken)
        {
            User? user = await context.Users.FirstOrDefaultAsync(u => u.UserName == userName, cancellationToken);
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
