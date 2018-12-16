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
    public class MaximalNonzeroEquivalenceClassRepresentativesResult<TVertex> where TVertex: IEquatable<TVertex>, IComparable<TVertex>
    {
        public bool NonCancellativityDetected { get; private set; }

        public bool TooLongPathEncountered { get; private set; }

        public IEnumerable<Path<TVertex>> MaximalNonzeroEquivalenceClassRepresentatives { get; private set; }

        public Path<TVertex> LongestPathEncountered { get; private set; }

        public MaximalNonzeroEquivalenceClassRepresentativesResult(
            bool nonCancellativityDetected,
            bool tooLongPathEncountered,
            IEnumerable<Path<TVertex>> maximalNonzeroEquivalenceClassRepresentatives,
            Path<TVertex> longestPathEncountered)
        {
            NonCancellativityDetected = nonCancellativityDetected;
            TooLongPathEncountered = tooLongPathEncountered;
            MaximalNonzeroEquivalenceClassRepresentatives = maximalNonzeroEquivalenceClassRepresentatives;
            LongestPathEncountered = longestPathEncountered;
        }
    }
}
