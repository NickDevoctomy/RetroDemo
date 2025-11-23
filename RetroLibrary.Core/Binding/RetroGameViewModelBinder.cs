using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
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

        _bindings.Add(bindingInfo);

        if (bindingInfo.SourceProperty != null && bindingInfo.DestinationProperty != null)
        {
            var initialValue = bindingInfo.SourceProperty.GetValue(bindingInfo.SourceObject);
            bindingInfo.DestinationProperty.SetValue(bindingInfo.DestinationObject, initialValue);
        }

        // !!! This needs testing, no idea if it works yet.
        var observableObject = bindingInfo.DestinationObject as ObservableObject;
        if (observableObject != null &&
            bindingInfo.Mode == Enums.BindingMode.TwoWay)
        {
            observableObject.PropertyChanged += ObservableObject_PropertyChanged;
        }
    }

    // !!! This needs testing, no idea if it works yet.
    private void ObservableObject_PropertyChanged(
        object? sender,
        System.ComponentModel.PropertyChangedEventArgs e)
    {
        var affectedBindings = _bindings.Where(x => x.Path == e.PropertyName && x.DestinationObject == sender);
        foreach (var binding in affectedBindings)
        {
            if (binding.SourceProperty == null ||
                binding.DestinationProperty == null ||
                binding.Mode == Enums.BindingMode.OneTime ||
                binding.Mode == Enums.BindingMode.OneWay)
            {
                continue;
            }

            var value = binding.DestinationProperty.GetValue(binding.DestinationObject);
            binding.SourceProperty.SetValue(binding.SourceObject, value);
        }
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
            binding.DestinationProperty.SetValue(binding.DestinationObject, value);
        }
    }
}