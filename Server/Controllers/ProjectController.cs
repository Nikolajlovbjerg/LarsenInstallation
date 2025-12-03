using Microsoft.AspNetCore.Mvc;
using Core;
using Server.Repositories;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/projectlist")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepo _projectRepo;

        public ProjectController(IProjectRepo projectRepo)
        {
            _projectRepo = projectRepo;
        }

        [HttpGet]
        public IEnumerable<Project> GetAll()
        {
            return _projectRepo.GetAll();
        }
        /*
        [HttpGet("{id}")]
        public ActionResult<Project> GetById(int id)
        {
            var project = _projectRepo.GetAll().FirstOrDefault(p => p.ProjectId == id);
            if (project == null) return NotFound();
            return Ok(project);
        } */
    }
}