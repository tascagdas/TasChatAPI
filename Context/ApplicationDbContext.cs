using Microsoft.EntityFrameworkCore;
using TasChatAPI.Entities;

namespace TasChatAPI.Context;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Chat> Chats { get; set; }
}