namespace Core;

// Svar fra login-endpointet: et JWT-token + brugerens data (uden password).
public class LoginResponse
{
    public string Token { get; set; } = string.Empty;

    public UserDto User { get; set; } = new();
}
