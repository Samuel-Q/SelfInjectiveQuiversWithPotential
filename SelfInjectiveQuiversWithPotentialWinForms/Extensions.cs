using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    // TODO: Much of this logic is as of this writing duplicated in
    // SelfInjectiveQuiversWithPotential.Plane for SelfInjectiveQuiversWithPotential.Plane.Point
    // in place of System.Drawing.Point

    public static class Extensions
    {
        private static Point ZeroVector = new Point(0, 0);

        public static Point ScaleBy(this Point @this, int scaleFactor)
        {
            return new Point(scaleFactor * @this.X, scaleFactor * @this.Y);
        }

        public static Point Minus(this Point @this, Point otherPoint)
        {
            return new Point(@this.X - otherPoint.X, @this.Y - otherPoint.Y);
        }

        public static int DistanceSquared(this Point @this, Point otherPoint)
        {
            return (@this.X - otherPoint.X) * (@this.X - otherPoint.X) + (@this.Y - otherPoint.Y) * (@this.Y - otherPoint.Y);
        }

        public static double Distance(this Point @this, Point otherPoint)
        {
            return Math.Sqrt(@this.DistanceSquared(otherPoint));
        }

        public static double Norm(this Point @this)
        {
            return @this.Distance(ZeroVector);
        }

        public static Point RotateBy90DegreesCounterclockwise(this Point @this)
        {
            return new Point(@this.Y, -@this.X);
        }

        // Endpoints included
        public static bool IsOnLineSegment(this Point @this, Point lineSegmentEnd1, Point lineSegmentEnd2)
        {
            // Translate the point and the line so that the the line is origin-based
            Point point = @this.Minus(lineSegmentEnd1);
            Point lineSegmentEnd = lineSegmentEnd2.Minus(lineSegmentEnd1);
            if (lineSegmentEnd.X != 0)
            {
                return lineSegmentEnd2.ScaleBy(point.X) == point.ScaleBy(lineSegmentEnd2.X);
            }
            else if (lineSegmentEnd.Y != 0)
            {
                return lineSegmentEnd2.ScaleBy(point.Y) == point.ScaleBy(lineSegmentEnd2.Y);
            }
            else
            {
                return point == ZeroVector;
            }
        }

        public static double DistanceToLineSegment(this Point @this, Point lineSegmentEnd1, Point lineSegmentEnd2)
        {
            if (lineSegmentEnd1 == lineSegmentEnd2) return @this.Distance(lineSegmentEnd1);

            // Translate the point and the line so that the the line is origin-based
            Point point = @this.Minus(lineSegmentEnd1);
            Point lineSegmentEnd = lineSegmentEnd2.Minus(lineSegmentEnd1);

            // Compute the distance to the corresponding *line* and the orthogonal projection of the point onto the line.
            // If the projection is on the line segment, then the distance to the line segment *is* the distance to the line.
            // If the projection is not on the line, then the shortest distance to the line segment is the distance to one
            // of the endpoints of the line segment.

            // Express the point in the basis (lineSegmentEnd, rotate(lineSegmentEnd)), where lineSegmentEnd is viewed as a vector:
            //     point = a*lineSegmentEnd + b*rotate(lineSegmentEnd)

            // The projection is then just the first term
            //     a*lineSegmentEnd
            // while the distance to the line is just the norm of the second term
            //     norm(b*rotate(lineSegmentEnd))

            // Get the coordinates in this new orthogonal basis ("projection basis", say) by tinkering with change-of-basis matrices
            // More precisely, multiply the standard-to-projection matrix and the standard coordinates to get the projection coordinates.

            Point v1 = lineSegmentEnd;
            Point v2 = lineSegmentEnd.RotateBy90DegreesCounterclockwise();
            var projectionBasisToStandardBasisMatrix = new double[2, 2]
            {
                { v1.X, v2.X },
                { v1.Y, v2.Y }
            };

            var det = v1.X * v2.Y - v2.X * v1.Y;
            double detInverse = 1 / ((double)det);
            var standardBasisToProjectionBasisMatrix = new double[2, 2]
            {
                { detInverse*v2.Y, -detInverse*v2.X },
                { -detInverse*v1.Y, detInverse*v1.X }
            };

            double a = standardBasisToProjectionBasisMatrix[0, 0] * point.X + standardBasisToProjectionBasisMatrix[1, 0] * point.Y;
            double b = standardBasisToProjectionBasisMatrix[0, 1] * point.X + standardBasisToProjectionBasisMatrix[1, 1] * point.Y;

            if (0 <= a && a <= 1) // Projection point is on the line segment
            {
                return Math.Abs(b) * v2.Norm();
            }
            else
            {
                // Return the minimum of the distances to the endpoints
                return Math.Min(point.Distance(ZeroVector), point.Distance(lineSegmentEnd));
            }
        }

        public static (double X, double Y) ScaleOriginBasedVectorToNorm(this (double X, double Y) vector, double outputNorm)
        {
            if (vector.X == 0.0 && vector.Y == 0)
            {
                if (outputNorm == 0.0) return vector;
                else throw new ArgumentException($"The input vector is the zero vector.");
            }

            var inputNorm = Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            var scaleFactor = outputNorm / inputNorm;
            return (vector.X * scaleFactor, vector.Y * scaleFactor);
        }

        public static (double X, double Y) RotateOriginBasedVectorCounterclockwise(this (double X, double Y) vector, double angle)
        {
            return (Math.Cos(angle) * vector.X + Math.Sin(angle) * vector.Y, -Math.Sin(angle) * vector.X + Math.Cos(angle) * vector.Y);
        }

        public static void DrawArrow(this Graphics @this, Pen pen, float x1, float y1, float x2, float y2)
        {
            if ((x1, y1) == (x2, y2)) throw new ArgumentException($"The source {(x1, y1)} and the target {(x2, y2)} are equal.");

            @this.DrawLine(pen, x1, y1, x2, y2);

            const float ArrowTipPartNorm = 5.0f;
            (double, double) originVect = (x2 - x1, y2 - y1);
            (float X, float Y) originArrowTipPart1 = ((float, float))originVect.RotateOriginBasedVectorCounterclockwise(Math.PI / 4 + Math.PI).ScaleOriginBasedVectorToNorm(ArrowTipPartNorm);
            (float X, float Y) originArrowTipPart2 = ((float, float))originVect.RotateOriginBasedVectorCounterclockwise(-Math.PI / 4 + Math.PI).ScaleOriginBasedVectorToNorm(ArrowTipPartNorm);

            @this.DrawLine(pen, x2, y2, x2 + originArrowTipPart1.X, y2 + originArrowTipPart1.Y);
            @this.DrawLine(pen, x2, y2, x2 + originArrowTipPart2.X, y2 + originArrowTipPart2.Y);
        }
    }
}
