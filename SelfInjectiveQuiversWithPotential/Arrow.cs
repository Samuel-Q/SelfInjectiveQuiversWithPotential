using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential
{
    /// <summary>
    /// This class represents an arrow in a quiver.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertices.</typeparam>
    /// <remarks>
    /// <para>As of this writing, this class represents an arrow in a quiver <em>without parallel
    /// arrows</em>.</para>
    /// <para>This class is immutable.</para>
    /// </remarks>
    public class Arrow<TVertex> : IEquatable<Arrow<TVertex>>, IComparable<Arrow<TVertex>>
        where TVertex : IEquatable<TVertex>, IComparable<TVertex>
    {
        public TVertex Source { get; private set; }

        public TVertex Target { get; private set; }

        public Arrow(TVertex source, TVertex target)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (target == null) throw new ArgumentNullException(nameof(target));

            Source = source;
            Target = target;
        }

        public Arrow<TVertex> Reverse() => new Arrow<TVertex>(Target, Source);

        public int CompareTo(Arrow<TVertex> other)
        {
            int cmpVal = Source.CompareTo(other.Source);
            if (cmpVal != 0) return cmpVal;

            return Target.CompareTo(other.Target);
        }

        public static bool operator ==(Arrow<TVertex> arrow1, Arrow<TVertex> arrow2)
        {
            if (ReferenceEquals(arrow1, null)) return ReferenceEquals(arrow2, null);
            return arrow1.Equals(arrow2);
        }

        public static bool operator !=(Arrow<TVertex> arrow1, Arrow<TVertex> arrow2) => !(arrow1 == arrow2);

        public bool Equals(Arrow<TVertex> otherArrow)
        {
            if (ReferenceEquals(otherArrow, null)) return false; // Careful with the overloaded == operator
            return Source.Equals(otherArrow.Source) && Target.Equals(otherArrow.Target);
        }

        public override bool Equals(object obj)
        {
            var arrowObj = obj as Arrow<TVertex>;
            if (ReferenceEquals(arrowObj, null)) return false; // Careful with the overloaded == operator
            return Equals(arrowObj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 29) + Source.GetHashCode();
                hash = (hash * 29) + Target.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return $"({Source}, {Target})";
        }
    }
}
