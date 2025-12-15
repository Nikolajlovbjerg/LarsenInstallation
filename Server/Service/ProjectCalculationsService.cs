using Core;
using Server.Repositories.HourRepositories;
using Server.Repositories.MaterialRepositories;
using Server.Repositories.ProjectRepositories;
using System.Globalization; // Vigtig for TextInfo

namespace Server.Service;

public class ProjectCalculationsService
{
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

    public Calculation? CalculateProject(int projectId)
    {
        var project = _projectRepo.GetById(projectId);
        if (project == null) return null;

        // Hent timer og materialer
        var hours = _hourRepo.GetByProjectId(projectId);
        var materials = _materialRepo.GetByProjectId(projectId);
        
        var dto = new Calculation
        {
            ProjectId = project.ProjectId,
            Project = project,
            Hours = hours,
            Materials = materials
        };

        
        foreach (var m in materials)
        {
            dto.TotalKostPrisMaterialer += (m.Kostpris * m.Antal);
            dto.TotalPrisMaterialer += m.Total;
        }

        
        var employeeGroups = hours.GroupBy(x => x.Medarbejder);
        foreach (var group in employeeGroups)
        {
            
            var normalType = group
                .FirstOrDefault(h => h.Type != null && !h.Type.ToLower().Contains("overtid"))?
                .Type?.ToLower() ?? "svend";

            
            decimal grundSats = 0;
            if (normalType.Contains("lærling")) grundSats = project.LærlingTimePris;
            else if (normalType.Contains("konsulent")) grundSats = project.KonsulentTimePris;
            else if (normalType.Contains("arbejdsmand")) grundSats = project.ArbejdsmandTimePris;
            else grundSats = project.SvendTimePris;
            
            foreach (var h in group)
            {
                decimal faktor = 1.0m;
                string typeLower = h.Type?.ToLower() ?? "";

                if (typeLower.Contains("overtid 1")) faktor = 1.5m;
                else if (typeLower.Contains("overtid 2")) faktor = 2.0m;

                decimal salgsPris = h.Timer * grundSats * faktor;

                dto.TotalPrisTimer += salgsPris;
                dto.TotalKostPrisTimer += h.Kostpris;
            }
        }

        dto.TotalTimer = dto.Hours.Sum(h => h.Timer);

        //  5. LOGIK FLYTTET FRA CLIENT TIL SERVER: GRUPPERING
  

        // A. Gruppering af Timer (Svend, Lærling osv.)
        dto.GroupedHours = hours
            .GroupBy(h => {
                var t = h.Type?.ToLower() ?? "";
                if (t.Contains("overtid 1")) return "Overtid 1";
                if (t.Contains("overtid 2")) return "Overtid 2";
                if (t.Contains("lærling")) return "Lærling";
                if (t.Contains("konsulent")) return "Konsulent";
                if (t.Contains("arbejdsmand")) return "Arbejdsmand";
                return "Svend";
            })
            .Select(g => new HourGroupDto { Type = g.Key, Total = g.Sum(x => x.Timer) })
            .OrderByDescending(x => x.Total)
            .ToList();

        // B. Gruppering af Materialer (KUNDEVISNING - Kategorier)
        var categories = new Dictionary<string, string[]>
        {
            { "Belysning", new[] { "spot", "lampe", "led", "lys", "armatur", "pendel", "driver", "dæmper", "skinne" } },
            { "Kabler & Rør", new[] { "kabel", "ledning", "rør", "nkt", "pvi", "flex", "5x1,5", "3x1,5", "5x2,5" } },
            { "Installation", new[] { "stikkontakt", "afbryder", "underlag", "dåse", "fuga", "opus", "ramme", "tangent" } },
            { "Sikringer & Tavler", new[] { "tavle", "sikring", "hpfi", "rce", "automatsikring" } }
        };


        dto.GroupedMaterialsClientView = materials
            .GroupBy(m => {
                string desc = m.Beskrivelse?.ToLower() ?? "";
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

        // C. Gruppering af Materialer (INTERN VISNING - Leverandør/Navn)
        // Kendte leverandører
        string[] knownSuppliers = { "Anker & Co", "Solar", "Lemvigh-Müller", "AO" };
        TextInfo textInfo = new CultureInfo("da-DK", false).TextInfo;

        dto.GroupedMaterialsInternView = materials
            .GroupBy(m => {
                string desc = m.Beskrivelse?.Trim() ?? "";
                // Match på leverandør
                foreach (var supplier in knownSuppliers)
                {
                    if (desc.Contains(supplier, StringComparison.OrdinalIgnoreCase)) return supplier;
                }
                return desc.ToLower();
            })
            .Select(g => new ProjectMaterial
            {
                Beskrivelse = knownSuppliers.Contains(g.Key) ? g.Key : textInfo.ToTitleCase(g.Key),
                Total = g.Sum(x => x.Total)
            })
            .OrderByDescending(m => m.Total)
            .ToList();

        return dto;
    }
}