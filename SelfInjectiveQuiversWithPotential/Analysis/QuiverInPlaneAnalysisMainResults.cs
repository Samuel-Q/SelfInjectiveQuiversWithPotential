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
    /// the union of <see cref="QPExtractionResult"/> and <see cref="QPAnalysisMainResults"/>.</remarks>
    [Flags]
    public enum QuiverInPlaneAnalysisMainResults
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
        /// The analysis indicates that the quiver in plane induces a QP that is not weakly cancellative.
        /// </summary>
        QPIsNotWeaklyCancellative = 0x100,

        /// <summary>
        /// Indicates that the analysis showed that some starting point had more than one maximal
        /// nonzero equivalence class of paths (which implies that the socle of the corresponding
        /// indecomposable projective module is multi-dimensional, at least for an admissible ideal).
        /// </summary>
        /// <remarks>
        /// <para>This slightly more verbose name might be preferable to
        /// &quot;MultiDimensionalSocle&quot;, because having multiple maximal nonzero classes is
        /// a possibly strictly stronger condition if the underlying bound quiver is not weakly
        /// cancellative. This is not to mention what could happen if the ideal is not admissible.</para>
        /// </remarks>
        SomeVertexHasMultipleMaximalNonzeroClasses = 0x200,

        /// <summary>
        /// Indicates that the analysis showed that the tentative Nakayama permutation (mapping a
        /// vertex to the ending point of a representative path of the unique maximal nonzero
        /// equivalence class) fails to be injective.
        /// </summary>
        /// <remarks>
        /// <para>This verbose name might be preferable to &quot;PermutationFails&quot;, because it
        /// clearly specifies what permutation fails and unambiguously specifies how it fails
        /// (failure of being a single-valued function might seem to fall under PermutationFails,
        /// but this problem is captured by <see cref="MultipleMaximalNonzeroClasses"/>).</para>
        /// </remarks>
        TentativeNakayamaPermutationIsNonInjective = 0x400
    }

    public static class QuiverInPlaneAnalysisMainResultsExtensions
    {
        /// <summary>
        /// Returns a boolean value indicating whether the specified results indicate that the QP
        /// is self-injective.
        /// </summary>
        /// <param name="results">The results.</param>
        /// <returns><see langword="true"/> if <paramref name="results"/> indicates that the QP is
        /// self-injective. <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>If the analysis settings dictate that weak cancellativity should not be checked,
        /// the underlying bound quiver could fail to be weakly cancellative without
        /// <paramref name="results"/> indicating this. In that case, the bound quiver algebra could
        /// fail to be self-injective but this method could still return <see langword="true"/>.</para>
        /// </remarks>
        public static bool IndicatesSelfInjectivity(this QuiverInPlaneAnalysisMainResults results)
        {
            return results.HasFlag(QuiverInPlaneAnalysisMainResults.Success)
                && !results.HasFlag(QuiverInPlaneAnalysisMainResults.QPIsNotWeaklyCancellative)
                && !results.HasFlag(QuiverInPlaneAnalysisMainResults.SomeVertexHasMultipleMaximalNonzeroClasses)
                && !results.HasFlag(QuiverInPlaneAnalysisMainResults.TentativeNakayamaPermutationIsNonInjective);
        }

        /// <summary>
        /// Returns a boolean value indicating whether the specified results indicate that the QP
        /// is self-injective.
        /// </summary>
        /// <param name="results">The results.</param>
        /// <returns><see langword="true"/> if <paramref name="results"/> indicates that the QP is
        /// self-injective. <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>This method uses strong cancellativity instead of weak cancellativity to ensure
        /// that the socles of the indecomposable projective modules are spanned by the
        /// corresponding maximal nonzero equivalence classes.</para>
        /// <para>If the analysis settings dictate that cancellativity should not be checked,
        /// the underlying bound quiver could fail to be cancellative without
        /// <paramref name="results"/> indicating this. In that case, the bound quiver algebra could
        /// fail to be self-injective but this method could still return <see langword="true"/>.</para>
        /// </remarks>
        public static bool IndicatesSelfInjectivityUsingStrongCancellativity(this QuiverInPlaneAnalysisMainResults results)
        {
            return results.HasFlag(QuiverInPlaneAnalysisMainResults.Success)
                && !results.HasFlag(QuiverInPlaneAnalysisMainResults.QPIsNotCancellative)
                && !results.HasFlag(QuiverInPlaneAnalysisMainResults.SomeVertexHasMultipleMaximalNonzeroClasses)
                && !results.HasFlag(QuiverInPlaneAnalysisMainResults.TentativeNakayamaPermutationIsNonInjective);
        }
    }
}
