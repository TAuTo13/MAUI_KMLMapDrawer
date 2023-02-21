using System;
using System.Text;
using System.Diagnostics;
using Microsoft.Maui.Controls;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using MauiKMLMap2.Polygon;
using MauiKMLMap2.Items;

namespace MauiKMLMap2
{
	public partial class MainPage : ContentPage
	{
		private string url = null;

		public MainPage()
		{
			InitializeComponent();
		}
		
		private string GenerateTimeLabelText(string labelText,TimeSpan ts)
        {
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(labelText);
			stringBuilder.Append(":");
			stringBuilder.Append(ts.Minutes);
			stringBuilder.Append("m");
			stringBuilder.Append(ts.Seconds);
			stringBuilder.Append("s");
			stringBuilder.Append(ts.Milliseconds);
			stringBuilder.Append("ms");

			return stringBuilder.ToString();
		}

		private void OnDrawButtonPressed(object sender, EventArgs e)
		{
			if (SkiaView != null)
			{
				DrawButton.IsEnabled = false;
				KMLPicker.IsEnabled = false;

				string selectedFileName = ((KML_Items)KMLPicker.SelectedIndex).ToString();
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("https://mauistorage.z11.web.core.windows.net/");
				stringBuilder.Append(selectedFileName);
				stringBuilder.Append(".kml");

				Console.WriteLine(stringBuilder.ToString());

				url = stringBuilder.ToString();

				SkiaView.InvalidateSurface();
			}
		}

		private void OnSKCanvasPaint(object sender, SKPaintSurfaceEventArgs e)
		{
			SKImageInfo info = e.Info;
			SKCanvas canvas = e.Surface.Canvas;

			canvas.Clear();

			if (url != null)
			{
				KMLReader reader = KMLReader.Create();
				reader.Read(url);
				KMLData data = reader.GetData();

				DownloadTimeCountLabel.Text = GenerateTimeLabelText("DownloadTime",reader.DownloadTime);
				LoadTimeCountLabel.Text = GenerateTimeLabelText("LoadTime", reader.LoadTime);

				float kmlBound;
				SKPolygon[] polygons = data.GetPolygons(out kmlBound);
				float scale = MathF.Min(info.Width / kmlBound, info.Height / kmlBound);
				canvas.Scale(scale, scale, data.Left, data.Top);

				SKPaint paint = new SKPaint()
				{
					Style = SKPaintStyle.Stroke,
					StrokeWidth = (float)(4 / scale),
					Color = SKColors.Black,
				};
				Stopwatch sw = Stopwatch.StartNew();
				foreach (var p in polygons)
				{
					canvas.DrawPath(p.Polygon, paint);
				}


				sw.Stop();

				TimeSpan ts = sw.Elapsed;

				DrawTimeCountLabel.Text = GenerateTimeLabelText("DrawTime",ts);
				DrawButton.IsEnabled = true;
				KMLPicker.IsEnabled = true;
			}
		}
	}
}
