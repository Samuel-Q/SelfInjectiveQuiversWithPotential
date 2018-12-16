using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Plane
{
    public class OrientedLineSegment
    {
        public Point Start { get; }

        public Point End { get; }

        public OrientedLineSegment(Point start, Point end)
        {
            Start = start;
            End = end;
        }

        public static bool operator ==(OrientedLineSegment lineSegment1, OrientedLineSegment lineSegment2)
        {
            return lineSegment1.Start == lineSegment2.Start && lineSegment1.End == lineSegment2.End;
        }

        public static bool operator !=(OrientedLineSegment lineSegment1, OrientedLineSegment lineSegment2)
        {
            return lineSegment1.Start != lineSegment2.Start || lineSegment1.End != lineSegment2.End;
        }

        public void Deconstruct(out Point start, out Point end)
        {
            start = Start;
            end = End;
        }

        public OrientedLineSegment Reverse()
        {
            return new OrientedLineSegment(End, Start);
        }

        public bool IsEqualToAsUnorientedLineSegments(OrientedLineSegment otherLineSegment)
        {
            return this == otherLineSegment || this == otherLineSegment.Reverse();
        }

        public bool Intersects(OrientedLineSegment otherLineSegment)
        {
            if (otherLineSegment is null) throw new ArgumentNullException(nameof(otherLineSegment));

            return Intersect(this, otherLineSegment);
        }

        /// <summary>
        /// Determines whether two line segments intersect.
        /// </summary>
        /// <param name="lineSegment1">The first line segment.</param>
        /// <param name="lineSegment2">The second line segment.</param>
        /// <returns></returns>
        /// <remarks>
        /// <para>See <see href="https://www.cdn.geeksforgeeks.org/check-if-two-given-line-segments-intersect/"/>
        /// for the inner workings of this method (details which I have not worked out myself).</para></remarks>
        public static bool Intersect(OrientedLineSegment lineSegment1, OrientedLineSegment lineSegment2)
        {
            if (lineSegment1 is null) throw new ArgumentNullException(nameof(lineSegment1));
            if (lineSegment2 is null) throw new ArgumentNullException(nameof(lineSegment2));

            if (lineSegment1.Start == lineSegment1.End) throw new NotImplementedException(); // Haven't checked that the method works in this case
            if (lineSegment2.Start == lineSegment2.End) throw new NotImplementedException(); // Haven't checked that the method works in this case

            var ls1 = lineSegment1;
            var ls2 = lineSegment2;

            var o1 = PlaneUtility.GetOrientation(ls1.Start, ls1.End, ls2.Start);
            var o2 = PlaneUtility.GetOrientation(ls1.Start, ls1.End, ls2.End);
            var o3 = PlaneUtility.GetOrientation(ls2.Start, ls2.End, ls1.Start);
            var o4 = PlaneUtility.GetOrientation(ls2.Start, ls2.End, ls1.End);

            if (o1 != o2 && o3 != o4) return true;

            if (o1 == TripletOrientation.Collinear && LineSegmentContainsPoint(ls1, ls2.Start)) return true;
            if (o2 == TripletOrientation.Collinear && LineSegmentContainsPoint(ls1, ls2.End)) return true;
            if (o3 == TripletOrientation.Collinear && LineSegmentContainsPoint(ls2, ls1.Start)) return true;
            if (o4 == TripletOrientation.Collinear && LineSegmentContainsPoint(ls2, ls1.End)) return true;

            return false;

            // Idea (obsolete in favor of the above I guess):
            // Look at the common range of, say, the x-coordinates
            // If empty, then return false
            // If equal at either endpoint, return true (because they intersect in the endpoint)
            // If the first line segment is below/above the other in one endpoint and above/below the other in the other, return true
            // Else return false

            // The above doesn't work when one of the lines is vertical though

            // Assumes that p is on the line determined by ls (this assumes that ls is non-degenerate)
            bool LineSegmentContainsPoint(OrientedLineSegment ls, Point p)
            {
                return Math.Min(ls.Start.X, ls.End.X) <= p.X
                    && p.X <= Math.Max(ls.Start.X, ls.End.X)
                    && Math.Min(ls.Start.Y, ls.End.Y) <= p.Y
                    && p.Y <= Math.Max(ls.Start.Y, ls.End.Y);
            }
        }

        /// <summary>
        /// Determines whether the line segment intersects the specified line segment properly, in
        /// the sense that the interiors of the line segments intersect.
        /// </summary>
        /// <param name="otherLineSegment">The other line segment.</param>
        /// <returns>A boolean value indicating whether the line segments intersect properly.</returns>
        public bool IntersectsProperly(OrientedLineSegment otherLineSegment)
        {
            if (otherLineSegment is null) throw new ArgumentNullException(nameof(otherLineSegment));

            return IntersectProperly(this, otherLineSegment);
        }

        /// <summary>
        /// Determines whether two line segments intersect properly, in the sense that the
        /// interiors of the line segments intersect.
        /// </summary>
        /// <param name="lineSegment1">The first line segment.</param>
        /// <param name="lineSegment2">The second line segment.</param>
        /// <returns></returns>
        public static bool IntersectProperly(OrientedLineSegment lineSegment1, OrientedLineSegment lineSegment2)
        {
            if (lineSegment1 is null) throw new ArgumentNullException(nameof(lineSegment1));
            if (lineSegment2 is null) throw new ArgumentNullException(nameof(lineSegment2));

            if (lineSegment1.Start == lineSegment1.End) return false; // The interior of a degenerate line segment is empty
            if (lineSegment2.Start == lineSegment2.End) return false; // The interior of a degenerate line segment is empty

            var ls1 = lineSegment1;
            var ls2 = lineSegment2;

            var o1 = PlaneUtility.GetOrientation(ls1.Start, ls1.End, ls2.Start);
            var o2 = PlaneUtility.GetOrientation(ls1.Start, ls1.End, ls2.End);
            var o3 = PlaneUtility.GetOrientation(ls2.Start, ls2.End, ls1.Start);
            var o4 = PlaneUtility.GetOrientation(ls2.Start, ls2.End, ls1.End);

            // The line segments are collinear
            if (o1 == TripletOrientation.Collinear && o2 == TripletOrientation.Collinear)
            {
                return ls1.IsEqualToAsUnorientedLineSegments(ls2)
                    || LineSegmentInteriorContainsPoint(ls1, ls2.Start)
                    || LineSegmentInteriorContainsPoint(ls1, ls2.End)
                    || LineSegmentInteriorContainsPoint(ls2, ls1.Start)
                    || LineSegmentInteriorContainsPoint(ls2, ls1.End);
            }

            // The line segments are not collinear, so if any three points are collinear, the line segments just "touch"
            // each other, which does not constitute a proper intersection
            if (o1 == TripletOrientation.Collinear || o2 == TripletOrientation.Collinear || o3 == TripletOrientation.Collinear || o4 == TripletOrientation.Collinear)
                return false;

            // Then just do "the usual" check
            return (o1 != o2 && o3 != o4);

            // Assumes that p is on the line determined by ls (this assumes that ls is non-degenerate)
            bool LineSegmentContainsPoint(OrientedLineSegment ls, Point p)
            {
                return Math.Min(ls.Start.X, ls.End.X) <= p.X
                    && p.X <= Math.Max(ls.Start.X, ls.End.X)
                    && Math.Min(ls.Start.Y, ls.End.Y) <= p.Y
                    && p.Y <= Math.Max(ls.Start.Y, ls.End.Y);
            }

            // Assumes that p is on the line determined by ls (this assumes that ls is non-degenerate)
            bool LineSegmentInteriorContainsPoint(OrientedLineSegment ls, Point p)
            {
                return LineSegmentContainsPoint(ls, p) && p != ls.Start && p != ls.End;
            }
        }

        public override bool Equals(object obj)
        {
            var segment = obj as OrientedLineSegment;
            return segment != null &&
                   EqualityComparer<Point>.Default.Equals(Start, segment.Start) &&
                   EqualityComparer<Point>.Default.Equals(End, segment.End);
        }

        public override int GetHashCode()
        {
            var hashCode = -1676728671;
            hashCode = hashCode * -1521134295 + EqualityComparer<Point>.Default.GetHashCode(Start);
            hashCode = hashCode * -1521134295 + EqualityComparer<Point>.Default.GetHashCode(End);
            return hashCode;
        }

        public override string ToString()
        {
            return $"{Start} to {End}";
        }
    }
}
