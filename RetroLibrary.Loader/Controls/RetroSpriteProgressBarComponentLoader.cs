using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RetroLibrary.Controls;
using RetroLibrary.Core;
using RetroLibrary.Core.Common;
using RetroLibrary.Core.Components;
using RetroLibrary.Core.Drawing;
using RetroLibrary.Core.Resources;

namespace RetroLibrary.XmlLoader.Controls;

public class RetroSpriteProgressBarComponentLoader(
    IResourceManager resourceManager,
    IColorLoader colorLoader,
    IVariableReplacer variableReplacer)
    : ComponentLoaderBase(resourceManager, colorLoader, variableReplacer), IComponentLoader
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
            position: ToPoint(element.Attribute("position"), gameContext, Point.Zero),
            size: ToPoint(element.Attribute("size"), gameContext, Point.Zero),
            backgroundColor: ToColor(element.Attribute("backgroundColor"), element.Attribute("backgroundColorAlpha"), null),
            foregroundColor: ToColor(element.Attribute("foregroundColor"), element.Attribute("foregroundColorAlpha"), null),
            borderTexture: GetResource<NineSliceTexture2D>(element.Attribute("borderTextureRef")),
            borderTint: ToColor(element.Attribute("borderTint"), element.Attribute("borderTintAlpha"), null),
            fromColor: ToColor(element.Attribute("fromColor"), element.Attribute("fromColorAlpha"), null),
            toColor: ToColor(element.Attribute("toColor"), element.Attribute("toColorAlpha"), null),
            font: GetResource<SpriteFont>(element.Attribute("fontRef")),
            isVisible: ToBool(element.Attribute("isVisible"), true));

        return (name, progressBar);
    }
}