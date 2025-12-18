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
    Create project/
      CreateProjectComponent.razor
    ProjectDetails/
      PrisOverblik.razor
      RessourceOverblik.razor
    Projects/
      ProjectComponent.razor
      ProjectGrid.razor
  Layout/
    MainLayout.razor
    NavMenu.razor
  Pages/
    Project/
      CreateProjectPage.razor
      EditProjectPage.razor
      ProjectDetailsPage.razor
      ProjectPage.razor
    User/
      AddUser.razor
      AdminUserPage.razor
    LoginPage.razor
  Service/
    ProjectService.cs
    Server.cs
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
    ProjectController/
      ProjectController.cs
    UserCon/
      UserController.cs
    ProjectHoursController.cs
    ProjectMaterialsController.cs
  Repositories/
    HourRepositories/
      HourRepositorySQL.cs
      IHourRepository.cs
    MaterialRepositories/
      IMaterialRepository.cs
      MaterialRepositorySQL.cs
    ProjectRepositories/
      IProjectRepository.cs
      ProjectRepositorySQL.cs
    User/
      CreateUserRepoSQL.cs
      ICreateUserRepo.cs
    BaseRepository.cs
  Service/
    MaterialConverter.cs
    ProjectCalculationsService.cs
    WorkerConverter.cs
  appsettings.Development.json
  appsettings.json
  Program.cs
</directory_structure>

<files>
This section contains the contents of the repository's files.

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

<file path="Server/Repositories/HourRepositories/IHourRepository.cs">
using Core;
namespace Server.Repositories.HourRepositories;
// Interface for HourRepositorySQL
// Definerer hvilke metoder en repository til ProjectHour skal implementere
public interface IHourRepository
{
    // Metode til at tilføje en ProjectHour til databasen
    // Implementeringen skal oprette en SQL-INSERT og eksekvere den
    void Add(ProjectHour hour);
    // Metode til at hente alle ProjectHour for et bestemt projekt
    // Returnerer en liste med ProjectHour-objekter
    List<ProjectHour> GetByProjectId(int projectId);
}
</file>

<file path="Server/Repositories/MaterialRepositories/IMaterialRepository.cs">
using Core;
namespace Server.Repositories.MaterialRepositories;
// Interface for MaterialRepositorySQL
// Definerer hvilke metoder en MaterialRepository skal implementere
public interface IMaterialRepository
{
    // Metode til at tilføje et ProjectMaterial til databasen
    // Implementeringen vil normalt oprette en SQL-INSERT og eksekvere den
    void Add(ProjectMaterial material);
    // Metode til at hente alle ProjectMaterialer for et specifikt projekt
    // Returnerer en liste med ProjectMaterial-objekter
    List<ProjectMaterial> GetByProjectId(int projectId);
}
</file>

<file path="Server/Repositories/User/ICreateUserRepo.cs">
using Core;
namespace Server.Repositories.User
{
    // Interface som beskriver hvilke metoder et "User Repository" skal have
    // Formål er at være en mellem mand imellem repo og controller
    public interface ICreateUserRepo
    {
        List<Users> GetAll();   // Skal kunne hente alle brugere
        void Add(Users user);   // Skal kunne tilføje en ny bruger
        void Delete(int id);    // Skal kunne slette en bruger ud fra ID
        Users? ValidateUser(string username, string password);  // Skal kunne validere login
    }
}
</file>

<file path="Client/Layout/MainLayout.razor">
@inject Blazored.LocalStorage.ILocalStorageService LocalStorage
@using Core
@inject NavigationManager navManager
@inherits LayoutComponentBase
<div class="page">
    
        <div class="sidebar">
            <NavMenu/>
        </div>

        <main>
            <div class="top-row px-2 btn-primary" style="color: white">
            <NavLink class="nav-link" href="logout" @onclick="Logout">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-door-closed" viewBox="0 0 16 16">
                    <path d="M3 2a1 1 0 0 1 1-1h8a1 1 0 0 1 1 1v13h1.5a.5.5 0 0 1 0 1h-13a.5.5 0 0 1 0-1H3zm1 13h8V2H4z"/>
                    <path d="M9 9a1 1 0 1 0 2 0 1 1 0 0 0-2 0"/>
                </svg> Log ud
            </NavLink>
            </div>
            <article class="content px-4">
                @Body </article>
        </main>
    
</div>

@code
{
    bool isLoggedIn = false;

    protected override async Task OnInitializedAsync()
    {
        var user = await LocalStorage.GetItemAsync<Users>("user");
        isLoggedIn = user != null;
    }
    // Metode der køres når man trykker log ud
    private async Task Logout()
    { 
        // Dataene slettes og man navigeres til login-siden
        await LocalStorage.RemoveItemAsync("user");
        navManager.NavigateTo("/", true);
    }
}
</file>

<file path="Client/Pages/Project/CreateProjectPage.razor">
@page "/CreateProjectPage"
@using Client.Components.Create_project

@* Denne side har kun formålet, at give siden en URL, så man kan navigere til CreateProjectPage*@
<CreateProjectComponent></CreateProjectComponent>

@* Her er ingen kode, da det hele ligger i CreateProjectComponent*@
@code {
    
}
</file>

