using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RetroLibrary.Controls;
using RetroLibrary.Core;
using RetroLibrary.Core.Common;
using RetroLibrary.Core.Components;
using RetroLibrary.Core.Drawing;

namespace RetroLibrary.XmlLoader.Components;

public class RetroSpriteProgressBarComponentLoader(
    IVariableReplacer variableReplacer,
    IColorLoader colorLoader) : ComponentLoaderBase, IComponentLoader
{
    public bool IsApplicable(XElement element)
    {
        return element.Name == "RetroSpriteProgressBar" ||
               element.Attribute("type")?.Value == "RetroLibrary.Controls.RetroSpriteProgressBar, RetroLibrary.Controls";
    }

    public (string Id, object Value) LoadComponent(
        RetroGameContext gameContext,
        XElement element)
    {
        var name = element.Attribute("name")!.Value;

        var progressBar = new RetroSpriteProgressBar(
            name,
            value: ToFloat(element.Attribute("value"), 0f),
            position: ToPoint(element.Attribute("position"), gameContext, variableReplacer, Point.Zero),
            size: ToPoint(element.Attribute("size"), gameContext, variableReplacer, Point.Zero),
            backgroundColor: ToColor(element.Attribute("backgroundColor"), element.Attribute("backgroundColorAlpha"), colorLoader, null),
            foregroundColor: ToColor(element.Attribute("foregroundColor"), element.Attribute("foregroundColorAlpha"), colorLoader, null),
            borderTexture: GetResource<NineSliceTexture2D>(element.Attribute("borderTextureRef"), gameContext.ResourceManager),
            borderTint: ToColor(element.Attribute("borderTint"), element.Attribute("borderTintAlpha"), colorLoader, null),
            fromColor: ToColor(element.Attribute("fromColor"), element.Attribute("fromColorAlpha"), colorLoader, null),
            toColor: ToColor(element.Attribute("toColor"), element.Attribute("toColorAlpha"), colorLoader, null),
            font: GetResource<SpriteFont>(element.Attribute("fontRef"), gameContext.ResourceManager),
            isVisible: ToBool(element.Attribute("isVisible"), true),
            buffered: ToBool(element.Attribute("buffered"), false));

        return (name, progressBar);
    }
}