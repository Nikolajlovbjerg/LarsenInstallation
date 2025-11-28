using Core;

namespace Server.Repositories.ProjCalc
{
    public interface IProjectTotalCalcRepo
    {
        List<Calculation> GetAll();
    }
}
