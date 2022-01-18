﻿using System.Runtime.InteropServices;

using Microsoft.Win32.SafeHandles;

using static ConsoleGameLib.Base.Graphics;

namespace ConsoleGameLib.Base
{
	public class Input
	{
		[DllImport("Kernel32.dll", SetLastError = true)]
		private static extern bool ReadConsoleInput(
			SafeFileHandle hConsoleInput,
			ref InputRecord lpBuffer,
			uint nLength,
			ref uint lpNumberOfEventsRead);

		[StructLayout(LayoutKind.Explicit)]
		private struct InputRecord
		{
			[FieldOffset(0)] public ushort EventType;
			[FieldOffset(sizeof(ushort))] public KeyEventRecord KeyEvent;
			[FieldOffset(sizeof(ushort))] public MouseEventRecord MouseEvent;
			[FieldOffset(sizeof(ushort))] public WindowBufferSizeRecord WindowBufferSizeEvent;
			[FieldOffset(sizeof(ushort))] public MenuEventRecord MenuEvent;
			[FieldOffset(sizeof(ushort))] public FocusEventRecord FocusEvent;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct FocusEventRecord { public bool bSetFocus; }

		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
		private struct KeyEventRecord
		{
			[FieldOffset(0)] public bool bKeyDown;
			[FieldOffset(sizeof(bool))] public uint wRepeatCount;
			[FieldOffset(sizeof(bool) + sizeof(uint))] public uint wVirtualKeyCode;
			[FieldOffset(sizeof(bool) + sizeof(uint) + sizeof(uint))] public uint wVirtualScanCode;
			[FieldOffset(sizeof(bool) + sizeof(uint) + sizeof(uint) + sizeof(uint))] public char UnicodeChar;
			[FieldOffset(sizeof(bool) + sizeof(uint) + sizeof(uint) + sizeof(uint))] public byte AsciiChar;
			[FieldOffset(sizeof(bool) + sizeof(uint) + sizeof(uint) + sizeof(uint) + sizeof(char))] public ulong dwControlKeyState;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct MenuEventRecord { public uint dwCommandId; }


		[StructLayout(LayoutKind.Sequential)]
		private struct MouseEventRecord
		{
			public Coord dwMousePosition;
			public ulong dwButtonState;
			public ulong dwControlKeyState;
			public ulong dwEventFlags;
		}


		[StructLayout(LayoutKind.Sequential)]
		private struct WindowBufferSizeRecord { public Coord dwSize; }


		/// <summary>
		/// Warning: This method is technically not C# <see langword="async"/>.
		/// </summary>
		/// <param name="vKey"></param>
		/// <returns></returns>
		[DllImport("User32.dll")]
		private static extern short GetAsyncKeyState(int vKey);

		public enum VKey : int
		{
			LBUTTON = 0x01,
			RBUTTON = 0x02,
			CANCEL = 0x03,
			MBUTTON = 0x04,    /* NOT contiguous with L & RBUTTON */

			XBUTTON1 = 0x05,    /* NOT contiguous with L & RBUTTON */
			XBUTTON2 = 0x06,    /* NOT contiguous with L & RBUTTON */

			/*
			 * = 0x07, : reserved
			 */


			BACK = 0x08,
			TAB = 0x09,

			/*
			 * = 0x0A, - = 0x0B, : reserved
			 */

			CLEAR = 0x0C,
			RETURN = 0x0D,

			/*
			 * = 0x0E, - = 0x0F, : unassigned
			 */

			SHIFT = 0x10,
			CONTROL = 0x11,
			MENU = 0x12,
			PAUSE = 0x13,
			CAPITAL = 0x14,

			KANA = 0x15,
			HANGEUL = 0x15,  /* old name - should be here for compatibility */
			HANGUL = 0x15,

			/*
			 * = 0x16, : unassigned
			 */

			JUNJA = 0x17,
			FINAL = 0x18,
			HANJA = 0x19,
			KANJI = 0x19,

			/*
			 * = 0x1A, : unassigned
			 */

			ESCAPE = 0x1B,

			CONVERT = 0x1C,
			NONCONVERT = 0x1D,
			ACCEPT = 0x1E,
			MODECHANGE = 0x1F,

			SPACE = 0x20,
			PRIOR = 0x21,
			NEXT = 0x22,
			END = 0x23,
			HOME = 0x24,
			LEFT = 0x25,
			UP = 0x26,
			RIGHT = 0x27,
			DOWN = 0x28,
			SELECT = 0x29,
			PRINT = 0x2A,
			EXECUTE = 0x2B,
			SNAPSHOT = 0x2C,
			INSERT = 0x2D,
			DELETE = 0x2E,
			HELP = 0x2F,

