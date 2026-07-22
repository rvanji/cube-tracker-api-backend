namespace CubeTrackerAPI.DTOs
{
    public class SummaryResponseDto
    {
        public decimal TotalAmount { get; set; }

        public decimal TotalCubes { get; set; }

        public decimal TargetCubes { get; set; }

        public decimal RemainingCubes { get; set; }

        public decimal ProgressPercentage { get; set; }

        public decimal DailyAverage { get; set; }

        public decimal TodayAmount { get; set; }

        public decimal TodayCubes { get; set; }

        public int TotalEntries { get; set; }
    }
}
