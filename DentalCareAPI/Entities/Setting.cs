namespace DentalCareAPI.Entities
{
    public class Setting
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string Language { get; set; }

        // Navigation
        public User User { get; set; }
    }
}
