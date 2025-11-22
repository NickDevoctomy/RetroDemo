using System.Reflection;

namespace RetroLibrary.Core.Binding;

public class BindingInfo
{
    // The target object that will receive updates
    public object? BoundObject { get; set; }

    // The target property that will be updated when the source changes
    public PropertyInfo? BoundProperty { get; set; }

    // Optional explicit target property name (used if BoundProperty not already resolved)
    public string? BoundPropertyName { get; set; }

    // The source object that raises change notifications
    public object? SourceObject { get; set; }

    // The source property info resolved from SourcePath
    public PropertyInfo? SourceProperty { get; set; }

    // The source property name (parsed from binding Path token). Previously called Path.
    public string? Path { get; set; }
}