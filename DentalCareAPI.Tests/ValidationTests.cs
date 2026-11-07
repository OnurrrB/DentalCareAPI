using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;

namespace DentalCareAPI.Tests;

public class ValidationTests(ApiFactory factory)
{
    private static CancellationToken Ct => TestContext.Current.CancellationToken;

    [Fact]
    public async Task Invalid_registration_returns_validation_problem_details()
    {
        var response = await factory.CreateClient().PostAsJsonAsync("/api/auth/register", new
        {
            email = "not-an-email",
            password = "short",
            name = "",
            age = -1,
        }, Ct);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);

        var problem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(Ct);
        Assert.Equal(400, problem!.Status);
        Assert.Equal(
            ["Age", "Email", "Name", "Password"],
            problem.Errors.Keys.Order().ToArray());
    }

    [Fact]
    public async Task Future_brush_log_is_rejected()
    {
        var client = await factory.CreateAuthenticatedClientAsync();

        var response = await client.PostAsJsonAsync("/api/brush-logs", new
        {
            brushedAt = DateTimeOffset.UtcNow.AddDays(1),
            durationInSeconds = 120,
        }, Ct);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    [InlineData(1801)]
    public async Task Brush_duration_must_be_realistic(int seconds)
    {
        var client = await factory.CreateAuthenticatedClientAsync();

        var response = await client.PostAsJsonAsync("/api/brush-logs", new
        {
            brushedAt = DateTimeOffset.UtcNow.AddMinutes(-1),
            durationInSeconds = seconds,
        }, Ct);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Malformed_time_returns_400()
    {
        var client = await factory.CreateAuthenticatedClientAsync();

        var response = await client.PostAsJsonAsync("/api/alarms", new { time = "25:00:00", label = "Bad" }, Ct);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Unknown_id_returns_problem_details()
    {
        var client = await factory.CreateAuthenticatedClientAsync();

        var response = await client.GetAsync("/api/alarms/2147483647", Ct);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
    }
}
