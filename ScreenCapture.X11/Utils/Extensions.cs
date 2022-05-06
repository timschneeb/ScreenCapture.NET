namespace ScreenCapture.X11.Utils;

public static class Extensions
{
    public static T Clamp<T>(this T value, T min, T max) where T : IComparable<T>
    {
        if (value.CompareTo(min) < 0)
            return min;
        if (value.CompareTo(max) > 0)
            return max;
        return value;
    }
}