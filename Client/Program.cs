using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Client;
using Blazored.LocalStorage;
using Client.Service;
using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Klienten serveres af Server-projektet (samme origin), så vi bruger appens egen base-adresse.
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<UserRepository>();

builder.Services.AddScoped<ProjectService>();


var host = builder.Build();

// Læs et evt. gemt JWT-token fra localStorage og sæt det som Authorization-header,
// så beskyttede API-kald virker — også efter en browser-refresh.
var localStorage = host.Services.GetRequiredService<ILocalStorageService>();
var token = await localStorage.GetItemAsync<string>("token");
if (!string.IsNullOrWhiteSpace(token))
{
    var http = host.Services.GetRequiredService<HttpClient>();
    http.DefaultRequestHeaders.Authorization =
        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
}

await host.RunAsync();