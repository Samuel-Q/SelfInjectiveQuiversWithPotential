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
        /// Gets a boolean value indicating whether non-cancellativity of the gadget should be
        /// detected.
        /// </summary>
        public bool DetectNonCancellativity { get; private set; }

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
        /// Gets a value of the <see cref="Analysis.EarlyTerminationCondition"/> enum indicating
        /// the conditions on which the analysis should terminate early.
        /// </summary>
        public EarlyTerminationCondition EarlyTerminationCondition { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalysisSettings"/> class.
        /// </summary>
        /// <param name="detectNonCancellativity">A boolean value indicating whether
        /// non-cancellativity of the gadget should be detected.</param>
        /// <param name="maxPathLength">The maximum path length in arrows (i.e., the value such
        /// that if a path of length greater than the value is encountered during the analysis, the
        /// analysis is to be aborted), or a negative value if no maximum path length is to be
        /// used.</param>
        /// <param name="earlyTerminationCondition">A value of the
        /// <see cref="Analysis.EarlyTerminationCondition"/> enum indicating the conditions on
        /// which the analysis should terminate early.</param>
        protected AnalysisSettings(
            bool detectNonCancellativity,
            int maxPathLength,
            EarlyTerminationCondition earlyTerminationCondition)
        {
            DetectNonCancellativity = detectNonCancellativity;
            MaxPathLength = maxPathLength;
            EarlyTerminationCondition = earlyTerminationCondition;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append($"Detect non-cancellativity: {DetectNonCancellativity}. ");
            if (UseMaxLength) builder.Append($"Max path length: {MaxPathLength}. ");
            else builder.Append("No max path length. ");
            builder.Append($"Early termination condition: {EarlyTerminationCondition}.");

            return builder.ToString();
        }
    }
}
