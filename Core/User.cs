namespace Core;

public class User
{
    public int UserId { get; set; }
    
    public string Username { get; set; } = String.Empty;
    
    public string Password { get; set; } = String.Empty;
    
    public string Role { get; set; } = "none";
}