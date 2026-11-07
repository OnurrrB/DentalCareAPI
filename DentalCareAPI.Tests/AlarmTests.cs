using System.Net;
using System.Net.Http.Json;
using DentalCareAPI.DTOs;

namespace DentalCareAPI.Tests;

public class AlarmTests(ApiFactory factory)
{
    private static CancellationToken Ct => TestContext.Current.CancellationToken;

    [Fact]
    public async Task Update_changes_the_same_row_instead_of_adding_one()
    {
        var client = await factory.CreateAuthenticatedClientAsync();
        var alarm = await client.CreateAlarmAsync("Before");

        var update = await client.PutAsJsonAsync($"/api/alarms/{alarm.Id}", ApiClient.NewAlarm("After"), Ct);
        Assert.Equal(HttpStatusCode.NoContent, update.StatusCode);

        var alarms = await client.GetFromJsonAsync<List<AlarmResponse>>("/api/alarms", Ct);
        var only = Assert.Single(alarms!);
        Assert.Equal(alarm.Id, only.Id);
        Assert.Equal("After", only.Label);
    }

    [Fact]
    public async Task Create_returns_the_location_of_the_new_alarm()
    {
        var client = await factory.CreateAuthenticatedClientAsync();

        var response = await client.PostAsJsonAsync("/api/alarms", ApiClient.NewAlarm(), Ct);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var alarm = await client.GetFromJsonAsync<AlarmResponse>(response.Headers.Location, Ct);
        Assert.Equal(new TimeOnly(7, 30), alarm!.Time);
    }

    [Fact]
    public async Task Delete_returns_404_when_the_alarm_does_not_exist()
    {
        var client = await factory.CreateAuthenticatedClientAsync();
        var alarm = await client.CreateAlarmAsync();

        var first = await client.DeleteAsync($"/api/alarms/{alarm.Id}", Ct);
        var second = await client.DeleteAsync($"/api/alarms/{alarm.Id}", Ct);

        Assert.Equal(HttpStatusCode.NoContent, first.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, second.StatusCode);
    }
}
