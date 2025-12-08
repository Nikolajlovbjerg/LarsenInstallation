using Core;

namespace Server.Repositories.ProjectRepositories;

public interface IProjectRepositorySQL
{
    int Create(Project p);
    void Update(Project p);
    void Delete(int id);
    Project? GetById(int id);
    IEnumerable<Project> GetAll();
}