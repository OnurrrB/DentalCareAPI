using System.Net;
using System.Net.Http.Json;
using DentalCareAPI.DTOs;

namespace DentalCareAPI.Tests;

public class OwnershipTests(ApiFactory factory)
{
    private static CancellationToken Ct => TestContext.Current.CancellationToken;

    [Fact]
    public async Task Another_users_alarm_looks_like_it_does_not_exist()
    {
        var owner = await factory.CreateAuthenticatedClientAsync();
        var stranger = await factory.CreateAuthenticatedClientAsync();
        var alarm = await owner.CreateAlarmAsync("Private");

        var get = await stranger.GetAsync($"/api/alarms/{alarm.Id}", Ct);
        var put = await stranger.PutAsJsonAsync($"/api/alarms/{alarm.Id}", ApiClient.NewAlarm("Stolen"), Ct);
        var delete = await stranger.DeleteAsync($"/api/alarms/{alarm.Id}", Ct);

        Assert.Equal(HttpStatusCode.NotFound, get.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, put.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, delete.StatusCode);

        var unchanged = await owner.GetFromJsonAsync<AlarmResponse>($"/api/alarms/{alarm.Id}", Ct);
        Assert.Equal("Private", unchanged!.Label);
    }

    [Fact]
    public async Task Another_users_brush_log_looks_like_it_does_not_exist()
    {
        var owner = await factory.CreateAuthenticatedClientAsync();
        var stranger = await factory.CreateAuthenticatedClientAsync();
        var created = await owner.PostAsJsonAsync("/api/brush-logs", new
        {
            brushedAt = DateTimeOffset.UtcNow.AddMinutes(-1),
            durationInSeconds = 120,
        }, Ct);
        var log = await created.Content.ReadFromJsonAsync<BrushLogResponse>(Ct);

        var get = await stranger.GetAsync($"/api/brush-logs/{log!.Id}", Ct);
        var delete = await stranger.DeleteAsync($"/api/brush-logs/{log.Id}", Ct);

        Assert.Equal(HttpStatusCode.NotFound, get.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, delete.StatusCode);
    }

    [Fact]
    public async Task Lists_contain_only_the_callers_records()
    {
        var owner = await factory.CreateAuthenticatedClientAsync();
        var stranger = await factory.CreateAuthenticatedClientAsync();
        await owner.CreateAlarmAsync();

        var alarms = await stranger.GetFromJsonAsync<List<AlarmResponse>>("/api/alarms", Ct);

        Assert.Empty(alarms!);
    }

    [Fact]
    public async Task A_user_id_in_the_body_is_ignored()
    {
        var owner = await factory.CreateAuthenticatedClientAsync();
        var attacker = await factory.CreateAuthenticatedClientAsync();
        var victim = await owner.GetFromJsonAsync<UserResponse>("/api/users/me", Ct);

        var response = await attacker.PostAsJsonAsync("/api/alarms", new
        {
            userId = victim!.Id,
            time = "07:30:00",
            label = "Injected",
        }, Ct);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Empty((await owner.GetFromJsonAsync<List<AlarmResponse>>("/api/alarms", Ct))!);
        Assert.Single((await attacker.GetFromJsonAsync<List<AlarmResponse>>("/api/alarms", Ct))!);
    }
}
