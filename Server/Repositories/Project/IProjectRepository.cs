using Core;

namespace Server.Repositories.Project
{
    public interface IProjectRepository
    {
        // Returnerer ID på det oprettede projekt
        int CreateProject(Core.Project project, List<ProjectHour> hours, List<ProjectMaterial> materials);
        
        ProjectDetailsDTO? GetProjectDetails(int projectId);
    }
}