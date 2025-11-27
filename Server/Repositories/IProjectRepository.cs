using System.Data;
using System.Threading.Tasks;
using Core;
namespace Server.Repositories
{
    public interface IProjectRepository
    {
        /// <summary>
        /// Opretter et projekt og returnerer ProjectId.
        /// </summary>
        Task<int> CreateProjectAsync(Project project);

        /// <summary>
        /// Opretter projektet og indsætter alle Hour- og Material-rækker
        /// i samme database-transaktion.
        /// </summary>
        Task CreateProjectWithDataAsync(Project project, DataTable? hoursTable, DataTable? materialsTable);
    }
}