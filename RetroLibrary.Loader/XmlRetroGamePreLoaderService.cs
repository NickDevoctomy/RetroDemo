using System.CommandLine;
using System.Xml.Linq;
using RetroLibrary.Core;
using RetroLibrary.Core.Configuration;
using RetroLibrary.Core.Interfaces;

namespace RetroLibrary.XmlLoader;

public class XmlRetroGamePreLoaderService : IRetroGamePreLoaderService
{
    public RetroGameConfiguration PreLoad(
        string[] args,
        RetroGameContext retroGameContext)
    {
        using var stream = File.OpenRead(retroGameContext.GameDefinitionFilePath);
        var document = XDocument.Load(
            stream,
            LoadOptions.None);

        if (document == null ||
            document.Root == null)
        {
            return new RetroGameConfiguration();
        }

        var width = document.Root.Attribute("width") != null ? int.Parse(document.Root.Attribute("width")!.Value) : 800;
        var height = document.Root.Attribute("height") != null ? int.Parse(document.Root.Attribute("height")!.Value) : 600;
        var isFullScreen = document.Root.Attribute("isFullScreen") != null && bool.Parse(document.Root.Attribute("isFullScreen")!.Value);

        var widthArg = new Option<int>("--width")
        {
            Description = "Force the game to use this width for rendering.",
            DefaultValueFactory = _ => width
        };
        var heightArg = new Option<int>("--height")
        {
            Description = "Force the game to use this height for rendering.",
            DefaultValueFactory = _ => height
        };
        var fullscreenArg = new Option<bool>("--fullscreen")
        {
            Description = "Force the game to use this fullscreen option for rendering.",
            DefaultValueFactory = _ => isFullScreen
        };

        var rootCommand = new RootCommand("RetroGame Application.");
        rootCommand.Options.Add(widthArg);
        rootCommand.Options.Add(heightArg);
        var parseResults = rootCommand.Parse(args);

        return new RetroGameConfiguration
        {
            Width = parseResults.GetValue(widthArg),
            Height = parseResults.GetValue(heightArg),
            IsFullScreen = parseResults.GetValue(fullscreenArg)
        };
    }
}