using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    /// <summary>
    /// This class is the most recent implementation of the
    /// <see cref="IMaximalNonzeroEquivalenceClassRepresentativeComputer"/> interface.
    /// </summary>
    public class MaximalNonzeroEquivalenceClassRepresentativeComputer : IMaximalNonzeroEquivalenceClassRepresentativeComputer
    {
        public bool SupportsNonCancellativityDetection => true;

        public bool SupportsNonAdmissibilityHandling => true;

        /// <summary>
        /// Defines the results of an equivalence class computation.
        /// </summary>
        enum EquivalenceClassResult
        {
            /// <summary>
            /// Indicates that the equivalence class is zero equivalent.
            /// </summary>
            Zero,

            /// <summary>
            /// Indicates that the equivalence class was computed in its entirety and that it is
            /// not the zero equivalence class.
            /// </summary>
            Nonzero,

            /// <summary>
            /// Indicates that the computation was aborted because a too long path was encountered.
            /// </summary>
            TooLongPath
        }

        enum PathLengthCheckResult
        {
            /// <summary>
            /// Indicates that the path is not too long.
            /// </summary>
            Fine,

            /// <summary>
            /// Indicates that the path is too long.
            /// </summary>
            TooLongPath
        }

        enum WeakCancellativityStateForPath
        {
            /// <summary>
            /// Indicates that the path has a distinguishing arrow.
            /// </summary>
            HasDistinguishingArrow,

            /// <summary>
            /// Indicates that a distinguishing arrow for the path has not been found yet.
            /// </summary>
            /// <remarks>
            /// <para>At the end of the weak-cancellativity check, this value indicates that the
            /// path has no distinguishing arrow period.</para>
            /// </remarks>
            HasNoDistinguishingArrowSoFar,

            /// <summary>
            /// Indicates that for the current equivalence class, p+I is so far distinguished by an
            /// arrow.
            /// </summary>
            /// <remarks>
            /// <para>Technically, we could do without this value and use non-membership in d but
            /// membership in parentDict to indicate this value, but it seems a lot cleaner to just
            /// use third explicit value.</para>
            /// </remarks>
            HasDistinguishingArrowForCurrentChildClass,
        }

        /// <summary>
        /// Essentially <see cref="ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt"/>
        /// but with a more detailed, implementation-specific return value.
        /// </summary>
        /// <typeparam name="TVertex"></typeparam>
        /// <param name="quiver"></param>
        /// <param name="startingVertex"></param>
        /// <param name="transformationRuleTree"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        private AnalysisResultsForSingleStartingVertex<TVertex> AnalyzeWithStartingVertex<TVertex>(
            Quiver<TVertex> quiver,
            TVertex startingVertex,
            TransformationRuleTreeNode<TVertex> transformationRuleTree,
            MaximalNonzeroEquivalenceClassRepresentativeComputationSettings settings)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            // Parameter validation
            if (quiver is null) throw new ArgumentNullException(nameof(quiver));
            if (!quiver.Vertices.Contains(startingVertex)) throw new ArgumentException($"The quiver does not contain the starting vertex {startingVertex}.");
            if (transformationRuleTree is null) throw new ArgumentNullException(nameof(transformationRuleTree));
            if (settings is null) throw new ArgumentNullException(nameof(settings));

            // Do search in the tree of all paths starting at startingVertex
            AnalysisStateForSingleStartingVertex<TVertex> state = DoSearchInPathTree(quiver, startingVertex, transformationRuleTree, settings);

            // Do cancellativity check
            bool shouldDoNonCancellativityDetection = settings.DetectNonCancellativity && !state.TooLongPathEncountered;
            bool nonCancellativityDetected = shouldDoNonCancellativityDetection ? DetectFailureOfCancellativity(state) : false;

            // Return results
            var results = new AnalysisResultsForSingleStartingVertex<TVertex>(state, nonCancellativityDetected);
            return results;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertices in the quiver.</typeparam>
        /// <param name="quiver">The quiver.</param>
        /// <param name="startingVertex">The starting vertex of the paths to look at.</param>
        /// <param name="transformationRuleTree"></param>
        /// <returns>The state of the search after it is done.</returns>
        private AnalysisStateForSingleStartingVertex<TVertex> DoSearchInPathTree<TVertex>(
            Quiver<TVertex> quiver,
            TVertex startingVertex,
            TransformationRuleTreeNode<TVertex> transformationRuleTree,
            MaximalNonzeroEquivalenceClassRepresentativeComputationSettings settings)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            // Set up for analysis/graph search
            var state = new AnalysisStateForSingleStartingVertex<TVertex>(startingVertex);

            // Analysis/graph search
            // Keep a stack of "recently equivalence-class-computed" nodes
            // In every iteration, pop a node from the stack and determine the equivalence classes
            // of its child nodes.
            // It would be cleaner to in every iteration determine the equivalence class of the
            // *current* node and enqueue its child nodes (in other words, to have the queue
            // contain nodes whose equivalence classes to explore), but this makes it non-trivial
            // to keep track of the maximal nonzero equivalence classes
            while (state.Stack.Count > 0)
            {
                var node = state.Stack.Pop();
                bool isMaximalNonzero = true;

                foreach (var nextVertex in quiver.AdjacencyLists[node.Vertex])
                {
                    var child = state.GetInsertChildNode(node, nextVertex);
                    if (DoPathLengthCheck(child, state, settings) == PathLengthCheckResult.TooLongPath)
                    {
                        state.TooLongPathEncountered = true;
                        return state;
                    }

                    var equivClassResult = DetermineEquivalenceClass(child, state, transformationRuleTree, settings);
                    if (equivClassResult == EquivalenceClassResult.Nonzero)
                    {
                        isMaximalNonzero = false;
                        state.Stack.Push(child);
                    }
                    else if (equivClassResult == EquivalenceClassResult.TooLongPath)
                    {
                        state.TooLongPathEncountered = true;
                        return state;
                    }
                }

                if (isMaximalNonzero)
                {
                    var representative = state.EquivalenceClasses.FindSet(node);
                    if (!state.maximalPathRepresentatives.Contains(representative))
                    {
                        state.maximalPathRepresentatives.Add(representative);
                    }
                }
            }

            return state;
        }

        /// <summary>
        /// Detects failure of cancellativity.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertices in the quiver.</typeparam>
        /// <param name="state">The state of the analysis after having done the search in the path
        /// tree.</param>
        /// <returns><see langword="true"/> if the analysis state contains a contradiction to
        /// cancellativity; <see langword="false"/> otherwise.</returns>
        private bool DetectFailureOfCancellativity<TVertex>(AnalysisStateForSingleStartingVertex<TVertex> state) where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            // Instead of checking cancellativity in the naïve way:
            //   for every (canonical representative) path p
            //     for every (canonical representative) path q
            //       for every arrow a
            //         if pa+I = qa+I but p+I != q+I, return NotCancellative
            //
            //   return Cancellative
            //
            // the code below checks cancellativity by iterating only once over the quotient set
            // (set of all equivalence classes). The intuition is that it operates on pa+I instead
            // of on p+I. That is, instead of looking at all equivalence classes that p+I extends to,
            // we look at equivalence classes that extend into pa+I.
            // Thus, the work of checking whether a path satisfies cancellativity (corresponding to
            // the body of the loop over p in the pseudocode above) is split over potentially
            // several iterations (each such iteration corresponding to only one extending class pa+I).
            //
            // It should be noted that the nested loop above might not be as awful as it seems,
            // because we loop only over the *representative* paths (i.e., a transversal of the
            // quotient set). The naïve implementation might thus perform better than the
            // implementation below or at least not considerably much worse and have the benefit of
            // clarity and correctness.
            //
            // It should be straightforward that the below code only indicates that cancellativity
            // fails when it actually fails. Conversely, if cancellativity fails, say for a path p
            // and arrow a, then the below code indicates that cancellativity fails when the
            // equivalence class pa+I is processed.

            var zeroNode = state.ZeroDummyNode;
            var stationaryPathNode = state.SearchTree;
            foreach (var equivalenceClass in state.EquivalenceClasses.GetSets())
            {
                // Exclude the zero class and the stationary class -- the zero class because
                // pa +I = 0+I is not interesting for cancellativity and the stationary class
                // because it is not of the form pa+I.
                if (equivalenceClass.Contains(zeroNode) || equivalenceClass.Contains(stationaryPathNode)) continue;

                // Dictionary to contain the canonical representative of an arbitrary parent for every distinct last arrow
                // (the *only* parent up to equivalence if the unbound quiver is cancellative).
                var parentDict = new Dictionary<Arrow<TVertex>, SearchTreeNode<TVertex>>();
                foreach (var pathNode in equivalenceClass)
                {
                    var curParent = pathNode.Parent;
                    var lastArrow = pathNode.ReversePathOfArrows.First();
                    if (!parentDict.TryGetValue(lastArrow, out var storedParent))
                    {
                        parentDict[lastArrow] = state.EquivalenceClasses.FindSet(curParent);
                    }
                    else
                    {
                        if (!state.EquivalenceClasses.FindSet(curParent).Equals(storedParent))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Detects failure of weak cancellativity.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertices in the quiver.</typeparam>
        /// <param name="state">The state of the analysis after having done the search in the path
        /// tree.</param>
        /// <returns><see langword="true"/> if the analysis state contains a contradiction to weak
        /// cancellativity; <see langword="false"/> otherwise.</returns>
        private bool DetectFailureOfWeakCancellativity<TVertex>(AnalysisStateForSingleStartingVertex<TVertex> state) where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            // Instead of checking weak cancellativity in the naïve way:
            //   for every (canonical representative) path p
            //     check if there an arrow a with the following:
            //       for every (canonical representative) path q
            //         pa+I = qa+I implies p+I = q+I
            //     if not, return NotWeaklyCancellative
            //
            //   return WeaklyCancellative
            //
            // the code below checks cancellativity by iterating only once over the quotient set
            // (set of all equivalence classes). The intuition is that it operates on pa+I instead
            // of on p+I. That is, instead of looking at all equivalence classes that p+I extends to,
            // we look at equivalence classes that extend into pa+I.
            // Thus, the work of checking whether a path satisfies weak cancellativity
            // (corresponding to the body of the loop over p in the pseudocode above) is split over
            // potentially several iterations (each such iteration corresponding to only one
            // extending class pa+I).
            //
            // It should be noted that the nested loop above might not be as awful as it seems,
            // because we loop only over the *representative* paths (i.e., a transversal of the
            // quotient set). The naïve implementation might thus perform better than the
            // implementation below or at least not considerably much worse and have the benefit of
            // clarity and correctness.
            //
            // Unlike in the similar implementation for strong cancellativity, where a single counter-
            // example (p,a) suffices to terminate with NotCancellative, this implementation
            // requires for some path p and *all* arrows a the pair (p,a) to fail the second
            // condition of weak cancellativity. Because the work for a single path p is typically
            // split across several iterations, we need to keep track of the state for p between
            // iterations (whether or not we have found a distinguishing ("non-counterexample")
            // arrow for p yet). This is done with the dictionary d.
            //
            // The assumption that the quiver has no parallel arrows should be necessary in order
            // for the below code to be correct. Otherwise, a path p with pa+I = pb+I = qb+I might
            // be incorrectly flagged as not having a distinguishing arrow even if a
            // distinguishes p from other paths.

            // Dictionary that will associate to every canonical representative path that is not
            // maximal nonzero-equivalent a value indicating whether the path has a
            // "distinguishing" arrow, i.e., an arrow a satisfying the second condition of weak
            // cancellativity for the path (p say):
            //   for all paths q: pa+I = qa+I implies p+I = q+I
            // Important to note is that a fourth value is stored implicitly as non-membership in d.
            // This value indicates that no nonzero extension class has been processed yet (at the
            // end, it means that the canonical representative path has no nonzero extension class,
            // i.e., is maximal nonzero-equivalent).
            var d = new Dictionary<SearchTreeNode<TVertex>, WeakCancellativityStateForPath>();
            var zeroNode = state.ZeroDummyNode;
            var stationaryPathNode = state.SearchTree;

            foreach (var equivalenceClass in state.EquivalenceClasses.GetSets())
            {
                // Exclude the zero class and the stationary class -- the zero path because
                // pa+I = 0+I is not interesting for the second condition for weak cancellativity
                // and the stationary path because it is not of the form pa+I.
                if (equivalenceClass.Contains(zeroNode) || equivalenceClass.Contains(stationaryPathNode)) continue;

                // Below, equivalenceClass is denoted by pa+I.
                // Dictionary to contain the canonical representative of the parent class p+I for
                // every distinct last arrow (the *only* parent for that arrow a iff the path
                // and arrow p,a satisfy the second condition of weak cancellativity)
                // This is used to detect for each arrow whether it fails the second condition of
                // weak cancellativity (for all paths q: pa+I = qa+I implies p+I = q+I).
                var parentDict = new Dictionary<Arrow<TVertex>, SearchTreeNode<TVertex>>();

                foreach (var pathNode in equivalenceClass)
                {
                    var curParent = pathNode.Parent;
                    var canonicalParent = state.EquivalenceClasses.FindSet(curParent);
                    var lastArrow = pathNode.ReversePathOfArrows.First();

                    // If we have already found a distinguishing arrow for the canonical parent,
                    // there's no need for additional work.
                    if (d.TryGetValue(canonicalParent, out var pathState) && pathState == WeakCancellativityStateForPath.HasDistinguishingArrow)
                        continue;

                    if (!parentDict.TryGetValue(lastArrow, out var storedParent))
                    {
                        // Note that the parent is stored in the parentDict only if d[parent] is
                        // not HasDistinguishingArrow. This makes sure that the else clause below
                        // does not overwrite HasDistinguishingArrow with HasNoDistinguishingArrowSoFar.
                        parentDict[lastArrow] = canonicalParent;
                        // Also, this write does not overwrite HasDistinguishingArrow either
                        d[canonicalParent] = WeakCancellativityStateForPath.HasDistinguishingArrowForCurrentChildClass;
                    }
                    else if (!canonicalParent.Equals(storedParent))
                    {
                        // Insert HasNoDistinguishingArrowSoFar into d if necessary (if not, the
                        // write is a no-op) indicating that the parents are not maximal
                        // nonzero-equivalent and that we have not yet found a distinguishing arrow
                        // for them.
                        d[canonicalParent] = WeakCancellativityStateForPath.HasNoDistinguishingArrowSoFar;
                        d[storedParent] = WeakCancellativityStateForPath.HasNoDistinguishingArrowSoFar;
                    }
                }

                foreach (var (_, storedParent) in parentDict)
                {
                    if (d[storedParent] == WeakCancellativityStateForPath.HasNoDistinguishingArrowSoFar)
                    {
                        d[storedParent] = WeakCancellativityStateForPath.HasDistinguishingArrow;
                    }
                }
            }

            return d.Values.All(pathState => pathState == WeakCancellativityStateForPath.HasDistinguishingArrow);
        }

        /// <summary>
        /// Determines the equivalence class of the specified node.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertices in the quiver.</typeparam>
        /// <param name="node">The node whose equivalence class to determine.</param>
        /// <param name="state">The state of the computation.</param>
        /// <param name="transformationRuleTree">The transformation rule tree.</param>
        /// <param name="settings">The computation settings.</param>
        /// <returns><see cref="EquivalenceClassResult.TooLongPath"/> if the equivalence class has
        /// not been computed before, <paramref name="settings"/> has
        /// <see cref="AnalysisSettings.UseMaxLength"/> equal to <see langword="true"/>, and a path
        /// of length (in arrows) strictly greater than
        /// <see cref="AnalysisSettings.MaxPathLength"/> of <paramref name="settings"/> is
        /// encountered during the equivalence class search. Otherwise,
        /// <see cref="EquivalenceClassResult.Zero"/> if the equivalence class is the zero
        /// class, and <see cref="EquivalenceClassResult.Nonzero"/> if the equivalence class is
        /// not the zero class.</returns>
        /// <remarks>
        /// <para>This method may modify the
        /// <see cref="AnalysisStateForSingleStartingVertex{TVertex}.SearchTree"/>,
        /// <see cref="AnalysisStateForSingleStartingVertex{TVertex}.EquivalenceClasses"/>, and
        /// <see cref="AnalysisStateForSingleStartingVertex{TVertex}.LongestPathEncounteredNode"/>
        /// properties of the <paramref name="state"/> argument.</para>
        /// </remarks>
        private EquivalenceClassResult DetermineEquivalenceClass<TVertex>(
            SearchTreeNode<TVertex> node,
            AnalysisStateForSingleStartingVertex<TVertex> state,
            TransformationRuleTreeNode<TVertex> transformationRuleTree,
            MaximalNonzeroEquivalenceClassRepresentativeComputationSettings settings)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            if (node.EquivalenceClassIsComputed)
            {
                return state.NodeIsZeroEquivalent(node) ? EquivalenceClassResult.Zero : EquivalenceClassResult.Nonzero;
            }

            return ComputeEquivalenceClass(node, state, transformationRuleTree, settings);            
        }

        /// <summary>
        /// Computes the equivalence class of the specified node.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertices in the quiver.</typeparam>
        /// <param name="node">The node whose equivalence class to determine.</param>
        /// <param name="state">The state of the computation.</param>
        /// <param name="transformationRuleTree">The transformation rule tree.</param>
        /// <param name="settings">The computation settings.</param>
        /// <returns><see cref="EquivalenceClassResult.TooLongPath"/> if
        /// <paramref name="settings"/> has <see cref="AnalysisSettings.UseMaxLength"/> equal to
        /// <see langword="true"/> and a path of length (in arrows) strictly greater than
        /// <see cref="AnalysisSettings.MaxPathLength"/> of <paramref name="settings"/> is
        /// encountered during the equivalence class search. Otherwise,
        /// <see cref="EquivalenceClassResult.Zero"/> if the equivalence class is the zero
        /// class, and <see cref="EquivalenceClassResult.Nonzero"/> if the equivalence class is not
        /// the zero class.</returns>
        /// <remarks>
        /// <para>This method may modify the
        /// <see cref="AnalysisStateForSingleStartingVertex{TVertex}.SearchTree"/>,
        /// <see cref="AnalysisStateForSingleStartingVertex{TVertex}.EquivalenceClasses"/>, and
        /// <see cref="AnalysisStateForSingleStartingVertex{TVertex}.LongestPathEncounteredNode"/>
        /// properties of the <paramref name="state"/> argument.</para>
        /// </remarks>
        private EquivalenceClassResult ComputeEquivalenceClass<TVertex>(
            SearchTreeNode<TVertex> startNode,
            AnalysisStateForSingleStartingVertex<TVertex> state,
            TransformationRuleTreeNode<TVertex> transformationRuleTree,
            MaximalNonzeroEquivalenceClassRepresentativeComputationSettings settings)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            // The stack or queue of nodes to process
            var equivClassStack = new Stack<SearchTreeNode<TVertex>>();
            equivClassStack.Push(startNode);
            startNode.HasBeenDiscoveredDuringEquivalenceClassComputation = true;

            while (equivClassStack.Count > 0)
            {
                var node = equivClassStack.Pop();
                var result = ExploreNodeForEquivalenceClassSearch(node, equivClassStack, state, transformationRuleTree, settings);
                if (result == EquivalenceClassResult.TooLongPath) return EquivalenceClassResult.TooLongPath; // If too long, return "too long".
                else if (result == EquivalenceClassResult.Zero) return EquivalenceClassResult.Zero; // If definitely zero-equivalent, return "zero"
            }

            return EquivalenceClassResult.Nonzero;
        }

        /// <summary>
        /// Explores the specified node in the equivalence class search.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertices in the quiver.</typeparam>
        /// <param name="node">The node to explore.</param>
        /// <param name="equivClassStack">The stack or queue of nodes to explore.</param>
        /// <param name="state">The state of the computation.</param>
        /// <param name="transformationRuleTree">The transformation rule tree.</param>
        /// <param name="settings">The computation settings.</param>
        /// <returns>A value of the <see cref="EquivalenceClassResult"/> enum indicating whether
        /// the node was <em>found</em> to be zero-equivalent. That is, the returned value is
        /// <see cref="EquivalenceClassResult.Zero"/> <em>only</em> if the node is zero-equivalent
        /// but may be <see cref="EquivalenceClassResult.Nonzero"/> even if the node is
        /// zero-equivalent.
        /// <remarks>
        /// <para>A node is found to be zero-equivalent either if its path can be killed or if
        /// the path is equivalent (up to replacement) to a path that has previously been
        /// determined to be zero-equivalent.</para>
        /// <para>This method may modify the
        /// <see cref="AnalysisStateForSingleStartingVertex{TVertex}.SearchTree"/>,
        /// <see cref="AnalysisStateForSingleStartingVertex{TVertex}.EquivalenceClasses"/>, and
        /// <see cref="AnalysisStateForSingleStartingVertex{TVertex}.LongestPathEncounteredNode"/>
        /// properties of the <paramref name="state"/> argument.</para>
        /// </remarks>
        private EquivalenceClassResult ExploreNodeForEquivalenceClassSearch<TVertex>(
            SearchTreeNode<TVertex> node,
            Stack<SearchTreeNode<TVertex>> equivClassStack,
            AnalysisStateForSingleStartingVertex<TVertex> state,
            TransformationRuleTreeNode<TVertex> transformationRuleTree,
            MaximalNonzeroEquivalenceClassRepresentativeComputationSettings settings)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            // A list of the vertices that appear after the path being considered from transformation.
            // The vertices are in reversed order.
            var trailingVertexPath = new List<TVertex>();

            foreach (var endingNode in node.ReversePathOfNodes) // The vertex (i.e., last vertex) of endingNode is the last vertex in the subpath
            {
                var transformationNode = transformationRuleTree;
                foreach (var startingNode in endingNode.ReversePathOfNodes) // The vertex (i.e., last vertex) of startingNode is the first vertex in the subpath
                {
                    var pathVertex = startingNode.Vertex;
                    if (!transformationNode.Children.TryGetValue(pathVertex, out transformationNode)) break;

                    if (transformationNode.CanBeKilled)
                    {
                        state.EquivalenceClasses.Union(node, state.ZeroDummyNode);
                        return EquivalenceClassResult.Zero;
                    }

                    // If replacement is possible, do the replacement on the subpath
                    // and add a search tree node for the entire resulting path
                    if (transformationNode.CanBeReplaced)
                    {
                        // Add search tree nodes for replacement path

                        // This doesn't work when pathNode is the root ...
                        // var lastUnreplacedNode = pathNode.Parent;
                        // var curNode = lastUnreplacedNode;

                        // ... so instead do the following (with Skip), which assumes that the replacement
                        // path has the same first vertex (which it does for the semimonomial unbound
                        // quivers under consideration)
                        var firstNodeInTransformedPath = startingNode;
                        var curNode = firstNodeInTransformedPath;

                        foreach (var vertex in transformationNode.ReplacementPath.Vertices.Skip(1))
                        {
                            curNode = state.GetInsertChildNode(curNode, vertex);
                        }

                        // Add search tree nodes for the trailing path
                        foreach (var vertex in trailingVertexPath.Reversed())
                        {
                            curNode = state.GetInsertChildNode(curNode, vertex);
                        }

                        // We now have the node obtained by applying the replacement rule.
                        var transformedNode = curNode;

                        // Do stuff with its path length.
                        bool tooLong = DoPathLengthCheck(transformedNode, state, settings) == PathLengthCheckResult.TooLongPath;
                        if (tooLong) return EquivalenceClassResult.TooLongPath;

                        if (state.NodeIsZeroEquivalent(transformedNode))
                        {
                            state.EquivalenceClasses.Union(node, transformedNode); // Unioning with state.ZeroDummyNode would work equally well
                            return EquivalenceClassResult.Zero;
                        }

                        // If transformedNode.HasBeenDiscoveredDuringEquivalenceClassComputation
                        // at this point, then it was discovered during this equivalence class
                        // search (not during an earlier search that ended with zero equivalence),
                        // and in this case, we should ignore it!

                        if (!transformedNode.HasBeenDiscoveredDuringEquivalenceClassComputation)
                        {
                            transformedNode.HasBeenDiscoveredDuringEquivalenceClassComputation = true;
                            state.EquivalenceClasses.Union(node, transformedNode);
                            equivClassStack.Push(transformedNode);
                        }
                    }
                }

                trailingVertexPath.Add(endingNode.Vertex);
            }

            return EquivalenceClassResult.Nonzero;
        }

        /// <summary>
        /// Checks if the path is too long given the specified computation settings and also updates
        /// the <see cref="AnalysisStateForSingleStartingVertex{TVertex}.LongestPathEncounteredNode"/>
        /// property if necessary.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertices in the quiver.</typeparam>
        /// <param name="node">The node whose corresponding path to check.</param>
        /// <param name="state">The state of the computation.</param>
        /// <param name="settings">The computation settings, which indicates whether to detect too
        /// long paths.</param>
        /// <returns>A value of the <see cref="PathLengthCheckResult"/> enum indicating whether the
        /// path was too long.</returns>
        private PathLengthCheckResult DoPathLengthCheck<TVertex>(
            SearchTreeNode<TVertex> node,
            AnalysisStateForSingleStartingVertex<TVertex> state,
            MaximalNonzeroEquivalenceClassRepresentativeComputationSettings settings)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            if (node.PathLength <= state.LongestPathEncounteredNode.Length) return PathLengthCheckResult.Fine;

            // node.Path is slow, but this will be executed at most
            // min(maxPathLength, largestPathLengthInQuiver) times or so
            state.LongestPathEncounteredNode = node.Path;

            return (settings.UseMaxLength && node.PathLength > settings.MaxPathLength) ? PathLengthCheckResult.TooLongPath : PathLengthCheckResult.Fine;
        }

        /// <inheritdoc/>
        public MaximalNonzeroEquivalenceClassRepresentativesResults<TVertex> ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt<TVertex>(
            Quiver<TVertex> quiver,
            TVertex startingVertex,
            TransformationRuleTreeNode<TVertex> transformationRuleTree,
            MaximalNonzeroEquivalenceClassRepresentativeComputationSettings settings)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            var analysisResults = AnalyzeWithStartingVertex(quiver, startingVertex, transformationRuleTree, settings);

            // The .ToList() call to eagerly evaluate the maximal path representatives is really
            // important here; otherwise, the MaximalPathRepresentatives property of the
            // MaximalNonzeroEquivalenceClassRepresentativesResult prevents the entire search tree
            // of analysisResults from being freed.
            var outputResults = new MaximalNonzeroEquivalenceClassRepresentativesResults<TVertex>(
                nonCancellativityDetected: analysisResults.NonCancellativityDetected,
                tooLongPathEncountered: analysisResults.TooLongPathEncountered,
                analysisResults.MaximalPathRepresentatives.Select(node => node.Path).ToList(),
                longestPathEncountered: analysisResults.LongestPathEncountered);
            return outputResults;
        }
    }
}
