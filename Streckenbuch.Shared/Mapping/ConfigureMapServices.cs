using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Streckenbuch.Shared.Mapping;

public static class ConfigureMapServices
{
    public static void AddSharedAutoMapper(this IServiceCollection services, params Assembly[] assemblies)
    {

        var mappings = assemblies.SelectMany(x => x.GetTypes()).Where(x => x.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IMap<,>)));


        services.AddAutoMapper(x =>
        {
            MethodInfo createMapMethodInfo = typeof(IProfileExpression).GetMethods().Where(x => x.Name == nameof(IProfileExpression.CreateMap) && x.GetGenericArguments().Length == 2 && !x.GetParameters().Any()).Single();
            foreach (Type mapping in mappings)
            {
                object? map = Activator.CreateInstance(mapping);

                foreach (Type interfaceType in mapping.GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IMap<,>)))
                {
                    Type sourceType = interfaceType.GetGenericArguments().ElementAt(0);
                    Type destinationType = interfaceType.GetGenericArguments().ElementAt(1);

                    MethodInfo specifiyCreateMapMethodInfo = createMapMethodInfo.MakeGenericMethod(sourceType, destinationType);
                    MethodInfo specifiyMapMethod = typeof(IMap<,>).MakeGenericType(sourceType, destinationType).GetMethods().Single();
                    object mappingExpression = specifiyCreateMapMethodInfo.Invoke(x, new object[0])!;

                    specifiyMapMethod.Invoke(map, new object[] { mappingExpression });
                }
            }
        });
    }
}
