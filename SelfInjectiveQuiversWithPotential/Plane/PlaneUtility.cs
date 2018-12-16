using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Plane
{
    public static class PlaneUtility
    {
        /// <summary>
        /// Gets the orientation of a triplet of points in the plane.
        /// </summary>
        /// <param name="p1">The first point.</param>
        /// <param name="p2">The second point.</param>
        /// <param name="p3">The third point.</param>
        /// <returns>The orientation of the triplet.</returns>
        /// <remarks>See <see href="https://www.geeksforgeeks.org/orientation-3-ordered-points/"/>
        /// for details that I have not worked out myself. Note that the orientation of the x-axis
        /// and the y-axis is the usual one in mathematics (not computer science) both in the
        /// reference and in this implementation, namely with the positive x-axis to the right and
        /// the positive y-axis upwards.</remarks>
        public static TripletOrientation GetOrientation(Point p1, Point p2, Point p3)
        {
            int val1 = (p2.Y - p1.Y) * (p3.X - p2.X);
            int val2 = (p3.Y - p2.Y) * (p2.X - p1.X);
            int cmpVal = val1.CompareTo(val2);
            if (cmpVal < 0) return TripletOrientation.Counterclockwise;
            else if (cmpVal == 0) return TripletOrientation.Collinear;
            else return TripletOrientation.Clockwise;
        }

        /// <summary>
        /// Gets the external angle for the directed line segments from <paramref name="p1"/> to <paramref name="p2"/>
        /// and from <paramref name="p2"/> to <paramref name="p3"/>.
        /// </summary>
        /// <param name="p1">The source of the first line segment.</param>
        /// <param name="p2">The target of the first line segment and the source of the second line segment.</param>
        /// <param name="p3">The target of the second line segment.</param>
        /// <returns>The external angle for the directed line segments.</returns>
        /// <remarks>The conventions used are as follows:
        /// <list type="bullet">
        /// <item><description>
        /// The external angle is between <c>-<see cref="Math.PI"/></c> (exclusive) and <c><see cref="Math.PI"/></c> (inclusive).
        /// </description></item>
        /// <item><description>
        /// The external angle is positive for turns &quot;to the left&quot; (counterclockwise) (and the degenerate turn by 180 degrees)
        /// and negative for turns &quot;to the right&quot; (clockwise).
        /// </description></item>
        /// </list>
        /// </remarks>
        public static double GetExternalAngle(Point p1, Point p2, Point p3)
        {
            var originBasedP1 = p1 - p2;
            var originBasedP3 = p3 - p2;

            var angle1 = originBasedP1.Angle;
            var angle2 = originBasedP3.Angle;

            var angle = angle2 - angle1;
            // Make sure that angle is in the range (-2*pi, 0]
            if (angle > 0) angle -= 2 * Math.PI;

            return Math.PI + angle;
        }
    }
}
