﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    /// <summary>
    /// This class represents settings for the analysis done by an <see cref="ISemimonomialUnboundQuiverAnalyzer"/>.
    /// </summary>
    public class SemimonomialUnboundQuiverAnalysisSettings : AnalysisSettings
    {
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SemimonomialUnboundQuiverAnalysisSettings"/> class.
        /// </summary>
        /// <param name="detectNonCancellativity">A boolean value indicating whether
        /// non-cancellativity of the unbound quiver should be detected.</param>
        public SemimonomialUnboundQuiverAnalysisSettings(bool detectNonCancellativity)
            : this(detectNonCancellativity, maxPathLength: -1, EarlyTerminationCondition.None)
        { }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="SemimonomialUnboundQuiverAnalysisSettings"/> class using a specified maximum
        /// path length and early termination condition.
        /// </summary>
        /// <param name="detectNonCancellativity">A boolean value indicating whether
        /// non-cancellativity of the unbound quiver should be detected.</param>
        /// <param name="maxPathLength">The maximum path length in arrows (i.e., the value such
        /// that if a path of length greater than the value is encountered during the analysis, the
        /// analysis is to be aborted), or a negative value if no maximum path length is to be
        /// used.</param>
        /// <param name="earlyTerminationCondition">A value of the
        /// <see cref="Analysis.EarlyTerminationCondition"/> enum indicating the conditions on
        /// which the analysis should terminate early.</param>
        public SemimonomialUnboundQuiverAnalysisSettings(
            bool detectNonCancellativity,
            int maxPathLength,
            EarlyTerminationCondition earlyTerminationCondition)
            : base(detectNonCancellativity, maxPathLength, earlyTerminationCondition)
        { }
    }
}
