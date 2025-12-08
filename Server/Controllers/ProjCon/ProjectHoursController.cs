/*using Microsoft.AspNetCore.Mvc;
using Core;
using Server.Repositories.Proj.HourCalculator;

namespace Server.Controllers.ProjCon
{
    [ApiController]
    [Route("api/projecthours")]
    public class ProjecthoursController : ControllerBase
    {
        private readonly IProjectHourCalcRepo _projectRepo;

        public ProjecthoursController(IProjectHourCalcRepo projectRepo)
        {
            _projectRepo = projectRepo;
        }

        [HttpGet]
        public IEnumerable<Calculation> GetCalculations()
        {
            return _projectRepo.GetCalculations();
        }
    }
}*/