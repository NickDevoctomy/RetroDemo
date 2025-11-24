using System.Reflection;
using RetroLibrary.Core.Base;

namespace RetroLibrary.Core.Binding;

public class RetroGameViewModelBinder : IBinder
{
    private readonly RetroGameViewModelBase _viewModel;
    private readonly List<BindingInfo> _bindings = [];

    public RetroGameViewModelBinder(RetroGameViewModelBase viewModel)
    {
        _viewModel = viewModel;
        _viewModel.PropertyChanged += ViewModel_PropertyChanged;
    }

    public void AddBinding(BindingInfo bindingInfo)
    {
        if (!string.IsNullOrWhiteSpace(bindingInfo.Path))
        {
            var sourceProp = _viewModel.GetType().GetProperty(
                bindingInfo.Path,
                BindingFlags.Public | BindingFlags.Instance);
            if (sourceProp != null)
            {
                bindingInfo.SourceObject = _viewModel;
                bindingInfo.SourceProperty = sourceProp;
            }
        }

        if (!string.IsNullOrWhiteSpace(bindingInfo.BoundPropertyName))
        {
            var boundObjectType = bindingInfo.DestinationObject!.GetType();
            var boundProperty = boundObjectType.GetProperty(
                bindingInfo.BoundPropertyName,
                BindingFlags.Public | BindingFlags.Instance);
            if (boundProperty != null)
            {
                bindingInfo.DestinationProperty = boundProperty;
            }
        }

        bindingInfo.InitializeDestinationValue();
        _bindings.Add(bindingInfo);

        var destinationValue = bindingInfo.DestinationProperty?.GetValue(bindingInfo.DestinationObject) as IBindingValue;
        if (destinationValue != null)
        {
            destinationValue.ValueChanged += BindingValue_ValueChanged;
        }

        if (bindingInfo.SourceProperty != null && bindingInfo.DestinationProperty != null)
        {
            var initialValue = bindingInfo.SourceProperty.GetValue(bindingInfo.SourceObject);
            destinationValue?.SetValue(initialValue);
        }
    }

    private void BindingValue_ValueChanged(
        object? sender,
        BindingValueChangedEventArgs e)
    {
        var binding = _bindings.SingleOrDefault(x => x.DestinationValue == sender);
        if (binding == null ||
            binding.Mode == Enums.BindingMode.OneTime ||
            binding.Mode == Enums.BindingMode.OneWay)
        {
            return;
        }

        var value = binding.DestinationProperty!.GetValue(binding.DestinationObject) as IBindingValue;
        binding.SourceProperty!.SetValue(binding.SourceObject, value!.GetValue());
    }

    private void ViewModel_PropertyChanged(
        object? sender,
        System.ComponentModel.PropertyChangedEventArgs e)
    {
        var affectedBindings = _bindings.Where(x => x.Path == e.PropertyName && x.SourceObject == sender);
        foreach (var binding in affectedBindings)
        {
            if (binding.SourceProperty == null ||
                binding.DestinationProperty == null ||
                binding.Mode == Enums.BindingMode.OneTime ||
                binding.Mode == Enums.BindingMode.OneWayToSource)
            {
                continue;
            }

            var value = binding.SourceProperty.GetValue(binding.SourceObject);
            var bindingValue = binding.DestinationProperty.GetValue(binding.DestinationObject) as IBindingValue;
            bindingValue?.SetValue(value);
        }
    }
}