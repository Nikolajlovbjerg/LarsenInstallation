using Core;
namespace Server.Repositories
{
    public interface IProjectRepo
    {
        List<Project> GetAll();

        List<Calculation> GetCalculations();
        
    }

}
