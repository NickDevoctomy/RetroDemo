using RetroLibrary.Enums;

namespace RetroLibrary;

public class NineSliceTextureOptions
{
    public int TopMargin { get; set; }

    public int LeftMargin { get; set; }

    public int BottomMargin { get; set; }

    public int RightMargin { get; set; }

    public CachingMode CachingMode { get; set; } = CachingMode.BySize;
}
