## RetroDemo 🎮

This is a rough little project containing some proof of concept code for creating a retro 8bit style UI using MonoGame.

### Controls 🕹️

#### Core Controls

* **RetroSpriteBase**
  - Base class for all retro UI controls
  - Mouse interaction support (hover, click, press states)
  - Events: `Clicking`, `Released`, `Clicked`
  - Property change tracking with automatic redraw
  - Configurable position, size, background/foreground colors, and fonts

* **RetroSpriteSmartButton** 🔘
  - Enhanced button control extending RetroSpriteBase
  - Utilizes 9-slice scaling for flexible sizing
  - Supports hover and click states with visual feedback
  - Toggle button functionality with `IsToggleButton` property
  - Customizable up/down textures and tint colors
  - Text rendering with configurable fonts
  - Events for click handling

* **RetroSpriteLabel** 🏷️
  - Simple text display control with center alignment (will add more alignment options soon)
  - Configurable text content via `Text` property
  - Supports custom fonts, colors, and positioning
  - Lightweight control for static text display
  - Perfect for titles, captions, and informational text

* **RetroSpriteProgressBar** 📊
  - Animated progress indicator with gradient fill
  - Configurable `Value` property (0.0 to 1.0 range)
  - Customizable gradient colors via `FromColor` and `ToColor`
  - Optional 9-slice border support with `BorderTexture` and `BorderTint`
  - Smooth linear gradient rendering with retro quantization
  - Ideal for loading screens, health bars, and progress indicators

* **RetroSpriteTexturePanel** 🖼️
  - Texture display control with support for Retro Gradients
  - Allows for laying out textures and gradients in the UI

#### Container Controls 📦

* **RetroSpriteContainer**
  - Container control for grouping multiple child sprites
  - Automatic child rendering and mouse input distribution
  - Offset mouse coordinates for child controls
  - Renders all children to an internal texture for performance

* **RetroSpriteGroupContainer**
  - Advanced container with border and label support
  - Customizable border textures using 9-slice scaling
  - Group label with background texture support
  - Configurable inner margins for child positioning
  - Border and label tinting options
  - Adjustable label offset and border margins

#### Texture Generation 🎨

* **NineSliceTexture2D**
  - 9-slice texture scaling system for UI elements
  - Configurable margin settings via `NineSliceTextureOptions`
  - Properties: `TopMargin`, `LeftMargin`, `BottomMargin`, `RightMargin`
  - Enables scalable textures without distortion
  - Cached texture generation for performance

* **Linear Retro Gradient** 🌈
  - 2 colour linear gradient support with retro styling
  - Configurable via `LinearRetroGradientOptions`:
    - `FromPoint` and `ToPoint` for direction control
    - `FromColor` and `ToColor` for gradient colors
    - `GradientStops` for authentic 8-bit quantized appearance (default: 8)
  - Supports any angle and direction
  - Cached rendering for performance optimization

* **Radial Retro Gradient** ⭕  
  - 2 colour radial gradient support with retro styling
  - Configurable via `RadialRetroGradientOptions`:
    - `CentrePoint` for gradient center positioning
    - `Radius` for gradient size control
    - `FromColor` and `ToColor` for gradient colors  
    - `GradientStops` for authentic 8-bit quantized appearance (default: 8)
  - Cached rendering for performance optimization

#### Scrolling Effects 🌊

* **MiniParallaxScroller**
  - Multi-layer parallax scrolling system
  - Configurable via `MiniParallaxScrollerOptions`:
    - `ViewportWidth` and `ViewportHeight` for display area
    - `Layers` collection for multiple scrolling layers
  - Each layer (`MiniParallaxScrollerLayer`) supports:
    - `TexturePath` for layer texture
    - `ScrollSpeed` for individual layer speed control
    - `YOffset` for vertical positioning
  - Optimized for horizontal scrolling effects
  - Automatic texture wrapping and seamless scrolling

### Layout Engine

* **XmlRetroGameLoderService**
  - XML-based layout loader for retro UI
  - Parses XML files to create and configure RetroSprite controls
  - Supports nested containers and complex layouts
  - Simplifies UI creation with declarative syntax
  - View model suppport with data binding

### Key Features ✨

- **Performance Optimized**: Buffered rendering with automatic cache invalidation
- **Mouse Interaction**: Full mouse support with hover and click states  
- **Modular Design**: Extensible base classes for custom controls
- **Retro Styling**: Authentic 8-bit appearance with quantized gradients
- **Flexible Texturing**: 9-slice scaling and gradient generation
- **Container System**: Hierarchical UI layout with automatic input handling
- **Layout Loading**: XML-based layout definitions for easy UI creation

### Example Layout Xml

