namespace Core
{
    public class Calculation
    {
        public int CalcId { get; set; }
        public int ProjectId { get; set; }

        public decimal TotalMaterialCost { get; set; }
        public decimal TotalHourlyCost { get; set; }
        public decimal TotalCustomerPrice { get; set; }
        public decimal TotalEarnings { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}