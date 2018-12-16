﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    /// <summary>
    /// Classes implementing this interface represent the result of an analysis of a quiver with potential.
    /// </summary>
    /// <seealso cref="IQPAnalyzer"/>
    /// <remarks>The counterpart for quivers in the plane is <see cref="Plane.IQuiverInPlaneAnalysisResults{TVertex}"/></remarks>
    public interface IQPAnalysisResults<TVertex> where TVertex : IEquatable<TVertex>, IComparable<TVertex>
    {
        /// <summary>
        /// Gets the main result of the analysis (whether it was successful or why it failed).
        /// </summary>
        QPAnalysisMainResult MainResult { get; }

        /// <summary>
        /// Gets a dictionary mapping every vertex of the quiver to a collection of representatives
        /// of all maximal non-zero equivalence classes of paths starting at the vertex if the
        /// analysis was successful; otherwise, gets <see langword="null"/>.
        /// </summary>
        IReadOnlyDictionary<TVertex, IEnumerable<Path<TVertex>>> MaximalPathRepresentatives { get; }

        /// <summary>
        /// Gets the Nakayama permutation for the QP if the analysis was successful and the QP has
        /// a Nakayama permutation; gets <see langword="null"/> otherwise.
        /// </summary>
        NakayamaPermutation<TVertex> NakayamaPermutation { get; }

        /// <summary>
        /// Gets a path whose length is maximal among the paths encountered during the analysis.
        /// </summary>
        Path<TVertex> LongestPathEncountered { get; }
    }
}
