using System;
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
        public SemimonomialUnboundQuiverAnalysisSettings(bool detectNonCancellativity)
            : this(detectNonCancellativity, maxPathLength: -1)
        { }

        public SemimonomialUnboundQuiverAnalysisSettings(bool detectNonCancellativity, int maxPathLength) : base(detectNonCancellativity, maxPathLength)
        { }
    }
}
