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

public class RetroSpriteCheckBoxComponentLoader(
    IVariableReplacer variableReplacer,
    IColorLoader colorLoader) : ComponentLoaderBase, IComponentLoader
{
    public bool IsApplicable(XElement element)
    {
        return element.Name == "RetroSpriteCheckBox" ||
               element.Attribute("type")!.Value == "RetroLibrary.Controls.RetroSpriteCheckBox, RetroLibrary.Controls";
    }

    public (string Id, object Value) LoadComponent(
        RetroGameContext gameContext,
        XElement element)
    {
        var name = element.Attribute("name")!.Value;

        var checkBox = new RetroSpriteCheckBox(
            name,
            element.Attribute("text")!.Value,
            ToPoint(element.Attribute("position"), gameContext, variableReplacer, Point.Zero),
            ToPoint(element.Attribute("size"), gameContext, variableReplacer, Point.Zero),
            font: GetResource<SpriteFont>(element.Attribute("fontRef"), gameContext.ResourceManager),
            backgroundColor: ToColor(element.Attribute("backgroundColor"), null, colorLoader, null),
            foregroundColor: ToColor(element.Attribute("foregroundColor"), null, colorLoader, null),
            boxTexture: GetResource<NineSliceTexture2D>(element.Attribute("boxTextureRef"), gameContext.ResourceManager),
            isChecked: ToBool(element.Attribute("isChecked"), false),
            isVisible: ToBool(element.Attribute("isVisible"), true));

        return (name, checkBox);
    }
}