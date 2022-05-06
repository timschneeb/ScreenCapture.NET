using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Color = System.Drawing.Color;

namespace ScreenCapture.Base;

public interface IImage : IDisposable
{
    /**
     * <summary>Get pixel color at specified coordinates</summary>
     **/
    Color GetPixel(uint x, uint y);

    /**
     * <summary>Get pixel colors of rectangular region as a 2D-array</summary>
     **/
    public uint[][] GetRegion(uint width, uint height, uint x, uint y);
    
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
     * <summary>Convert full canvas to image object</summary>
     **/
    public Image ToImage()
    {
        return ToImage((uint)Width, (uint)Height, 0, 0);
    }

    /**
     * <summary>Convert region to image object</summary>
     **/
    public Image ToImage(uint width, uint height, uint x, uint y)
    {
        var image = new Image<Rgba32>((int)width, (int)height);
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
    public async Task SavePngAsync(string destinationPath, uint width, uint height, uint x, uint y)
    {
        await ToImage(width, height, x, y).SaveAsPngAsync(destinationPath);
    }
    
    /**
     * <summary>Calculate average colors based on random pixels in region</summary>
     */
    public Color AverageColorOfRegionRandomize(uint width, uint height, uint offsetX, uint offsetY, uint pixelsToProcess) {
        int[] total = {0, 0, 0};

        for (var j = 0; j < pixelsToProcess; j++)
        {
            var x = (uint)Random.Shared.Next((int)offsetX, (int)(offsetX + width));
            var y = (uint)Random.Shared.Next((int)offsetY, (int)(offsetY + height));

            var color = GetPixel(x, y);
            total[0] += color.R;
            total[1] += color.G;
            total[2] += color.B;
        }

        for (var j = 0; j < 3; j++)
        {
            total[j] = (byte)(total[j] / pixelsToProcess);
        }

        return Color.FromArgb(total[0], total[1], total[2]);
    }  
    
    /**
     * <summary>Calculate average colors based on all pixels in region</summary>
     * <remarks>Only suitable for small regions</remarks>
     */
    public Color AverageColorOfRegion(uint width, uint height, uint offsetX, uint offsetY) {
        int[] total = {0, 0, 0};
        var count = 0;

        var colors = GetRegion(width, height, offsetX, offsetY);
        for (var i = 0; i < colors.Length; i++)
        {
            for (var j = 0; j < colors[i].Length; j++)
            {
                var color = colors[i][j];
                total[0] += (int)((color >> 16) & 0xFF);
                total[1] += (int)((color >> 8) & 0xFF);
                total[2] += (int)(color & 0xFF);
                count++;
            }
        }
        
        for (var i = 0; i < 3; i++)
        {
            total[i] = (byte)(total[i] / count);
        }

        return Color.FromArgb(total[0], total[1], total[2]);
    }
}