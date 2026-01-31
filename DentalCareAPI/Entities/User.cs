namespace DentalCareAPI.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }

        public bool IsSmoker { get; set; }
        public bool IsAlcoholUser { get; set; }
        public bool HasOrthodontic { get; set; }
        public bool IsBruxizim { get; set; }

        public DateTime CreatedAt { get; set; }

        // Navigation
        public ICollection<Alarm> Alarms { get; set; }
        public ICollection<BrushLog> BrushLogs { get; set; }
        public Setting Setting { get; set; }
    }
}
