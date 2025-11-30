using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RetroLibrary.Controls;
using RetroLibrary.Controls.Interfaces;
using RetroLibrary.Core;
using RetroLibrary.Core.Base;
using RetroLibrary.Core.Binding;
using RetroLibrary.Core.Common;
using RetroLibrary.Core.Components;
using RetroLibrary.Core.Drawing;
using RetroLibrary.Core.Resources;
using RetroLibrary.XmlLoader.SubLoaders;
using RetroLibrary.XmlLoader.SubLoaders.Interfaces;

namespace RetroLibrary.XmlLoader.Controls;

public class RetroSpriteTabbedContainerComponentLoader(
    IResourceManager resourceManager,
    IColorLoader colorLoader,
    IVariableReplacer variableReplacer,
    IBindingParser bindingParser,
    IEnumerable<ISubLoader> subLoaders)
    : ComponentLoaderBase(resourceManager, colorLoader, variableReplacer, subLoaders), IComponentLoader
{
    public bool IsApplicable(XElement element)
    {
        return element.Name == "RetroSpriteTabbedContainer" ||
               element.Attribute("type")!.Value == "RetroLibrary.Controls.RetroSpriteTabbedContainer, RetroLibrary.Controls";
    }

    public (string Id, object Value) LoadComponent(
        RetroGameContext gameContext,
        XElement element)
    {
        var name = element.Attribute("name")!.Value;
        var size = ToPoint(element.Attribute("size"), gameContext, Point.Zero);

        VariableReplacer.DefaultParameters.Add("ParentWidth", size.X);
        VariableReplacer.DefaultParameters.Add("ParentHeight", size.Y);

        var childCompositorElement = element.Element("ChildCompositor");
        var compositor = SubLoader<IContainerChildCompositor>(childCompositorElement);

        var tabbedContainer = new RetroSpriteTabbedContainer(
            name,
            ToPoint(element.Attribute("position"), gameContext, Point.Zero),
            size,
            ToColor(element.Attribute("foregroundColor"), null, Color.Black),
            innerMargins: ToRectangle(element.Attribute("innerMargins"), gameContext, Rectangle.Empty),
            tabUpTint: ToColor(element.Attribute("tabUpTint"), element.Attribute("tabUpTintAlpha"), Color.White),
            tabDownTint: ToColor(element.Attribute("tabDownTint"), element.Attribute("tabDownTintAlpha"), Color.White),
            tabPageTint: ToColor(element.Attribute("tabPageTint"), element.Attribute("tabPageTintAlpha"), Color.White),
            tabUpTexture: GetResource<NineSliceTexture2D>(element.Attribute("tabUpTextureRef")),
            tabDownTexture: GetResource<NineSliceTexture2D>(element.Attribute("tabDownTextureRef")),
            tabPageTexture: GetResource<NineSliceTexture2D>(element.Attribute("tabPageTextureRef")),
            font: GetResource<SpriteFont>(element.Attribute("fontRef")),
            childCompositor: compositor,
            isVisible: ToBool(element.Attribute("isVisible"), true));

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
                    var componentLoader = gameContext.RetroGameLoaderService.ComponentLoaders.Single(loader => loader.IsApplicable(curChildSprite));
                    (string id, object value) = componentLoader.LoadComponent(
                        gameContext,
                        curChildSprite);

                    if (value is RetroSpriteBase sprite)
                    {
                        children.Add(sprite);
                    }
                }

                var isBindingString = bindingParser.IsBindingString(curTabPage.Attribute("title")?.Value);

                var tabPage = new TabPage(
                    isBindingString ? new BindingValue<string>(bindingString: curTabPage.Attribute("title")!.Value!) :
                    new BindingValue<string>(value: curTabPage.Attribute("title")?.Value),
                    children);

                tabbedContainer.TabPages.Add(tabPage);

                ApplyBindings(tabPage, gameContext, bindingParser);
            }
        }

        return (name, tabbedContainer);
    }
}