using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RetroLibrary.Controls;
using RetroLibrary.Core;
using RetroLibrary.Core.Base;
using RetroLibrary.Core.Common;
using RetroLibrary.Core.Components;
using RetroLibrary.Core.Drawing;

namespace RetroLibrary.XmlLoader.Components;

public class RetroSpriteTabbedContainerComponentLoader(
    IVariableReplacer variableReplacer,
    IColorLoader colorLoader) : ComponentLoaderBase, IComponentLoader
{
    public bool IsApplicable(XElement element)
    {
        return element.Name == "RetroSpriteTabbedContainer" ||
               element.Attribute("type")!.Value == "RetroLibrary.Controls.RetroSpriteTabbedContainer, RetroLibrary";
    }

    public (string Id, object Value) LoadComponent(
        RetroGameContext gameContext,
        XElement element)
    {
        var name = element.Attribute("name")!.Value;

        var tabbedContainer = new RetroSpriteTabbedContainer(
            name,
            ToPoint(element.Attribute("position"), gameContext, variableReplacer, Point.Zero),
            ToPoint(element.Attribute("size"), gameContext, variableReplacer, Point.Zero),
            ToColor(element.Attribute("foregroundColor"), null, colorLoader, Color.Black),
            innerMargins: ToRectangle(element.Attribute("innerMargins"), gameContext, variableReplacer, Rectangle.Empty),
            tabUpTint: ToColor(element.Attribute("tabUpTint"), element.Attribute("tabUpTintAlpha"), colorLoader, Color.White),
            tabDownTint: ToColor(element.Attribute("tabDownTint"), element.Attribute("tabDownTintAlpha"), colorLoader, Color.White),
            tabPageTint: ToColor(element.Attribute("tabPageTint"), element.Attribute("tabPageTintAlpha"), colorLoader, Color.White),
            tabUpTexture: GetResource<NineSliceTexture2D>(element.Attribute("tabUpTextureRef"), gameContext.ResourceManager),
            tabDownTexture: GetResource<NineSliceTexture2D>(element.Attribute("tabDownTextureRef"), gameContext.ResourceManager),
            tabPageTexture: GetResource<NineSliceTexture2D>(element.Attribute("tabPageTextureRef"), gameContext.ResourceManager),
            font: GetResource<SpriteFont>(element.Attribute("fontRef"), gameContext.ResourceManager));

        var tabPagesRoot = element.Element("TabPages");
        if (tabPagesRoot != null)
        {
            var tabPages = tabPagesRoot.Elements("TabPage");
            foreach (var curTabPage in tabPages)
            {
                var children = new System.Collections.ObjectModel.ObservableCollection<Core.Base.RetroSpriteBase>();
                var childSprites = curTabPage.Elements();
                foreach (var curChildSprite in childSprites)
                {
                    var componentLoader = gameContext.ComponentLoaders.Single(loader => loader.IsApplicable(curChildSprite));
                    (string id, object value) = componentLoader.LoadComponent(
                        gameContext,
                        curChildSprite);

                    if (value is RetroSpriteBase sprite)
                    {
                        children.Add(sprite);
                    }
                }

                var tabPage = new TabPage(
                    curTabPage.Attribute("title")?.Value ?? "Tab Page",
                    children);

                tabbedContainer.TabPages.Add(tabPage);
            }
        }

        return (name, tabbedContainer);
    }
}
