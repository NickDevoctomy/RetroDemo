using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RetroLibrary;

public partial class RetroSpriteTabbedContainer : RetroSpriteContainer
{
    [ObservableProperty]
    private ObservableCollection<TabPage> tabPages = new ();

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
        ContentMargins = innerMargins ?? Rectangle.Empty;
        UpdateEffectiveInnerMargins();

        TabPages.CollectionChanged += TabPages_CollectionChanged;
    }

    public override void SetWatchedProperties(List<string> propertyNames)
    {
        base.SetWatchedProperties(propertyNames);
        propertyNames.Add(nameof(TabPages));
        ////propertyNames.Add(nameof(SelectedIndex));
        propertyNames.Add(nameof(TabUpTexture));
        propertyNames.Add(nameof(TabDownTexture));
        propertyNames.Add(nameof(TabUpTint));
        propertyNames.Add(nameof(TabDownTint));
        propertyNames.Add(nameof(TabHeaderHeight));
        propertyNames.Add(nameof(TabHorizontalPadding));
        propertyNames.Add(nameof(TabSpacing));
        propertyNames.Add(nameof(TabLeftPadding));
        propertyNames.Add(nameof(TabPageTexture));
        propertyNames.Add(nameof(TabPageTint));
        propertyNames.Add(nameof(TabOverlapPixels));
        propertyNames.Add(nameof(ContentMargins));
        propertyNames.Add(nameof(Font));
    }

    protected override void OnRedraw(
        SpriteBatch spriteBatch,
        Point location)
    {
        var tabRects = BuildTabRects();
        var selected = SelectedIndex;

        for (int i = 0; i < tabRects.Count; i++)
        {
            if (i == selected)
            {
                continue;
            }

            DrawSingleTab(spriteBatch, location, i, tabRects[i], i == selected);
        }

        var innerLocation = new Point(
            location.X + InnerMargins.X,
            location.Y + InnerMargins.Y - TabOverlapPixels);
        var innerSize = new Point(
            Size.X - InnerMargins.X - InnerMargins.Width,
            Size.Y - InnerMargins.Y - InnerMargins.Height + TabOverlapPixels);

        if (TabPageTexture != null && innerSize.X > 0 && innerSize.Y > 0)
        {
            var pageTex = TabPageTexture.BuildTexture(spriteBatch.GraphicsDevice, innerSize.X, innerSize.Y);
            spriteBatch.Draw(pageTex, new Rectangle(innerLocation, innerSize), TabPageTint);
        }

        if (selected >= 0 && selected < tabRects.Count)
        {
            DrawSingleTab(spriteBatch, location, selected, tabRects[selected], true);
        }

        base.OnRedraw(spriteBatch, location);
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
        }
    }

    protected override void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine($"Property changed: {e.PropertyName}");
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
            UpdateActiveChildren();
        }
    }

    private void TabPages_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
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
            foreach (var page in TabPages)
            {
                ////!!! handle leak
                ////!!! page.PropertyChanged -= TabPage_PropertyChanged;

                page.PropertyChanged += TabPage_PropertyChanged;
            }
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

    private void TabPage_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
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
            Children = new ();
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

    private void DrawSingleTab(
        SpriteBatch spriteBatch,
        Point location,
        int tabIndex,
        Rectangle rect,
        bool isSelected)
    {
        System.Diagnostics.Debug.WriteLine($"Drawing single tab {tabIndex}.");
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
            var title = TabPages[tabIndex].Title ?? string.Empty;
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
                var title = TabPages[i].Title ?? string.Empty;
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
