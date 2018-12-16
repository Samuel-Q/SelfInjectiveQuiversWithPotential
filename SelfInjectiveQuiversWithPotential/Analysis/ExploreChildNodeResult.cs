using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    internal enum ExploreChildNodeResult
    {
        /// <summary>
        /// Indicates that the child node was explored successfully and that the child node is
        /// zero equivalent.
        /// </summary>
        ZeroEquivalent,

        /// <summary>
        /// Indicates that the child node was explored successfully and that the child node is
        /// not zero equivalent.
        /// </summary>
        NotZeroEquivalent,

        /// <summary>
        /// Indicates that the exploration was aborted because the QP was detected to be
        /// non-cancellative.
        /// </summary>
        NonCancellativityDetected,

        /// <summary>
        /// Indicates that the exploration was aborted because a too long path was encountered.
        /// </summary>
        TooLongPath
    }
}
