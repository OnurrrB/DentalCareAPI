namespace DentalCareAPI.DTOs;

public class SettingUpdateDto
{
    public int UserId { get; set; }
    public string Language { get; set; } = string.Empty;
}
