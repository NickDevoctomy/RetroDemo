using System.Reflection;
using RetroLibrary.Core.Enums;

namespace RetroLibrary.Core.Binding;

public class BindingInfo
{
    public IBindingValue? DestinationValue { get; private set; }

    public object? DestinationObject { get; set; }

    public PropertyInfo? DestinationProperty { get; set; }

    public string? BoundPropertyName { get; set; }

    public object? SourceObject { get; set; }

    public PropertyInfo? SourceProperty { get; set; }

    public string? Path { get; set; }

    public BindingMode Mode { get; set; } = BindingMode.OneWay;

    public void InitializeDestinationValue()
    {
        if (DestinationObject != null &&
            DestinationProperty != null)
        {
            DestinationValue = DestinationProperty.GetValue(DestinationObject) as IBindingValue;
        }
    }
}