using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Plane
{
    // This class having to implement IComparable smells pretty badly; the requirement of
    // IComparable is spreading like wildfire!

    public class VertexInPlane<TVertex> :
        IEquatable<VertexInPlane<TVertex>>, IComparable<VertexInPlane<TVertex>>, IVertexInPlane
        where TVertex : IEquatable<TVertex>, IComparable<TVertex>
    {
        public TVertex Vertex { get; }

        public Point Position { get; }

        public VertexInPlane(TVertex vertex, Point position)
        {
            Vertex = vertex;
            Position = position;
        }

        public bool Equals(VertexInPlane<TVertex> other)
        {
            return Vertex.Equals(other.Vertex) && Position.Equals(other.Position);
        }

        public int CompareTo(VertexInPlane<TVertex> other)
        {
            int cmpVal = Vertex.CompareTo(other.Vertex);
            if (cmpVal != 0) return cmpVal;

            cmpVal = Position.X.CompareTo(other.Position.X);
            if (cmpVal != 0) return cmpVal;

            cmpVal = Position.Y.CompareTo(other.Position.Y);
            return cmpVal;
        }

        public override bool Equals(object obj)
        {
            if (obj is VertexInPlane<TVertex> otherVertex) return Equals(otherVertex);
            return false;
        }

        public override string ToString()
        {
            return $"{Vertex} at {Position}";
        }

        public override int GetHashCode()
        {
            var hashCode = -1793918199;
            hashCode = hashCode * -1521134295 + EqualityComparer<TVertex>.Default.GetHashCode(Vertex);
            hashCode = hashCode * -1521134295 + EqualityComparer<Point>.Default.GetHashCode(Position);
            return hashCode;
        }
    }
}
