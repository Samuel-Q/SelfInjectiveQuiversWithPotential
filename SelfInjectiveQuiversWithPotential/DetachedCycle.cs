using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential
{
    // TODO: This class should probably make use of Datastructures.CircularList<T>.

    /// <summary>
    /// This class represents a detached cycle, which is a cycle (a closed path, i.e., a path
    /// starting and ending at the same vertex) up to starting point.
    /// That is, a closed path and all its cyclic permutations correspond to
    /// a single instance of this class.
    /// </summary>
    public class DetachedCycle<TVertex> : IEquatable<DetachedCycle<TVertex>> where TVertex : IEquatable<TVertex>, IComparable<TVertex>
    {
        /// <summary>
        /// Gets the canonical representative path for this cycle.
        /// </summary>
        /// <remarks>The canonical representative path is the smallest path representing the cycle,
        /// where &quot;smallest&quot is with respect to the lexicographical ordering induced by
        /// the order on the vertices (viewing the path as a sequence of vertices).</remarks>
        public Path<TVertex> CanonicalPath { get; private set; }

        /// <summary>
        /// Gets the length of the cycle.
        /// </summary>
        public int Length { get { return CanonicalPath.Length;  } }

        public int LengthInVertices { get => Length + 1; }

        public IEnumerable<TVertex> Vertices { get => CanonicalPath.Vertices.Take(LengthInVertices - 1); }

        public IEnumerable<Arrow<TVertex>> Arrows { get => CanonicalPath.Arrows; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DetachedCycle{TVertex}"/> class.
        /// </summary>
        /// <param name="closedPath">A representative closed path of the cycle.</param>
        /// <exception cref="ArgumentNullException"><paramref name="closedPath"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="closedPath"/> is not a
        /// <em>closed</em> path. In other words, the first and last vertex of the path are not the
        /// same.</exception>
        public DetachedCycle(Path<TVertex> closedPath)
        {
            if (closedPath == null) throw new ArgumentNullException(nameof(closedPath));
            if (!closedPath.IsClosed) throw new ArgumentException("The path is not closed.", nameof(closedPath));

            CanonicalPath = CanonizeArrowSequence(closedPath);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DetachedCycle{TVertex}"/> class.
        /// </summary>
        /// <param name="vertices">The vertices of a representative closed path of the cycle.</param>
        /// <exception cref="ArgumentNullException"><paramref name="vertices"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="vertices"/> does not constitute a non-empty
        /// <em>closed</em> path.</exception>
        public DetachedCycle(IEnumerable<TVertex> vertices) : this(new Path<TVertex>(vertices?.ToArray() ?? throw new ArgumentNullException(nameof(vertices)))) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DetachedCycle{TVertex}"/> class.
        /// </summary>
        /// <param name="arrows">The arrows of a representative closed path of the cycle.</param>
        /// <exception cref="ArgumentNullException"><paramref name="arrows"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="arrows"/> does not constitute a valid
        /// <em>closed</em> path. In other words, it is either not a valid path or it fails to be
        /// closed (has different first and last vertex).</exception>
        public DetachedCycle(IEnumerable<Arrow<TVertex>> arrows) : this(new Path<TVertex>(arrows ?? throw new ArgumentNullException(nameof(arrows)))) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DetachedCycle{TVertex}"/> class.
        /// </summary>
        /// <param name="arrows">The arrows of a representative closed path of the cycle.</param>
        /// <exception cref="ArgumentNullException"><paramref name="arrows"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="arrows"/> does not constitute a valid
        /// <em>closed</em> path. In other words, it is either not a valid path or it fails to be
        /// closed (has different first and last vertex).</exception>
        public DetachedCycle(params Arrow<TVertex>[] arrows) : this(new Path<TVertex>(arrows ?? throw new ArgumentNullException(nameof(arrows)))) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DetachedCycle{TVertex}"/> class.
        /// </summary>
        /// <param name="vertices">The vertices of a representative closed path of the cycle.</param>
        /// <exception cref="ArgumentNullException"><paramref name="vertices"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="vertices"/> does not constitute a non-empty
        /// <em>closed</em> path.</exception>
        public DetachedCycle(params TVertex[] vertices) : this(new Path<TVertex>(vertices?.ToArray() ?? throw new ArgumentNullException(nameof(vertices)))) { }

        /// <summary>
        /// Returns the canonical representative path, equivalent to the given path under cyclical
        /// permutations.
        /// </summary>
        /// <remarks>The canonical representative is the path with smallest arrow sequence in the
        /// lexicographical order (induced by the order on the vertices).</remark>
        private Path<TVertex> CanonizeArrowSequence(Path<TVertex> path)
        {
            if (path.Length <= 1) return path;

            int bestStartIndex = 0;
            for (int currentStartIndex = 1; currentStartIndex < path.Length; currentStartIndex++)
            {
                for (int baseIndex = 0; baseIndex < path.Length; baseIndex++)
                {
                    int bestIndex = (baseIndex + bestStartIndex) % path.Length;
                    int currentIndex = (baseIndex + currentStartIndex) % path.Length;
                    var bestArrow = path.Arrows[bestIndex];
                    var permutedArrow = path.Arrows[currentIndex];

                    // Suffices to compare the sources, because the current target is the source in the next iteration
                    var comp = permutedArrow.Source.CompareTo(bestArrow.Source);
                    if (comp < 0)
                    {
                        bestStartIndex = currentStartIndex;
                        break;
                    }
                    else if (comp > 0)
                    {
                        break;
                    }
                }
            }

            var canonizedArrows = path.Arrows.Skip(bestStartIndex).Concat(path.Arrows.Take(bestStartIndex));
            var canonizedStartingPoint = canonizedArrows.First().Source;

            return new Path<TVertex>(canonizedStartingPoint, canonizedArrows);
        }

        public LinearCombination<Path<TVertex>> DifferentiateCyclically(Arrow<TVertex> arrow)
        {
            var linCombDict = new Dictionary<Path<TVertex>, int>();
            for (int index = 0; index < CanonicalPath.Length; index++)
            {
                var cycleArrow = CanonicalPath.Arrows[index];
                if (cycleArrow.Equals(arrow))
                {
                    var pathArrows = CanonicalPath.Arrows.Skip(index + 1).Concat(CanonicalPath.Arrows.Take(index));
                    var path = new Path<TVertex>(arrow.Target, pathArrows);
                    if (!linCombDict.ContainsKey(path)) linCombDict[path] = 1;
                    else linCombDict[path] += 1;
                }
            }

            return new LinearCombination<Path<TVertex>>(linCombDict);
        }

        public DetachedCycle<TVertex> Reverse()
        {
            return new DetachedCycle<TVertex>(CanonicalPath.Reverse());
        }

        public bool Equals(DetachedCycle<TVertex> otherCycle)
        {
            if (otherCycle == null) return false;
            return CanonicalPath.Equals(otherCycle.CanonicalPath);
        }

        public override bool Equals(object obj)
        {
            var cycleObj = obj as DetachedCycle<TVertex>;
            if (ReferenceEquals(cycleObj, null)) return false; // Careful with the overloaded == operator
            return Equals(cycleObj);
        }

        public override int GetHashCode()
        {
            return CanonicalPath.GetHashCode();
        }

        public static bool operator ==(DetachedCycle<TVertex> cycle1, DetachedCycle<TVertex> cycle2)
        {
            if (ReferenceEquals(cycle1, null)) return ReferenceEquals(cycle2, null);
            return cycle1.Equals(cycle2);
        }

        public static bool operator !=(DetachedCycle<TVertex> cycle1, DetachedCycle<TVertex> cycle2) => !(cycle1 == cycle2);

        public override string ToString()
        {
            return CanonicalPath.ToString();
        }
    }
}
