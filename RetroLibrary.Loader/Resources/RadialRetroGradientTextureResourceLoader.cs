using System.Xml.Linq;
using RetroLibrary.Core;
using RetroLibrary.Core.Common;
using RetroLibrary.Core.Drawing;
using RetroLibrary.Core.Resources;
using SixLabors.ImageSharp;

namespace RetroLibrary.XmlLoader.Resources;

public class RadialRetroGradientTextureResourceLoader(
    IColorLoader colorLoader,
    IVariableReplacer variableReplacer)
    : ResourceLoaderBase, IResourceLoader
{
    public bool IsApplicable(XElement element)
    {
        return element.Name == "RadialRetroGradient";
    }

    public (string Id, object Value) LoadResource(
        RetroGameContext gameContext,
        XElement element)
    {
        var id = element.Attribute("id")!.Value;

        var fromColor = colorLoader.ColorFromName(element.Attribute("fromColor")!.Value, Microsoft.Xna.Framework.Color.White);
        var toColor = colorLoader.ColorFromName(element.Attribute("toColor")!.Value, Microsoft.Xna.Framework.Color.White);

        var radialRetroGradient = new RadialRetroGradientTexture2D(
            new RadialRetroGradientOptions
            {
                CentrePoint = ToPointF(element.Attribute("centrePoint"), gameContext, variableReplacer, PointF.Empty),
                Radius = ToFloat(element.Attribute("radius"), gameContext, variableReplacer, 0f),
                FromColor = fromColor.GetValueOrDefault(),
                ToColor = toColor.GetValueOrDefault(),
                GradientStops = int.Parse(element.Attribute("gradientStops")!.Value)
            });

        return radialRetroGradient == null
            ? throw new InvalidOperationException("Failed to create RadialRetroGradientTexture2D.")
            : (id, (object)radialRetroGradient);
    }
}
