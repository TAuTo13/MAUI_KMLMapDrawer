using SkiaSharp;

namespace MauiKMLMap2.Schemas
{
    internal class Placemark
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public SKRect Bounds { get; set; }

        public SKPoint[] coordinates { get; set; }
    }
}
