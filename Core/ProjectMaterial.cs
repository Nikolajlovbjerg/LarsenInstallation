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