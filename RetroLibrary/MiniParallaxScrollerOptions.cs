namespace RetroLibrary;

public class MiniParallaxScrollerOptions
{
    public List<MiniParallaxScrollerLayer> Layers { get; set; } = new List<MiniParallaxScrollerLayer>();

    public int ViewportWidth { get; set; } = 800;

    public int ViewportHeight { get; set; } = 600;
}
