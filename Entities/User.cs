namespace TasChatAPI.Entities;

public class User
{
    public User()
    {
        Id = Guid.NewGuid();
    }
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string UserImage { get; set; }
    public string Status { get; set; }
}