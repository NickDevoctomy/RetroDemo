namespace RetroLibrary.Core.Binding;

public interface IBindingValue
{
    bool HasBindingString { get; }

    public string? BindingString { get; set; }

    public void SetValue(object? value);
}