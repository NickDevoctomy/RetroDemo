using System.Xml.Linq;
using RetroLibrary.Core;
using RetroLibrary.Core.Common;
using RetroLibrary.Core.Drawing;
using RetroLibrary.Core.Resources;
using SixLabors.ImageSharp;

namespace RetroLibrary.XmlLoader.Resources;

public class RadialRetroGradientTextureResourceLoader(IColorLoader colorLoader) : IResourceLoader
{
    public bool IsApplicable(XElement element)
    {
        return element.Name == "RadialRetroGradient";
    }

    public Task<(string Id, object Value)> LoadResourceAsync(
        RetroGameContext gameContext,
        XElement element,
        CancellationToken cancellationToken)
    {
        var id = element.Attribute("id")!.Value;
        var fromColor = colorLoader.ColorFromName(element.Element("FromColor")!.Value, Microsoft.Xna.Framework.Color.White);
        var toColor = colorLoader.ColorFromName(element.Element("ToColor")!.Value, Microsoft.Xna.Framework.Color.White);

        var radialRetroGradient = new RadialRetroGradientTexture2D(
            new RadialRetroGradientOptions
            {
                CentrePoint = new PointF(
                    float.Parse(element.Element("CentrePoint")!.Attribute("x")!.Value),
                    float.Parse(element.Element("CentrePoint")!.Attribute("y")!.Value)),
                Radius = float.Parse(element.Element("Radius")!.Value),
                FromColor = fromColor.GetValueOrDefault(),
                ToColor = toColor.GetValueOrDefault(),
                GradientStops = int.Parse(element.Element("GradientStops")!.Value)
            });

        return radialRetroGradient == null
            ? throw new InvalidOperationException("Failed to create RadialRetroGradientTexture2D.")
            : (Task<(string Id, object Value)>)Task.FromResult((id, (object)radialRetroGradient));
    }
}
