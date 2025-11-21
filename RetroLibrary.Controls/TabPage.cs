using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using RetroLibrary.Core.Base;

namespace RetroLibrary.Controls;

public partial class TabPage(
    string title,
    ObservableCollection<RetroSpriteBase> children) : ObservableObject
{
    [ObservableProperty]
    private string title = title;

    [ObservableProperty]
    private ObservableCollection<RetroSpriteBase> children = children;
}