<file path="Client/Service/ProjectService.cs">
using Core;
using System.Net.Http.Json;
namespace Client.Service
{
    public class ProjectService
    {
        private readonly HttpClient _http;
        public async Task<Calculation?> GetProjectDetails(int id)
        {
            try
            {
                // Kalder endpointet: api/project/5 (hvis id er 5)
                return await _http.GetFromJsonAsync<Calculation>($"api/project/{id}");
            }
            catch (Exception)
            {
                // Hvis projektet ikke findes eller der er fejl, returner null
                return null;
            }
        }
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
            // Pakker brugernavn og password ind i et objekt
            var login = new { UserName = name, Password = password };
            HttpResponseMessage response;
            try
            {
                // Dataene bliver sendt til "api/user/login"
                response = await _http.PostAsJsonAsync("api/user/login", login);
            }
            catch (Exception)
            {
                // Hvis der sker fejl returneres null, så hele programmet ikke går ned
                return null;
            }
            if (response.IsSuccessStatusCode)
            {
                // Hvis serveren godkender, laves det til et User-objekt.
                var user = await response.Content.ReadFromJsonAsync<Users>();
                return user;
            }
            return null;
        }
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
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri($"{Server.Url}") });
// Dette får appen til at bruge den samme adresse, som den selv ligger på
/*builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });*/
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<ProjectService>();
await builder.Build().RunAsync();
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
        public string? Leverandør { get; set; }
        public decimal Avance { get; set; }
        public decimal Dækningsgrad { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
</file>

<file path="Server/Repositories/MaterialRepositories/MaterialRepositorySQL.cs">
using Core;
namespace Server.Repositories.MaterialRepositories;
// Repository til håndtering af projectmaterials i databasen
public class MaterialRepositorySQL : BaseRepository, IMaterialRepository
{
    // Tilføjer et nyt ProjectMaterial til databasen
    public void Add(ProjectMaterial m)
    {
        // Opretter databaseforbindelse, lukkes automatisk når using-blokken afsluttes
        using var conn = GetConnection();
        conn.Open(); // Åbner forbindelsen
        // Opretter en SQL-kommando, lukkes automatisk med using
        using var command = conn.CreateCommand();
        command.CommandText = @"
            INSERT INTO projectmaterials
                (projectid, beskrivelse, kostpris, antal, leverandør, total, avance, dækningsgrad) 
            VALUES 
                (@pid, @besk, @kost, @antal, @lev, @total, @avance, @dg)";
        // Binder værdier fra ProjectMaterial-objektet til SQL-parametre
        // Brug af parametre forhindrer SQL-injection og sikrer korrekt datatype
        command.Parameters.AddWithValue("pid", m.ProjectId);
        // @pid i SQL får værdien af ProjectId fra objektet
        command.Parameters.AddWithValue("besk", m.Beskrivelse ?? (object)DBNull.Value);
        // @besk får værdien af Beskrivelse, hvis null sendes DBNull.Value til databasen
        //osv
        command.Parameters.AddWithValue("kost", m.Kostpris);
        command.Parameters.AddWithValue("antal", m.Antal);
        command.Parameters.AddWithValue("lev", m.Leverandør ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("total", m.Total);
        command.Parameters.AddWithValue("avance", m.Avance);
        command.Parameters.AddWithValue("dg", m.Dækningsgrad);
        // Køre INSERT-kommandoen
        command.ExecuteNonQuery();
    }
    // Henter alle ProjectMaterial til et specifikt projekt
    public List<ProjectMaterial> GetByProjectId(int projectId)
    {
        var list = new List<ProjectMaterial>(); // Liste til resultater
        // Opretter databaseforbindelse
        using var conn = GetConnection();
        conn.Open(); // Åbner forbindelsen
        // Opretter SQL-kommando
        using var command = conn.CreateCommand();
        command.CommandText = "SELECT * FROM projectmaterials WHERE projectid = @id";
        command.Parameters.AddWithValue("id", projectId); // Parameter for sikkerhed
        // Læser resultater fra databasen
        using var reader = command.ExecuteReader();
        while (reader.Read()) // Læser én række ad gangen
        {
            // Mapper databasekolonner til et ProjectMaterial-objekt
            // Hver række i databasen bliver til et objekt i C#-listen
            list.Add(new ProjectMaterial
            {
                // Henter 'materialsid' kolonnen fra databasen
                // Hvis værdien er NULL, bruges 0 som default
                MaterialsId = Convert.ToInt32(reader["materialsid"] is DBNull ? 0 : reader["materialsid"]),
                // Henter 'projectid' kolonnen, forventes altid at have en værdi
                ProjectId = Convert.ToInt32(reader["projectid"]),
                // Henter 'beskrivelse' kolonnen
                // Hvis NULL, bruges tom streng, ellers konverteres til string
                Beskrivelse = reader["beskrivelse"] == DBNull.Value ? "" : reader["beskrivelse"].ToString(),
                // Henter 'kostpris' kolonnen og konverterer til decimal
                Kostpris = Convert.ToDecimal(reader["kostpris"]),
                //Osv
                Antal = Convert.ToDecimal(reader["antal"]),
                Total = Convert.ToDecimal(reader["total"]),
                Leverandør = reader["leverandør"] == DBNull.Value ? "" : reader["leverandør"].ToString(),
                Dækningsgrad = Convert.ToDecimal(reader["dækningsgrad"]),
            });
        }
        return list; // Returnerer listen med materialer
    }
}
</file>

<file path="Server/Repositories/ProjectRepositories/IProjectRepository.cs">
using Core;
namespace Server.Repositories.ProjectRepositories;
// Interface som beskriver hvilke metoder et Project-repository skal have
public interface IProjectRepository
{
    int Create(Project p); // Skal kunne oprette et projekt og returnere projektets ID
    void Update(Project p); // Skal kunne opdatere et eksisterende projekt
    void Delete(int id); // Skal kunne slette et projekt ud fra ID
    Project? GetById(int id); // Skal kunne hente et projekt ud fra ID (eller null)
    IEnumerable<Project> GetAll(); // Skal kunne hente alle projekter
}
</file>

<file path="Server/Repositories/BaseRepository.cs">
using Npgsql;
using Server.PW1;
namespace Server.Repositories;
// Baseklasse som andre repositories arver fra
public abstract class BaseRepository
{
    // Connection string til online PostgreSQL-database (Neon)
    protected string ConnectionString =>
        // 1. Online database-server og port (Server=...)
        // 2. Brugernavn til databasen (UserId)
        // Password hentes fra PASSWORD-klassen
        // Databasens navn
        // Kræver krypteret forbindelse (nødvendigt for online database)
        @"Server=ep-spring-unit-a2y1k0pd.eu-central-1.aws.neon.tech:5432;
          User Id=neondb_owner;
          Password=" + PASSWORD.PW1 + @";
          Database=LarsenInstallation;
          Ssl Mode=Require;";
    // Opretter en ny databaseforbindelse
    // Forbindelsen åbnes først, når den bruges
    protected NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(ConnectionString);
    }
}
</file>

<file path="Client/Components/ProjectDetails/PrisOverblik.razor">
@using Core
@namespace Client.Components.ProjectDetails

@if (Details != null)
{
    // TOP BANNER
    <div class="card text-white mb-4 shadow text-center" style="@(IsClientView ? "background-color: #071226;" : "background-color: #071226;")">
        <div class="card-body py-4">
            @if (IsClientView)
            {
                // KUNDEVISNING: Kun den store samlede pris
                <h2 class="display-3 fw-bold">@Details.SamletTotalPris.ToString("N2") kr.</h2>
                <p class="fs-5 mb-0">Total Pris</p>
            }
            else
            {
                // INTERN VISNING: Fokus på fortjeneste
                <div class="row text-center divide-cols">
                    <div class="col-md-4">
                        <h6 class="text-white-50">TOTAL PRIS</h6>
                        <h2>@Details.SamletTotalPris.ToString("N2") kr.</h2>
                    </div>
                    <div class="col-md-4" style="border-left: 1px solid rgba(255,255,255,0.2); border-right: 1px solid rgba(255,255,255,0.2);">
                        <h6 class="text-white-50">KOST TOTAL</h6>
                        <h2>@Details.SamletKostPris.ToString("N2") kr.</h2>
                    </div>
                    <div class="col-md-4">
                        <h6 class="text-white">FORTJENESTE</h6>
                        <h2 class="text-white">@Details.Dækningsbidrag.ToString("N2") kr.</h2>
                        <span class="badge bg-light text-success">DG: @Details.Dækningsgrad.ToString("N1") %</span>
                    </div>
                </div>
            }
        </div>
    </div>

    // DE 3 BOKSE (materialer, timer, tid)
    <div class="row mb-4">

        <div class="col-md-4">
            <div class="card h-100 shadow-sm text-center" style="border-color: #071226;">
                <div class="card-header bg-transparent fw-bold" style="border-color: #071226; color: #071226;">
                    Materialer
                </div>
                <div class="card-body d-flex align-items-center justify-content-center flex-column">
                    <h3 class="card-title text-dark">
                        @if (IsClientView)
                        {
                            @Details.TotalPrisMaterialer.ToString("N2")
                            // Kunden ser salgspris
                        }
                        else
                        {
                            @Details.TotalKostPrisMaterialer.ToString("N2")
                            // Intern ser kostpris
                        }
                        kr.
                    </h3>
                    <small class="text-muted">
                        @(IsClientView ? "Salg af materialer" : "Kostpris (Indkøb)")
                    </small>
                </div>
            </div>
        </div>

        <div class="col-md-4">
            <div class="card h-100 shadow-sm text-center" style="border-color: #071226;">
                <div class="card-header bg-transparent fw-bold" style="border-color: #071226; color: #071226;">
                    Arbejdsløn
                </div>
                <div class="card-body d-flex align-items-center justify-content-center flex-column">
                    <h3 class="card-title text-dark">
                        @if (IsClientView)
                        {
                            // Kunde ser timepris
                            @Details.TotalPrisTimer.ToString("N2")
                        }
                        else
                        {
                            // Intern ser lønomkostning
                            @Details.TotalKostPrisTimer.ToString("N2")
                        }
                        kr.
                    </h3>
                    <small class="text-muted">
                        @(IsClientView ? "Total for timer" : "Lønomkostninger")
                    </small>
                </div>
            </div>
        </div>

        <div class="col-md-4">
            <div class="card h-100 shadow-sm text-center" style="border-color: #071226;">
                <div class="card-header bg-transparent fw-bold" style="border-color: #071226; color: #071226;">
                    Tid
                </div>
            
                <div class="card-body d-flex align-items-center justify-content-center flex-column">
                   
                    <h3 class="card-title text-dark">@Details.TotalTimer.ToString("N2") timer</h3>

                </div>
            </div>
        </div>
    </div>
}

@code {
    // Beregninger for projektet (kommer fra serveren)
    [Parameter] public Calculation? Details { get; set; }
    // True = kundevisning, False = intern visning
    [Parameter] public bool IsClientView { get; set; }
}
</file>

<file path="Client/Components/ProjectDetails/RessourceOverblik.razor">
@using Core
@namespace Client.Components.ProjectDetails

<div class="row mb-5">
    <!-- Timer -->
    <div class="col-md-6 mb-3">
        <div class="card h-100 shadow-sm" style="border-color: #071226;">
            <div class="card-header bg-transparent fw-bold" style="border-color: #071226; color: #071226;">
                Timeoversigt
            </div>
            <ul class="list-group list-group-flush">
                @foreach (var group in HourGroups)
                {
                    <li class="list-group-item d-flex justify-content-between align-items-center">
                        <span>@group.Type</span>
                        <span class="badge rounded-pill" style="background-color: #071226;">
                            @group.Total.ToString("N2") t
                        </span>
                    </li>
                }
            </ul>
        </div>
    </div>

    <!-- Materialer (ALTID kategori-liste fra serveren) -->
    <div class="col-md-6 mb-3">
        <div class="card h-100 shadow-sm"
             style="border-color: #071226; max-height: 500px; display:flex; flex-direction:column;">

            <div class="card-header bg-transparent fw-bold"
                 style="border-color: #071226; color: #071226;">
                Materialer fordelt på kategorier
            </div>

            <div style="overflow-y: auto; flex: 1;">
                <ul class="list-group list-group-flush">
                    @if (MaterialGroups.Any())
                    {
                        @foreach (var mat in MaterialGroups)
                        {
                            <li class="list-group-item d-flex justify-content-between align-items-center">
                                <span>@mat.Beskrivelse</span>
                                <span class="fw-bold">@mat.Total.ToString("N2") kr.</span>
                            </li>
                        }
                    }
                    else
                    {
                        <li class="list-group-item text-muted text-center">Ingen materialer registreret</li>
                    }
                </ul>
            </div>
        </div>
    </div>
</div>

@code {
    [Parameter] public List<HourGroupDto> HourGroups { get; set; } = new();

    // ALTID kategori-liste fra serveren
    [Parameter] public List<ProjectMaterial> MaterialGroups { get; set; } = new();

    [Parameter] public bool IsClientView { get; set; }
}
</file>

<file path="Client/Pages/LoginPage.razor">
@using Core
@using Blazored.LocalStorage
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
        isLoading = true; // Dette er loading animationen

        try
        {
            // Sender indtastede brugernavn og password til serveren (API'et).
            // UserRepo er en hjælpeklasse (Service), der snakker med backend
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
            catch // Ignorer hvis det fejler
            {
                // Hvis Users.Password er readonly eller lign.
                var safeUser = new
                {
                    userObject.UserName, userObject.Role
                };
                await LocalStorage.SetItemAsync("user", safeUser);
                Nav.NavigateTo("/", forceLoad: true); // Genindlæser siden for at App.razor kører forfra og viser forsiden
                return;
            }

            await LocalStorage.SetItemAsync("user", userObject);
            Nav.NavigateTo("/", forceLoad: true);
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

<file path="Server/Controllers/UserCon/UserController.cs">
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
        public UserController(ICreateUserRepo userRepo)
        {
            this.userRepo = userRepo; // Dependency Injection: Systemet giver controlleren et repository
        }
        [HttpGet] // Endpoint: GET api/user
        public IEnumerable<Users> Get()
        {
            return userRepo.GetAll(); // Henter alle brugere via repository
        }
        [HttpPost] // Endpoint: POST api/user
        public void Add(Users user)
        {
            userRepo.Add(user); // Kalder repo for at tilføje en ny bruger
        }
        [HttpPost("login")] // Endpoint: POST api/user/login
        public ActionResult<Users> Login([FromBody] Login dto)
        {
            // Validerer brugernavn og password gennem repo
            var user = userRepo.ValidateUser(dto.UserName, dto.Password);
            if (user == null)
                return Unauthorized(); // Returnerer HTTP 401 hvis login mislykkes
            return Ok(user); // Returnerer HTTP 200 + brugerdata hvis login lykkes
        }
        [HttpDelete]  // Endpoint: DELETE api/user/delete/{id}
        [Route("delete/{id:int}")] // Route med variabel id, kun int
        public void Delete(int id)
        {
            userRepo.Delete(id); // Sletter bruger via repository
        }
    }
}
</file>

