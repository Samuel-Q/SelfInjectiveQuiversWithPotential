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
        public MaximalNonzeroEquivalenceClassRepresentativeComputationSettings(bool detectNonCancellativity)
            : this(detectNonCancellativity, maxPathLength: -1)
        { }

        public MaximalNonzeroEquivalenceClassRepresentativeComputationSettings(bool detectNonCancellativity, int maxPathLength) : base(detectNonCancellativity, maxPathLength)
        { }
    }
}
