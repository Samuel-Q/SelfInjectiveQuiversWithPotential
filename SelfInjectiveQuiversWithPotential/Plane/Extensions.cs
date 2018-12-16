using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures;
using SelfInjectiveQuiversWithPotential;

namespace SelfInjectiveQuiversWithPotential.Plane
{
    public static class Extensions
    {
        /// <summary>
        /// Returns a boolean value indicating whether the quiver is plane, i.e., whether the
        /// arrows (drawn as straight lines) are pairwise non-intersecting.
        /// </summary>
        /// <returns>A boolean value indicating whether the quiver is plane.</returns>
        public static bool IsPlane<TVertex>(this IReadOnlyUndirectedGraph<TVertex> graph) where TVertex : IEquatable<TVertex>, IVertexInPlane
        {
            // Remark: Deconstructing lambda arguments does not seem to be possible as of this writing. In other words,
            // the following would not work:
            //     ... .Where( ((e1, e2)) => e1 != e2 )
            foreach (var (edge1, edge2) in Utility.CartesianProduct(graph.Edges, graph.Edges).Where(p => p.Item1 != p.Item2))
            {
                var lineSegment1 = new OrientedLineSegment(edge1.Vertex1.Position, edge1.Vertex2.Position);
                var lineSegment2 = new OrientedLineSegment(edge2.Vertex1.Position, edge2.Vertex2.Position);
                if (lineSegment1.IntersectsProperly(lineSegment2)) return false;
            }

            return true;
        }

        // Sort of bad to introduce a dependency on the Path class, but this works.
        // The option to have this logic in QPExtractor is worse imo (does not promote code reuse and is hard to test,
        // because QPExtractor would not expose the method).
        public static Orientation GetOrientationOfCycle<TVertex>(this IReadOnlyUndirectedGraph<TVertex> graph, IEnumerable<TVertex> closedPath)
            where TVertex : IEquatable<TVertex>, IVertexInPlane
        {
            if (graph is null) throw new ArgumentNullException(nameof(graph));
            if (closedPath is null) throw new ArgumentNullException(nameof(closedPath));

            if (!closedPath.First().Equals(closedPath.Last())) throw new ArgumentException($"The path is not closed.", nameof(closedPath));
            if (closedPath.Count() == 1) throw new ArgumentException($"The path is stationary.");

            var pathAsCircularListOfVertices = new CircularList<TVertex>(closedPath.SkipLast(1));

            // Sum the external angle at every vertex
            double externalAngleSum = 0;
            for (int i = 0; i < pathAsCircularListOfVertices.Count; i++)
            {
                var vertex1 = pathAsCircularListOfVertices[i - 1];
                var vertex2 = pathAsCircularListOfVertices[i];
                var vertex3 = pathAsCircularListOfVertices[i + 1];

                var pos1 = vertex1.Position;
                var pos2 = vertex2.Position;
                var pos3 = vertex3.Position;

                externalAngleSum += PlaneUtility.GetExternalAngle(pos1, pos2, pos3);
            }

            const double Tolerance = 0.01;

            if (Math.Abs(externalAngleSum - 2 * Math.PI) < Tolerance) return Orientation.Counterclockwise;
            else if (Math.Abs(externalAngleSum + 2 * Math.PI) < Tolerance) return Orientation.Clockwise;
            else throw new OrientationException($"Failed to determine the orientation of {closedPath}; external angle sum was {externalAngleSum}.");
        }
    }
}