<file path="Server/Controllers/ProjectMaterialsController.cs">
using Core;
using Microsoft.AspNetCore.Mvc;
using Server.Repositories.MaterialRepositories;
using Server.Service;
namespace Server.Controllers
{
    [ApiController]
    [Route("api/projectmaterials")]
    public class ProjectMaterialsController : ControllerBase
    {
        // Repository som gemmer materialer i databasen
        private readonly IMaterialRepository _repo;
        // Repository som gemmer materialer i databasen
        public ProjectMaterialsController(IMaterialRepository repo)
        {
            _repo = repo;
        }
        // Endpoint: bruges når man uploader en fil
        [HttpPost("upload")]
        public IActionResult Upload(IFormFile? file, [FromQuery] int projectId)
        {
            // Hvis der ikke er valgt en fil, fejlbesked
            if (file == null || file.Length == 0) return BadRequest("No file");
            try
            {
                // Opretter en midlertidig hukommelse (stream)
                using Stream s = new MemoryStream();
                file.CopyTo(s); // Kopierer filens indhold ind i hukommelsen
                s.Position = 0; // starter læsningen fra begyndelsen af filen 
                //konverterer excel filen til material objekter 
                var materials = MaterialConverter.Convert(s);
                foreach (var m in materials)
                {
                    //sætter materialer til projekter
                    m.ProjectId = projectId;
                    // gemmer materialet til db 
                    _repo.Add(m);
                }
                return Ok($"Uploaded {materials.Count} materials."); //succes besked 
            }
            catch (Exception ex)
            {
                return BadRequest("Error parsing file: " + ex.Message); // fejlbesked 
            }
        }
    }
}
</file>

<file path="Server/Repositories/HourRepositories/HourRepositorySQL.cs">
using Core;
namespace Server.Repositories.HourRepositories;
// Repository til håndtering af timer (ProjectHour) i databasen
public class HourRepositorySQL : BaseRepository, IHourRepository
{
    // Tilføjer en ny ProjectHour til databasen
    public void Add(ProjectHour h)
    {
        // Opretter databaseforbindelse, lukker automatisk når using-blokken afsluttes
        using var conn = GetConnection();
        conn.Open(); // Åbner forbindelsen
        // Opretter en SQL-kommando, lukker automatisk med using
        using var command = conn.CreateCommand();
        command.CommandText = @"
            INSERT INTO projecthours
                (projectid, medarbejder, dato, stoptid, timer, type, kostpris) 
            VALUES 
                (@pid, @med, @dato, @stop, @timer, @type, @kost)";
        // Binder værdier fra ProjectHour-objektet til SQL-parametre
        // Parametre beskytter mod SQL-injection og sikrer korrekt datatype
        command.Parameters.AddWithValue("pid", h.ProjectId);
        //pid for værdien af projectid
        command.Parameters.AddWithValue("med", h.Medarbejder ?? (object)DBNull.Value); //Hvis null så for den null som værdi
        command.Parameters.AddWithValue("dato", h.Dato ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("stop", h.Stoptid ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("timer", h.Timer);
        command.Parameters.AddWithValue("type", h.Type ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("kost", h.Kostpris);
        // Eksekverer INSERT-kommandoen
        command.ExecuteNonQuery();
    }
    // Henter alle ProjectHour for et bestemt projekt
    public List<ProjectHour> GetByProjectId(int projectId)
    {
        var list = new List<ProjectHour>(); // Liste til at gemme resultater
        // Opretter databaseforbindelse
        using var conn = GetConnection();
        conn.Open(); // Åbner forbindelsen
        // Opretter SQL-kommando
        using var command = conn.CreateCommand();
        command.CommandText = "SELECT * FROM projecthours WHERE projectid = @id";
        command.Parameters.AddWithValue("id", projectId); // Parameter for sikkerhed
        // Læser resultater fra databasen
        using var reader = command.ExecuteReader();
        while (reader.Read()) // Læser én række ad gangen
        {
            // Mapper databasekolonner til ProjectHour-objekt
            list.Add(new ProjectHour
            {
                // Håndterer NULL eller manglende ID-kolonne med default 0
                HourId = Convert.ToInt32(reader["hourid"] is DBNull ? 0 : reader["hourid"]),
                ProjectId = Convert.ToInt32(reader["projectid"]),
                // Hvis medarbejder er NULL, bruges standard "Ukendt"
                Medarbejder = reader["medarbejder"] == DBNull.Value ? "Ukendt" : reader["medarbejder"].ToString(),
                // Dato og stoptid håndteres som nullable DateTime
                Dato = reader["dato"] == DBNull.Value ? null : Convert.ToDateTime(reader["dato"]),
                Stoptid = reader["stoptid"] == DBNull.Value ? null : Convert.ToDateTime(reader["stoptid"]),
                Timer = Convert.ToDecimal(reader["timer"]), // decimal fra databasen
                Type = reader["type"] == DBNull.Value ? "" : reader["type"].ToString(), // tom streng hvis NULL
                Kostpris = Convert.ToDecimal(reader["kostpris"]),
            });
        }
        // Returnerer listen med ProjectHour-objekter
        return list;
    }
}
</file>

<file path="Server/Repositories/ProjectRepositories/ProjectRepositorySQL.cs">
using Core;
using Npgsql;
namespace Server.Repositories.ProjectRepositories;
// Repository der håndterer projekter i databasen
public class ProjectRepositorySQL : BaseRepository, IProjectRepository
{
    // Opretter et nyt projekt i databasen og returnerer det nye projectid
    public int Create(Project pro)
    {
        using var conn = GetConnection(); // Henter databaseforbindelse
        conn.Open(); // Åbner forbindelsen
        using var command = conn.CreateCommand(); // Opretter SQL-kommando
        command.CommandText = @"
            INSERT INTO projects
                (name, billedeurl, datecreated, svend_timepris, lærling_timepris, konsulent_timepris, arbejdsmand_timepris) 
            VALUES 
                (@name, @billedeurl, @datecreated, @svend, @lærling, @konsulent, @arbejdsmand)          
            RETURNING projectid"; // Returnerer ID på det nye projekt
        // Parametre sender værdier sikkert til SQL
        command.Parameters.AddWithValue("name", pro.Name);
        command.Parameters.AddWithValue("billedeurl", pro.ImageUrl);
        command.Parameters.AddWithValue("datecreated", pro.DateCreated);
        command.Parameters.AddWithValue("svend", pro.SvendTimePris);
        command.Parameters.AddWithValue("lærling", pro.LærlingTimePris);
        command.Parameters.AddWithValue("konsulent", pro.KonsulentTimePris);
        command.Parameters.AddWithValue("arbejdsmand", pro.ArbejdsmandTimePris);
        return (int)command.ExecuteScalar()!; // Kører INSERT og får projectid tilbage
    }
    // Opdaterer et eksisterende projekt
    public void Update(Project p)
    {
        using var conn = GetConnection(); // Henter databaseforbindelse
        conn.Open();
        using var command = conn.CreateCommand();
        command.CommandText = @"
            UPDATE projects SET 
                name = @name, 
                billedeurl = @billedeurl,
                svend_timepris = @svend, 
                lærling_timepris = @lærling, 
                konsulent_timepris = @konsulent, 
                arbejdsmand_timepris = @arbejdsmand
            WHERE projectid = @id"; // Opdaterer projekt ud fra ID
        // Parametre med nye værdier
        command.Parameters.AddWithValue("id", p.ProjectId);
        command.Parameters.AddWithValue("name", p.Name ?? "");
        command.Parameters.AddWithValue("billedeurl", p.ImageUrl ?? "");
        command.Parameters.AddWithValue("svend", p.SvendTimePris);
        command.Parameters.AddWithValue("lærling", p.LærlingTimePris);
        command.Parameters.AddWithValue("konsulent", p.KonsulentTimePris);
        command.Parameters.AddWithValue("arbejdsmand", p.ArbejdsmandTimePris);
        command.ExecuteNonQuery();            // Kører UPDATE-kommandoen
    }
    // Sletter et projekt ud fra ID
    public void Delete(int id)
    {
        using var conn = GetConnection();    // Henter databaseforbindelse
        conn.Open();
        using var command = conn.CreateCommand();
        command.CommandText = "DELETE FROM projects WHERE projectid = @id"; // SQL delete
        command.Parameters.AddWithValue("id", id);
        command.ExecuteNonQuery(); // Kører DELETE-kommandoen
    }
    // Henter ét projekt ud fra ID
    public Project? GetById(int id)
    {
        using var conn = GetConnection(); // Henter databaseforbindelse
        conn.Open();
        using var command = conn.CreateCommand();
        command.CommandText = "SELECT * FROM projects WHERE projectid = @id";
        command.Parameters.AddWithValue("id", id);
        using var reader = command.ExecuteReader(); // Kører SELECT og læser resultatet
        if (reader.Read()) // Hvis der findes en række
        {
            return MapProject(reader); // Mapper DB-data til Project-objekt
        }
        return null; // Returnerer null hvis intet projekt findes
    }
    // Henter alle projekter
    public IEnumerable<Project> GetAll()
    {
        var list = new List<Project>();
        using var conn = GetConnection();
        conn.Open();
        using var command = conn.CreateCommand();
        command.CommandText = "SELECT * FROM projects ORDER BY datecreated DESC";
        using var reader = command.ExecuteReader(); // Kører SELECT
        while (reader.Read())  // Læser én række ad gangen
        {
            list.Add(MapProject(reader)); // Mapper og tilføjer til listen
        }
        return list; // Returnerer alle projekter
    }
    // Hjælpemetode: laver et Project-objekt ud fra en database-række
    // Denne metode tager én række fra databasen (reader)
    // og laver den om til et Project-objekt i C#
    private Project MapProject(NpgsqlDataReader reader)
    {
        return new Project
        {
            // Henter projectid fra databasen og laver det om til int
            ProjectId = Convert.ToInt32(reader["projectid"]),
            // Henter navnet på projektet
            // Hvis værdien i databasen er NULL, bruges "Ukendt" i stedet
            Name = reader["name"] == DBNull.Value
                ? "Ukendt"
                : reader["name"].ToString()!,
            //Ligsom før men med dato
            DateCreated = reader["datecreated"] == DBNull.Value
                ? DateTime.MinValue
                : Convert.ToDateTime(reader["datecreated"]),
            ImageUrl = reader["billedeurl"] == DBNull.Value
                ? string.Empty
                : reader["billedeurl"].ToString()!,
            SvendTimePris = reader["svend_timepris"] == DBNull.Value
                ? 0
                : Convert.ToInt32(reader["svend_timepris"]),
            LærlingTimePris = reader["lærling_timepris"] == DBNull.Value
                ? 0
                : Convert.ToInt32(reader["lærling_timepris"]),
            KonsulentTimePris = reader["konsulent_timepris"] == DBNull.Value
                ? 0
                : Convert.ToInt32(reader["konsulent_timepris"]),
            ArbejdsmandTimePris = reader["arbejdsmand_timepris"] == DBNull.Value
                ? 0
                : Convert.ToInt32(reader["arbejdsmand_timepris"])
        };
    }
}
</file>

<file path="Server/Service/WorkerConverter.cs">
using Core;
using ExcelDataReader; // Bruges til at læse Excel-filer
using System.IO;
using System.Text;
namespace Server.Service
{
    // Service-klasse der konverterer Excel-data til ProjectHour
    public class WorkerConverter
    {
        // Konverterer Excel-stream til liste af ProjectHour
        public static List<ProjectHour> Convert(Stream stream)
        {
            // Registrerer encoding provider
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            // Opretter Excel reader til .xlsx filer
            IExcelDataReader reader =  ExcelReaderFactory.CreateOpenXmlReader(stream);
            // Læser Excel-data ind i et DataSet
            var ds = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = false // Bruger ikke første række som header
                }
            });
            // Liste der skal indeholde arbejdstimer
            List<ProjectHour> result = new();
            int row_no = 1;
            // Gennemløber alle rækker i Excel-arket
            while (row_no < ds.Tables[0].Rows.Count)
            {
                var row = ds.Tables[0].Rows[row_no];
                ProjectHour p = new ProjectHour();
                p.Medarbejder = row[0].ToString();
                p.Dato = DateTime.Parse(row[1].ToString());
                p.Stoptid = DateTime.Parse(row[2].ToString());
                p.Timer = Decimal.Parse(row[5].ToString());
                p.Type = row[6].ToString();
                p.Kostpris = Decimal.Parse(row[8].ToString());
                //Koden læser data fra bestemte kolonner i Excel og gemmer dem i et ProjectHour-objekt.
                // Tilføjer arbejdstimen til listen
                result.Add(p);
                // Logger arbejdstimen til konsollen
                Console.WriteLine($"dato= {p.Dato}, stop tid = {p.Stoptid} Timer = {p.Timer}, Type = {p.Type}  kostpris = {p.Kostpris}");
                // Går videre til næste række
                row_no++;
            }
            // Returnerer listen med arbejdstimer
            return result;
        }
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
            <NavLink class="nav-link" href="/" Match="NavLinkMatch.All">
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
                <NavLink class="nav-link" href="adminuser">
                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-people" viewBox="0 0 16 16">
                        <path d="M15 14s1 0 1-1-1-4-5-4-5 3-5 4 1 1 1 1zm-7.978-1L7 12.996c.001-.264.167-1.03.76-1.72C8.312 10.629 9.282 10 11 10c1.717 0 2.687.63 3.24 1.276.593.69.758 1.457.76 1.72l-.008.002-.014.002zM11 7a2 2 0 1 0 0-4 2 2 0 0 0 0 4m3-2a3 3 0 1 1-6 0 3 3 0 0 1 6 0M6.936 9.28a6 6 0 0 0-1.23-.247A7 7 0 0 0 5 9c-4 0-5 3-5 4q0 1 1 1h4.216A2.24 2.24 0 0 1 5 13c0-1.01.377-2.042 1.09-2.904.243-.294.526-.569.846-.816M4.92 10A5.5 5.5 0 0 0 4 13H1c0-.26.164-1.03.76-1.724.545-.636 1.492-1.256 3.16-1.275ZM1.5 5.5a3 3 0 1 1 6 0 3 3 0 0 1-6 0m3-2a2 2 0 1 0 0 4 2 2 0 0 0 0-4"/>
                    </svg> Se brugere
                </NavLink>
            </div>
        }
        
