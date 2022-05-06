using System.Drawing;

var capture = ScreenCapture.ScreenCaptureFactory.Build();

void PrintColorAt(uint x, uint y)
{
    Console.WriteLine($"x={x} y={y}\t{capture?.GetImage(1920, 1080).GetPixel(x,y)}");
}

void PrintColorRegion2D(IReadOnlyList<Color[]> colors)
{
    for (var i = 0; i < colors.Count; i++)
    {
        for (var j = 0; j < colors[i].Length; j++)
        {
            Console.Write(colors[i][j].ToNearestNamedColor().Item1 +"\t");
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}

var resolution = (1920u, 1080u);

//await Task.Delay(1000);
while (true)
{
    var img = capture.GetImage();

    //var region = img.GetRegion(100, 800, 100, 240);

    var avg = img.AverageColorOfRegionRandomize(100, 800, 100, 240, 10);
    Console.WriteLine(avg.ToNearestNamedColor().Item1);
    
    //await img.SavePngAsync("/home/tim/test.png");
    
    // Display a few pixels in the center as text in the console
    //var reg = img.GetRegion(5, 10, resolution.Item1/2, resolution.Item2/2);
    //PrintColorRegion2D(reg);
    img.Dispose();
    //break;
    
    await Task.Delay(50);
}
