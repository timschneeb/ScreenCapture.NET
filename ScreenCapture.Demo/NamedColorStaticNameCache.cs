  // Using-statements:
  using System;
  using System.Collections.Generic;
  using System.Drawing;
  using System.Linq;
  using System.Reflection;

  /// <summary>
  /// Static class to assist with looking up known, named colors, by name.
  /// </summary>
  public static class NamedColorStaticCache
  {
    /// <summary>
    /// Stores the lookup cache of RGB colors to known names.
    /// </summary>
    private static Dictionary<int, string> rgbLookupCache;

    /// <summary>
    /// Initializes static members of the <see cref="NamedColorStaticCache"/> class.
    /// </summary>
    static NamedColorStaticCache()
    {
      rgbLookupCache = new Dictionary<int, string>();
      Type colorType = typeof(Color);
      PropertyInfo[] knownColorProperties = colorType
        .GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
        .Where(t => t.PropertyType == typeof(Color))
        .ToArray();

      // Avoid treating "transparent" as "white".
      AddToCache(Color.White, "White");

      foreach (PropertyInfo pi in knownColorProperties)
      {
        Color asColor = (Color)(pi.GetValue(null) ?? Color.Empty);
        AddToCache(asColor, pi.Name);
      }
    }

    /// <summary>
    /// Looks up the name of the specified <see cref="Color"/>.
    /// </summary>
    /// <param name="toLookup">The <see cref="Color"/> to lookup a name for.</param>
    /// <returns>A string of the associated name of the specified <see cref="Color"/>.</returns>
    public static string LookupName(this Color toLookup)
    {
      int rgb = toLookup.ToRGB();
      if (rgbLookupCache.ContainsKey(rgb))
      {
        return rgbLookupCache[rgb];
      }

      return string.Empty;
    }

    /// <summary>
    /// Adds the specified <see cref="Color"/> to a lookup cache of named colors.
    /// </summary>
    /// <param name="toAdd">The <see cref="Color"/> to add to the lookup cache.</param>
    /// <param name="name">The name of the <see cref="Color"/> to add to the lookup cache.</param>
    /// <returns>True if adding successful, else, false (the color was already in the cache).</returns>
    public static bool AddToCache(this Color toAdd, string name)
    {
      int rgb = toAdd.ToRGB();
      if (rgbLookupCache.ContainsKey(rgb))
      {
        return false;
      }

      rgbLookupCache.Add(rgb, name);
      return true;
    }
    
    public static int ToRGB(this Color toRGB)
    {
      return
        (toRGB.R << 16) |
        (toRGB.G << 8) |
        toRGB.B;
    }

    /// <summary>
    /// Takes the specified input <see cref="Color"/>, and translates it to its nearest counterpart, using root square sum.
    /// </summary>
    /// <param name="toNearest">The <see cref="Color"/> to look up to the nearest named color.</param>
    /// <returns>A tuple structure of name, color, error.</returns>
    public static Tuple<string, Color, double> ToNearestNamedColor(this Color toNearest)
    {
      string foundName = string.Empty;
      Color foundColor = Color.Black;
      double error = double.MaxValue;

      int toNearestRGB = toNearest.ToRGB();
      if (rgbLookupCache.ContainsKey(toNearestRGB))
      {
        foundName = rgbLookupCache[toNearestRGB];
        foundColor = toNearest;
        error = 0;
      }
      else
      {
        foreach (KeyValuePair<int, string> pair in rgbLookupCache)
        {
          int rgb = pair.Key;
          byte r = (byte)(rgb >> 16);
          byte g = (byte)(rgb << 16 >> 24);
          byte b = (byte)(rgb << 24 >> 24);
          int dr = r - toNearest.R;
          int dg = g - toNearest.G;
          int db = b - toNearest.B;
          double newError =
            Math.Sqrt(
              (dr * dr) +
              (dg * dg) +
              (db * db));

          if (newError < error)
          {
            foundName = pair.Value;
            foundColor = Color.FromArgb(r, g, b);
            error = newError;
          }

          if (newError <= .0005)
          {
            break;
          }
        }
      }

      return Tuple.Create(foundName, foundColor, error);
    }
  }