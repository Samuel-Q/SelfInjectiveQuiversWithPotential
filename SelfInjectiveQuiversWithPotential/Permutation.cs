using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential
{
    /// <summary>
    /// This class represents a permutation, typically but not at all necessarily of vertices of a QP).
    /// </summary>
    /// <typeparam name="T">The type of objects that are permuted.</typeparam>
    /// <remarks>This type is immutable.</remarks>
    public class Permutation<T>
    {
        /// <summary>
        /// A dictionary representing the permutation, with each key-value pair indicating that the
        /// key is mapped to the value by the permutation.
        /// </summary>
        private readonly IReadOnlyDictionary<T, T> dictionary;

        /// <summary>
        /// Gets the order of the permutation.
        /// </summary>
        public int Order { get => Utility.GetOrderOfPermutation(dictionary); }

        /// <summary>
        /// Initializes a new instance of the <see cref="Permutation{T}"/> class.
        /// </summary>
        /// <param name="permutation">A dictionary representing the permutation: a pair (x, y)
        /// representing p(x) = y, for p the permutation.</param>
        public Permutation(IReadOnlyDictionary<T, T> permutation)
        {
            if (permutation == null) throw new ArgumentNullException(nameof(permutation));
            if (!new HashSet<T>(permutation.Keys).SetEquals(permutation.Values)) throw new ArgumentException("The dictionary does not represent a permutation.", nameof(permutation));
            this.dictionary = permutation;
        }
    }
}
