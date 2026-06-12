# Deployment & configuration

The application needs two secrets that are **not** stored in source control:

| Key | Purpose |
| --- | --- |
| `ConnectionStrings:Default` | PostgreSQL (Neon) connection string |
| `Jwt:Key` | Symmetric signing key for login tokens (HS256, min. 32 bytes) |

If either is missing the server fails fast at startup with a clear message.

## Local development

Secrets are stored in **.NET user-secrets** (machine-local, outside the repo).
Run these once in the repo root:

```bash
# Database connection string
dotnet user-secrets set "ConnectionStrings:Default" \
  "Server=<host>;Port=5432;User Id=<user>;Password=<password>;Database=<db>;SSL Mode=Require;Pooling=true;Trust Server Certificate=true;" \
  --project Server

# JWT signing key (generate a fresh random one)
dotnet user-secrets set "Jwt:Key" "$(openssl rand -base64 48)" --project Server
```

User-secrets are loaded automatically when `ASPNETCORE_ENVIRONMENT=Development`
(the default for the `Server` launch profiles).

To inspect what is stored: `dotnet user-secrets list --project Server`.

## Production

User-secrets are **not** used outside Development. Provide the same two values as
environment variables on the host (note the double-underscore `__` separator):

```bash
ConnectionStrings__Default="Server=<host>;Port=5432;User Id=<user>;Password=<password>;Database=<db>;SSL Mode=Require;Pooling=true;Trust Server Certificate=true;"
Jwt__Key="<a long random base64 string, kept secret>"
```

Also set the client's allowed origin(s) for CORS:

```bash
Cors__AllowedOrigins__0="https://your-client-url"
```

(In Development these default to the local Blazor dev URLs — see
`Server/appsettings.Development.json`.)

## Rotating the database password

If the database password is ever exposed, rotate it in the **Neon console**
(Roles → reset password), then update the stored connection string:

```bash
# local
dotnet user-secrets set "ConnectionStrings:Default" "Server=...;Password=<new>;..." --project Server
# production: update the ConnectionStrings__Default env var on the host
```
