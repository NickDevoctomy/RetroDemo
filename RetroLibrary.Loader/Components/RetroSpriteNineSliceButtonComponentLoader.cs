using System.Xml.Linq;
using Microsoft.Xna.Framework;
using RetroLibrary.Controls;
using RetroLibrary.Core;
using RetroLibrary.Core.Common;
using RetroLibrary.Core.Drawing;
using RetroLibrary.Core.Resources;
using RetroLibrary.XmlLoader.Extensions;

namespace RetroLibrary.XmlLoader.Components;

public class RetroSpriteNineSliceButtonComponentLoader(
    IVariableReplacer variableReplacer,
    IColorLoader colorLoader) : IComponentLoader
{
    public bool IsApplicable(XElement element)
    {
        return
            element.Name == "RetroSpriteNineSliceButton" ||
            element.Attribute("type")!.Value == "RetroLibrary.RetroSpriteNineSliceButton, RetroLibrary";
    }

    public (string Id, object Value) LoadComponent(
        RetroGameContext gameContext,
        XElement element)
    {
        var name = element.Attribute("name")!.Value;

        var button = new RetroSpriteNineSliceButton(
            name,
            element.Attribute("text")!.Value,
            ToPoint(element.Attribute("position"), gameContext, variableReplacer, Point.Zero),
            ToPoint(element.Attribute("size"), gameContext, variableReplacer, Point.Zero),
            ToBool(element.Attribute("isToggle"), false),
            backgroundColor: ToColor(element.Attribute("backgroundColor"), colorLoader, null),
            foregroundColor: ToColor(element.Attribute("foregroundColor"), colorLoader, null),
            upTint: ToColor(element.Attribute("upTint"), colorLoader, null),
            downTint: ToColor(element.Attribute("downTint"), colorLoader, null),
            upTexture: GetResource<NineSliceTexture2D>(element.Attribute("upTextureRef"), gameContext.ResourceManager),
            downTexture: GetResource<NineSliceTexture2D>(element.Attribute("downTextureRef"), gameContext.ResourceManager));

        return (name, (object)button);
    }

    private static T? GetResource<T>(
        XAttribute? attribute,
        IResourceManager resourceManager)
    {
        if (attribute == null)
        {
            return default;
        }

        return resourceManager.GetResource<T>(attribute.Value);
    }

    private static Color? ToColor(
        XAttribute? attribute,
        IColorLoader colorLoader,
        Color? defaultColor)
    {
        if (attribute == null)
        {
            return defaultColor;
        }

        return attribute.ToColor(colorLoader, defaultColor);
    }

    private static Point ToPoint(
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

    private static bool ToBool(
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