			/*
			 * 0 - 9 are the same as ASCII '0' - '9' (= 0x30, - = 0x39,)
			 * = 0x3A, - = 0x40, : unassigned
			 * A - Z are the same as ASCII 'A' - 'Z' (= 0x41, - = 0x5A,)
			 */

			LWIN = 0x5B,
			RWIN = 0x5C,
			APPS = 0x5D,

			/*
			 * = 0x5E, : reserved
			 */

			SLEEP = 0x5F,

			NUMPAD0 = 0x60,
			NUMPAD1 = 0x61,
			NUMPAD2 = 0x62,
			NUMPAD3 = 0x63,
			NUMPAD4 = 0x64,
			NUMPAD5 = 0x65,
			NUMPAD6 = 0x66,
			NUMPAD7 = 0x67,
			NUMPAD8 = 0x68,
			NUMPAD9 = 0x69,
			MULTIPLY = 0x6A,
			ADD = 0x6B,
			SEPARATOR = 0x6C,
			SUBTRACT = 0x6D,
			DECIMAL = 0x6E,
			DIVIDE = 0x6F,
			F1 = 0x70,
			F2 = 0x71,
			F3 = 0x72,
			F4 = 0x73,
			F5 = 0x74,
			F6 = 0x75,
			F7 = 0x76,
			F8 = 0x77,
			F9 = 0x78,
			F10 = 0x79,
			F11 = 0x7A,
			F12 = 0x7B,
			F13 = 0x7C,
			F14 = 0x7D,
			F15 = 0x7E,
			F16 = 0x7F,
			F17 = 0x80,
			F18 = 0x81,
			F19 = 0x82,
			F20 = 0x83,
			F21 = 0x84,
			F22 = 0x85,
			F23 = 0x86,
			F24 = 0x87,

			/*
			 * = 0x88, - = 0x8F, : UI navigation
			 */

			NAVIGATION_VIEW = 0x88,
			NAVIGATION_MENU = 0x89,
			NAVIGATION_UP = 0x8A,
			NAVIGATION_DOWN = 0x8B,
			NAVIGATION_LEFT = 0x8C,
			NAVIGATION_RIGHT = 0x8D,
			NAVIGATION_ACCEPT = 0x8E,
			NAVIGATION_CANCEL = 0x8F,

			NUMLOCK = 0x90,
			SCROLL = 0x91,

			/*
			 * NEC PC-9800 kbd definitions
			 */
			OEM_NEC_EQUAL = 0x92,   // '=' key on numpad

			/*
			 * Fujitsu/OASYS kbd definitions
			 */
			OEM_FJ_JISHO = 0x92,   // 'Dictionary' key
			OEM_FJ_MASSHOU = 0x93,   // 'Unregister word' key
			OEM_FJ_TOUROKU = 0x94,   // 'Register word' key
			OEM_FJ_LOYA = 0x95,   // 'Left OYAYUBI' key
			OEM_FJ_ROYA = 0x96,   // 'Right OYAYUBI' key

			/*
			 * = 0x97, - = 0x9F, : unassigned
			 */

			/*
			 * L* & R* - left and right Alt, Ctrl and Shift virtual keys.
			 * Used only as parameters to GetAsyncKeyState() and GetKeyState().
			 * No other API or message will distinguish left and right keys in this way.
			 */
			LSHIFT = 0xA0,
			RSHIFT = 0xA1,
			LCONTROL = 0xA2,
			RCONTROL = 0xA3,
			LMENU = 0xA4,
			RMENU = 0xA5,

			BROWSER_BACK = 0xA6,
			BROWSER_FORWARD = 0xA7,
			BROWSER_REFRESH = 0xA8,
			BROWSER_STOP = 0xA9,
			BROWSER_SEARCH = 0xAA,
			BROWSER_FAVORITES = 0xAB,
			BROWSER_HOME = 0xAC,

			VOLUME_MUTE = 0xAD,
			VOLUME_DOWN = 0xAE,
			VOLUME_UP = 0xAF,
			MEDIA_NEXT_TRACK = 0xB0,
			MEDIA_PREV_TRACK = 0xB1,
			MEDIA_STOP = 0xB2,
			MEDIA_PLAY_PAUSE = 0xB3,
			LAUNCH_MAIL = 0xB4,
			LAUNCH_MEDIA_SELECT = 0xB5,
			LAUNCH_APP1 = 0xB6,
			LAUNCH_APP2 = 0xB7,

