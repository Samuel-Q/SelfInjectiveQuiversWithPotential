using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential
{
    /// <summary>
    /// This class represents an edge in an undirected graph.
    /// </summary>
    public class Edge<TVertex> where TVertex : IEquatable<TVertex>
    {
        public TVertex Vertex1 { get; }
        public TVertex Vertex2 { get; }

        public Edge(TVertex vertex1, TVertex vertex2)
        {
            Vertex1 = vertex1;
            Vertex2 = vertex2;
        }

        public void Deconstruct(out TVertex vertex1, out TVertex vertex2)
        {
            vertex1 = Vertex1;
            vertex2 = Vertex2;
        }

        public static bool operator ==(Edge<TVertex> edge1, Edge<TVertex> edge2)
        {
            return (edge1.Vertex1.Equals(edge2.Vertex1) && edge1.Vertex2.Equals(edge2.Vertex2))
                || (edge1.Vertex1.Equals(edge2.Vertex2) && edge1.Vertex2.Equals(edge2.Vertex1));
        }

        public static bool operator !=(Edge<TVertex> edge1, Edge<TVertex> edge2)
        {
            return !(edge1 == edge2);
        }

        public bool Equals(Edge<TVertex> otherEdge)
        {
            return (Vertex1.Equals(otherEdge.Vertex1) && Vertex2.Equals(otherEdge.Vertex2))
                || (Vertex1.Equals(otherEdge.Vertex2) && Vertex2.Equals(otherEdge.Vertex1));
        }

        public override bool Equals(object obj)
        {
            if (obj is Edge<TVertex> otherEdge) return Equals(otherEdge);
            else return false;
        }

        public override int GetHashCode()
        {
            // Make sure that the output does not depend on the order of the vertices.
            // I guess addition is not the best commutative operation to apply here, but I think it works

            return GetHashCode(Vertex1, Vertex2) + GetHashCode(Vertex2, Vertex1);

            int GetHashCode(TVertex v1, TVertex v2)
            {
                var hashCode = 112200001;
                hashCode = hashCode * -1521134295 + EqualityComparer<TVertex>.Default.GetHashCode(v1);
                hashCode = hashCode * -1521134295 + EqualityComparer<TVertex>.Default.GetHashCode(v2);
                return hashCode;
            }
        }

        /// <inheritdoc/>
        /// <remarks>Note that equal edges may return different string representations.</remarks>
        public override string ToString()
        {
            return $"{{{Vertex1}, {Vertex2}}}";
        }
    }
}
