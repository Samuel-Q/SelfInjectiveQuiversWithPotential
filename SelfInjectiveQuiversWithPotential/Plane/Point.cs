using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Plane
{
    /// <summary>
    /// This struct represents a point in the plane.
    /// </summary>
    public struct Point
    {
        public static readonly Point Origin = new Point(0, 0);

        public int X;
        public int Y;

        public double Angle => Math.Atan2(Y, X);
        public double Radius => Math.Sqrt(X * X + Y * Y);

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point"/> class.
        /// </summary>
        /// <param name="x">The x-coordinate.</param>
        /// <param name="y">The y-coordinate.</param>
        /// <exception cref="OverflowException"><paramref name="x"/> is greater than
        /// <see cref="int.MaxValue"/> or less than <see cref="int.MinValue"/>,
        /// or <paramref name="y"/> is greater than <see cref="int.MaxValue"/> or less than
        /// <see cref="int.MinValue"/>.</exception>
        public Point(double x, double y)
        {
            X = Convert.ToInt32(x);
            Y = Convert.ToInt32(y);
        }

        public static bool operator ==(Point point1, Point point2)
        {
            return point1.X == point2.X && point1.Y == point2.Y;
        }

        public static bool operator !=(Point point1, Point point2)
        {
            return point1.X != point2.X || point1.Y != point2.Y;
        }

        public static Point operator +(Point point1, Point point2)
        {
            return new Point(point1.X + point2.X, point1.Y + point2.Y);
        }

        public static Point operator -(Point point1, Point point2)
        {
            return new Point(point1.X - point2.X, point1.Y - point2.Y);
        }

        public static Point operator *(int scalar, Point point)
        {
            return new Point(scalar * point.X, scalar * point.Y);
        }

        public int SquareDistanceTo(Point otherPoint)
        {
            return (X - otherPoint.X) * (X - otherPoint.X) + (Y - otherPoint.Y) * (Y - otherPoint.Y);
        }

        /// <summary>
        /// Gets the quadrant to which the point belongs.
        /// </summary>
        /// <exception cref="InvalidOperationException">The point is <see cref="Origin"/>.</exception>
        /// <returns>The 1-based quadrant to which the point belongs.</returns>
        /// <remarks>
        /// <para>The quadrants are half-closed by convention in the sense the positive x-axis
        /// belongs to the first quadrant, the positive y-axis belongs to the second quadrant, the
        /// negative x-axis belongs the third quadrant, and the negative y-axis belongs to the
        /// fourth quadrant. In terms of polar coordinates, the lower endpoint of the angle is
        /// included and the upper endpoint of the angle is excluded.</para>
        /// </remarks>
        public int GetQuadrant()
        {
            if (this == Origin) throw new InvalidOperationException($"The point is the origin.");

            if (X > 0 && Y >= 0) return 1;
            else if (X <= 0 && Y > 0) return 2;
            else if (X < 0 && Y <= 0) return 3;
            else return 4;
        }

        public Point Rotate90DegreesCounterclockwiseAboutTheOrigin()
        {
            return new Point(-Y, X);
        }

        public Point Rotate90DegreesClockwiseAboutTheOrigin()
        {
            return new Point(Y, -X);
        }

        public Point MirrorInTheOrigin()
        {
            return new Point(-X, -Y);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Point))
            {
                return false;
            }

            var point = (Point)obj;
            return X == point.X &&
                   Y == point.Y;
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
    }
}
