namespace RetroLibrary.Core.Binding;

public class BindingValueChangedEventArgs(
    object? oldValue,
    object? newValue) : EventArgs
{
    public object? OldValue { get; } = oldValue;

    public object? NewValue { get; } = newValue;
}