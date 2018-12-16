using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential
{
    /// <summary>
    /// This class represents a mutable quiver, i.e. a multigraph with directed arrows (and loops allowed),
    /// with the arrows specified with adjacency lists (which are really adjacency <em>sets</em>).
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertices of the quiver.</typeparam>
    /// <remarks>
    /// <para>This class does not support multiple arrows as of this writing.</para>
    /// </remarks>
    /// <seealso cref="Quiver{TVertex}"/>
    public class MutableQuiver<TVertex> where TVertex : IEquatable<TVertex>, IComparable<TVertex>
    {
        /// <summary>
        /// Gets the vertices of the quiver.
        /// </summary>
        /// <remarks>Do not modify the returned set; its type would be
        /// &quot;IReadOnlySet{TVertex}&quot; if such a type existed.</remarks>
        public ISet<TVertex> Vertices { get; }

        private Dictionary<TVertex, ISet<TVertex>> adjacencyLists;

        /// <summary>
        /// Gets the adjacency lists for the quiver.
        /// </summary>
        /// <remarks>Do not modify the adjacency lists (sets); the return type would be
        /// &quot;IReadOnlySet{TVertex}&quot; if such a type existed.</remarks>
        public IReadOnlyDictionary<TVertex, ISet<TVertex>> AdjacencyLists { get => adjacencyLists; }

        public IEnumerable<Arrow<TVertex>> GetArrows()
        {
            return AdjacencyLists.SelectMany(p => p.Value.Select(w => new Arrow<TVertex>(p.Key, w)));
        }

        public IEnumerable<Arrow<TVertex>> GetArrowsInvolvingVertex(TVertex vertex)
        {
            return GetArrows().Where(a => a.Source.Equals(vertex) || a.Target.Equals(vertex));
        }

        /// <summary>
        /// Returns an immutable copy of this quiver in its current state.
        /// </summary>
        /// <returns>An immutable copy of this quiver at the time of the method call.</returns>
        public Quiver<TVertex> Freeze()
        {
            var arrows = AdjacencyLists.SelectMany(pair => pair.Value.Select(w => new Arrow<TVertex>(pair.Key, w)));
            return new Quiver<TVertex>(Vertices, arrows);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MutableQuiver{TVertex}"/> class to
        /// represent an empty quiver.
        /// </summary>
        public MutableQuiver() : this(new TVertex[0], new Arrow<TVertex>[0]) { }

        public MutableQuiver(Quiver<TVertex> quiver) : this(quiver.Vertices, quiver.Arrows) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MutableQuiver{TVertex}"/> class.
        /// </summary>
        /// <param name="vertices">The vertices of the quiver.</param>
        /// <param name="arrows">The arrows of the quiver.</param>
        /// <exception cref="ArgumentNullException"><paramref name="vertices"/> is <see langword="null"/>,
        /// or <paramref name="arrows"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="vertices"/> contains a duplicate,
        /// <paramref name="arrows"/> contains a duplicate, or <paramref name="arrows"/> contains
        /// an arrow with source or target vertex not in <paramref name="vertices"/>.</exception>
        public MutableQuiver(IEnumerable<TVertex> vertices, IEnumerable<Arrow<TVertex>> arrows)
        {
            if (vertices == null) throw new ArgumentNullException(nameof(vertices));
            if (arrows == null) throw new ArgumentNullException(nameof(arrows));

            var verticesSet = new HashSet<TVertex>(vertices);
            if (verticesSet.Count != vertices.Count()) throw new ArgumentException("Vertex collection contains duplicates.", nameof(vertices));

            var arrowsSet = new HashSet<Arrow<TVertex>>(arrows);
            if (arrowsSet.Count != arrows.Count()) throw new ArgumentException("Arrow collection contains duplicates.", nameof(arrows));


            Vertices = verticesSet;
            adjacencyLists = ConstructAdjacencyListDictionary(verticesSet, arrows);
        }

        private Dictionary<TVertex, ISet<TVertex>> ConstructAdjacencyListDictionary(ISet<TVertex> vertices, IEnumerable<Arrow<TVertex>> arrows)
        {
            var tempDict = new Dictionary<TVertex, ISet<TVertex>>();
            foreach (var vertex in vertices)
            {
                tempDict[vertex] = new HashSet<TVertex>();
            }

            foreach (var arrow in arrows)
            {
                if (!(vertices.Contains(arrow.Source) && vertices.Contains(arrow.Target)))
                    throw new ArgumentException($"The arrow {arrow} has an endpoint not present in the vertex collection.");

                tempDict[arrow.Source].Add(arrow.Target);
            }

            var returnDict = new Dictionary<TVertex, ISet<TVertex>>();
            foreach (var arrow in tempDict.Keys)
            {
                returnDict[arrow] = tempDict[arrow];
            }

            return returnDict;
        }

        public bool ContainsVertex(TVertex vertex)
        {
            return Vertices.Contains(vertex);
        }

        public bool ContainsArrow(Arrow<TVertex> arrow)
        {
            if (arrow is null) throw new ArgumentNullException(nameof(arrow));
            return ContainsArrow(arrow.Source, arrow.Target);
        }

        // Seems debatable whether the method should throw on source/target vertex not in the quiver. Not throwing for now.
        public bool ContainsArrow(TVertex source, TVertex target)
        {
            if (!Vertices.Contains(source)) return false;

            return AdjacencyLists[source].Contains(target);
        }

        public void AddVertex(TVertex vertex)
        {
            if (Vertices.Contains(vertex)) throw new ArgumentException($"The vertex {vertex} is already in the quiver.", nameof(vertex));

            Vertices.Add(vertex);
            adjacencyLists.Add(vertex, new HashSet<TVertex>());
        }

        public void RemoveVertex(TVertex vertex, out IEnumerable<Arrow<TVertex>> arrowsRemoved)
        {
            if (!Vertices.Contains(vertex)) throw new ArgumentException($"The vertex {vertex} is not contained in the quiver.", nameof(vertex));

            arrowsRemoved = GetArrowsInvolvingVertex(vertex).ToList();
            foreach (var arrow in arrowsRemoved) RemoveArrow(arrow);
            Vertices.Remove(vertex);
            adjacencyLists.Remove(vertex);
        }

        public void AddArrow(Arrow<TVertex> arrow)
        {
            if (arrow is null) throw new ArgumentNullException(nameof(arrow));
            AddArrow(arrow.Source, arrow.Target);
        }

        public void AddArrow(TVertex source, TVertex target)
        {
            if (!Vertices.Contains(source)) throw new ArgumentException($"The source vertex {source} is not contained in the quiver.", nameof(source));
            if (!Vertices.Contains(target)) throw new ArgumentException($"The target vertex {target} is not contained in the quiver.", nameof(target));

            var adjList = AdjacencyLists[source];
            if (adjList.Contains(target)) throw new ArgumentException($"The arrow from {source} to {target} already exists in the quiver.");
            adjList.Add(target);
        }

        public void RemoveArrow(Arrow<TVertex> arrow)
        {
            if (arrow is null) throw new ArgumentNullException(nameof(arrow));
            RemoveArrow(arrow.Source, arrow.Target);
        }

        public void RemoveArrow(TVertex source, TVertex target)
        {
            if (!Vertices.Contains(source)) throw new ArgumentException($"The source vertex {source} is not contained in the quiver.", nameof(source));
            if (!Vertices.Contains(target)) throw new ArgumentException($"The target vertex {target} is not contained in the quiver.", nameof(target));

            var adjList = AdjacencyLists[source];
            if (!adjList.Contains(target)) throw new ArgumentException($"The arrow from {source} to {target} is not contained in the quiver.");
            adjList.Remove(target);
        }

        public bool Equals(MutableQuiver<TVertex> otherQuiver)
        {
            if (otherQuiver is null) return false;

            if (!Vertices.SetEquals(otherQuiver.Vertices)) return false;
            foreach (var vertex in Vertices)
            {
                if (!AdjacencyLists[vertex].SetEquals(otherQuiver.AdjacencyLists[vertex])) return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is MutableQuiver<TVertex> quiver) return Equals(quiver);
            else return false;
        }
    }
}
