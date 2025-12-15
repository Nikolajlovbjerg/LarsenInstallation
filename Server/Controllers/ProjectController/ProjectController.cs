using Core;
using Microsoft.AspNetCore.Mvc;
using Server.Repositories.ProjectRepositories;
using Server.Service;

namespace Server.Controllers.ProjectController
{
    [ApiController]
    [Route("api/project")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _projectRepo;
        private readonly ProjectCalculationsService _calcService;

        public ProjectController(IProjectRepository projectRepo, ProjectCalculationsService calcService)
        {
            _projectRepo = projectRepo;
            _calcService = calcService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Project>> GetAll()
        {
            return Ok(_projectRepo.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Calculation> GetDetails(int id)
        {
            var result = _calcService.CalculateProject(id);
            if (result == null) return NotFound("Project not found");
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Create(Project pro)
        {
            int newId = _projectRepo.Create(pro);
            return Ok(newId);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Project pro)
        {
            if (id != pro.ProjectId) return BadRequest("ID mismatch");
            _projectRepo.Update(pro);
            return Ok();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _projectRepo.Delete(id);
        }
    }
}