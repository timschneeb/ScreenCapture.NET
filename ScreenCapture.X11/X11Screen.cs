using ScreenCapture.Base;
using X11;

namespace ScreenCapture.X11;

public class X11Screen : IScreen
{
    public X11Screen(Screen xScreen, int screenNumber)
    {
        Id = screenNumber;
        Width = xScreen.width;
        Height = xScreen.height;
        BitsPerPixel = xScreen.root_depth;
    }

    public int Id { get; }
    public int Width { get; }
    public int Height { get; }
    public int BitsPerPixel { get; }
}