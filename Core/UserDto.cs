namespace Core;

// DTO til at sende brugerdata til klienten UDEN at afsløre password (heller ikke hashet).
public class UserDto
{
    public int UserId { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
}
