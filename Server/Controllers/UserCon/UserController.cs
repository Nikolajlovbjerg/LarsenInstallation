using Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Repositories.User;
using Server.Service;

namespace ServerApp.Controllers
{
    [ApiController] // Fortæller at denne klasse er en Web API controller
    [Route("api/user")] // Base-URL for alle endpoints i denne controller
    [Authorize(Roles = "Admin")] // Kun Admin-brugere; login + dev-endpoint åbnes med [AllowAnonymous]
    public class UserController : ControllerBase
    {

        private ICreateUserRepo userRepo; // Interface til repository (controlleren afhænger kun af interfacet)
        private readonly JwtTokenService _tokenService; // Laver JWT-token ved login

        public UserController(ICreateUserRepo userRepo, JwtTokenService tokenService)
        {
            this.userRepo = userRepo; // Dependency Injection: Systemet giver controlleren et repository
            _tokenService = tokenService;
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

        [AllowAnonymous] // Login skal kunne kaldes uden token
        [HttpPost("login")] // Endpoint: POST api/user/login
        public ActionResult<LoginResponse> Login([FromBody] Login dto)
        {
            // Validerer brugernavn og password gennem repo
            var user = userRepo.ValidateUser(dto.UserName, dto.Password);

            if (user == null)
                return Unauthorized(); // Returnerer HTTP 401 hvis login mislykkes

            // Udsteder et JWT-token og returnerer det sammen med brugerdata (uden password)
            var token = _tokenService.CreateToken(user);
            return Ok(new LoginResponse { Token = token, User = ToDto(user) });
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
    }
}
