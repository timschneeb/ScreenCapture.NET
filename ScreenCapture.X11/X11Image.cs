using ScreenCapture.X11.Utils;
using X11;
using Color = System.Drawing.Color;
using IImage = ScreenCapture.Base.IImage;

namespace ScreenCapture.X11;

public class X11Image : IImage
{
    private readonly IntPtr _display;
    private XImage _image;

    public X11Image(IntPtr display, XImage image)
    {
        _display = display;
        _image = image;
    }

    #region Native X11 API
    /**
     * <summary>Get XPixel at coordinates from XImage</summary>
     **/
    public XPixel GetXPixel(int x, int y)
    {
        return X11NativeExtensions.XGetPixel(ref _image, x, y);
    }
    
    /**
     * <summary>Query XColor information from XPixel (slow)</summary>
     **/
    public XColor QueryXColor(XPixel xPixel)
    {
        var colorMap = Xlib.XDefaultColormap(_display, Xlib.XDefaultScreen(_display));
        var xColor = new XColor
        {
            pixel = xPixel
        };
        Xlib.XQueryColor(_display, colorMap, ref xColor);
        return xColor;
    }
    
    /**
     * <summary>Read color data directly from XPixel (fast)</summary>
     **/
    public Color ConvertPixelToRgb(XPixel pixel)
    {
        var b = (byte)(pixel & _image.blue_mask);
        var g = (byte)((pixel & _image.green_mask) >> 8);
        var r = (byte)((pixel & _image.red_mask) >> 16);
        return Color.FromArgb(r,g,b);
    } 
    
    public uint ConvertPixelToRgbUint(XPixel pixel)
    {
        var b = (byte)(pixel & _image.blue_mask);
        var g = (byte)((pixel & _image.green_mask) >> 8);
        var r = (byte)((pixel & _image.red_mask) >> 16);
        return (uint)(((r << 16) | (g << 8) | b) & 0xffffffffL);
    }

    public bool UseXColorQuery { set; get; } = false;
    #endregion
    
    #region Global API
    public Color GetPixel(int x, int y)
    {
        if (x > _image.width)
            throw new ArgumentOutOfRangeException(nameof(x));
        if (y > _image.height)
            throw new ArgumentOutOfRangeException(nameof(y));
        
        var xPixel = GetXPixel(x, y);
        if (UseXColorQuery)
        {
            /* slow */
            var xColor = QueryXColor(xPixel);
            return Color.FromArgb((xColor.red / 256).Clamp(0,255), 
                (xColor.green / 256).Clamp(0,255), 
                (xColor.blue / 256).Clamp(0,255));
        }
        
        return ConvertPixelToRgb(xPixel);
    }

    public uint GetPixelPacked(int x, int y)
    {
        if (x > _image.width)
            throw new ArgumentOutOfRangeException(nameof(x));
        if (y > _image.height)
            throw new ArgumentOutOfRangeException(nameof(y));
        
        return ConvertPixelToRgbUint(GetXPixel(x, y));
    }
    
    public uint[][] GetRegion(int width, int height, int x, int y)
    {
        if (x + width > _image.width)
            throw new ArgumentOutOfRangeException(nameof(height));
        if (y + height > _image.height)
            throw new ArgumentOutOfRangeException(nameof(width));

        var region = new uint[height][];
        for (var i = y; i < y + height; i++)
        {
            region[i - y] = new uint[width]; 
            for (var j = x; j < x + width; j++)
            {
                region[i - y][j - x] = GetPixelPacked(j, i);
            }
        }
        return region;
    }

    public uint[][] GetAll()
    {
        return GetRegion(Width, Height, 0, 0);
    }
    
    public int Width => _image.width;
    public int Height => _image.height;
    public int PixelCount => _image.height * _image.width;
    #endregion

    public void Dispose()
    {
        Xutil.XDestroyImage(ref _image);
    }
}