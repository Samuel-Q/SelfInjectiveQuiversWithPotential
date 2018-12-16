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
            bool nonCancellativityDetected = shouldDoNonCancellativityDetection ? DetectNonCancellativity(state) : false;

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
        /// Detects non-cancellativity.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertices in the quiver.</typeparam>
        /// <param name="state">The state of the analysis after having done the search in the path
        /// tree.</param>
        /// <returns><see langword="true"/> if non-cancellativity was detected; <see langword=""/> otherwise.</returns>
        private bool DetectNonCancellativity<TVertex>(AnalysisStateForSingleStartingVertex<TVertex> state) where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            var zeroNode = state.ZeroDummyNode;
            var stationaryPathNode = state.SearchTree;
            foreach (var equivalenceClass in state.EquivalenceClasses.GetSets())
            {
                // Exclude the zero path and the stationary path, because they do not have a parent
                // (the below code accesses the parent) and cannot ruin cancellativity
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
        public MaximalNonzeroEquivalenceClassRepresentativesResult<TVertex> ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt<TVertex>(
            Quiver<TVertex> quiver,
            TVertex startingVertex,
            TransformationRuleTreeNode<TVertex> transformationRuleTree,
            MaximalNonzeroEquivalenceClassRepresentativeComputationSettings settings)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            var analysisResults = AnalyzeWithStartingVertex(quiver, startingVertex, transformationRuleTree, settings);
            var outputResults = new MaximalNonzeroEquivalenceClassRepresentativesResult<TVertex>(
                nonCancellativityDetected: analysisResults.NonCancellativityDetected,
                tooLongPathEncountered: analysisResults.TooLongPathEncountered,
                analysisResults.MaximalPathRepresentatives.Select(node => node.Path),
                longestPathEncountered: analysisResults.LongestPathEncountered);
            return outputResults;
        }
    }
}
