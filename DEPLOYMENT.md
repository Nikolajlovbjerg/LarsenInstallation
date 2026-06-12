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

## Deploying to Render (free, single combined service)

The `Server` project serves **both** the API and the Blazor WASM client from one
origin, so only one service is needed and there is no CROSS-origin to configure.
A `Dockerfile` at the repo root builds and runs the whole thing.

1. Push this repo to GitHub.
2. In [Render](https://render.com): **New → Web Service**, connect the repo.
3. Render auto-detects the `Dockerfile`. Choose the **Free** instance type.
4. Add environment variables (Render injects `PORT` automatically; the Dockerfile
   already sets `ASPNETCORE_ENVIRONMENT=Production`):

   | Key | Value |
   | --- | --- |
   | `ConnectionStrings__Default` | the Neon connection string (see above) |
   | `Jwt__Key` | a long random secret (`openssl rand -base64 48`) |

5. **Create Web Service**. Render builds the image and deploys. The app is then at
   `https://<your-service>.onrender.com` — it serves the UI at `/` and the API at
   `/api/...` from the same URL.

Notes:
- **No CORS setup needed** — client and API share an origin. `Cors:AllowedOrigins`
  only matters if you ever split them onto different hosts.
- The **free instance spins down after ~15 min idle**; the first request after that
  takes ~30–60s to wake (cold start). Subsequent requests are fast.
- The database stays on Neon's free tier — nothing to deploy there.

### Run the production image locally (optional sanity check)

```bash
docker build -t larsen .
docker run -p 8080:8080 \
  -e ConnectionStrings__Default="Server=...;Password=...;..." \
  -e Jwt__Key="$(openssl rand -base64 48)" \
  larsen
# open http://localhost:8080
```

## Rotating the database password

If the database password is ever exposed, rotate it in the **Neon console**
(Roles → reset password), then update the stored connection string:

```bash
# local
dotnet user-secrets set "ConnectionStrings:Default" "Server=...;Password=<new>;..." --project Server
# production: update the ConnectionStrings__Default env var on the host
```
