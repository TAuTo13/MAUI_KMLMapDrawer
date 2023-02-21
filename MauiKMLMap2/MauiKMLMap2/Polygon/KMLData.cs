using System;
using System.Collections.Generic;
using MauiKMLMap2.Schemas;

namespace MauiKMLMap2.Polygon
{
    internal class KMLData:KMLDocuments
    {
        private List<SKPolygon> Placemarks { get; set; }

        public float Left = float.MaxValue;
        public float Top = float.MaxValue;
        public float Right = 0;
        public float Bottom = 0;

        public float Width = 0;
        public float Height = 0;

        public KMLData()
        {
            Placemarks = new List<SKPolygon>();
        }


        public SKPolygon[] GetPolygons(out float Bound)
        {
            SKPolygon[] polygons = Placemarks.ToArray();
            for (int i = 0; i < polygons.Length; i++)
            {
                float left = polygons[i].GetLeft();
                float top = polygons[i].GetTop();
                float right = polygons[i].GetRight();
                float bottom = polygons[i].GetBottom();

                if (Left > left) Left = left;
                if (Top > top) Top = top;
                if (Right < right) Right = right;
                if (Bottom < bottom) Bottom = bottom;
                Width = Right - Left;
                Height = Bottom - Top;
            }

            for (int i = 0; i < polygons.Length; i++)
            {
                for (int j = 0; j < polygons[i].coordinates.Length; j++)
                {
                    polygons[i].coordinates[j].Y = (Height - (polygons[i].coordinates[j].Y - Top)) + Top;
                }
                polygons[i].PutPolygon();
            }

            Bound = MathF.Max(Width, Height);

            return polygons;
        }

        public void AddPolygon(SKPolygon polygon)
        {
            Placemarks.Add(polygon);
        }
    }
}
