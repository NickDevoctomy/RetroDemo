using RetroLibrary.Controls.Interfaces;

namespace RetroLibrary.XmlLoader.Components;

public class ContainerChildCompositorLoader
{
    public IContainerChildCompositor? Load(
        string type,
        IDictionary<string, string> properties)
    {
        var instance = Activator.CreateInstance(
            Type.GetType(type) ?? throw new InvalidOperationException($"Type '{type}' not found.")) as IContainerChildCompositor;

        instance.SetProperties(properties);

        return instance as IContainerChildCompositor;
    }
}