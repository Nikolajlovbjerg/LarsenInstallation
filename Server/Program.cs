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

app.UseCors("policy");

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();