    </nav>
</div>




@code {
    // Variabel der styrer om menuen er åben eller lukket. Udelukkende til mobil for at den folder sig pænt sammen.
    private bool collapseNavMenu = true;

    // Hvis collapseNavManu er true, tilføjes CSS-klassen "collapse", som skjuler menuen
    private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

    // Metode der køres, når man klikker på menu-knappen eller et link. Vender værdien om fra sand til falsk eller omvendt
    private void ToggleNavMenu()
    {
        collapseNavMenu = !collapseNavMenu;
    }
    // Variabel til at gemme brugeren der er logget ind
    Users? loggedIn;

    // Kører når menuen indlæses
    protected override async Task OnInitializedAsync()
    {
        // Henter brugeren fra LocalStorage for at tjekke deres rolle.
        loggedIn = await LocalStorage.GetItemAsync<Users?>("user");
    }
}
</file>

<file path="Client/Pages/User/AddUser.razor">
@page "/adduser"
@using Core
@using Client.Service
@inject HttpClient http
@inject NavigationManager navMan

<PageTitle>Opret Bruger</PageTitle>

<div class="d-flex justify-content-center">
    <div class="create-con">
        <h3>Opret Ny Bruger</h3>

        <EditForm Model="@_user" OnValidSubmit="@HandleValidSubmit">
            <DataAnnotationsValidator />
            
            @* Viser kun fejl hvis der er nogen *@
            @if (context.GetValidationMessages().Any())
            {
                <div class="alert alert-danger">
                    <ValidationSummary />
                </div>
            }

            <div class="form-grid">
                <div class="form-floating-group">
                    <InputText id="UserName" @bind-Value="_user.UserName" class="form-control" placeholder=" " />
                    <label for="UserName">Brugernavn</label>
                </div>

                <div class="form-floating-group">
                    <InputText id="Password" type="password" @bind-Value="_user.Password" class="form-control" placeholder=" " />
                    <label for="Password">Adgangskode</label>
                </div>
            </div>

            <div class="form-floating-group">
                <InputSelect id="Role" @bind-Value="_user.Role" class="form-control form-select-styled">
                    @foreach (var r in roles)
                    {
                        <option value="@r">@r</option>
                    }
                </InputSelect>
                <label for="Role">Rolle</label>
            </div>

            <div class="d-flex gap-3 mt-4">
                <button type="submit" class="opretbtn">Opret Bruger</button>
                <button type="button" class="cancelbtn" @onclick="Cancel">Annuller</button>
            </div>
        </EditForm>
    </div>
</div>

@code {
    private Users _user = new() { Role = "Bruger" }; // Sætter default rolle
    private string[] roles = ["Bruger", "Admin"];

    private async Task HandleValidSubmit()
    {
        // Sender POST request direkte til API'et
        var response = await http.PostAsJsonAsync($"{Server.Url}/api/user", _user);

        if (response.IsSuccessStatusCode)
        {
            _user = new Users();
            navMan.NavigateTo("adminuser");
        }
    }

    private void Cancel()
    {
        navMan.NavigateTo("adminuser");
    }
}
</file>

<file path="Client/Pages/User/AdminUserPage.razor">
@page "/adminuser"
@using Core
@using Client.Service
@inject HttpClient Http
@inject NavigationManager Nav


<div class="admin-container">
    <div class="header-section">
        <h3>Brugeradministration</h3>
        <button class="btn btn-create" @onclick="CreateUser">
            <i class="bi bi-plus-lg"></i> Opret ny bruger
        </button>
    </div>

