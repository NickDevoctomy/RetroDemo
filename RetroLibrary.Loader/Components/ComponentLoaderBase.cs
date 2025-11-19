using System.Xml.Linq;
using Microsoft.Xna.Framework;
using RetroLibrary.Core;
using RetroLibrary.Core.Common;
using RetroLibrary.Core.Resources;
using RetroLibrary.XmlLoader.Extensions;

namespace RetroLibrary.XmlLoader.Components;

public class ComponentLoaderBase
{
    protected static T? GetResource<T>(
        XAttribute? attribute,
        IResourceManager resourceManager)
    {
        if (attribute == null)
        {
            return default;
        }

        return resourceManager.GetResource<T>(attribute.Value);
    }

    protected static Color? ToColor(
        XAttribute? attribute,
        XAttribute? alpha,
        IColorLoader colorLoader,
        Color? defaultColor)
    {
        if (attribute == null)
        {
            return defaultColor;
        }

        var alphaValue = alpha != null ? float.Parse(alpha.Value) : 1.0f;
        return attribute.ToColor(colorLoader, defaultColor, alphaValue);
    }

    protected static Point ToPoint(
        XAttribute? attribute,
        RetroGameContext gameContext,
        IVariableReplacer variableReplacer,
        Point defaultValue)
    {
        if (attribute == null)
        {
            return defaultValue;
        }

        return attribute.ToPoint(gameContext, variableReplacer);
    }

    protected static Rectangle ToRectangle(
        XAttribute? attribute,
        RetroGameContext gameContext,
        IVariableReplacer variableReplacer,
        Rectangle defaultValue)
    {
        if (attribute == null)
        {
            return defaultValue;
        }

        return attribute.ToRectangle(gameContext, variableReplacer);
    }

    protected static float ToFloat(
        XAttribute? attribute,
        float defaultValue)
    {
        if (attribute == null)
        {
            return defaultValue;
        }

        return float.Parse(attribute.Value);
    }

    protected static bool ToBool(
        XAttribute? attribute,
        bool defaultValue)
    {
        if (attribute == null)
        {
            return defaultValue;
        }

        return bool.Parse(attribute.Value);
    }
}
