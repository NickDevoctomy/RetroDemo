using System.Xml.Linq;
using RetroLibrary.Core;
using RetroLibrary.Core.Common;
using RetroLibrary.Core.Resources;
using RetroLibrary.XmlLoader.Extensions;

namespace RetroLibrary.XmlLoader.Resources;

public class ResourceLoaderBase
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

    protected static Microsoft.Xna.Framework.Color? ToColor(
        XAttribute? attribute,
        IColorLoader colorLoader,
        Microsoft.Xna.Framework.Color? defaultColor)
    {
        if (attribute == null)
        {
            return defaultColor;
        }

        return attribute.ToColor(colorLoader, defaultColor);
    }

    protected static Microsoft.Xna.Framework.Point ToPoint(
        XAttribute? attribute,
        RetroGameContext gameContext,
        IVariableReplacer variableReplacer,
        Microsoft.Xna.Framework.Point defaultValue)
    {
        if (attribute == null)
        {
            return defaultValue;
        }

        return attribute.ToPoint(gameContext, variableReplacer);
    }

    protected static SixLabors.ImageSharp.PointF ToPointF(
        XAttribute? attribute,
        RetroGameContext gameContext,
        IVariableReplacer variableReplacer,
        SixLabors.ImageSharp.PointF defaultValue)
    {
        if (attribute == null)
        {
            return defaultValue;
        }

        return attribute.ToPointF(gameContext, variableReplacer);
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

    protected static float ToFloat(
        XAttribute? attribute,
        RetroGameContext gameContext,
        IVariableReplacer variableReplacer,
        float defaultValue)
    {
        if (attribute == null)
        {
            return defaultValue;
        }

        return attribute.ToFloat(gameContext, variableReplacer);
    }
}
