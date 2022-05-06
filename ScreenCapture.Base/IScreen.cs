using System.Drawing;

namespace ScreenCapture.Base;

public interface IScreen
{
    /**
     * <summary>Screen identifier (platform-specific)</summary>
     **/
    public int Id { get; }
    
    /**
     * <summary>Screen width (px)</summary>
     **/
    public int Width { get; }
    
    /**
     * <summary>Screen height (px)</summary>
     **/
    public int Height { get; }
    
    /**
     * <summary>Color depth (bpp)</summary>
     **/
    public int BitsPerPixel { get; }
}