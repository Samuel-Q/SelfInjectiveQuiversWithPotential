using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    public class AnalysisStateForSingleStartingVertexOld<TVertex> where TVertex : IEquatable<TVertex>, IComparable<TVertex>
    {
        public Stack<SearchTreeNodeOld<TVertex>> Stack { get; private set; }
        /// <summary>
        /// Gets a dummy node for the zero path.
        /// </summary>
        /// <remarks>The property value is a dummy node in the sense that all its members are bogus
        /// and should not be used.</remarks>
        internal SearchTreeNodeOld<TVertex> ZeroDummyNode { get; set; }

        public SearchTreeNodeOld<TVertex> SearchTree { get; internal set; }

        internal List<SearchTreeNodeOld<TVertex>> maximalPathRepresentatives;

        public IEnumerable<SearchTreeNodeOld<TVertex>> MaximalPathRepresentatives { get => maximalPathRepresentatives; }

        public DisjointSets<SearchTreeNodeOld<TVertex>> EquivalenceClasses { get; private set; }

        internal AnalysisStateForSingleStartingVertexOld(TVertex startingVertex)
        {
            Stack = new Stack<SearchTreeNodeOld<TVertex>>();
            ZeroDummyNode = new SearchTreeNodeOld<TVertex>(null, default, null);
            var root = new SearchTreeNodeOld<TVertex>(null, startingVertex, null);
            SearchTree = root;
            this.maximalPathRepresentatives = new List<SearchTreeNodeOld<TVertex>>();
            EquivalenceClasses = new DisjointSets<SearchTreeNodeOld<TVertex>>();
            EquivalenceClasses.MakeSet(ZeroDummyNode);
            EquivalenceClasses.MakeSet(root);
            Stack.Push(root);
        }

        public bool NodeIsZeroEquivalent(SearchTreeNodeOld<TVertex> node)
        {
            var equivClass = EquivalenceClasses.FindSet(node);
            var zeroClass = EquivalenceClasses.FindSet(ZeroDummyNode);
            return equivClass.Equals(zeroClass);
        }

        public SearchTreeNodeOld<TVertex> InsertChildNode(SearchTreeNodeOld<TVertex> parent, TVertex vertex, SearchTreeNodeOld<TVertex> origin)
        {
            var node = new SearchTreeNodeOld<TVertex>(parent, vertex, origin);
            parent.children.Add(vertex, node);
            EquivalenceClasses.MakeSet(node);
            return node;
        }

        public SearchTreeNodeOld<TVertex> GetInsertChildNode(SearchTreeNodeOld<TVertex> parent, TVertex vertex, SearchTreeNodeOld<TVertex> origin)
        {
            if (!parent.Children.ContainsKey(vertex)) InsertChildNode(parent, vertex, origin);
            return parent.Children[vertex];
        }
    }
}
