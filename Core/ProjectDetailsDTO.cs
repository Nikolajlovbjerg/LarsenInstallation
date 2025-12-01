namespace Core
{
    public class ProjectDetailsDTO
    {
        // Stamdata
        public Project Project { get; set; }
        
        // Lister (hvis vi vil vise detaljerne i tabeller)
        public List<ProjectHour> Hours { get; set; } = new();
        public List<ProjectMaterial> Materials { get; set; } = new();

        // Beregnede Totaler
        public decimal TotalKostprisMaterialer { get; set; }
        public decimal TotalSalgsprisMaterialer { get; set; }
        
        public decimal TotalTimer { get; set; }
        public decimal TotalKostprisTimer { get; set; }
        public decimal TotalSalgsprisTimer { get; set; }

        // Samlet
        public decimal SamletKostpris => TotalKostprisMaterialer + TotalKostprisTimer;
        public decimal SamletSalgspris => TotalSalgsprisMaterialer + TotalSalgsprisTimer;
        public decimal Dækningsbidrag => SamletSalgspris - SamletKostpris;
        public decimal Dækningsgrad => SamletSalgspris > 0 ? (Dækningsbidrag / SamletSalgspris) * 100 : 0;
    }
}