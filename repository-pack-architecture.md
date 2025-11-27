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
  Layout/
    MainLayout.razor
    NavMenu.razor
  Pages/
    AddUser.razor
    CreateProjectPage.razor
    Home.razor
    LoginPage.razor
  Service/
    UserRepository.cs
  _Imports.razor
  App.razor
  Program.cs
Core/
  Login.cs
  Project.cs
  User.cs
Server/
  Controllers/
    UserCon/
      UserController.cs
  Repositories/
    User/
      CreateUserRepoSQL.cs
      ICreateUserRepoSQL.cs
  Service/
    ExcelFileHelper.cs
    ExcelReader.cs
  appsettings.Development.json
  appsettings.json
  Program.cs
</directory_structure>

<files>
This section contains the contents of the repository's files.

<file path="Server/Service/ExcelFileHelper.cs">
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Service;
public class ExcelFileHelper
{
    public static bool SaveAsCsv(string excelFilePath, string destinationCsvFilePath)
    {
        using (var stream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            IExcelDataReader reader = null;
            if (excelFilePath.EndsWith(".xls"))
            {
                reader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else if (excelFilePath.EndsWith(".xlsx"))
            {
                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            if (reader == null)
                return false;
            var ds = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = false
                }
            });
            var csvContent = string.Empty;
            int row_no = 0;
            while (row_no < ds.Tables[0].Rows.Count)
            {
                var arr = new List<string>();
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    arr.Add(ds.Tables[0].Rows[row_no][i].ToString());
                }
                row_no++;
                csvContent += string.Join(",", arr) + "\n";
            }
            StreamWriter csv = new StreamWriter(destinationCsvFilePath, false);
            csv.Write(csvContent);
            csv.Close();
            return true;
        }
    }
}
</file>

<file path="Server/Service/ExcelReader.cs">
using System.Data;
using System.Text;
using ExcelDataReader;
namespace Service;
public class ExcelReader
{
        string excelFilePath = "data.xlsx";
        using (var stream = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            IExcelDataReader reader = null;
            if (excelFilePath.EndsWith(".xls"))
            {
                reader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else if (excelFilePath.EndsWith(".xlsx"))
            {
                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            if (reader == null)
                throw new Exception("could not read excel-file");
            var ds = reader.AsDataSet();
            int row_no = 1;
            DataRowCollection rows = ds.Tables[0].Rows;
            while (row_no < rows.Count)
            {
                DataRow aRow = rows[row_no];
                //dato in idx 0
                string dato = aRow[0].ToString();
                // timer in idx 2
                string timer = aRow[2].ToString();
                int timerAsInt = int.Parse(timer);
                Console.WriteLine($"{row_no}: {dato}, {timerAsInt}");
                row_no++;
            }
        }
}
</file>

<file path="Client/Components/CreateProjectComponent.razor">
@using Core

<div class="create-page-back">
    <div class="create-con">
        <h3>Opret projekt</h3>

        <EditForm Model="@_project" OnValidSubmit="OnClickCreate">
            
                <div class="upload-grid">
                    <div class="upload-box">
                        <label class="upload-title"><strong>Vælg time fil</strong></label>

                        <div class="upload-area">
                            <InputFile class="hidden-file-input" multiple="" />
                        </div>
                    </div>

                    <div class="upload-box">
                        <label class="upload-title"><strong>Vælg materiale fil</strong></label>

                        <div class="upload-area">
                            <InputFile class="hidden-file-input" multiple="" />
                        </div>
                    </div>
                </div>

                <div class="form-floating-group">
                    <InputText id="name" class="form-control" @bind-Value="_project.Name"/>
                    <label for="name">Navn</label>
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
                        <InputNumber id="arbejdsmand" class="form-control" @bind-Value="_project.ArbjedsmandTimePris"/>
                        <label for="konsulent">Arbejdsmand sats</label>
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
    Project _project = new();

    private void OnClickCreate(EditContext obj)
    {
        throw new NotImplementedException();
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

<file path="Client/Pages/Home.razor">
@page "/"

<PageTitle>Home</PageTitle>

<h1>Hello, world!</h1>

Welcome to your new app.
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

<file path="Core/Project.cs">
namespace Core;
public class Project
{
    public int ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; }
    public int SvendTimePris { get; set; }
    public int LærlingTimePris { get; set; }
    public int KonsulentTimePris { get; set; }
    public int ArbjedsmandTimePris { get; set; }
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

<file path="Server/Program.cs">
using Server.Repositories.User;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddSingleton<ICreateUserRepoSQL, CreateUserRepoSQL>();
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
                Nav.NavigateTo("projects", forceLoad: true);
                return;
            }

            await LocalStorage.SetItemAsync("user", userObject);
            Nav.NavigateTo("projects", forceLoad: true);
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

<file path="Client/Layout/NavMenu.razor">
@using Core
@inject Blazored.LocalStorage.ILocalStorageService LocalStorage
@inject NavigationManager navManager


<div class="logo-con">
    <div class="m-4">
        <img src="Assets/Larsen-logo_2021hvid.png" class="logo-larsen"/>
        <button title="Navigation menu" class="navbar-toggler" @onclick="ToggleNavMenu">
            <span><svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-list" viewBox="0 0 16 16">
                <path fill-rule="evenodd" d="M2.5 12a.5.5 0 0 1 .5-.5h10a.5.5 0 0 1 0 1H3a.5.5 0 0 1-.5-.5m0-4a.5.5 0 0 1 .5-.5h10a.5.5 0 0 1 0 1H3a.5.5 0 0 1-.5-.5m0-4a.5.5 0 0 1 .5-.5h10a.5.5 0 0 1 0 1H3a.5.5 0 0 1-.5-.5"/>
            </svg></span>
        </button>
    </div>
</div>

<div class="@NavMenuCssClass nav-scrollable" @onclick="ToggleNavMenu">
    <nav class="nav flex-column h-100">
        <div class="nav-item px-3">
            <NavLink class="nav-link" href="projects" Match="NavLinkMatch.All">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-folder" viewBox="0 0 16 16">
                    <path d="M.54 3.87.5 3a2 2 0 0 1 2-2h3.672a2 2 0 0 1 1.414.586l.828.828A2 2 0 0 0 9.828 3h3.982a2 2 0 0 1 1.992 2.181l-.637 7A2 2 0 0 1 13.174 14H2.826a2 2 0 0 1-1.991-1.819l-.637-7a2 2 0 0 1 .342-1.31zM2.19 4a1 1 0 0 0-.996 1.09l.637 7a1 1 0 0 0 .995.91h10.348a1 1 0 0 0 .995-.91l.637-7A1 1 0 0 0 13.81 4zm4.69-1.707A1 1 0 0 0 6.172 2H2.5a1 1 0 0 0-1 .981l.006.139q.323-.119.684-.12h5.396z"/>
                </svg> Projects
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
            <div class="nav-item px-3">
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

</files>
