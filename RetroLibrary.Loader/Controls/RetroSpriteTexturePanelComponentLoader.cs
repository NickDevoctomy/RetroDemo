using System.Xml.Linq;
using Microsoft.Xna.Framework;
using RetroLibrary.Controls;
using RetroLibrary.Core;
using RetroLibrary.Core.Common;
using RetroLibrary.Core.Components;
using RetroLibrary.Core.Drawing;
using RetroLibrary.Core.Resources;
using RetroLibrary.XmlLoader.SubLoaders.Interfaces;

namespace RetroLibrary.XmlLoader.Controls
{
    internal class RetroSpriteTexturePanelComponentLoader(
        IResourceManager resourceManager,
        IColorLoader colorLoader,
        IVariableReplacer variableReplacer,
        IEnumerable<ISubLoader> subLoaders)
        : ComponentLoaderBase(resourceManager, colorLoader, variableReplacer, subLoaders), IComponentLoader
    {
        public bool IsApplicable(XElement element)
        {
            return element.Name == "RetroSpriteTexturePanel" ||
                   element.Attribute("type")!.Value == "RetroLibrary.Controls.RetroSpriteTexturePanel, RetroLibrary.Controls";
        }

        public (string Id, object Value) LoadComponent(
            RetroGameContext gameContext,
            XElement element)
        {
            var name = element.Attribute("name")!.Value;

            var panel = new RetroSpriteTexturePanel(
                name,
                ToPoint(element.Attribute("position"), gameContext, Point.Zero),
                ToPoint(element.Attribute("size"), gameContext, Point.Zero),
                texture: GetResource<IRetroTexture2D>(element.Attribute("textureRef")),
                isVisible: ToBool(element.Attribute("isVisible"), true),
                margins: ToRectangle(element.Attribute("margins"), gameContext, Rectangle.Empty),
                padding: ToRectangle(element.Attribute("padding"), gameContext, Rectangle.Empty));

            return (name, panel);
        }
    }
}