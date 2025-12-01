using Core;
using Microsoft.AspNetCore.Mvc;
using Server.Repositories;
using Server.Repositories.ProjCalc;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/projcalc")]
    public class ProjectCalcController : Controller
    {
        private IProjectTotalCalcRepo calcRepo;
        public ProjectCalcController(IProjectTotalCalcRepo calcRepo)
        {
            this.calcRepo = calcRepo;
        }

        [HttpGet("calculations")]
        public IEnumerable<Calculation> get()
        {
            return calcRepo.GetAll();
        }
        [HttpGet("projects")]
        public IEnumerable<Project> getprojects()
        {
            return calcRepo.GetAllProjects();
        }
    }
}
