using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    // TODO: Consider making this class static?
    // Harder to test but this class should be static imo.

    /// <summary>
    /// This class is used to create instances of the <see cref="TransformationRuleTreeNode{TVertex}"/> class.
    /// </summary>
    public class TransformationRuleTreeCreator
    {
        /// <summary>
        /// Creates a transformation rule tree for a <see cref="SemimonomialUnboundQuiver{TVertex}"/>.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertices of the quiver.</typeparam>
        /// <param name="unboundQuiver">The unbound quiver.</param>
        /// <returns>A transformation rule tree for <paramref name="semimonomialIdeal"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="semimonomialIdeal"/> is <see langword="null"/>.</exception>
        /// <exception cref="NotSupportedException"><paramref name="semimonomialIdeal"/> has two
        /// distinct non-monomial generators <c>p1 - q1</c> and <c>p2 - q2</c> where
        /// <c>p1, q1, p2, q2</c> are not all distinct.</exception>
        /// <exception cref="ArgumentException"><paramref name="semimonomialIdeal"/> has a
        /// non-monomial generator <c>p - q</c> where the paths <c>p</c> and <c>q</c> do not have
        /// the same endpoints.</exception>
        public TransformationRuleTreeNode<TVertex> CreateTransformationRuleTree<TVertex>(SemimonomialIdeal<TVertex> semimonomialIdeal)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            var root = new TransformationRuleTreeNode<TVertex>(false, null, null);

            // Monomial generators (kill rules)
            foreach (var path in semimonomialIdeal.Paths)
            {
                var node = GetOrInsertDefaultNode(path, root);
                node.CanBeKilled = true;
            }

            // Non-monomial generators (replace rules)
            foreach (var difference in semimonomialIdeal.DifferencesOfPaths)
            {
                // Replacement rules where the paths have different starting or ending points are disallowed
                if (!difference.Minuend.StartingPoint.Equals(difference.Subtrahend.StartingPoint) || !difference.Minuend.EndingPoint.Equals(difference.Subtrahend.EndingPoint))
                {
                    throw new NotSupportedException("Ideals with a non-monomial generator whose two paths have different starting points or ending points are not supported.");
                }

                InsertReplacementRule(difference.Minuend, difference.Subtrahend);
                InsertReplacementRule(difference.Subtrahend, difference.Minuend);
            }

            void InsertReplacementRule(Path<TVertex> originalPath, Path<TVertex> replacementPath)
            {
                var node = GetOrInsertDefaultNode(originalPath, root);
                if (node.ReplacementPath != null)
                {
                    // Shouldn't be too much work to add support for this imo
                    throw new NotSupportedException("Ideals with a path occurring in more than one non-monomial generator are not supported.");
                }

                node.ReplacementPath = replacementPath;
            }

            return root;
        }

        /// <summary>
        /// Creates a transformation rule tree for a <see cref="SemimonomialUnboundQuiver{TVertex}"/>.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertices of the quiver.</typeparam>
        /// <param name="unboundQuiver">The unbound quiver.</param>
        /// <returns>A transformation rule tree for <paramref name="unboundQuiver"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="unboundQuiver"/> is <see langword="null"/>.</exception>
        /// <exception cref="NotSupportedException">The semimonomial ideal of
        /// <paramref name="unboundQuiver"/> has two distinct non-monomial generators
        /// <c>p1 - q1</c> and <c>p2 - q2</c> where <c>p1, q1, p2, q2</c> are not all distinct.</exception>
        /// <exception cref="ArgumentException">The semimonomial ideal of
        /// <paramref name="unboundQuiver"/> has a non-monomial generator <c>p - q</c> where the
        /// paths <c>p</c> and <c>q</c> do not have the same endpoints.</exception>
        public TransformationRuleTreeNode<TVertex> CreateTransformationRuleTree<TVertex>(SemimonomialUnboundQuiver<TVertex> unboundQuiver)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            if (unboundQuiver is null) throw new ArgumentNullException(nameof(unboundQuiver));
            return CreateTransformationRuleTree(unboundQuiver.Ideal);
        }

        /// <remarks>This method should be made obsolete (prefer converting the QP to a
        /// semimonomial unbound quiver, and analyzing that instead (or at least creating the
        /// transformation rule tree for that instead).</remarks>
        public TransformationRuleTreeNode<TVertex> CreateTransformationRuleTree<TVertex>(QuiverWithPotential<TVertex> qp)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            if (qp is null) throw new ArgumentNullException(nameof(qp));

            var root = new TransformationRuleTreeNode<TVertex>(false, null, null);

            foreach (var arrow in qp.Quiver.Arrows)
            {
                var linComb = new LinearCombination<Path<TVertex>>();
                foreach (var (cycle, coefficient) in qp.Potential.LinearCombinationOfCycles.ElementToCoefficientDictionary)
                {
                    linComb = linComb.Add(cycle.DifferentiateCyclically(arrow).Scale(coefficient));
                }

                AddRulesFromSingleLinearCombination(linComb, root);
            }

            return root;
        }

        private void AddRulesFromSingleLinearCombination<TVertex>(LinearCombination<Path<TVertex>> linComb, TransformationRuleTreeNode<TVertex> root)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            switch (linComb.ElementToCoefficientDictionary.Count)
            {
                case 0: return;
                case 1:
                    var path = linComb.Elements.Single();
                    var node = GetOrInsertDefaultNode(path, root);
                    node.CanBeKilled = true;
                    break;
                case 2:
                    var coefficients = linComb.ElementToCoefficientDictionary.Values.ToList();
                    var paths = linComb.Elements.ToList(); // Could be in different order from the coefficients, but don't care
                    if (coefficients[1] == -coefficients[0])
                    {
                        GetOrInsertDefaultNode(paths[0], root).ReplacementPath = paths[1];
                        GetOrInsertDefaultNode(paths[1], root).ReplacementPath = paths[0];
                    }
                    else throw new NotSupportedException("Linear combinations of length 2 with coefficients that are not the additive inverse of each other are not supported.");
                    break;
                default:
                    throw new NotSupportedException("Linear combinations of length greater than 2 are not supported.");
            }
        }

        /// <summary>
        /// Gets a node according to a specified paths, inserting nodes without any transformation
        /// data into the tree as necessary along the way.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertices.</typeparam>
        /// <param name="path">The path according to which to get the node from the tree.</param>
        /// <param name="root">The root node of the tree.</param>
        /// <returns>The retrieved node.</returns>
        /// <remarks>Additional nodes without any transformation data are inserted into the tree as
        /// necessary between the root node and the node to insert.</remarks>
        private TransformationRuleTreeNode<TVertex> GetOrInsertDefaultNode<TVertex>(Path<TVertex> path, TransformationRuleTreeNode<TVertex> root)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            var curNode = root;
            foreach (var vertex in path.Vertices.Reverse())
            {
                if (!curNode.Children.ContainsKey(vertex)) curNode.children[vertex] = new TransformationRuleTreeNode<TVertex>(false, null, curNode);
                curNode = curNode.Children[vertex];
            }

            return curNode;
        }
    }
}
