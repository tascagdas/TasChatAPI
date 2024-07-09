namespace TasChatAPI.Entities;

public class User
{
    public User()
    {
        Id = Guid.NewGuid();
    }
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserImage { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}