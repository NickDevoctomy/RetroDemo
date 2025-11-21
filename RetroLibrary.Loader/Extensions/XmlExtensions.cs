using System.Xml.Linq;
using Microsoft.Xna.Framework;
using RetroLibrary.Core;
using RetroLibrary.Core.Common;

namespace RetroLibrary.XmlLoader.Extensions;

public static class XmlExtensions
{
    public static int ToInt(
        this XAttribute attribute,
        RetroGameContext gameContext,
        IVariableReplacer variableReplacer)
    {
        var rawValue = attribute.Value;
        var value = variableReplacer.ReplaceAllVariables(gameContext, rawValue);

        return int.Parse(value);
    }

    public static float ToFloat(
        this XAttribute attribute,
        RetroGameContext gameContext,
        IVariableReplacer variableReplacer)
    {
        var rawValue = attribute.Value;
        var value = variableReplacer.ReplaceAllVariables(gameContext, rawValue);

        return float.Parse(value);
    }

    public static Microsoft.Xna.Framework.Rectangle ToRectangle(
        this XAttribute attribute,
        RetroGameContext gameContext,
        IVariableReplacer variableReplacer)
    {
        var rawValue = attribute.Value;
        var parts = rawValue.Split(',');
        var x = variableReplacer.ReplaceAllVariables(gameContext, parts[0]);
        var y = variableReplacer.ReplaceAllVariables(gameContext, parts[1]);
        var width = variableReplacer.ReplaceAllVariables(gameContext, parts[2]);
        var height = variableReplacer.ReplaceAllVariables(gameContext, parts[3]);

        return new Microsoft.Xna.Framework.Rectangle(
            int.Parse(x),
            int.Parse(y),
            int.Parse(width),
            int.Parse(height));
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
        Microsoft.Xna.Framework.Color? defaultColor,
        float alpha)
    {
        var color = colorLoader.ColorFromName(
            attribute.Value,
            defaultColor);
        return color == null ? null : Color.MultiplyAlpha(color.GetValueOrDefault(), alpha);
    }
}