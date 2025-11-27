using Server.Repositories.User;
using Server.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSingleton<IProjectRepository, ProjectRepositorySQL>();

builder.Services.AddSingleton<ICreateUserRepoSQL, CreateUserRepoSQL>();

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("policy",
        policy =>
        {
            policy.AllowAnyOrigin();
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

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();