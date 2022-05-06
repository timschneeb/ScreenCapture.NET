# ScreenCapture.NET
Low-level screen capturing library for .NET Core; **supports only X11/Linux currently**

```c#
// Create ScreenCapture for the current platform (only X11/Linux supported at the moment)
IScreenCapture capture = ScreenCapture.ScreenCaptureFactory.Build();

// Select default screen
IScreen screen = capture.GetDefaultScreen();
Console.WriteLine($"Screen resolution: {s.Width}x{s.Height}px; Color depth: {s.BitsPerPixel}bpp");

// Create snapshot of full screen
IImage snapshot = capture.GetImage(screen);

// Alternatively, create snapshot of a screen region 
IImage smallRectSnapshot = capture.GetImage(width: 400, height: 200, offsetX: 50, offsetY: 50, screen);

// Get single pixel color
Color pixelColor = snapshot.GetPixel(10, 10);

// Chop a specific snapshot into a 2D pixel array (RGB32) with optional boundaries
uint[][] region = snapshot.GetRegion(width: 200, height: 100, x: 0, y: 0);

// Convert to SixLabors.ImageSharp-compatible image object
SixLabors.ImageSharp.Image imageData = snapshot.ToImage();

// Save as png
await snapshot.SavePngAsync("snapshot.png");

// Release unmanaged snapshot memory
snapshot.Dispose();
```
