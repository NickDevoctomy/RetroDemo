using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RetroLibrary;

public partial class RetroSpriteTabbedContainer : RetroSpriteContainer
{
    // Pages and selection
    [ObservableProperty]
    private ObservableCollection<TabPage> tabPages = new ();

    [ObservableProperty]
    private int selectedIndex = -1;

    // Styling for tabs
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

    // Content margins exclude the header; effective InnerMargins = ContentMargins with Top + TabHeaderHeight
    [ObservableProperty]
    private Rectangle contentMargins;

    private bool _tabPressed;
    private int _pressedTabIndex = -1;

    // Keep subscription to currently selected page's children
    private TabPage? _currentSelectedPage;

    public RetroSpriteTabbedContainer(
        string name,
        Point position,
        Point size,
        Color? backgroundColor = null,
        Color? foregroundColor = null,
        Rectangle? innerMargins = null,
        SpriteFont? font = null,
        bool buffered = true,
        bool updateWatchedProperties = true)
        : base(
            name,
            position,
            size,
            backgroundColor,
            foregroundColor,
            innerMargins,
            font,
            buffered,
            updateWatchedProperties)
    {
        // Treat provided innerMargins as content margins and lift header above it
        ContentMargins = innerMargins ?? Rectangle.Empty;
        UpdateEffectiveInnerMargins();

        // Subscribe to page list changes
        TabPages.CollectionChanged += TabPages_CollectionChanged;
    }

    public override void SetWatchedProperties(List<string> propertyNames)
    {
        base.SetWatchedProperties(propertyNames);
        propertyNames.Add(nameof(TabPages));
        propertyNames.Add(nameof(SelectedIndex));
        propertyNames.Add(nameof(TabUpTexture));
        propertyNames.Add(nameof(TabDownTexture));
        propertyNames.Add(nameof(TabUpTint));
        propertyNames.Add(nameof(TabDownTint));
        propertyNames.Add(nameof(TabHeaderHeight));
        propertyNames.Add(nameof(TabHorizontalPadding));
        propertyNames.Add(nameof(TabSpacing));
        propertyNames.Add(nameof(TabLeftPadding));
        propertyNames.Add(nameof(ContentMargins));
        propertyNames.Add(nameof(Font));
    }

    protected override void OnRedraw(
        SpriteBatch spriteBatch,
        Point location)
    {
        // Draw tab header (outside of InnerMargins, at top of the control)
        DrawTabsHeader(spriteBatch, location);

        // Draw content (children) via base into effective InnerMargins
        base.OnRedraw(spriteBatch, location);
    }

    protected override void OnUpdate(
        MouseState mouseState,
        MouseState previousMouseState)
    {
        base.OnUpdate(mouseState, previousMouseState);

        // Determine mouse position relative to this container's top-left
        var localMouse = new Point(mouseState.X - Position.X, mouseState.Y - Position.Y);
        var wasPressed = previousMouseState.LeftButton == ButtonState.Pressed;
        var isPressed = mouseState.LeftButton == ButtonState.Pressed;

        // Build rects for tabs
        var tabRects = BuildTabRects();

        // Find hovered tab
        int hoveredIndex = -1;
        for (int i = 0; i < tabRects.Count; i++)
        {
            if (tabRects[i].Contains(localMouse))
            {
                hoveredIndex = i;
                break;
            }
        }

        // Mouse down over a tab
        if (hoveredIndex >= 0 && !wasPressed && isPressed)
        {
            _tabPressed = true;
            _pressedTabIndex = hoveredIndex;
        }

        // Mouse up - commit selection if released over the same tab
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

        // Cancel press state if mouse moved off tabs while holding, or no longer pressed
        if (!isPressed)
        {
            _tabPressed = false;
            _pressedTabIndex = -1;
        }
    }

