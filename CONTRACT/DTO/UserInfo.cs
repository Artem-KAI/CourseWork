namespace CONTRACT.DTO;

public class UserInfo
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    public UserInfo() { }

    public UserInfo(int id, string username, string email, string role)
    {
        Id = id;
        Username = username;
        Email = email;
        Role = role;
    }
}
