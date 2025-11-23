namespace RetroLibrary.Core.Binding;

public class BindingValue<T> : IBindingValue
{
    public BindingValue(string bindingString)
    {
        BindingString = bindingString;
    }

    public BindingValue(T? value)
    {
        Value = value;
    }

    public string? BindingString { get; set; }

    public bool HasBindingString => !string.IsNullOrEmpty(BindingString);

    public T? Value { get; set; }

    public void SetValue(T? value)
    {
        Value = value;
    }

    public void SetValue(object? value)
    {
        if (value is T typedValue)
        {
            SetValue(typedValue);
        }
    }
}