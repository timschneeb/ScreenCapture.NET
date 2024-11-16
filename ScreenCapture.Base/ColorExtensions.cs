using System.Drawing;

namespace ScreenCapture.Base;

public static class ColorExtensions
{
    public static uint ToUInt32(this Color c)
    {
        return (uint)(((c.A << 24) | (c.R << 16) | (c.G << 8) | c.B) & 0xffffffffL);
    }
    
    public static Color ToColor(this uint value, bool opaque = false)
    {
        return Color.FromArgb(opaque ? 0xFF : (byte)((value >> 24) & 0xFF),
            (byte)((value >> 16) & 0xFF),
            (byte)((value >> 8) & 0xFF),
            (byte) (value & 0xFF));
    }
}