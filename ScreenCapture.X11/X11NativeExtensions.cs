using System.Runtime.InteropServices;
using X11;

namespace ScreenCapture.X11;

public class X11NativeExtensions
{
    [DllImport("libX11.so.6")]
    public static extern ulong XGetPixel(ref XImage image, int x, int y);
}