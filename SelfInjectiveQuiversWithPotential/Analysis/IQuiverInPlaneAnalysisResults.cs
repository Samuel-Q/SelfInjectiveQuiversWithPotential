using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    public interface IQuiverInPlaneAnalysisResults<TVertex> where TVertex : IEquatable<TVertex>, IComparable<TVertex>
    {
        /// <summary>
        /// Gets the main result of the analysis (whether it was successful or why it failed).
        /// </summary>
        QuiverInPlaneAnalysisMainResult MainResult { get; }

        /// <summary>
        /// Gets a dictionary mapping every vertex of the quiver to a collection of representatives
        /// of all maximal non-zero equivalence classes of paths starting at the vertex.
        /// </summary>
        IReadOnlyDictionary<TVertex, IEnumerable<Path<TVertex>>> MaximalPathRepresentatives { get; }

        /// <summary>
        /// Gets the Nakayama permutation for the QP if there is one, or <see langword="null"/> if
        /// there is none.
        /// </summary>
        NakayamaPermutation<TVertex> NakayamaPermutation { get; }

        /// <summary>
        /// Gets a path whose length is maximal among the paths encountered during the analysis.
        /// </summary>
        Path<TVertex> LongestPathEncountered { get; }
    }
}
