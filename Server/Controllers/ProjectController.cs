using Microsoft.AspNetCore.Mvc;
using System.Data;
using ExcelDataReader;
using Core;
using Server.Repositories;
using System.Text;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/project")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _repo;

        public ProjectController(IProjectRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProject()
        {
            if (!Request.HasFormContentType)
                return BadRequest("Forventet multipart/form-data.");

            var form = await Request.ReadFormAsync();

            // Læs project JSON (felt "project")
            var projectJson = form["project"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(projectJson))
                return BadRequest("Mangler 'project' JSON i form-data.");

            Project project;
            try
            {
                project = System.Text.Json.JsonSerializer.Deserialize<Project>(projectJson, new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })!;
            }
            catch (Exception ex)
            {
                return BadRequest("Ugyldigt project JSON: " + ex.Message);
            }

            // Indsaml ALLE filer med navn "timeFile" og "materialFile"
            var timeFileUploads = form.Files.Where(f => f.Name == "timeFile").ToList();
            var materialFileUploads = form.Files.Where(f => f.Name == "materialFile").ToList();

            // Til brug for ExcelDataReader
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            DataTable? combinedHours = null;
            DataTable? combinedMaterials = null;

            try
            {
                // Helper: læs første ark i en excel fil og returnér DataTable (UseHeaderRow = true)
                DataTable? ReadFirstTableFromExcel(string filePath)
                {
                    using var stream = System.IO.File.OpenRead(filePath);
                    // Vælg reader type efter fil-endelse
                    using var reader = filePath.EndsWith(".xls", StringComparison.OrdinalIgnoreCase)
                        ? ExcelReaderFactory.CreateBinaryReader(stream)
                        : ExcelReaderFactory.CreateOpenXmlReader(stream);

                    var ds = reader.AsDataSet(new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = true }
                    });

                    return ds.Tables.Count > 0 ? ds.Tables[0] : null;
                }

                // Process time files (behold kolonnenavne fra første fil)
                foreach (var f in timeFileUploads)
                {
                    // Gem midlertidigt
                    var tmp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + Path.GetExtension(f.FileName));
                    await using (var fs = System.IO.File.Create(tmp))
                        await f.CopyToAsync(fs);

                    var dt = ReadFirstTableFromExcel(tmp);

                    try { System.IO.File.Delete(tmp); } catch { /* swallow */ }

                    if (dt == null) continue;

                    if (combinedHours == null)
                    {
                        combinedHours = dt.Clone(); // clone schema (kolonnenavne)
                    }

                    foreach (DataRow r in dt.Rows)
                        combinedHours.ImportRow(r);
                }

                // Process material files
                foreach (var f in materialFileUploads)
                {
                    var tmp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + Path.GetExtension(f.FileName));
                    await using (var fs = System.IO.File.Create(tmp))
                        await f.CopyToAsync(fs);

                    var dt = ReadFirstTableFromExcel(tmp);
                    try { System.IO.File.Delete(tmp); } catch { }

                    if (dt == null) continue;

                    if (combinedMaterials == null)
                    {
                        combinedMaterials = dt.Clone();
                    }

                    foreach (DataRow r in dt.Rows)
                        combinedMaterials.ImportRow(r);
                }

                // (Valgfri) kort validering: hvis du vil kræve mindst én række i en fil, uncomment
                // if ((combinedHours == null || combinedHours.Rows.Count == 0) && (combinedMaterials == null || combinedMaterials.Rows.Count == 0))
                //     return BadRequest("Ingen rækker fundet i uploadede filer.");

                // Send til repository (som forventer DataTable? for hver type)
                await _repo.CreateProjectWithDataAsync(project, combinedHours, combinedMaterials);

                return Ok(new { message = "Projekt oprettet" });
            }
            catch (Exception ex)
            {
                // Log evt. her
                return StatusCode(500, "Fejl ved import: " + ex.Message);
            }
        }
    }
}
