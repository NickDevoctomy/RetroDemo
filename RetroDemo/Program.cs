using Microsoft.Extensions.DependencyInjection;
using RetroDemo;
using RetroLibrary.Core;
using RetroLibrary.Core.Extensions;
using RetroLibrary.Core.Interfaces;
using RetroLibrary.XmlLoader.Extensions;

var services = new ServiceCollection();
services.AddRetroLibraryCoreServices();
services.AddRetroLibraryXmlLoader();
using var serviceProvider = services.BuildServiceProvider();

var contextFactory = serviceProvider.GetRequiredService<IRetroGameContextFactory>();
var retroGameContext = contextFactory.CreateRetroGameContext(
    800,
    600,
    false,
    string.Empty);
using var game = new Game(retroGameContext);
game.TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 59.0);
game.Run();
