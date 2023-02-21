using System;
using System.Diagnostics;
using System.Xml;
using SkiaSharp;

namespace MauiKMLMap2.Polygon
{
    internal class KMLReader
    {
        public TimeSpan DownloadTime { get; set; }
        public TimeSpan LoadTime { get; set; }

        private static KMLReader _reader = new KMLReader();

        private KMLData data;


        private KMLReader()
        {

        }

        public static KMLReader Create()
        {
            return _reader;
        }

        public void Read(string path)
        {
            Stopwatch sw = Stopwatch.StartNew();
            using XmlReader reader = XmlReader.Create(path);
            sw.Stop();
            DownloadTime = sw.Elapsed;
            sw.Reset();

            sw.Start();
            data = new KMLData();

            SKPoint point = new();
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "coordinates":
                            if (reader.Read())
                            {

                                string coords = reader.Value.Trim();
                                string[] split = coords.Split(new char[] { '\n' });

                                SKPolygon polygon = new();

                                SKPoint[] points = new SKPoint[split.Length];

                                for (int i = 0; i < points.Length; i++)
                                {
                                    string[] sp = split[i].Split(new char[] { ',' });
                                    point.X = float.Parse(sp[0]);
                                    point.Y = float.Parse(sp[1]);

                                    points[i] = point;
                                }
                                polygon.coordinates = points;
                                data.AddPolygon(polygon);
                            }
                            break;
                        case "name":
                            if (reader.Read())
                            {
                                string name = reader.Value.Trim();
                                data.Name = name;
                            }
                            break;
                        case "description":
                            if (reader.Read())
                            {
                                string description = reader.Value.Trim();
                                data.Description = description;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            sw.Stop();

            LoadTime = sw.Elapsed;
        }

        public KMLData GetData() { return data; }
    }
}
