using System.Net.Http.Headers;
using System.Net.Http.Json;
using DentalCareAPI.DTOs;

namespace DentalCareAPI.Tests;

public static class ApiClient
{
    public const string Password = "correct-horse-battery";

    public static object NewRegistration(string? email = null) => new
    {
        email = email ?? NewEmail(),
        password = Password,
        name = "Test User",
        age = 30,
        gender = "Unspecified",
    };

    public static object NewAlarm(string label = "Morning") => new
    {
        time = "07:30:00",
        repeatMonday = true,
        isActive = true,
        label,
    };

    public static string NewEmail() => $"{Guid.NewGuid():N}@example.com";

    /// <summary>Registers a new account and returns a client that sends its token.</summary>
    public static async Task<HttpClient> CreateAuthenticatedClientAsync(this ApiFactory factory)
    {
        var client = factory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/auth/register", NewRegistration());
        response.EnsureSuccessStatusCode();

        var auth = await response.Content.ReadFromJsonAsync<AuthResponse>();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth!.AccessToken);
        return client;
    }

    public static async Task<AlarmResponse> CreateAlarmAsync(this HttpClient client, string label = "Morning")
    {
        var response = await client.PostAsJsonAsync("/api/alarms", NewAlarm(label));
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<AlarmResponse>())!;
    }
}
