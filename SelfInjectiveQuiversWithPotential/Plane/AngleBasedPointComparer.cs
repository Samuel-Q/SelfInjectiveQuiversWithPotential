using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Plane
{
    /// <summary>
    /// This class is used to compare points by angle (in [0, 2*pi)).
    /// </summary>
    public class AngleBasedPointComparer : IComparer<Point>
    {
        Point basePoint;

        public AngleBasedPointComparer() : this(Point.Origin) { }

        public AngleBasedPointComparer(Point point)
        {
            basePoint = point;
        }

        public int Compare(Point p, Point q)
        {
            p = p - basePoint;
            q = q - basePoint;

            var pQuadrant = p.GetQuadrant();
            var qQuadrant = q.GetQuadrant();

            var cmpVal = pQuadrant.CompareTo(qQuadrant);
            if (cmpVal != 0) return cmpVal;

            // Rotate p and q to the first quadrant
            switch (pQuadrant)
            {
                case 1: break;
                case 2:
                    p = p.Rotate90DegreesClockwiseAboutTheOrigin();
                    q = q.Rotate90DegreesClockwiseAboutTheOrigin();
                    break;
                case 3:
                    p = p.MirrorInTheOrigin();
                    q = q.MirrorInTheOrigin();
                    break;
                case 4:
                    p = p.Rotate90DegreesCounterclockwiseAboutTheOrigin();
                    q = q.Rotate90DegreesCounterclockwiseAboutTheOrigin();
                    break;
            }

            // In the first quadrant, the x-coordinate is strictly positive, so "cross-multiply" to make the x-coordinates equal
            // Then just compare the y-coordinates
            var scaledP = q.X * p;
            var scaledQ = p.X * q;

            return scaledP.Y.CompareTo(scaledQ.Y);
        }
    }
}
