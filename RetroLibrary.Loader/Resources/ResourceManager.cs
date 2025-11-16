
namespace RetroLibrary.Loader.Resources;

public class ResourceManager : IResourceManager
{
    private readonly Dictionary<string, object> _resources = new ();

    public IReadOnlyDictionary<string, object> Resources => _resources;

    public void AddResource(
        string id,
        object resource)
    {
        _resources.Add(id, resource);
    }

    public T GetResource<T>(
        string id)
    {
        if (!_resources.TryGetValue(id, out object? value))
        {
            throw new KeyNotFoundException($"Resource with id '{id}' not found.");
        }

        return (T)value;
    }
}
