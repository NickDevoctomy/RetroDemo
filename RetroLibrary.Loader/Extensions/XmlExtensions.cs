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
        rawValue = variableReplacer.ReplaceAllVariables(gameContext, rawValue);
        var parts = rawValue.Split(',');

        return new Point(
            int.Parse(parts[0]),
            int.Parse(parts[0]));
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
