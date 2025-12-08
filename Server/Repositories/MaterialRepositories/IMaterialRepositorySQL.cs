using Core;

namespace Server.Repositories.MaterialRepositories;

public interface IMaterialRepositorySQL
{
    void Add(ProjectMaterial material);
    List<ProjectMaterial> GetByProjectId(int projectId);
}