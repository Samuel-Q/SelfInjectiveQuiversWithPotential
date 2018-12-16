using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential
{
    public interface INakayamaPermutation<TVertex> where TVertex : IEquatable<TVertex>, IComparable<TVertex>
    {
        /// <summary>
        /// Gets the vertex to which the Nakayama permutation maps the specified vertex.
        /// </summary>
        /// <param name="vertex">The input vertex for the Nakayama permutation.</param>
        /// <returns>The output vertex for the Nakayama permutation.</returns>
        TVertex this[TVertex vertex] { get; }

        /// <summary>
        /// Gets the order of the Nakayama permutation.
        /// </summary>
        int Order { get; }
    }
}
