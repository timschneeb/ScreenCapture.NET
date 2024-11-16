using System.Drawing;
using ScreenCapture.Base;
using X11;
// ReSharper disable BuiltInTypeReferenceStyle

namespace ScreenCapture.X11;

public class X11ScreenCapture : IScreenCapture
{
    private readonly IntPtr _display;
    
    public X11ScreenCapture()
    {
        _display = Xlib.XOpenDisplay(null);
        if (_display == IntPtr.Zero)
            throw new Exception("XOpenDisplay failed");
    }
    
    #region Native X11 API
    /**
     * <summary>Allocates a XImage struct and fills it with data</summary>
     * <remarks>You must destroy the XImage object when it is not needed anymore using <c>DestroyXImage</c></remarks>
     * <seealso cref="DestroyXImage"/>
     **/
    public XImage GetXImage(int screenNumber, uint width, uint height, uint offsetX = 0, uint offsetY = 0)
    {
        var xImage = Xlib.XGetImage(_display, Xlib.XRootWindow(_display, screenNumber), (int)offsetX, (int)offsetY,
            width, height, (ulong)Planes.AllPlanes, PixmapFormat.ZPixmap);
        return xImage;
    }
    
    /**
     * <summary>Destroys a XImage struct and frees its unmanaged memory</summary>
     **/
    public Status DestroyXImage(ref XImage image)
    {
        return Xutil.XDestroyImage(ref image);
    }

    /**
     * <summary>Gets the number of the default screen</summary>
     **/
    public int GetDefaultXScreenNumber()
    {
        return Xlib.XDefaultScreen(_display);
    } 
    
    /**
     * <summary>Gets detailed information about the default screen</summary>
     **/
    public Screen GetDefaultXScreen()
    {
        return Xlib.XScreenOfDisplay(_display, GetDefaultXScreenNumber());
    }
    #endregion
    
    #region Global API
    public IImage GetImage(uint width, uint height, uint offsetX = 0, uint offsetY = 0, IScreen? screen = null)
    {
        var screenNumber = screen?.Id ?? Xlib.XDefaultScreen(_display);
        return new X11Image(_display, GetXImage(screenNumber, width, height, offsetX, offsetY));
    }

    public IImage GetImage(IScreen? screen = null)
    {
        var targetScreen = screen ?? GetDefaultScreen();
        return GetImage((uint)targetScreen.Width, (uint)targetScreen.Height, 0, 0, targetScreen);
    }

    public IScreen GetDefaultScreen()
    {
        return new X11Screen(GetDefaultXScreen(), GetDefaultXScreenNumber());
    }

    public IReadOnlyList<IScreen> GetScreens()
    {
        var count = Xlib.XScreenCount(_display);
        var screens = new List<X11Screen>();
        for (var i = 0; i < count; ++i)
        {
            screens.Add(new X11Screen(Xlib.XScreenOfDisplay(_display, GetDefaultXScreenNumber()), 
                GetDefaultXScreenNumber()));
        }
        return screens;
    }
    #endregion
    
    public void Dispose()
    {
        Xlib.XCloseDisplay(_display);
    }
}