using Core;

namespace Server.Repositories
{
    public interface ICreateProjectRepo
    {
        int Add(Project pro);
        
        void AddHour(ProjectHour proj);
        
        void AddMaterials(ProjectMaterial projmat);
        
        Calculation? GetProjectDetails(int projectId);
        
        IEnumerable<Project> GetAllProjects();
        
        void Update(Project pro);

        void Delete(int id);
    }
}