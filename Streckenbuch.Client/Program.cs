using GoogleMapsComponents;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Streckenbuch.Client;
using Streckenbuch.Client.Services;
using Streckenbuch.Shared.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
builder.Services.AddSingleton<FahrenPositionService>();
builder.Services.AddMudServices();
builder.Services.AddMudPopoverService();
builder.Services.AddClientMapping();
builder.Services.AddGeolocationServices();
builder.Services.AddBlazorGoogleMaps("AIzaSyBg9oVLq85w_iGU_krKnId50ZhtmO3wZRA");
builder.Services.AddTransient<ClipboardService>();
builder.Services.AddGrpcService<StreckenService.StreckenServiceClient>();
builder.Services.AddGrpcService<BetriebspunkteService.BetriebspunkteServiceClient>();
builder.Services.AddGrpcService<SignaleService.SignaleServiceClient>();
builder.Services.AddGrpcService<FahrenService.FahrenServiceClient>();

await builder.Build().RunAsync();