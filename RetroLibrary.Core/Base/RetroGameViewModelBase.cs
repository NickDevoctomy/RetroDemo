using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RetroLibrary.Core.Binding;

namespace RetroLibrary.Core.Base;

public class RetroGameViewModelBase : ObservableObject
{
    private IBinder? _binder;

    public IBinder Binder
    {
        get
        {
            _binder ??= new RetroGameViewModelBinder(this);
            return _binder;
        }
    }

    public RelayCommand? GetRelayCommandByPath(string path)
    {
        var propertyInfo = GetType().GetProperty(path);
        if (propertyInfo != null)
        {
            var relayCommand = propertyInfo.GetValue(this) as RelayCommand;
            return relayCommand;
        }

        return null;
    }
}