using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace RetroLibrary;

public partial class TabPage(
    string title,
    ObservableCollection<RetroSpriteBase> children) : ObservableObject
{
    [ObservableProperty]
    private string title = title;

    [ObservableProperty]
    private ObservableCollection<RetroSpriteBase> children = children;
}
