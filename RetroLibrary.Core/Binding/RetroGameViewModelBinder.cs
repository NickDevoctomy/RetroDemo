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
            var boundObjectType = bindingInfo.BoundObject!.GetType();
            var boundProperty = boundObjectType.GetProperty(
                bindingInfo.BoundPropertyName,
                BindingFlags.Public | BindingFlags.Instance);
            if (boundProperty != null)
            {
                bindingInfo.BoundProperty = boundProperty;
            }
        }

        _bindings.Add(bindingInfo);

        if (bindingInfo.SourceProperty != null && bindingInfo.BoundProperty != null)
        {
            var initialValue = bindingInfo.SourceProperty.GetValue(bindingInfo.SourceObject);
            bindingInfo.BoundProperty.SetValue(bindingInfo.BoundObject, initialValue);
        }
    }

    private void ViewModel_PropertyChanged(
        object? sender,
        System.ComponentModel.PropertyChangedEventArgs e)
    {
        var affectedBindings = _bindings.Where(x => x.Path == e.PropertyName && x.SourceObject == sender);
        foreach (var binding in affectedBindings)
        {
            if (binding.SourceProperty == null || binding.BoundProperty == null)
            {
                continue;
            }

            var value = binding.SourceProperty.GetValue(binding.SourceObject);
            binding.BoundProperty.SetValue(binding.BoundObject, value);
        }
    }
}