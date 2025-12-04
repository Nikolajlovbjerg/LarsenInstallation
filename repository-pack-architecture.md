This file is a merged representation of a subset of the codebase, containing specifically included files and files not matching ignore patterns, combined into a single document by Repomix.
The content has been processed where empty lines have been removed.

<file_summary>
This section contains a summary of this file.

<purpose>
This file contains a packed representation of a subset of the repository's contents that is considered the most important context.
It is designed to be easily consumable by AI systems for analysis, code review,
or other automated processes.
</purpose>

<file_format>
The content is organized as follows:
1. This summary section
2. Repository information
3. Directory structure
4. Repository files (if enabled)
5. Multiple file entries, each consisting of:
  - File path as an attribute
  - Full contents of the file
</file_format>

<usage_guidelines>
- This file should be treated as read-only. Any changes should be made to the
  original repository files, not this packed version.
- When processing this file, use the file path to distinguish
  between different files in the repository.
- Be aware that this file may contain sensitive information. Handle it with
  the same level of security as you would the original repository.
</usage_guidelines>

<notes>
- Some files may have been excluded based on .gitignore rules and Repomix's configuration
- Binary files are not included in this packed representation. Please refer to the Repository Structure section for a complete list of file paths, including binary files
- Only files matching these patterns are included: Client/App.razor, Client/_Imports.razor, Client/Program.cs, Client/Components/**/*.razor, Client/Layout/**/*.razor, Client/Pages/**/*.razor, Client/Service/**/*.cs, Client/Services/**/*.cs, Core/**/*.cs, Server/Controllers/**/*.cs, Server/Repositories/**/*.cs, Server/Service/**/*.cs, Server/Data/**/*.cs, Server/Program.cs, Server/appsettings*.json
- Files matching these patterns are excluded: **/bin/**, **/obj/**, **/*.csproj, **/*.sln, **/*.css, **/*.map, **/*.min.js, **/wwwroot/**, ServerApp/ServerApp.http
- Files matching patterns in .gitignore are excluded
- Files matching default ignore patterns are excluded
- Empty lines have been removed from all files
- Files are sorted by Git change count (files with more changes are at the bottom)
</notes>

</file_summary>

<directory_structure>
Client/
  Components/
    CreateProjectComponent.razor
    ProjectComponent.razor
    ProjectGrid.razor
  Layout/
    MainLayout.razor
    NavMenu.razor
  Pages/
    AddUser.razor
    CreateProjectPage.razor
    Home.razor
    LoginPage.razor
    ProjectDetailsPage.razor
    ProjectPage.razor
  Service/
    UserRepository.cs
  _Imports.razor
  App.razor
  Program.cs
Core/
  Calculation.cs
  Login.cs
  Project.cs
  ProjectHours.cs
  ProjectMaterial.cs
  User.cs
Server/
  Controllers/
    UserCon/
      UserController.cs
    CreateProjectController.cs
    MaterialUploadController.cs
    UploadController.cs
  Repositories/
    ExcelRepos/
      ExcelRepo.cs
      IExcelRepo.cs
      IMaterialExcelRepo.cs
      MaterialExcelRepo.cs
    User/
      CreateUserRepoSQL.cs
      ICreateUserRepoSQL.cs
    CreateProjectRepo.cs
    ICreateProjectRepo.cs
  Service/
    MaterialConverter.cs
    WorkerConverter.cs
  appsettings.Development.json
  appsettings.json
  Program.cs
</directory_structure>

<files>
This section contains the contents of the repository's files.

<file path="Client/Components/ProjectComponent.razor">
@using Core
@inject NavigationManager Nav



<article class="ad-card">
<div class="ad-media">
    <img src="@Project.ImageUrl" alt="Project billede" />
</div>

<div class="ad-body">
    <div class="ad-meta">@Project.DateCreated</div>
    <div class="ad-title">@Project.Name</div>
</div>

<div class="ad-footer">
    <strong><button class="MoreBtn" @onclick="() => GotoDetailPage()" >Se mere</button></strong>
</div>
</article>
@code {
    [Parameter] public Project Project { get; set; }


    private Task GotoDetailPage()
    {
        var id = Project.ProjectId;
        Nav.NavigateTo($"project/{id}");
        return Task.CompletedTask;
    }

}
</file>

<file path="Client/Components/ProjectGrid.razor">
@using Core


<div class="ads-grid">
    @if (Projects == null || !Projects.Any())
    {
        <p>Ingen projekter at vise.</p>
    }
    else
    {
        @foreach (var a in Projects)
        {
            <ProjectComponent Project="a" />
        }
    }
</div>

@code 
{
    [Parameter] public IEnumerable<Project> Projects { get; set; }
}
</file>

<file path="Client/Pages/ProjectPage.razor">
@page "/projectpage"
@using Client.Components
@using Core
@inject HttpClient Http
<h3>ProjectPage</h3>


<section>
    @if (_projects.Count == 0)
    {
        <p>Ingen produkter</p>
    }
    else
    {
        <ProjectGrid Projects="_projects" />
    }
      
   
</section>


@code {
    private List<Project>? _projects = new();
    
    
    protected override async Task OnInitializedAsync()
    {
        _projects = await Http.GetFromJsonAsync<List<Project>>("http://localhost:5028/api/createproject");
    }
}
</file>

<file path="Client/Pages/CreateProjectPage.razor">
@page "/CreateProjectPage"
@using Client.Components


<CreateProjectComponent></CreateProjectComponent>

@code {
    
}
</file>

<file path="Client/_Imports.razor">
@using System.Net.Http
@using System.Net.Http.Json
@using Microsoft.AspNetCore.Components.Forms
@using Microsoft.AspNetCore.Components.Routing
@using Microsoft.AspNetCore.Components.Web
@using Microsoft.AspNetCore.Components.Web.Virtualization
@using Microsoft.AspNetCore.Components.WebAssembly.Http
@using Microsoft.JSInterop
@using Client
@using Client.Layout
</file>

<file path="Server/Service/MaterialConverter.cs">
using System.IO;
using System.Text;
using Core;
using ExcelDataReader;
namespace Server.Service
{
    public class MaterialConverter
    {
        public static List<ProjectMaterial> Convert(Stream stream)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            var ds = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = false
                }
            });
            List<ProjectMaterial> result = new();
            int row_no = 1;
            while (row_no < ds.Tables[0].Rows.Count)
            {
                var row = ds.Tables[0].Rows[row_no];
                ProjectMaterial p = new ProjectMaterial();
                p.Beskrivelse = row[1].ToString();
                p.Kostpris = Decimal.Parse(row[2].ToString());
                p.Antal = Decimal.Parse(row[4].ToString());
                p.Total = Decimal.Parse(row[17].ToString());
                p.Avance = decimal.Parse(row[19].ToString());
                p.Dækningsgrad = Decimal.Parse(row[20].ToString());
                // other fields here...
                result.Add(p);
                Console.WriteLine($"Beskrivelse= {p.Beskrivelse}, Kostpris = {p.Kostpris} Antal = {p.Antal}, Total = {p.Total}  Avance = {p.Avance}, dækningsgrad = {p.Dækningsgrad}");
                row_no++;
            }
            return result;
        }
    }
}
</file>

<file path="Server/Service/WorkerConverter.cs">
using System.IO;
using System.Text;
using Core;
using ExcelDataReader;
namespace Server.Service
{
    public class WorkerConverter
    {
        public static List<ProjectHour> Convert(Stream stream)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            IExcelDataReader reader =  ExcelReaderFactory.CreateOpenXmlReader(stream);
            var ds = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = false
                }
            });
            List<ProjectHour> result = new();
            int row_no = 1;
            while (row_no < ds.Tables[0].Rows.Count)
            {
                var row = ds.Tables[0].Rows[row_no];
                ProjectHour p = new ProjectHour();
                p.Dato = DateTime.Parse(row[1].ToString());
                p.Stoptid = DateTime.Parse(row[2].ToString());
                p.Timer = Decimal.Parse(row[5].ToString());
                p.Type = row[6].ToString();
                p.Kostpris = Decimal.Parse(row[8].ToString());
                // other fields here...
                result.Add(p);
                Console.WriteLine($"dato= {p.Dato}, stop tid = {p.Stoptid} Timer = {p.Timer}, Type = {p.Type}  kostpris = {p.Kostpris}");
                row_no++;
            }
            return result;
        }
    }
}
</file>

<file path="Server/appsettings.Development.json">
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
</file>

<file path="Server/appsettings.json">
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
</file>

<file path="Client/Pages/Home.razor">
@page "/"

<PageTitle>Home</PageTitle>

<h1>Velkommen til Larsen Installation</h1>
</file>

<file path="Client/Pages/ProjectDetailsPage.razor">
@page "/project/{Id:int}"
@using Core
@inject HttpClient Http

<PageTitle>Projekt Detaljer</PageTitle>

@if (details == null)
{
    <p><em>Indlæser beregninger...</em></p>
}
else
{
    <div class="container">
        <div class="d-flex justify-content-between align-items-center mb-4">
            <h1>@details.Project.Name</h1>
            <span class="badge bg-secondary">Oprettet: @details.Project.DateCreated.ToShortDateString()</span>
        </div>

        <div class="row mb-4">
            <div class="col-md-4">
                <div class="card text-white bg-success mb-3">
                    <div class="card-header">Samlet Salgspris</div>
                    <div class="card-body">
                        <h3 class="card-title">@details.SamletTotalPris.ToString("N2") kr.</h3>
                        <p class="card-text">
                            Materialer: @details.TotalPrisMaterialer.ToString("N0")<br/>
                            Arbejdsløn: @details.TotalPrisTimer.ToString("N0")
                        </p>
                    </div>
                </div>
            </div>

            <div class="col-md-4">
                <div class="card text-dark bg-light mb-3">
                    <div class="card-header">Samlet Kostpris</div>
                    <div class="card-body">
                        <h3 class="card-title">@details.SamletKostPris.ToString("N2") kr.</h3>
                        <p class="card-text">
                            Materialer: @details.TotalKostPrisMaterialer.ToString("N0")<br/>
                            Lønudgift: @details.TotalKostPrisTimer.ToString("N0")
                        </p>
                    </div>
                </div>
            </div>

            <div class="col-md-4">
                <div class="card text-white bg-primary mb-3">
                    <div class="card-header">Dækningsbidrag</div>
                    <div class="card-body">
                        <h3 class="card-title">@details.Dækningsbidrag.ToString("N2") kr.</h3>
                        <p class="card-text">
                            Dækningsgrad: <strong>@details.Dækningsgrad.ToString("N1") %</strong>
                        </p>
                    </div>
                </div>
            </div>
        </div>

        <h3>Timeregistreringer (@details.TotalTimer timer)</h3>
        <table class="table table-striped">
            <thead>
                <tr>
                    <th>Dato</th>
                    <th>Type</th>
                    <th>Timer</th>
                    <th>Kostpris</th>
                </tr>
            </thead>
            <tbody>
                @foreach (var h in details.Hours)
                {
                    <tr>
                        <td>@(h.Dato?.ToShortDateString() ?? "-")</td>
                        <td>@h.Type</td>
                        <td>@h.Timer</td>
                        <td>@h.Kostpris</td>
                    </tr>
                }
            </tbody>
        </table>
    </div>
}

@code {
    [Parameter]
    public int id { get; set; }

    private Calculation? details;

    protected override async Task OnInitializedAsync()
    {
        details = await Http.GetFromJsonAsync<Calculation>($"api/createproject/{id}");
    }
}
</file>

<file path="Client/App.razor">
@inject Blazored.LocalStorage.ILocalStorageService LocalStorage
@inject NavigationManager Nav
@using Client.Pages

@if (!initialized)
{
    <!-- small placeholder while we read local storage -->
    <div></div>
}
else if (!isLoggedIn)
{
    <!-- Render the LoginPage component directly if not logged in -->
    <LoginPage/>
}
else
{
    <Router AppAssembly="@typeof(App).Assembly">
        <Found Context="routeData">
            <RouteView RouteData="@routeData" DefaultLayout="@typeof(Client.Layout.MainLayout)" />
            <FocusOnNavigate RouteData="@routeData" Selector="h1" />
        </Found>
        <NotFound>
            <LayoutView Layout="@typeof(Client.Layout.MainLayout)">
                <p>Page not found.</p>
            </LayoutView>
        </NotFound>
    </Router>
}

@code
{
    private bool initialized = false;
    private bool isLoggedIn = false;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var user = await LocalStorage.GetItemAsync<object>("user");
            isLoggedIn = user != null;
        }
        catch (Exception)
        {
            // If localStorage read fails, treat as not logged in
            isLoggedIn = false;
        }
        initialized = true;
    }
}
</file>

<file path="Core/Login.cs">
namespace Core;
public class Login
{
    public string UserName { get; set; }
    public string Password { get; set; }
}
</file>

<file path="Core/Project.cs">
namespace Core;
public class Project
{
    public int ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public string ImageUrl { get; set; } = string.Empty;
    public int SvendTimePris { get; set; }
    public int LærlingTimePris { get; set; }
    public int KonsulentTimePris { get; set; }
    public int ArbjedsmandTimePris { get; set; }
}
</file>

<file path="Server/Controllers/UserCon/UserController.cs">
using Core;
using Microsoft.AspNetCore.Mvc;
using Server.Repositories.User;
namespace ServerApp.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private ICreateUserRepoSQL userRepo;
        public UserController(ICreateUserRepoSQL userRepo)
        {
            this.userRepo = userRepo;
        }
        [HttpGet]
        public IEnumerable<Users> Get()
        {
            return userRepo.GetAll();
        }
        [HttpPost]
        public void Add(Users user)
        {
            userRepo.Add(user);
        }
        [HttpPost("login")]
        public ActionResult<Users> Login([FromBody] Login dto)
        {
            var user = userRepo.ValidateUser(dto.UserName, dto.Password); // vi tilføjer denne metode til repo
            if (user == null)
                return Unauthorized(); // 401
            return Ok(user);
        }
        [HttpDelete]
        [Route("delete/{id:int}")]
        public void Delete(int id)
        {
            userRepo.Delete(id);
        }
        [HttpDelete]
        [Route("delete")]
        public void DeleteByQuery([FromQuery] int id)
        {
            userRepo.Delete(id);
        }
    }
}
</file>

<file path="Server/Repositories/ExcelRepos/IExcelRepo.cs">
/*
using Core;
namespace Server.Repositories.ExcelRepos
{
    public interface IExcelRepo
    {
        void Add(ProjectHour proj);
        Calculation GetHoursDetails();
    }
}
*/
</file>

