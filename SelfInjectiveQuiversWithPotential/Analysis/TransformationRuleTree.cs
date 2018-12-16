using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    /// <summary>
    /// This class represents a node in a tree of transformation rules.
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    /// <remarks>
    /// <para>As usual, the root node (node with <see cref="Parent"/> equal to <see langword="null"/>)
    /// is used to represent the tree.</para>
    /// <para>Every node other than the root represents a path, namely the path whose vertices
    /// <em>in reverse order</em> are the unique vertices such that going from the root node via
    /// the <see cref="Children"/> property recursively yields the current node.</para>
    /// </remarks>
    public class TransformationRuleTreeNode<TVertex> where TVertex : IEquatable<TVertex>, IComparable<TVertex>
    {
        public bool CanBeKilled { get; internal set; }

        public bool CanBeReplaced { get => ReplacementPath != null; }

        public Path<TVertex> ReplacementPath { get; internal set; }

        public TransformationRuleTreeNode<TVertex> Parent { get; private set; }

        internal Dictionary<TVertex, TransformationRuleTreeNode<TVertex>> children;
        public IReadOnlyDictionary<TVertex, TransformationRuleTreeNode<TVertex>> Children { get => children; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransformationRuleTreeNode{TVertex}"/> class.
        /// </summary>
        /// <param name="canBeKilled">A boolean value indicating whether the path can be killed.</param>
        /// <param name="replacementPath">The path that the entire path that the node to construct
        /// represents can be replaced by, or <see langword="null"/> if the entire path that
        /// the node to construct represents cannot be replaced by any other path.</param>
        /// <param name="parent">The parent node, or <see langword="null"/> for constructing the
        /// root node.</param>
        public TransformationRuleTreeNode(bool canBeKilled, Path<TVertex> replacementPath, TransformationRuleTreeNode<TVertex> parent)
        {
            CanBeKilled = CanBeKilled;
            ReplacementPath = replacementPath;

            Parent = parent;
            children = new Dictionary<TVertex, TransformationRuleTreeNode<TVertex>>();
        }
    }
}
