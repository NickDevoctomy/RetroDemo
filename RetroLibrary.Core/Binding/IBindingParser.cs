namespace RetroLibrary.Core.Binding;

public interface IBindingParser
{
    public bool IsBindingString(string? value);

    public BindingInfo Parse(
        object boundObject,
        string bindingString);
}