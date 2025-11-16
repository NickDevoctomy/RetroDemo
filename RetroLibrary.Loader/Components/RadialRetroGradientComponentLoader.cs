using System.Xml.Linq;
using RetroLibrary.Loader.Common;
using SixLabors.ImageSharp;

namespace RetroLibrary.Loader.Components;

public class RadialRetroGradientComponentLoader(IColorLoader colorLoader) : IComponentLoader
{
    public bool IsApplicable(XElement element)
    {
        return element.Name == "RadialRetroGradient";
    }

    public Task<(string Id, object Value)> LoadComponentAsync(
        RetroGameContext gameContext,
        XElement element,
        CancellationToken cancellationToken)
    {
        var id = element.Attribute("id")!.Value;
        var radialRetroGradient = new RadialRetroGradientTexture2D(
            new RadialRetroGradientOptions
            {
                CentrePoint = new PointF(
                    float.Parse(element.Element("CentrePoint")!.Attribute("x")!.Value),
                    float.Parse(element.Element("CentrePoint")!.Attribute("y")!.Value)),
                Radius = float.Parse(element.Element("Radius")!.Value),
                FromColor = colorLoader.ColorFromName(element.Element("FromColor")!.Value, Microsoft.Xna.Framework.Color.White),
                ToColor = colorLoader.ColorFromName(element.Element("ToColor")!.Value, Microsoft.Xna.Framework.Color.White),
                GradientStops = int.Parse(element.Element("GradientStops")!.Value)
            });

        return radialRetroGradient == null
            ? throw new InvalidOperationException("Failed to create RadialRetroGradientTexture2D.")
            : (Task<(string Id, object Value)>)Task.FromResult((id, (object)radialRetroGradient));
    }
}
