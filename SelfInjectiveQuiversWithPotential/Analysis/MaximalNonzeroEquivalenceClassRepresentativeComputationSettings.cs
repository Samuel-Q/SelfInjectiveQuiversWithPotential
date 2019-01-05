using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    /// <summary>
    /// This class represents settings for the analysis/computation done by an
    /// <see cref="IMaximalNonzeroEquivalenceClassRepresentativeComputer"/>.
    /// </summary>
    public class MaximalNonzeroEquivalenceClassRepresentativeComputationSettings : AnalysisSettings
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MaximalNonzeroEquivalenceClassRepresentativeComputationSettings"/>
        /// class.
        /// </summary>
        /// <param name="cancellativityFailureDetection">A
        /// <see cref="CancellativityTypes"/> value indicating which types of cancellativity to
        /// detect failures of.</param>
        public MaximalNonzeroEquivalenceClassRepresentativeComputationSettings(CancellativityTypes cancellativityFailureDetection)
            : this(cancellativityFailureDetection, maxPathLength: -1, EarlyTerminationConditions.None)
        { }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="MaximalNonzeroEquivalenceClassRepresentativeComputationSettings"/> class
        /// using a specified maximum path length and early termination condition.
        /// </summary>
        /// <param name="cancellativityFailureDetection">A
        /// <see cref="CancellativityTypes"/> value indicating which types of cancellativity to
        /// detect failures of.</param>
        /// <param name="maxPathLength">The maximum path length in arrows (i.e., the value such
        /// that if a path of length greater than the value is encountered during the analysis, the
        /// analysis is to be aborted), or a negative value if no maximum path length is to be
        /// used.</param>
        /// <param name="earlyTerminationConditions">A value of the
        /// <see cref="Analysis.EarlyTerminationConditions"/> enum indicating the conditions on
        /// which the analysis should terminate early.</param>
        public MaximalNonzeroEquivalenceClassRepresentativeComputationSettings(
            CancellativityTypes cancellativityFailureDetection,
            int maxPathLength,
            EarlyTerminationConditions earlyTerminationConditions)
            : base(cancellativityFailureDetection, maxPathLength, earlyTerminationConditions)
        { }
    }
}
