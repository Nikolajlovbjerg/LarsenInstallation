using Microsoft.AspNetCore.Mvc;
using Core;
using Server.Repositories;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/projecthours")]
    public class ProjecthoursController : ControllerBase
    {
        private readonly IProjectRepo _projectRepo;

        public ProjecthoursController(IProjectRepo projectRepo)
        {
            _projectRepo = projectRepo;
        }

        [HttpGet]
        public IEnumerable<Calculation> GetCalculations()
        {
            return _projectRepo.GetCalculations();
        }
    }
}