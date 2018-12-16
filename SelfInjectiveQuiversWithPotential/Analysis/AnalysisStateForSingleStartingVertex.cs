using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    /// <summary>
    /// This class contains all the state for the computation of maximal nonzero equivalence class
    /// representatives (or &quot;analysis for single starting vertex&quot;) by
    /// <see cref="MaximalNonzeroEquivalenceClassRepresentativeComputer"/>.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertices in the quiver.</typeparam>
    public class AnalysisStateForSingleStartingVertex<TVertex> where TVertex : IEquatable<TVertex>, IComparable<TVertex>
    {
        /// <summary>
        /// Gets the stack (or queue) of paths to process in the tree search.
        /// </summary>
        /// <remarks>
        /// <para>This property satisfies the invariant that the queue only ever contains paths
        /// whose equivalence class has been computed and determined to be nonzero.</para>
        /// <para>Every not zero-equivalent path will be enqueued and processed in the tree search
        /// before the computation terminates.</para>
        /// </remarks>
        public Stack<SearchTreeNode<TVertex>> Stack { get; private set; }

        /// <summary>
        /// Gets a dummy node for the zero path.
        /// </summary>
        /// <remarks>The property value is a dummy node in the sense that all its members are bogus
        /// and should not be used.</remarks>
        internal SearchTreeNode<TVertex> ZeroDummyNode { get; set; }

        /// <summary>
        /// Gets the search tree generated so far.
        /// </summary>
        /// <remarks>
        /// <para>The search tree contains every path encountered in the tree search and in the
        /// equivalence class computations (but not <see cref="ZeroDummyNode"/>).</para>
        /// </remarks>
        public SearchTreeNode<TVertex> SearchTree { get; internal set; }

        internal HashSet<SearchTreeNode<TVertex>> maximalPathRepresentatives;

        /// <summary>
        /// Gets a collection of maximal path representatives found so far.
        /// </summary>
        /// <remarks>
        /// <para>The collection contains only one representative for every maximal equivalence
        /// class found so far.</para>
        /// </remarks>
        public IEnumerable<SearchTreeNode<TVertex>> MaximalPathRepresentatives { get => maximalPathRepresentatives; }

        /// <summary>
        /// Gets a <see cref="DisjointSets{T}"/> containing 
        /// </summary>
        /// <remarks>
        /// <para>The returned <see cref="DisjointSets{T}"/> may contain distinct equivalence
        /// classes for equivalent paths at any point in time during the search, even in the
        /// &quot;outer loop&quot; (the graph search loop). In fact, an equivalence-class-explored
        /// node may have a equivalence-class-non-explored parent. This happens when a path is
        /// transformed (during the equivalence class search) and parent nodes for the transformed
        /// node have to be inserted into the search tree.</para>
        /// <para>What can be said, however, is that when a path (node) is dequeued for exploration
        /// from <see cref="Stack"/> in the outer loop (the graph search loop), then it is
        /// explored. In fact, already when the path (node) is <em>enqueued</em>, it is guaranteed
        /// to be explored.</para>
        /// </remarks>
        public DisjointSets<SearchTreeNode<TVertex>> EquivalenceClasses { get; private set; }

        /// <summary>
        /// Gets or sets a boolean value indicating whether a path exceeding the max length of the
        /// analysis settings was encountered.
        /// </summary>
        public bool TooLongPathEncountered { get; set; }

        /// <summary>
        /// Gets or sets the node with the longest path encountered.
        /// </summary>
        public Path<TVertex> LongestPathEncounteredNode { get; set; }

        internal AnalysisStateForSingleStartingVertex(TVertex startingVertex)
        {
            Stack = new Stack<SearchTreeNode<TVertex>>();
            ZeroDummyNode = new SearchTreeNode<TVertex>(null, default);
            var root = new SearchTreeNode<TVertex>(null, startingVertex); // The stationary path
            SearchTree = root;
            this.maximalPathRepresentatives = new HashSet<SearchTreeNode<TVertex>>();
            EquivalenceClasses = new DisjointSets<SearchTreeNode<TVertex>>();
            TooLongPathEncountered = false;
            LongestPathEncounteredNode = root.Path;

            EquivalenceClasses.MakeSet(ZeroDummyNode);
            EquivalenceClasses.MakeSet(root);
            Stack.Push(root);
        }

        /// <summary>
        /// Indicates whether the path of the specified node is zero-equivalent.
        /// </summary>
        /// <param name="node">The node whose path to check for zero-equivalence.</param>
        /// <returns><see langword="true"/> if the path of <paramref name="node"/> is
        /// zero-equivalent; <see langword="false"/> otherwise.</returns>
        public bool NodeIsZeroEquivalent(SearchTreeNode<TVertex> node)
        {
            var equivClass = EquivalenceClasses.FindSet(node);
            var zeroClass = EquivalenceClasses.FindSet(ZeroDummyNode);
            return equivClass.Equals(zeroClass);
        }

        /// <summary>
        /// Creates and inserts a node as a child to an already existent node in the search tree.
        /// </summary>
        /// <param name="parent">The parent of the node to insert.</param>
        /// <param name="vertex">The vertex of the new node.</param>
        /// <returns>The inserted node.</returns>
        /// <remarks>
        /// <para><see cref="EquivalenceClasses"/> is updated to contain a singleton for the new
        /// node.</para>
        /// </remarks>
        public SearchTreeNode<TVertex> InsertChildNode(SearchTreeNode<TVertex> parent, TVertex vertex)
        {
            var node = new SearchTreeNode<TVertex>(parent, vertex);
            parent.children.Add(vertex, node);
            EquivalenceClasses.MakeSet(node);
            return node;
        }

        /// <summary>
        /// Gets a child node (creating and inserting it into the search tree if necessary) of a
        /// specified node.
        /// </summary>
        /// <param name="parent">The parent of the node to get.</param>
        /// <param name="vertex">The vertex of the node to get.</param>
        /// <returns>The child node.</returns>
        public SearchTreeNode<TVertex> GetInsertChildNode(SearchTreeNode<TVertex> parent, TVertex vertex)
        {
            if (!parent.Children.ContainsKey(vertex)) InsertChildNode(parent, vertex);
            return parent.Children[vertex];
        }
    }
}