<file path="Server/Repositories/ExcelRepos/IMaterialExcelRepo.cs">
/*using Core;
namespace Server.Repositories.ExcelRepos
{
    public interface IMaterialExcelRepo
    {
        void Add(ProjectMaterial projmat);
    }
}*/
</file>

<file path="Server/Repositories/User/CreateUserRepoSQL.cs">
using Core;
using Npgsql;
using Server.PW1;
namespace Server.Repositories.User
{
    public class CreateUserRepoSQL : ICreateUserRepoSQL
    {
        private const string conString =
    "Host=ep-spring-unit-a2y1k0pd.eu-central-1.aws.neon.tech;" +
    "Port=5432;" +
    "Database=LarsenInstallation;" +
    "Username=neondb_owner;" +
    $"Password={PASSWORD.PW1};" +
    "Ssl Mode=Require;" +
    "Trust Server Certificate=true;";
        public CreateUserRepoSQL()
        {
        }
        public List<Users> GetAll()
        {
            var result = new List<Users>();
            using (var mConnection = new NpgsqlConnection(conString))
            {
                mConnection.Open();
                var command = mConnection.CreateCommand();
                command.CommandText = @"SELECT * FROM Users";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var userid = reader.GetInt32(0);
                        var username = reader.GetString(1);
                        var password = reader.GetString(2);
                        var role = reader.GetString(3);
                        Users u = new Users
                        {
                            UserId = userid,
                            UserName = username,
                            Password = password,
                            Role = role
                        };
                        result.Add(u);
                    }
                }
            }
            return result;
        }
        public void Add(Users user)
        {
            using (var mConnection = new NpgsqlConnection(conString))
            {
                mConnection.Open();
                var command = mConnection.CreateCommand();
                command.CommandText =
                    "INSERT INTO Users (username, password, role) VALUES (@username, @password, @role)";
                Console.WriteLine(command.CommandText);
                var paramName = command.CreateParameter();
                paramName.ParameterName = "username";
                command.Parameters.Add(paramName);
                paramName.Value = user.UserName;
                var paramPassword = command.CreateParameter();
                paramPassword.ParameterName = "password";
                command.Parameters.Add(paramPassword);
                paramPassword.Value = user.Password;
                var paramRole = command.CreateParameter();
                paramRole.ParameterName = "role";
                command.Parameters.Add(paramRole);
                paramRole.Value = user.Role;
                command.ExecuteNonQuery();
            }
        }
        public Users? ValidateUser(string username, string password)
        {
            return GetAll().FirstOrDefault(u => u.UserName == username && u.Password == password);
        }
        public void Delete(int id)
        {
            using (var mConnection = new NpgsqlConnection(conString))
            {
                mConnection.Open();
                var command = mConnection.CreateCommand();
                command.CommandText = $"DELETE FROM users WHERE userid={id}";
                command.ExecuteNonQuery();
            }
        }
    }
}
</file>

