namespace Core;

public class Project
{
    public int ProjectId { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    
    public int SvendTimePris { get; set; }
    
    public int LærlingTimePris { get; set; }
    
    public int KonsulentTimePris { get; set; }
    
    public int ArbejdsmandTimePris { get; set; }
    public int TotalCostCompany { get; set; }
    public int TotalCostCustomer { get; set; }
}