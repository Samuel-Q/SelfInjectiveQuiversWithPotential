using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    /// <summary>
    /// This class represents the results of an analysis by
    /// <see cref="MaximalNonzeroEquivalenceClassRepresentativeComputer"/>.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertices in the quiver.</typeparam>
    /// <remarks>
    /// <para>This class is essentially just a more detailed version of
    /// <see cref="MaximalNonzeroEquivalenceClassRepresentativesResults{TVertex}"/>.</para>
    /// </remarks>
    public class AnalysisResultsForSingleStartingVertex<TVertex> where TVertex : IEquatable<TVertex>, IComparable<TVertex>
    {
        /// <summary>
        /// Gets a dummy node for the zero path.
        /// </summary>
        /// <remarks>The property value is a dummy node in the sense that all its members are bogus
        /// and should not be used.</remarks>
        internal SearchTreeNode<TVertex> ZeroDummyNode { get; set; }

        public SearchTreeNode<TVertex> SearchTree { get; private set; }

        public IEnumerable<SearchTreeNode<TVertex>> MaximalPathRepresentatives { get; private set; }

        public DisjointSets<SearchTreeNode<TVertex>> EquivalenceClasses { get; private set; }

        /// <summary>
        /// Gets a <see cref="CancellativityTypes"/> value indicating which types of cancellativity
        /// was detected to fail.
        /// </summary>
        public CancellativityTypes CancellativityFailuresDetected { get; private set; }

        /// <summary>
        /// Gets a boolean value indicating whether failure of cancellativity has been detected.
        /// </summary>
        /// <remarks>
        /// <para>If the analysis settings dictate that cancellativity failure should not be
        /// detected, then this value is <see langword="false"/>.</para>
        /// </remarks>
        public bool CancellativityFailureDetected => CancellativityFailuresDetected.HasFlag(CancellativityTypes.Cancellativity);

        /// <summary>
        /// Gets a boolean value indicating whether failure of weak cancellativity has been detected.
        /// </summary>
        /// <remarks>
        /// <para>If the analysis settings dictate that weak cancellativity failure should not be
        /// detected, then this value is <see langword="false"/>.</para>
        /// </remarks>
        public bool WeakCancellativityFailureDetected => CancellativityFailuresDetected.HasFlag(CancellativityTypes.WeakCancellativity);

        /// <summary>
        /// Gets a boolean value indicating whether a path exceeding the max length of the analysis
        /// settings was encountered.
        /// </summary>
        public bool TooLongPathEncountered { get; private set; }

        /// <summary>
        /// Gets the longest path encountered.
        /// </summary>
        public Path<TVertex> LongestPathEncountered { get; private set; }

        internal AnalysisResultsForSingleStartingVertex(
            SearchTreeNode<TVertex> zeroDummyNode,
            SearchTreeNode<TVertex> searchTree,
            IEnumerable<SearchTreeNode<TVertex>> maximalPathRepresentatives,
            DisjointSets<SearchTreeNode<TVertex>> equivalenceClasses,
            CancellativityTypes cancellativityFailuresDetected,
            bool tooLongPathEncountered,
            Path<TVertex> longestPathEncountered)
        {
            ZeroDummyNode = zeroDummyNode;
            SearchTree = searchTree;
            MaximalPathRepresentatives = maximalPathRepresentatives;
            EquivalenceClasses = equivalenceClasses;
            CancellativityFailuresDetected = cancellativityFailuresDetected;
            TooLongPathEncountered = tooLongPathEncountered;
            LongestPathEncountered = longestPathEncountered;
        }

        internal AnalysisResultsForSingleStartingVertex(
            AnalysisStateForSingleStartingVertex<TVertex> state,
            CancellativityTypes cancellativityFailuresDetected)
            : this(
                  state.ZeroDummyNode,
                  state.SearchTree,
                  state.MaximalPathRepresentatives,
                  state.EquivalenceClasses,
                  cancellativityFailuresDetected,
                  state.TooLongPathEncountered,
                  state.LongestPathEncounteredNode)
        { }
    }
}
