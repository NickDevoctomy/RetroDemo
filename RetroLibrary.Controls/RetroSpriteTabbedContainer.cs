using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RetroLibrary.Controls.Interfaces;
using RetroLibrary.Core.Drawing;

namespace RetroLibrary.Controls;

public partial class RetroSpriteTabbedContainer : RetroSpriteContainer
{
    private readonly HashSet<TabPage> _subscribedPages = [];

    [ObservableProperty]
    private ObservableCollection<TabPage> tabPages = [];

    [ObservableProperty]
    private int selectedIndex = -1;

    [ObservableProperty]
    private NineSliceTexture2D? tabUpTexture;

    [ObservableProperty]
    private NineSliceTexture2D? tabDownTexture;

    [ObservableProperty]
    private Color tabUpTint = Color.White;

    [ObservableProperty]
    private Color tabDownTint = Color.White;

    [ObservableProperty]
    private int tabHeaderHeight = 24;

    [ObservableProperty]
    private int tabHorizontalPadding = 12;

    [ObservableProperty]
    private int tabSpacing = 4;

    [ObservableProperty]
    private int tabLeftPadding = 4;

    [ObservableProperty]
    private NineSliceTexture2D? tabPageTexture;

    [ObservableProperty]
    private Color tabPageTint = Color.White;

    [ObservableProperty]
    private int tabOverlapPixels = 2;

    [ObservableProperty]
    private Rectangle contentMargins;

    private bool _tabPressed;
    private int _pressedTabIndex = -1;
    private TabPage? _currentSelectedPage;

    public RetroSpriteTabbedContainer(
        string name,
        Point position,
        Point size,
        Color? backgroundColor = null,
        Color? foregroundColor = null,
        Rectangle? innerPadding = null,
        Color? tabUpTint = null,
        Color? tabDownTint = null,
        Color? tabPageTint = null,
        NineSliceTexture2D? tabUpTexture = null,
        NineSliceTexture2D? tabDownTexture = null,
        NineSliceTexture2D? tabPageTexture = null,
        SpriteFont? font = null,
        IContainerChildCompositor? childCompositor = null,
        Rectangle? margins = null,
        Rectangle? padding = null,
        bool isVisible = true)
        : base(
            name,
            position,
            size,
            backgroundColor,
            foregroundColor,
            innerPadding,
            font,
            childCompositor,
            margins,
            padding,
            isVisible)
    {
        TabUpTint = tabUpTint ?? Color.White;
        TabDownTint = tabDownTint ?? Color.White;
        TabPageTint = tabPageTint ?? Color.White;
        TabUpTexture = tabUpTexture;
        TabDownTexture = tabDownTexture;
        TabPageTexture = tabPageTexture;

        ContentMargins = innerPadding ?? Rectangle.Empty;
        UpdateEffectiveInnerMargins();

        TabPages.CollectionChanged += TabPages_CollectionChanged;
        EnsurePageSubscriptions();
    }

    protected override void OnRedraw(
        SpriteBatch spriteBatch,
        Rectangle bounds)
    {
        var tabRects = BuildTabRects();
        var selected = SelectedIndex;

        for (int i = 0; i < tabRects.Count; i++)
        {
            if (i == selected)
            {
                continue;
            }

            DrawSingleTab(spriteBatch, bounds.Location, i, tabRects[i], i == selected);
        }

        var innerLocation = new Point(
            bounds.X + InnerPadding.X,
            bounds.Y + InnerPadding.Y - TabOverlapPixels);
        var innerSize = new Point(
            Size.X - InnerPadding.X - InnerPadding.Width,
            Size.Y - InnerPadding.Y - InnerPadding.Height + TabOverlapPixels);

        if (TabPageTexture != null && innerSize.X > 0 && innerSize.Y > 0)
        {
            var pageTex = TabPageTexture.BuildTexture(spriteBatch.GraphicsDevice, innerSize.X, innerSize.Y);
            spriteBatch.Draw(pageTex, new Rectangle(innerLocation, innerSize), TabPageTint);
        }

        if (selected >= 0 && selected < tabRects.Count)
        {
            DrawSingleTab(spriteBatch, bounds.Location, selected, tabRects[selected], true);
        }

        base.OnRedraw(spriteBatch, bounds);
    }

    protected override void OnUpdate(
        MouseState mouseState,
        MouseState previousMouseState)
    {
        base.OnUpdate(mouseState, previousMouseState);

        var localMouse = new Point(mouseState.X - Position.X, mouseState.Y - Position.Y);
        var wasPressed = previousMouseState.LeftButton == ButtonState.Pressed;
        var isPressed = mouseState.LeftButton == ButtonState.Pressed;
        var tabRects = BuildTabRects();
        int hoveredIndex = -1;
        for (int i = 0; i < tabRects.Count; i++)
        {
            if (tabRects[i].Contains(localMouse))
            {
                hoveredIndex = i;
                break;
            }
        }

        if (hoveredIndex >= 0 && !wasPressed && isPressed)
        {
            _tabPressed = true;
            _pressedTabIndex = hoveredIndex;
        }

        if (!isPressed && wasPressed && _tabPressed)
        {
            if (hoveredIndex >= 0 && hoveredIndex == _pressedTabIndex)
            {
                if (hoveredIndex != SelectedIndex)
                {
                    SelectedIndex = hoveredIndex;
                }
            }

            _tabPressed = false;
            _pressedTabIndex = -1;
        }

        if (!isPressed)
        {
            _tabPressed = false;
            _pressedTabIndex = -1;
        }
    }

