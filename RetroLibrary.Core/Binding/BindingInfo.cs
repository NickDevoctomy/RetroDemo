using System.Reflection;

namespace RetroLibrary.Core.Binding;

public class BindingInfo
{
    public object? BoundObject { get; set; }

    public PropertyInfo? BoundProperty { get; set; }

    public string? BoundPropertyName { get; set; }

    public object? SourceObject { get; set; }

    public PropertyInfo? SourceProperty { get; set; }

    public string? Path { get; set; }
}