<file path="Server/Repositories/User/ICreateUserRepoSQL.cs">
using Core;
using System.Collections.Generic;
namespace Server.Repositories.User
{
    public interface ICreateUserRepoSQL
    {
        List<Users> GetAll();
        void Add(Users user);
        void Delete(int id);
        Users? ValidateUser(string username, string password);
    }
}
</file>

<file path="Client/Service/UserRepository.cs">
using Core;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
namespace Client.Service
{
    public class UserRepository
    {
        private readonly HttpClient _http;
        public UserRepository(HttpClient http)
        {
            _http = http;
        }
        public async Task<Users?> ValidLoginAsync(string name, string password)
        {
            var login = new { UserName = name, Password = password };
            HttpResponseMessage response;
            try
            {
                response = await _http.PostAsJsonAsync("/api/user/login", login);
            }
            catch (Exception)
            {
                return null;
            }
            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadFromJsonAsync<Users>();
                return user;
            }
            return null;
        }
    }
}
</file>

<file path="Core/ProjectHours.cs">
namespace Core
{
    public class ProjectHour
    {
        public int HourId { get; set; }
        public int ProjectId { get; set; }
        public string? Medarbejder { get; set; }
        public DateTime? Dato { get; set; }
        public DateTime? Stoptid { get; set; }
        public decimal Timer { get; set; }
        public string? Type { get; set; }
        public decimal Kostpris { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
</file>

<file path="Core/ProjectMaterial.cs">
namespace Core
{
    public class ProjectMaterial
    {
        public int MaterialsId { get; set; }
        public int ProjectId { get; set; }
        public string? Beskrivelse { get; set; }
        public decimal Kostpris { get; set; }
        public decimal Antal { get; set; }
        public decimal Total { get; set; }
        public decimal Avance { get; set; }
        public decimal Dækningsgrad { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
</file>

<file path="Server/Controllers/MaterialUploadController.cs">
/*
using Core;
using Microsoft.AspNetCore.Mvc;
using Server.Repositories;
using Server.Repositories.ExcelRepos;
using Server.Service;
namespace Server.Controllers
{
    [ApiController]
    [Route("api/materialuploadexcel")]
    public class MaterialUploadController : ControllerBase
    {
        private readonly ICreateProjectRepo MatExRepo;
        public MaterialUploadController(ICreateProjectRepo MatExRepo)
        {
            this.MatExRepo = MatExRepo;
        }
        [HttpPost]
        public IActionResult UploadMaterial(IFormFile? file, int projectId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");
            else if (file.FileName.StartsWith("Mater"))
            {
                Stream s = new MemoryStream();
                file.CopyTo(s);
                s.Position = 0;
                List<ProjectMaterial> res = MaterialConverter.Convert(s);
                foreach (var row in res)
                {
                    row.ProjectId = projectId;
                    MatExRepo.Add(row);
                }
                return Ok("Material uploaded" + projectId);
            }
            return Ok();
        }
    }
}
*/
</file>

<file path="Server/Controllers/UploadController.cs">
/*
using Core;
using Microsoft.AspNetCore.Mvc;
using Server.Repositories;
using Server.Repositories.ExcelRepos;
using Server.Service;
namespace Server.Controllers
{
    [ApiController]
    [Route("api/uploadexcel")]
    public class UploadController : ControllerBase
    {
        private readonly ICreateProjectRepo exRepo;
        public UploadController(ICreateProjectRepo exRepo)
        {
            this.exRepo = exRepo;
        }
        [HttpPost]
        public IActionResult UploadHour(IFormFile? file, int projectId) 
            //ProjectId parameteren er der for at vi kan modtage id udefra
        {
            if (file == null || file.Length == 0) 
                return BadRequest("No file uploaded");
            else if (file.FileName.StartsWith("Work"))
            {
                    Stream s = new MemoryStream();
                    file.CopyTo(s);
                    s.Position = 0; //Går tilbage til starten så vi kan læse dataen. Fordi efter filen er indlæst vil den være i sidste kolonne
                    List<ProjectHour> res = WorkerConverter.Convert(s);
                foreach (var row1 in res)
                {
                    row1.ProjectId = projectId; //Her for id værdien
                    exRepo.Add(row1);
                }
                return Ok("worker uploaded" + projectId);
            }
            return Ok();
        }
        [HttpGet("{id}")]
        public ActionResult<Calculation> GetProjectDetails(int id)
        {
            try
            {
                var result = exRepo.GetProjectDetails(id);
                if (result == null) return NotFound("Project not found");
                {
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500,  "Error: " + ex.Message);
            }
        }
    }
}
*/
</file>

<file path="Server/Repositories/ExcelRepos/MaterialExcelRepo.cs">
/*
using Core;
using Npgsql;
using Server.PW1;
using Server.Repositories.ExcelRepos;
using Server.Repositories;
namespace Server.Repositories.User
{
    public class MaterialExcelRepo : IMaterialExcelRepo
    {
        private const string conString =
    "Host=ep-spring-unit-a2y1k0pd.eu-central-1.aws.neon.tech;" +
    "Port=5432;" +
    "Database=LarsenInstallation;" +
    "Username=neondb_owner;" +
    $"Password={PASSWORD.PW1};" +
    "Ssl Mode=Require;" +
    "Trust Server Certificate=true;";
        public MaterialExcelRepo()
        {
        }
        public void Add(ProjectMaterial projmat)
        {
            var result = new List<ProjectMaterial>();
            using (var mConnection = new NpgsqlConnection(conString))
            {
                mConnection.Open();
                var command = mConnection.CreateCommand();
                command.CommandText = @"INSERT INTO projectmaterials
                    (projectid, beskrivelse, kostpris, antal, total, avance, dækningsgrad) 
                    VALUES (@projectid, @beskrivelse, @kostpris, @antal, @total, @avance, @dækningsgrad)";
                var paramProjId = command.CreateParameter();
                paramProjId.ParameterName = "projectid";
                command.Parameters.Add(paramProjId);
                paramProjId.Value = projmat.ProjectId;
                Console.WriteLine(command.CommandText);
                var paramBeskriv = command.CreateParameter();
                paramBeskriv.ParameterName = "beskrivelse";
                command.Parameters.Add(paramBeskriv);
                paramBeskriv.Value = projmat.Beskrivelse;
                var paramKost = command.CreateParameter();
                paramKost.ParameterName = "kostpris";
                command.Parameters.Add(paramKost);
                paramKost.Value = projmat.Kostpris;
                var paramAntal = command.CreateParameter();
                paramAntal.ParameterName = "antal";
                command.Parameters.Add(paramAntal);
                paramAntal.Value = projmat.Antal;
                var paramTotal = command.CreateParameter();
                paramTotal.ParameterName = "total";
                command.Parameters.Add(paramTotal);
                paramTotal.Value = projmat.Total;
                var PriceAvance = command.CreateParameter();
                PriceAvance.ParameterName = "avance";
                command.Parameters.Add(PriceAvance);
                PriceAvance.Value = projmat.Avance;
                var paramDaek = command.CreateParameter();
                paramDaek.ParameterName = "dækningsgrad";
                command.Parameters.Add(paramDaek);
                paramDaek.Value = projmat.Dækningsgrad;
                command.ExecuteNonQuery();
            }
        }
    }
}
*/
</file>

<file path="Client/Pages/LoginPage.razor">
@using Core
@using Blazored.LocalStorage
@using Client.Service
@inject ILocalStorageService LocalStorage
@inject NavigationManager Nav
@inject Client.Service.UserRepository UserRepo
@page "/login"
<PageTitle>Log ind</PageTitle>

<div class="login-page-back">
    <div class="login-con">
        <h3>Log ind</h3>

        <EditForm Model="@user" OnValidSubmit="OnClickLogin">
            <div class="form-floating-group">
                <InputText id="un" class="form-control" @bind-Value="user.UserName" placeholder="Username" />
                <label for="un">Username</label>
            </div>

            <div class="form-floating-group">
                <InputText id="pwd"
                           type="@PasswordType"
                           class="form-control"
                           @bind-Value="user.Password"
                           placeholder="Password" />
                <label for="pwd">Password</label>

                <!-- Brug en button type="button" så vi ikke utilsigtet submitter formen -->
                <button type="button" class="btn btn-link p-0" @onclick="TogglePasswordVisibility" aria-label="Toggle password visibility">
                    <i class="bi @(visPassword ? "bi-eye-slash" : "bi-eye")"></i>
                </button>
            </div>

            <div style="text-align: center">
                <button type="submit" class="btn btn-primary" disabled="@isLoading">
                    @if (isLoading)
                    {
                        <span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>
                        <span class="visually-hidden"> Logging in...</span>
                    }
                    else
                    {
                        <span>Login</span>
                    }
                </button>
            </div>
        </EditForm>

        @if (!string.IsNullOrEmpty(errorText))
        {
            <div class="error-message">
                <span class="error-icon">⚠️</span>
                <span class="error-text">@errorText</span>
            </div>
        }
    </div>
</div>

@code {
    Users user = new();
    string errorText = "";
    bool visPassword = false;
    bool isLoading = false;

    string PasswordType => visPassword ? "text" : "password";

    void TogglePasswordVisibility()
    {
        visPassword = !visPassword;
    }

    private async Task OnClickLogin()
    {
        errorText = "";
        isLoading = true;

        try
        {
            // Kald den asynkrone metode i din UserRepository (som bruger HttpClient)
            Users? userObject = await UserRepo.ValidLoginAsync(user.UserName, user.Password);

            if (userObject == null)
            {
                errorText = "Forkert brugernavn eller adgangskode — prøv igen.";
                isLoading = false;
                return;
            }

            // Fjern adgangskoden før vi gemmer i localStorage (GEM ALDRIG password i klartekst)
            try
            {
                userObject.Password = string.Empty; // eller null afhængig af modellen
            }
            catch
            {
                // Hvis Users.Password er readonly eller lign., lav et lille DTO i stedet:
                var safeUser = new { userObject.UserName, userObject.Role, /* evt. Id = userObject.Id */ };
                await LocalStorage.SetItemAsync("user", safeUser);
                Nav.NavigateTo("projectpage", forceLoad: true);
                return;
            }

            await LocalStorage.SetItemAsync("user", userObject);
            Nav.NavigateTo("projectpage", forceLoad: true);
        }
        catch (HttpRequestException)
        {
            errorText = "Netværksfejl — kunne ikke kontakte serveren. Tjek at serveren kører og CORS er korrekt sat op.";
        }
        catch (Exception ex)
        {
            errorText = $"Der skete en fejl: {ex.Message}";
        }
        finally
        {
            isLoading = false;
        }
    }
}
</file>

<file path="Core/Calculation.cs">
namespace Core
{
    public class Calculation
    {
        // Stamdata
        public Project Project { get; set; }
        // Lister af data
        public List<ProjectHour> Hours { get; set; } = new();
        public List<ProjectMaterial> Materials { get; set; } = new();
        // Bregninger
        public decimal TotalKostPrisMaterialer { get; set; }
        public decimal TotalPrisMaterialer { get; set; }
        public decimal TotalTimer { get; set; }
        public decimal TotalKostPrisTimer { get; set; }
        public decimal TotalPrisTimer { get; set; }
        // Samlet
        public decimal SamletKostPris => TotalKostPrisMaterialer + TotalKostPrisTimer;
        public decimal SamletTotalPris => TotalPrisMaterialer + TotalPrisTimer;
        public decimal Dækningsgrad => SamletTotalPris > 0 ? (Dækningsbidrag/SamletTotalPris) * 100 : 0;
        public decimal Dækningsbidrag => SamletTotalPris - SamletKostPris;
    }
}
</file>

<file path="Server/Controllers/CreateProjectController.cs">
using Core;
using Microsoft.AspNetCore.Mvc;
using Server.Repositories;
using Server.Service;
using System.IO;
namespace Server.Controllers
{
    [ApiController]
    [Route("api/createproject")]
    public class CreateProjectController : ControllerBase
    {
        private ICreateProjectRepo crProj;
        public CreateProjectController(ICreateProjectRepo crProj)
        {
            this.crProj = crProj;
        }
        [HttpGet] 
        public ActionResult<IEnumerable<Project>> GetAllProjects()
        {
            var projects = crProj.GetAllProjects();
            return Ok(projects);
        }
        [HttpPost]
        public IActionResult Add(Project pro) //Fleksibel pakke. Bruges når man for en masse forskellige slags data.
        //Det er et interface der giver dig lov til at retunere hvad som helst så længe der er et gyldigt http svar
        {
            int newProjectId = crProj.Add(pro);
            return Ok(newProjectId); //Ok er en hjælpe metode der fortæller klienten det lykkedes og giver svaret
        }
        [HttpGet("{id}")]
        public ActionResult<Calculation> GetProjectDetails(int id)
        {
            try
            {
                var result = crProj.GetProjectDetails(id);
                if (result == null) return NotFound("Project not found");
                {
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500,  "Error: " + ex.Message);
            }
        }
        [HttpPost("uploadhours")]
        public IActionResult UploadHours(IFormFile? file, int projectId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");
            // Tjekker om filnavnet starter med "Work" (som i den gamle controller)
            if (file.FileName.StartsWith("Work"))
            {
                using Stream s = new MemoryStream();
                file.CopyTo(s);
                s.Position = 0;
                List<ProjectHour> res = WorkerConverter.Convert(s);
                foreach (var row in res)
                {
                    row.ProjectId = projectId;
                    crProj.AddHour(row); // Bruger AddHour fra interfacet
                }
                return Ok("Worker hours uploaded for project " + projectId);
            }
            return BadRequest("Invalid file name or format");
        }
        [HttpPost("uploadmaterials")]
        public IActionResult UploadMaterials(IFormFile? file, int projectId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");
            // Tjekker om filnavnet starter med "Mater" (som i den gamle controller)
            if (file.FileName.StartsWith("Mater"))
            {
                using Stream s = new MemoryStream();
                file.CopyTo(s);
                s.Position = 0;
                List<ProjectMaterial> res = MaterialConverter.Convert(s);
                foreach (var row in res)
                {
                    row.ProjectId = projectId;
                    crProj.AddMaterials(row); // Bruger AddMaterials fra interfacet
                }
                return Ok("Materials uploaded for project " + projectId);
            }
            return BadRequest("Invalid file name or format");
        }
    }
}
</file>

<file path="Server/Repositories/ExcelRepos/ExcelRepo.cs">
/*
using Core;
using Npgsql;
using Server.PW1;
using Server.Repositories.ExcelRepos;
using Server.Repositories;
namespace Server.Repositories.User
{
    public class ExcelRepo : IExcelRepo
    {
        private const string conString =
    "Host=ep-spring-unit-a2y1k0pd.eu-central-1.aws.neon.tech;" +
    "Port=5432;" +
    "Database=LarsenInstallation;" +
    "Username=neondb_owner;" +
    $"Password={PASSWORD.PW1};" +
    "Ssl Mode=Require;" +
    "Trust Server Certificate=true;";
        public ExcelRepo()
        {
        }
        public void AddHour(ProjectHour proj)
        {
            var result = new List<ProjectHour>();
            using (var mConnection = new NpgsqlConnection(conString))
            {
                mConnection.Open();
                var command = mConnection.CreateCommand();
                command.CommandText = @"INSERT INTO projecthours
                    (projectid, dato, stoptid, timer, type, kostpris) 
                    VALUES (@projectid, @dato, @stoptid, @timer, @type, @kostpris)";
                var paramProjId = command.CreateParameter();
                paramProjId.ParameterName = "projectid";
                command.Parameters.Add(paramProjId);
                paramProjId.Value = proj.ProjectId;
                Console.WriteLine(command.CommandText);
                var paramDato = command.CreateParameter();
                paramDato.ParameterName = "dato";
                command.Parameters.Add(paramDato);
                paramDato.Value = proj.Dato;
                var paramStop = command.CreateParameter();
                paramStop.ParameterName = "stoptid";
                command.Parameters.Add(paramStop);
                paramStop.Value = proj.Stoptid;
                var paramTimer = command.CreateParameter();
                paramTimer.ParameterName = "timer";
                command.Parameters.Add(paramTimer);
                paramTimer.Value = proj.Timer;
                var paramType = command.CreateParameter();
                paramType.ParameterName = "type";
                command.Parameters.Add(paramType);
                paramType.Value = proj.Type;
                var paramKost = command.CreateParameter();
                paramKost.ParameterName = "kostpris";
                command.Parameters.Add(paramKost);
                paramKost.Value = proj.Kostpris;
                command.ExecuteNonQuery();
            }
        }
    }
}
*/
</file>

<file path="Server/Repositories/ICreateProjectRepo.cs">
using Core;
namespace Server.Repositories
{
    public interface ICreateProjectRepo
    {
        int Add(Project pro);
        void AddHour(ProjectHour proj);
        void AddMaterials(ProjectMaterial projmat);
        Calculation? GetProjectDetails(int projectId);
        IEnumerable<Project> GetAllProjects();
    }
}
</file>

<file path="Client/Program.cs">
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client;
using Blazored.LocalStorage;
using Client.Service;
using Microsoft.Extensions.DependencyInjection;
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5028/") });
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<UserRepository>();
await builder.Build().RunAsync();
</file>

<file path="Core/User.cs">
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Core;
public class Users
{
    public int UserId { get; set; }
    public string UserName { get; set; } = String.Empty;
    public string Password { get; set; } = String.Empty;
    public string Role { get; set; } = "none";
}
</file>

<file path="Client/Layout/MainLayout.razor">
@inject Blazored.LocalStorage.ILocalStorageService LocalStorage
@using Core
@inject NavigationManager navManager
@inherits LayoutComponentBase
<div class="page">
    @if (isLoggedIn)
    {
        <div class="sidebar">
            <NavMenu/>
        </div>

        <main>
            <div class="top-row px-2 btn-primary" style="color: white">
            <NavLink class="nav-link" href="logout" @onclick="Logout">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-door-closed" viewBox="0 0 16 16">
                    <path d="M3 2a1 1 0 0 1 1-1h8a1 1 0 0 1 1 1v13h1.5a.5.5 0 0 1 0 1h-13a.5.5 0 0 1 0-1H3zm1 13h8V2H4z"/>
                    <path d="M9 9a1 1 0 1 0 2 0 1 1 0 0 0-2 0"/>
                </svg> Log out
            </NavLink>
            </div>
            <article class="content px-4">
                @Body
            </article>
        </main>
    }
</div>

@code
{
    bool isLoggedIn = false;

    protected override async Task OnInitializedAsync()
    {
        var user = await LocalStorage.GetItemAsync<Users>("user");
        isLoggedIn = user != null;
    }
    
    private async Task Logout()
    { 
        await LocalStorage.RemoveItemAsync("user");
        navManager.NavigateTo("/", true);
    }
}
</file>

<file path="Client/Pages/AddUser.razor">
@page "/adduser"
@using Core
@inject HttpClient http
@inject NavigationManager navMan

<PageTitle>Add User</PageTitle>

<h3>Add User</h3>

<EditForm Model="@_user" OnValidSubmit="@HandleValidSubmit" class="row p-3">
    <DataAnnotationsValidator />
    <ValidationSummary />

    <div class="col-md-12 mb-3">
        <label for="UserName">User Name</label>
        <InputText id="UserName" @bind-Value="_user.UserName" class="form-control" />
    </div>

    <div class="col-md-12 mb-3">
        <label for="Password">Password</label>
        <InputText id="Password" @bind-Value="_user.Password" class="form-control" type="password" />
    </div>

    <div class="col-md-12 mb-3">
        <label for="Role">Role</label>
        <InputSelect id="Role" @bind-Value="_user.Role" class="form-select" >

            @foreach (var r in roles)
            {
                <option value="@r">@r</option>
            }
        </InputSelect>
    </div>

    <div class="col-12 mb-3">
        <button type="submit" class="btn btn-primary">Add User</button>
    </div>
</EditForm>

@code {
    private Users _user = new();

    private string[] roles = ["Bruger", "Admin"];

    private async Task HandleValidSubmit()
    {
        // Sender POST request direkte til API'et
        var response = await http.PostAsJsonAsync("http://localhost:5028/api/user", _user);

        if (response.IsSuccessStatusCode)
        {
            // Clear form
            _user = new Users();
            // Naviger tilbage til brugersiden (juster route som ønsket)
            navMan.NavigateTo("/");
        }
    }
}
</file>

<file path="Server/Repositories/CreateProjectRepo.cs">
using Core;
using Npgsql;
using Server.PW1;
using Server.Repositories;
namespace Server.Repositories
{
    public class CreateProjectRepo : ICreateProjectRepo
    {
        private const string conString =
            "Host=ep-spring-unit-a2y1k0pd.eu-central-1.aws.neon.tech;" +
            "Port=5432;" +
            "Database=LarsenInstallation;" +
            "Username=neondb_owner;" +
            $"Password={PASSWORD.PW1};" +
            "Ssl Mode=Require;" +
            "Trust Server Certificate=true;";
        public int Add(Project pro) //Er en int fordi vi retunere et int
        {
            var result = new List<Project>();
            using (var mConnection = new NpgsqlConnection(conString))
            {
                mConnection.Open();
                var command = mConnection.CreateCommand();
                command.CommandText = @"INSERT INTO projects
                    (name, billedeurl ,datecreated, svend_timepris, lærling_timepris, konsulent_timepris, arbejdsmand_timepris) 
                    VALUES (@name, @billedeurl ,@datecreated, @svend_timepris, @lærling_timepris, @konsulent_timepris, @arbejdsmand_timepris)          
                    RETURNING projectid"; //Retunering query som sender projectid tilbage
                var paramStop = command.CreateParameter();
                paramStop.ParameterName = "name";
                command.Parameters.Add(paramStop);
                paramStop.Value = pro.Name;
                var paramBillede = command.CreateParameter();
                paramBillede.ParameterName = "billedeurl";
                command.Parameters.Add(paramBillede);
                paramBillede.Value = pro.ImageUrl;
                var paramTimer = command.CreateParameter();
                paramTimer.ParameterName = "datecreated";
                command.Parameters.Add(paramTimer);
                paramTimer.Value = pro.DateCreated;
                var paramType = command.CreateParameter();
                paramType.ParameterName = "svend_timepris";
                command.Parameters.Add(paramType);
                paramType.Value = pro.SvendTimePris;
                var paramKost = command.CreateParameter();
                paramKost.ParameterName = "lærling_timepris";
                command.Parameters.Add(paramKost);
                paramKost.Value = pro.LærlingTimePris;
                var paramKons = command.CreateParameter();
                paramKons.ParameterName = "konsulent_timepris";
                command.Parameters.Add(paramKons);
                paramKons.Value = pro.KonsulentTimePris;
                var paramArb = command.CreateParameter();
                paramArb.ParameterName = "arbejdsmand_timepris";
                command.Parameters.Add(paramArb);
                paramArb.Value = pro.ArbjedsmandTimePris;
                var newProjectId = (int)command.ExecuteScalar();
                return newProjectId; //retunere det nye id
                command.ExecuteNonQuery();
            }
        }
        public void AddHour(ProjectHour proj)
        {
            var result = new List<ProjectHour>();
            using (var mConnection = new NpgsqlConnection(conString))
            {
                mConnection.Open();
                var command = mConnection.CreateCommand();
                command.CommandText = @"INSERT INTO projecthours
                    (projectid, medarbejder, dato, stoptid, timer, type, kostpris) 
                    VALUES (@projectid, @medarbejder, @dato, @stoptid, @timer, @type, @kostpris)";
                var paramProjId = command.CreateParameter();
                paramProjId.ParameterName = "projectid";
                command.Parameters.Add(paramProjId);
                paramProjId.Value = proj.ProjectId;
                var paramMed = command.CreateParameter();
                paramMed.ParameterName = "medarbejder";
                paramMed.Value = proj.Medarbejder ?? (object)DBNull.Value;
                command.Parameters.Add(paramMed);
                Console.WriteLine(command.CommandText);
                var paramDato = command.CreateParameter();
                paramDato.ParameterName = "dato";
                command.Parameters.Add(paramDato);
                paramDato.Value = proj.Dato;
                var paramStop = command.CreateParameter();
                paramStop.ParameterName = "stoptid";
                command.Parameters.Add(paramStop);
                paramStop.Value = proj.Stoptid;
                var paramTimer = command.CreateParameter();
                paramTimer.ParameterName = "timer";
                command.Parameters.Add(paramTimer);
                paramTimer.Value = proj.Timer;
                var paramType = command.CreateParameter();
                paramType.ParameterName = "type";
                command.Parameters.Add(paramType);
                paramType.Value = proj.Type;
                var paramKost = command.CreateParameter();
                paramKost.ParameterName = "kostpris";
                command.Parameters.Add(paramKost);
                paramKost.Value = proj.Kostpris;
                command.ExecuteNonQuery();
            }
        }
        public void AddMaterials(ProjectMaterial projmat)
        {
            var result = new List<ProjectMaterial>();
            using (var mConnection = new NpgsqlConnection(conString))
            {
                mConnection.Open();
                var command = mConnection.CreateCommand();
                command.CommandText = @"INSERT INTO projectmaterials
                    (projectid, beskrivelse, kostpris, antal, total, avance, dækningsgrad) 
                    VALUES (@projectid, @beskrivelse, @kostpris, @antal, @total, @avance, @dækningsgrad)";
                var paramProjId = command.CreateParameter();
                paramProjId.ParameterName = "projectid";
                command.Parameters.Add(paramProjId);
                paramProjId.Value = projmat.ProjectId;
                Console.WriteLine(command.CommandText);
                var paramBeskriv = command.CreateParameter();
                paramBeskriv.ParameterName = "beskrivelse";
                command.Parameters.Add(paramBeskriv);
                paramBeskriv.Value = projmat.Beskrivelse;
                var paramKost = command.CreateParameter();
                paramKost.ParameterName = "kostpris";
                command.Parameters.Add(paramKost);
                paramKost.Value = projmat.Kostpris;
                var paramAntal = command.CreateParameter();
                paramAntal.ParameterName = "antal";
                command.Parameters.Add(paramAntal);
                paramAntal.Value = projmat.Antal;
                var paramTotal = command.CreateParameter();
                paramTotal.ParameterName = "total";
                command.Parameters.Add(paramTotal);
                paramTotal.Value = projmat.Total;
                var PriceAvance = command.CreateParameter();
                PriceAvance.ParameterName = "avance";
                command.Parameters.Add(PriceAvance);
                PriceAvance.Value = projmat.Avance;
                var paramDaek = command.CreateParameter();
                paramDaek.ParameterName = "dækningsgrad";
                command.Parameters.Add(paramDaek);
                paramDaek.Value = projmat.Dækningsgrad;
                command.ExecuteNonQuery();
            }
        }
        public IEnumerable<Project> GetAllProjects()
        {
            var projects = new List<Project>();
            using (var conn = new NpgsqlConnection(conString))
            {
                conn.Open();
                // Vi henter alle projekter, sorteret med nyeste først (valgfrit, men brugervenligt)
                string query = "SELECT * FROM projects ORDER BY datecreated DESC";
                using (var command = new NpgsqlCommand(query, conn))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var p = new Project
                        {
                            ProjectId = Convert.ToInt32(reader["projectid"]),
                            Name = reader["name"] == DBNull.Value ? "Ukendt" : reader["name"].ToString(),
                            DateCreated = reader["datecreated"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["datecreated"]),
                            ImageUrl = reader["imageurl"] == DBNull.Value ? string.Empty : reader["imageurl"].ToString(),
                            SvendTimePris = reader["svend_timepris"] == DBNull.Value ? 0 : Convert.ToInt32(reader["svend_timepris"]),
                            LærlingTimePris = reader["lærling_timepris"] == DBNull.Value ? 0 : Convert.ToInt32(reader["lærling_timepris"]),
                            KonsulentTimePris = reader["konsulent_timepris"] == DBNull.Value ? 0 : Convert.ToInt32(reader["konsulent_timepris"]),
                            ArbjedsmandTimePris = reader["arbejdsmand_timepris"] == DBNull.Value ? 0 : Convert.ToInt32(reader["arbejdsmand_timepris"])
                        };
                        projects.Add(p);
                    }
                }
            }
            return projects;
        }
        public Calculation? GetProjectDetails(int projectId)
        {
            using var conn = new NpgsqlConnection(conString);
            conn.Open();
            var dto = new Calculation();
            using (var command = new NpgsqlCommand("SELECT * FROM projects WHERE projectid = @id", conn))
            {
                command.Parameters.AddWithValue("id", projectId);
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    dto.Project = new Project
                    {
                        ProjectId = Convert.ToInt32(reader["projectid"]),
                        Name = reader["name"].ToString(),
                        DateCreated = Convert.ToDateTime(reader["datecreated"]),
                        // Her henter vi satserne som skal ganges med timerne senere
                        SvendTimePris = Convert.ToInt32(reader["svend_timepris"]),
                        LærlingTimePris = Convert.ToInt32(reader["lærling_timepris"]),
                        KonsulentTimePris = Convert.ToInt32(reader["konsulent_timepris"]),
                        ArbjedsmandTimePris = Convert.ToInt32(reader["arbejdsmand_timepris"])
                    };
                }
                else return null;
            }
            using (var command = new NpgsqlCommand("SELECT * FROM projectmaterials WHERE projectid = @id", conn))
            {
                command.Parameters.AddWithValue("id", projectId);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var m = new ProjectMaterial
                    {
                        ProjectId = Convert.ToInt32(reader["projectid"]),
                        Beskrivelse = reader["beskrivelse"] == DBNull.Value ? "" : reader["beskrivelse"].ToString(),
                        Kostpris = Convert.ToDecimal(reader["kostpris"]),
                        Antal = Convert.ToDecimal(reader["antal"]),
                        Total = Convert.ToDecimal(reader["total"]),
                        Avance = Convert.ToDecimal(reader["avance"]),
                        Dækningsgrad = Convert.ToDecimal(reader["dækningsgrad"]),
                    };
                    dto.Materials.Add(m);
                    dto.TotalKostPrisMaterialer += (m.Kostpris * m.Antal);
                    dto.TotalPrisMaterialer += m.Total;
                }
            }
            using (var command = new NpgsqlCommand("SELECT * FROM projecthours WHERE projectid = @id", conn))
            {
                command.Parameters.AddWithValue("id", projectId);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    dto.Hours.Add(new ProjectHour
                    {
                        ProjectId = Convert.ToInt32(reader["projectid"]),
                        // Henter navnet så vi kan koble overtid med rollen
                        Medarbejder = reader["medarbejder"] == DBNull.Value ? "Ukendt" : reader["medarbejder"].ToString(),
                        Dato = reader["dato"] == DBNull.Value ? null : Convert.ToDateTime(reader["dato"]),
                        Stoptid = reader["stoptid"] == DBNull.Value ? null : Convert.ToDateTime(reader["stoptid"]),
                        Timer = Convert.ToDecimal(reader["timer"]),
                        Type = reader["type"] == DBNull.Value ? "" : reader["type"].ToString(),
                        Kostpris = Convert.ToDecimal(reader["kostpris"]),
                    });
                }
            }
            // 4. BEREGN PRISER PÅ TIMER (Logik med LINQ)
            // Vi grupperer pr. medarbejder for at finde ud af, om de er Svend eller Lærling
            foreach (var group in dto.Hours.GroupBy(x => x.Medarbejder))
            {
                // A. Find rollen: Kig efter den første linje, der IKKE er overtid.
                var normalType = group
                    .FirstOrDefault(h => !h.Type.ToLower().Contains("overtid"))?
                    .Type.ToLower() ?? "svend"; // Fallback til Svend hvis intet findes
                // B. Find grundsatsen baseret på rollen
                decimal grundSats = 0;
                if (normalType.Contains("lærling"))       grundSats = dto.Project.LærlingTimePris;
                else if (normalType.Contains("konsulent")) grundSats = dto.Project.KonsulentTimePris;
                else if (normalType.Contains("arbejdsmand")) grundSats = dto.Project.ArbjedsmandTimePris;
                else                                       grundSats = dto.Project.SvendTimePris;
                // C. Gennemgå timerne og læg overtidstillæg på
                foreach (var h in group)
                {
                    decimal faktor = 1.0m;
                    string typeLower = h.Type.ToLower();
                    if (typeLower.Contains("overtid 1")) faktor = 1.5m; // 50% ekstra
                    else if (typeLower.Contains("overtid 2")) faktor = 2.0m; // 100% ekstra
                    // Beregn og gem
                    decimal salgsPrisForRække = h.Timer * grundSats * faktor;
                    dto.TotalPrisTimer += salgsPrisForRække;
                    dto.TotalKostPrisTimer += h.Kostpris;
                }
            }
            // Opdater total timer til visning
            dto.TotalTimer = dto.Hours.Sum(h => h.Timer);
            return dto;
        }
    }
}
</file>

<file path="Client/Components/CreateProjectComponent.razor">
@using Core
@inject NavigationManager Nav
@inject HttpClient http

<div class="create-page-back">
    <div class="create-con">
        <h3>Opret projekt</h3>

