﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    /// <summary>
    /// This class encapsulates the logic for creating instances of various types extending the
    /// <see cref="AnalysisSettings"/>.
    /// </summary>
    public static class AnalysisSettingsFactory
    {
        public static MaximalNonzeroEquivalenceClassRepresentativeComputationSettings CreateMaximalNonzeroEquivalenceClassRepresentativeComputationSettings(SemimonomialUnboundQuiverAnalysisSettings settings)
        {
            if (settings is null) throw new ArgumentNullException(nameof(settings));

            return new MaximalNonzeroEquivalenceClassRepresentativeComputationSettings(
                settings.DetectNonCancellativity,
                settings.MaxPathLength,
                settings.EarlyTerminationCondition);
        }

        public static SemimonomialUnboundQuiverAnalysisSettings CreateSemimonomialUnboundQuiverAnalysisSettings(QPAnalysisSettings settings)
        {
            if (settings is null) throw new ArgumentNullException(nameof(settings));

            return new SemimonomialUnboundQuiverAnalysisSettings(
                settings.DetectNonCancellativity,
                settings.MaxPathLength,
                settings.EarlyTerminationCondition);
        }

        public static QPAnalysisSettings CreateQPAnalysisSettings(QuiverInPlaneAnalysisSettings settings)
        {
            if (settings is null) throw new ArgumentNullException(nameof(settings));

            return new QPAnalysisSettings(
                settings.DetectNonCancellativity,
                settings.MaxPathLength,
                settings.EarlyTerminationCondition);
        }
    }
}
