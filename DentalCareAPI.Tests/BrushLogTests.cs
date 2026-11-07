using System.Net.Http.Json;
using DentalCareAPI.DTOs;

namespace DentalCareAPI.Tests;

public class BrushLogTests(ApiFactory factory)
{
    private static CancellationToken Ct => TestContext.Current.CancellationToken;

    [Fact]
    public async Task History_is_paged_and_newest_first()
    {
        var client = await factory.CreateAuthenticatedClientAsync();
        var now = DateTimeOffset.UtcNow;
        foreach (var hoursAgo in new[] { 3, 1, 2 })
        {
            var created = await client.PostAsJsonAsync("/api/brush-logs", new
            {
                brushedAt = now.AddHours(-hoursAgo),
                durationInSeconds = 100 + hoursAgo,
            }, Ct);
            created.EnsureSuccessStatusCode();
        }

        var firstPage = await client.GetFromJsonAsync<PagedResponse<BrushLogResponse>>("/api/brush-logs?page=1&pageSize=2", Ct);
        var secondPage = await client.GetFromJsonAsync<PagedResponse<BrushLogResponse>>("/api/brush-logs?page=2&pageSize=2", Ct);

        Assert.Equal(3, firstPage!.TotalCount);
        Assert.Equal([101, 102], firstPage.Items.Select(b => b.DurationInSeconds));
        Assert.Equal([103], secondPage!.Items.Select(b => b.DurationInSeconds));
    }

    [Fact]
    public async Task Page_size_is_capped()
    {
        var client = await factory.CreateAuthenticatedClientAsync();

        var page = await client.GetFromJsonAsync<PagedResponse<BrushLogResponse>>("/api/brush-logs?pageSize=100000", Ct);

        Assert.Equal(100, page!.PageSize);
    }
}
