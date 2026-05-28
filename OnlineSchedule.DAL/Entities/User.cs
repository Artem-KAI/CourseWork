namespace DAL.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }

    // Admin, Editor, Management, Teacher, Student
    public string Role { get; set; }
}