using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components;
using Streckenbuch.Client.Mappings;
using Streckenbuch.Client.States;
using Streckenbuch.Shared.Mapping;

namespace Streckenbuch.Client;

public static class ConfigureServices
{
    public static void AddClientMapping(this IServiceCollection services)
    {
        services.AddSharedAutoMapper(typeof(Program).Assembly);
        services.AddTransient<StreckenKonfigurationResolver>();
        services.AddTransient<FahrenEntryConverter>();
    }

    public static void AddMediator(this IServiceCollection services)
    {
        services.AddMediatR((x) => x.RegisterServicesFromAssembly(typeof(Program).Assembly));
        services.AddSingleton<ContinuousConnectionState>();
    }

    public static void AddGrpcService<TService>(this IServiceCollection services) where TService : Grpc.Core.ClientBase
    {
        if (!services.Any(x => x.ServiceType == typeof(GrpcChannel)))
        {
            services.AddSingleton(services =>
            {
                var baseUri = services.GetRequiredService<NavigationManager>().BaseUri;
                return GrpcChannel.ForAddress(baseUri, new GrpcChannelOptions()
                {
                    HttpHandler = new GrpcWebHandler(new HttpClientHandler())
                });
            });
        }

        services.AddSingleton(services =>
        {
            var channel = services.GetRequiredService<GrpcChannel>();
            return (TService)Activator.CreateInstance(typeof(TService), channel)!;
        });
    }
}
