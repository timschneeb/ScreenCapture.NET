using System.Drawing;

namespace ScreenCapture.Base;

public interface IScreenCapture : IDisposable
{
    /**
     * <summary>Create image from rectangle with custom resolution</summary>
     * <remarks>If <c>screen</c> is set to null, the default screen will be picked</remarks>
     **/
    public IImage GetImage(uint width, uint height, uint offsetX = 0, uint offsetY = 0, IScreen? screen = null);
    
    /**
     * <summary>Create image from full screen</summary>
     * <remarks>If <c>screen</c> is set to null, the default screen will be picked</remarks>
     **/
    public IImage GetImage(IScreen? screen = null);
    
    /**
     * <summary>Gets detailed information about the default screen</summary>
     **/
    public IScreen GetDefaultScreen();

    /**
     * <summary>Enumerates all available screens</summary>
     **/
    public IReadOnlyList<IScreen> GetScreens();

}