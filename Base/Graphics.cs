using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using Microsoft.Win32.SafeHandles;

namespace ConsoleGameLib.Base
{
	[SupportedOSPlatform("windows")]
	public class Graphics
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct Coord
		{
			public short X;
			public short Y;

			public Coord(short X, short Y)
			{
				this.X = X;
				this.Y = Y;
			}
		};

		public struct CharInfo
		{
			public char Char;
			public ConsoleColor fg;
			public ConsoleColor bg;

			public CharInfo(char Char, ConsoleColor fg, ConsoleColor bg)
			{
				this.Char = Char;
				this.fg = fg;
				this.bg = bg;
			}
		}

		private WindowsConsole console;
		private CharInfo[] envData;
		public short width, height;
		public Graphics(WindowsConsole console)
		{
			this.console = console;
			width = console.width;
			height = console.height;

			envData = new CharInfo[width * height];

			for (int i = 0; i < envData.Length; i++)
				envData[i] = new CharInfo();
		}
		public void Print()
		{
			Console.SetCursorPosition(0, 0);
			foreach (var c in envData)
			{
				Console.ForegroundColor = c.fg;
				Console.BackgroundColor = c.bg;
				Console.Write(c.Char);
			}
		}

		public void Fill(CharInfo ci)
		{
			for (int i = 0; i < envData.Length; i++)
			{
				envData[i].Char = ci.Char;
				envData[i].fg = ci.fg;
				envData[i].bg = ci.bg;
			}
		}

		public void Clear()
		{
			Fill(new CharInfo(' ', ConsoleColor.White, ConsoleColor.Black));
		}

		public void AddText(CharInfo[] charInfo, short textWidth, short textHeight, short x = 0, short y = 0)
		{
			for (short yIter = 0; yIter < textHeight; yIter++)
				for (short xIter = 0; xIter < textWidth; xIter++)
				{
					var envDataIndex = Index(x + xIter, y + yIter, width);

					if (envDataIndex > 0 && envDataIndex < envData.Length)
						envData[envDataIndex] = charInfo[Index(xIter, yIter, textWidth)];
				}
		}

		public void AddText(string info, short x = 0, short y = 0)
		{
			short initialX = x;

			for (int i = 0; i < info.Length; i++)
			{
				if (info[i] != '\n')
				{
					envData[Index(x, y, width)].Char = info[i];
					envData[Index(x, y, width)].fg = ConsoleColor.White;
					envData[Index(x, y, width)].bg = ConsoleColor.Black;
					x++;    //	moves 1 to the right... One must be careful when intentionally modifying values that were originally passed as params.
				}
				else
				{
					x = initialX;
					y++;
				}
			}
		}

		public void Show(GameObject g)
		{
			AddText(g.charInfo, g.width, g.height, g.location.X, g.location.Y);
		}

		public static int Index(int x, int y, int width)
		{
			return x + (width * y);
		}
	}
}
