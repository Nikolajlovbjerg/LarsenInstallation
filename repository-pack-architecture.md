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
      BarChart.razor
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
      EditUserPage.razor
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

<file path="Core/Login.cs">
namespace Core;
public class Login
{
    public string UserName { get; set; }
    public string Password { get; set; }
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
        public string? Leverandør { get; set; }
        public decimal Avance { get; set; }
        public decimal Dækningsgrad { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
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

<file path="Client/Components/ProjectDetails/BarChart.razor">
@using Core
@namespace Client.Components.ProjectDetails

<div class="card shadow-sm mb-2" style="border-color: #071226;">
    <div class="card-header bg-transparent fw-bold" style="border-color: #071226; color: #071226;">
        Tidsforbrug over tid (Timer pr. måned)
    </div>
    <div class="card-body">
        @if (ChartData == null || !ChartData.Any())
        {
            <p class="text-muted text-center">Ingen tids data tilgængelig for grafen.</p>
        }
        else
        {
            @* Removed overflow-x and fixed width to ensure it stays within bounds *@
            <div class="chart-container" style="width: 100%; height: 250px;">
                <svg width="100%" height="100%" viewBox="0 0 @TotalWidth 250" preserveAspectRatio="none">
                    @{
                        double chartHeight = 180;
                        double maxVal = (double)ChartData.Max(x => x.TotalHours);
                        if (maxVal == 0) maxVal = 1; 
                        
                        // Dynamically adjust bar width and spacing based on count to keep it small
                        double barWidth = 30; 
                        double spacing = 15;
                        double startX = 40;
                    }

                    @for (int i = 0; i < ChartData.Count; i++)
                    {
                        var item = ChartData[i];
                        double barHeight = ((double)item.TotalHours / maxVal) * chartHeight;
                        double x = startX + (i * (barWidth + spacing));
                        double y = chartHeight - barHeight + 30;

                        <rect x="@x" y="@y" width="@barWidth" height="@barHeight" fill="#071226" rx="2">
                            <title>@item.MonthLabel: @item.TotalHours.ToString("N1") t</title>
                        </rect>

                        @* Explicit namespace to avoid RZ1023 error *@
                        <svg:text x="@(x + barWidth/2)" y="@(y - 5)" font-size="10" text-anchor="middle" fill="#071226" font-weight="bold">
                            @item.TotalHours.ToString("N0")
                        </svg:text>

                        <svg:text x="@(x + barWidth/2)" y="@(chartHeight + 50)" font-size="9" text-anchor="middle" fill="gray">
                            @item.MonthLabel
                        </svg:text>
                    }
                    
                    <line x1="20" y1="@(chartHeight + 30)" x2="@(TotalWidth - 20)" y2="@(chartHeight + 30)" stroke="#dee2e6" stroke-width="1" />
                </svg>
            </div>
        }
    </div>
</div>

@code {
    [Parameter] public List<ProjectHour> Hours { get; set; } = new();

    private List<MonthlySummary> ChartData = new();
    private double TotalWidth => ChartData.Count > 0 ? (ChartData.Count * 45) + 80 : 500;

    protected override void OnParametersSet()
    {
        if (Hours != null && Hours.Any())
        {
            ChartData = Hours
                .Where(h => h.Dato.HasValue)
                .GroupBy(h => new { h.Dato.Value.Year, h.Dato.Value.Month })
                .Select(g => new MonthlySummary
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalHours = g.Sum(h => h.Timer),
                    MonthLabel = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM yy")
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToList();
        }
    }

    private class MonthlySummary
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalHours { get; set; }
        public string MonthLabel { get; set; } = "";
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

<file path="Client/Pages/User/EditUserPage.razor">
@page "/adduser/edit/{UserId:int}"
@using Core
@using Client.Service
@inject HttpClient http
@inject NavigationManager navMan

<PageTitle>Opret Bruger</PageTitle>

<div class="d-flex justify-content-center">
    <div class="create-con">
        <h3>Redigere bruger</h3>

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
                <button type="submit" class="opretbtn">Gem ændringer</button>
                <button type="button" class="cancelbtn" @onclick="Cancel">Annuller</button>
            </div>
        </EditForm>
    </div>
</div>

@code {
    [Parameter] public int UserId { get; set; }
    
    private Users _user = new(); 
    private string[] roles = ["Bruger", "Admin"];

    // Kører når komponenten initialiseres
    protected override async Task OnInitializedAsync()
    {
        // Henter den specifikke bruger baseret på ID fra URL'en
        var response = await http.GetFromJsonAsync<Users>($"{Server.Url}/api/user/{UserId}");
        
        if (response != null)
        {
            _user = response;
        }
    }

    private async Task HandleValidSubmit()
    {
        var response = await http.PutAsJsonAsync($"{Server.Url}/api/user", _user);
        if (response.IsSuccessStatusCode)
        {
            navMan.NavigateTo("adminuser");
        }
    }

    private void Cancel()
    {
        navMan.NavigateTo("adminuser");
    }
}
</file>

<file path="Client/App.razor">
@inject Blazored.LocalStorage.ILocalStorageService LocalStorage
@inject NavigationManager Nav
@using Client.Pages

@if (!initialized)
{
    /* small placeholder while we read local storage */
    <div></div>
}
else if (!isLoggedIn)
{
    /* Render the LoginPage component directly if not logged in */
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
        // NYT: Færdigsorterede lister fra Serveren
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

<file path="Server/Repositories/User/ICreateUserRepo.cs">
using Core;
namespace Server.Repositories.User
{
    // Interface som beskriver hvilke metoder et "User Repository" skal have
    // Formål er at være en mellem mand imellem repo og controller
    public interface ICreateUserRepo
    {
        List<Users> GetAll();   // Skal kunne hente alle brugere
        Users GetById(int id);
        void Add(Users user);   // Skal kunne tilføje en ny bruger
        void Update(Users user);
        void Delete(int id);    // Skal kunne slette en bruger ud fra ID
        Users? ValidateUser(string username, string password);  // Skal kunne validere login
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
                // Går videre til næste række
                row_no++;
            }
            // Returnerer listen med materialer
            return result;
        }
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
                <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" fill="currentColor" class="bi 
                bi-door-closed" viewBox="0 0 16 16">
                    <path d="M3 2a1 1 0 0 1 1-1h8a1 1 0 0 1 1 1v13h1.5a.5.5 0 0 1 0 1h-13a.5.5 0 0 1 0-1H3zm1 13h8V2H4z"/>
                    <path d="M9 9a1 1 0 1 0 2 0 1 1 0 0 0-2 0"/>
                </svg>
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

@* Denne side har kun formålet, at give siden en URL, så man kan navigere til CreateProjectPage *@
<CreateProjectComponent></CreateProjectComponent>

@* Her er ingen kode, da det hele ligger i CreateProjectComponent *@
@code {
    
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
/* builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }); */
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<ProjectService>();
await builder.Build().RunAsync();
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
        [HttpGet("{id:int}")] 
        public ActionResult<Users> Get(int id)
        {
            var user = userRepo.GetById(id);
            if (user == null) return NotFound();
            return Ok(user);
        }
        [HttpPut] 
        public IActionResult Update([FromBody] Users user)
        {
            userRepo.Update(user);
            return Ok();
        }
    }
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
    void AddRange(List<ProjectHour> hours);
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
    void AddRange(List<ProjectMaterial> materials);
    // Metode til at hente alle ProjectMaterialer for et specifikt projekt
    // Returnerer en liste med ProjectMaterial-objekter
    List<ProjectMaterial> GetByProjectId(int projectId);
}
</file>

<file path="Server/Repositories/MaterialRepositories/MaterialRepositorySQL.cs">
using Core;
namespace Server.Repositories.MaterialRepositories;
// Repository til håndtering af projectmaterials i databasen
public class MaterialRepositorySQL : BaseRepository, IMaterialRepository
{
    // Tilføjer et nyt ProjectMaterial til databasen
    public void AddRange(List<ProjectMaterial> materials)
    {
        using var conn = GetConnection();
        conn.Open();
        using var transaction = conn.BeginTransaction();
        try
        {
            foreach (var m in materials)
            {
                using var command = conn.CreateCommand();
                command.CommandText = @"
                INSERT INTO projectmaterials
                    (projectid, beskrivelse, kostpris, antal, leverandør, total, avance, dækningsgrad) 
                VALUES 
                    (@pid, @besk, @kost, @antal, @lev, @total, @avance, @dg)";
                command.Parameters.AddWithValue("pid", m.ProjectId);
                command.Parameters.AddWithValue("besk", m.Beskrivelse ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("kost", m.Kostpris);
                command.Parameters.AddWithValue("antal", m.Antal);
                command.Parameters.AddWithValue("lev", m.Leverandør ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("total", m.Total);
                command.Parameters.AddWithValue("avance", m.Avance);
                command.Parameters.AddWithValue("dg", m.Dækningsgrad);
                command.ExecuteNonQuery();
            }
            transaction.Commit(); // All rows are saved in one go
        }
        catch (Exception)
        {
            transaction.Rollback(); // If one row fails, nothing is saved (data integrity)
            throw;
        }
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

<file path="Client/Components/Projects/ProjectGrid.razor">
@using Core


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
    // Initialiserer en ny bruger med standardrollen "Bruger"
    private Users _user = new() { Role = "Bruger" }; 
    private string[] roles = ["Bruger", "Admin"];

    // Køres kun når formularen er gyldig (valideret ok)
    private async Task HandleValidSubmit()
    {
        // Sender brugerdata som JSON via en POST request direkte til API'et
        var response = await http.PostAsJsonAsync($"{Server.Url}/api/user", _user);

        // Hvis serveren svarer OK (200), nulstiller vi og går tilbage til oversigten
        if (response.IsSuccessStatusCode)
        {
            _user = new Users();
            navMan.NavigateTo("adminuser");
        }
    }

    private void Cancel()
    {
        // Sender brugeren tilbage til admin-oversigten uden at gemme
        navMan.NavigateTo("adminuser");
    }
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

                @* Brug en button type="button" så vi ikke utilsigtet submitter formen *@
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
    bool isLoading = false; // Styrer om spinneren vises

    // Bestemmer input-feltets type baseret på om visPassword er sand/falsk
    string PasswordType => visPassword ? "text" : "password";

    void TogglePasswordVisibility()
    {
        visPassword = !visPassword;
    }

    private async Task OnClickLogin()
    {
        errorText = "";
        isLoading = true; // Starter loading animationen

        try
        {
            // Forsøg at logge ind via API'et (UserRepo sender data til serveren)
            Users? userObject = await UserRepo.ValidLoginAsync(user.UserName, user.Password);

            if (userObject == null)
            {
                // Hvis serveren returnerer null, var login ugyldigt
                errorText = "Forkert brugernavn eller adgangskode — prøv igen.";
                isLoading = false;
                return;
            }

            //Fjern adgangskoden fra objektet før vi gemmer det i browseren
            // Vi vil aldrig have passwords liggende i LocalStorage
            try
            {
                userObject.Password = string.Empty; 
            }
            catch 
            {
                // Hvis vi ikke kan tømme passwordet direkte, laver vi et nyt "sikkert" objekt
                var safeUser = new
                {
                    userObject.UserName, userObject.Role
                };
                await LocalStorage.SetItemAsync("user", safeUser);
                Nav.NavigateTo("/", forceLoad: true); 
                return;
            }

            // Gem brugeren i LocalStorage så vi kan huske at de er logget ind
            await LocalStorage.SetItemAsync("user", userObject);
            
            // Naviger til forsiden (forceLoad: true sikrer at App.razor genindlæses og tjekker login-status)
            Nav.NavigateTo("/", forceLoad: true);
        }
        catch (HttpRequestException)
        {
            errorText = "Netværksfejl — kunne ikke kontakte serveren. Tjek at serveren kører.";
        }
        catch (Exception ex)
        {
            errorText = $"Der skete en fejl: {ex.Message}";
        }
        finally
        {
            // Sørger for at fjerne loading-spinneren uanset om det gik godt eller skidt
            isLoading = false;
        }
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

<file path="Server/Repositories/HourRepositories/HourRepositorySQL.cs">
using Core;
namespace Server.Repositories.HourRepositories;
// Repository til håndtering af timer (ProjectHour) i databasen
public class HourRepositorySQL : BaseRepository, IHourRepository
{
    // Tilføjer en ny ProjectHour til databasen
    public void AddRange(List<ProjectHour> hours)
    {
        using var conn = GetConnection();
        conn.Open();
        using var transaction = conn.BeginTransaction();
        try
        {
            foreach (var h in hours)
            {
                using var command = conn.CreateCommand();
                command.CommandText = @"
                INSERT INTO projecthours
                    (projectid, medarbejder, dato, stoptid, timer, type, kostpris) 
                VALUES 
                    (@pid, @med, @dato, @stop, @timer, @type, @kost)";
                command.Parameters.AddWithValue("pid", h.ProjectId);
                command.Parameters.AddWithValue("med", h.Medarbejder ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("dato", h.Dato ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("stop", h.Stoptid ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("timer", h.Timer);
                command.Parameters.AddWithValue("type", h.Type ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("kost", h.Kostpris);
                command.ExecuteNonQuery();
            }
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
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
                // Koden læser data fra bestemte kolonner i Excel og gemmer dem i et ProjectHour-objekt.
                // Tilføjer arbejdstimen til listen
                result.Add(p);
                // Går videre til næste række
                row_no++;
            }
            // Returnerer listen med arbejdstimer
            return result;
        }
    }
}
</file>

<file path="Client/Components/ProjectDetails/PrisOverblik.razor">
@using Core
@namespace Client.Components.ProjectDetails

@if (Details != null)
{
    
    <div class="card text-white mb-4 shadow text-center" style="@(IsClientView ? "background-color: #071226;" : "background-color: #071226;")">
        <div class="card-body py-4">
            @if (IsClientView)
            {
                
                <h2 class="display-3 fw-bold">@Details.SamletTotalPris.ToString("N2") kr.</h2>
                <p class="fs-5 mb-0">Total Pris</p>
            }
            else
            {
                
                <div class="row text-center divide-cols">
                    <div class="col-md-4">
                        <h6 class="text-white-50">TOTAL PRIS</h6>
                        <h2>@Details.SamletTotalPris.ToString("N2") kr.</h2>
                    </div>
                    <div class="col-md-4" style="border-left: 1px solid rgba(255,255,255,0.2); border-right: 1px solid rgba(255,255,255,0.2);">
                        <h6 class="text-white-50">TOTAL KOSTPRIS</h6>
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

    // De 3 bokse (materialer, timer, tid)
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
                            // Salgspris
                        }
                        else
                        {
                            @Details.TotalKostPrisMaterialer.ToString("N2")
                            // Kostpris
                        }
                        kr.
                    </h3>
                    <small class="text-muted">
                        @(IsClientView ? "Salg af materialer" : "Kostpris")
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
                            // Timepris
                            @Details.TotalPrisTimer.ToString("N2")
                        }
                        else
                        {
                            // Lønomkostning
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
                    Total Timer
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

<file path="Server/Program.cs">
/* using Server.Repositories.Proj.CreateProjectsFolder;
using Server.Repositories.Proj.HourCalculator; */
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
/* builder.Services.AddSingleton<ICreateProjectRepo, CreateProjectRepo>();
builder.Services.AddSingleton<IProjectHourCalcRepo, ProjectHourCalcRepositorySQL>(); */
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
// app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
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
        <div class="table-responsive rounded">
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
                                <button class="btn btn-warning me-2" 
                                        @onclick="() => GotoEditPage(user.UserId)">
                                    <i class="bi bi-pencil"></i> Rediger
                                </button>
                                
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

    // Henter listen af brugere når siden startes
    protected override async Task OnInitializedAsync()
    {
        _users = await Http.GetFromJsonAsync<List<Users>>($"{Server.Url}/api/user");
    }

    private async Task DeleteUser(int id)
    {
        // Sender slette-kommando til API'et
        var response = await Http.DeleteAsync($"{Server.Url}/api/user/delete/{id}");
    
        if (response.IsSuccessStatusCode)
        {
            // Fjerner lynhurtigt brugeren fra listen i UI'en uden nyt API-kald
            _users?.RemoveAll(u => u.UserId == id);
        }
    }
    
    private void GotoEditPage(int UserId)
    {
        Nav.NavigateTo($"adduser/edit/{UserId}");
    }

    // Hjælpemetode (Switch expression) til at vælge CSS-klasse baseret på rollens navn
    private string GetRoleBadgeClass(string role)
    {
        return role.ToLower() switch
        {
            "admin" => "bg-danger",   // Rød
            "bruger" => "bg-primary", // Blå
            _ => "bg-secondary"       // Grå (hvis andet)
        };
    }

    private void CreateUser()
    {
        Nav.NavigateTo("adduser");
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
        // Et endpoint der bruges n�r man uploader en fil
        [HttpPost("upload")]
        public IActionResult Upload(IFormFile? file, [FromQuery] int projectId)
        {
            if (file == null || file.Length == 0) return BadRequest("No file selected.");
            try
            {
                using Stream s = new MemoryStream();
                file.CopyTo(s);
                s.Position = 0; 
                // Convert Excel rows to objects
                var materials = MaterialConverter.Convert(s);
                // Assign ProjectId to all items
                foreach (var m in materials)
                {
                    m.ProjectId = projectId;
                }
                // Bulk Save
                _repo.AddRange(materials);
                return Ok($"Successfully processed {materials.Count} materials.");
            }
            catch (Exception ex)
            {
                return BadRequest("Error parsing material file: " + ex.Message);
            }
        }
    }
}
</file>

<file path="Server/Repositories/User/CreateUserRepoSQL.cs">
using System.Data.Common;
using Core;                   
namespace Server.Repositories.User
{
    public class CreateUserRepoSQL : BaseRepository, ICreateUserRepo
    {
    public List<Users> GetAll()
        {
            var result = new List<Users>();    // Tom liste til brugere der hentes
            // Connection string: beskriver hvordan man forbinder til databasen
            using var mConnection = GetConnection(); //Bruger using for at sikre at forbindelsen bliver lukket(Using = sørger for at det bliver lukket)
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
        public Users? GetById(int id)
        {
            using var mConnection = GetConnection();
            mConnection.Open();
            var command = mConnection.CreateCommand();
            command.CommandText = "SELECT * FROM Users WHERE userid = @id";
            var paramId = command.CreateParameter();
            paramId.ParameterName = "id";
            paramId.Value = id;
            command.Parameters.Add(paramId);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Users
                {
                    UserId = reader.GetInt32(0),
                    UserName = reader.GetString(1),
                    Password = reader.GetString(2),
                    Role = reader.GetString(3)
                };
            }
            return null;
        }
        public void Update(Users user)
        {
            using var mConnection = GetConnection();
            mConnection.Open();
            var command = mConnection.CreateCommand();
            command.CommandText = @"UPDATE Users 
                            SET username = @username, 
                                password = @password, 
                                role = @role 
                            WHERE userid = @id";
            // Tilføj parametre (vigtigt for at undgå SQL injection)
            var pId = command.CreateParameter(); pId.ParameterName = "id"; pId.Value = user.UserId;
            command.Parameters.Add(pId);
            var pName = command.CreateParameter(); pName.ParameterName = "username"; pName.Value = user.UserName;
            command.Parameters.Add(pName);
            var pPass = command.CreateParameter(); pPass.ParameterName = "password"; pPass.Value = user.Password;
            command.Parameters.Add(pPass);
            var pRole = command.CreateParameter(); pRole.ParameterName = "role"; pRole.Value = user.Role;
            command.Parameters.Add(pRole);
            command.ExecuteNonQuery();
        }
    }
}
</file>

<file path="Client/Components/Projects/ProjectComponent.razor">
@using Core
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
    
    
}
</file>

<file path="Server/Repositories/BaseRepository.cs">
using Npgsql;
using Server.PW1;
namespace Server.Repositories;
// Baseklasse som andre repositories arver fra
// Abstract class fungere som en skabelon (giver funktionalitet)
public abstract class BaseRepository
{
    // Connection string til online PostgreSQL-database (Neon)
    protected string ConnectionString =>
        "Server=ep-billowing-river-a2khj1xe.eu-central-1.aws.neon.tech;" + // Removed '-pooler'
        "Port=5432;" +
        "User Id=neondb_owner;" +
        "Password=" + PASSWORD.PW1 + ";" +
        "Database=Larsen_InstallationAps;" +
        "SSL Mode=Require;" +
        "Pooling=false;" +
        "Trust Server Certificate=true;";
    // Opretter en ny databaseforbindelse
    // Forbindelsen åbnes først, når den bruges
    protected NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(ConnectionString);
    }
}
</file>

<file path="Client/Components/ProjectDetails/RessourceOverblik.razor">
@using Core
@namespace Client.Components.ProjectDetails

<div class="row mb-2">
    <div class="col-md-6 mb-3">
        <div class="card h-auto shadow-sm" style="border-color: #071226;">
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

    <div class="col-md-6 mb-3">
        <div class="card h-auto shadow-sm" style="border-color: #071226; max-height: 500px; display:flex; flex-direction:column;">
            <div class="card-header bg-transparent fw-bold" style="border-color: #071226; color: #071226;">
                @(IsClientView ? "Materialer" : "Materialer")
            </div>

            <div style="overflow-y: auto; flex: 1;">
                <ul class="list-group list-group-flush">
                    @{
                        // Choose which list to iterate over based on the view mode
                        var displayList = IsClientView ? MaterialGroups : InternMaterialGroups;
                    }

                    @if (displayList != null && displayList.Any())
                    {
                        @foreach (var mat in displayList)
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
    [Parameter] public List<ProjectMaterial> MaterialGroups { get; set; } = new();
    [Parameter] public List<ProjectMaterial> InternMaterialGroups { get; set; } = new();
    [Parameter] public bool IsClientView { get; set; }
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
        [HttpPost("upload")]
        public IActionResult Upload(IFormFile? file, [FromQuery] int projectId)
        {
            if (file == null || file.Length == 0) return BadRequest("No file");
            try 
            {
                using Stream s = new MemoryStream();
                file.CopyTo(s);
                s.Position = 0; 
                var hours = WorkerConverter.Convert(s);
                foreach (var h in hours)
                {
                    h.ProjectId = projectId;
                }
                // ONE call to the database instead of many
                _hourRepo.AddRange(hours); 
                return Ok($"Uploaded {hours.Count} hours.");
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }
    }
}
</file>

<file path="Client/Pages/Project/EditProjectPage.razor">
@using Core
@using Client.Service
@inject HttpClient http
@inject NavigationManager Nav
@page "/project/edit/{ProjectId:int}"

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
    /* Binder formularen til '_project' objektet. Når brugeren trykker 'Gem', køres metoden 'HandleSubmit' */
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

    // 1. Hent data (Kører når siden åbner)
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
    
    // 2. Gem data (Kører når man trykker "Gem ændringer")
    private async Task HandleSubmit()
    {
        if (_project == null) return;

        // Sikkerhedsforanstaltning: Vi sikrer os, at ID'et i objektet matcher URL'en.
        _project.ProjectId = ProjectId;

        try
        {
            // Her bruger vi 'PutAsJsonAsync'.
            // PUT bruges normalt til at opdatere/rette noget eksisterende.
            var res = await http.PutAsJsonAsync($"{Server.Url}/api/project/{ProjectId}", _project);

            // Hvis serveren siger OK (status 200), sender vi brugeren til forsiden.
            if (res.IsSuccessStatusCode)
            {
                Nav.NavigateTo("/");
                return;
            }

            // Hvis fejl: Læs fejlbeskeden fra serveren, så vi kan se, hvad der gik galt.
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
                <div class="upload-box" style="grid-column: span 2;">
                    <label class="upload-title"><strong>Vælg Excel filer (Timer & Materialer)</strong></label>
                    <div class="upload-area">
                        <InputFile OnChange="UploadFiles" accept=".xls,.xlsx" multiple />
                    </div>
                    @* Show the selected files to the user *@
                    @if (selectedFiles != null && selectedFiles.Any())
                    {
                        <div class="mt-2 text-start">
                            <strong>Valgte filer:</strong>
                            <ul class="mb-0">
                                @foreach (var file in selectedFiles)
                                {
                                    <li>@file.Name</li>
                                }
                            </ul>
                        </div>
                    }
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

    // A list to hold multiple files chosen by the user
    IReadOnlyList<IBrowserFile> selectedFiles = new List<IBrowserFile>();
    private bool isSubmitting = false;

    // This runs when the user selects files
    public void UploadFiles(InputFileChangeEventArgs e)
    {
        // Fetches all the selected files. You can specify maximum allowed files as an argument if needed.
        selectedFiles = e.GetMultipleFiles(); 
    }

    private async Task OnClickCreate()
    {
        isSubmitting = true; // Show a spinner!
        StateHasChanged(); // Force UI to show spinner

        var response = await http.PostAsJsonAsync($"{Server.Url}/api/project", aProject);
        if (response.IsSuccessStatusCode)
        {
            var newProjectId = await response.Content.ReadFromJsonAsync<int>();
            var uploadTasks = new List<Task>();

            foreach (var file in selectedFiles)
            {
                var fileName = file.Name.ToLower();
                string endpoint = "";

                if (fileName.Contains("hour") || fileName.Contains("time") || fileName.Contains("case"))
                {
                    endpoint = "api/projecthours/upload";
                }
                else if (fileName.Contains("material") || fileName.Contains("ordre"))
                {
                    endpoint = "api/projectmaterials/upload";
                }

                if (!string.IsNullOrEmpty(endpoint))
                {
                    // Add the task without awaiting it yet
                    uploadTasks.Add(UploadFileToApi(file, newProjectId, endpoint));
                }
            }

            // 3. Wait for all files to upload simultaneously
            await Task.WhenAll(uploadTasks);
        }

        // IMPORTANT: Add a tiny delay to let the server finish its DB transaction
        // and let the Blazor UI thread finish the "IsSubmitting" state
        await Task.Delay(100); 
    
        // Use forceLoad: true ONLY if your App.razor needs a hard refresh
        Nav.NavigateTo("/", forceLoad: false); 
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

<file path="Client/Pages/Project/ProjectPage.razor">
@page "/"
@using Client.Components.Projects
@using Core
@using Client.Service
@inject HttpClient Http
@inject NavigationManager Nav

<div class="admin-container">
    <div class="header-section">
        <h3>Alle projekter</h3>
    </div>

    @if (_projects == null)
    {
        <div class="loading-state full-screen-center">
            <div class="spinner-border text-primary" role="status"></div>
            <p>Indlæser projekter...</p>
        </div>
    }
    else if (!_projects.Any())
    {
        <div class="empty-state">
            <p>Ingen projekter fundet.</p>
        </div>
    }
    else
    {
        <ProjectGrid Projects="_projects" OnDelete="DeleteProject" />
    }
</div>


@code {
    private List<Project>? _projects;
    private int? _activeMenuId = null;

    protected override async Task OnInitializedAsync()
    {
        try 
        {
            _projects = await Http.GetFromJsonAsync<List<Project>>($"{Server.Url}/api/project");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fejl: {ex.Message}");
        }
    }
    // Slet projekt
    private async Task DeleteProject(int id)
    {
        var response = await Http.DeleteAsync($"{Server.Url}/api/project/{id}");

        if (response.IsSuccessStatusCode)
        {
            _projects = await Http.GetFromJsonAsync<List<Project>>($"{Server.Url}/api/project");
            _activeMenuId = null;
        }
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
        // Vi løber igennem alle materialelinjer for at finde total kostpris (indkøb) 
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
        // A. Gruppering af Timer (Svend, Lærling osv.)
        dto.GroupedHours = hours // liste med timer 
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
        // B. Gruppering af Materialer (Kundevisning - Kategorier)
        var categories = new Dictionary<string, string[]>
        {
            { "Belysning", new[] { 
                "spot", "lampe", "led", "lys", "armatur", "pendel", "driver", "dæmper", "skinne", 
                "pære", "lyskilde", "halogen", "projektør", "sensor", "pir", "downlight", "uplight", 
                "lysrør", "fatning", "trafo", "transformator", "plafond", "bevægelsessensor",
                "lampeudtag", "corepro", "strømforsyning", "ceiling", "track", "anker", "dekorativ", 
                "spreaderlight", "stipo", "lighstick", "anker&co", "anker & co",
                "philips", "sg", "nordlux", "louis poulsen", "fabbian", "iguzzini", "zumtobel", "vossloh", "osram", "batten", "glimtænder"
            } },
            { "Alarm", new[] {
                "røgdetektor", "abdl", "brandalarm", "detektor", "alarm", "nøgleskab", "cylinder", "almsten sikring aps",
                "kamera", "itv", "overvågning", "hikvision", "axis", "pax", "adgangskontrol", "magnetkontakt", 
                "siréne", "passiv", "infra", "almsten"
            } },
            { "KNX", new[] {
                "ihc", "knx", "wiser", "dali", "devisnow", "relæmodul", "smart", "home", "saas", 
                "programmering", "controller", "astrour", "timer",
                "gateway", "aktuator", "schneider", "abb", "zennio", "buskabel", "fortrådning", "logikmodul"
            } },
            { "Sikringer & Tavler", new[] { 
                "tavle", "sikring", "hpfi", "rce", "automatsikring", "kombiafbryder", "rcbo", 
                "gruppeafbryder", "hovedafbryder", "måler", "målertavle", "smeltesikring", "neozed", 
                "diazed", "din-skinne", "samleskinne", "klemrække", "overspænding", "gruppetavle", 
                "jordspyd", "byggepladstavle", "abs", "din", "byggepladscentral", "pfi", "sikkerhedsafb", "hovedtavle",
                "tavlemateriel", "kontaktor", "transistor", "strømtransformer", "skinnesystem", "effektafbryder"
            } },
            { "Lyd & AV-udstyr", new[] {
                "lydsystem", "højtaler", "westsound", "iport", "connect pro", "av", "lyd", "sonos", "højtalere",
                "hdmi", "skærm", "projektor", "tv", "lydstyring", "forstærker", "beosound", "bang & olufsen", "displayport"
            } },
            { "Netværk & Data", new[] {
                "datakabel", "netværkskabel", "cat5", "cat6", "cat7", "coax", "antennekabel", 
                "utp", "kat", "patchkabel", "patchpanel", "rack", "rj45", "wifi", "unifi", "surf", 
                "vægbøjle", "19\"",
                "switch", "router", "fiber", "sfp", "keystone", "pdu", "server", "accesspoint", "ubiquiti"
            } },
            { "Kabler & Rør", new[] { 
                "kabel", "ledning", "rør", "nkt", "pvi", "flex", "5x1,5", "3x1,5", "5x2,5", "3x2,5", 
                "5x6", "7x1,5", "installationskabel", "gummikabel", "tomrør", "pn", "noflik", "noiklx", 
                "flexrør", "plastrør", "kabelrør", "afumex", "pklj", "pvl", "signalkabel", "forlængerk", 
                "forlængerkabel", "tilslutningstråd", "kobbertråd", "noikal", "caddy", "qaddy", "tromle",
                "jordledning", "brandkabel", "pknm", "pvikly", "halogenfri", "kabelbakke", "gitterbakke", "stålør", "pariser"
            } },
            { "Installation", new[] { 
                "stikkontakt", "afbryder", "underlag", "dåse", "fuga", "opus", "ramme", "tangent", 
                "wago", "muffe", "samlemuffe", "forfradåse", "indmuringsdåse", "loftdåse", "udtag", 
                "roset", "stikprop", "schuko", "blinddæksel", "korrespondance", "krydsning", 
                "tryk", "clips", "dæksel", "lk", "pressemuffe", "stikk", "polykonmuffe", "jung", 
                "blænddæksel", "designramme", "krympemuffe", "jord", "membrandåse", "forgreningsdåse", 
                "afd", "cee", "mat", "boks", "box", "afdækning", "klemme", "hybridstikprop", "modul",
                "cover", "kasse", "låg", "skrue", "spånskrue", "plugs", "kabelkanal", "ledningskanal", "strips", "kabelbinder", 
                "kabelbøjle", "strip", "bøjlebånd", "tape", "dampspærretape", "fzb", "galv", "metal", 
                "krympeflex", "krympeslange", "hulsav", "bor", "fladbor", "spiralbor", "klinge", "savklinge", "lenox", "dymotape", 
                "fuge", "fugemasse", "batteri", "handske", "nitril", "affald", "støvpose", "qaddy", 
                "tromle", "loddekolbe", "rengøring", "støvsugning", "vinkel", "bolt", "møtrik", "gevind", "unbrakoskrue", 
                "ankerskrue", "isolering", "brandlukning", "easee", "zaptec", "ladekabel", "solcelle", "inverter", "varmepumpe", "montagesystem", "ventilation", "ølund", "udsugning"
            } }
        };
        dto.GroupedMaterialsClientView = materials 
            .GroupBy(m => { 
                // ÆNDRING: Nu slår vi Beskrivelse og Leverandør sammen til én sætning
                string desc = $"{m.Beskrivelse} {m.Leverandør}".ToLower();
                // Find første kategori der matcher
                foreach (var category in categories)
                {
                    if (category.Value.Any(keyword => desc.Contains(keyword))) return category.Key; 
                }
                return "Øvrige materialer"; 
            })
            .Select(g => new ProjectMaterial 
            {
                Beskrivelse = g.Key, 
                Total = g.Sum(x => x.Total) 
            })
            .OrderByDescending(m => m.Total) 
            .ToList();
        // C. Gruppering af Materialer (Intern visning - Leverandør/Navn)
        // Kendte leverandører
        dto.GroupedMaterialsInternView = materials 
            .GroupBy(m => { 
                string desc = $"{m.Beskrivelse} {m.Leverandør}".ToLower();
                foreach (var category in categories)
                {
                    if (category.Value.Any(keyword => desc.Contains(keyword))) return category.Key; 
                }
                return "Øvrige materialer"; 
            })
            .Select(g => new ProjectMaterial 
            {
                Beskrivelse = g.Key, 
                // HER ER ÆNDRINGEN: Brug Kostpris * Antal i stedet for x.Total
                Total = g.Sum(x => x.Kostpris * x.Antal) 
            })
            .OrderByDescending(m => m.Total) 
            .ToList();
        return dto; // Returnerer dto (timer + materialer) 
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
                           InternMaterialGroups="details.GroupedMaterialsInternView"
                           IsClientView="showClientView" />
        <BarChart Hours="@details.Hours" />

    </div>
}

@code {
    [Parameter] public int Id { get; set; }

    private Calculation? details;
    private bool showClientView = false;

    protected override async Task OnInitializedAsync()
    {
        details = await ProjectService.GetProjectDetails(Id);
        details = await Http.GetFromJsonAsync<Calculation>($"{Server.Url}/api/project/{Id}");
    }

   
}
</file>

</files>
