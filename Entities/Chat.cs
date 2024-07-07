namespace TasChatAPI.Entities;

public class Chat
{
    public Chat()
    {
        Id = Guid.NewGuid();
    }
    public Guid Id { get; set; }
    public Guid FromUserId { get; set; }
    public Guid ToUserId { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime SendDate { get; set; }
    public DateTime ReadDate { get; set; }
}