global using Blazored.LocalStorage;
global using Fora.Client.Services.InterestService;
global using Fora.Client.Services.ThreadService;
global using Fora.Client.Services.SettingsService;
global using Microsoft.AspNetCore.Components.Authorization;
using Fora.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<IInterestService, InterestService>();
builder.Services.AddScoped<IThreadService, ThreadService>();
builder.Services.AddScoped<ISettingsService, SettingsService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();