    @if (_users == null)
    {
        <div class="loading-state">
            <div class="spinner-border text-primary" role="status"></div>
            <p>Indlæser brugere...</p>
        </div>
    }
    else if (_users.Count == 0)
    {
        <div class="empty-state">
            <p>Ingen brugere fundet i systemet.</p>
        </div>
    }
    else
    {
        <div class="table-responsive shadow-sm rounded">
            <table class="table table-hover mb-0">
                <thead class="table-dark">
                    <tr>
                        <th>ID</th>
                        <th>Brugernavn</th>
                        <th>Rolle</th>
                        <th class="text-end">Handling</th>
                    </tr>
                </thead>
                <tbody>
                    @foreach (var user in _users)
                    {
                        <tr>
                            <td>#@user.UserId</td>
                            <td>
                                <span class="fw-bold">@user.UserName</span>
                            </td>
                            <td>
                                @* Giver rollen en lille badge for bedre visuelt overblik *@
                                <span class="badge @GetRoleBadgeClass(user.Role)">
                                    @user.Role
                                </span>
                            </td>
                            <td class="text-end">
                                <button class="btn btn-delete" 
                                        @onclick="() => DeleteUser(user.UserId)">
                                    <i class="bi bi-trash"></i> Slet
                                </button>
                            </td>
                        </tr>
                    }
                </tbody>
            </table>
        </div>
    }
</div>

@code {
    private List<Users>? _users = new();

    protected override async Task OnInitializedAsync()
    {
        _users = await Http.GetFromJsonAsync<List<Users>>($"{Server.Url}/api/user");
    }

    private async Task DeleteUser(int id)
    {
        var response = await Http.DeleteAsync($"{Server.Url}/api/user/delete/{id}");
    
        if (response.IsSuccessStatusCode)
        {
            _users = await Http.GetFromJsonAsync<List<Users>>($"{Server.Url}/api/user");
        }
    }

    // Hjælpemetode til at farve badges baseret på rolle
    private string GetRoleBadgeClass(string role)
    {
        return role.ToLower() switch
        {
            "admin" => "bg-danger",
            "bruger" => "bg-primary",
            _ => "bg-secondary"
        };
    }

    private void CreateUser()
    {
        Nav.NavigateTo("adduser");
    }

}
</file>

<file path="Client/Service/Server.cs">
namespace Client.Service
{
    public class Server
    {
        //public const string Url = "https://larsenserver.azurewebsites.net";
        public const string Url = "http://localhost:5028";
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

<file path="Server/Controllers/ProjectController/ProjectController.cs">
using Core;
using Microsoft.AspNetCore.Mvc;
using Server.Repositories.ProjectRepositories;
using Server.Service;
namespace Server.Controllers.ProjectController
{
    [ApiController] // Gør klassen til en API-controller
    [Route("api/project")] // Base endpoint
    public class ProjectController : ControllerBase 
    {
        private readonly IProjectRepository _projectRepo;  // Adgang til projekter
        private readonly ProjectCalculationsService _calcService; // Projektberegninger
        public ProjectController(IProjectRepository projectRepo, ProjectCalculationsService calcService) 
        {
            _projectRepo = projectRepo;
            _calcService = calcService;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Project>> GetAll()  // Henter alle projekter
        {
            return Ok(_projectRepo.GetAll()); // Returnerer projektliste
        }
        [HttpGet("{id}")] // Henter detaljer for et projekt
        public ActionResult<Calculation> GetDetails(int id) // Beregner projekt
        {
            var result = _calcService.CalculateProject(id);
            if (result == null) return NotFound("Project not found"); // Hvis projekt ikke findes
            return Ok(result);  // Returnerer beregning
        }
        [HttpPost]// Opretter nyt projekt
        public IActionResult Create(Project pro)
        {
            int newId = _projectRepo.Create(pro); // Gemmer projekt
            return Ok(newId);  // Returnerer nyt id
        }
        [HttpPut("{id}")] // Opdaterer eksisterende projekt
        public IActionResult Update(int id, [FromBody] Project pro)
        {
            if (id != pro.ProjectId) return BadRequest("ID mismatch"); // Tjekker id matcher
            _projectRepo.Update(pro);  // Opdaterer projekt
            return Ok();
        }
        [HttpDelete("{id}")] // Sletter projekt
        public void Delete(int id)
        {
            _projectRepo.Delete(id);   // Fjerner projektet med givne id 
        }
    }
}
</file>

<file path="Server/Repositories/User/CreateUserRepoSQL.cs">
using System.Data.Common;
using Core;                   
//forklar hvorfor der bruges using
namespace Server.Repositories.User
{
    public class CreateUserRepoSQL : BaseRepository, ICreateUserRepo
    {
    public List<Users> GetAll()
        {
            var result = new List<Users>();    // Tom liste til brugere der hentes
            // Connection string: beskriver hvordan man forbinder til databasen
            using var mConnection = GetConnection(); //Bruger using for at sikre at forbindelsen bliver lukket(Using = køre og sørger for at det bliver lukket)
            mConnection.Open(); // Åbner forbindelsen
            {
                var command = mConnection.CreateCommand();   // Opretter SQL-kommando
                command.CommandText = @"SELECT * FROM Users"; // SQL der henter alle brugere
                using (var reader = command.ExecuteReader())  // Kører SELECT og får en "reader" (Læser kolloner) 
                {
                    while (reader.Read()) // Læser én række ad gangen
                    {
                        // Henter værdier fra kolonnerne i rækken
                        var userid = reader.GetInt32(0);
                        var username = reader.GetString(1);
                        var password = reader.GetString(2);
                        var role = reader.GetString(3);
                        // Mapper database-værdier til et Users-objekt (lægger data ind i C# objekt)
                        Users u = new Users
                        {
                            UserId = userid,
                            UserName = username,
                            Password = password,
                            Role = role
                        };
                        result.Add(u); // Tilføjer bruger til listen
                    }
                }
            }
            return result; // Returnerer hele listen
        }
        public void Add(Users user)
        {
            // Connection string: beskriver hvordan man forbinder til databasen
            using var mConnection = GetConnection();
            mConnection.Open(); // Åbner forbindelsen
            {
                var command = mConnection.CreateCommand(); // Ny SQL-kommando
                // SQL med parametre
                command.CommandText =
                    "INSERT INTO Users (username, password, role) VALUES (@username, @password, @role)";
                Console.WriteLine(command.CommandText);
                // Opretter parameter for @username
                var paramName = command.CreateParameter();
                paramName.ParameterName = "username"; // Navn der matcher @username
                command.Parameters.Add(paramName); // Tilføjer til SQL-kommandos parameterliste
                paramName.Value = user.UserName; // Sætter værdien
                // Opretter parameter for @password
                var paramPassword = command.CreateParameter();
                paramPassword.ParameterName = "password";
                command.Parameters.Add(paramPassword);
                paramPassword.Value = user.Password;
                // Opretter parameter for @role
                var paramRole = command.CreateParameter();
                paramRole.ParameterName = "role";
                command.Parameters.Add(paramRole);
                paramRole.Value = user.Role;
                command.ExecuteNonQuery();   // Eksekverer INSERT (kører SQL-kommandoen)
            }
        }
        public Users? ValidateUser(string username, string password)
        {
            // Finder første bruger der matcher username og password (efter GetAll har hentet alle)
            return GetAll().FirstOrDefault(u => u.UserName == username && u.Password == password);
        }
        public void Delete(int id)
        {
            // Connection string: beskriver hvordan man forbinder til databasen
            using var mConnection = GetConnection();
            mConnection.Open(); // Åbner forbindelsen
            {
                var command = mConnection.CreateCommand(); // Ny SQL-kommando
                command.CommandText = $"DELETE FROM users WHERE userid={id}"; // SQL der sletter ud fra id
                command.ExecuteNonQuery();   // Kører DELETE-kommandoen
            }
        }
    }
}
</file>

<file path="Server/Service/MaterialConverter.cs">
using Core;
using ExcelDataReader; // Bruges til at læse excel filer
using System.IO;
using System.Runtime.Intrinsics.X86;
using System.Text;
namespace Server.Service
{
    public class MaterialConverter
    {
        // Konverterer Excel-stream til liste af ProjectMaterial
        public static List<ProjectMaterial> Convert(Stream stream)
        {
            // Registrerer encoding provider (kræves til ExcelDataReader)
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            // Opretter Excel reader til .xlsx filer
            IExcelDataReader reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            // Læser Excel-data ind i et DataSet
            var ds = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = false
                }
            });
            // Liste der skal indeholde materialer
            List<ProjectMaterial> result = new();
            int row_no = 1; // starter ved række 1 
            // Gennemløber alle rækker i Excel-arket
            while (row_no < ds.Tables[0].Rows.Count)
            {
                // Henter nuværende række
                var row = ds.Tables[0].Rows[row_no];
                ProjectMaterial p = new ProjectMaterial(); // Opretter nyt ProjectMaterial objekt
                p.Beskrivelse = row[1].ToString();
                p.Kostpris = Decimal.Parse(row[2].ToString());
                p.Antal = Decimal.Parse(row[4].ToString());
                p.Leverandør = row[8].ToString();
                p.Total = Decimal.Parse(row[17].ToString());
                p.Avance = Decimal.Parse(row[19].ToString()); // Læser værdier fra valgte kolonner og gemmer dem i objektet
                p.Dækningsgrad = string.IsNullOrEmpty(row[20].ToString()) ? 0 : decimal.Parse(row[20].ToString());
                // Tilføjer materialet til listen
                result.Add(p);
                // Logger materialet til konsollen
                Console.WriteLine($"Beskrivelse= {p.Beskrivelse}, Kostpris = {p.Kostpris} Antal = {p.Antal}, Total = {p.Total}  Avance = {p.Avance}, dækningsgrad = {p.Dækningsgrad}");
                // Går videre til næste række
                row_no++;
            }
            // Returnerer listen med materialer
            return result;
        }
    }
}
</file>

<file path="Client/Components/Projects/ProjectGrid.razor">
@*@using Core


<div class="ads-grid">
    @if (Projects == null || !Projects.Any())
    {
        <p></p>
    }
    else
    {
        @foreach (var a in Projects)
        {
            <ProjectComponent Project="a" OnDelete="OnDelete" />
        }
    }
</div>

@code 
{
    [Parameter] public IEnumerable<Project>? Projects { get; set; }
    [Parameter] public EventCallback<int> OnDelete { get; set; }
}*@
</file>

<file path="Core/Calculation.cs">
using System.Collections.Generic;
namespace Core
{
    public class Calculation
    {
        public int CalcId { get; set; }
        public int ProjectId { get; set; }
        // Stamdata
        public Project Project { get; set; }
        // Rå data (kan evt. udelades hvis du kun vil sende de grupperede, men vi beholder dem for en sikkerheds skyld)
        public List<ProjectHour> Hours { get; set; } = new();
        public List<ProjectMaterial> Materials { get; set; } = new();
        // --- NYT: Færdigsorterede lister fra Serveren ---
        public List<HourGroupDto> GroupedHours { get; set; } = new();
        public List<ProjectMaterial> GroupedMaterialsClientView { get; set; } = new(); // Kategoriseret (Belysning, Kabler...)
        public List<ProjectMaterial> GroupedMaterialsInternView { get; set; } = new(); // Leverandør/Navn
        // Beregninger
        public decimal TotalKostPrisMaterialer { get; set; }
        public decimal TotalPrisMaterialer { get; set; }
        public decimal TotalTimer { get; set; }
        public decimal TotalKostPrisTimer { get; set; }
        public decimal TotalPrisTimer { get; set; }
        // Samlet
        public decimal SamletKostPris => TotalKostPrisMaterialer + TotalKostPrisTimer;
        public decimal SamletTotalPris => TotalPrisMaterialer + TotalPrisTimer;
        public decimal Dækningsgrad => SamletTotalPris > 0 ? (Dækningsbidrag / SamletTotalPris) * 100 : 0;
        public decimal Dækningsbidrag => SamletTotalPris - SamletKostPris;
        public string Type { get; set; }
        public decimal TotalHours { get; set; }
    }
    // Lille hjælpeklasse til time-gruppering
    public class HourGroupDto
    {
        public string Type { get; set; } = "";
        public decimal Total { get; set; }
    }
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
    public int SvendTimePris { get; set; } = 572;
    public int LærlingTimePris { get; set; } = 395;
    public int KonsulentTimePris { get; set; } = 995;
    public int ArbejdsmandTimePris { get; set; } = 515;
}
</file>

<file path="Server/Controllers/ProjectHoursController.cs">
using Core;
using Microsoft.AspNetCore.Mvc;
using Server.Repositories.HourRepositories;
using Server.Service; 
namespace Server.Controllers
{
    [ApiController]
    [Route("api/projecthours")]
    public class ProjectHoursController : ControllerBase
    {
        private readonly IHourRepository _hourRepo;
        // Repository som gemmer timer i databasen
        public ProjectHoursController(IHourRepository hourRepo)
        {
            _hourRepo = hourRepo;
        }
        /*[HttpGet]
        public IEnumerable<Calculation> GetOverview()
        {
            return _hourRepo.GetTotalHoursGroupedByType();
        }*/
        [HttpPost("upload")]
        //
        public IActionResult Upload(IFormFile? file, [FromQuery] int projectId)
        {
            if (file == null || file.Length == 0) return BadRequest("No file");
            try 
            {
                // Opretter en midlertidig hukommelse (stream)
                using Stream s = new MemoryStream();
                file.CopyTo(s); // Kopierer filens indhold ind i hukommelsen
                s.Position = 0; // starter læsningen fra begyndelsen af filen 
                //konverterer excel filen til material objekter 
                var hours = WorkerConverter.Convert(s);
                foreach (var h in hours)
                {
                    //sætter materialer til projekter
                    h.ProjectId = projectId;
                    // gemmer materialet til db 
                    _hourRepo.Add(h);
                }
                return Ok($"Uploaded {hours.Count} hours.");
            }
            catch (Exception ex)
            {
                return BadRequest("Error parsing file: " + ex.Message);
            }
        }
    }
}
</file>

<file path="Server/Program.cs">
/*using Server.Repositories.Proj.CreateProjectsFolder;
using Server.Repositories.Proj.HourCalculator;*/
using Server.Repositories.User;
using Server.Repositories.HourRepositories;
using Server.Repositories.MaterialRepositories;
using Server.Repositories.ProjectRepositories;
using Server.Service;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
builder.Services.AddScoped<IProjectRepository, ProjectRepositorySQL>();
builder.Services.AddScoped<IHourRepository, HourRepositorySQL>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepositorySQL>();
builder.Services.AddScoped<ProjectCalculationsService>();
builder.Services.AddSingleton<ICreateUserRepo, CreateUserRepoSQL>();
/*builder.Services.AddSingleton<ICreateProjectRepo, CreateProjectRepo>();
builder.Services.AddSingleton<IProjectHourCalcRepo, ProjectHourCalcRepositorySQL>();*/
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

<file path="Client/Pages/Project/EditProjectPage.razor">
@using Core
@using Client.Service
@inject HttpClient http
@inject NavigationManager Nav
@page "/project/edit/{ProjectId:int}" @* Vigtig at URL indeholder en variabel, så variablen gemmes som et ProjectID*@

<div class="create-page-back">
<div class="create-con">
<h3>Redigere projekt</h3>

@* Når siden startes, er _project null, fordi data fra serveren ikke er kommet endnu.
Derfor vises ventetekst, så siden ikke crasher ved at prøve at vise tomme data *@
@if (_project == null)
{
    <p><em>Indlæser projektdata...</em></p>
}
else
{
    /* Binder formularen til '_project' objektet. Når brugeren trykker 'Gem', køres metoden 'HandleSubmit'*/
        <EditForm Model="@_project" OnValidSubmit="HandleSubmit">
                <DataAnnotationsValidator/>
                <ValidationSummary/>

                <div class="form-floating-group">
                    <InputText id="name" class="form-control" @bind-Value="_project.Name"/>
                    <label for="name">Navn</label>
                </div>
                <div class="form-floating-group">
                    <InputText id="billede" class="form-control" @bind-Value="_project.ImageUrl"/>
                    <label for="billede">Billede link</label>
                </div>

                <div class="form-grid">
                    <div class="form-floating-group">
                        <InputNumber id="svend" class="form-control" @bind-Value="_project.SvendTimePris"/>
                        <label for="svend">Svend sats</label>
                    </div>
                    <div class="form-floating-group">
                        <InputNumber id="lærling" class="form-control" @bind-Value="_project.LærlingTimePris"/>
                        <label for="lærling">Lærling sats</label>
                    </div>
                    <div class="form-floating-group">
                        <InputNumber id="konsulent" class="form-control" @bind-Value="_project.KonsulentTimePris"/>
                        <label for="konsulent">Konsulent sats</label>
                    </div>
                    <div class="form-floating-group">
                        <InputNumber id="arbejdsmand" class="form-control" @bind-Value="_project.ArbejdsmandTimePris"/>
                        <label for="konsulent">Arbejdsmand sats</label>
                    </div>
                </div>

                <div class="">
                    <div class="">
                        <button type="submit" class="opretbtn">Gem ændringer</button>
                        <button type="button" class="cancelbtn" @onclick="Cancel">Annuller</button>
                    </div>
                </div>

            </EditForm>
}
</div>
</div>

@code {
    // Property der automatisk får værdien fra URL'en (f.eks. 5)
    [Parameter] public int ProjectId { get; set; }
    // Her gemmes de data vi redigerer. Starter som 'null'
    private Project?  _project;
    
    private string? errorText;

    // 1. HENT DATA (Kører når siden åbner)
    protected override async Task OnInitializedAsync()
    {
        // Vi spørger serveren: "Giv mig alt info om projekt nr. [ProjectId]"
        // Serveren returnerer en 'Calculation', som indeholder 'Project'-objektet.
        var result = await http.GetFromJsonAsync<Calculation>($"{Server.Url}/api/project/{ProjectId}");
        
        if (result != null)
        { 
            // Vi trækker selve projekt-delen ud og lægger den i vores variabel,
            // så formularen ovenfor kan vise dataene.
            _project = result.Project;
        }
    }
    
    // 2. GEM DATA (Kører når man trykker "Gem ændringer")
    private async Task HandleSubmit()
    {
        if (_project == null) return;

        // Sikkerhedsforanstaltning: Vi sikrer os, at ID'et i objektet matcher URL'en.
        _project.ProjectId = ProjectId;

        try
        {
            // Her bruger vi 'PutAsJsonAsync'.
            // PUT bruges normalt til at OPDATERE/RETTE noget eksisterende.
            var res = await http.PutAsJsonAsync($"{Server.Url}/api/project/{ProjectId}", _project);

            // Hvis serveren siger OK (status 200), sender vi brugeren til forsiden.
            if (res.IsSuccessStatusCode)
            {
                Nav.NavigateTo("/");
                return;
            }

            // HVIS FEJL: Læs fejlbeskeden fra serveren, så vi kan se, hvad der gik galt.
            var body = await res.Content.ReadAsStringAsync();
            errorText = $"Fejl ved opdatering: {(int)res.StatusCode} {res.ReasonPhrase}. Server siger: {body}";
            Console.WriteLine(errorText);
        }
        catch (HttpRequestException hrEx)
        {
            errorText = "Netværksfejl: " + hrEx.Message;
            Console.WriteLine(errorText);
        }
        catch (Exception ex)
        {
            errorText = "Uventet fejl: " + ex.Message;
            Console.WriteLine(errorText);
        }
    }
    
    // Knappen "Annuller" sender bare brugeren til forsiden
    private void Cancel()
    {
        Nav.NavigateTo("/");
    }
}
</file>

<file path="Server/Service/ProjectCalculationsService.cs">
using Core;
using Server.Repositories.HourRepositories;
using Server.Repositories.MaterialRepositories;
using Server.Repositories.ProjectRepositories;
using System.Globalization;
namespace Server.Service;
public class ProjectCalculationsService
{
    // Vi bruger Dependency Injection til at få adgang til vores 3 repositories.
    // Dette gør, at servicen ikke selv skal vide, hvordan man snakker SQL, 
    // men blot beder om data.
    private readonly IProjectRepository _projectRepo;
    private readonly IHourRepository _hourRepo;
    private readonly IMaterialRepository _materialRepo;
    public ProjectCalculationsService(
        IProjectRepository projectRepo,
        IHourRepository hourRepo,
        IMaterialRepository materialRepo)
    {
        _projectRepo = projectRepo;
        _hourRepo = hourRepo;
        _materialRepo = materialRepo;
    }
    // Hovedmetoden: Tager et projekt-ID og returnerer et færdigt 'Calculation' objekt
    // med alle tal lagt sammen og sorteret.
    public Calculation? CalculateProject(int projectId)
    {
        // Vi henter stamdata, timer og materialer hver for sig.
        var project = _projectRepo.GetById(projectId);
        if (project == null) return null; // Stop hvis projektet ikke findes
        var hours = _hourRepo.GetByProjectId(projectId);
        var materials = _materialRepo.GetByProjectId(projectId);
        // Vi starter vores "Data Transfer Object", som skal sendes til Client.
        var dto = new Calculation
        {
            ProjectId = project.ProjectId,
            Project = project,
            Hours = hours, // Rå liste
            Materials = materials // Rå liste
        };
        //Vi løber igennem alle materialelinjer for at finde total kostpris (indkøb) 
        // og total salgspris.
        foreach (var m in materials)
        {
            dto.TotalKostPrisMaterialer += (m.Kostpris * m.Antal);
            dto.TotalPrisMaterialer += m.Total; // "Total her kommer fra excel filen"
        }
        // Vi grupperer timerne per medarbejder, da en medarbejder kan have flere timeregistreringer.
        var employeeGroups = hours.GroupBy(x => x.Medarbejder);
        foreach (var group in employeeGroups)
        {
            // Find medarbejderens "Basistype" (Svend, Lærling, osv.)
            // Vi kigger på alle deres timer og ser bort fra "overtid" for at finde grundtypen.
            // Hvis intet er fundet, antager vi det er en "svend".
            var normalType = group
                .FirstOrDefault(h => h.Type != null && !h.Type.ToLower().Contains("overtid"))?
                .Type?.ToLower() ?? "svend";
            // Find den timepris, der er aftalt på selve projektet fra Project-tabellen
            decimal grundSats = 0;
            if (normalType.Contains("lærling")) grundSats = project.LærlingTimePris;
            else if (normalType.Contains("konsulent")) grundSats = project.KonsulentTimePris;
            else if (normalType.Contains("arbejdsmand")) grundSats = project.ArbejdsmandTimePris;
            else grundSats = project.SvendTimePris;
            // Nu beregner vi prisen for hver time-registrering
            foreach (var h in group)
            {
                decimal faktor = 1.0m;
                string typeLower = h.Type?.ToLower() ?? "";
                // Håndter overtidstillæg (f.eks. 50% eller 100% ekstra)
                if (typeLower.Contains("overtid 1")) faktor = 1.5m;
                else if (typeLower.Contains("overtid 2")) faktor = 2.0m;
                // Beregn salgspris: Antal timer * Timepris fra projektet * Overtidsfaktor
                decimal salgsPris = h.Timer * grundSats * faktor;
                dto.TotalPrisTimer += salgsPris;
                dto.TotalKostPrisTimer += h.Kostpris;
            }
        }
        // Simpel summering af antal timer totalt
        dto.TotalTimer = dto.Hours.Sum(h => h.Timer);
        //  5. LOGIK FLYTTET FRA CLIENT TIL SERVER: GRUPPERING
        // A. Gruppering af Timer (Svend, Lærling osv.)
        dto.GroupedHours = hours // litse med timer 
            .GroupBy(h => { // grupperer timer 
                var t = h.Type?.ToLower() ?? ""; // Henter typer og gøre det til lowercase  
                if (t.Contains("overtid 1")) return "Overtid 1";
                if (t.Contains("overtid 2")) return "Overtid 2";
                if (t.Contains("lærling")) return "Lærling";
                if (t.Contains("konsulent")) return "Konsulent";
                if (t.Contains("arbejdsmand")) return "Arbejdsmand";
                return "Svend";
            })
            .Select(g => new HourGroupDto { Type = g.Key, Total = g.Sum(x => x.Timer) }) //gruppens navn og total timer 
            .OrderByDescending(x => x.Total) //sorterer efter flest timer 
            .ToList();
        // B. Gruppering af Materialer (KUNDEVISNING - Kategorier)
        var categories = new Dictionary<string, string[]>
        {
            { "Belysning", new[] { "spot", "lampe", "led", "lys", "armatur", "pendel", "driver", "dæmper", "skinne" } },
            { "Kabler & Rør", new[] { "kabel", "ledning", "rør", "nkt", "pvi", "flex", "5x1,5", "3x1,5", "5x2,5" } },
            { "Installation", new[] { "stikkontakt", "afbryder", "underlag", "dåse", "fuga", "opus", "ramme", "tangent" } },
            { "Sikringer & Tavler", new[] { "tavle", "sikring", "hpfi", "rce", "automatsikring" } }
        };
        dto.GroupedMaterialsClientView = materials // liste med materialer 
            .GroupBy(m => { //grupperer materialer 
                string desc = m.Beskrivelse?.ToLower() ?? "";
                // Find første kategori der matcher
                foreach (var category in categories)
                {
                    if (category.Value.Any(keyword => desc.Contains(keyword))) return category.Key; // Matcher søgeord og retunerer kategori 
                }
                return "Øvrige materialer"; // standard kategori 
            })
            .Select(g => new ProjectMaterial // opretter nyt objekt 
            {
                Beskrivelse = g.Key, //kategoinavn 
                Total = g.Sum(x => x.Total) //samlet pris
            })
            .OrderByDescending(m => m.Total) // efter pris 
            .ToList();
        // C. Gruppering af Materialer (INTERN VISNING - Leverandør/Navn)
        // Kendte leverandører
        string[] knownSuppliers = { "Anker & Co", "Solar", "Lemvigh-Müller", "AO" }; //leverandører
        TextInfo textInfo = new CultureInfo("da-DK", false).TextInfo;
        dto.GroupedMaterialsInternView = materials
            .GroupBy(m => {
                string desc = m.Beskrivelse?.Trim() ?? ""; //henter beskrivelse 
                // Match på leverandør
                foreach (var supplier in knownSuppliers) //gennemløber leverandør 
                {
                    if (desc.Contains(supplier, StringComparison.OrdinalIgnoreCase)) return supplier; // matcher leverandør og return
                }
                return desc.ToLower();
            })
            .Select(g => new ProjectMaterial // opretter nyt objekt 
            {
                Beskrivelse = knownSuppliers.Contains(g.Key) ? g.Key : textInfo.ToTitleCase(g.Key), 
                Total = g.Sum(x => x.Total)
            })
            .OrderByDescending(m => m.Total) //Sorterer efter pris 
            .ToList();
        return dto; // retunerer dto (timer + materialer) 
    }
}
</file>

<file path="Client/Components/Projects/ProjectComponent.razor">
@*@using Core
@inject NavigationManager Nav



<article class="ad-card">
    <div class="ad-media">
        <img src="@Project?.ImageUrl" alt="Project billede" />
        <div class="price-badge">
            <button class="dots-btn" @onclick="ToggleMenu" @onfocusout="CloseMenuDelayed">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-three-dots" viewBox="0 0 16 16">
                    <path d="M3 9.5a1.5 1.5 0 1 1 0-3 1.5 1.5 0 0 1 0 3m5 0a1.5 1.5 0 1 1 0-3 1.5 1.5 0 0 1 0 3m5 0a1.5 1.5 0 1 1 0-3 1.5 1.5 0 0 1 0 3"/>
                </svg>
            </button>
            
