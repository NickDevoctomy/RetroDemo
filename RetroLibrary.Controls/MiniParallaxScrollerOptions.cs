namespace RetroLibrary.Controls;

public class MiniParallaxScrollerOptions
{
    public List<MiniParallaxScrollerMiniParallaxScrollerLayer> Layers { get; set; } = new List<MiniParallaxScrollerMiniParallaxScrollerLayer>();

    public int ViewportWidth { get; set; } = 800;

    public int ViewportHeight { get; set; } = 600;
}
