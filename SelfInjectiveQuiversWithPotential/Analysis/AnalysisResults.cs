using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    /// <summary>
    /// This class represents the results of an analysis of something &quot;quivery&quot;.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertices in the quiver.</typeparam>
    /// <typeparam name="TMainResult">The type of the main result.</typeparam>
    public abstract class AnalysisResults<TVertex, TMainResult>
        where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        where TMainResult : Enum
    {
        /// <summary>
        /// Gets the main result of the analysis (whether it was successful or why it failed).
        /// </summary>
        public TMainResult MainResults { get; protected set; }

        /// <summary>
        /// Gets a dictionary mapping every vertex of the quiver to a collection of representatives
        /// of all maximal non-zero equivalence classes of paths starting at the vertex if the
        /// analysis was successful; otherwise, gets <see langword="null"/>.
        /// </summary>
        public IReadOnlyDictionary<TVertex, IEnumerable<Path<TVertex>>> MaximalPathRepresentatives { get; protected set; }

        /// <summary>
        /// Gets the Nakayama permutation for the analyzed &quot;gadget&quot; if the analysis was
        /// successful and the gadget has a Nakayama permutation; gets <see langword="null"/> otherwise.
        /// </summary>
        public NakayamaPermutation<TVertex> NakayamaPermutation { get; protected set; }

        /// <summary>
        /// Gets a path whose length is maximal among the paths encountered during the analysis.
        /// </summary>
        public Path<TVertex> LongestPathEncountered { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalysisResults{TVertex, TMainResult}"/> class.
        /// </summary>
        /// <param name="mainResult">The main result.</param>
        /// <param name="maximalPathRepresentatives">A dictionary mapping every vertex of the
        /// quiver to a collection of representatives of all maximal non-zero equivalence classes
        /// of paths starting at the vertex, or <see langword="null"/> depending on whether the
        /// analysis was successful.</param>
        /// <param name="nakayamaPermutation">The Nakayama permutation for the analyzed
        /// &quot;gadget&quot; or <see langword="null"/>, depending on whether the analysis was
        /// successful and the gadget has a Nakayama permutation.</param>
        /// <param name="longestPathEncountered">A path of maximal length of the paths encountered
        /// during the analysis, or <see langword="null"/> if no path was encountered (e.g., if a
        /// quiver-in-plane analysis is done on a non-plane quiver, or if the quiver is empty).</param>
        protected AnalysisResults(
            TMainResult mainResult,
            IReadOnlyDictionary<TVertex, IEnumerable<Path<TVertex>>> maximalPathRepresentatives,
            NakayamaPermutation<TVertex> nakayamaPermutation,
            Path<TVertex> longestPathEncountered)
        {
            MainResults = mainResult;
            MaximalPathRepresentatives = maximalPathRepresentatives;
            NakayamaPermutation = nakayamaPermutation;

            LongestPathEncountered = longestPathEncountered;
        }
    }
}
