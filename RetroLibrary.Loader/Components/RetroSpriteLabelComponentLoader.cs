using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RetroLibrary.Controls;
using RetroLibrary.Core;
using RetroLibrary.Core.Binding;
using RetroLibrary.Core.Common;
using RetroLibrary.Core.Components;
using RetroLibrary.Core.Enums;

namespace RetroLibrary.XmlLoader.Components;

public class RetroSpriteLabelComponentLoader(
    IVariableReplacer variableReplacer,
    IColorLoader colorLoader,
    IBindingParser bindingParser) : ComponentLoaderBase, IComponentLoader
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

        var textAttribute = element.Attribute("text")?.Value ?? string.Empty;
        var isTextBound = bindingParser.IsBindingString(textAttribute);

        var label = new RetroSpriteLabel(
            name,
            isTextBound ? string.Empty : textAttribute,
            ToPoint(element.Attribute("position"), gameContext, variableReplacer, Point.Zero),
            ToPoint(element.Attribute("size"), gameContext, variableReplacer, Point.Zero),
            font: GetResource<SpriteFont>(element.Attribute("fontRef"), gameContext.ResourceManager),
            backgroundColor: ToColor(element.Attribute("backgroundColor"), null, colorLoader, null),
            foregroundColor: ToColor(element.Attribute("foregroundColor"), null, colorLoader, null),
            horizontalAlignment: ToEnum(element.Attribute("horizontalAlignment"), HorizontalAlignment.Left),
            verticalAlignment: ToEnum(element.Attribute("verticalAlignment"), VerticalAlignment.Middle));

        // This needs to be nicer, bit more dynamic than this.
        if (isTextBound)
        {
            var bindingInfo = bindingParser.Parse(label, textAttribute);
            bindingInfo.BoundPropertyName ??= nameof(RetroSpriteLabel.Text);
            gameContext.RetroGameLoaderService.Binder!.AddBinding(bindingInfo);
        }

        return (name, label);
    }
}