```xml
<Scene name="Main"
       backgroundColor="red"
       viewModelType="RetroDemo.GameViewModel, RetroDemo">

    <Resources>
        <Font id="DefaultFont" />
        <RadialRetroGradient
            id="sunset"
            centrePoint="GameWidth/2,GameHeight-80"
            radius="GameWidth"
            fromColor="Yellow"
            toColor="Purple"
            gradientStops="8" />

        <NineSliceTexture
            id="surface"
            path="Content/Textures/surface.png"
            top="4"
            left="4"
            bottom="4"
            right="4" />
        <NineSliceTexture
            id="surfaceGrey"
            path="Content/Textures/surfacegrey.png"
            top="4"
            left="4"
            bottom="4"
            right="4" />
        <NineSliceTexture
            id="border"
            path="Content/Textures/border.png"
            top="4"
            left="4"
            bottom="4"
            right="4" />
        <NineSliceTexture
            id="buttonUp"
            path="Content/Textures/greybuttonup.png"
            top="4"
            left="4"
            bottom="8"
            right="4" />
        <NineSliceTexture
            id="buttonDown"
            path="Content/Textures/greybuttondown.png"
            top="6"
            left="4"
            bottom="6"
            right="4" />
        <NineSliceTexture
            id="tab"
            path="Content/Textures/tab.png"
            top="4"
            left="4"
            bottom="4"
            right="4" />
    </Resources>

    <Components>
        <Sprite type="RetroLibrary.Controls.RetroSpriteTexturePanel, RetroLibrary.Controls"
                name="Sunset"
                position="0,0"
                size="GameWidth,GameHeight"
                textureRef="sunset" />

        <Sprite type="RetroLibrary.Controls.RetroSpriteLabel, RetroLibrary.Controls"
                name="FpsLabel"
                position="8,8"
                size="200,32"
                text="FPS = 000"
                fontRef="DefaultFont"
                foregroundColor="white">
        </Sprite>        

        <Sprite type="RetroLibrary.Controls.RetroSpriteMiniParallaxScroller, RetroLibrary.Controls"
                name="ParalaxScroller"
                position="0,0"
                size="GameWidth,GameHeight"
                fontRef="DefaultFont"
                isVisible="false">
            <Layers>
                <Layer
                    name="Grass"
                    texturePath="Content/Textures/grass.png"
                    scrollSpeed="6.0"
                    yOffset="0" />
                <Layer
                    name="Road"
                    texturePath="Content/Textures/road.png"
                    scrollSpeed="4.0"
                    yOffset="30" />
                <Layer
                    name="Sand"
                    texturePath="Content/Textures/sandyrocks.png"
                    scrollSpeed="2.0"
                    yOffset="60" />
            </Layers>
        </Sprite>

        <Sprite type="RetroLibrary.Controls.RetroSpriteTabbedContainer, RetroLibrary.Controls"
                name="TabbedContainer"
                position="100,150"
                size="400,300"
                foregroundColor="White"
                fontRef="DefaultFont"
                tabPageTextureRef="surfaceGrey"
                tabPageTint="LightGray"
                tabPageTintAlpha="0.2"
                tabUpTextureRef="tab"
                tabUpTint="LightGray"
                tabUpTintAlpha="0.5"
                tabDownTextureRef="tab"
                tabDownTint="LightGray"
                tabDownTintAlpha="0.2">
            <TabPages>
                <TabPage title="Apple">
                    <Sprite type="RetroLibrary.Controls.RetroSpriteNineSliceButton, RetroLibrary.Controls"
                            name="AppleTabButton"
                            position="8,8"
                            size="200,50"
                            text="Do stuff!"
                            fontRef="DefaultFont"
                            isToggleButton="false"
                            foregroundColor="White"
                            upTint="LightGreen"
                            downTint="LightGreen"
                            upTextureRef="buttonUp"
                            downTextureRef="buttonDown"
                            clickCommand="AppleTabButton_ClickedCommand"/>                    
                </TabPage>
                <TabPage title="Hello World!!!">
                    <Sprite type="RetroLibrary.Controls.RetroSpriteProgressBar, RetroLibrary.Controls"
                            name="TestProgressBar"
                            position="8,8"
                            size="ParentWidth-16,50"
                            value="0.75"
                            borderTextureRef="border"
                            borderTint="Red">
                    </Sprite>
                </TabPage>  
                <TabPage title="Oranges" />
            </TabPages>
        </Sprite>        

        <Sprite type="RetroLibrary.Controls.RetroSpriteNineSliceButton, RetroLibrary.Controls"
                name="OrangeButton"
                position="20,60"
                size="200,50"
                text="Orange"
                fontRef="DefaultFont"
                isToggleButton="false"
                foregroundColor="White"
                upTint="Orange"
                downTint="Orange"
                upTextureRef="buttonUp"
                downTextureRef="buttonDown"
                clickCommand="OrangeButton_ClickedCommand"/>

        <Sprite type="RetroLibrary.Controls.RetroSpriteNineSliceButton, RetroLibrary.Controls"
                name="ExitButton"
                position="GameWidth-208,8"
                size="200,50"
                text="Exit"
                fontRef="DefaultFont"
                foregroundColor="White"
                upTint="Green"
                downTint="Green"
                upTextureRef="buttonUp"
                downTextureRef="buttonDown"        
                clickCommand="ExitButton_ClickedCommand"/>
    </Components>
</Scene>
```

### Screenshots 📸

![Screenshot](https://github.com/NickDevoctomy/RetroDemo/blob/main/Resources/Screenshots/1.png?raw=true)