using RetroLibrary.Controls.Interfaces;
using RetroLibrary.XmlLoader.SubLoaders.Interfaces;

namespace RetroLibrary.XmlLoader.SubLoaders;

public class ContainerChildCompositorLoader : ISubLoader
{
    public string ElementName => "ChildCompositor";

    public IContainerChildCompositor? Load(
        string type,
        IDictionary<string, string> properties)
    {
        var instance = Activator.CreateInstance(
            Type.GetType(type) ?? throw new InvalidOperationException($"Type '{type}' not found.")) as IContainerChildCompositor;
        if (instance == null)
        {
            // throw an exception instead or continue?
            return null;
        }

        instance.SetProperties(properties);

        return instance;
    }
}