        <EditForm Model="@aProject" OnValidSubmit="OnClickCreate">
            
            <DataAnnotationsValidator />
            <ValidationSummary />

            <div class="upload-grid">
                <div class="upload-box">
                    <label class="upload-title"><strong>Vælg time fil (.xls, .xlsx)</strong></label>
                    <div class="upload-area">
                        <InputFile OnChange="UploadFile" accept=".xls,.xlsx" />
                    </div>
                </div>

                <div class="upload-box">
                    <label class="upload-title"><strong>Vælg materiale fil (.xls, .xlsx)</strong></label>
                    <div class="upload-area">
                        <InputFile OnChange="MaterialUploadFile" accept=".xls,.xlsx" />
                    </div>
                </div>
            </div>

            <div class="form-floating-group">
                <InputText id="name" class="form-control" @bind-Value="aProject.Name" />
                <label for="name">Navn</label>
            </div>
                
            <div class="form-grid">
                <div class="form-floating-group">
                    <InputNumber id="svend" class="form-control" @bind-Value="aProject.SvendTimePris" />
                    <label for="svend">Svend sats</label>
                </div>
                <div class="form-floating-group">
                    <InputNumber id="lærling" class="form-control" @bind-Value="aProject.LærlingTimePris" />
                    <label for="lærling">Lærling sats</label>
                </div>
                <div class="form-floating-group">
                    <InputNumber id="konsulent" class="form-control" @bind-Value="aProject.KonsulentTimePris" />
                    <label for="konsulent">Konsulent sats</label>
                </div>
                <div class="form-floating-group">
                    <InputNumber id="arbejdsmand" class="form-control" @bind-Value="aProject.ArbjedsmandTimePris" />
                    <label for="konsulent">Arbejdsmand sats</label>
                </div>

