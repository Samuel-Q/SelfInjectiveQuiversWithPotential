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
    /// <see cref="MaximalNonzeroEquivalenceClassRepresentativesResult{TVertex}"/>.</para>
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
        /// Gets a boolean value indicating whether non-cancellativity has been detected.
        /// </summary>
        /// <remarks>
        /// <para>If the analysis settings dictate that cancellativity should not be detected, then
        /// this value is <see langword="false"/>.</para>
        /// </remarks>
        public bool NonCancellativityDetected { get; private set; }

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
            bool nonCancellativityDetected,
            bool tooLongPathEncountered,
            Path<TVertex> longestPathEncountered)
        {
            ZeroDummyNode = zeroDummyNode;
            SearchTree = searchTree;
            MaximalPathRepresentatives = maximalPathRepresentatives;
            EquivalenceClasses = equivalenceClasses;
            NonCancellativityDetected = nonCancellativityDetected;
            TooLongPathEncountered = tooLongPathEncountered;
            LongestPathEncountered = longestPathEncountered;
        }

        internal AnalysisResultsForSingleStartingVertex(
            AnalysisStateForSingleStartingVertex<TVertex> state,
            bool nonCancellativityDetected)
            : this(
                  state.ZeroDummyNode,
                  state.SearchTree,
                  state.MaximalPathRepresentatives,
                  state.EquivalenceClasses,
                  nonCancellativityDetected,
                  state.TooLongPathEncountered,
                  state.LongestPathEncounteredNode)
        { }
    }
}
