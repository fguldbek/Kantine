using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using KantineApp;
using KantineApp.Services;
using MongoDB.Driver;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://kantineserverapi.azurewebsites.net") });
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<UserSessionService>();
builder.Services.AddSingleton<ServerUrl>();


await builder.Build().RunAsync();

