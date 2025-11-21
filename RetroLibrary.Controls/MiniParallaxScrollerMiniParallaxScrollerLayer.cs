namespace RetroLibrary.Controls;

public class MiniParallaxScrollerMiniParallaxScrollerLayer(
    string name,
    string texturePath,
    float scrollSpeed,
    int yOffset)
{
    public string Name { get; set; } = name;

    public string TexturePath { get; set; } = texturePath;

    public float ScrollSpeed { get; set; } = scrollSpeed;

    public int YOffset { get; set; } = yOffset;
}