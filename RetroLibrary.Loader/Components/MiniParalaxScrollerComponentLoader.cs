using System.Xml.Linq;
using RetroLibrary.Controls;
using RetroLibrary.Core;
using RetroLibrary.Core.Common;
using RetroLibrary.Core.Components;
using RetroLibrary.Core.Resources;

namespace RetroLibrary.XmlLoader.Components;

public class MiniParalaxScrollerComponentLoader(
    ITexture2DResourceLoader texture2DResourceLoader,
    IVariableReplacer variableReplacer) : ComponentLoaderBase, IComponentLoader
{
    public bool IsApplicable(XElement element)
    {
        return element.Name == "MiniParalaxScroller" ||
               element.Attribute("type")!.Value == "RetroLibrary.Controls.MiniParalaxScroller, RetroLibrary.Controls";
    }

    public (string Id, object Value) LoadComponent(
        RetroGameContext gameContext,
        XElement element)
    {
        var name = element.Attribute("name")!.Value;

        var layers = new List<MiniParallaxScrollerLayer>();
        var layersRoot = element.Element("Layers");
        if (layersRoot != null)
        {
            foreach (var layerElement in layersRoot.Elements("Layer"))
            {
                var layer = new MiniParallaxScrollerLayer(
                    layerElement.Attribute("texturePath")!.Value,
                    ToFloat(layerElement.Attribute("scrollSpeed"), 0f),
                    ToInt(layerElement.Attribute("yOffset"), gameContext, variableReplacer, 0));
                layers.Add(layer);
            }
        }

        var options = new MiniParallaxScrollerOptions
        {
            ViewportWidth = ToInt(element.Attribute("viewportWidth"), gameContext, variableReplacer, 0),
            ViewportHeight = ToInt(element.Attribute("viewportHeight"), gameContext, variableReplacer, 0),
            Layers = layers
        };
        return (name, new MiniParallaxScroller(
            texture2DResourceLoader,
            options));
    }
}
