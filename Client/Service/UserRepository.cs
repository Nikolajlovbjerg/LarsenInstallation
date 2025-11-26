using Core;
namespace Client.Service;

public class UserRepository
{
    private List<Users> mUsers;
    
    public UserRepository()
    {
        mUsers =
        [
            new Users { UserName = "admin", Password = "admin", Role = "admin" },
            new Users { UserName = "rip", Password = "1234", Role = "Normal" }
        ];
    }

    public Users? ValidLogin(string name, string password)
    {
        foreach (Users u in mUsers)
            if (u.UserName == name && u.Password == password)
            {
                return u;
            }

        return null;
    }
}