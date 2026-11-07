using System.Net;
using System.Net.Http.Json;
using DentalCareAPI.DTOs;

namespace DentalCareAPI.Tests;

public class SettingsAndTipsTests(ApiFactory factory)
{
    private static CancellationToken Ct => TestContext.Current.CancellationToken;

    [Fact]
    public async Task New_user_gets_default_settings_and_can_change_them()
    {
        var client = await factory.CreateAuthenticatedClientAsync();

        var initial = await client.GetFromJsonAsync<SettingResponse>("/api/settings", Ct);
        Assert.Equal("en", initial!.Language);

        var update = await client.PutAsJsonAsync("/api/settings", new { language = "tr" }, Ct);
        Assert.Equal(HttpStatusCode.NoContent, update.StatusCode);

        var changed = await client.GetFromJsonAsync<SettingResponse>("/api/settings", Ct);
        Assert.Equal("tr", changed!.Language);
    }

    [Fact]
    public async Task Unsupported_language_is_rejected()
    {
        var client = await factory.CreateAuthenticatedClientAsync();

        var response = await client.PutAsJsonAsync("/api/settings", new { language = "xx" }, Ct);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Tips_are_public_and_seeded()
    {
        var client = factory.CreateClient();

        var tips = await client.GetFromJsonAsync<List<TipResponse>>("/api/tips", Ct);
        var first = await client.GetFromJsonAsync<TipResponse>($"/api/tips/{tips![0].Id}", Ct);

        Assert.Equal(4, tips.Count);
        Assert.Equal(tips[0].Title, first!.Title);
    }
}
