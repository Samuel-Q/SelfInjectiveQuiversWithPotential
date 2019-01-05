using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential.Plane;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    // Thoughts: The conversions from specific enum (QPExtractionResult or QPAnalysisMainResult) to QuiverInPlaneAnalysisMainResult
    // could be placed in extension methods for the specific enums.

    /// <summary>
    /// This class represents the results of an analysis of a quiver in the plane done by an <see cref="IQuiverInPlaneAnalyzer"/>.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertices in the quiver.</typeparam>
    /// <remarks>The counterpart for QPs (though mutable) is <see cref="QPAnalysisResults{TVertex}"/></remarks>
    public class QuiverInPlaneAnalysisResults<TVertex> : AnalysisResults<TVertex, QuiverInPlaneAnalysisMainResult>, IQuiverInPlaneAnalysisResults<TVertex>
        where TVertex : IEquatable<TVertex>, IComparable<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuiverInPlaneAnalysisResults{TVertex}"/> class.
        /// </summary>
        /// <param name="mainResult">The main result.</param>
        /// <param name="maximalPathRepresentatives">A dictionary mapping every vertex of the
        /// quiver to a collection of representatives of all maximal non-zero equivalence classes
        /// of paths starting at the vertex, or <see langword="null"/> depending on whether the
        /// analysis was successful.</param>
        /// <param name="nakayamaPermutation">The Nakayama permutation for the induced QP or
        /// <see langword="null"/>, depending on whether the analysis was successful and the
        /// induced QP has a Nakayama permutation.</param>
        /// <param name="longestPathEncountered">A path of maximal length of the paths encountered
        /// during the analysis.</param>
        public QuiverInPlaneAnalysisResults(
            QuiverInPlaneAnalysisMainResult mainResult,
            IReadOnlyDictionary<TVertex, IEnumerable<Path<TVertex>>> maximalPathRepresentatives,
            NakayamaPermutation<TVertex> nakayamaPermutation,
            Path<TVertex> longestPathEncountered)
            : base(mainResult, maximalPathRepresentatives, nakayamaPermutation, longestPathEncountered)
        {
            if (mainResult.HasFlag(QuiverInPlaneAnalysisMainResult.Success) && maximalPathRepresentatives is null)
                throw new ArgumentNullException(nameof(maximalPathRepresentatives));

            if (mainResult.IndicatesSelfInjectivity() && nakayamaPermutation is null)
                throw new ArgumentNullException(nameof(nakayamaPermutation));
        }
    }
}
