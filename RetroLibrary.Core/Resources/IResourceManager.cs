namespace RetroLibrary.Core.Resources;

public interface IResourceManager
{
    public IReadOnlyDictionary<string, object> Resources { get; }

    public void AddResource(
        string id,
        object resource);

    public T GetResource<T>(string id);
}