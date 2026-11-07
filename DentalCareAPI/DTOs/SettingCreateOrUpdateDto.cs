namespace DentalCareAPI.DTOs;

public class SettingCreateOrUpdateDto
{
    public int UserId { get; set; }
    public string Language { get; set; } = string.Empty;
}
