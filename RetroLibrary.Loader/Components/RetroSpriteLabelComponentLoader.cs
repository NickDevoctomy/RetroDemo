using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RetroLibrary.Controls;
using RetroLibrary.Core;
using RetroLibrary.Core.Binding;
using RetroLibrary.Core.Common;
using RetroLibrary.Core.Components;
using RetroLibrary.Core.Enums;
using RetroLibrary.Core.Resources;

namespace RetroLibrary.XmlLoader.Components;

public class RetroSpriteLabelComponentLoader(
    IResourceManager resourceManager,
    IVariableReplacer variableReplacer,
    IColorLoader colorLoader,
    IBindingParser bindingParser)
    : ComponentLoaderBase(resourceManager, colorLoader, variableReplacer), IComponentLoader
{
    public bool IsApplicable(XElement element)
    {
        return element.Name == "RetroSpriteLabel" ||
               element.Attribute("type")!.Value == "RetroLibrary.Controls.RetroSpriteLabel, RetroLibrary.Controls";
    }

    public (string Id, object Value) LoadComponent(
        RetroGameContext gameContext,
        XElement element)
    {
        var name = element.Attribute("name")!.Value;

        var label = new RetroSpriteLabel(
            name,
            ToPoint(element.Attribute("position"), gameContext, Point.Zero),
            ToPoint(element.Attribute("size"), gameContext, Point.Zero),
            text: ToBindingValue(element.Attribute("text"), bindingParser, new BindingValue<string>(value: string.Empty)),
            font: GetResource<SpriteFont>(element.Attribute("fontRef")),
            backgroundColor: ToColor(element.Attribute("backgroundColor"), null, null),
            foregroundColor: ToColor(element.Attribute("foregroundColor"), null, null),
            horizontalAlignment: ToEnum(element.Attribute("horizontalAlignment"), HorizontalAlignment.Left),
            verticalAlignment: ToEnum(element.Attribute("verticalAlignment"), VerticalAlignment.Middle));

        ApplyBindings(label, gameContext, bindingParser);

        return (name, label);
    }
}