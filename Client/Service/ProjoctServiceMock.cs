using Core;
using ExcelDataReader;
using Microsoft.AspNetCore.Components.Forms;
using System.Data;
using System.Globalization;
using System.Text;

namespace Client.Service;

public class ProjectServiceMock : IProjectService
{
    public async Task<bool> CreateProject(Project project, IBrowserFile timeFile, IBrowserFile materialFile)
    {
        try
        {
            // Vigtigt: Registrer encoding så ExcelDataReader virker
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Console.WriteLine($"Starter oprettelse af projekt: {project.Name}");

            
            if (timeFile != null)
            {
                Console.WriteLine("--- Læser Timefil ---");
                await ReadTimeFile(timeFile);
            }
            else
            {
                Console.WriteLine("Ingen timefil valgt.");
            }

            
            if (materialFile != null)
            {
                Console.WriteLine("--- Læser Materialefil ---");
                await ReadMaterialFile(materialFile);
            }
            else
            {
                Console.WriteLine("Ingen materialefil valgt.");
            }

            
            Console.WriteLine("Mock Service: Alle data læst og 'gemt'.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Kritisk fejl i service: " + ex.Message);
            return false;
        }
    }

    
    private async Task ReadTimeFile(IBrowserFile file)
    {
        using (var stream = await GetFileStream(file))
        {
            var reader = GetExcelReader(file, stream);
            if (reader == null) return;

            var ds = reader.AsDataSet();
            if (ds.Tables.Count > 0)
            {
                var rows = ds.Tables[0].Rows;
                // Starter fra række 1 for at springe overskrifter over
                for (int i = 1; i < rows.Count; i++)
                {
                    DataRow row = rows[i];

                    // Index baseret på Worker2.xlsx
                    string dato = row[1]?.ToString() ?? "";       
                    string stoptid = row[2]?.ToString() ?? "";    
                    string timerStr = row[5]?.ToString() ?? "0";  
                    string type = row[6]?.ToString() ?? "";       
                    string kostprisStr = row[8]?.ToString() ?? "0"; 

                    double timer = ParseDouble(timerStr);
                    double kostpris = ParseDouble(kostprisStr);
                    
                    Console.WriteLine($"[TIME] Række {i}: Dato={dato}, Timer={timer}, Type={type}, Pris={kostpris}");
                }
            }
        }
    }

    
    private async Task ReadMaterialFile(IBrowserFile file)
    {
        using (var stream = await GetFileStream(file))
        {
            var reader = GetExcelReader(file, stream);
            if (reader == null) return;

            var ds = reader.AsDataSet();
            if (ds.Tables.Count > 0)
            {
                var rows = ds.Tables[0].Rows;
                // Starter fra række 1 for at springe overskrifter over
                for (int i = 1; i < rows.Count; i++)
                {
                    DataRow row = rows[i];

                    // Index baseret på Materialer2.xlsx
                    string beskrivelse = row[1]?.ToString() ?? "";  
                    string kostprisStr = row[2]?.ToString() ?? "0"; 
                    string antalStr = row[4]?.ToString() ?? "0";    
                    string totalprisStr = row[17]?.ToString() ?? "0";
                    
                    double kostpris = ParseDouble(kostprisStr);
                    double antal = ParseDouble(antalStr);
                    double totalpris = ParseDouble(totalprisStr);

                    Console.WriteLine($"[MAT] Række {i}: {beskrivelse}, Antal={antal}, Pris={totalpris}");
                }
            }
        }
    }

    // Hjælpe-metode til at konvertere IBrowserFile til MemoryStream
    private async Task<MemoryStream> GetFileStream(IBrowserFile file)
    {
        long maxFileSize = 1024 * 1024 * 50; // 50 MB
        var stream = new MemoryStream();
        await file.OpenReadStream(maxFileSize).CopyToAsync(stream);
        stream.Position = 0;
        return stream;
    }

    // Hjælpe-metode til at lave den rigtige Reader (xls vs xlsx)
    private IExcelDataReader? GetExcelReader(IBrowserFile file, Stream stream)
    {
        if (file.Name.EndsWith(".xls"))
            return ExcelReaderFactory.CreateBinaryReader(stream);
        if (file.Name.EndsWith(".xlsx"))
            return ExcelReaderFactory.CreateOpenXmlReader(stream);
        
        Console.WriteLine("Ukendt filformat");
        return null;
    }

    // Hjælpe-metode til at sikre vi kan læse tal med både komma og punktum
    private double ParseDouble(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return 0;
        
        if (double.TryParse(input, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
            return result;
            
        return 0;
    }
}