            @if (_showMenu)
            {
                <div class="dropdown-menu">
                    <button class="dropdown-item" @onclick="GotoEditPage">Rediger</button>
                    <button class="dropdown-item danger" @onclick="DeleteProject">Slet projekt</button>
                </div>
            }
        </div>
    </div>

    <div class="ad-body">
        <div class="ad-meta">Oprettet: @Project.DateCreated</div>
        <div class="ad-title">@Project.Name</div>
    </div>

    <div class="ad-footer">
        <button class="MoreBtn" @onclick="() => GotoDetailPage()" >Se detaljer</button>
    </div>
</article>

@code {
    [Parameter] public Project? Project { get; set; }
    [Parameter] public EventCallback<int> OnDelete { get; set; }
    
    private Task GotoDetailPage()
    {
        var id = Project.ProjectId;
        Nav.NavigateTo($"project/{id}");
        return Task.CompletedTask;
    }
    
    private Task GotoEditPage()
    {
        var id = Project?.ProjectId;
        Nav.NavigateTo($"project/edit/{id}");
        return Task.CompletedTask;
    }

    private bool _showMenu = false;

    private void ToggleMenu()
    {
        _showMenu = !_showMenu;
    }
    
    private async Task CloseMenuDelayed()
    {
        await Task.Delay(200); 
        _showMenu = false;
    }

    private async Task DeleteProject()
    {
        if (OnDelete.HasDelegate) await OnDelete.InvokeAsync(Project.ProjectId);
    }
    
    
}*@
</file>

<file path="Client/Pages/Project/ProjectPage.razor">
@page "/"
@using Core
@using Client.Service
@inject HttpClient Http
@inject NavigationManager Nav

<div class="admin-container">
    <div class="header-section">
        <h3>Mine projekter</h3>
    </div>

