using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    /// <summary>
    /// Defines the conditions upon which early termination of the analysis (of a quiver or so) is
    /// an option.
    /// </summary>
    [Flags]
    public enum EarlyTerminationCondition
    {
        /// <summary>
        /// A value corresponding to no conditions.
        /// </summary>
        None = 0,

        /// <summary>
        /// The condition that weak cancellativity has been found to fail.
        /// </summary>
        WeakCancellativityFails = 0x01,

        /// <summary>
        /// The condition that cancellativity has been found to fail.
        /// </summary>
        CancellativityFails = 0x02,

        /// <summary>
        /// The condition that a multi-dimensional socle has been found.
        /// </summary>
        MultiDimensionalSocle = 0x04,

        /// <summary>
        /// The condition that the tentative Nakayama permutation is found not to be a permutation
        /// (i.e., non-injective).
        /// </summary>
        PermutationFails = 0x08,
    }
}
