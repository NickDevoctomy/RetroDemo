using System.Xml.Linq;
using RetroLibrary.Loader.Common;

namespace RetroLibrary.Loader.Components;

public class RetroSpriteNineSliceButtonComponentLoader(IVariableReplacer variableReplacer) : IComponentLoader
{
    public bool IsApplicable(XElement element)
    {
        return
            element.Name == "RetroSpriteNineSliceButton" ||
            element.Attribute("type")!.Value == "RetroLibrary.RetroSpriteNineSliceButton, RetroLibrary";
    }

    public Task<(string Id, object Value)> LoadComponentAsync(
        RetroGameContext gameContext,
        XElement element,
        CancellationToken cancellationToken)
    {
        var id = element.Attribute("id")!.Value;
        var size = variableReplacer.ReplaceAllVariables(
            gameContext,
            element.Attribute("size")!.Value);
        ////var button = new RetroSpriteNineSliceButton(
        ////    element.Attribute("name")!.Value,
        ////    element.Attribute("text")!.Value,

    }
}
