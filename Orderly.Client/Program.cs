
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Orderly.Client;
using Orderly.Client.Providers;
using Orderly.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Load app configurations

using var http = new HttpClient {
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
};

using var stream = await http.GetStreamAsync("appsettings.json");

var config = new ConfigurationBuilder()
    .AddJsonStream(stream)
    .Build();

builder.Services.AddSingleton<IConfiguration>(config);

// Configure API base address

builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ChatService>();
builder.Services.AddScoped<MarketplaceService>();

builder.Services.AddAuthorizationCore();

//

await builder.Build().RunAsync();
