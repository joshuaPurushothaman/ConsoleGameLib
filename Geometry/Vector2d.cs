namespace ConsoleGameLib.Geometry
{
	public class Vector2d
	{
		public double x, y;
		public double Magnitude
		{
			get { return x * x + y * y; }
			set
			{
				x = value * Math.Cos(AngleRadians);
				y = value * Math.Sin(AngleRadians);
			}
		}
		public double AngleRadians
		{
			get { return Math.Atan2(y, x); }
			set
			{
				x = Magnitude * Math.Cos(value);
				y = Magnitude * Math.Sin(value);
			}
		}
		public Vector2d(double x = 0.0, double y = 0.0)
		{
			this.x = x;
			this.y = y;
		}

		public static Vector2d FromMagAndAngle(double magnitude, double angleRadians)
		{
			var vec = new Vector2d
			{
				Magnitude = magnitude,
				AngleRadians = angleRadians
			};

			return vec;
		}
		
		public static double DegreesToRadians(double degrees)
		{
			return degrees * Math.PI / 180;
		}

		public static double RadiansToDegrees(double radians)
		{
			return radians * 180 / Math.PI;
		}

		public void SetComponents(double x, double y)
		{
			this.x = x;
			this.y = y;
		}

		public double GetAngleDegrees()
		{
			return RadiansToDegrees(AngleRadians);
		}

		public double Dot(Vector2d other)
		{
			return Magnitude * other.Magnitude * Math.Cos(Math.Abs(AngleRadians - other.AngleRadians));
		}

		public bool AlmostEquals(Vector2d? other, double epsilonX, double epsilonY)
		{
			if (ReferenceEquals(this, other))
			{
				return true;
			}

			if (other is null)
			{
				return false;
			}

			return Math.Abs(x - other.x) <= epsilonX && Math.Abs(y - other.y) <= epsilonY;
		}
		public bool AlmostEquals(Vector2d? other, double epsilon)
		{
			return AlmostEquals(other, epsilon, epsilon);
		}

		public bool AlmostEqualsMagAndAngle(Vector2d? other, double epsilonMag, double epsilonAngle)
		{
			if (ReferenceEquals(this, other))
			{
				return true;
			}

			if (other is null)
			{
				return false;
			}

			return Math.Abs(Magnitude - other.Magnitude) <= epsilonMag && 
				Math.Abs(AngleRadians - other.AngleRadians) <= epsilonAngle;
		}


		public static Vector2d operator +(Vector2d first, Vector2d second)
		{
			return new Vector2d(first.x + second.x, first.y + second.y);
		}

		public static Vector2d operator -(Vector2d first, Vector2d second)
		{
			return new Vector2d(first.x - second.x, first.y - second.y);
		}

		public static Vector2d operator *(Vector2d vec, double value)
		{
			return new Vector2d(vec.x * value, vec.y * value);
		}

		public static Vector2d operator /(Vector2d vec, double value)
		{
			return new Vector2d(vec.x / value, vec.y / value);
		}

		public static bool operator ==(Vector2d first, Vector2d second)
		{
			return first.x == second.x && first.y == second.y;
		}

		public static bool operator !=(Vector2d first, Vector2d second)
		{
			return !(first == second);
		}

		public static bool operator <(Vector2d first, Vector2d second)
		{
			return first.x < second.x && first.y < second.y;
		}

		public static bool operator <=(Vector2d first, Vector2d second)
		{
			return first.x <= second.x && first.y <= second.y;
		}

		public static bool operator >(Vector2d first, Vector2d second)
		{
			return first.x > second.x && first.y > second.y;
		}

		public static bool operator >=(Vector2d first, Vector2d second)
		{
			return first.x >= second.x && first.y >= second.y;
		}

		public override bool Equals(object? obj)
		{
			return AlmostEquals(obj as Vector2d, 0);
		}

		public override int GetHashCode()
		{
			return (int) (x * y);
		}
	}
}
