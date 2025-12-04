namespace Core
{
    public class Calculation
    {
        public int CalcId { get; set; }
        public int ProjectId { get; set; }
        // Stamdata
        public Project Project { get; set; }

        // Lister af data
        public List<ProjectHour> Hours { get; set; } = new();
        public List<ProjectMaterial> Materials { get; set; } = new();

        // Bregninger
        public decimal TotalKostPrisMaterialer { get; set; }
        public decimal TotalPrisMaterialer { get; set; }
        
        public decimal TotalTimer { get; set; }
        public decimal TotalKostPrisTimer { get; set; }
        public decimal TotalPrisTimer { get; set; }
        
        // Samlet
        public decimal SamletKostPris => TotalKostPrisMaterialer + TotalKostPrisTimer;
        public decimal SamletTotalPris => TotalPrisMaterialer + TotalPrisTimer;
        public decimal Dækningsgrad => SamletTotalPris > 0 ? (Dækningsbidrag/SamletTotalPris) * 100 : 0;
        public decimal Dækningsbidrag => SamletTotalPris - SamletKostPris;
        
        public string Type { get; set; }
        public decimal TotalHours { get; set; }
    }
}