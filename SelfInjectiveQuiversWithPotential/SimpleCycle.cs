using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential
{
    /// <summary>
    /// This class represents a simple cycle (equivalently, a simple closed path, or equivalently,
    /// a cycle without repeated vertices (and arrows)) up to starting point.
    /// </summary>
    public class SimpleCycle<TVertex> : DetachedCycle<TVertex> where TVertex : IEquatable<TVertex>, IComparable<TVertex>
    {
        /// <summary>
        /// Gets the vertices of the simple cycle.
        /// </summary>
        public IEnumerable<TVertex> Vertices
        {
            get
            {
                if (Length == 0) return new TVertex[] { CanonicalPath.StartingPoint };
                else return CanonicalPath.Arrows.Select(e => e.Source);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleCycle{TVertex}"/> class.
        /// </summary>
        /// <param name="simpleClosedPath">A representative simple closed path of the cycle.</param>
        /// <exception cref="ArgumentNullException"><paramref name="simpleClosedPath"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="simpleClosedPath"/> is not a
        /// <em>closed</em> path (i.e., the first and last vertex of the path are not the same), or
        /// <paramref name="simpleClosedPath"/> is not a <em>simple</em> path (i.e., a vertex is
        /// repeated somewhere other than at the start or end of the path).</exception>
        public SimpleCycle(Path<TVertex> simpleClosedPath) : base(
            simpleClosedPath == null ? throw new ArgumentNullException(nameof(simpleClosedPath)) :
            !simpleClosedPath.IsClosed ? throw new ArgumentException("The path is not closed.", nameof(simpleClosedPath)) :
            !simpleClosedPath.IsSimple ? throw new ArgumentException("The path is not simple.", nameof(simpleClosedPath)) :
            simpleClosedPath)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleCycle{TVertex}"/> class.
        /// </summary>
        /// <param name="arrows">The arrows of a representative simple closed path of the cycle.</param>
        /// <exception cref="ArgumentNullException"><paramref name="arrows"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="arrows"/> does not constitute a valid
        /// <em>simple</em> and <em>closed</em> path. In other words, it is either of the following:
        /// not a valid path, fails to be simple (a vertex is repeated somewhere other than at the
        /// start or the end), or fails to be closed (has different first and last vertex).</exception>
        public SimpleCycle(IEnumerable<Arrow<TVertex>> arrows) : this(new Path<TVertex>(arrows ?? throw new ArgumentNullException(nameof(arrows)))) { }

        /// <summary>
        /// Gets the unique representative path for this cycle starting at the specified vertex.
        /// </summary>
        /// <param name="startVertex"></param>
        public Path<TVertex> GetPathStartingAt(TVertex startVertex)
        {
            if (CanonicalPath.Length == 0)
            {
                if (!startVertex.Equals(CanonicalPath.StartingPoint)) throw new ArgumentException($"The cycle does not contain the vertex {startVertex}.", nameof(startVertex));
                else return CanonicalPath;
            }

            int count = CanonicalPath.Arrows.Count(a => a.Source.Equals(startVertex));
            if (count == 0) throw new ArgumentException($"The cycle does not contain the vertex {startVertex}.", nameof(startVertex));

            var start = CanonicalPath.Arrows.SkipWhile(a => !a.Source.Equals(startVertex));
            var end = CanonicalPath.Arrows.TakeWhile(a => !a.Source.Equals(startVertex));
            var path = new Path<TVertex>(start.Concat(end));
            return path;
        }
    }
}
