using RetroLibrary.Controls.Interfaces;

namespace RetroLibrary.XmlLoader.SubLoaders.Interfaces;

public interface ISubLoader
{
    public string ElementName { get; }

    public IContainerChildCompositor? Load(
        string type,
        IDictionary<string, string> properties);
}