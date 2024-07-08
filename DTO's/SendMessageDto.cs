namespace TasChatAPI.DTO_s;

public class SendMessageDto
{
    public Guid FromUserId { get; set; }
    public Guid ToUserId { get; set; }
    public string Message { get; set; }
}