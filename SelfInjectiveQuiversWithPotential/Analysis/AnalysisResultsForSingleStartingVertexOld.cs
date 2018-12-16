using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    public class AnalysisResultsForSingleStartingVertexOld<TVertex> where TVertex : IEquatable<TVertex>, IComparable<TVertex>
    {
        /// <summary>
        /// Gets a dummy node for the zero path.
        /// </summary>
        /// <remarks>The property value is a dummy node in the sense that all its members are bogus
        /// and should not be used.</remarks>
        internal SearchTreeNodeOld<TVertex> ZeroDummyNode { get; set; }

        public SearchTreeNodeOld<TVertex> SearchTree { get; private set; }

        public IEnumerable<SearchTreeNodeOld<TVertex>> MaximalPathRepresentatives { get; private set; }

        public DisjointSets<SearchTreeNodeOld<TVertex>> EquivalenceClasses { get; private set; }

        public bool NonCancellativityDetected { get; private set; }

        public bool TooLongPathEncountered { get; private set; }

        internal AnalysisResultsForSingleStartingVertexOld(
            SearchTreeNodeOld<TVertex> zeroDummyNode,
            SearchTreeNodeOld<TVertex> searchTree,
            IEnumerable<SearchTreeNodeOld<TVertex>> maximalPathRepresentatives,
            DisjointSets<SearchTreeNodeOld<TVertex>> equivalenceClasses,
            bool nonCancellativityDetected,
            bool tooLongPathEncountered)
        {
            ZeroDummyNode = zeroDummyNode;
            SearchTree = searchTree;
            MaximalPathRepresentatives = maximalPathRepresentatives;
            EquivalenceClasses = equivalenceClasses;
            NonCancellativityDetected = nonCancellativityDetected;
            TooLongPathEncountered = tooLongPathEncountered;
        }

        internal AnalysisResultsForSingleStartingVertexOld(
            AnalysisStateForSingleStartingVertexOld<TVertex> state,
            bool nonCancellativityDetected,
            bool tooLongPathEncountered)
        {
            ZeroDummyNode = state.ZeroDummyNode;
            SearchTree = state.SearchTree;
            MaximalPathRepresentatives = state.MaximalPathRepresentatives;
            EquivalenceClasses = state.EquivalenceClasses;
            NonCancellativityDetected = nonCancellativityDetected;
            TooLongPathEncountered = tooLongPathEncountered;
        }
    }
}
