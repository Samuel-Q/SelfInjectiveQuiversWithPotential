using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    /// <summary>
    /// This class is used to analyze QPs (<see cref="QuiverWithPotential{TVertex}"/>s).
    /// </summary>
    /// <remarks>
    /// <para>This analyzer requires the potential of the QP to have only coefficients -1 and +1,
    /// every arrow to appear in at most one positive cycle and at most once,
    /// and every arrow to appear in at most one negative cycle and at most once.</para>
    /// <para>This class reduces the problem of analyzing a QP to analyzing a
    /// <see cref="SemimonomialUnboundQuiver{TVertex}"/> and uses
    /// <see cref="SemimonomialUnboundQuiverAnalyzer"/> to solve that problem.</para>
    /// </remarks>
    public class QPAnalyzer : IQPAnalyzer
    {
        private readonly IMaximalNonzeroEquivalenceClassRepresentativeComputer defaultComputer;

        public QPAnalyzer() : this(new MaximalNonzeroEquivalenceClassRepresentativeComputer())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="QPAnalyzer"/> class with the specified
        /// computer of maximal nonzero equivalence class representatives to use by default when
        /// analyzing.
        /// </summary>
        /// <param name="defaultComputer">The default computer of maximal nonzero equivalence class
        /// representatives.</param>
        /// <exception cref="ArgumentNullException"><paramref name="defaultComputer"/> is
        /// <see langword="null"/>.</exception>
        public QPAnalyzer(IMaximalNonzeroEquivalenceClassRepresentativeComputer defaultComputer)
        {
            this.defaultComputer = defaultComputer ?? throw new ArgumentNullException(nameof(defaultComputer));
        }

        /// <summary>
        /// Analyzes a QP with the default computer of maximal nonzero equivalence class
        /// representatives.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertices of the quiver.</typeparam>
        /// <param name="qp">The quiver with potential.</param>
        /// <param name="settings">The settings for the analysis.</param>
        /// <returns>The results of the analysis.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="qp"/> is
        /// <see langword="null"/>, or <paramref name="settings"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="NotSupportedException">The potential of <paramref name="qp"/> has a
        /// cycle with coefficient not equal to either of -1 and +1,
        /// or some arrow occurs multiple times in a single cycle of the potential of
        /// <paramref name="qp"/>.</exception>
        /// <exception cref="ArgumentException">For some arrow in the potential of
        /// <paramref name="qp"/> and sign, the arrow is contained in more than one cycle of that
        /// sign.</exception>
        public IQPAnalysisResults<TVertex> Analyze<TVertex>(QuiverWithPotential<TVertex> qp, QPAnalysisSettings settings)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            return Analyze(qp, settings, defaultComputer);
        }

        /// <summary>
        /// Analyzes a QP with the specified computer of maximal nonzero equivalence class
        /// representatives.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertices of the quiver.</typeparam>
        /// <param name="qp">The quiver with potential.</param>
        /// <param name="settings">The settings for the analysis.</param>
        /// <param name="computer">A computer of maximal nonzero equivalence class representatives.</param>
        /// <returns>The results of the analysis.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="qp"/> is
        /// <see langword="null"/>, or <paramref name="settings"/> is <see langword="null"/>,
        /// or <paramref name="computer"/> is <see langword="null"/>.</exception>
        /// <exception cref="NotSupportedException">The potential of <paramref name="qp"/> has a
        /// cycle with coefficient not equal to either of -1 and +1,
        /// or some arrow occurs multiple times in a single cycle of the potential of
        /// <paramref name="qp"/>.</exception>
        /// <exception cref="ArgumentException">For some arrow in the potential of
        /// <paramref name="qp"/> and sign, the arrow is contained in more than one cycle of that
        /// sign.</exception>
        public IQPAnalysisResults<TVertex> Analyze<TVertex>(
            QuiverWithPotential<TVertex> qp,
            QPAnalysisSettings settings,
            IMaximalNonzeroEquivalenceClassRepresentativeComputer computer)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            if (qp is null) throw new ArgumentNullException(nameof(qp));
            if (settings is null) throw new ArgumentNullException(nameof(settings));
            if (computer is null) throw new ArgumentNullException(nameof(computer));

            // Simply get the underlying semimonomial unbound quiver and analyze it using the appropriate analyzer
            var semimonomialUnboundQuiver = SemimonomialUnboundQuiverFactory.CreateSemimonomialUnboundQuiverFromQP(qp);
            var suqAnalyzer = new SemimonomialUnboundQuiverAnalyzer();
            var suqSettings = AnalysisSettingsFactory.CreateSemimonomialUnboundQuiverAnalysisSettings(settings);
            var suqResults = suqAnalyzer.Analyze(semimonomialUnboundQuiver, suqSettings, computer);
            var results = AnalysisResultsFactory.CreateQPAnalysisResults(suqResults);

            return results;
        }

        /// <summary>
        /// Analyzes a QP in a way that utilizes the &quot;periodicity&quot; of the QP and
        /// concurrently.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertices of the quiver.</typeparam>
        /// <param name="qp">The quiver with potential.</param>
        /// <param name="periods">A collection of consecutive non-empty periods of the QP that are
        /// jointly exhaustive and mutually exclusive.</param>
        /// <param name="settings">The settings for the analysis.</param>
        /// <returns>The results of the analysis.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="qp"/> is
        /// <see langword="null"/>,
        /// or <paramref name="periods"/> is <see langword="null"/>,
        /// or <paramref name="settings"/> is <see langword="null"/>.</exception>
        /// <exception cref="NotSupportedException">The potential of <paramref name="qp"/> has a
        /// cycle with coefficient not equal to either of -1 and +1,
        /// or some arrow occurs multiple times in a single cycle of the potential of
        /// <paramref name="qp"/>.</exception>
        /// <exception cref="ArgumentException">For some arrow in the potential of
        /// <paramref name="qp"/> and sign, the arrow is contained in more than one cycle of that
        /// sign, or some of the periods in <paramref name="periods"/> overlap, or the union of all
        /// periods in <paramref name="periods"/> is not precisely the collection of all vertices
        /// in the quiver.</exception>
        /// <remarks>
        /// <para>Some validation of <paramref name="periods"/> is done, but
        /// <paramref name="periods"/> is not verified to constitute a sequence of consecutive
        /// &quot;periods&quot; of the QP.</para>
        /// </remarks>
        public IQPAnalysisResults<TVertex> AnalyzeUtilizingPeriodicityConcurrently<TVertex>(
            QuiverWithPotential<TVertex> qp,
            IEnumerable<IEnumerable<TVertex>> periods,
            QPAnalysisSettings settings)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            return AnalyzeUtilizingPeriodicityConcurrently(
                qp,
                periods,
                settings,
                defaultComputer);
        }

        /// <summary>
        /// Analyzes a QP in a way that utilizes the &quot;periodicity&quot; of the QP and
        /// concurrently. The analysis is done using a specified computer of maximal nonzero
        /// equivalence class representatives.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertices of the quiver.</typeparam>
        /// <param name="qp">The quiver with potential.</param>
        /// <param name="periods">A collection of consecutive non-empty periods of the QP that are
        /// jointly exhaustive and mutually exclusive.</param>
        /// <param name="settings">The settings for the analysis.</param>
        /// <param name="computer">A computer of maximal nonzero equivalence class representatives.</param>
        /// <returns>The results of the analysis.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="qp"/> is
        /// <see langword="null"/>,
        /// or <paramref name="periods"/> is <see langword="null"/>,
        /// or <paramref name="settings"/> is <see langword="null"/>,
        /// or <paramref name="computer"/> is <see langword="null"/>.</exception>
        /// <exception cref="NotSupportedException">The potential of <paramref name="qp"/> has a
        /// cycle with coefficient not equal to either of -1 and +1,
        /// or some arrow occurs multiple times in a single cycle of the potential of
        /// <paramref name="qp"/>.</exception>
        /// <exception cref="ArgumentException">For some arrow in the potential of
        /// <paramref name="qp"/> and sign, the arrow is contained in more than one cycle of that
        /// sign, or some of the periods in <paramref name="periods"/> overlap, or the union of all
        /// periods in <paramref name="periods"/> is not precisely the collection of all vertices
        /// in the quiver.</exception>
        /// <remarks>
        /// <para>Some validation of <paramref name="periods"/> is done, but
        /// <paramref name="periods"/> is not verified to constitute a sequence of consecutive
        /// &quot;periods&quot; of the QP.</para>
        /// </remarks>
        public IQPAnalysisResults<TVertex> AnalyzeUtilizingPeriodicityConcurrently<TVertex>(
            QuiverWithPotential<TVertex> qp,
            IEnumerable<IEnumerable<TVertex>> periods,
            QPAnalysisSettings settings,
            IMaximalNonzeroEquivalenceClassRepresentativeComputer computer)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            if (qp is null) throw new ArgumentNullException(nameof(qp));
            if (periods is null) throw new ArgumentNullException(nameof(periods));
            if (settings is null) throw new ArgumentNullException(nameof(settings));
            if (computer is null) throw new ArgumentNullException(nameof(computer));

            // Simply get the underlying semimonomial unbound quiver and analyze it using the appropriate analyzer
            var semimonomialUnboundQuiver = SemimonomialUnboundQuiverFactory.CreateSemimonomialUnboundQuiverFromQP(qp);
            var suqAnalyzer = new SemimonomialUnboundQuiverAnalyzer();
            var suqSettings = AnalysisSettingsFactory.CreateSemimonomialUnboundQuiverAnalysisSettings(settings);
            var suqResults = suqAnalyzer.AnalyzeUtilizingPeriodicityConcurrently(semimonomialUnboundQuiver, periods, suqSettings, computer);
            var results = AnalysisResultsFactory.CreateQPAnalysisResults(suqResults);

            return results;
        }

        // After figuring out the use cases (high performance?) for these methods and figuring out
        // whether to change the interface, implement the counterparts of the methods in
        // SemimonomialUnboundQuiverAnalyzer and have the public methods of this class just be a
        // wrapper for said methods in SemimonomialUnboundQuiverAnalyzer (like the Analyze method
        // is right now).
        #region Code to refactor
        public AnalysisResultsForSingleStartingVertexOld<TVertex> AnalyzeWithStartingVertex<TVertex>(
            QuiverWithPotential<TVertex> qp,
            TVertex startingVertex,
            TransformationRuleTreeNode<TVertex> transformationRuleTree,
            QPAnalysisSettings settings)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            // Parameter validation
            if (qp == null) throw new ArgumentNullException(nameof(qp));
            if (!qp.Quiver.Vertices.Contains(startingVertex)) throw new ArgumentException($"The QP does not contain the starting vertex {startingVertex}.");

            // Set up for analysis/graph search
            var state = new AnalysisStateForSingleStartingVertexOld<TVertex>(startingVertex);
            bool cancellativityFailed = false;
            bool tooLongPathEncountered = false;

            // Analysis/graph search
            // Keep a stack of "recently explored" nodes
            // In every iteration, pop a recently explored node from the stack and explore (determine
            // the equivalence classes of) its child nodes.
            // It would be cleaner to in every iteration explore the current node (determine its equivalence class)
            // and discover its child nodes (push new equivalence class representatives (which may overlap?) onto
            // the stack for future iterations, but this makes it non-trivial to keep track of the maximal nonzero
            // equivalence classes
            while (state.Stack.Count > 0)
            {
                var node = state.Stack.Pop();
                var result = ExploreChildNodes(qp, transformationRuleTree, state, node, settings);
                if (result == ExploreChildNodesResult.PathHasNoNonzeroExtension) state.maximalPathRepresentatives.Add(node);
                else if (result == ExploreChildNodesResult.NonCancellativityDetected)
                {
                    cancellativityFailed = true;
                    break;
                }
                else if (result == ExploreChildNodesResult.TooLongPath)
                {
                    tooLongPathEncountered = true;
                    break;
                }
            }

            // Return results
            var results = new AnalysisResultsForSingleStartingVertexOld<TVertex>(state, cancellativityFailed, tooLongPathEncountered);
            return results;
        }

        private ExploreChildNodesResult ExploreChildNodes<TVertex>(
            QuiverWithPotential<TVertex> qp,
            TransformationRuleTreeNode<TVertex> transformationRuleTree,
            AnalysisStateForSingleStartingVertexOld<TVertex> state,
            SearchTreeNodeOld<TVertex> parentNode,
            QPAnalysisSettings settings)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            bool pathHasNonzeroExtension = false; // Path has no nonzero extension until proven otherwise
            // Explore every child node (determine its equivalence class)
            foreach (var nextVertex in qp.Quiver.AdjacencyLists[parentNode.Vertex])
            {
                var result = ExploreChildNode(transformationRuleTree, state, parentNode, nextVertex, settings);
                switch (result)
                {
                    case ExploreChildNodeResult.NotZeroEquivalent:
                        pathHasNonzeroExtension = true;
                        break;
                    case ExploreChildNodeResult.NonCancellativityDetected:
                        return ExploreChildNodesResult.NonCancellativityDetected;
                    case ExploreChildNodeResult.TooLongPath:
                        return ExploreChildNodesResult.TooLongPath;
                }
            }

            return pathHasNonzeroExtension ? ExploreChildNodesResult.PathHasNonzeroExtension : ExploreChildNodesResult.PathHasNoNonzeroExtension;
        }

        /// <returns>A boolean value indicating whether the path correspondings to the explored
        /// child node is nonzero (up to equivalence).</returns>
        private ExploreChildNodeResult ExploreChildNode<TVertex>(
            TransformationRuleTreeNode<TVertex> transformationRuleTree,
            AnalysisStateForSingleStartingVertexOld<TVertex> state,
            SearchTreeNodeOld<TVertex> parentNode,
            TVertex nextVertex,
            QPAnalysisSettings settings)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            // Either the child node has not already been discovered, or the child node was
            // discovered during an equivalence class search.

            if (parentNode.Children.TryGetValue(nextVertex, out var childNode))
            {
                // The child node has already been discovered.
                // Do a non-cancellativity check (if we are to detect non-cancellativity):
                //     If the child node is zero, everything is fine.
                //     If a different arrow was added to the origin than to the parent to get child node (i.e., origin and parent have different last vertices), things are fine.
                //     Else, make sure that the origin is equivalent to the parent
                // (otherwise, the QP is not cancellative).
                if (
                    settings.DetectCancellativityFailure &&
                    !state.NodeIsZeroEquivalent(childNode) &&
                    parentNode.Vertex.Equals(childNode.Origin.Vertex) &&
                    state.EquivalenceClasses.FindSet(parentNode) != state.EquivalenceClasses.FindSet(childNode.Origin))
                {
                    return ExploreChildNodeResult.NonCancellativityDetected;
                }
            }
            else
            {
                // The child node has not been discovered, so let's discover it by inserting it
                // into the search tree.
                childNode = state.InsertChildNode(parentNode, nextVertex, parentNode);
            }

            if (childNode.Explored) return state.NodeIsZeroEquivalent(childNode) ? ExploreChildNodeResult.ZeroEquivalent : ExploreChildNodeResult.NotZeroEquivalent;

            // Code for equivalence class searching begins here
            // Stack containing all the nodes to explore (by path transformation)
            var equivClassStack = new Stack<SearchTreeNodeOld<TVertex>>(state.EquivalenceClasses.GetSet(childNode));

            // This could happen with the current code, namely when the current childNode was encountered
            // in a previous call to ExploreChildNodes (in EquivClassExploreNode, as transformationNode)
            // and 'node' was found to be zero equivalent (in which case the search is cancelled
            // before transformationNode is explored in EquivClassExploreNode)
            // Thought: Would probably be good to have several Explored properties (or an
            // enum-valued Explored property) containing information about the extent to which the
            // node is explored (explored-as-in-encountered-in-EquivClassExploreNode would be
            // useful here; more easily understood and maintained than this comment methinks)
            if (equivClassStack.Count != 1) return ExploreChildNodeResult.ZeroEquivalent;

            while (equivClassStack.Count > 0)
            {
                var equivalentNode = equivClassStack.Pop();
                if (equivalentNode.Explored) continue;
                if (settings.UseMaxLength && equivalentNode.PathLengthInVertices > settings.MaxPathLength + 1) return ExploreChildNodeResult.TooLongPath;
                var exploredNodeWasFoundZeroEquivalent = EquivClassExploreNode(transformationRuleTree, equivalentNode, state, equivClassStack, parentNode);
                if (exploredNodeWasFoundZeroEquivalent)
                {
                    return ExploreChildNodeResult.ZeroEquivalent;
                }
            }

            // Child node was not zero-equivalent
            state.Stack.Push(childNode);
            return ExploreChildNodeResult.NotZeroEquivalent;
        }

        /// <returns>A boolean value indicating whether the node was found to be zero-equivalent.
        /// That is, the returned value is <see langword="true"/> <em>only</em> if the node is
        /// zero-equivalent but may be <see langword="false"/> even if the node is zero-equivalent.</returns>
        /// <remarks>A node is found to be zero-equivalent either if its path can be killed or if
        /// the path is equivalent to (up to replacement) to a path that has previously been
        /// determined to be zero-equivalent.</remarks>
        private static bool EquivClassExploreNode<TVertex>(
            TransformationRuleTreeNode<TVertex> transformationRuleTree,
            SearchTreeNodeOld<TVertex> node,
            AnalysisStateForSingleStartingVertexOld<TVertex> state,
            Stack<SearchTreeNodeOld<TVertex>> equivClassStack,
            SearchTreeNodeOld<TVertex> origin)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            node.Explored = true;

            var trailingVertexPath = new List<TVertex>(); // In reversed order
            foreach (var endingNode in node.ReversePathOfNodes) // The last vertex of endingNode is the last vertex in the subpath
            {
                var transformationNode = transformationRuleTree;
                foreach (var startingNode in endingNode.ReversePathOfNodes) // The last vertex of startingNode is the first vertex in the subpath
                {
                    var pathVertex = startingNode.Vertex;
                    if (!transformationNode.Children.TryGetValue(pathVertex, out transformationNode)) break;

                    if (transformationNode.CanBeKilled)
                    {
                        state.EquivalenceClasses.Union(node, state.ZeroDummyNode);
                        return true;
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
                        // path has the same first vertex (which it does for the QPs under consideration)
                        var firstReplacementNode = startingNode;
                        var curNode = firstReplacementNode;

                        foreach (var vertex in transformationNode.ReplacementPath.Vertices.Skip(1))
                        {
                            curNode = state.GetInsertChildNode(curNode, vertex, origin);
                        }

                        // Add search tree nodes for the trailing path
                        foreach (var vertex in trailingVertexPath.Reversed())
                        {
                            curNode = state.GetInsertChildNode(curNode, vertex, origin);
                        }

                        var transformedNode = curNode;
                        if (state.NodeIsZeroEquivalent(transformedNode))
                        {
                            state.EquivalenceClasses.Union(node, transformedNode);
                            return true;
                        }

                        // transformedNode.Explored may be true and is then a node that we have
                        // already encountered during the equivalence class search.
                        // (It cannot be an explored node of some *other* equivalence class,
                        // because then this current equivalence class search would not have
                        // started in the first place.)
                        //if (!transformedNode.Explored)
                        if (!state.EquivalenceClasses.FindSet(node).Equals(state.EquivalenceClasses.FindSet(transformedNode)))
                        {
                            state.EquivalenceClasses.Union(node, transformedNode);
                            equivClassStack.Push(transformedNode);
                        }
                    }
                }

                trailingVertexPath.Add(endingNode.Vertex);
            }

            return false;
        }

        public MaximalNonzeroEquivalenceClassRepresentativesResults<TVertex> ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt<TVertex>(
            QuiverWithPotential<TVertex> qp,
            TVertex startingVertex,
            TransformationRuleTreeNode<TVertex> transformationRuleTree,
            QPAnalysisSettings settings)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            var bogusPath = new Path<TVertex>(qp.Quiver.Vertices.First());
            var analysisResults = AnalyzeWithStartingVertex(qp, startingVertex, transformationRuleTree, settings);
            var outputResults = new MaximalNonzeroEquivalenceClassRepresentativesResults<TVertex>(
                analysisResults.NonCancellativityDetected ? CancellativityTypes.Cancellativity : CancellativityTypes.None,
                analysisResults.TooLongPathEncountered,
                analysisResults.MaximalPathRepresentatives.Select(node => node.Path),
                longestPathEncountered: bogusPath); // bogus path because this is an old method and the longest path feature is new
            return outputResults;
        }
        #endregion
    }
}
