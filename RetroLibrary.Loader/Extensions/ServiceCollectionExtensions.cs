using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RetroLibrary.Loader.Common;
using RetroLibrary.Loader.Components;
using RetroLibrary.Loader.Resources;

namespace RetroLibrary.Loader.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddRetroLibraryXmlLoader(this IServiceCollection services)
    {
        services.AddSingleton<IResourceManager, ResourceManager>();

        services.AddScoped<IColorLoader, ColorLoader>();
        services.AddScoped<IRetroGameLoaderService, XmlRetroGameLoaderService>();

        AddAllOfType<IResourceLoader>(services);
        AddAllOfType<IComponentLoader>(services);
    }

    private static void AddAllOfType<T>(IServiceCollection services)
    {
        var assembly = typeof(T).Assembly;
        var allTypes = assembly.GetTypes().Where(x => typeof(T).IsAssignableFrom(x) && !x.IsInterface).ToList();
        foreach (var curType in allTypes)
        {
            services.AddScoped(typeof(T), curType);
        }
    }
}
