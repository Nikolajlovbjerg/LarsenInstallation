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
    ProjectController.cs
  Repositories/
    User/
      CreateUserRepoSQL.cs
      ICreateUserRepoSQL.cs
    IProjectRepository.cs
    ProjectRepositorySQL.cs
  Service/
    ExcelFileHelper.cs
    ExcelReader.cs
  appsettings.Development.json
  appsettings.json
  Program.cs
</directory_structure>

<files>
This section contains the contents of the repository's files.

<file path="Core/Calculation.cs">
namespace Core
{
    public class Calculation
    {
        public int CalcId { get; set; }
        public int ProjectId { get; set; }
        public decimal TotalMaterialCost { get; set; }
        public decimal TotalHourlyCost { get; set; }
        public decimal TotalCustomerPrice { get; set; }
        public decimal TotalEarnings { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
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
        public string? Fra { get; set; }
        public string? Til { get; set; }
        public decimal Timer { get; set; }
        public string? Type { get; set; }
        public string? Beskrivelse { get; set; }
        public decimal Kostpris { get; set; }
        public string? Ordrenr { get; set; }
        public string RawRow { get; set; } = string.Empty;
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
        public string? Varenummer { get; set; }
        public string? Beskrivelse { get; set; }
        public decimal Kostpris { get; set; }
        public decimal Listepris { get; set; }
        public decimal Antal { get; set; }
        public decimal Total { get; set; }
        public decimal Markedspris { get; set; }
        public decimal Avance { get; set; }
        public decimal Dækningsgrad { get; set; }
        public string RawRow { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
</file>

<file path="Server/Controllers/ProjectController.cs">
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using System.Data;
using ExcelDataReader;
using Server.Repositories;
using Core;
using System.Text;
namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _repo;
        private readonly IWebHostEnvironment _env;
        public ProjectController(IProjectRepository repo, IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
        }
        [HttpPost]
        public async Task<IActionResult> CreateProject()
        {
            if (!Request.HasFormContentType) return BadRequest("Form-data forventes");
            var form = await Request.ReadFormAsync();
            var projectJson = form["project"].FirstOrDefault();
            if (string.IsNullOrEmpty(projectJson)) return BadRequest("Mangler project JSON");
            Project project;
            try
            {
                project = JsonSerializer.Deserialize<Project>(projectJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            }
            catch (Exception e)
            {
                return BadRequest("Invalid project JSON: " + e.Message);
            }
            IFormFile? timeFile = form.Files.FirstOrDefault(f => f.Name == "timeFile");
            IFormFile? materialFile = form.Files.FirstOrDefault(f => f.Name == "materialFile");
            DataTable? hoursTable = null;
            DataTable? materialsTable = null;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            try
            {
                if (timeFile != null)
                {
                    var tmpPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + Path.GetExtension(timeFile.FileName));
                    await using (var fs = System.IO.File.Create(tmpPath)) await timeFile.CopyToAsync(fs);
                    // Read Excel -> DataSet/DataTable using ExcelDataReader
                    using var stream = System.IO.File.OpenRead(tmpPath);
                    using var reader = tmpPath.EndsWith(".xls", StringComparison.OrdinalIgnoreCase)
                        ? ExcelReaderFactory.CreateBinaryReader(stream)
                        : ExcelReaderFactory.CreateOpenXmlReader(stream);
                    var ds = reader.AsDataSet(new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = true }
                    });
                    if (ds.Tables.Count > 0) hoursTable = ds.Tables[0];
                    try { System.IO.File.Delete(tmpPath); } catch { }
                }
                if (materialFile != null)
                {
                    var tmpPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + Path.GetExtension(materialFile.FileName));
                    await using (var fs = System.IO.File.Create(tmpPath)) await materialFile.CopyToAsync(fs);
                    using var stream = System.IO.File.OpenRead(tmpPath);
                    using var reader = tmpPath.EndsWith(".xls", StringComparison.OrdinalIgnoreCase)
                        ? ExcelReaderFactory.CreateBinaryReader(stream)
                        : ExcelReaderFactory.CreateOpenXmlReader(stream);
                    var ds = reader.AsDataSet(new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = true }
                    });
                    if (ds.Tables.Count > 0) materialsTable = ds.Tables[0];
                    try { System.IO.File.Delete(tmpPath); } catch { }
                }
                // call repo to create project + import rows
                await _repo.CreateProjectWithDataAsync(project, hoursTable, materialsTable);
                return Ok(new { message = "Projekt oprettet" });
            }
            catch (Exception ex)
            {
                // Log exception in real app
                return StatusCode(500, "Fejl ved oprettelse af projekt: " + ex.Message);
            }
        }
    }
}
</file>

