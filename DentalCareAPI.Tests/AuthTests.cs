using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using DentalCareAPI.DTOs;
using Microsoft.AspNetCore.Hosting;

namespace DentalCareAPI.Tests;

public class AuthTests(ApiFactory factory)
{
    private static CancellationToken Ct => TestContext.Current.CancellationToken;

    [Fact]
    public async Task Registered_user_can_log_in_and_read_their_profile()
    {
        var email = ApiClient.NewEmail();
        var client = factory.CreateClient();

        var register = await client.PostAsJsonAsync("/api/auth/register", ApiClient.NewRegistration(email), Ct);
        Assert.Equal(HttpStatusCode.Created, register.StatusCode);

        var login = await client.PostAsJsonAsync("/api/auth/login", new { email, password = ApiClient.Password }, Ct);
        Assert.Equal(HttpStatusCode.OK, login.StatusCode);

        var auth = await login.Content.ReadFromJsonAsync<AuthResponse>(Ct);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth!.AccessToken);

        var profile = await client.GetFromJsonAsync<UserResponse>("/api/users/me", Ct);
        Assert.Equal(email, profile!.Email);
    }

    [Fact]
    public async Task Response_never_contains_the_password_hash()
    {
        var client = await factory.CreateAuthenticatedClientAsync();

        var body = await client.GetStringAsync("/api/users/me", Ct);

        Assert.DoesNotContain("password", body, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Wrong_password_returns_401()
    {
        var email = ApiClient.NewEmail();
        var client = factory.CreateClient();
        await client.PostAsJsonAsync("/api/auth/register", ApiClient.NewRegistration(email), Ct);

        var login = await client.PostAsJsonAsync("/api/auth/login", new { email, password = "wrong-password" }, Ct);

        Assert.Equal(HttpStatusCode.Unauthorized, login.StatusCode);
    }

    [Fact]
    public async Task Duplicate_email_returns_409_regardless_of_case()
    {
        var email = ApiClient.NewEmail();
        var client = factory.CreateClient();
        await client.PostAsJsonAsync("/api/auth/register", ApiClient.NewRegistration(email), Ct);

        var second = await client.PostAsJsonAsync("/api/auth/register", ApiClient.NewRegistration(email.ToUpperInvariant()), Ct);

        Assert.Equal(HttpStatusCode.Conflict, second.StatusCode);
    }

    [Theory]
    [InlineData("/api/alarms")]
    [InlineData("/api/brush-logs")]
    [InlineData("/api/settings")]
    [InlineData("/api/users/me")]
    public async Task Protected_endpoints_return_401_without_a_token(string url)
    {
        var response = await factory.CreateClient().GetAsync(url, Ct);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Tampered_token_returns_401()
    {
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "not.a.token");

        var response = await client.GetAsync("/api/alarms", Ct);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Token_stops_working_after_the_account_is_deleted()
    {
        var client = await factory.CreateAuthenticatedClientAsync();
        await client.CreateAlarmAsync();

        var delete = await client.DeleteAsync("/api/users/me", Ct);
        Assert.Equal(HttpStatusCode.NoContent, delete.StatusCode);

        var response = await client.GetAsync("/api/alarms", Ct);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Credential_endpoints_are_rate_limited()
    {
        var client = factory
            .WithWebHostBuilder(builder => builder.UseSetting("RateLimiting:CredentialsPerMinute", "2"))
            .CreateClient();
        var body = new { email = ApiClient.NewEmail(), password = "whatever-password" };

        await client.PostAsJsonAsync("/api/auth/login", body, Ct);
        await client.PostAsJsonAsync("/api/auth/login", body, Ct);
        var third = await client.PostAsJsonAsync("/api/auth/login", body, Ct);

        Assert.Equal(HttpStatusCode.TooManyRequests, third.StatusCode);
    }
}
