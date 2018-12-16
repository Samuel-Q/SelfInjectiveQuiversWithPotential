using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential
{
    /// <summary>
    /// This class represents a quiver, i.e., a multigraph with directed arrows (and loops allowed).
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertices.</typeparam>
    /// <remarks>As of this writing, this class does not support parallel arrows.</remarks>
    /// <seealso cref="MutableQuiver{TVertex}"/>
    public class Quiver<TVertex> where TVertex : IEquatable<TVertex>, IComparable<TVertex>
    {
        public ISet<TVertex> Vertices { get; private set; }

        /// <summary>
        /// Gets a dictionary mapping every vertex to its adjacency list (set), i.e., the list of arrows <em>starting</em> at the vertex.
        /// </summary>
        public IReadOnlyDictionary<TVertex, ISet<TVertex>> AdjacencyLists { get; private set; }

        public IEnumerable<Arrow<TVertex>> Arrows {
            get
            {
                foreach (var pair in AdjacencyLists)
                {
                    var source = pair.Key;
                    var list = pair.Value;
                    foreach (var target in list) yield return new Arrow<TVertex>(source, target);
                }
            }
        }

        public Quiver(IEnumerable<TVertex> vertices, IEnumerable<Arrow<TVertex>> arrows)
        {
            if (vertices == null) throw new ArgumentNullException(nameof(vertices));
            if (arrows == null) throw new ArgumentNullException(nameof(arrows));

            var verticesSet = new HashSet<TVertex>(vertices);
            if (verticesSet.Count != vertices.Count()) throw new ArgumentException("Vertex collection contains duplicates.", nameof(vertices));

            var arrowsSet = new HashSet<Arrow<TVertex>>(arrows);
            if (arrowsSet.Count != arrows.Count()) throw new ArgumentException("Arrow collection contains duplicates.", nameof(arrows));

            Vertices = verticesSet;
            AdjacencyLists = ConstructAdjacencyListDictionary(verticesSet, arrows);
        }

        private IReadOnlyDictionary<TVertex, ISet<TVertex>> ConstructAdjacencyListDictionary(ISet<TVertex> vertices, IEnumerable<Arrow<TVertex>> arrows)
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

        // Seems debatable whether the method should throw on source/target vertex not in the quiver. Not throwing for now.
        public bool ContainsArrow(Arrow<TVertex> arrow)
        {
            if (arrow is null) throw new ArgumentNullException(nameof(arrow));
            if (!Vertices.Contains(arrow.Source)) return false;

            return AdjacencyLists[arrow.Source].Contains(arrow.Target);
        }

        public bool ContainsArrow(TVertex source, TVertex target)
        {
            return ContainsArrow(new Arrow<TVertex>(source, target));
        }

        public bool ContainsPath(Path<TVertex> path)
        {
            if (path is null) throw new ArgumentNullException(nameof(path));

            return path.Arrows.All(a => ContainsArrow(a));
        }

        public bool Equals(Quiver<TVertex> otherQuiver)
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
            if (obj is Quiver<TVertex> quiver) return Equals(quiver);
            else return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 485112572;
            hashCode = hashCode * -1521134295 + EqualityComparer<ISet<TVertex>>.Default.GetHashCode(Vertices);
            hashCode = hashCode * -1521134295 + EqualityComparer<IReadOnlyDictionary<TVertex, ISet<TVertex>>>.Default.GetHashCode(AdjacencyLists);
            return hashCode;
        }
    }
}
