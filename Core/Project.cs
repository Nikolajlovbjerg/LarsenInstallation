namespace Core;

public class Project
{
    public int ProjectId { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTime DateCreated { get; set; }

    // Timepriser pr. type medarbejder
    public decimal SvendTimePris { get; set; }
    public decimal LærlingTimePris { get; set; }
    public decimal KonsulentTimePris { get; set; }
    public decimal ArbejdsmandTimePris { get; set; }

    // Liste af huse til projektet
    public List<House>? Houses { get; set; }

    // Total omkostninger kunde/virksomhed (dummy eller beregnet senere)
    public decimal TotalCostCustomer { get; set; }
    public decimal TotalCostCompany { get; set; }

    public List<TimeEntry> TimeEntries { get; set; } = new();
}

public class House
{
    public string Name { get; set; } = string.Empty;

    public decimal CostCustomer { get; set; }
    public decimal CostCompany { get; set; }
    public double Hours { get; set; }

    public Dictionary<string, decimal> InstallationCosts { get; set; } = new();
    public Dictionary<string, int> MaterialsUsed { get; set; } = new();
}   


public class TimeEntry
{
    public DateTime Date { get; set; }
    public double Hours { get; set; }
}