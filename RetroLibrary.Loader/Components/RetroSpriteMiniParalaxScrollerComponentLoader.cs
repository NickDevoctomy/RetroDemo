using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RetroLibrary.Controls;
using RetroLibrary.Core;
using RetroLibrary.Core.Common;
using RetroLibrary.Core.Components;

namespace RetroLibrary.XmlLoader.Components;

public class RetroSpriteMiniParalaxScrollerComponentLoader(
    IVariableReplacer variableReplacer,
    IColorLoader colorLoader) : ComponentLoaderBase, IComponentLoader
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
                    ToInt(layerElement.Attribute("yOffset"), gameContext, variableReplacer, 0));
                layers.Add(layer);
            }
        }

        var scroller = new RetroSpriteMiniParallaxScroller(
            name,
            ToPoint(element.Attribute("position"), gameContext, variableReplacer, Point.Zero),
            ToPoint(element.Attribute("size"), gameContext, variableReplacer, Point.Zero),
            backgroundColor: ToColor(element.Attribute("backgroundColor"), null, colorLoader, null),
            foregroundColor: ToColor(element.Attribute("foregroundColor"), null, colorLoader, null),
            layers,
            font: GetResource<SpriteFont>(element.Attribute("fontRef"), gameContext.ResourceManager),
            isVisible: ToBool(element.Attribute("isVisible"), true));

        return (name, scroller);
    }
}