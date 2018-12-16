using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures;

namespace SelfInjectiveQuiversWithPotential.Plane
{
    /// <summary>
    /// This class is used to find the faces of a plane undirected graph.
    /// </summary>
    public class FaceFinder
    {
        /// <summary>
        /// Finds the faces of the specified graph.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertices.</typeparam>
        /// <param name="graph">The graph whose faces to find.</param>
        /// <param name="boundingCycles">Output parameter for the collection of faces, each
        /// represented by a bounding cycle oriented counterclockwise.</param>
        /// <returns><see langword="true"/> if the search was successful (or equivalently, the
        /// graph is plane). <see langword="false"/> if the search failed (or equivalently, the
        /// graph is not plane).</returns>
        public bool TryFindFaces<TVertex>(IReadOnlyUndirectedGraph<TVertex> graph, out IEnumerable<IEnumerable<TVertex>> boundingCycles)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>, IVertexInPlane
        {
            if (graph is null) throw new ArgumentNullException(nameof(graph));

            if (!graph.IsPlane())
            {
                boundingCycles = null;
                return false;
            }

            boundingCycles = SearchForFaces(graph, Orientation.Counterclockwise);
            return true;
        }

        private IEnumerable<IEnumerable<TVertex>> SearchForFaces<TVertex>(IReadOnlyUndirectedGraph<TVertex> graph, Orientation searchOrientation)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>, IVertexInPlane
        {
            // Algorithm (outline):
            // Keep track of traversed edges *and the direction in which the edge was traversed*
            // (i.e., keep track of traversed directed edges).
            //
            // For every directed edge (not already traversed), make a search:
            //   Keep track of the path of edges traversed.
            //   Start by traversing that edge.
            //   Keep turning "maximally" in the direction given by the search orientation until a vertex is visited twice.
            //   This gives a full path that decomposes into a path and a cycle.
            //   The path always consists of only boundary directed edges (boundary w.r.t. the search orientation),
            //   while the cycle bounds a (bounded) face precisely when its orientation agrees with the search orientation.

            // Remark: Sort of bad to use Arrow here (writing a DirectedEdge class with a common interface would be a proper solution),
            // but it allows so much code to be reused.

            var cycles = new HashSet<DetachedCycle<TVertex>>();
            var directedEdges = graph.Edges.SelectMany(e => new Arrow<TVertex>[] { new Arrow<TVertex>(e.Vertex1, e.Vertex2), new Arrow<TVertex>(e.Vertex2, e.Vertex1) });
            var remainingEdges = new HashSet<Arrow<TVertex>>(directedEdges);
            var remainingEdgesStack = new Stack<Arrow<TVertex>>(directedEdges);
            while (remainingEdges.Count > 0)
            {
                Arrow<TVertex> startEdge;
                while (!remainingEdges.Contains(startEdge = remainingEdgesStack.Pop())) ;

                var path = new Path<TVertex>(startEdge.Source, startEdge.Target);

                var prevVertex = startEdge.Source;
                var curVertex = startEdge.Target;

                // Begin the search for this start edge!
                while (true)
                {
                    // Terminate the search if we have found a cycle
                    if (path.TryExtractTrailingCycle(out var closedPath, out int startIndex))
                    {
                        var cycleOrientation = graph.GetOrientationOfCycle(closedPath.Vertices);
                        // If the orientations agree, then the cycle is a bounding cycle for a (bounded) face
                        // Else, it bounds the unbounded face (which we do not care about)
                        if (cycleOrientation == searchOrientation)
                        {
                            var detachedCycle = new DetachedCycle<TVertex>(closedPath);

                            // When restarting with a boundary arrow (directed edge), we might get the same cycle again
                            if (!cycles.Contains(detachedCycle)) cycles.Add(new DetachedCycle<TVertex>(closedPath));
                        }

                        foreach (var edge in path.Arrows) remainingEdges.Remove(edge);
                        break;
                    }

                    // Get next successor (next in the angle sense)
                    // If no successor, terminate the search (all arrows are direction-boundary arrows)
                    // Else, update path, prevVertex and curVertex and do next iteration

                    var neighbors = graph.AdjacencyLists[curVertex]; // Note that this includes prevVertex
                    if (neighbors.Count == 1) // The last edge was a dead end!
                    {
                        foreach (var arrow in path.Arrows) remainingEdges.Remove(arrow);
                        break;
                    }

                    var baseVertex = curVertex;
                    var basePos = baseVertex.Position;

                    // Sort the vertices by angle so that the vertex following the predecessor vertex (prevVertex) is
                    // the vertex corresponding to a "maximal" turn.
                    var neighborsSortedByAngle = searchOrientation == Orientation.Clockwise ?
                        neighbors.OrderBy(vertex => vertex.Position, new AngleBasedPointComparer(basePos)) :
                        neighbors.OrderByDescending(vertex => vertex.Position, new AngleBasedPointComparer(basePos));

                    var neighborsSortedByAngleList = new CircularList<TVertex>(neighborsSortedByAngle);
                    int predecessorIndex = neighborsSortedByAngleList.IndexOf(prevVertex);
                    neighborsSortedByAngleList.RotateLeft(predecessorIndex);

                    var nextVertex = neighborsSortedByAngleList.Skip(1).First();

                    path = path.AppendVertex(nextVertex);
                    prevVertex = curVertex;
                    curVertex = nextVertex;
                }
            }

            return cycles.Select(cycle => cycle.CanonicalPath.Vertices);
        }
    }
}