    protected override void OnPropertyChanging(PropertyChangingEventArgs e)
    {
        base.OnPropertyChanging(e);

        if (e.PropertyName == nameof(TabPages))
        {
            TabPages.CollectionChanged -= TabPages_CollectionChanged;
            foreach (var page in _subscribedPages)
            {
                page.PropertyChanged -= TabPage_PropertyChanged;
            }

            _subscribedPages.Clear();
        }
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (string.IsNullOrEmpty(e.PropertyName))
        {
            return;
        }

        if (e.PropertyName == nameof(SelectedIndex))
        {
            UpdateActiveChildren();
        }
        else if (e.PropertyName == nameof(TabHeaderHeight) || e.PropertyName == nameof(ContentMargins))
        {
            UpdateEffectiveInnerMargins();
        }
        else if (e.PropertyName == nameof(TabPages))
        {
            TabPages.CollectionChanged += TabPages_CollectionChanged;
            EnsurePageSubscriptions();
            UpdateActiveChildren();
        }
    }

    private void TabPages_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems != null)
        {
            foreach (var item in e.OldItems)
            {
                if (item is TabPage oldPage && _subscribedPages.Contains(oldPage))
                {
                    oldPage.PropertyChanged -= TabPage_PropertyChanged;
                    _subscribedPages.Remove(oldPage);
                }
            }
        }

        if (e.NewItems != null)
        {
            foreach (var item in e.NewItems)
            {
                if (item is TabPage newPage && !_subscribedPages.Contains(newPage))
                {
                    newPage.PropertyChanged += TabPage_PropertyChanged;
                    _subscribedPages.Add(newPage);
                }
            }
        }

        if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
        {
            foreach (var page in _subscribedPages)
            {
                page.PropertyChanged -= TabPage_PropertyChanged;
            }

            _subscribedPages.Clear();
            EnsurePageSubscriptions();
        }

        if (TabPages.Count == 0)
        {
            SelectedIndex = -1;
        }
        else if (SelectedIndex < 0 || SelectedIndex >= TabPages.Count)
        {
            SelectedIndex = 0;
        }

        UpdateActiveChildren();
    }

    private void EnsurePageSubscriptions()
    {
        foreach (var page in TabPages)
        {
            if (_subscribedPages.Contains(page))
            {
                continue;
            }

            page.PropertyChanged += TabPage_PropertyChanged;
            _subscribedPages.Add(page);
        }

        var removed = _subscribedPages.Where(p => !TabPages.Contains(p)).ToList();
        foreach (var page in removed)
        {
            page.PropertyChanged -= TabPage_PropertyChanged;
            _subscribedPages.Remove(page);
        }
    }

    private void TabPage_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(TabPage.Title))
        {
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(TabPages)));
        }
    }

    private void UpdateActiveChildren()
    {
        if (_currentSelectedPage != null)
        {
            _currentSelectedPage.Children.CollectionChanged -= SelectedChildren_CollectionChanged;
        }

        if (SelectedIndex < 0 || SelectedIndex >= TabPages.Count)
        {
            Children = [];
            _currentSelectedPage = null;
            return;
        }

        _currentSelectedPage = TabPages[SelectedIndex];
        _currentSelectedPage.Children.CollectionChanged += SelectedChildren_CollectionChanged;
        Children = [.. _currentSelectedPage.Children];
    }

    private void SelectedChildren_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (_currentSelectedPage != null)
        {
            Children = [.. _currentSelectedPage.Children];
        }
    }

    private void UpdateEffectiveInnerMargins()
    {
        var effective = new Rectangle(
            ContentMargins.X,
            ContentMargins.Y + TabHeaderHeight,
            ContentMargins.Width,
            ContentMargins.Height);
        InnerPadding = effective;
    }

    private void DrawSingleTab(
        SpriteBatch spriteBatch,
        Point location,
        int tabIndex,
        Rectangle rect,
        bool isSelected)
    {
        var texture = isSelected ? TabDownTexture : TabUpTexture;
        var tint = isSelected ? TabDownTint : TabUpTint;

        if (texture != null)
        {
            var built = texture.BuildTexture(
                spriteBatch.GraphicsDevice,
                rect.Width,
                rect.Height);
            var drawRect = new Rectangle(
                location.X + rect.X,
                location.Y + rect.Y,
                rect.Width,
                rect.Height);
            spriteBatch.Draw(
                built,
                drawRect,
                tint);
        }

        if (Font != null && tabIndex >= 0 && tabIndex < TabPages.Count)
        {
            var title = TabPages[tabIndex].Title?.Value ?? string.Empty;
            if (!string.IsNullOrEmpty(title))
            {
                var textSize = Font.MeasureString(title);
                var textPos = new Vector2(
                    location.X + rect.X + ((rect.Width - textSize.X) / 2f),
                    location.Y + rect.Y + TabOverlapPixels + ((rect.Height - textSize.Y) / 2f));
                spriteBatch.DrawString(Font, title, textPos, ForegroundColor);
            }
        }
    }

    private List<Rectangle> BuildTabRects()
    {
        var rects = new List<Rectangle>();
        var x = TabLeftPadding;
        var h = TabHeaderHeight;
        var count = TabPages.Count;

        for (var i = 0; i < count; i++)
        {
            int w;
            if (Font != null)
            {
                var title = TabPages[i].Title?.Value ?? string.Empty;
                var size = Font.MeasureString(title);
                w = (int)size.X + (TabHorizontalPadding * 2);
            }
            else
            {
                w = 80;
            }

            rects.Add(new Rectangle(x, 0, w, h));
            x += w + TabSpacing;
        }

        return rects;
    }
}