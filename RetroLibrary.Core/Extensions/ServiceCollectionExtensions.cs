using Microsoft.Extensions.DependencyInjection;
using RetroLibrary.Core.Common;
using RetroLibrary.Core.Drawing;
using RetroLibrary.Core.Resources;

namespace RetroLibrary.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddRetroLibraryCoreServices(this IServiceCollection services)
    {
        services.AddSingleton<IResourceManager, ResourceManager>();
        services.AddSingleton<IRetroGameContextFactory, RetroGameContextFactory>();
        services.AddSingleton<ITexture2DResourceLoader, Texture2DResourceLoader>();
        services.AddScoped<IBlitterService, BlitterService>();
        services.AddSingleton<IColorLoader, ColorLoader>();
        services.AddSingleton<IVariableReplacer, VariableReplacer>();
    }
}
