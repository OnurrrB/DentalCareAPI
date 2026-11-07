# DentalCareAPI

[![CI](https://github.com/OnurrrB/DentalCareAPI/actions/workflows/ci.yml/badge.svg)](https://github.com/OnurrrB/DentalCareAPI/actions/workflows/ci.yml)

A REST API for a dental care app. Users keep brushing alarms, log their brushing sessions, store dental
health habits on their profile and read care tips. Each user can reach only their own data.

**Stack:** C#, ASP.NET Core 10, EF Core 10, SQL Server, JWT, FluentValidation, xUnit, Testcontainers, Docker.

## Run it

You need Docker. This starts the API and SQL Server and applies the migrations:

```bash
cp .env.example .env
docker compose up --build
```

Open <http://localhost:5161/swagger>, call `POST /api/auth/register`, then paste the returned token into
**Authorize**. [`DentalCareAPI.http`](DentalCareAPI/DentalCareAPI.http) has ready-made requests.

### Without Docker

You need the .NET 10 SDK and a local SQL Server. The default connection string uses `.\SQLEXPRESS`.

```bash
dotnet tool restore
dotnet user-secrets set "Jwt:Key" "any-local-value-of-at-least-32-bytes" --project DentalCareAPI
dotnet ef database update --project DentalCareAPI
dotnet run --project DentalCareAPI
```

## Endpoints

| Method | Path | Purpose | Auth |
|---|---|---|---|
| POST | `/api/auth/register` | Create an account and get a token | No |
| POST | `/api/auth/login` | Get a token | No |
| GET, PUT, DELETE | `/api/users/me` | Read, update or delete your account | Yes |
| GET, POST | `/api/alarms` | List or create alarms | Yes |
| GET, PUT, DELETE | `/api/alarms/{id}` | Read, update or delete an alarm | Yes |
| GET, POST | `/api/brush-logs` | Paged history, newest first, or add a session | Yes |
| GET, DELETE | `/api/brush-logs/{id}` | Read or delete a session | Yes |
| GET, PUT | `/api/settings` | Read or change your language (`en`, `tr`) | Yes |
| GET | `/api/tips`, `/api/tips/{id}` | Read care tips | No |

Errors use [ProblemDetails](https://www.rfc-editor.org/rfc/rfc9457). Validation failures list the errors
for each field.

## Structure

```
DentalCareAPI/
  Auth/          JWT options, token creation, current user
  Controllers/   HTTP endpoints
  Data/          DbContext and migrations
  DTOs/          Request and response contracts
  Entities/      Database model
  Errors/        Global exception handler
  Interfaces/    Repository contracts
  Mappings/      Entity and DTO conversion
  Repositories/  EF Core queries
  Validation/    FluentValidation rules and the action filter
DentalCareAPI.Tests/  Integration tests
```

## Design decisions

- **The user id comes from the token, never from the request.** Repository queries take the owner's id,
  so ownership is enforced where the data is read.
- **Another user's record returns 404, not 403.** A 403 would confirm that the id exists.
- **Authentication is the default.** A fallback policy protects every endpoint, and public ones opt out with
  `[AllowAnonymous]`, so a new endpoint cannot be left open by accident.
- **A deleted account's token stops working.** Each authenticated request checks that the user still exists,
  at the cost of one primary-key lookup.
- **Mapping is written by hand.** The mappings are small, and explicit code keeps a client from setting a
  field such as `PasswordHash` by mistake.
- **No CORS policy.** The client is a native mobile app, which browsers' CORS rules do not apply to.

## Tests

The integration tests start the real application against a real SQL Server container, created by the same
migrations production would use. They need Docker running.

```bash
dotnet test
```

They cover the auth flow, isolation between two users, validation, error responses, paging and rate limiting.

## Known limits

- There are no refresh tokens, so users log in again when the token expires (60 minutes by default).
- There are no roles and no admin endpoints; tips are managed through migrations.
- Email addresses are not verified, and there is no password reset.
- The rate limit on auth endpoints is per IP and held in memory. It resets on restart and is not shared
  between instances.
- The API serves plain HTTP and expects a reverse proxy to terminate TLS. It is not deployed anywhere yet.

## License

[MIT](LICENSE)
