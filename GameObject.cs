using ConsoleGameLib.Geometry;
using System.Runtime.Versioning;
using static ConsoleGameLib.Base.Graphics;

namespace ConsoleGameLib
{
	[SupportedOSPlatform("windows")]
	public class GameObject
	{
		public Coord location;
		public short width, height;
		public CharInfo[] charInfo;
		public double massKg;
		public Vector2d netForce;

		protected Vector2d velocity;

		public GameObject(Coord location, short width, short height, double massKg, CharInfo[] charInfo)
		{
			this.location = location;
			this.width = width;
			this.height = height;
			this.massKg = massKg;
			velocity = new Vector2d();
			netForce = new Vector2d();
			this.charInfo = charInfo;
		}

		public bool IsCollision(GameObject other)
		{
			return
				(location.X <= (other.location.X + other.width + 1)) &&
				((location.X + width) >= other.location.X + 1) &&
				(location.Y <= (other.location.Y + other.height + 1)) &&
				((location.Y + height) >= other.location.Y + 1);
		}

		public void UpdatePosition(TimeSpan dt)
		{
			var accel = netForce * dt.TotalSeconds / massKg;

			velocity.x += accel.x;
			velocity.y += accel.y;

			location.X += (short) (velocity.x * dt.TotalSeconds);
			location.Y += (short) (velocity.y * dt.TotalSeconds);
		}

		public void ApplyForce(Vector2d force)
		{
			netForce += force;
		}

		public Vector2d GetAcceleration()
		{
			return netForce / massKg;
		}
	}
}
