using Core;
namespace Server.Repositories
{
    public interface IProjectRepo
    {
        List<Project> GetAll();
        void Add(Project p);
        void DeleteById(int id);
    }

}
