using System.Drawing;
using System.Runtime.Versioning;

using static System.ConsoleColor;
using static ConsoleGameLib.Base.Graphics;

namespace ConsoleGameLib.Util
{
	[SupportedOSPlatform("windows")]
	public static class AssetIO
	{
		public static async Task<CharInfo[]> ReadAsync(string path)
		{
			var lines = await File.ReadAllLinesAsync(path);

			short width = Convert.ToInt16(lines[0]);
			short height = Convert.ToInt16(lines[1]);

			CharInfo[] asset = new CharInfo[width * height];

			for (int i = 0; i < asset.Length; i++)
				asset[i] = new CharInfo();


			for (int y = 2; y < height + 2; y++)
				for (int x = 0; x < width; x++)
					asset[Index(x, y - 2, width)].Char = lines[y][x];



			for (int y = height + 3; y < 2 * height + 2; y++)
				for (int x = 0; x < lines[y].Length; x += 2)
				{
					var twoHexChars = lines[y].Substring(x, 2);
					asset[Index(x / 2, y - (height + 3), width)].fg = (ConsoleColor) twoHexChars[0];
					asset[Index(x / 2, y - (height + 3), width)].bg = (ConsoleColor) twoHexChars[1];
				}

			return asset;
		}

		/// <summary>
		/// Thanks https://www.c-sharpcorner.com/article/generating-ascii-art-from-an-resized-using-C-Sharp/
		/// </summary>
		/// <param name="path"></param>
		/// <param name="desiredWidth"></param>
		/// <returns></returns>
		public static CharInfo[] ReadFromPng(string path, short desiredWidth, short desiredHeight)
		{
			var img = new Bitmap(path, true);

			var resized = GetResizedImage(img, desiredWidth, desiredHeight);

			var result = new CharInfo[desiredHeight * desiredWidth];

			for (int y = 0; y < resized.Height; y++)
				for (int x = 0; x < resized.Width; x++)
				{
					Color pixelColor = resized.GetPixel(x, y);

					result[Index(x, y, resized.Width)] = GetCIFromPixel(pixelColor);
				}

			return result;
		}

		private static Bitmap GetResizedImage(Bitmap inputBitmap, int asciiWidth, int asciiHeight)
		{
			//Create a new Bitmap and define its resolution
			Bitmap result = new Bitmap(asciiWidth, asciiHeight);
			Graphics g = Graphics.FromImage((Image)result);

			//The interpolation mode produces high quality resizeds
			g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
			g.DrawImage(inputBitmap, 0, 0, asciiWidth, asciiHeight);
			g.Dispose();

			return result;
		}

		private static Dictionary<(ConsoleColor, ConsoleColor), Color> colors = new Dictionary<(ConsoleColor, ConsoleColor), Color>
		{
			{ (White, White), Color.White },
			{ (White, White), Color.White }
		};

		private static CharInfo GetCIFromPixel(Color pixel)
		{
			char[] asciiChars = { ' ', '░', '▒', '▓', '█' };
			int gray = (pixel.R + pixel.G + pixel.B) / 3;

			char charToUse = asciiChars[(gray * (asciiChars.Length - 1) / 255)];

			var closestConsoleColor = GetClosestConsoleColor(pixel);
			return new CharInfo(charToUse, closestConsoleColor, closestConsoleColor);

			//return new CharInfo('░', ConsoleColor.Yellow, ConsoleColor.DarkGreen);
		}

		private static ConsoleColor GetClosestConsoleColor(Color pixel)
		{
			ConsoleColor ret = 0;
			double rr = pixel.R, gg = pixel.G, bb = pixel.B, delta = double.MaxValue;

			foreach (ConsoleColor cc in Enum.GetValues(typeof(ConsoleColor)))
			{
				var n = Enum.GetName(typeof(ConsoleColor), cc);
				var c = Color.FromName(n == "DarkYellow" ? "Orange" : n); // bug fix
				var t = Math.Pow(c.R - rr, 2.0) + Math.Pow(c.G - gg, 2.0) + Math.Pow(c.B - bb, 2.0);
				if (t == 0.0)
					return cc;
				if (t < delta)
				{
					delta = t;
					ret = cc;
				}
			}
			return ret;
		}

		public static async Task WriteAsync(CharInfo[] info, short width, short height, string path)
		{
			string? dir = Path.GetDirectoryName(path);

			if (dir is not null)
				Directory.CreateDirectory(dir);

			var assetFile = File.CreateText(path);

			await assetFile.WriteLineAsync(Convert.ToString(width));
			await assetFile.WriteLineAsync(Convert.ToString(height));

			for (int i = 0; i < info.Length; i++)
			{
				await assetFile.WriteAsync(info[i].Char);

				if ((i + 1) % width == 0)
					await assetFile.WriteLineAsync();
			}

			assetFile.WriteLine();

			for (int i = 0; i < info.Length; i++)
			{
				await assetFile.WriteAsync(info[i].fg.ToString("X"));
				await assetFile.WriteAsync(info[i].bg.ToString("X"));

				if ((i + 1) % width == 0)
					assetFile.WriteLine();
			}

			assetFile.Close();
		}
	}
}
