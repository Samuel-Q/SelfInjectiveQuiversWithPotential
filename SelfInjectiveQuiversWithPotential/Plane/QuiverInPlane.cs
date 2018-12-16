using System;
using System.Collections.Generic;
using DataStructures;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Plane
{
    // Thoughts:
    // It might have been better to use an IVertexInPlane interface with properties TVertex Vertex and Point PositionInPlane.
    // Then QuiverInPlane would just have been a MutableQuiver<TVertex> where TVertex : IVertexInPlane.
    // The benefits with this would be twofold:
    //   1. Less code duplication/boilerplate code (this class)
    //   2. Being able to access vertex position by just vertex.Position is nicer than having to go through the quiver
    //      as in quiverInPlane.GetVertexPosition(vertex)
    //
    // Admittedly, accessing the vertex (TVertex) would be smelly: vertex.Vertex.
    //
    // For the undirected version, I decided to use IVertexInPlane instead.
    // Good to experiment and see if it seems to work out, but awful to mix the conventions:
    // note in particular that 
    //

    /// <summary>
    /// This class represents a quiver embedded (in some informal sense) in the plane.
    /// </summary>
    /// <remarks>
    /// <para>Do not confuse a quiver in the plane with a <em>plane quiver</em>, which is a quiver
    /// in the plane with the additional assumption that no arrows intersect.</para>
    /// <para>This class is mutable.</para>
    /// </remarks>
    public class QuiverInPlane<TVertex> where TVertex : IEquatable<TVertex>, IComparable<TVertex>
    {
        /// <summary>
        /// The underlying quiver.
        /// </summary>
        private readonly MutableQuiver<TVertex> quiver;

        public Quiver<TVertex> GetUnderlyingQuiver()
        {
            return quiver.Freeze();
        }

        public IReadOnlyUndirectedGraph<VertexInPlane<TVertex>> GetUnderlyingGraph()
        {
            var vertices = Vertices.Select(vertex => VertexInPlanify(vertex));
            var edges = new HashSet<Edge<VertexInPlane<TVertex>>>(GetArrows().SelectMany(arrow => new Edge<VertexInPlane<TVertex>>[]
            {
                new Edge<VertexInPlane<TVertex>>(VertexInPlanify(arrow.Source), VertexInPlanify(arrow.Target)),
                new Edge<VertexInPlane<TVertex>>(VertexInPlanify(arrow.Target), VertexInPlanify(arrow.Source))
            }));

            return new ImmutableUndirectedGraph<VertexInPlane<TVertex>>(vertices, edges);

            VertexInPlane<TVertex> VertexInPlanify(TVertex vertex)
            {
                return new VertexInPlane<TVertex>(vertex, GetVertexPosition(vertex));
            }
        }

        /// <summary>
        /// A dictionary mapping a vertex to its position in the plane
        /// </summary>
        private readonly IDictionary<TVertex, Point> vertexPositions;

        /// <summary>
        /// Gets the vertices of the quiver.
        /// </summary>
        /// <remarks>Do not modify the returned set; its type would be
        /// &quot;IReadOnlySet{TVertex}&quot; if such a type existed.</remarks>
        public ISet<TVertex> Vertices => quiver.Vertices;

        /// <summary>
        /// Gets the adjacency lists for the quiver.
        /// </summary>
        /// <remarks>Do not modify the adjacency lists; the return type would be
        /// &quot;IReadOnlySet{TVertex}&quot; if such a type existed.</remarks>
        public IReadOnlyDictionary<TVertex, ISet<TVertex>> AdjacencyLists { get => quiver.AdjacencyLists; }

        public IEnumerable<Arrow<TVertex>> GetArrows()
        {
            return quiver.GetArrows();
        }

        public IEnumerable<Arrow<TVertex>> GetArrowsInvolvingVertex(TVertex vertex)
        {
            return quiver.GetArrowsInvolvingVertex(vertex);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuiverInPlane{TVertex}"/> class to
        /// represent the empty quiver.
        /// </summary>
        public QuiverInPlane() : this(new MutableQuiver<TVertex>(), new Dictionary<TVertex, Point>()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuiverInPlane{TVertex}"/> class.
        /// </summary>
        /// <param name="vertices">The vertices of the quiver.</param>
        /// <param name="arrows">The arrows of the quiver.</param>
        /// <param name="vertexPositions">A dictionary specifying the positions in the plane of every vertex in the quiver.</param>
        /// <exception cref="ArgumentNullException"><paramref name="vertices"/> is <see langword="null"/>,
        /// or <paramref name="arrows"/> is <see langword="null"/>,
        /// or <paramref name="vertexPositions"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="vertices"/> contains a duplicate,
        /// <paramref name="arrows"/> contains a duplicate, or <paramref name="arrows"/> contains
        /// an arrow with source or target vertex not in <paramref name="vertices"/>.</exception>
        public QuiverInPlane(IEnumerable<TVertex> vertices, IEnumerable<Arrow<TVertex>> arrows, IDictionary<TVertex, Point> vertexPositions)
            : this(new MutableQuiver<TVertex>(vertices, arrows), vertexPositions) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuiverInPlane{TVertex}"/> class.
        /// </summary>
        /// <param name="quiver">The quiver (without information about the vertex positions).</param>
        /// <param name="vertexPositions">A dictionary specifying the positions in the plane of every vertex in the quiver.</param>
        /// <exception cref="ArgumentNullException"><paramref name="quiver"/> is <see langword="null"/> or
        /// <paramref name="vertexPositions"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">The vertices in <paramref name="quiver"/> and <paramref name="vertexPositions"/>
        /// are not equal as unordered collections.</exception>
        public QuiverInPlane(MutableQuiver<TVertex> quiver, IDictionary<TVertex, Point> vertexPositions)
        {
            this.quiver = quiver ?? throw new ArgumentNullException(nameof(quiver));
            this.vertexPositions = vertexPositions ?? throw new ArgumentNullException(nameof(vertexPositions));

            if (!quiver.Vertices.EqualUpToOrder(vertexPositions.Keys))
            {
                throw new ArgumentException("The collections of vertices in the specified quiver and vertices in the map from vertices to positions are not equal.", nameof(vertexPositions));
            }
        }

        public QuiverInPlane(Quiver<TVertex> quiver, IDictionary<TVertex, Point> vertexPositions) :
            this(new MutableQuiver<TVertex>(quiver ?? throw new ArgumentNullException(nameof(quiver))), vertexPositions)
        { }

        public QuiverInPlane(QuiverInPlane<TVertex> quiverInPlane)
            : this(quiverInPlane is null ? throw new ArgumentNullException(nameof(quiverInPlane)) :
                  quiverInPlane.Vertices.ToList(),
                  quiverInPlane.GetArrows(),
                  quiverInPlane.vertexPositions.ToDictionary(p => p.Key, p => p.Value))
        { }

        public QuiverInPlane<TVertex> Copy()
        {
            return new QuiverInPlane<TVertex>(this);
        }

        public bool ContainsVertex(TVertex vertex) => quiver.ContainsVertex(vertex);

        public bool ContainsArrow(Arrow<TVertex> arrow)
        {
            if (arrow is null) throw new ArgumentNullException(nameof(arrow));
            return quiver.ContainsArrow(arrow);
        }

        // Seems debatable whether the method should throw on source/target vertex not in the quiver. Not throwing for now.
        public bool ContainsArrow(TVertex source, TVertex target)
        {
            return quiver.ContainsArrow(source, target);
        }

        public Point GetVertexPosition(TVertex vertex)
        {
            if (!quiver.ContainsVertex(vertex)) throw new ArgumentException($"The vertex {vertex} is not contained in the quiver.", nameof(vertex));

            return vertexPositions[vertex];
        }

        public void SetVertexPosition(TVertex vertex, Point position)
        {
            if (!quiver.ContainsVertex(vertex)) throw new ArgumentException($"The vertex {vertex} is not contained in the quiver.", nameof(vertex));

            vertexPositions[vertex] = position;
        }

        public (Point, Point) GetArrowEndpointPositions(Arrow<TVertex> arrow)
        {
            if (arrow is null) throw new ArgumentNullException(nameof(arrow));
            if (!quiver.ContainsArrow(arrow)) throw new ArgumentException($"The arrow {arrow} is not contained in the quiver.", nameof(arrow));

            return (vertexPositions[arrow.Source], vertexPositions[arrow.Target]);
        }

        public void AddVertex(TVertex vertex, Point position)
        {
            if (quiver.ContainsVertex(vertex)) throw new ArgumentException($"The quiver already contains the vertex {vertex}.", nameof(vertex));

            quiver.AddVertex(vertex);
            vertexPositions.Add(vertex, position);
        }

        public void RemoveVertex(TVertex vertex, out IEnumerable<Arrow<TVertex>> arrowsRemoved)
        {
            if (!quiver.ContainsVertex(vertex)) throw new ArgumentException($"The vertex {vertex} is not contained in the quiver.");

            quiver.RemoveVertex(vertex, out arrowsRemoved);
            vertexPositions.Remove(vertex);
        }

        public void AddArrow(Arrow<TVertex> arrow)
        {
            if (arrow is null) throw new ArgumentNullException(nameof(arrow));
            AddArrow(arrow.Source, arrow.Target);
        }

        public void AddArrow(TVertex source, TVertex target)
        {
            if (!quiver.ContainsVertex(source)) throw new ArgumentException($"The source vertex {source} is not contained in the quiver.", nameof(source));
            if (!quiver.ContainsVertex(target)) throw new ArgumentException($"The target vertex {target} is not contained in the quiver.", nameof(target));

            if (quiver.ContainsArrow(source, target)) throw new ArgumentException($"The arrow from {source} to {target} already exists in the quiver.");

            quiver.AddArrow(source, target);
        }

        public void RemoveArrow(Arrow<TVertex> arrow)
        {
            if (arrow is null) throw new ArgumentNullException(nameof(arrow));
            RemoveArrow(arrow.Source, arrow.Target);
        }

        public void RemoveArrow(TVertex source, TVertex target)
        {
            if (!quiver.ContainsVertex(source)) throw new ArgumentException($"The source vertex {source} is not contained in the quiver.", nameof(source));
            if (!quiver.ContainsVertex(target)) throw new ArgumentException($"The target vertex {target} is not contained in the quiver.", nameof(target));

            if (!quiver.ContainsArrow(source, target)) throw new ArgumentException($"The arrow from {source} to {target} is not contained in the quiver.");

            quiver.RemoveArrow(source, target);
        }

        public bool HasLoops()
        {
            return Vertices.Any(vertex => AdjacencyLists[vertex].Contains(vertex));
        }

        public bool HasAntiParallelArrows()
        {
            return Utility.CartesianProduct(Vertices, Vertices).Where(pair => !pair.Item1.Equals(pair.Item2))
                                                               .Any(pair =>
                                                               {
                                                                   var vertex1 = pair.Item1;
                                                                   var vertex2 = pair.Item2;
                                                                   return AdjacencyLists[vertex1].Contains(vertex2) && AdjacencyLists[vertex2].Contains(vertex1);
                                                               });
        }

        /// <summary>
        /// Returns a boolean value indicating whether the quiver is plane, i.e., whether the
        /// arrows (drawn as straight lines) are pairwise non-intersecting.
        /// </summary>
        /// <returns>A boolean value indicating whether the quiver is plane.</returns>
        public bool IsPlane()
        {
            // Remark: Deconstructing lambda arguments does not seem to be possible as of this writing. In other words,
            // the following would not work:
            //     ... .Where( ((a1, a2)) => a1 != a2 )
            foreach (var (arrow1, arrow2) in Utility.CartesianProduct(GetArrows(), GetArrows()).Where(p => p.Item1 != p.Item2))
            {
                var lineSegment1 = new OrientedLineSegment(GetVertexPosition(arrow1.Source), GetVertexPosition(arrow1.Target));
                var lineSegment2 = new OrientedLineSegment(GetVertexPosition(arrow2.Source), GetVertexPosition(arrow2.Target));
                if (lineSegment1.IntersectsProperly(lineSegment2)) return false;
            }

            return true;
        }

        // Sort of bad to introduce a dependency on the Path class, but this works.
        // The option to have this logic in QPExtractor is worse imo (does not promote code reuse and is hard to test,
        // because QPExtractor would not expose the method).
        public Orientation GetOrientationOfCycle(Path<TVertex> closedPath)
        {
            if (!closedPath.IsClosed) throw new ArgumentException($"The path {closedPath} is not closed.", nameof(closedPath));
            if (closedPath.Length == 0) throw new ArgumentException($"The path {closedPath} is stationary.");

            var pathAsCircularListOfVertices = new CircularList<TVertex>(closedPath.Vertices.SkipLast(1));

            // Sum the external angle at every vertex
            double externalAngleSum = 0;
            for (int i = 0; i < pathAsCircularListOfVertices.Count; i++)
            {
                var vertex1 = pathAsCircularListOfVertices[i - 1];
                var vertex2 = pathAsCircularListOfVertices[i];
                var vertex3 = pathAsCircularListOfVertices[i + 1];

                var pos1 = GetVertexPosition(vertex1);
                var pos2 = GetVertexPosition(vertex2);
                var pos3 = GetVertexPosition(vertex3);

                externalAngleSum += PlaneUtility.GetExternalAngle(pos1, pos2, pos3);
            }

            const double Tolerance = 0.01;

            if (Math.Abs(externalAngleSum - 2 * Math.PI) < Tolerance) return Orientation.Counterclockwise;
            else if (Math.Abs(externalAngleSum + 2 * Math.PI) < Tolerance) return Orientation.Clockwise;
            else throw new OrientationException($"Failed to determine the orientation of {closedPath}; external angle sum was {externalAngleSum}.");
        }

        public override bool Equals(object obj)
        {
            return obj is QuiverInPlane<TVertex> otherQuiverInPlane &&
                   EqualityComparer<MutableQuiver<TVertex>>.Default.Equals(quiver, otherQuiverInPlane.quiver) &&
                   vertexPositions.ToSet().SetEquals(otherQuiverInPlane.vertexPositions);
        }
    }
}
