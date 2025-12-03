using Core;

namespace Server.Repositories
{
    public interface ICreateProjectRepo
    {
        int Add(Core.Project pro);
        
        Calculation? GetProjectDetails(int projectId);
    }
}