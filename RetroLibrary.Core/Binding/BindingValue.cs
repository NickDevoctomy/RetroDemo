namespace RetroLibrary.Core.Binding;

public class BindingValue<T> : IBindingValue
{
    private T? _value;

    public BindingValue(string bindingString)
    {
        BindingString = bindingString;
    }

    public BindingValue(T? value)
    {
        Value = value;
    }

    public event EventHandler<BindingValueChangedEventArgs>? ValueChanged;

    public string? BindingString { get; set; }

    public bool HasBindingString => !string.IsNullOrEmpty(BindingString);

    public T? Value
    {
        get
        {
            return _value;
        }

        set
        {
            if (!EqualityComparer<T?>.Default.Equals(_value, value))
            {
                var prevValue = _value;
                _value = value;
                ValueChanged?.Invoke(this, new BindingValueChangedEventArgs(
                    prevValue,
                    value));
            }
        }
    }

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

    public object? GetValue()
    {
        return Value;
    }
}