using Microsoft.Extensions.DependencyInjection;
using RetroLibrary.Core.Common;
using RetroLibrary.Core.Resources;

namespace RetroLibrary.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddRetroLibraryCoreServices(this IServiceCollection services)
    {
        services.AddSingleton<IResourceManager, ResourceManager>();
        services.AddSingleton<IRetroGameContextFactory, RetroGameContextFactory>();
        services.AddScoped<ITexture2DResourceLoader, Texture2DResourceLoader>();
        services.AddScoped<IColorLoader, ColorLoader>();
        services.AddScoped<IVariableReplacer, VariableReplacer>();
    }
}
