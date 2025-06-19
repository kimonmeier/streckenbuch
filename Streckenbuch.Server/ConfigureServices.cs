using Microsoft.AspNetCore.Identity;
using Streckenbuch.Server.Configuration;
using Streckenbuch.Server.Data.Entities;
using Streckenbuch.Server.Data.Repositories;
using Streckenbuch.Server.Mappings;
using Streckenbuch.Shared.Mapping;
using Streckenbuch.Shared.Models;
using System.Text;

namespace Streckenbuch.Server;

public static class ConfigureServices
{
    public static void AddCustomAutoMapper(this IServiceCollection services)
    {
        services.AddSharedAutoMapper(typeof(Program).Assembly);
        services.AddTransient<FahrenEntryConverter>();
        services.AddTransient(typeof(RepeatedFieldConverter<>));
    }

    public static void AddCustomServices(this IServiceCollection services)
    {
        services.AddTransient<StreckenKonfigurationResolver>();
        services.AddTransient<LinienStreckenResolver>();
    }

    public static void AddRepostories(this IServiceCollection services)
    {
        services.AddTransient<StreckenRepository>();
        services.AddTransient<StreckenKonfigurationRepository>();
        services.AddTransient<BetriebspunkteRepository>();
        services.AddTransient<BetriebspunktStreckenZuordnungRepository>();
        services.AddTransient<SignalRepository>();
        services.AddTransient<SignalStreckenZuordnungRepository>();
        services.AddTransient<SignalStreckenZuordnungSortingStreckeRepository>();
        services.AddTransient<SignalStreckenZuordnungSortingBetriebspunktRepository>();
        services.AddTransient<SignalStreckenZuordnungSortingSignalRepository>();
        services.AddTransient<FahrenRepository>();
        services.AddTransient<LinienRepository>();
        services.AddTransient<LinienKonfigurationRepository>();
        services.AddTransient<LinieTrainRepository>();
    }

    public static async Task CreateRoles(this IServiceProvider provider)
    {
        using (var scope = provider.CreateScope())
        {
            //initializing custom roles 
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = [Permissions.Admin];
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }

    public static void AddMail(this WebApplicationBuilder app)
    {
        MailConfiguration? mailConfiguration = app.Configuration.GetSection("Mail")?.Get<MailConfiguration>();
        WebsiteConfiguration? websiteConfiguration = app.Configuration.GetSection("Website")?.Get<WebsiteConfiguration>();


        if (mailConfiguration is null)
        {
            throw new Exception("The Mail section is missing in the configuration");
        }
        if (websiteConfiguration is null)
        {
            throw new Exception("The Website section is missing in the configuration");
        }

        app.Services
            .AddFluentEmail(mailConfiguration.Mail!, $"{websiteConfiguration.Name} - No-Reply")
            .AddMailKitSender(new FluentEmail.MailKitSmtp.SmtpClientOptions()
            {
                Server = mailConfiguration.Server,
                Port = mailConfiguration.Port,
                UseSsl = mailConfiguration.SSL,
                User = mailConfiguration.Username,
                Password = mailConfiguration.Password,
                RequiresAuthentication = true,
                PreferredEncoding = Encoding.UTF8.EncodingName,
            });
    }
}
