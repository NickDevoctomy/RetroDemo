using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RetroLibrary.Controls;
using RetroLibrary.Core;
using RetroLibrary.Core.Binding;
using RetroLibrary.Core.Common;
using RetroLibrary.Core.Components;
using RetroLibrary.Core.Drawing;
using RetroLibrary.Core.Resources;
using RetroLibrary.XmlLoader.SubLoaders.Interfaces;

namespace RetroLibrary.XmlLoader.Controls;

public class RetroSpriteSliderBarComponentLoader(
    IResourceManager resourceManager,
    IColorLoader colorLoader,
    IVariableReplacer variableReplacer,
    IBindingParser bindingParser,
    IEnumerable<ISubLoader> subLoaders)
    : ComponentLoaderBase(resourceManager, colorLoader, variableReplacer, subLoaders), IComponentLoader
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
            position: ToPoint(element.Attribute("position"), gameContext, Point.Zero),
            size: ToPoint(element.Attribute("size"), gameContext, Point.Zero),
            backgroundColor: ToColor(element.Attribute("backgroundColor"), element.Attribute("backgroundColorAlpha"), null),
            foregroundColor: ToColor(element.Attribute("foregroundColor"), element.Attribute("foregroundColorAlpha"), null),
            value: ToBindingValue(element.Attribute("value"), bindingParser, new BindingValue<float>(50f)),
            sliderBarTexture: GetResource<NineSliceTexture2D>(element.Attribute("sliderBarTextureRef")),
            sliderBarTint: ToColor(element.Attribute("sliderBarTint"), element.Attribute("sliderBarTintAlpha"), Color.White),
            buttonTexture: GetResource<NineSliceTexture2D>(element.Attribute("buttonTextureRef")),
            buttonTint: ToColor(element.Attribute("buttonTint"), element.Attribute("buttonAlpha"), Color.White),
            font: GetResource<SpriteFont>(element.Attribute("fontRef")),
            isVisible: ToBool(element.Attribute("isVisible"), true));

        ApplyBindings(slider, gameContext, bindingParser);

        return (name, slider);
    }
}