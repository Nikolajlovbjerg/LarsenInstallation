using Core;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private static readonly List<Project> _projects = new();

        [HttpPost]
        public IActionResult Create(Project project)
        {
            project.ProjectId = _projects.Count + 1;
            project.DateCreated = DateTime.Now;

            _projects.Add(project);
            return Ok(project);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_projects);
        }
    }
}