    protected override void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
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
            // Re-subscribe when the collection instance changes
            TabPages.CollectionChanged -= TabPages_CollectionChanged;
            TabPages.CollectionChanged += TabPages_CollectionChanged;
            UpdateActiveChildren();
        }
    }

    private void TabPages_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        // Manage per-page title change subscriptions for redraw
        if (e.OldItems != null)
        {
            foreach (var item in e.OldItems)
            {
                if (item is TabPage oldPage)
                {
                    oldPage.PropertyChanged -= TabPage_PropertyChanged;
                }
            }
        }
        if (e.NewItems != null)
        {
            foreach (var item in e.NewItems)
            {
                if (item is TabPage newPage)
                {
                    newPage.PropertyChanged += TabPage_PropertyChanged;
                }
            }
        }
        if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
        {
            // Clear and reattach to all
            foreach (var page in TabPages)
            {
                page.PropertyChanged -= TabPage_PropertyChanged;
                page.PropertyChanged += TabPage_PropertyChanged;
            }
        }

        // Clamp or initialize selection and refresh children
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

    private void TabPage_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(TabPage.Title))
        {
            // Title changed, just flag redraw
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(TabPages)));
        }
    }

    private void UpdateActiveChildren()
    {
        // Unhook old
        if (_currentSelectedPage != null)
        {
            _currentSelectedPage.Children.CollectionChanged -= SelectedChildren_CollectionChanged;
        }

        if (SelectedIndex < 0 || SelectedIndex >= TabPages.Count)
        {
            Children = new List<RetroSpriteBase>();
            _currentSelectedPage = null;
            return;
        }

        _currentSelectedPage = TabPages[SelectedIndex];
        _currentSelectedPage.Children.CollectionChanged += SelectedChildren_CollectionChanged;
        Children = _currentSelectedPage.Children.ToList();
    }

    private void SelectedChildren_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        if (_currentSelectedPage != null)
        {
            Children = _currentSelectedPage.Children.ToList();
        }
    }

    private void UpdateEffectiveInnerMargins()
    {
        var effective = new Rectangle(
            ContentMargins.X,
            ContentMargins.Y + TabHeaderHeight,
            ContentMargins.Width,
            ContentMargins.Height);
        InnerMargins = effective;
    }

    private void DrawTabsHeader(SpriteBatch spriteBatch, Point location)
    {
        var tabRects = BuildTabRects();
        for (int i = 0; i < tabRects.Count; i++)
        {
            var rect = tabRects[i];
            var isSelected = i == SelectedIndex;
            var texture = isSelected ? TabDownTexture : TabUpTexture;
            var tint = isSelected ? TabDownTint : TabUpTint;

            if (texture != null)
            {
                var built = texture.BuildTexture(
                    spriteBatch.GraphicsDevice,
                    rect.Width,
                    rect.Height);
                var drawRect = new Rectangle(location.X + rect.X, location.Y + rect.Y, rect.Width, rect.Height);
                spriteBatch.Draw(built, drawRect, tint);
            }

            // Draw title text
            if (Font != null && i < TabPages.Count)
            {
                var title = TabPages[i].Title ?? string.Empty;
                if (!string.IsNullOrEmpty(title))
                {
                    var textSize = Font.MeasureString(title);
                    var textPos = new Vector2(
                        location.X + rect.X + (rect.Width - textSize.X) / 2f,
                        location.Y + rect.Y + (rect.Height - textSize.Y) / 2f);
                    spriteBatch.DrawString(Font, title, textPos, ForegroundColor);
                }
            }
        }
    }

    private List<Rectangle> BuildTabRects()
    {
        var rects = new List<Rectangle>();
        int x = TabLeftPadding;
        int h = TabHeaderHeight;
        int count = TabPages.Count;

        for (int i = 0; i < count; i++)
        {
            int w;
            if (Font != null)
            {
                var title = TabPages[i].Title ?? string.Empty;
                var size = Font.MeasureString(title);
                w = (int)size.X + (TabHorizontalPadding * 2);
            }
            else
            {
                // Fallback width if no font set
                w = 80;
            }

            rects.Add(new Rectangle(x, 0, w, h));
            x += w + TabSpacing;
        }

        return rects;
    }
}
