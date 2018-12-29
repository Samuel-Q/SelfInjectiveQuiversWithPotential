using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    /// <summary>
    /// Defines the &quot;main results&quot; for the analysis of a quiver in the plane (a sort of
    /// status code for the analysis, if you will).
    /// </summary>
    /// <remarks>Because the analysis of quivers in the plane is just QP extraction
    /// (see <see cref="QPExtractor"/>) followed by analysis of the QP, this enum is essentially
    /// the union of <see cref="QPExtractionResult"/> and <see cref="QPAnalysisMainResult"/>.</remarks>
    [Flags]
    public enum QuiverInPlaneAnalysisMainResult
    {
        None = 0,

        /// <summary>
        /// The analysis was successful (but the quiver in plane does not necessarily induce a QP
        /// that is self-injective).
        /// </summary>
        Success = 0x01,

        /// <summary>
        /// The quiver has loops.
        /// </summary>
        QuiverHasLoops = 0x02,

        /// <summary>
        /// The quiver has anti-parallel arrows.
        /// </summary>
        QuiverHasAntiParallelArrows = 0x04,

        /// <summary>
        /// The quiver is not plane.
        /// </summary>
        QuiverIsNotPlane = 0x08,

        /// <summary>
        /// The quiver has a face whose bounding arrows do not form a directed cycle.
        /// </summary>
        QuiverHasFaceWithInconsistentOrientation = 0x10,

        /// <summary>
        /// The analysis of the induced QP was aborted.
        /// </summary>
        QPAnalysisAborted = 0x20,

        /// <summary>
        /// The analysis of the induced QP was cancelled (by the user or so)
        /// </summary>
        QPAnalysisCancelled = 0x40,

        /// <summary>
        /// The analysis indicates that the quiver in plane induces a QP that is not cancellative.
        /// </summary>
        QPIsNotCancellative = 0x80,

        /// <summary>
        /// The analysis indicates that the quiver in plane induces a QP that is self-injective.
        /// </summary>
        QPIsSelfInjective = 0x100
    }
}
