namespace RetroLibrary;

public class ParallaxScrollerLayer(
    string texturePath,
    float scrollSpeed,
    int yOffset)
{
    public string TexturePath { get; set; } = texturePath;

    public float ScrollSpeed { get; set; } = scrollSpeed;

    public int YOffset { get; set; } = yOffset;
}
