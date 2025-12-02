using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client;
using Blazored.LocalStorage;
using Client.Service;
using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5028/") });

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddSingleton<IProjectService, ProjectServiceMock>();

await builder.Build().RunAsync();