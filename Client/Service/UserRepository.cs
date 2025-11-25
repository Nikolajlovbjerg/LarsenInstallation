using Core;
namespace Client.Service;

public class UserRepository
{
    private List<User> mUsers;
    
    public UserRepository()
    {
        mUsers =
        [
            new User { Username = "admin", Password = "admin", Role = "admin" },
            new User { Username = "rip", Password = "1234", Role = "Normal" }
        ];
    }

    public User? ValidLogin(string name, string password)
    {
        foreach (User u in mUsers)
            if (u.Username == name && u.Password == password)
            {
                return u;
            }

        return null;
    }
}