namespace CubeTrackerAPI.Models
{
    public class DailyReturn
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; } // LKR received

        public decimal CubeRate { get; set; } = 2000;

        public decimal Cubes => Amount / CubeRate;
    }
}
