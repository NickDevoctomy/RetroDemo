using RetroLibrary.Core.Base;
using System.Reflection;

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
        // Resolve source info (from view model)
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

        // Resolve target property: explicit BoundPropertyName overrides Path usage
        var targetPropertyName = bindingInfo.BoundPropertyName ?? bindingInfo.BoundProperty?.Name;
        if (string.IsNullOrWhiteSpace(targetPropertyName))
        {
            // If no explicit target property specified, try common conventions: Text, Value, Content
            var candidateNames = new[] { bindingInfo.BoundPropertyName, "Text", "Value", "Content" };
            var boundObjectType = bindingInfo.BoundObject!.GetType();
            var found = candidateNames.Where(x => !string.IsNullOrWhiteSpace(x))
                                       .Select(name => boundObjectType.GetProperty(name!, BindingFlags.Public | BindingFlags.Instance))
                                       .FirstOrDefault(p => p != null);
            if (found != null)
            {
                bindingInfo.BoundProperty = found;
            }
        }
        else
        {
            var boundObjectType = bindingInfo.BoundObject!.GetType();
            var boundProperty = boundObjectType.GetProperty(targetPropertyName!, BindingFlags.Public | BindingFlags.Instance);
            if (boundProperty != null)
            {
                bindingInfo.BoundProperty = boundProperty;
            }
        }

        _bindings.Add(bindingInfo);

        // Perform initial push of value if source resolved
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
            if (binding.SourceProperty == null)
            {
                continue;
            }
            var value = binding.SourceProperty.GetValue(binding.SourceObject);
            binding.BoundProperty?.SetValue(binding.BoundObject, value);
        }
    }
}