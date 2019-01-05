using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    /// <summary>
    /// This abstract class provides a base class for settings of analyses of various
    /// &quot;gadgets&quot; (e.g, of semimonomial unbound quivers, QPs, and quivers in the plane).
    /// </summary>
    public abstract class AnalysisSettings
    {
        /// <summary>
        /// Gets a value indicating which types of cancellativity to detect failure of.
        /// </summary>
        public CancellativityTypes CancellativityFailureDetection { get; private set; }

        /// <summary>
        /// Gets a boolean value indicating whether to detect failure of (strong) cancellativity.
        /// </summary>
        public bool DetectCancellativityFailure => CancellativityFailureDetection.HasFlag(CancellativityTypes.Cancellativity);

        /// <summary>
        /// Gets a boolean value indicating whether to detect failure of weak cancellativity.
        /// </summary>
        public bool DetectWeakCancellativityFailure => CancellativityFailureDetection.HasFlag(CancellativityTypes.WeakCancellativity);

        /// <summary>
        /// Gets a boolean value indicating whether to use a maximum path length (i.e., whether to
        /// abort if a too long path is encountered during the analysis).
        /// </summary>
        public bool UseMaxLength { get => MaxPathLength >= 0; }

        /// <summary>
        /// Gets the maximum path length in arrows (i.e., the value such that if a path of length
        /// greater than the value is encountered during the analysis, the analysis is to be aborted),
        /// or a negative value if no maximum path length is to be used.
        /// </summary>
        public int MaxPathLength { get; private set; }

        /// <summary>
        /// Gets a value of the <see cref="Analysis.EarlyTerminationConditions"/> enum indicating
        /// the conditions on which the analysis should terminate early.
        /// </summary>
        public EarlyTerminationConditions EarlyTerminationConditions { get; private set; }

        /// <summary>
        /// Gets a boolean value indicating whether to terminate early if (strong) cancellativity
        /// fails.
        /// </summary>
        public bool TerminateEarlyIfCancellativityFails => EarlyTerminationConditions.HasFlag(EarlyTerminationConditions.CancellativityFails);

        /// <summary>
        /// Gets a boolean value indicating whether to terminate early if weak cancellativity
        /// fails.
        /// </summary>
        public bool TerminateEarlyIfWeakCancellativityFails => EarlyTerminationConditions.HasFlag(EarlyTerminationConditions.WeakCancellativityFails);

        /// <summary>
        /// Gets a boolean value indicating whether to terminate early if the socle of one of the
        /// indecomposable projective modules is multi-dimensional.
        /// </summary>
        public bool TerminateEarlyOnMultiDimensionalSocle => EarlyTerminationConditions.HasFlag(EarlyTerminationConditions.MultiDimensionalSocle);

        /// <summary>
        /// Gets a boolean value indicating whether to terminate early the tentative Nakayama
        /// permutation is found not to be a permutation (i.e., non-injective).
        /// </summary>
        public bool TerminateEarlyIfNakayamaPermutationFails => EarlyTerminationConditions.HasFlag(EarlyTerminationConditions.PermutationFails);

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalysisSettings"/> class.
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
        protected AnalysisSettings(
            CancellativityTypes cancellativityFailureDetection,
            int maxPathLength,
            EarlyTerminationConditions earlyTerminationConditions)
        {
            CancellativityFailureDetection = cancellativityFailureDetection;
            MaxPathLength = maxPathLength;
            EarlyTerminationConditions = earlyTerminationConditions;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append($"Cancellativity failure detection: {CancellativityFailureDetection}. ");
            if (UseMaxLength) builder.Append($"Max path length: {MaxPathLength}. ");
            else builder.Append("No max path length. ");
            builder.Append($"Early termination condition: {EarlyTerminationConditions}.");

            return builder.ToString();
        }
    }
}
