using Microsoft.Extensions.DependencyInjection;
using RetroDemo;
using RetroLibrary.Core;
using RetroLibrary.Core.Extensions;
using RetroLibrary.XmlLoader.Extensions;

var services = new ServiceCollection();
services.AddRetroLibraryCoreServices();
services.AddRetroLibraryXmlLoader();
using var serviceProvider = services.BuildServiceProvider();

var contentLocation = File.Exists("Content/scene.xml")
    ? "Content"
    : "bin/Debug/net8.0/Content";

var contextFactory = serviceProvider.GetRequiredService<IRetroGameContextFactory>();
var retroGameContext = contextFactory.CreateRetroGameContext(
    1024,
    576,
    true,
    Path.Combine(contentLocation, "scene.xml"));
using var game = new Game(retroGameContext);
game.SetTargetFps(59);
game.Run();