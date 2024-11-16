using System.Drawing;

namespace ScreenCapture.Base;

public static class ImageExtensions
{
    /**
     * <summary>Calculate average colors based on random pixels in region</summary>
     */
    public static Color AverageColorOfRegionRandomize(this IImage img, int width, int height, int offsetX, int offsetY, uint pixelsToProcess) {
        int[] total = {0, 0, 0};

        for (var j = 0; j < pixelsToProcess; j++)
        {
            var x = Random.Shared.Next(offsetX, offsetX + width);
            var y = Random.Shared.Next(offsetY, offsetY + height);

            var color = img.GetPixel(x, y);
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
    public static Color AverageColorOfRegion(this IImage img, int width, int height, int offsetX, int offsetY) {
        int[] total = {0, 0, 0};
        var count = 0;

        var colors = img.GetRegion(width, height, offsetX, offsetY);
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