			/*
			 * = 0xB8, - = 0xB9, : reserved
			 */

			OEM_1 = 0xBA,   // ';:' for US
			OEM_PLUS = 0xBB,   // '+' any country
			OEM_COMMA = 0xBC,   // ',' any country
			OEM_MINUS = 0xBD,   // '-' any country
			OEM_PERIOD = 0xBE,   // '.' any country
			OEM_2 = 0xBF,   // '/?' for US
			OEM_3 = 0xC0,   // '`~' for US

			/*
			 * = 0xC1, - = 0xC2, : reserved
			 */


			/*
			 * = 0xC3, - = 0xDA, : Gamepad input
			 */

			GAMEPAD_A = 0xC3,
			GAMEPAD_B = 0xC4,
			GAMEPAD_X = 0xC5,
			GAMEPAD_Y = 0xC6,
			GAMEPAD_RIGHT_SHOULDER = 0xC7,
			GAMEPAD_LEFT_SHOULDER = 0xC8,
			GAMEPAD_LEFT_TRIGGER = 0xC9,
			GAMEPAD_RIGHT_TRIGGER = 0xCA,
			GAMEPAD_DPAD_UP = 0xCB,
			GAMEPAD_DPAD_DOWN = 0xCC,
			GAMEPAD_DPAD_LEFT = 0xCD,
			GAMEPAD_DPAD_RIGHT = 0xCE,
			GAMEPAD_MENU = 0xCF,
			GAMEPAD_VIEW = 0xD0,
			GAMEPAD_LEFT_THUMBSTICK_BUTTON = 0xD1,
			GAMEPAD_RIGHT_THUMBSTICK_BUTTON = 0xD2,
			GAMEPAD_LEFT_THUMBSTICK_UP = 0xD3,
			GAMEPAD_LEFT_THUMBSTICK_DOWN = 0xD4,
			GAMEPAD_LEFT_THUMBSTICK_RIGHT = 0xD5,
			GAMEPAD_LEFT_THUMBSTICK_LEFT = 0xD6,
			GAMEPAD_RIGHT_THUMBSTICK_UP = 0xD7,
			GAMEPAD_RIGHT_THUMBSTICK_DOWN = 0xD8,
			GAMEPAD_RIGHT_THUMBSTICK_RIGHT = 0xD9,
			GAMEPAD_RIGHT_THUMBSTICK_LEFT = 0xDA,



			OEM_4 = 0xDB,  //  '[{' for US
			OEM_5 = 0xDC,  //  '\|' for US
			OEM_6 = 0xDD,  //  ']}' for US
			OEM_7 = 0xDE,  //  ''"' for US
			OEM_8 = 0xDF,

			/*
			 * = 0xE0, : reserved
			 */

			/*
			 * Various extended or enhanced keyboards
			 */
			OEM_AX = 0xE1,  //  'AX' key on Japanese AX kbd
			OEM_102 = 0xE2,  //  "<>" or "\|" on RT 102-key kbd.
			ICO_HELP = 0xE3,  //  Help key on ICO
			ICO_00 = 0xE4,  //  00 key on ICO

			PROCESSKEY = 0xE5,

			ICO_CLEAR = 0xE6,


			PACKET = 0xE7,

			/*
			 * = 0xE8, : unassigned
			 */

			/*
			 * Nokia/Ericsson definitions
			 */
			OEM_RESET = 0xE9,
			OEM_JUMP = 0xEA,
			OEM_PA1 = 0xEB,
			OEM_PA2 = 0xEC,
			OEM_PA3 = 0xED,
			OEM_WSCTRL = 0xEE,
			OEM_CUSEL = 0xEF,
			OEM_ATTN = 0xF0,
			OEM_FINISH = 0xF1,
			OEM_COPY = 0xF2,
			OEM_AUTO = 0xF3,
			OEM_ENLW = 0xF4,
			OEM_BACKTAB = 0xF5,

			ATTN = 0xF6,
			CRSEL = 0xF7,
			EXSEL = 0xF8,
			EREOF = 0xF9,
			PLAY = 0xFA,
			ZOOM = 0xFB,
			NONAME = 0xFC,
			PA1 = 0xFD,
			OEM_CLEAR = 0xFE
		}

		WindowsConsole console;
		public bool[] keys = new bool[256];

		public Input(WindowsConsole console)
		{
			this.console = console;
		}

		public void Update()
		{
			for (int i = 0; i < keys.Length; i++)
				keys[i] = GetAsyncKeyState(i) != 0;
		}
	}
}
