using System.Runtime.InteropServices;
using ScreenCapture.Base;

namespace ScreenCapture;

public static class ScreenCaptureFactory
{
    public static IScreenCapture Build()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return new X11.X11ScreenCapture();
        }

        throw new PlatformNotSupportedException("Only Linux is currently supported");
    }
}