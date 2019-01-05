using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    /// <summary>
    /// The return type for the computations of maximal nonzero equivalence classes
    /// (<see cref="IMaximalNonzeroEquivalenceClassRepresentativeComputer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt{TVertex}(Quiver{TVertex}, TVertex, TransformationRuleTreeNode{TVertex}, SemimonomialUnboundQuiverAnalysisSettings)"/>).
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertices in the quiver.</typeparam>
    public class MaximalNonzeroEquivalenceClassRepresentativesResults<TVertex> where TVertex: IEquatable<TVertex>, IComparable<TVertex>
    {
        /// <summary>
        /// Gets a <see cref="CancellativityTypes"/> value indicating which types of cancellativity
        /// was detected to fail.
        /// </summary>
        public CancellativityTypes CancellativityFailuresDetected { get; private set; }

        /// <summary>
        /// Gets a boolean value indicating whether failure of cancellativity has been detected.
        /// </summary>
        /// <remarks>
        /// <para>If the analysis settings dictate that cancellativity failure should not be
        /// detected, then this value is <see langword="false"/>.</para>
        /// </remarks>
        public bool CancellativityFailureDetected => CancellativityFailuresDetected.HasFlag(CancellativityTypes.Cancellativity);

        /// <summary>
        /// Gets a boolean value indicating whether failure of weak cancellativity has been detected.
        /// </summary>
        /// <remarks>
        /// <para>If the analysis settings dictate that weak cancellativity failure should not be
        /// detected, then this value is <see langword="false"/>.</para>
        /// </remarks>
        public bool WeakCancellativityFailureDetected => CancellativityFailuresDetected.HasFlag(CancellativityTypes.WeakCancellativity);

        /// <summary>
        /// Gets a boolean value indicating whether a path exceeding the max length of the analysis
        /// settings was encountered.
        /// </summary>
        public bool TooLongPathEncountered { get; private set; }

        /// <summary>
        /// Gets a collection of maximal nonzero-equivalent paths, one for each maximal nonzero
        /// equivalence class.
        /// </summary>
        public IEnumerable<Path<TVertex>> MaximalNonzeroEquivalenceClassRepresentatives { get; private set; }

        /// <summary>
        /// Gets the longest path encountered.
        /// </summary>
        /// <remarks>
        /// <para>This value is never <see langword="null"/>, not even if the computation ended
        /// early because of seeming non-admissibility.</para>
        /// </remarks>
        public Path<TVertex> LongestPathEncountered { get; private set; }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MaximalNonzeroEquivalenceClassRepresentativesResults{TVertex}"/> class.
        /// </summary>
        /// <param name="cancellativityFailuresDetected">A <see cref="CancellativityTypes"/> value
        /// indicating which types of cancellativity was detected to fail.</param>
        /// <param name="tooLongPathEncountered">A boolean value indicating whether a path
        /// exceeding the max length of the analysis settings was encountered.</param>
        /// <param name="maximalNonzeroEquivalenceClassRepresentatives">A collection of maximal
        /// nonzero-equivalent paths, one for each maximal nonzero equivalence class.</param>
        /// <param name="longestPathEncountered">The longest path encountered.</param>
        public MaximalNonzeroEquivalenceClassRepresentativesResults(
            CancellativityTypes cancellativityFailuresDetected,
            bool tooLongPathEncountered,
            IEnumerable<Path<TVertex>> maximalNonzeroEquivalenceClassRepresentatives,
            Path<TVertex> longestPathEncountered)
        {
            CancellativityFailuresDetected = cancellativityFailuresDetected;
            TooLongPathEncountered = tooLongPathEncountered;
            MaximalNonzeroEquivalenceClassRepresentatives = maximalNonzeroEquivalenceClassRepresentatives
                ?? throw new ArgumentNullException(nameof(maximalNonzeroEquivalenceClassRepresentatives));
            LongestPathEncountered = longestPathEncountered ?? throw new ArgumentNullException(nameof(longestPathEncountered));
        }
    }
}
