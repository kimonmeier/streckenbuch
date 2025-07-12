using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Newtonsoft.Json;
using Streckenbuch.Components.Account;
using Streckenbuch.Miku;
using Streckenbuch.Miku.Models.Fahrten;
using Streckenbuch.Server;
using Streckenbuch.Server.Background;
using Streckenbuch.Server.Components;
using Streckenbuch.Server.Components.Account;
using Streckenbuch.Server.Configuration;
using Streckenbuch.Server.Data;
using Streckenbuch.Server.Data.Entities;
using Streckenbuch.Server.Data.Entities.Betriebspunkte;
using Streckenbuch.Server.Data.Repositories;
using Streckenbuch.Server.Services;
using Streckenbuch.Server.States;
using Streckenbuch.Shared.Data;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.AddMail();

builder.Configuration.AddCommandLine(args);
builder.Configuration.AddEnvironmentVariables();
builder.Configuration.AddJsonFile("./data/appsettings.json", true);
builder.Configuration.AddJsonFile("./data/appsettings.Development.json", true);
builder.Configuration.AddJsonFile("appsettings.json", true);
builder.Configuration.AddJsonFile("appsettings.Development.json", true);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingServerAuthenticationStateProvider>();
builder.Services.AddTransient<EmailSender>();
builder.Services.AddMudServices();
builder.Services.AddGrpc(options =>
{
    options.EnableDetailedErrors = true;
    options.MaxReceiveMessageSize = 2 * 1024 * 1024; // 2 MB
    options.MaxSendMessageSize = 5 * 1024 * 1024; // 5 MB
});
builder.Services.AddRepostories();
builder.Services.AddCustomAutoMapper();
builder.Services.AddCustomServices();

builder.Services.AddSingleton((services) => services.GetRequiredService<IConfiguration>().GetSection("Mail")?.Get<MailConfiguration>() ?? new MailConfiguration());
builder.Services.AddSingleton((services) => services.GetRequiredService<IConfiguration>().GetSection("Website")?.Get<WebsiteConfiguration>() ?? new WebsiteConfiguration());
builder.Services.AddSingleton(services => new MikuApi(services.GetRequiredService<WebsiteConfiguration>().MikuLink!));

builder.Services.AddSingleton<ContinuousConnectionState>();
builder.Services.AddHostedService<UpdateBackgroundInformation>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString).EnableDetailedErrors());
builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
builder.Services.AddScoped<DbTransactionFactory>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddRoleManager<RoleManager<IdentityRole>>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, EmailSender>();

var app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseGrpcWeb(new GrpcWebOptions
{
    DefaultEnabled = true,
});
// Services
app.MapGrpcService<StreckenService>().EnableGrpcWeb();
app.MapGrpcService<BetriebspunkteService>().EnableGrpcWeb();
app.MapGrpcService<SignaleService>().EnableGrpcWeb();
app.MapGrpcService<FahrenService>().EnableGrpcWeb();
app.MapGrpcService<LinienService>().EnableGrpcWeb();
app.MapGrpcService<RecordingService>().EnableGrpcWeb();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(Streckenbuch.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Services.CreateRoles().GetAwaiter().GetResult();

app.Run();
