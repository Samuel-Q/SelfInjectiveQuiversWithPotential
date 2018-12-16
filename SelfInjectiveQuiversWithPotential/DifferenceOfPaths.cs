using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential
{
    /// <summary>
    /// This class represents a difference of paths in a quiver, which is useful in the context of
    /// semimonomial ideals.
    /// </summary>
    public class DifferenceOfPaths<TVertex> where TVertex : IEquatable<TVertex>, IComparable<TVertex>
    {
        /// <summary>
        /// Gets the minuend path (i.e., <c>p</c> in the difference <c>p-q</c>).
        /// </summary>
        public Path<TVertex> Minuend { get; private set; }

        /// <summary>
        /// Gets the subtrahend path (i.e., <c>q</c> in the difference <c>p-q</c>).
        /// </summary>
        public Path<TVertex> Subtrahend { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DifferenceOfPaths{TVertex}"/> class.
        /// </summary>
        /// <param name="minuend">The minuend path.</param>
        /// <param name="subtrahend">The subtrahend path.</param>
        public DifferenceOfPaths(Path<TVertex> minuend, Path<TVertex> subtrahend)
        {
            Minuend = minuend ?? throw new ArgumentNullException(nameof(minuend));
            Subtrahend = subtrahend ?? throw new ArgumentNullException(nameof(subtrahend));
        }

        public DifferenceOfPaths<TVertex> Negate()
        {
            return new DifferenceOfPaths<TVertex>(Subtrahend, Minuend);
        }

        public bool Equals(DifferenceOfPaths<TVertex> otherDifference)
        {
            if (otherDifference is null) return false;
            return Minuend.Equals(otherDifference.Minuend) && Subtrahend.Equals(otherDifference.Subtrahend);
        }

        public override bool Equals(object obj)
        {
            if (obj is DifferenceOfPaths<TVertex> otherDifference) return Equals(otherDifference);
            else return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 2125346914;
            hashCode = hashCode * -1521134295 + EqualityComparer<Path<TVertex>>.Default.GetHashCode(Minuend);
            hashCode = hashCode * -1521134295 + EqualityComparer<Path<TVertex>>.Default.GetHashCode(Subtrahend);
            return hashCode;
        }
    }
}
