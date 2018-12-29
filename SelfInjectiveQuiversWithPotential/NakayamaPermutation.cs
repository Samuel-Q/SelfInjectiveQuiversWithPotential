using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential
{
    // TODO: Override Object.GetHashCode() as well
    // TODO: This class does nothing Nakayama-specific and could just be a general Permutation class.

    /// <summary>
    /// This class represents a Nakayama permutation for a bound quiver or a quiver with potential.
    /// </summary>
    public class NakayamaPermutation<TVertex> : IEnumerable<KeyValuePair<TVertex, TVertex>>
        where TVertex : IEquatable<TVertex>, IComparable<TVertex>
    {
        private void ValidateVertex(TVertex vertex)
        {
            if (!UnderlyingDictionary.ContainsKey(vertex))
                throw new ArgumentException($"The vertex {vertex} is not a member of the domain of this permutation.", nameof(vertex));
        }

        /// <summary>
        /// The underlying dictionary that represents the Nakayama permutation.
        /// </summary>
        public IReadOnlyDictionary<TVertex, TVertex> UnderlyingDictionary { get; }

        /// <summary>
        /// Gets the vertex to which the Nakayama permutation maps the specified vertex.
        /// </summary>
        /// <param name="vertex">The input vertex for the Nakayama permutation.</param>
        /// <returns>The output vertex for the Nakayama permutation.</returns>
        /// <exception cref="ArgumentException"><paramref name="vertex"/> is not in the domain of
        /// this permutation.</exception>
        public TVertex this[TVertex vertex]
        {
            get
            {
                ValidateVertex(vertex);
                return UnderlyingDictionary[vertex];
            }
        }

        /// <summary>
        /// Gets the order of the Nakayama permutation.
        /// </summary>
        public int Order
        {
            get
            {
                return Utility.GetOrderOfPermutation(UnderlyingDictionary);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NakayamaPermutation{TVertex}"/> class.
        /// </summary>
        /// <param name="nakayamaPermutation">The Nakayama permutation represented by a dictionary.</param>
        /// <exception cref="ArgumentNullException"><paramref name="nakayamaPermutation"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="nakayamaPermutation"/> does not
        /// represent a permutation.</exception>
        public NakayamaPermutation(IReadOnlyDictionary<TVertex, TVertex> nakayamaPermutation)
        {
            UnderlyingDictionary = nakayamaPermutation ?? throw new ArgumentNullException(nameof(nakayamaPermutation));
            if (!nakayamaPermutation.Keys.EqualUpToOrder(nakayamaPermutation.Values))
            {
                throw new ArgumentException("The dictionary does not represent a permutation.", nameof(nakayamaPermutation));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NakayamaPermutation{TVertex}"/> class.
        /// </summary>
        /// <param name="nakayamaPermutation">The Nakayama permutation represented by a dictionary.</param>
        /// <exception cref="ArgumentNullException"><paramref name="nakayamaPermutation"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="nakayamaPermutation"/> does not
        /// represent a permutation.</exception>
        public NakayamaPermutation(IDictionary<TVertex, TVertex> nakayamaPermutation)
        {
            if (nakayamaPermutation is null) throw new ArgumentNullException(nameof(nakayamaPermutation));
            UnderlyingDictionary = nakayamaPermutation.ToDictionary(p => p.Key, p => p.Value);
            if (!nakayamaPermutation.Keys.EqualUpToOrder(nakayamaPermutation.Values))
            {
                throw new ArgumentException("The dictionary does not represent a permutation.", nameof(nakayamaPermutation));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NakayamaPermutation{TVertex}"/> class.
        /// </summary>
        /// <param name="nakayamaPermutation">The Nakayama permutation represented by a dictionary.</param>
        /// <exception cref="ArgumentNullException"><paramref name="nakayamaPermutation"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="nakayamaPermutation"/> does not
        /// represent a permutation.</exception>
        public NakayamaPermutation(Dictionary<TVertex, TVertex> nakayamaPermutation) : this((IReadOnlyDictionary<TVertex, TVertex>)nakayamaPermutation)
        { }

        /// <summary>
        /// Gets the orbit of a vertex.
        /// </summary>
        /// <param name="vertex">The vertex whose orbit to get.</param>
        /// <returns>The orbit of <paramref name="vertex"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="vertex"/> is in the domain of this
        /// permutation.</exception>
        public IEnumerable<TVertex> GetOrbit(TVertex vertex)
        {
            ValidateVertex(vertex);

            var vertices = new HashSet<TVertex>();
            do
            {
                yield return vertex;
                vertices.Add(vertex);
                vertex = this[vertex];
            } while (!vertices.Contains(vertex));
        }

        public bool Equals(NakayamaPermutation<TVertex> otherPermutation)
        {
            if (otherPermutation is null) return false;

            return UnderlyingDictionary.EqualUpToOrder(otherPermutation.UnderlyingDictionary);
        }

        /// <remarks>Beware that this method is asymmetric in the sense that
        /// <c>foo.Equals(bar)</c> is not guaranteed to be the same as
        /// <c>bar.Equals(foo)</c>.</remarks>
        public bool Equals(IReadOnlyDictionary<TVertex, TVertex> otherPermutation)
        {
            if (otherPermutation is null) return false;

            return UnderlyingDictionary.EqualUpToOrder(otherPermutation);
        }

        /// <remarks>Beware that this method is asymmetric in the sense that
        /// <c>foo.Equals(bar)</c> is not guaranteed to be the same as
        /// <c>bar.Equals(foo)</c>.</remarks>
        public bool Equals(IDictionary<TVertex, TVertex> otherPermutation)
        {
            if (otherPermutation is null) return false;

            return UnderlyingDictionary.EqualUpToOrder(otherPermutation);
        }

        /// <remarks>Beware that this method is asymmetric in the sense that
        /// <c>foo.Equals(bar)</c> is not guaranteed to be the same as
        /// <c>bar.Equals(foo)</c>.</remarks>
        public override bool Equals(object obj)
        {
            if (obj is NakayamaPermutation<TVertex> otherPermutation1) return Equals(otherPermutation1);
            if (obj is IReadOnlyDictionary<TVertex, TVertex> otherPermutation2) return Equals(otherPermutation2);
            if (obj is IDictionary<TVertex, TVertex> otherPermutation3) return Equals(otherPermutation3);
            return false;
        }

        public override int GetHashCode()
        {
            var hashCode = 911554221;
            hashCode = hashCode * -1521134295 + EqualityComparer<IReadOnlyDictionary<TVertex, TVertex>>.Default.GetHashCode(UnderlyingDictionary);
            hashCode = hashCode * -1521134295 + Order.GetHashCode();
            return hashCode;
        }

        public IEnumerator<KeyValuePair<TVertex, TVertex>> GetEnumerator()
        {
            return UnderlyingDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return UnderlyingDictionary.GetEnumerator();
        }
    }
}
