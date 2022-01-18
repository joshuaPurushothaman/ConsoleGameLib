using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using Microsoft.Win32.SafeHandles;

namespace ConsoleGameLib.Base
{
	[SupportedOSPlatform("windows")]
	public class Graphics
	{		
		[DllImport("Kernel32.dll", SetLastError = true)]
		private static extern bool WriteConsoleOutputW(
			SafeFileHandle hConsoleOutput,
			CharInfo[] lpBuffer,
			Coord dwBufferSize,
			Coord dwBufferCoord,
			ref SmallRect lpWriteRegion);

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

		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
		public struct CharUnion
		{
			[FieldOffset(0)] public char UnicodeChar;
			[FieldOffset(0)] public byte AsciiChar;
		}

		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
		public struct CharInfo
		{
			[FieldOffset(0)] public CharUnion Char;
			[FieldOffset(2)] public ushort Attributes;

			public ConsoleColor ForegroundColor => (ConsoleColor)((this.Attributes & 0x0F));
			public ConsoleColor BackgroundColor => (ConsoleColor)((this.Attributes & 0xF0) >> 4);

			public CharInfo(char character = ' ', ConsoleColor? foreground = null, ConsoleColor? background = null)
			{
				this.Char = new CharUnion() { UnicodeChar = character };
				this.Attributes = (ushort)((int)(foreground ?? 0) | (((ushort)(background ?? 0)) << 4));
			}

			public CharInfo(byte character, ConsoleColor? foreground = null, ConsoleColor? background = null)
			{
				this.Char = new CharUnion() { AsciiChar = character };
				this.Attributes = (ushort)((int)(foreground ?? 0) | (((ushort)(background ?? 0)) << 4));
			}

			public void SetColor(ConsoleColor fg, ConsoleColor bg)
			{
				Attributes = (ushort)(((int)bg << 4) | (int)fg);
			}

			public static bool operator ==(CharInfo first, CharInfo second)
			{
				return first.Char.UnicodeChar == second.Char.UnicodeChar
					&& first.Char.AsciiChar == second.Char.AsciiChar
					&& first.Attributes == second.Attributes;
			}

			public static bool operator !=(CharInfo first, CharInfo second)
			{
				return !(first == second);
			}

			public override bool Equals(object? other)
			{
				if (other == null)
					return false;
				else
					return this == (CharInfo)other;
			}

			public override int GetHashCode()
			{
				/// ugh...
				return Char.UnicodeChar << 4 | Attributes;
			}

			public void SetChar(char c)
			{
				this.Char.UnicodeChar = c;
			}
		}

		WindowsConsole console;
		CharInfo[] envData;
		SmallRect rect;

		[StructLayout(LayoutKind.Sequential)]
		public struct SmallRect
		{
			public short Left;
			public short Top;
			public short Right;
			public short Bottom;
		}


		public short width, height;
		public Graphics(WindowsConsole console)
		{
			this.console = console;
			this.width = console.width;
			this.height = console.height;

			envData = new CharInfo[width * height];

			for (int i = 0; i < envData.Length; i++)
				envData[i] = new CharInfo();

			rect = new SmallRect() { Left = 0, Top = 0, Right = width, Bottom = height };
		}
		public bool Print()
		{
			return WriteConsoleOutputW(console.handle, envData,
				new Coord() { X = width, Y = height },
				new Coord() { X = 0, Y = 0 },
				ref rect);
		}

		public void Fill(CharInfo ci)
		{
			for (int i = 0; i < envData.Length; i++)
			{
				envData[i].Char = ci.Char;
				envData[i].Attributes = ci.Attributes;
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
					envData[Index(x, y, width)].SetChar(info[i]);
					envData[Index(x, y, width)].SetColor(ConsoleColor.White, ConsoleColor.Black);
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
