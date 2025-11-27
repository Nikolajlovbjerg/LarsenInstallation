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