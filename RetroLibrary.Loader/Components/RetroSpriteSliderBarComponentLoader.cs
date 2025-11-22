using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RetroLibrary.Controls;
using RetroLibrary.Core;
using RetroLibrary.Core.Binding;
using RetroLibrary.Core.Common;
using RetroLibrary.Core.Components;
using RetroLibrary.Core.Drawing;

namespace RetroLibrary.XmlLoader.Components;

public class RetroSpriteSliderBarComponentLoader(
    IVariableReplacer variableReplacer,
    IColorLoader colorLoader) : ComponentLoaderBase, IComponentLoader
{
    public bool IsApplicable(XElement element)
    {
        return element.Name == "RetroSpriteSliderBar" ||
               element.Attribute("type")!.Value == "RetroLibrary.Controls.RetroSpriteSliderBar, RetroLibrary.Controls";
    }

    public (string Id, object Value) LoadComponent(
        RetroGameContext gameContext,
        XElement element)
    {
        var name = element.Attribute("name")!.Value;

        var slider = new RetroSpriteSliderBar(
            name,
            position: ToPoint(element.Attribute("position"), gameContext, variableReplacer, Point.Zero),
            size: ToPoint(element.Attribute("size"), gameContext, variableReplacer, Point.Zero),
            backgroundColor: ToColor(element.Attribute("backgroundColor"), element.Attribute("backgroundColorAlpha"), colorLoader, null),
            foregroundColor: ToColor(element.Attribute("foregroundColor"), element.Attribute("foregroundColorAlpha"), colorLoader, null),
            sliderBarTexture: GetResource<NineSliceTexture2D>(element.Attribute("sliderBarTextureRef"), gameContext.ResourceManager),
            sliderBarTint: ToColor(element.Attribute("sliderBarTint"), element.Attribute("sliderBarTintAlpha"), colorLoader, Color.White),
            buttonTexture: GetResource<NineSliceTexture2D>(element.Attribute("buttonTextureRef"), gameContext.ResourceManager),
            buttonTint: ToColor(element.Attribute("buttonTint"), element.Attribute("buttonAlpha"), colorLoader, Color.White),
            font: GetResource<SpriteFont>(element.Attribute("fontRef"), gameContext.ResourceManager),
            isVisible: ToBool(element.Attribute("isVisible"), true));

        return (name, slider);
    }
}