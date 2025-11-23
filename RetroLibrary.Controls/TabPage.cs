using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using RetroLibrary.Core.Attributes;
using RetroLibrary.Core.Base;
using RetroLibrary.Core.Binding;

namespace RetroLibrary.Controls;

public partial class TabPage(
    BindingValue<string> title,
    ObservableCollection<RetroSpriteBase> children) : ObservableObject
{
    [RetroSpriteBindableProperty]
    [ObservableProperty]
    private BindingValue<string> title = title;

    [ObservableProperty]
    private ObservableCollection<RetroSpriteBase> children = children;
}