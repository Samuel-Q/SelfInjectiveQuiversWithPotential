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
    public enum SemimonomialUnboundQuiverAnalysisMainResults
    {
        None = 0,

        /// <summary>
        /// Indicates that the analysis was successful in that it was not terminated early.
        /// </summary>
        Success = 0x01,

        /// <summary>
        /// Indicates that the analysis was aborted (because an infinite loop was detected or so).
        /// </summary>
        Aborted = 0x02,

        /// <summary>
        /// Indicates that the analysis was cancelled (by the user, typically)
        /// </summary>
        Cancelled = 0x04,

        /// <summary>
        /// Indicates that the analysis showed that the semimonomial unbound quiver is not
        /// cancellative.
        /// </summary>
        NotCancellative = 0x08,

        /// <summary>
        /// Indicates that the analysis showed that the semimonomial unbound quiver is not weakly
        /// cancellative.
        /// </summary>
        NotWeaklyCancellative = 0x10,

        /// <summary>
        /// Indicates that the analysis showed that some starting point had more than one maximal
        /// nonzero equivalence class of paths (which implies that the socle of the corresponding
        /// indecomposable projective module is multi-dimensional, at least for an admissible ideal).
        /// </summary>
        /// <remarks>
        /// <para>This slightly more verbose name might be preferable to
        /// &quot;MultiDimensionalSocle&quot;, because having multiple maximal nonzero classes is
        /// a possibly strictly stronger condition if the bound quiver is not weakly cancellative.
        /// This is not to mention what could happen if the ideal is not admissible.</para>
        /// </remarks>
        MultipleMaximalNonzeroClasses = 0x20,

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
        NonInjectiveTentativeNakayamaPermutation = 0x40
    }

    public static class SemimonomialUnboundQuiverAnalysisMainResultsExtensions
    {
        /// <summary>
        /// Returns a boolean value indicating whether the specified results indicate that the
        /// unbound quiver is self-injective.
        /// </summary>
        /// <param name="results">The results.</param>
        /// <returns><see langword="true"/> if <paramref name="results"/> indicates that the unbound
        /// quiver is self-injective. <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>If the analysis settings dictate that weak cancellativity should not be checked,
        /// the bound quiver could fail to be weakly cancellative without <paramref name="results"/>
        /// indicating this. In that case, the bound quiver algebra could fail to be self-injective
        /// but this method could still return <see langword="true"/>.</para>
        /// </remarks>
        public static bool IndicatesSelfInjectivity(this SemimonomialUnboundQuiverAnalysisMainResults results)
        {
            return results.HasFlag(SemimonomialUnboundQuiverAnalysisMainResults.Success)
                && !results.HasFlag(SemimonomialUnboundQuiverAnalysisMainResults.NotWeaklyCancellative)
                && !results.HasFlag(SemimonomialUnboundQuiverAnalysisMainResults.MultipleMaximalNonzeroClasses)
                && !results.HasFlag(SemimonomialUnboundQuiverAnalysisMainResults.NonInjectiveTentativeNakayamaPermutation);
        }

        /// <summary>
        /// Returns a boolean value indicating whether the specified results indicate that the
        /// unbound quiver is self-injective.
        /// </summary>
        /// <param name="results">The results.</param>
        /// <returns><see langword="true"/> if <paramref name="results"/> indicates that the unbound
        /// quiver is self-injective. <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para>This method uses strong cancellativity instead of weak cancellativity to ensure
        /// that the socles of the indecomposable projective modules are spanned by the
        /// corresponding maximal nonzero equivalence classes.</para>
        /// <para>If the analysis settings dictate that cancellativity should not be checked,
        /// the bound quiver could fail to be cancellative without <paramref name="results"/>
        /// indicating this. In that case, the bound quiver algebra could fail to be self-injective
        /// but this method could still return <see langword="true"/>.</para>
        /// </remarks>
        public static bool IndicatesSelfInjectivityUsingStrongCancellativity(this SemimonomialUnboundQuiverAnalysisMainResults results)
        {
            return results.HasFlag(SemimonomialUnboundQuiverAnalysisMainResults.Success)
                && !results.HasFlag(SemimonomialUnboundQuiverAnalysisMainResults.NotCancellative)
                && !results.HasFlag(SemimonomialUnboundQuiverAnalysisMainResults.MultipleMaximalNonzeroClasses)
                && !results.HasFlag(SemimonomialUnboundQuiverAnalysisMainResults.NonInjectiveTentativeNakayamaPermutation);
        }
    }
}
