## RetroDemo

This is a rough little project containing some proof of concept code for creating a retro 8bit style UI using MonoGame.

### Controls

#### Core Controls

* **RetroSpriteBase**
  - Base class for all retro UI controls
  - Features buffered/unbuffered rendering modes
  - Mouse interaction support (hover, click, press states)
  - Events: `Clicking`, `Released`, `Clicked`
  - Property change tracking with automatic redraw
  - Configurable position, size, background/foreground colors, and fonts

* **RetroSpriteSmartButton**
  - Enhanced button control extending RetroSpriteBase
  - Utilizes 9-slice scaling for flexible sizing
  - Supports hover and click states with visual feedback
  - Toggle button functionality with `IsToggleButton` property
  - Customizable up/down textures and tint colors
  - Text rendering with configurable fonts
  - Events for click handling

#### Container Controls

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

#### Texture Generation

* **NineSliceTexture2D**
  - 9-slice texture scaling system for UI elements
  - Configurable margin settings via `NineSliceTextureOptions`
  - Properties: `TopMargin`, `LeftMargin`, `BottomMargin`, `RightMargin`
  - Enables scalable textures without distortion
  - Cached texture generation for performance

* **Linear Retro Gradient**
  - 2 colour linear gradient support with retro styling
  - Configurable via `LinearRetroGradientOptions`:
    - `FromPoint` and `ToPoint` for direction control
    - `FromColor` and `ToColor` for gradient colors
    - `GradientStops` for authentic 8-bit quantized appearance (default: 8)
  - Supports any angle and direction
  - Cached rendering for performance optimization

* **Radial Retro Gradient**  
  - 2 colour radial gradient support with retro styling
  - Configurable via `RadialRetroGradientOptions`:
    - `CentrePoint` for gradient center positioning
    - `Radius` for gradient size control
    - `FromColor` and `ToColor` for gradient colors  
    - `GradientStops` for authentic 8-bit quantized appearance (default: 8)
  - Cached rendering for performance optimization

#### Scrolling Effects

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

#### Utility Extensions

* **ColorExtensions**
  - Helper methods for color manipulation and gradient generation
  - Supports retro-style color quantization
  - Gradient color calculation utilities

### Key Features

- **Performance Optimized**: Buffered rendering with automatic cache invalidation
- **Mouse Interaction**: Full mouse support with hover and click states  
- **Modular Design**: Extensible base classes for custom controls
- **Retro Styling**: Authentic 8-bit appearance with quantized gradients
- **Flexible Texturing**: 9-slice scaling and gradient generation
- **Container System**: Hierarchical UI layout with automatic input handling

### Screenshots

![Screenshot](https://github.com/NickDevoctomy/RetroDemo/blob/main/Resources/Screenshots/1.png?raw=true)