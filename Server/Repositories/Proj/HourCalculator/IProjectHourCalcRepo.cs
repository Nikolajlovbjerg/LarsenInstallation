using Core;
namespace Server.Repositories.Proj.HourCalculator
{
    public interface IProjectHourCalcRepo
    {
        List<Project> GetAll();

        List<Calculation> GetCalculations();
        
    }

}
