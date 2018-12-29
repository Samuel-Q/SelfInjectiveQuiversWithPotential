using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    /// <summary>
    /// Defines the &quot;main results&quot; for the analysis of a semimonomial unbound quiver
    /// (a sort of status code for the analysis, if you will).
    /// </summary>
    [Flags]
    public enum SemimonomialUnboundQuiverAnalysisMainResult
    {
        None = 0,

        /// <summary>
        /// Indicates that the analysis was successful (but the semimonomial unbound quiver was not
        /// necessarily self-injective).
        /// </summary>
        Success = 1,

        /// <summary>
        /// Indicates that the analysis was aborted (because an infinite loop was detected or so).
        /// </summary>
        Aborted = 2,

        /// <summary>
        /// Indicates that the analysis was cancelled (by the user, typically)
        /// </summary>
        Cancelled = 4,

        /// <summary>
        /// Indicates that the analysis showed that the semimonomial unbound quiver is not
        /// cancellative.
        /// </summary>
        NotCancellative = 8,

        /// <summary>
        /// Indicates that the analysis showed that the semimonomial unbound quiver is
        /// self-injective.
        /// </summary>
        SelfInjective = 16
    }
}
