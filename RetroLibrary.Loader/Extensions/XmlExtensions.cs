using System.Xml.Linq;
using Microsoft.Xna.Framework;
using RetroLibrary.Core;
using RetroLibrary.Core.Common;

namespace RetroLibrary.XmlLoader.Extensions;

public static class XmlExtensions
{
    public static Point ToPoint(
        this XAttribute attribute,
        RetroGameContext gameContext,
        IVariableReplacer variableReplacer)
    {
        var rawValue = attribute.Value;
        var parts = rawValue.Split(',');
        var width = variableReplacer.ReplaceAllVariables(gameContext, parts[0]);
        var height = variableReplacer.ReplaceAllVariables(gameContext, parts[1]);

        return new Point(
            int.Parse(width),
            int.Parse(height));
    }

    public static Color? ToColor(
        this XAttribute attribute,
        IColorLoader colorLoader,
        Color? defaultColor)
    {
        return colorLoader.ColorFromName(
            attribute.Value,
            defaultColor);
    }
}
