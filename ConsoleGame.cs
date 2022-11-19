using System.Diagnostics;
using System.Runtime.Versioning;
using ConsoleGameLib.Base;

namespace ConsoleGameLib
{
	[SupportedOSPlatform("windows")]
	public abstract class ConsoleGame
	{
		private WindowsConsole console;
		protected Graphics graphics;
		protected Input input;
		protected Stopwatch stopwatch = new Stopwatch();
		protected TimeSpan dt;

		public ConsoleGame(short width, short height)
		{
			console = new WindowsConsole(width, height);
			graphics = new Graphics(console);
			input = new Input(console);
		}

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
		protected virtual async Task<bool> Setup()
		{
			return true;
		}

		/// <summary>
		/// Update game data by overriding this method in your game class.
		/// </summary>
		/// <param name="cki"></param>
		/// <returns>whether to continue or not</returns>
		/// <exception cref="NotImplementedException"></exception>
		protected virtual async Task<bool> Update()
		{
			throw new NotImplementedException();
		}
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

		public async Task Run()
		{
			Console.Title = GetType().Name;

			bool shouldContinue = await Setup();

			stopwatch.Start();

			var tp1 = stopwatch.Elapsed;
			var tp2 = stopwatch.Elapsed;

			while (shouldContinue)
			{
				tp2 = stopwatch.Elapsed;
				dt = tp2 - tp1;
				tp1 = tp2;

				input.Update();

				graphics.Clear();
				shouldContinue = await Update();
				graphics.Print();
			}
		}
	}
}