    @* _projects starter med at være 'null'. Så længe den er det, vises en spinner. *@
    @if (_projects == null)
    {
        <div class="loading-state">
            <div class="spinner-border text-primary" role="status"></div>
            <p>Indlæser projekter...</p>
        </div>
    }
    @* Hvis listen er hentet, men den er tom (0 projekter), vises en besked. *@
    else if (_projects.Count == 0)
    {
        <div class="empty-state">
            <p>Ingen projekter fundet.</p>
        </div>
    }
    else
    {
        <div class="table-card">
            <table class="clean-table">
                <tbody>
                @* Kører et loop igennem alle projekter i listen *@
                @foreach (var project in _projects)
                {
                    <tr>
                        @* BILLEDE OG NAVN *@
                        <td class="cell-main">
                            <div class="project-flex">
                                @* Hvis billed-linket er dødt eller tomt,
                                       så indlæser den automatisk et gråt placeholder-billede i stedet. *@
                                <img src="@project.ImageUrl" alt="img"
                                     onerror="this.src='https://placehold.co/100?text=No+Img'"/>
                                <div class="text-group">
                                    <span class="name">@project.Name</span>
                                    <span class="sub-id">ID: #@project.ProjectId</span>
                                </div>
                            </div>
                        </td>

                        @* DATO *@
                        <td class="cell-date">
                            <span class="date-tag">
                                @project.DateCreated.ToString("dd. MMM yyyy")
                            </span>
                        </td>

                        @* KNAPPER *@
                        <td class="cell-action">
                            <div class="action-flex">
                                @* Knap til at se detaljer (man sendes til ProjectDetailsPage) *@
                                <button class="btn-simple" @onclick="() => GotoDetails(project.ProjectId)">
                                    Se detaljer
                                </button>

                                @* Den lille menu (3 prikker) *@
                                <div class="menu-container">
                                    <button class="btn-dots"
                                            @onclick="() => ToggleMenu(project.ProjectId)"
                                            @onfocusout="CloseMenuDelayed">
                                        @* Ikon kode for tre prikker ... *@
                                        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-three-dots" viewBox="0 0 16 16">
                                            <path d="M3 9.5a1.5 1.5 0 1 1 0-3 1.5 1.5 0 0 1 0 3m5 0a1.5 1.5 0 1 1 0-3 1.5 1.5 0 0 1 0 3m5 0a1.5 1.5 0 1 1 0-3 1.5 1.5 0 0 1 0 3"/>
                                        </svg>
                                    </button>

                                    @* Her tjekkes: Er det lige præcis dette projekt, der har menuen åben?
                                            Vi bruger ID'et til at styre det, så ikke alle menuer åbner samtidig. *@
                                    @if (_activeMenuId == project.ProjectId)
                                    {
                                        <div class="dropdown-popover">
                                            <button @onclick="() => GotoEdit(project.ProjectId)">
                                                <i class="bi bi-pencil"></i> Rediger
                                            </button>
                                            <button class="danger" @onclick="() => DeleteProject(project.ProjectId)">
                                                <i class="bi bi-trash"></i> Slet
                                            </button>
                                        </div>
                                    }
                                </div>
                            </div>
                        </td>
                    </tr>
                }
                </tbody>
            </table>
        </div>
    }
</div>

@code {
    // Listen over projekter. Den er null indtil der er hentet data.
    private List<Project>? _projects;
    // Holder styr på, hvilket projekt der har sin "3-prikker-menu" åben lige nu.
    // Hvis den er null, er ingen menuer åbne.
    private int? _activeMenuId = null;

    // 1. HENT PROJEKTER (Kører når siden starter)
    protected override async Task OnInitializedAsync()
    {
        try 
        {
            // Henter listen fra API'et: api/project
            _projects = await Http.GetFromJsonAsync<List<Project>>($"{Server.Url}/api/project");
        }
        catch (Exception ex)
        {
            // Hvis serveren er nede, skrives fejlen i browserens konsol
            Console.WriteLine($"Fejl: {ex.Message}");
        }
    }
    
    // Navigations-hjælpere
    private void GotoDetails(int id) => Nav.NavigateTo($"project/{id}");
    private void GotoEdit(int id) => Nav.NavigateTo($"project/edit/{id}");

    // 2. Åben/Luk menu
    // Hvis man klikker på samme ID igen, lukkes menuen (null).
    // Hvis man klikker på et nyt ID, åbnes den menu (id).
    private void ToggleMenu(int id) => _activeMenuId = (_activeMenuId == id) ? null : id;

    // 3. Luk menu ved klik udenfor
    // Når musen forlader knappen (focus out), lukker vi menuen.
    // Vi venter 200ms (Delay), så brugeren når at registrere et klik på "Slet" eller "Rediger", inden menuen forsvinder helt.
    private async Task CloseMenuDelayed()
    {
        await Task.Delay(200);
        _activeMenuId = null;
    }

    // 4. Slet projekt
    private async Task DeleteProject(int id)
    {
        // Beder serveren slette projektet
        var response = await Http.DeleteAsync($"{Server.Url}/api/project/{id}");
        // Hvis det lykkedes...
        if (response.IsSuccessStatusCode)
        {
            // ...så hentes hele listen igen, så det slettede projekt forsvinder fra skærmen.
            _projects = await Http.GetFromJsonAsync<List<Project>>($"{Server.Url}/api/project");
            _activeMenuId = null;
        }
    }
}
</file>

<file path="Client/Components/Create project/CreateProjectComponent.razor">
@using Core
@using Client.Service
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
            <div class="form-floating-group">
                <InputText id="billede" class="form-control" @bind-Value="aProject.ImageUrl" />
                <label for="billede">Billede link</label>
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
                    <InputNumber id="arbejdsmand" class="form-control" @bind-Value="aProject.ArbejdsmandTimePris" />
                    <label for="arbejdsmand">Arbejdsmand sats</label>
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
        var response = await http.PostAsJsonAsync($"{Server.Url}/api/project", aProject);

