using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential
{
    /// <summary>
    /// This class represents a (nonzero) path with a starting point.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertices.</typeparam>
    /// <remarks>
    /// <para>A path is a possibly empty sequence of adjacent arrows.</para>
    /// <para>This class is immutable.</para>
    /// </remarks>
    public class Path<TVertex> : IEquatable<Path<TVertex>> where TVertex : IEquatable<TVertex>, IComparable<TVertex>
    {
        /// <summary>
        /// Gets the starting point of the path.
        /// </summary>
        /// <remarks>The value of this property is never <see langword="null"/>.</remarks>
        public TVertex StartingPoint { get; private set; }

        /// <summary>
        /// Gets the ending point of the path.
        /// </summary>
        /// <remarks>&quot;Ending point&quot is not a well-established (compound) noun but it makes
        /// a good amount of sense.</remarks>
        public TVertex EndingPoint {
            get
            {
                if (Length == 0) return StartingPoint;
                return Arrows[Arrows.Count - 1].Target;
            }
        }

        /// <summary>
        /// Gets the sequence of adjacent arrows that constitute the path.
        /// </summary>
        /// <remarks>The value of this property is never <see langword="null"/>.</remarks>
        public IReadOnlyList<Arrow<TVertex>> Arrows { get; private set; }

        /// <summary>
        /// Gets the length of the path in arrows.
        /// </summary>
        public int Length { get => Arrows.Count; }

        /// <summary>
        /// Gets the length of the path in vertices.
        /// </summary>
        public int LengthInVertices { get => Length + 1; }

        /// <summary>
        /// Gets the sequence of vertices that constitute the path.
        /// </summary>
        /// <remarks>The returned <see cref="IEnumerable{T}"/> may contain duplicates.</remarks>
        public IEnumerable<TVertex> Vertices
        {
            get
            {
                yield return StartingPoint;
                foreach (var arrow in Arrows) yield return arrow.Target;
            }
        }

        /// <summary>
        /// Gets a boolean value indicating whether the path is closed (has the same starting point
        /// as ending point).
        /// </summary>
        public bool IsClosed { get => StartingPoint.Equals(EndingPoint); }

        /// <summary>
        /// Gets a boolean value indicating whether the path is simple (has no vertices (and arrows)
        /// repeated, except possibly for the first and last vertex).
        /// </summary>
        public bool IsSimple {
            get
            {
                int numVerticesIfSimple = StartingPoint.Equals(EndingPoint) ? LengthInVertices - 1 : LengthInVertices;
                var vertexSet = new HashSet<TVertex>(Vertices);
                return vertexSet.Count == numVerticesIfSimple;
            }
        }

        public Path(TVertex startingPoint) : this(startingPoint, new Arrow<TVertex>[] { }) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arrows"></param>
        /// <exception cref="ArgumentNullException"><paramref name="arrows"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="arrows"/> do not constitute a non-empty path.
        /// Explicitly, <paramref name="arrows"/> is empty or some pair of consecutive arrows are not adjacent.</exception>
        public Path(IEnumerable<Arrow<TVertex>> arrows)
            : this(arrows == null ? throw new ArgumentNullException(nameof(arrows)) : arrows.Count() == 0 ? throw new ArgumentException("The path must contain at least one arrow for this constructor.", nameof(arrows)) : arrows.First().Source, arrows)
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arrows"></param>
        /// <exception cref="ArgumentNullException"><paramref name="vertices"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="vertices"/> is empty..</exception>
        public Path(params TVertex[] vertices) :
            this(
                vertices == null ? throw new ArgumentNullException(nameof(vertices)) :
                vertices.Length > 0 ? vertices.First() : throw new ArgumentException("The vertex collection must be non-empty.", nameof(vertices)),
                GetArrows(vertices))
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arrows"></param>
        /// <exception cref="ArgumentNullException"><paramref name="vertices"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="vertices"/> is empty..</exception>
        public Path(IEnumerable<TVertex> vertices) :
            this((vertices ?? throw new ArgumentNullException(nameof(vertices))).ToArray())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Path{TVertex}"/> class.
        /// </summary>
        /// <param name="startingPoint">The starting point of the path.</param>
        /// <param name="arrows">The sequence of adjacent arrows that constitute the nonzero path.</param>
        /// <exception cref="ArgumentNullException"><paramref name="startingPoint"/> is <see langword="null"/>
        /// or <paramref name="arrows"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">The arrows do not constitute a path starting at
        /// <paramref name="startingPoint"/>. Explicitly, <paramref name="arrows"/> is non-empty and
        /// either some pair of consecutive arrows are not adjacent, or the source of the first arrow
        /// is not <paramref name="startingPoint"/>.</exception>
        public Path(TVertex startingPoint, IEnumerable<Arrow<TVertex>> arrows)
        {
            if (startingPoint == null) throw new ArgumentNullException(nameof(startingPoint));
            if (arrows == null) throw new ArgumentNullException(nameof(arrows));

            var arrowsList = arrows.ToList();
            if (!ArrowsConstituteAPathWithStartingPoint(startingPoint, arrowsList))
            {
                throw new ArgumentException($"The arrows do not constitute a path starting at {startingPoint}.", nameof(arrows));
            }

            StartingPoint = startingPoint;
            Arrows = arrowsList;
        }

        private Path(TVertex startingPoint, IReadOnlyList<Arrow<TVertex>> arrows)
        {
            StartingPoint = startingPoint;
            Arrows = arrows;
        }

        private bool ArrowsConstituteAPathWithStartingPoint(TVertex startingPoint, IEnumerable<Arrow<TVertex>> arrows)
        {
            if (!arrows.Any()) return true;
            var expectedSource = startingPoint;
            foreach (var arrow in arrows)
            {
                if (!arrow.Source.Equals(expectedSource)) return false;
                expectedSource = arrow.Target;
            }

            return true;
        }

        private static IEnumerable<Arrow<TVertex>> GetArrows(params TVertex[] vertices)
        {
            if (vertices.Length == 0) throw new ArgumentException(nameof(vertices));
            var arrows = new Arrow<TVertex>[vertices.Length - 1];
            for (int i = 0; i < arrows.Length; i++)
            {
                arrows[i] = new Arrow<TVertex>(vertices[i], vertices[i + 1]);
            }

            return arrows;
        }

        /// <summary>
        /// Extracts a subpath.
        /// </summary>
        /// <param name="index">The index of the first arrow in the subpath.</param>
        /// <param name="count">The number of arrows in the subpath.</param>
        /// <returns>The subpath.</returns>
        public Path<TVertex> ExtractSubpath(int index, int count)
        {
            if (index < 0) throw new ArgumentOutOfRangeException("The index is negative.", nameof(index));
            if (count < 0) throw new ArgumentOutOfRangeException("The count is negative.", nameof(count));
            if (index + count > Arrows.Count) throw new ArgumentOutOfRangeException(nameof(count));

            if (count == 0 && index == Length) return new Path<TVertex>(EndingPoint);
            var startingPoint = Arrows[index].Source;
            //var arrows = Arrows.Skip(index).Take(count).ToList(); // Use this for speed
            var arrows = Arrows.Skip(index).Take(count);
            return new Path<TVertex>(startingPoint, arrows);
        }

        public Path<TVertex> PrependArrow(Arrow<TVertex> arrow)
        {
            if (arrow == null) throw new ArgumentNullException(nameof(arrow));
            if (!(arrow.Target.Equals(StartingPoint))) throw new ArgumentException($"The target of the arrow {arrow} to prepend is not the starting point of the path.", nameof(arrow));

            var arrows = new List<Arrow<TVertex>> { arrow };
            arrows.AddRange(Arrows);
            return new Path<TVertex>(arrow.Source, arrows);
        }

        public Path<TVertex> AppendVertex(TVertex vertex)
        {
            var vertices = Vertices.ToList();
            vertices.Add(vertex);
            return new Path<TVertex>(vertices.ToArray());
        }

        public Path<TVertex> AppendArrow(Arrow<TVertex> arrow)
        {
            if (arrow == null) throw new ArgumentNullException(nameof(arrow));
            if (!(arrow.Source.Equals(EndingPoint))) throw new ArgumentException($"The source of the arrow {arrow} to append is not the ending point of the path.", nameof(arrow));

            var arrows = Arrows.ToList();
            arrows.Add(arrow);
            return new Path<TVertex>(StartingPoint, arrows);
        }

        public Path<TVertex> AppendPath(Path<TVertex> path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (!path.StartingPoint.Equals(this.EndingPoint)) throw new ArgumentException($"The starting point of the path to append, {path}, is not the ending point of this path, {this}.");

            var arrows = this.Arrows.ToList();
            arrows.AddRange(path.Arrows);
            return new Path<TVertex>(this.StartingPoint, arrows);
        }

        public Path<TVertex> ReplaceSubpath(int index, int count, Path<TVertex> newSubpath)
        {
            if (newSubpath == null) throw new ArgumentNullException(nameof(newSubpath));
            if (index < 0) throw new ArgumentOutOfRangeException("The index is negative.", nameof(index));
            if (count < 0) throw new ArgumentOutOfRangeException("The count is negative.", nameof(count));
            if (index + count > Arrows.Count) throw new ArgumentOutOfRangeException(nameof(count));

            var oldSubpath = ExtractSubpath(index, count);
            if (!(oldSubpath.StartingPoint.Equals(newSubpath.StartingPoint) && oldSubpath.EndingPoint.Equals(newSubpath.EndingPoint)))
            {
                throw new ArgumentException("The endpoints of the subpaths do not match.");
            }

            //var arrows = Arrows.Take(index).Concat(newSubpath.Arrows).Concat(Arrows.Skip(index + count)).ToList(); // Use this for speed
            var arrows = Arrows.Take(index).Concat(newSubpath.Arrows).Concat(Arrows.Skip(index + count));
            return new Path<TVertex>(StartingPoint, arrows);
        }

        public Path<TVertex> Reverse()
        {
            // Reverse both the order of the arrowa and each arrow
            return new Path<TVertex>(EndingPoint, Arrows.Reverse().Select(arrow => new Arrow<TVertex>(arrow.Target, arrow.Source)));
        }

        public bool Equals(Path<TVertex> otherPath)
        {
            if (ReferenceEquals(otherPath, null)) return false; // Careful with the overloaded == operator

            return StartingPoint.Equals(otherPath.StartingPoint) && Arrows.SequenceEqual(otherPath.Arrows);
        }

        public override bool Equals(object obj)
        {
            var pathObj = obj as Path<TVertex>;
            if (ReferenceEquals(pathObj, null)) return false; // Careful with the overloaded == operator
            return Equals(pathObj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 29) + StartingPoint.GetHashCode();
                foreach (var arrow in Arrows)
                {
                    hash = (hash * 29) + arrow.GetHashCode();
                }

                return hash;
            }
        }

        public static bool operator ==(Path<TVertex> path1, Path<TVertex> path2)
        {
            if (ReferenceEquals(path1, null)) return ReferenceEquals(path2, null);
            return path1.Equals(path2);
        }

        public static bool operator !=(Path<TVertex> path1, Path<TVertex> path2) => !(path1 == path2);

        public override string ToString()
        {
            if (Length == 0) return String.Format("({0})", StartingPoint);

            var b = new StringBuilder("(");
            foreach (var arrow in Arrows)
            {
                b.AppendFormat("{0}->", arrow.Source);
            }

            b.Append(EndingPoint);
            b.Append(")");
            return b.ToString();
        }
    }
}
