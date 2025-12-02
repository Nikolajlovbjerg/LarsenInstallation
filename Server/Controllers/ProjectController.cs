
using Microsoft.AspNetCore.Mvc;
using Core;
using Server.Repositories;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepo _projectRepo;

        public ProjectController(IProjectRepo projectRepo)
        {
            _projectRepo = projectRepo;
        }

        [HttpGet]
        public ActionResult<List<Project>> GetAll()
        {
            var projects = _projectRepo.GetAll();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public ActionResult<Project> GetById(int id)
        {
            var project = _projectRepo.GetAll().FirstOrDefault(p => p.ProjectId == id);
            if (project == null) return NotFound();
            return Ok(project);
        }

        [HttpPost]
        public ActionResult Add(Project project)
        {
            _projectRepo.Add(project);
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _projectRepo.DeleteById(id);
            return Ok();
        }
    }
}
