using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RetroLibrary.Controls;
using RetroLibrary.Core;
using RetroLibrary.Core.Common;
using RetroLibrary.Core.Components;
using RetroLibrary.Core.Resources;
using RetroLibrary.XmlLoader.SubLoaders.Interfaces;

namespace RetroLibrary.XmlLoader.Controls;

public class RetroSpriteMiniParalaxScrollerComponentLoader(
    IResourceManager resourceManager,
    IColorLoader colorLoader,
    IVariableReplacer variableReplacer,
    IEnumerable<ISubLoader> subLoaders)
    : ComponentLoaderBase(resourceManager, colorLoader, variableReplacer, subLoaders), IComponentLoader
{
    public bool IsApplicable(XElement element)
    {
        return element.Name == "RetroSpriteMiniParallaxScroller" ||
               element.Attribute("type")!.Value == "RetroLibrary.Controls.RetroSpriteMiniParallaxScroller, RetroLibrary.Controls";
    }

    public (string Id, object Value) LoadComponent(
        RetroGameContext gameContext,
        XElement element)
    {
        var name = element.Attribute("name")!.Value;

        var layers = new List<MiniParallaxScrollerMiniParallaxScrollerLayer>();
        var layersRoot = element.Element("Layers");
        if (layersRoot != null)
        {
            foreach (var layerElement in layersRoot.Elements("Layer"))
            {
                var layer = new MiniParallaxScrollerMiniParallaxScrollerLayer(
                    layerElement.Attribute("name")!.Value,
                    layerElement.Attribute("texturePath")!.Value,
                    ToFloat(layerElement.Attribute("scrollSpeed"), 0f),
                    ToInt(layerElement.Attribute("yOffset"), gameContext, 0));
                layers.Add(layer);
            }
        }

        var scroller = new RetroSpriteMiniParallaxScroller(
            name,
            ToPoint(element.Attribute("position"), gameContext, Point.Zero),
            ToPoint(element.Attribute("size"), gameContext, Point.Zero),
            backgroundColor: ToColor(element.Attribute("backgroundColor"), null, null),
            foregroundColor: ToColor(element.Attribute("foregroundColor"), null, null),
            layers,
            font: GetResource<SpriteFont>(element.Attribute("fontRef")),
            isVisible: ToBool(element.Attribute("isVisible"), true));

        return (name, scroller);
    }
}