        // 2. Tjek om oprettelsen gik godt (HTTP status 200-299)
        if (response.IsSuccessStatusCode)
        {
            // 3. Læs det nye ID som API'et returnerer 
            var newProjectId = await response.Content.ReadFromJsonAsync<int>();

            // 4. Hvis der er valgt en Time-fil, upload den nu med det nye ID
            if (timeFile != null)
            {
                await UploadFileToApi(timeFile, newProjectId, "api/projecthours/upload");
            }

            // 5. Hvis der er valgt en Materiale-fil, upload den nu med det nye ID
            if (materialeFile != null)
            {
                await UploadFileToApi(materialeFile, newProjectId, "api/projectmaterials/upload");
            }
        }

        // 6. Nulstil formularen og naviger brugeren tilbage til forsiden
        aProject = new();
        Nav.NavigateTo("/");
    }

    // Hjælpemetode til selve fil-uploaden 
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
        await http.PostAsync($"{Server.Url}/{endpoint}?projectId={projectId}", content);
    }
}
</file>

<file path="Client/Pages/Project/ProjectDetailsPage.razor">
@page "/project/{Id:int}"
@using Core
@using Client.Components.ProjectDetails
@using Client.Service
@inject HttpClient Http
@inject ProjectService ProjectService

<PageTitle>Projekt Detaljer</PageTitle>

@if (details == null)
{
    <p><em>Indlæser beregninger fra serveren...</em></p>
}
else
{
    <div class="container">
        <div class="d-flex justify-content-between align-items-center mb-4">
            <div>
                <h1>@details.Project.Name</h1>
                <span class="badge bg-secondary">Oprettet: @details.Project.DateCreated.ToShortDateString()</span>
            </div>

            <div class="form-check form-switch">
                <input class="form-check-input" type="checkbox" role="switch" id="clientViewSwitch"
                       @bind="showClientView">
                <label class="form-check-label switch-label" for="clientViewSwitch">
                    @(showClientView ? "Kundevisning: AKTIV" : "Kundevisning: INAKTIV")
                </label>
            </div>
        </div>

        <PrisOverblik Details="details" IsClientView="showClientView" />

        <RessourceOverblik HourGroups="details.GroupedHours"
                           MaterialGroups="details.GroupedMaterialsClientView"
                           IsClientView="showClientView" />

    </div>
}

@code {
    [Parameter] public int Id { get; set; } // Tager tallet fra URL'en f.eks. /project/5, bliver til id 5

    private Calculation? details;
    // Denne bool styrer "kunde visning" knappen. False = Virksomhedens oplysninger. True = Kundens oplysninger
    private bool showClientView = false;

    protected override async Task OnInitializedAsync()
    {
        // 1. Service henter data
        // 2. Serveren har allerede sorteret timer og materialer
        // 3. Vi gemmer det i 'details'
        details = await ProjectService.GetProjectDetails(Id);
        details = await Http.GetFromJsonAsync<Calculation>($"{Server.Url}/api/project/{Id}");
    }

   
}
</file>

</files>
