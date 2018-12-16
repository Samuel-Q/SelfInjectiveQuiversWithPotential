using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    /// <summary>
    /// This class represents the results of a QP analysis done by an <see cref="IQPAnalyzer"/> and is mutable.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertices in the QP.</typeparam>
    /// <remarks>The counterpart for quivers in plane (though immutable) is <see cref="Plane.QuiverInPlaneAnalysisResults{TVertex}"/></remarks>
    public class QPAnalysisResults<TVertex> : AnalysisResults<TVertex, QPAnalysisMainResult>, IQPAnalysisResults<TVertex> where TVertex : IEquatable<TVertex>, IComparable<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QPAnalysisResults{TVertex}"/> class.
        /// </summary>
        /// <param name="mainResult">The main result.</param>
        /// <param name="maximalPathRepresentatives">A dictionary mapping every vertex of the
        /// quiver to a collection of representatives of all maximal non-zero equivalence classes
        /// of paths starting at the vertex, or <see langword="null"/> depending on whether the
        /// analysis was successful.</param>
        /// <param name="nakayamaPermutation">The Nakayama permutation for the analyzed QP or
        /// <see langword="null"/>, depending on whether the analysis was successful and the QP has
        /// a Nakayama permutation.</param>
        /// <param name="longestPathEncountered">A path of maximal length of the paths encountered
        /// during the analysis.</param>
        public QPAnalysisResults(
            QPAnalysisMainResult mainResult,
            IReadOnlyDictionary<TVertex, IEnumerable<Path<TVertex>>> maximalPathRepresentatives,
            NakayamaPermutation<TVertex> nakayamaPermutation,
            Path<TVertex> longestPathEncountered)
            : base(mainResult, maximalPathRepresentatives, nakayamaPermutation, longestPathEncountered)
        {
            if (mainResult.HasFlag(QPAnalysisMainResult.Success) && maximalPathRepresentatives is null)
                throw new ArgumentNullException(nameof(maximalPathRepresentatives));

            if (mainResult.HasFlag(QPAnalysisMainResult.SelfInjective) && nakayamaPermutation is null)
                throw new ArgumentNullException(nameof(nakayamaPermutation));

            if (LongestPathEncountered is null) throw new ArgumentNullException(nameof(longestPathEncountered));
        }
    }
}
