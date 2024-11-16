using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Color = System.Drawing.Color;

namespace ScreenCapture.Base;

public interface IImage : IDisposable
{
    /**
     * <summary>Get pixel color at specified coordinates</summary>
     **/
    Color GetPixel(int x, int y);

    /**
     * <summary>Get pixel colors of rectangular region as a 2D-array</summary>
     **/
    public uint[][] GetRegion(int width, int height, int x, int y);
    
    /**
     * <summary>Get full picture as a 2D-array</summary>
     **/
    public uint[][] GetAll();
    
    /**
     * <summary>Image width (px)</summary>
     **/
    public int Width { get; }
    
    /**
     * <summary>Image height (px)</summary>
     **/
    public int Height { get; }
    
    /**
     * <summary>Pixel count</summary>
     **/
    public int PixelCount { get; }

    /**
     * <summary>Convert full canvas to image object</summary>
     **/
    public Image ToImage()
    {
        return ToImage(Width, Height, 0, 0);
    }

    /**
     * <summary>Convert region to image object</summary>
     **/
    public Image ToImage(int width, int height, int x, int y)
    {
        var image = new Image<Rgba32>(width, height);
        var colors = GetRegion(width, height, x, y);
        for (var i = 0; i < colors.Length; i++)
        {
            for (var j = 0; j < colors[i].Length; j++)
            {
                image[j, i] = new Rgba32(colors[i][j]);
            }
        }
        return image;
    }

    /**
     * <summary>Save full canvas as PNG</summary>
     **/
    public async Task SavePngAsync(string destinationPath)
    {
        await ToImage().SaveAsPngAsync(destinationPath);
    } 
    
    /**
     * <summary>Save region as PNG</summary>
     **/
    public async Task SavePngAsync(string destinationPath, int width, int height, int x, int y)
    {
        await ToImage(width, height, x, y).SaveAsPngAsync(destinationPath);
    }
    
    /**
     * <summary>Treat image as 1D-array and get pixel from index</summary>
     **/
    public Color GetPixelFlattened(int pixelIndex)
    {
        if (pixelIndex < 0 || pixelIndex > Width * Height)
            throw new ArgumentOutOfRangeException(nameof(pixelIndex));
        
        return GetPixel(pixelIndex / Width, pixelIndex % Width);
    } 
}