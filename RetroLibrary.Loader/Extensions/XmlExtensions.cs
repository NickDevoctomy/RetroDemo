using System.Xml.Linq;
using RetroLibrary.Core;
using RetroLibrary.Core.Common;

namespace RetroLibrary.XmlLoader.Extensions;

public static class XmlExtensions
{
    public static float ToFloat(
        this XAttribute attribute,
        RetroGameContext gameContext,
        IVariableReplacer variableReplacer)
    {
        var rawValue = attribute.Value;
        var value = variableReplacer.ReplaceAllVariables(gameContext, rawValue);

        return float.Parse(value);
    }

    public static Microsoft.Xna.Framework.Point ToPoint(
        this XAttribute attribute,
        RetroGameContext gameContext,
        IVariableReplacer variableReplacer)
    {
        var rawValue = attribute.Value;
        var parts = rawValue.Split(',');
        var width = variableReplacer.ReplaceAllVariables(gameContext, parts[0]);
        var height = variableReplacer.ReplaceAllVariables(gameContext, parts[1]);

        return new Microsoft.Xna.Framework.Point(
            int.Parse(width),
            int.Parse(height));
    }

    public static SixLabors.ImageSharp.PointF ToPointF(
        this XAttribute attribute,
        RetroGameContext gameContext,
        IVariableReplacer variableReplacer)
    {
        var rawValue = attribute.Value;
        var parts = rawValue.Split(',');
        var width = variableReplacer.ReplaceAllVariables(gameContext, parts[0]);
        var height = variableReplacer.ReplaceAllVariables(gameContext, parts[1]);

        return new SixLabors.ImageSharp.PointF(
            float.Parse(width),
            float.Parse(height));
    }

    public static Microsoft.Xna.Framework.Color? ToColor(
        this XAttribute attribute,
        IColorLoader colorLoader,
        Microsoft.Xna.Framework.Color? defaultColor)
    {
        return colorLoader.ColorFromName(
            attribute.Value,
            defaultColor);
    }
}
