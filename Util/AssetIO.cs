using System.Runtime.Versioning;

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
			

			for (int y = 2; y < height+2; y++)
				for (int x = 0; x < width; x++)
					asset[Index(x, y-2, width)].Char.UnicodeChar = lines[y][x];
				
			

			for (int y = height + 3; y < 2 * height + 2; y++)
				for (int x = 0; x < lines[y].Length; x += 2)
				{
					var twoHexChars = lines[y].Substring(x, 2);
					asset[Index(x/2, y-(height+3), width)].Attributes = Convert.ToUInt16(twoHexChars, 16);
				}
			
			return asset;
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
				await assetFile.WriteAsync(info[i].Char.UnicodeChar);

				if ((i + 1) % width == 0)
					await assetFile.WriteLineAsync();
			}

			assetFile.WriteLine();

			for (int i = 0; i < info.Length; i++)
			{
				await assetFile.WriteAsync(info[i].Attributes.ToString("X2"));

				if ((i + 1) % width == 0)
					assetFile.WriteLine();
			}

			assetFile.Close();
		}
	}
}
