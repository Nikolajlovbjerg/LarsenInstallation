using Core;

namespace Server.Repositories.HourRepositories;

public interface IHourRepositorySQL
{
    void Add(ProjectHour hour);
    List<ProjectHour> GetByProjectId(int projectId);
    //hej
    List<Calculation> GetTotalHoursGroupedByType();
}