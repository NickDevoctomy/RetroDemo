using System.Xml.Linq;
using Microsoft.Xna.Framework;
using RetroLibrary.Loader.Common;

namespace RetroLibrary.Loader.Extensions;

public class XmlExtensions
{
    public static Point ToPoint(
        this XAttribute element,
        IVariableReplacer variableReplacer)
    {
        var rawValue = element.Value;
        rawValue = variableReplacer.ReplaceAllVariables(rawValue);

        return new Point(
            int.Parse(element.Attribute("x")!.Value),
            int.Parse(element.Attribute("y")!.Value));
    }
}
