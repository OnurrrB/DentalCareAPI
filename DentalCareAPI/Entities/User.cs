namespace DentalCareAPI.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Gender { get; set; } = string.Empty;

    public bool IsSmoker { get; set; }
    public bool IsAlcoholUser { get; set; }
    public bool HasOrthodontic { get; set; }
    public bool IsBruxism { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<Alarm> Alarms { get; set; } = new List<Alarm>();
    public ICollection<BrushLog> BrushLogs { get; set; } = new List<BrushLog>();
    public Setting? Setting { get; set; }
}
