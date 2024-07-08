using Microsoft.EntityFrameworkCore;
using TasChatAPI.Context;
using TasChatAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(optionsBuilder => 
    optionsBuilder.UseSqlite(
        builder.Configuration.GetConnectionString("Sqlite")
    ));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chat-hub");


app.Run();