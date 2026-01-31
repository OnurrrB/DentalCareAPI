namespace DentalCareAPI.Entities
{
    public class BrushLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public DateTime Date { get; set; }
        public int DurationInSeconds { get; set; }
        public string Notes { get; set; }

        // Navigation
        public User User { get; set; }
    }
}
