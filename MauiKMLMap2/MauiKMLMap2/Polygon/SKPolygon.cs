using System.Linq;
using SkiaSharp;
using MauiKMLMap2.Schemas;

namespace MauiKMLMap2.Polygon
{
    internal class SKPolygon : Placemark
    {
        public SKPath Polygon { get; private set; }

        public float GetLeft()
        {
            return (coordinates.MinBy(p => p.X)).X;
        }

        public float GetTop()
        {
            return (coordinates.MinBy(p => p.Y)).Y;
        }

        public float GetRight()
        {
            return (coordinates.MaxBy(p => p.X)).X;
        }

        public float GetBottom()
        {
            return (coordinates.MaxBy(p => p.Y)).Y;
        }

        public float GetWidth()
        {
            return GetRight() - GetLeft();
        }

        public float GetHeight()
        {
            return GetBottom() - GetTop();
        }

        public void PutPolygon()
        {
            Polygon = new SKPath();
            Polygon.MoveTo(coordinates[0]);
            for (int i = 0; i < coordinates.Length - 1; i++)
            {
                Polygon.LineTo(coordinates[i + 1]);

            }
            Polygon.LineTo(coordinates[0]);
            Polygon.Close();

            Bounds = Polygon.Bounds;

        }
    }
}