<file path="Server/Repositories/IProjectRepository.cs">
using System.Data;
using System.Threading.Tasks;
using Core;
namespace Server.Repositories
{
    public interface IProjectRepository
    {
        /// <summary>
        /// Opretter et projekt og returnerer ProjectId.
        /// </summary>
        Task<int> CreateProjectAsync(Project project);
        /// <summary>
        /// Opretter projektet og indsætter alle Hour- og Material-rækker
        /// i samme database-transaktion.
        /// </summary>
        Task CreateProjectWithDataAsync(Project project, DataTable? hoursTable, DataTable? materialsTable);
    }
}
</file>

<file path="Server/Repositories/ProjectRepositorySQL.cs">
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Server.PW1;
using Microsoft.Extensions.Configuration;
using Core;
namespace Server.Repositories
{
    public class ProjectRepositorySQL : IProjectRepository
    {
        private readonly string _conString = 
            "Host=ep-spring-unit-a2y1k0pd.eu-central-1.aws.neon.tech;" +
            "Port=5432;" +
            "Database=LarsenInstallation;" +
            "Username=neondb_owner;" +
            $"Password={PASSWORD.PW1};" +
            "Ssl Mode=Require;" +
            "Trust Server Certificate=true;";
        public async Task<int> CreateProjectAsync(Project project)
        {
            using var conn = new NpgsqlConnection(_conString);
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO projects (name, datecreated, svend_timepris, lærling_timepris, konsulent_timepris, arbjedsmand_timepris)
                                VALUES (@name, @datecreated, @svend, @lærling, @konsulent, @arbejdsmand)
                                RETURNING projectid;";
            cmd.Parameters.AddWithValue("name", project.Name ?? string.Empty);
            cmd.Parameters.AddWithValue("datecreated", project.DateCreated == default ? DateTime.UtcNow : project.DateCreated);
            cmd.Parameters.AddWithValue("svend", project.SvendTimePris);
            cmd.Parameters.AddWithValue("lærling", project.LærlingTimePris);
            cmd.Parameters.AddWithValue("konsulent", project.KonsulentTimePris);
            cmd.Parameters.AddWithValue("arbejdsmand", project.ArbjedsmandTimePris);
            var res = await cmd.ExecuteScalarAsync();
            return Convert.ToInt32(res);
        }
        public async Task CreateProjectWithDataAsync(Project project, DataTable? hoursTable, DataTable? materialsTable)
        {
            using var conn = new NpgsqlConnection(_conString);
            await conn.OpenAsync();
            using var tx = await conn.BeginTransactionAsync();
            try
            {
                int projectId;
                // Insert project
                using (var cmd = conn.CreateCommand())
                {
                    cmd.Transaction = tx;
                    cmd.CommandText = @"INSERT INTO projects (name, datecreated, svend_timepris, lærling_timepris, konsulent_timepris, arbjedsmand_timepris)
                                        VALUES (@name, @datecreated, @svend, @lærling, @konsulent, @arbejdsmand)
                                        RETURNING projectid;";
                    cmd.Parameters.AddWithValue("name", project.Name ?? string.Empty);
                    cmd.Parameters.AddWithValue("datecreated", project.DateCreated == default ? DateTime.UtcNow : project.DateCreated);
                    cmd.Parameters.AddWithValue("svend", project.SvendTimePris);
                    cmd.Parameters.AddWithValue("lærling", project.LærlingTimePris);
                    cmd.Parameters.AddWithValue("konsulent", project.KonsulentTimePris);
                    cmd.Parameters.AddWithValue("arbejdsmand", project.ArbjedsmandTimePris);
                    projectId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                }
                // Insert hours
                if (hoursTable != null && hoursTable.Rows.Count > 0)
                {
                    foreach (DataRow row in hoursTable.Rows)
                    {
                        DateTime? dato = TryParseDate(hoursTable, row, "Dato");
                        DateTime? stoptid = TryParseDate(hoursTable, row, "Stoptid");
                        decimal timer = TryParseDecimal(hoursTable, row, "Timer");
                        var type = hoursTable.Columns.Contains("Type") ? row["Type"]?.ToString() : null;
                        decimal kostpris = TryParseDecimal(hoursTable, row, "Kostpris");
                        using var cmd = conn.CreateCommand();
                        cmd.Transaction = tx;
                        cmd.CommandText = @"INSERT INTO projecthours (projectid, dato, stoptid, timer, type, beskrivelse, kostpris, raw_row)
                                            VALUES (@projectid,@dato,@stoptid,@timer,@type,@beskrivelse,@kostpris,@rawrow)";
                        cmd.Parameters.AddWithValue("projectid", projectId);
                        cmd.Parameters.AddWithValue("dato", (object)dato ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("stoptid", (object)stoptid ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("timer", timer);
                        cmd.Parameters.AddWithValue("type", (object)type ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("kostpris", kostpris);
                        cmd.Parameters.AddWithValue("rawrow", string.Join("|", row.ItemArray.Select(x => x?.ToString() ?? "")));
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                // Insert materials
                if (materialsTable != null && materialsTable.Rows.Count > 0)
                {
                    foreach (DataRow row in materialsTable.Rows)
                    {
                        var beskrivelse = materialsTable.Columns.Contains("Beskrivelse") ? row["Beskrivelse"]?.ToString() : null;
                        decimal kostpris = TryParseDecimal(materialsTable, row, "Kostpris");
                        decimal antal = TryParseDecimal(materialsTable, row, "Antal");
                        decimal total = TryParseDecimal(materialsTable, row, "Total");
                        decimal avance = TryParseDecimal(materialsTable, row, "Avance.1");
                        decimal dækningsgrad = TryParseDecimal(materialsTable, row, "Dækningsgrad");
                        using var cmd = conn.CreateCommand();
                        cmd.Transaction = tx;
                        cmd.CommandText = @"INSERT INTO projectmaterials (projectid, beskrivelse, kostpris, antal, total, avance, dækningsgrad, raw_row)
                                            VALUES (@projectid,@beskrivelse,@kostpris,@antal,@total,@avance,@dækningsgrad,@rawrow)";
                        cmd.Parameters.AddWithValue("projectid", projectId);
                        cmd.Parameters.AddWithValue("beskrivelse", (object)beskrivelse ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("kostpris", kostpris);
                        cmd.Parameters.AddWithValue("antal", antal);
                        cmd.Parameters.AddWithValue("total", total);
                        cmd.Parameters.AddWithValue("avance", avance);
                        cmd.Parameters.AddWithValue("dækningsgrad", dækningsgrad);
                        cmd.Parameters.AddWithValue("rawrow", string.Join("|", row.ItemArray.Select(x => x?.ToString() ?? "")));
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                // Simple calculation and insert to calculations
                decimal totalMaterialCost = 0;
                decimal totalHourlyCost = 0;
                using (var cmdMat = conn.CreateCommand())
                {
                    cmdMat.Transaction = tx;
                    cmdMat.CommandText = "SELECT COALESCE(SUM(total),0) FROM projectmaterials WHERE projectid=@pid";
                    cmdMat.Parameters.AddWithValue("pid", projectId);
                    var matObj = await cmdMat.ExecuteScalarAsync();
                    totalMaterialCost = Convert.ToDecimal(matObj ?? 0);
                }
                using (var cmdHour = conn.CreateCommand())
                {
                    cmdHour.Transaction = tx;
                    cmdHour.CommandText = "SELECT COALESCE(SUM(timer * COALESCE(kostpris,0)),0) FROM projecthours WHERE projectid=@pid";
                    cmdHour.Parameters.AddWithValue("pid", projectId);
                    var hourObj = await cmdHour.ExecuteScalarAsync();
                    totalHourlyCost = Convert.ToDecimal(hourObj ?? 0);
                }
                using (var cmdCalc = conn.CreateCommand())
                {
                    cmdCalc.Transaction = tx;
                    cmdCalc.CommandText = @"INSERT INTO calculations (projectid, total_material_cost, total_hourly_cost, total_customer_price, total_earnings)
                                            VALUES (@pid,@mat,@hour,@cust,@earnings)";
                    decimal totalCustomerPrice = totalMaterialCost + totalHourlyCost;
                    decimal totalEarnings = 0;
                    cmdCalc.Parameters.AddWithValue("pid", projectId);
                    cmdCalc.Parameters.AddWithValue("mat", totalMaterialCost);
                    cmdCalc.Parameters.AddWithValue("hour", totalHourlyCost);
                    cmdCalc.Parameters.AddWithValue("cust", totalCustomerPrice);
                    cmdCalc.Parameters.AddWithValue("earnings", totalEarnings);
                    await cmdCalc.ExecuteNonQueryAsync();
                }
                await tx.CommitAsync();
            }
            catch
            {
                try { await tx.RollbackAsync(); } catch { }
                throw;
            }
        }
        private static DateTime? TryParseDate(DataTable dt, DataRow row, string columnName)
        {
            if (!dt.Columns.Contains(columnName)) return null;
            var obj = row[columnName];
            if (obj == null || obj == DBNull.Value) return null;
            if (DateTime.TryParse(obj.ToString(), out var d)) return d;
            // Excel may store as double (OADate)
            if (double.TryParse(obj.ToString(), out var od) && od > 0)
            {
                try { return DateTime.FromOADate(od); } catch { }
            }
            return null;
        }
        private static decimal TryParseDecimal(DataTable dt, DataRow row, string columnName)
        {
            if (!dt.Columns.Contains(columnName)) return 0;
            var obj = row[columnName];
            if (obj == null || obj == DBNull.Value) return 0;
            if (decimal.TryParse(obj.ToString(), out var d)) return d;
            if (double.TryParse(obj.ToString(), out var dd)) return Convert.ToDecimal(dd);
            return 0;
        }
    }
}
</file>

<file path="Client/Components/CreateProjectComponent.razor">
@using Core
@inject NavigationManager Nav
@inject HttpClient Http

<div class="create-page-back">
    <div class="create-con">
        <h3>Opret projekt</h3>

        <EditForm Model="@_project" OnValidSubmit="OnClickCreate">
            
            <DataAnnotationsValidator />
            <ValidationSummary />

            <div class="upload-grid">
                <div class="upload-box">
                    <label class="upload-title"><strong>Vælg time fil (.xls, .xlsx)</strong></label>
                    <div class="upload-area">
                        <InputFile OnChange="OnTimeFileChange" accept=".xls,.xlsx" />
                        @if (!string.IsNullOrEmpty(timeFileName))
                        {
                            <div class="mt-2 small">Valgt: @timeFileName (@FormatBytes(timeFileSize))</div>
                        }
                    </div>
                </div>

                <div class="upload-box">
                    <label class="upload-title"><strong>Vælg materiale fil (.xls, .xlsx)</strong></label>
                    <div class="upload-area">
                        <InputFile OnChange="OnMaterialFileChange" accept=".xls,.xlsx" />
                        @if (!string.IsNullOrEmpty(materialFileName))
                        {
                            <div class="mt-2 small">Valgt: @materialFileName (@FormatBytes(materialFileSize))</div>
                        }
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
    private Project _project = new() { DateCreated = DateTime.UtcNow };
    private IBrowserFile? timeFile;
    private IBrowserFile? materialFile;
    private string timeFileName = string.Empty;
    private string materialFileName = string.Empty;
    private long timeFileSize = 0;
    private long materialFileSize = 0;
    private bool isUploading = false;
    private string statusMessage = string.Empty;
    private string statusClass = "alert-info";

    // Max file size per file: 50 MB (tilpas efter behov)
    private const long MaxFileBytes = 50L * 1024 * 1024;

    private void OnTimeFileChange(InputFileChangeEventArgs e)
    {
        timeFile = e.File;
        timeFileName = timeFile?.Name ?? string.Empty;
        timeFileSize = timeFile?.Size ?? 0;
        if (timeFileSize > MaxFileBytes)
        {
            statusClass = "alert-danger";
            statusMessage = $"Timefil er for stor (maks {FormatBytes(MaxFileBytes)}).";
            timeFile = null;
            timeFileName = string.Empty;
            timeFileSize = 0;
        }
        else
        {
            statusMessage = string.Empty;
        }
    }

    private void OnMaterialFileChange(InputFileChangeEventArgs e)
    {
        materialFile = e.File;
        materialFileName = materialFile?.Name ?? string.Empty;
        materialFileSize = materialFile?.Size ?? 0;
        if (materialFileSize > MaxFileBytes)
        {
            statusClass = "alert-danger";
            statusMessage = $"Materialefil er for stor (maks {FormatBytes(MaxFileBytes)}).";
            materialFile = null;
            materialFileName = string.Empty;
            materialFileSize = 0;
        }
        else
        {
            statusMessage = string.Empty;
        }
    }

    private static string FormatBytes(long bytes)
    {
        if (bytes < 1024) return $"{bytes} B";
        if (bytes < 1024 * 1024) return $"{bytes / 1024.0:F1} KB";
        return $"{bytes / (1024.0 * 1024.0):F2} MB";
    }

    private async Task OnClickCreate()
    {
        statusMessage = string.Empty;
        statusClass = "alert-info";

        if (string.IsNullOrWhiteSpace(_project.Name))
        {
            statusClass = "alert-danger";
            statusMessage = "Projektet skal have et navn.";
            return;
        }

        if (timeFile == null && materialFile == null)
        {
            statusClass = "alert-danger";
            statusMessage = "Vælg mindst én fil: timefil eller materialefil.";
            return;
        }

        isUploading = true;

        try
        {
            using var content = new MultipartFormDataContent();

            // Add project JSON as a string content (name "project")
            var projectJson = System.Text.Json.JsonSerializer.Serialize(_project);
            var projectContent = new StringContent(projectJson, System.Text.Encoding.UTF8, "application/json");
            content.Add(projectContent, "project");

            // Add timeFile
            if (timeFile != null)
            {
                // OpenReadStream bruger maxAllowedSize for sikkerhed; server skal også have passende limit
                var stream = timeFile.OpenReadStream(maxAllowedSize: MaxFileBytes);
                var fileContent = new StreamContent(stream);
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(timeFile.ContentType ?? "application/octet-stream");
                // IMPORTANT: field name must be "timeFile" to matche controller
                content.Add(fileContent, "timeFile", timeFile.Name);
            }

            // Add materialFile
            if (materialFile != null)
            {
                var stream = materialFile.OpenReadStream(maxAllowedSize: MaxFileBytes);
                var fileContent = new StreamContent(stream);
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(materialFile.ContentType ?? "application/octet-stream");
                content.Add(fileContent, "materialFile", materialFile.Name);
            }

            var response = await Http.PostAsync("api/project", content);

            if (response.IsSuccessStatusCode)
            {
                statusClass = "alert-success";
                statusMessage = "Projekt oprettet og filer uploadet.";
                // Option: naviger til projects-side
                await Task.Delay(600); // kort pause så bruger ser besked (valgfri)
                Nav.NavigateTo("projects", forceLoad: true);
            }
            else
            {
                var err = await response.Content.ReadAsStringAsync();
                statusClass = "alert-danger";
                statusMessage = $"Fejl fra server: {(int)response.StatusCode} {response.ReasonPhrase}. {err}";
            }
        }
        catch (OperationCanceledException)
        {
            statusClass = "alert-warning";
            statusMessage = "Upload annulleret.";
        }
        catch (Exception ex)
        {
            statusClass = "alert-danger";
            statusMessage = $"Fejl ved upload: {ex.Message}";
        }
        finally
        {
            isUploading = false;
            // dispose IBrowserFile streams are remote-managed; StreamContent will be disposed with using
        }
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
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public int SvendTimePris { get; set; }
    public int LærlingTimePris { get; set; }
    public int KonsulentTimePris { get; set; }
    public int ArbjedsmandTimePris { get; set; }
}
</file>

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
/*using System.Data;
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
}*/
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
using Server.Repositories;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddSingleton<IProjectRepository, ProjectRepositorySQL>();
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
