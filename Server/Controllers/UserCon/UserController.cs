using Core;
using Microsoft.AspNetCore.Mvc;
using Server.Repositories.User;

namespace ServerApp.Controllers
{
    [ApiController] // Fortæller at denne klasse er en Web API controller
    [Route("api/user")] // Base-URL for alle endpoints i denne controller
    public class UserController : ControllerBase
    {

        private ICreateUserRepo userRepo; // Interface til repository (controlleren afhænger kun af interfacet)
        private readonly IWebHostEnvironment _env; // Bruges til at begrænse migrations-endpoint til Development

        public UserController(ICreateUserRepo userRepo, IWebHostEnvironment env)
        {
            this.userRepo = userRepo; // Dependency Injection: Systemet giver controlleren et repository
            _env = env;
        }

        // Mapper en intern Users (med password-hash) til en UserDto uden følsomme felter
        private static UserDto ToDto(Users u) => new()
        {
            UserId = u.UserId,
            UserName = u.UserName,
            Role = u.Role
        };

        [HttpGet] // Endpoint: GET api/user
        public IEnumerable<UserDto> Get()
        {
            // Returnerer DTO'er uden password (heller ikke hashet)
            return userRepo.GetAll().Select(ToDto);
        }

        [HttpPost] // Endpoint: POST api/user
        public void Add(Users user)
        {
            userRepo.Add(user); // Kalder repo for at tilføje en ny bruger
        }

        [HttpPost("login")] // Endpoint: POST api/user/login
        public ActionResult<UserDto> Login([FromBody] Login dto)
        {
            // Validerer brugernavn og password gennem repo
            var user = userRepo.ValidateUser(dto.UserName, dto.Password);

            if (user == null)
                return Unauthorized(); // Returnerer HTTP 401 hvis login mislykkes

            return Ok(ToDto(user)); // Returnerer HTTP 200 + brugerdata UDEN password
        }

        [HttpDelete]  // Endpoint: DELETE api/user/delete/{id}
        [Route("delete/{id:int}")] // Route med variabel id, kun int
        public void Delete(int id)
        {
            userRepo.Delete(id); // Sletter bruger via repository
        }
        
        [HttpGet("{id:int}")]
        public ActionResult<UserDto> Get(int id)
        {
            var user = userRepo.GetById(id);
            if (user == null) return NotFound();
            return Ok(ToDto(user)); // Uden password
        }


        [HttpPut]
        public IActionResult Update([FromBody] Users user)
        {
            userRepo.Update(user);
            return Ok();
        }

        // Engangs-migration af gamle klartekst-passwords til hash.
        // KUN tilgængelig i Development, så den ikke kan misbruges i produktion.
        // Slet dette endpoint når migrationen er kørt.
        [HttpPost("dev/rehash-passwords")]
        public IActionResult RehashPasswords()
        {
            if (!_env.IsDevelopment())
                return NotFound();

            int updated = userRepo.RehashLegacyPasswords();
            return Ok($"Rehashed {updated} user(s).");
        }
    }
}
