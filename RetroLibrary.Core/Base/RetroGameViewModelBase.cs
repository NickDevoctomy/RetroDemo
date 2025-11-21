using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace RetroLibrary.Core.Base;

public class RetroGameViewModelBase : ObservableObject
{
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