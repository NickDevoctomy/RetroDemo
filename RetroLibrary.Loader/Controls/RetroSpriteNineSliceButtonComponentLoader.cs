using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using RetroLibrary.Controls;
using RetroLibrary.Core;
using RetroLibrary.Core.Common;
using RetroLibrary.Core.Components;
using RetroLibrary.Core.Drawing;
using RetroLibrary.Core.Resources;

namespace RetroLibrary.XmlLoader.Controls;

public class RetroSpriteNineSliceButtonComponentLoader(
    IResourceManager resourceManager,
    IColorLoader colorLoader,
    IVariableReplacer variableReplacer)
    : ComponentLoaderBase(resourceManager, colorLoader, variableReplacer), IComponentLoader
{
    public bool IsApplicable(XElement element)
    {
        return
            element.Name == "RetroSpriteNineSliceButton" ||
            element.Attribute("type")!.Value == "RetroLibrary.Controls.RetroSpriteNineSliceButton, RetroLibrary.Controls";
    }

    public (string Id, object Value) LoadComponent(
        RetroGameContext gameContext,
        XElement element)
    {
        var name = element.Attribute("name")!.Value;

        var button = new RetroSpriteNineSliceButton(
            name,
            element.Attribute("text")!.Value,
            ToPoint(element.Attribute("position"), gameContext, Point.Zero),
            ToPoint(element.Attribute("size"), gameContext, Point.Zero),
            ToBool(element.Attribute("isToggle"), false),
            font: GetResource<SpriteFont>(element.Attribute("fontRef")),
            backgroundColor: ToColor(element.Attribute("backgroundColor"), null, null),
            foregroundColor: ToColor(element.Attribute("foregroundColor"), null, null),
            upTint: ToColor(element.Attribute("upTint"), null, null),
            downTint: ToColor(element.Attribute("downTint"), null, null),
            upTexture: GetResource<NineSliceTexture2D>(element.Attribute("upTextureRef")),
            downTexture: GetResource<NineSliceTexture2D>(element.Attribute("downTextureRef")),
            isVisible: ToBool(element.Attribute("isVisible"), true),
            clickCommand: GetRelayCommand(element.Attribute("clickCommand"), gameContext),
            clickSound: GetResource<SoundEffect>(element.Attribute("clickSoundRef")));

        return (name, (object)button);
    }
}