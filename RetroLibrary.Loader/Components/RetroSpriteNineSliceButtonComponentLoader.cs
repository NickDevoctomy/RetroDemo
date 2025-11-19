using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RetroLibrary.Controls;
using RetroLibrary.Core;
using RetroLibrary.Core.Common;
using RetroLibrary.Core.Components;
using RetroLibrary.Core.Drawing;

namespace RetroLibrary.XmlLoader.Components;

public class RetroSpriteNineSliceButtonComponentLoader(
    IVariableReplacer variableReplacer,
    IColorLoader colorLoader) : ComponentLoaderBase, IComponentLoader
{
    public bool IsApplicable(XElement element)
    {
        return
            element.Name == "RetroSpriteNineSliceButton" ||
            element.Attribute("type")!.Value == "RetroLibrary.Controls.RetroSpriteNineSliceButton, RetroLibrary";
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
            font: GetResource<SpriteFont>(element.Attribute("fontRef"), gameContext.ResourceManager),
            backgroundColor: ToColor(element.Attribute("backgroundColor"), null, colorLoader, null),
            foregroundColor: ToColor(element.Attribute("foregroundColor"), null, colorLoader, null),
            upTint: ToColor(element.Attribute("upTint"), null, colorLoader, null),
            downTint: ToColor(element.Attribute("downTint"), null, colorLoader, null),
            upTexture: GetResource<NineSliceTexture2D>(element.Attribute("upTextureRef"), gameContext.ResourceManager),
            downTexture: GetResource<NineSliceTexture2D>(element.Attribute("downTextureRef"), gameContext.ResourceManager),
            buffered: ToBool(element.Attribute("buffered"), false));

        return (name, (object)button);
    }
}
