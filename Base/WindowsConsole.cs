using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Microsoft.Win32.SafeHandles;

namespace ConsoleGameLib.Base
{
	[SupportedOSPlatform("windows")]
	public class WindowsConsole
	{
		[DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		private static extern SafeFileHandle CreateFile(
			[MarshalAs(UnmanagedType.LPWStr)] string fileName,
			[MarshalAs(UnmanagedType.U4)] uint fileAccess,
			[MarshalAs(UnmanagedType.U4)] uint fileShare,
			IntPtr securityAttributes,
			[MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
			[MarshalAs(UnmanagedType.U4)] int flags,
			IntPtr template);

		

		[DllImport("Kernel32.dll", SetLastError = true)]
		private extern static bool SetConsoleMode(
			SafeFileHandle hConsoleHandle,
			ulong dwMode);

		public readonly short width;
		public readonly short height;

		public readonly SafeFileHandle handle = CreateFile("CONOUT$", 0x40000000, 2, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
		public WindowsConsole(short width, short height)
		{
			if (handle.IsInvalid)
			{
				Console.WriteLine("Could not obtain handle to CONOUT$");
				Environment.Exit(-1);
			}

			if (width > Console.LargestWindowWidth)
				throw new ArgumentOutOfRangeException($"Width {width} suppplied was too large.");
			if (height > Console.LargestWindowHeight)
				throw new ArgumentOutOfRangeException($"Height {height} suppplied was too large.");

			this.width = width;
			this.height = height;

			Console.SetWindowSize(width, height);

			SetConsoleMode(handle, 0x0080 | 0x0008 | 0x0010 | 0x0200 | 0x0004); // ENABLE_EXTENDED_FLAGS | ENABLE_WINDOW_INPUT | ENABLE_MOUSE_INPUT | ENABLE_VIRTUAL_TERMINAL_INPUT | _ENABLE_VIRTUAL_TERMINAL_PROCESSING
		}		
	}
}
