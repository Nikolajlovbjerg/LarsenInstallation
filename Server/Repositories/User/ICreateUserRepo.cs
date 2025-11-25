using Core;
using System.Collections.Generic;
namespace Server.Repositories.User
{
    public interface ICreateUserRepo
    {
        List<User> GetAll();
        void Add(User user);
        void Delete(int id);
    }
}
