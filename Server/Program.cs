using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Server.Repositories.User;
using Server.Repositories.HourRepositories;
using Server.Repositories.MaterialRepositories;
using Server.Repositories.ProjectRepositories;
using Server.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);


builder.Services.AddScoped<IProjectRepository, ProjectRepositorySQL>();
builder.Services.AddScoped<IHourRepository, HourRepositorySQL>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepositorySQL>();

builder.Services.AddScoped<ProjectCalculationsService>();

builder.Services.AddSingleton<ICreateUserRepo, CreateUserRepoSQL>();

builder.Services.AddScoped<JwtTokenService>();

// JWT-baseret authentication (minimal: symmetrisk nøgle, HS256, kun signatur + levetid valideres)
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key mangler. Sæt den via user-secrets eller Jwt__Key.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();

// Tilladte origins læses fra konfiguration (appsettings). Falder tilbage til de lokale
// udviklings-URL'er, så 'dotnet run' altid virker lokalt. Tilføj produktions-URL i appsettings.
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
if (allowedOrigins is null || allowedOrigins.Length == 0)
{
    allowedOrigins = new[] { "http://localhost:5255", "https://localhost:7042" };
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("policy",
        policy =>
        {
            policy.WithOrigins(allowedOrigins);
            policy.AllowAnyMethod();
            policy.AllowAnyHeader();
        });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Server hoster også Blazor WASM-klienten (samme origin → ingen CORS nødvendig)
app.UseBlazorFrameworkFiles();

// index.html og den scoped CSS-bundle (Client.styles.css) er IKKE fingerprintede.
// De må derfor ikke caches på tværs af deploys — ellers kan en browser holde en gammel
// CSS-bundle, hvis scope-hashes (b-xxxx) ikke matcher det nye DOM, og komponent-styles forsvinder.
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        var name = ctx.File.Name;
        if (name == "index.html" || name.EndsWith(".styles.css", StringComparison.OrdinalIgnoreCase))
            ctx.Context.Response.Headers.CacheControl = "no-cache, must-revalidate";
    }
});

app.UseCors("policy");

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Alle ruter der ikke er /api/... serverer Blazor-klientens index.html (SPA-routing).
// Samme no-cache som ovenfor, så fallback-leveret index.html heller ikke bliver stale.
app.MapFallbackToFile("index.html", new StaticFileOptions
{
    OnPrepareResponse = ctx =>
        ctx.Context.Response.Headers.CacheControl = "no-cache, must-revalidate"
});

app.Run();