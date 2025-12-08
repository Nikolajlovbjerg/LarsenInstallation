using Core;
using Server.Repositories.HourRepositories;
using Server.Repositories.MaterialRepositories;
using Server.Repositories.ProjectRepositories;

namespace Server.Service;

public class ProjectCalculationsService
{
    private readonly IProjectRepositorySQL _projectRepo;
    private readonly IHourRepositorySQL _hourRepo;
    private readonly IMaterialRepositorySQL _materialRepo;

    public ProjectCalculationsService(
        IProjectRepositorySQL projectRepo, 
        IHourRepositorySQL hourRepo, 
        IMaterialRepositorySQL materialRepo)
    {
        _projectRepo = projectRepo;
        _hourRepo = hourRepo;
        _materialRepo = materialRepo;
    }

    public Calculation? CalculateProject(int projectId)
    {
        // 1. Hent data
        var project = _projectRepo.GetById(projectId);
        if (project == null) return null;

        var hours = _hourRepo.GetByProjectId(projectId);
        var materials = _materialRepo.GetByProjectId(projectId);

        // 2. Opret DTO
        var dto = new Calculation
        {
            ProjectId = project.ProjectId,
            Project = project,
            Hours = hours,
            Materials = materials
        };

        // 3. Beregn Materialer
        foreach (var m in materials)
        {
            dto.TotalKostPrisMaterialer += (m.Kostpris * m.Antal);
            dto.TotalPrisMaterialer += m.Total;
        }

        // 4. Beregn Timer (Logik flyttet fra SQL Repo)
        var employeeGroups = hours.GroupBy(x => x.Medarbejder);

        foreach (var group in employeeGroups)
        {
            // A. Find rollen (kig efter linjer der IKKE er overtid)
            var normalType = group
                .FirstOrDefault(h => h.Type != null && !h.Type.ToLower().Contains("overtid"))?
                .Type?.ToLower() ?? "svend";

            // B. Find grundsats
            decimal grundSats = 0;
            if (normalType.Contains("lærling"))       grundSats = project.LærlingTimePris;
            else if (normalType.Contains("konsulent")) grundSats = project.KonsulentTimePris;
            else if (normalType.Contains("arbejdsmand")) grundSats = project.ArbejdsmandTimePris;
            else                                       grundSats = project.SvendTimePris;

            // C. Beregn pris inkl. overtidstillæg
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

        // Opdater total timer
        dto.TotalTimer = dto.Hours.Sum(h => h.Timer);

        return dto;
    }
}