                <div class="form-floating-group">
                    <InputText id="billede" class="form-control" @bind-Value="aProject.ImageUrl" />
                    <label for="billede">Billede link</label>
                </div>
                
            </div>
                
            <div class="">
                <div class="">
                    <button type="submit" class="opretbtn">Opret</button>
                </div>
            </div>
            
        </EditForm>
        
        
    </div>
</div>


@code {
    // Initialiserer en ny instans af projekt-modellen
    Project aProject = new();

    // Variabler til at holde filerne midlertidigt. 'IBrowserFile?' betyder, de kan være null.
    IBrowserFile? timeFile;
    IBrowserFile? materialeFile;

    // Metode der køres, når brugeren vælger en fil i "Time" upload-feltet
    public void UploadFile(InputFileChangeEventArgs e)
    {
        // Gemmer filen i variablen, men uploader den ikke endnu
        timeFile = e.File; 
    }

    // Metode der køres, når brugeren vælger en fil i "Materiale" upload-feltet
    public void MaterialUploadFile(InputFileChangeEventArgs e)
    {
        materialeFile = e.File;
    }

    // Hovedmetoden der køres, når brugeren trykker på "Opret" knappen
    private async Task OnClickCreate()
    {
        // 1. Send projektdata (tekst/tal) til API'et for at oprette rækken i databasen
        var response = await http.PostAsJsonAsync("http://localhost:5028/api/createproject", aProject);

        // 2. Tjek om oprettelsen gik godt (HTTP status 200-299)
        if (response.IsSuccessStatusCode)
        {
            // 3. Læs det nye ID som API'et returnerer (vigtigt for at kunne koble filerne)
            var newProjectId = await response.Content.ReadFromJsonAsync<int>();

            // 4. Hvis der er valgt en Time-fil, upload den nu med det nye ID
            if (timeFile != null)
            {
                await UploadFileToApi(timeFile, newProjectId, "api/createproject/uploadhours");
            }

            // 5. Hvis der er valgt en Materiale-fil, upload den nu med det nye ID
            if (materialeFile != null)
            {
                await UploadFileToApi(materialeFile, newProjectId, "api/createproject/uploadmaterials");
            }
        }

        // 6. Nulstil formularen og naviger brugeren tilbage til forsiden
        aProject = new();
        Nav.NavigateTo("/");
    }

    // Hjælpemetode til selve fil-uploaden (genbruges for at undgå duplikeret kode)
    private async Task UploadFileToApi(IBrowserFile file, int projectId, string endpoint)
    {
        // Åbner en strøm til læsning af filen. 
        // 10000000 bytes = ca. 10 MB grænse. Filer større end dette vil fejle.
        var stream = file.OpenReadStream(10000000);

        // Opretter en 'container' til filen, ligesom en HTML <form>
        using var content = new MultipartFormDataContent();
        
        // Tilføjer fil-strømmen til indholdet med navnet "file"
        content.Add(new StreamContent(stream), "file", file.Name);

        // Sender filen til det specifikke endpoint (API URL) med projectId som query parameter
        await http.PostAsync($"http://localhost:5028/{endpoint}?projectId={projectId}", content);
    }
}
</file>

<file path="Client/Layout/NavMenu.razor">
@using Core
@inject Blazored.LocalStorage.ILocalStorageService LocalStorage
@inject NavigationManager navManager


<div class="logo-con">
    <div class="m-4">
        <a href="/">
            <img src="Assets/Larsen-logo_2021hvid.png" class="logo-larsen"/>
        </a>
        <button title="Navigation menu" class="navbar-toggler" @onclick="ToggleNavMenu">
            <span>
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-list" viewBox="0 0 16 16">
                    <path fill-rule="evenodd" d="M2.5 12a.5.5 0 0 1 .5-.5h10a.5.5 0 0 1 0 1H3a.5.5 0 0 1-.5-.5m0-4a.5.5 0 0 1 .5-.5h10a.5.5 0 0 1 0 1H3a.5.5 0 0 1-.5-.5m0-4a.5.5 0 0 1 .5-.5h10a.5.5 0 0 1 0 1H3a.5.5 0 0 1-.5-.5"/>
                </svg></span>
        </button>
    </div>
</div>

<div class="@NavMenuCssClass nav-scrollable" @onclick="ToggleNavMenu">
    <nav class="nav flex-column h-100">
        <div class="nav-item px-3">
            <NavLink class="nav-link" href="projectpage" Match="NavLinkMatch.All">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-folder" viewBox="0 0 16 16">
                    <path d="M.54 3.87.5 3a2 2 0 0 1 2-2h3.672a2 2 0 0 1 1.414.586l.828.828A2 2 0 0 0 9.828 3h3.982a2 2 0 0 1 1.992 2.181l-.637 7A2 2 0 0 1 13.174 14H2.826a2 2 0 0 1-1.991-1.819l-.637-7a2 2 0 0 1 .342-1.31zM2.19 4a1 1 0 0 0-.996 1.09l.637 7a1 1 0 0 0 .995.91h10.348a1 1 0 0 0 .995-.91l.637-7A1 1 0 0 0 13.81 4zm4.69-1.707A1 1 0 0 0 6.172 2H2.5a1 1 0 0 0-1 .981l.006.139q.323-.119.684-.12h5.396z"/>
                </svg> Projekter
            </NavLink>
        </div>
        <div class="nav-item px-3">
            <NavLink class="nav-link" href="CreateProjectPage">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-file-plus" viewBox="0 0 16 16">
                    <path d="M8.5 6a.5.5 0 0 0-1 0v1.5H6a.5.5 0 0 0 0 1h1.5V10a.5.5 0 0 0 1 0V8.5H10a.5.5 0 0 0 0-1H8.5z"/>
                    <path d="M2 2a2 2 0 0 1 2-2h8a2 2 0 0 1 2 2v12a2 2 0 0 1-2 2H4a2 2 0 0 1-2-2zm10-1H4a1 1 0 0 0-1 1v12a1 1 0 0 0 1 1h8a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1"/>
                </svg> Opret projekt
            </NavLink>
        </div>
        
