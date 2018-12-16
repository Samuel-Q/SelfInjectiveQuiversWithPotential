using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    internal enum ExploreChildNodesResult
    {
        /// <summary>
        /// Indicates that the exploration was successful and that the node has
        /// a nonzero extension.
        /// </summary>
        PathHasNonzeroExtension,

        /// <summary>
        /// Indicates that the exploration was successful and that the node has
        /// no nonzero extension.
        /// </summary>
        PathHasNoNonzeroExtension,

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
