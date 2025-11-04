namespace RetroLibrary;

public class ParallaxScrollerOptions
{
    public List<ParallaxScrollerLayer> Layers { get; set; } = [];

    public int ViewportWidth { get; set; } = 800;

    public int ViewportHeight { get; set; } = 600;
}