        @if (loggedIn != null && loggedIn.Role.Equals("admin", StringComparison.CurrentCultureIgnoreCase))
        {
            <div class="nav-item px-3 push-bottom">
                <NavLink class="nav-link" href="adduser">
                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-receipt-cutoff" viewBox="0 0 16 16">
                        <path d="M3 4.5a.5.5 0 0 1 .5-.5h6a.5.5 0 1 1 0 1h-6a.5.5 0 0 1-.5-.5m0 2a.5.5 0 0 1 .5-.5h6a.5.5 0 1 1 0 1h-6a.5.5 0 0 1-.5-.5m0 2a.5.5 0 0 1 .5-.5h6a.5.5 0 1 1 0 1h-6a.5.5 0 0 1-.5-.5m0 2a.5.5 0 0 1 .5-.5h6a.5.5 0 0 1 0 1h-6a.5.5 0 0 1-.5-.5m0 2a.5.5 0 0 1 .5-.5h6a.5.5 0 0 1 0 1h-6a.5.5 0 0 1-.5-.5M11.5 4a.5.5 0 0 0 0 1h1a.5.5 0 0 0 0-1zm0 2a.5.5 0 0 0 0 1h1a.5.5 0 0 0 0-1zm0 2a.5.5 0 0 0 0 1h1a.5.5 0 0 0 0-1zm0 2a.5.5 0 0 0 0 1h1a.5.5 0 0 0 0-1zm0 2a.5.5 0 0 0 0 1h1a.5.5 0 0 0 0-1z" />
                        <path d="M2.354.646a.5.5 0 0 0-.801.13l-.5 1A.5.5 0 0 0 1 2v13H.5a.5.5 0 0 0 0 1h15a.5.5 0 0 0 0-1H15V2a.5.5 0 0 0-.053-.224l-.5-1a.5.5 0 0 0-.8-.13L13 1.293l-.646-.647a.5.5 0 0 0-.708 0L11 1.293l-.646-.647a.5.5 0 0 0-.708 0L9 1.293 8.354.646a.5.5 0 0 0-.708 0L7 1.293 6.354.646a.5.5 0 0 0-.708 0L5 1.293 4.354.646a.5.5 0 0 0-.708 0L3 1.293zm-.217 1.198.51.51a.5.5 0 0 0 .707 0L4 1.707l.646.647a.5.5 0 0 0 .708 0L6 1.707l.646.647a.5.5 0 0 0 .708 0L8 1.707l.646.647a.5.5 0 0 0 .708 0L10 1.707l.646.647a.5.5 0 0 0 .708 0L12 1.707l.646.647a.5.5 0 0 0 .708 0l.509-.51.137.274V15H2V2.118z" />
                    </svg> Tilføj bruger
                </NavLink>
            </div>
        }
        
    </nav>
</div>




@code {
    private bool collapseNavMenu = true;

    private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

    private void ToggleNavMenu()
    {
        collapseNavMenu = !collapseNavMenu;
    }

    Users? loggedIn;

    protected override async Task OnInitializedAsync()
    {
        loggedIn = await LocalStorage.GetItemAsync<Users?>("user");
    }
}
</file>

<file path="Server/Program.cs">
using Server.Repositories;
using Server.Repositories.User;
using Server.Service;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
builder.Services.AddSingleton<ICreateUserRepoSQL, CreateUserRepoSQL>();
builder.Services.AddSingleton<ICreateProjectRepo, CreateProjectRepo>();
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("policy",
        policy =>
        {
            policy.AllowAnyOrigin();
            policy.AllowAnyMethod();
            policy.AllowAnyHeader();
        });
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseCors("policy");
//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
</file>

</files>
