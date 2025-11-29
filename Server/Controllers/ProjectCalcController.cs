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

        [HttpGet]
        public IEnumerable<Project> get()
        {
            return calcRepo.GetAll();
        }
    }
}
