using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using RetroLibrary.Core.Components;
using RetroLibrary.Core.Interfaces;
using RetroLibrary.Core.Resources;

namespace RetroLibrary.XmlLoader.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddRetroLibraryXmlLoader(this IServiceCollection services)
    {
        services.AddScoped<IRetroGameLoaderService, XmlRetroGameLoaderService>();
        AddAllOfType<IResourceLoader>(services);
        AddAllOfType<IComponentLoader>(services);
    }

    private static void AddAllOfType<T>(IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var allTypes = assembly.GetTypes().Where(x => typeof(T).IsAssignableFrom(x) && !x.IsInterface).ToList();
        foreach (var curType in allTypes)
        {
            services.AddScoped(typeof(T), curType);
        }
    }
}
