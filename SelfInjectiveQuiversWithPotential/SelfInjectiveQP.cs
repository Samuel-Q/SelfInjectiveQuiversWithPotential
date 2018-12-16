using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential
{
    /// <summary>
    /// This class represents a self-injective QP equipped with its Nakayama permutation.
    /// </summary>
    public class SelfInjectiveQP<TVertex> where TVertex : IEquatable<TVertex>, IComparable<TVertex>
    {
        /// <summary>
        /// Gets the QP.
        /// </summary>
        public QuiverWithPotential<TVertex> QP { get; }

        /// <summary>
        /// Gets the Nakayama permutation.
        /// </summary>
        public NakayamaPermutation<TVertex> NakayamaPermutation { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelfInjectiveQP{TVertex}"/> class.
        /// </summary>
        /// <param name="qp">The quiver with potential.</param>
        /// <param name="nakayamaPermutation">The Nakayama permutation.</param>
        /// <remarks>Almost no sanity checks are performed on the parameters.</remarks>
        public SelfInjectiveQP(QuiverWithPotential<TVertex> qp, NakayamaPermutation<TVertex> nakayamaPermutation)
        {
            QP = qp ?? throw new ArgumentNullException(nameof(qp));
            NakayamaPermutation = nakayamaPermutation ?? throw new ArgumentNullException(nameof(nakayamaPermutation));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelfInjectiveQP{TVertex}"/> class.
        /// </summary>
        /// <param name="qp">The quiver with potential.</param>
        /// <param name="nakayamaPermutation">The Nakayama permutation.</param>
        /// <remarks>Almost no sanity checks are performed on the parameters.</remarks>
        public SelfInjectiveQP(QuiverWithPotential<TVertex> qp, IReadOnlyDictionary<TVertex, TVertex> nakayamaPermutation) : this(qp, new NakayamaPermutation<TVertex>(nakayamaPermutation))
        {
        }
    }
}
