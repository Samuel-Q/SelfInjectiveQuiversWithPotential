﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    public class SemimonomialUnboundQuiverAnalysisResults<TVertex> :
        AnalysisResults<TVertex, SemimonomialUnboundQuiverAnalysisMainResults>, ISemimonomialUnboundQuiverAnalysisResults<TVertex>
        where TVertex : IEquatable<TVertex>, IComparable<TVertex>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SemimonomialUnboundQuiverAnalysisResults{TVertex}"/> class.
        /// </summary>
        /// <param name="mainResults">The main results.</param>
        /// <param name="maximalPathRepresentatives">A dictionary mapping every vertex of the
        /// quiver to a collection of representatives of all maximal non-zero equivalence classes
        /// of paths starting at the vertex, or <see langword="null"/> depending on whether the
        /// analysis was successful.</param>
        /// <param name="nakayamaPermutation">The Nakayama permutation for the analyzed
        /// semimonomial unbound quiver or
        /// <see langword="null"/>, depending on whether the analysis was successful and the
        /// semimonomial unbound quiver has a Nakayama permutation.</param>
        /// <param name="longestPathEncountered">A path of maximal length of the paths encountered
        /// during the analysis, or <see langword="null"/> if no path was encountered during the
        /// analysis (i.e., if the quiver was empty).</param>
        public SemimonomialUnboundQuiverAnalysisResults(
            SemimonomialUnboundQuiverAnalysisMainResults mainResults,
            IReadOnlyDictionary<TVertex, IEnumerable<Path<TVertex>>> maximalPathRepresentatives,
            NakayamaPermutation<TVertex> nakayamaPermutation,
            Path<TVertex> longestPathEncountered)
            : base(mainResults, maximalPathRepresentatives, nakayamaPermutation, longestPathEncountered)
        {
            if (mainResults.HasFlag(SemimonomialUnboundQuiverAnalysisMainResults.Success) && maximalPathRepresentatives is null)
                throw new ArgumentNullException(nameof(maximalPathRepresentatives));

            if (mainResults.IndicatesSelfInjectivity() && nakayamaPermutation is null)
                throw new ArgumentNullException(nameof(nakayamaPermutation));
        }
    }
}
