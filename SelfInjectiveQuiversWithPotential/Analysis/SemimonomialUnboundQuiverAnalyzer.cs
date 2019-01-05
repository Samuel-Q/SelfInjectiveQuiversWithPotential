using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataStructures;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    /// <summary>
    /// This class is used to analyze <see cref="SemimonomialUnboundQuiver{TVertex}"/>s.
    /// </summary>
    public class SemimonomialUnboundQuiverAnalyzer : ISemimonomialUnboundQuiverAnalyzer
    {
        private readonly IMaximalNonzeroEquivalenceClassRepresentativeComputer defaultComputer;

        /// <summary>
        /// Initializes a new instance of the <see cref="SemimonomialUnboundQuiverAnalyzer"/> class.
        /// </summary>
        public SemimonomialUnboundQuiverAnalyzer() : this(new MaximalNonzeroEquivalenceClassRepresentativeComputer())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SemimonomialUnboundQuiverAnalyzer"/> class
        /// with the specified computer of maximal nonzero equivalence class representatives to use
        /// by default when analyzing.
        /// </summary>
        /// <param name="defaultComputer">The default computer of maximal nonzero equivalence class
        /// representatives.</param>
        /// <exception cref="ArgumentNullException"><paramref name="defaultComputer"/> is
        /// <see langword="null"/>.</exception>
        public SemimonomialUnboundQuiverAnalyzer(IMaximalNonzeroEquivalenceClassRepresentativeComputer defaultComputer)
        {
            this.defaultComputer = defaultComputer ?? throw new ArgumentNullException(nameof(defaultComputer));
        }

        /// <summary>
        /// Analyzes a semimonomial unbound quiver with the default computer of maximal nonzero
        /// equivalence class representatives.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertices of the quiver.</typeparam>
        /// <param name="unboundQuiver">The unbound quiver.</param>
        /// <param name="settings">The settings for the analysis.</param>
        /// <returns>The results of the analysis.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="unboundQuiver"/> is
        /// <see langword="null"/>, or <paramref name="settings"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="NotSupportedException">The semimonomial ideal of
        /// <paramref name="unboundQuiver"/> has two distinct non-monomial generators
        /// <c>p1 - q1</c> and <c>p2 - q2</c> where <c>p1, q1, p2, q2</c> are not all distinct.</exception>
        /// <exception cref="ArgumentException">The semimonomial ideal of
        /// <paramref name="unboundQuiver"/> has a non-monomial generator <c>p - q</c> where the
        /// paths <c>p</c> and <c>q</c> do not have the same endpoints.</exception>
        public ISemimonomialUnboundQuiverAnalysisResults<TVertex> Analyze<TVertex>(SemimonomialUnboundQuiver<TVertex> unboundQuiver, SemimonomialUnboundQuiverAnalysisSettings settings)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            return Analyze(unboundQuiver, settings, defaultComputer);
        }

        /// <summary>
        /// Analyzes a semimonomial unbound quiver with a default computer of maximal nonzero
        /// equivalence class representatives.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertices of the quiver.</typeparam>
        /// <param name="unboundQuiver">The unbound quiver.</param>
        /// <param name="settings">The settings for the analysis.</param>
        /// <param name="computer">A computer of maximal nonzero equivalence class representatives.</param>
        /// <returns>The results of the analysis.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="unboundQuiver"/> is
        /// <see langword="null"/>, or <paramref name="settings"/> is <see langword="null"/>,
        /// or <paramref name="computer"/> is <see langword="null"/>.</exception>
        /// <exception cref="NotSupportedException">The semimonomial ideal of
        /// <paramref name="unboundQuiver"/> has two distinct non-monomial generators
        /// <c>p1 - q1</c> and <c>p2 - q2</c> where <c>p1, q1, p2, q2</c> are not all distinct.</exception>
        /// <exception cref="ArgumentException">The semimonomial ideal of
        /// <paramref name="unboundQuiver"/> has a non-monomial generator <c>p - q</c> where the
        /// paths <c>p</c> and <c>q</c> do not have the same endpoints.</exception>
        public ISemimonomialUnboundQuiverAnalysisResults<TVertex> Analyze<TVertex>(
            SemimonomialUnboundQuiver<TVertex> unboundQuiver,
            SemimonomialUnboundQuiverAnalysisSettings settings,
            IMaximalNonzeroEquivalenceClassRepresentativeComputer computer)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            if (unboundQuiver is null) throw new ArgumentNullException(nameof(unboundQuiver));
            if (settings is null) throw new ArgumentNullException(nameof(settings));
            if (computer is null) throw new ArgumentNullException(nameof(computer));

            var computationSettings = AnalysisSettingsFactory.CreateMaximalNonzeroEquivalenceClassRepresentativeComputationSettings(settings);
            var transformationRuleTreeCreator = new TransformationRuleTreeCreator();
            var transformationRuleTree = transformationRuleTreeCreator.CreateTransformationRuleTree(unboundQuiver);

            var maximalPathRepresentatives = new Dictionary<TVertex, IEnumerable<Path<TVertex>>>();
            var nakayamaPermutationDict = new Dictionary<TVertex, TVertex>();

            var mainResults = SemimonomialUnboundQuiverAnalysisMainResults.None;
            Path<TVertex> longestPath = null;
            foreach (var startingVertex in unboundQuiver.Quiver.Vertices)
            {
                var representativesResult = computer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(unboundQuiver.Quiver, startingVertex, transformationRuleTree, computationSettings);
                if (longestPath is null || representativesResult.LongestPathEncountered.Length > longestPath.Length)
                    longestPath = representativesResult.LongestPathEncountered;

                if (representativesResult.WeakCancellativityFailureDetected) mainResults |= SemimonomialUnboundQuiverAnalysisMainResults.NotWeaklyCancellative;
                if (representativesResult.CancellativityFailureDetected) mainResults |= SemimonomialUnboundQuiverAnalysisMainResults.NotCancellative;
                if (representativesResult.TooLongPathEncountered) mainResults |= SemimonomialUnboundQuiverAnalysisMainResults.Aborted;

                if ((representativesResult.WeakCancellativityFailureDetected && settings.TerminateEarlyIfWeakCancellativityFails)
                    || (representativesResult.CancellativityFailureDetected && settings.TerminateEarlyIfCancellativityFails)
                    || (representativesResult.TooLongPathEncountered))
                {
                    return new SemimonomialUnboundQuiverAnalysisResults<TVertex>(mainResults, null, null, longestPath);
                }

                var maximalNonzeroEquivalenceClassRepresentatives = representativesResult.MaximalNonzeroEquivalenceClassRepresentatives;
                maximalPathRepresentatives[startingVertex] = maximalNonzeroEquivalenceClassRepresentatives;
                int numMaximalNonzeroPathClasses = maximalNonzeroEquivalenceClassRepresentatives.Count();
                Debug.Assert(numMaximalNonzeroPathClasses > 0, "Analysis with starting vertex found 0 maximal nonzero classes.");
                if (numMaximalNonzeroPathClasses > 1)
                {
                    mainResults |= SemimonomialUnboundQuiverAnalysisMainResults.MultipleMaximalNonzeroClasses;
                    if (settings.TerminateEarlyOnMultiDimensionalSocle)
                    {
                        return new SemimonomialUnboundQuiverAnalysisResults<TVertex>(mainResults, null, null, longestPath);
                    }
                }
                else
                {
                    var endingPoint = maximalNonzeroEquivalenceClassRepresentatives.Single().EndingPoint;
                    // Check if the tentative Nakayama permutation fails to be injective.
                    if (nakayamaPermutationDict.ContainsValue(endingPoint))
                    {
                        mainResults |= SemimonomialUnboundQuiverAnalysisMainResults.NonInjectiveTentativeNakayamaPermutation;
                        if (settings.TerminateEarlyIfNakayamaPermutationFails)
                        {
                            return new SemimonomialUnboundQuiverAnalysisResults<TVertex>(mainResults, null, null, longestPath);
                        }
                    }
                    else nakayamaPermutationDict[startingVertex] = endingPoint;
                }
            }

            mainResults |= SemimonomialUnboundQuiverAnalysisMainResults.Success;
            NakayamaPermutation<TVertex> nakayamaPermutation = null;
            if (mainResults.IndicatesSelfInjectivity())
            {
                nakayamaPermutation = new NakayamaPermutation<TVertex>(nakayamaPermutationDict);
            }

            return new SemimonomialUnboundQuiverAnalysisResults<TVertex>(mainResults, maximalPathRepresentatives, nakayamaPermutation, longestPath);
        }

        /// <summary>
        /// Analyzes a semimonomial unbound quiver in a way that utilizes the
        /// &quot;periodicity&quot; of the unbound quiver and concurrently.
        /// The analysis is done using a specified computer of maximal nonzero
        /// equivalence class representatives.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertices of the quiver.</typeparam>
        /// <param name="unboundQuiver">The unbound quiver.</param>
        /// <param name="periods">A collection of consecutive non-empty periods of the unbound
        /// quiver that are jointly exhaustive and mutually exclusive.</param>
        /// <param name="settings">The settings for the analysis.</param>
        /// <param name="computer">A computer of maximal nonzero equivalence class representatives.</param>
        /// <returns>The results of the analysis.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="unboundQuiver"/> is
        /// <see langword="null"/>,
        /// or <paramref name="periods"/> is <see langword="null"/>
        /// or <paramref name="settings"/> is <see langword="null"/>,
        /// or <paramref name="computer"/> is <see langword="null"/>.</exception>
        /// <exception cref="NotSupportedException">The semimonomial ideal of
        /// <paramref name="unboundQuiver"/> has two distinct non-monomial generators
        /// <c>p1 - q1</c> and <c>p2 - q2</c> where <c>p1, q1, p2, q2</c> are not all distinct.</exception>
        /// <exception cref="ArgumentException">The semimonomial ideal of
        /// <paramref name="unboundQuiver"/> has a non-monomial generator <c>p - q</c> where the
        /// paths <c>p</c> and <c>q</c> do not have the same endpoints,
        /// or some of the periods in <paramref name="periods"/> overlap, or the union of all
        /// periods in <paramref name="periods"/> is not precisely the collection of all vertices
        /// in the quiver.</exception>
        /// <remarks>
        /// <para>Some validation of <paramref name="periods"/> is done, but
        /// <paramref name="periods"/> is not verified to constitute a sequence of consecutive
        /// &quot;periods&quot; of the unbound quiver.</para>
        /// </remarks>
        public ISemimonomialUnboundQuiverAnalysisResults<TVertex> AnalyzeUtilizingPeriodicityConcurrently<TVertex>(
            SemimonomialUnboundQuiver<TVertex> unboundQuiver,
            IEnumerable<IEnumerable<TVertex>> periods,
            SemimonomialUnboundQuiverAnalysisSettings settings,
            IMaximalNonzeroEquivalenceClassRepresentativeComputer computer)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            if (unboundQuiver is null) throw new ArgumentNullException(nameof(unboundQuiver));
            if (periods is null) throw new ArgumentNullException(nameof(periods));
            if (settings is null) throw new ArgumentNullException(nameof(settings));
            if (computer is null) throw new ArgumentNullException(nameof(computer));

            var computationSettings = AnalysisSettingsFactory.CreateMaximalNonzeroEquivalenceClassRepresentativeComputationSettings(settings);
            var transformationRuleTreeCreator = new TransformationRuleTreeCreator();
            var transformationRuleTree = transformationRuleTreeCreator.CreateTransformationRuleTree(unboundQuiver);

            // Remark: We cannot terminate before the CreateTransformationRuleTree method is called,
            // because it is responsible for throwing an ArgumentException.
            var verticesInPeriods = new HashSet<TVertex>();
            foreach (var period in periods)
            {
                if (period.Any(vertex => verticesInPeriods.Contains(vertex)))
                    throw new ArgumentException("The periods overlap.");

                verticesInPeriods.UnionWith(period);
            }

            var quiver = unboundQuiver.Quiver;
            if (!quiver.Vertices.SetEquals(verticesInPeriods))
                throw new ArgumentException("The union of the periods is not the collection of all vertices in the quiver.");

            var maximalPathRepresentativesDict = new ConcurrentDictionary<TVertex, IEnumerable<Path<TVertex>>>();
            var nakayamaPermutationDict = new ConcurrentDictionary<TVertex, TVertex>();

            if (!periods.Any())
            {
                return ExtendResultForPeriodToEntireQuiver(
                    periods,
                    maximalPathRepresentativesDict,
                    nakayamaPermutationDict,
                    null,
                    false,
                    false,
                    false,
                    false,
                    false);
            }

            // The period of the vertices whose socles to investigate.
            var firstPeriod = periods.First();

            // The period into which all the analyzed vertices are mapped by the tentative Nakayama permutation.
            ISet<TVertex> targetPeriod = null;

            // These aren't used, are they?
            var cts = new CancellationTokenSource();
            var options = new ParallelOptions()
            {
                CancellationToken = cts.Token
            };

            // Variable into which to put the output of the execution that doesn't go into the
            // some other variable (maximalPathRepresentatives and nakayamaPermutationDict).
            // Called globalState because it aggregates all the local states.
            var globalState = (
                longestPath: (Path<TVertex>)null,
                nonCancellativityDetected: false,
                tooLongPathEncountered: false,
                multiDimensionalSocleEncountered: false,
                permutationFails: false,
                cancelledEarly: false);
            var globalStateLock = new object();

            Parallel.ForEach(
                // The enumerable to iterate over.
                firstPeriod,
                // Options for the parallel execution, used to pass a cancellation token.
                options,
                // localInit delegate used to get the initial value of localState for each task/"thread".
                () => (
                    longestPath: (Path<TVertex>)null,
                    nonCancellativityDetected: false,
                    tooLongPathEncountered: false,
                    multiDimensionalSocleEncountered: false,
                    permutationFails: false,
                    cancelledEarly: false),
                // body delegate that contains the loop body.
                (startingVertex, loopState, localState) =>
                {
                    if (loopState.ShouldExitCurrentIteration) return localState;

                    var representativesResult = computer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(unboundQuiver.Quiver, startingVertex, transformationRuleTree, computationSettings);
                    if (localState.longestPath is null || representativesResult.LongestPathEncountered.Length > localState.longestPath.Length)
                        localState.longestPath = representativesResult.LongestPathEncountered;

                    if (representativesResult.CancellativityFailureDetected)
                    {
                        localState.nonCancellativityDetected = true;
                        if (settings.TerminateEarlyIfCancellativityFails)
                        {
                            localState.cancelledEarly = true;
                            loopState.Stop();
                        }
                    }

                    if (representativesResult.TooLongPathEncountered)
                    {
                        localState.tooLongPathEncountered = true;
                        localState.cancelledEarly = true;
                        loopState.Stop();
                    }

                    if (loopState.ShouldExitCurrentIteration)
                    {
                        return localState;
                    }

                    var maximalNonzeroEquivalenceClassRepresentatives = representativesResult.MaximalNonzeroEquivalenceClassRepresentatives;
                    maximalPathRepresentativesDict[startingVertex] = maximalNonzeroEquivalenceClassRepresentatives;
                    int numMaximalNonzeroPathClasses = maximalNonzeroEquivalenceClassRepresentatives.Count();
                    if (numMaximalNonzeroPathClasses > 1)
                    {
                        localState.multiDimensionalSocleEncountered = true;
                        if (settings.TerminateEarlyOnMultiDimensionalSocle)
                        {
                            localState.cancelledEarly = true;
                            loopState.Stop();
                            return localState;
                        }
                    }

                    var endingPoint = maximalNonzeroEquivalenceClassRepresentatives.Single().EndingPoint;
                    lock (nakayamaPermutationDict)
                    {
                        // If this was the first vertex analyzed, set the target period.
                        if (targetPeriod is null)
                        {
                            targetPeriod = periods.First(period => period.Contains(endingPoint)).ToSet();
                        }
                        // Else check that the current vertex is mapped into the same period as the first vertex.
                        // If not, there cannot be a Nakayama permutation.
                        else if (!targetPeriod.Contains(endingPoint))
                        {
                            localState.permutationFails = true;
                            if (settings.TerminateEarlyIfNakayamaPermutationFails)
                            {
                                localState.cancelledEarly = true;
                                loopState.Stop();
                                return localState;
                            }
                        }

                        // If the current vertex maps to a vertex to which some other vertex have
                        // already been mapped.
                        // If not, there cannot be a Nakayama permutation.
                        if (nakayamaPermutationDict.Values.Contains(endingPoint))
                        {
                            localState.permutationFails = true;
                            if (settings.TerminateEarlyIfNakayamaPermutationFails)
                            {
                                localState.cancelledEarly = true;
                                loopState.Stop();
                                return localState;
                            }
                        }

                        nakayamaPermutationDict[startingVertex] = endingPoint;
                        return localState;
                    }
                },
                // localFinally delegate used to output the local states to the outside.
                localState =>
                {
                    lock (globalStateLock)
                    {
                        if (globalState.longestPath is null
                        || (localState.longestPath != null && localState.longestPath.Length > globalState.longestPath.Length))
                        {
                            globalState.longestPath = localState.longestPath;
                        }

                        if (localState.tooLongPathEncountered) globalState.tooLongPathEncountered = true;
                        if (localState.nonCancellativityDetected) globalState.nonCancellativityDetected = true;
                        if (localState.multiDimensionalSocleEncountered) globalState.multiDimensionalSocleEncountered = true;
                        if (localState.permutationFails) localState.permutationFails = true;
                        if (localState.cancelledEarly) globalState.cancelledEarly = true;
                    }
                });

            return ExtendResultForPeriodToEntireQuiver(
                periods,
                maximalPathRepresentativesDict,
                nakayamaPermutationDict,
                globalState.longestPath,
                globalState.tooLongPathEncountered,
                globalState.nonCancellativityDetected,
                globalState.multiDimensionalSocleEncountered,
                globalState.permutationFails,
                globalState.cancelledEarly);
        }

        /// <summary>
        /// Extends the results of the analysis of a single period to all periods.
        /// </summary>
        /// <typeparam name="TVertex"></typeparam>
        /// <param name="periods"></param>
        /// <param name="maximalPathRepresentativesDict"></param>
        /// <param name="nakayamaPermutationDict"></param>
        /// <param name="tooLongPathEncountered"></param>
        /// <param name="nonCancellativityDetected"></param>
        /// <param name="multiDimensionalSocleEncountered"></param>
        /// <param name="permutationFails"></param>
        /// <param name="cancelledEarly"></param>
        /// <returns></returns>
        /// <remarks>
        /// <para>Even if the analysis was cancelled early, the partial results can be extended to
        /// all periods.</para>
        /// <para><paramref name="maximalPathRepresentativesDict"/> and
        /// <paramref name="nakayamaPermutationDict"/> are mutated.</para>
        /// </remarks>
        private ISemimonomialUnboundQuiverAnalysisResults<TVertex> ExtendResultForPeriodToEntireQuiver<TVertex>(
            IEnumerable<IEnumerable<TVertex>> periodsEnumerable,
            IDictionary<TVertex, IEnumerable<Path<TVertex>>> maximalPathRepresentativesDict,
            IDictionary<TVertex, TVertex> nakayamaPermutationDict,
            Path<TVertex> longestPathEncountered,
            bool tooLongPathEncountered,
            bool nonCancellativityDetected,
            bool multiDimensionalSocleEncountered,
            bool permutationFails,
            bool cancelledEarly)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            // Get main result
            var mainResults = SemimonomialUnboundQuiverAnalysisMainResults.None;
            if (!cancelledEarly) mainResults |= SemimonomialUnboundQuiverAnalysisMainResults.Success;
            if (tooLongPathEncountered) mainResults |= SemimonomialUnboundQuiverAnalysisMainResults.Aborted;
            if (nonCancellativityDetected) mainResults |= SemimonomialUnboundQuiverAnalysisMainResults.NotCancellative;
            bool isSelfInjective = mainResults.IndicatesSelfInjectivityUsingStrongCancellativity();

            var periods = new CircularList<IReadOnlyList<TVertex>>(periodsEnumerable.Select(period => period.ToList()));
            if (!periods.Any())
            {
                return new SemimonomialUnboundQuiverAnalysisResults<TVertex>(
                    mainResults,
                    maximalPathRepresentativesDict.ToReadOnlyDictionary(),
                    new NakayamaPermutation<TVertex>(nakayamaPermutationDict),
                    longestPathEncountered);
            }

            var firstPeriod = periods.First().ToList();

            // Dictionary mapping a vertex to the tuple (periodIndex, indexInPeriod).
            var vertexToPeriodCoordinatesDict = new Dictionary<TVertex, (int periodIndex, int indexInPeriod)>();
            foreach (var (period, periodIndex) in periods.EnumerateWithIndex())
            {
                foreach (var (vertex, indexInPeriod) in period.EnumerateWithIndex())
                {
                    vertexToPeriodCoordinatesDict[vertex] = (periodIndex, indexInPeriod);
                }
            }

            // Extend the dictionary of maximal path representatives
            foreach (var (period, periodIndex) in periods.EnumerateWithIndex().Skip(1))
            {
                foreach (var (vertex, indexInPeriod) in period.EnumerateWithIndex())
                {
                    var vertexInFirstPeriod = firstPeriod[indexInPeriod];

                    // If vertexInFirstPeriod was not analyzed, there is nothing to extend.
                    if (!maximalPathRepresentativesDict.TryGetValue(vertexInFirstPeriod, out var maximalPathRepsInFirstPeriod))
                        continue;

                    var maximalPathRepresentatives = maximalPathRepsInFirstPeriod.Select(
                        path =>
                        {
                            var vertices = path.Vertices.Select(v => TranslateVertex(v, periodIndex));
                            return new Path<TVertex>(vertices);
                        });

                    maximalPathRepresentativesDict[vertex] = maximalPathRepresentatives;
                }
            }

            // Extend the Nakayama permutation dictionary
            if (isSelfInjective)
            {
                foreach (var (period, periodIndex) in periods.EnumerateWithIndex().Skip(1))
                {
                    foreach (var (vertex, indexInPeriod) in period.EnumerateWithIndex())
                    {
                        var vertexInFirstPeriod = firstPeriod[indexInPeriod];

                        // If vertexInFirstPeriod has no mapping pair, there is nothing to extend.
                        if (!nakayamaPermutationDict.TryGetValue(vertexInFirstPeriod, out var targetForFirstPeriod))
                        {
                            Debug.Fail($"The vertex {vertexInFirstPeriod} has no Nakayama mapping pair " +
                                $"despite the algebra being self-injective.");
                            continue;
                        }

                        var targetVertex = TranslateVertex(targetForFirstPeriod, periodIndex);
                        nakayamaPermutationDict[vertex] = targetVertex;
                    }
                }
            }

            var nakayamaPermutation = isSelfInjective ? new NakayamaPermutation<TVertex>(nakayamaPermutationDict) : null;
            var results = new SemimonomialUnboundQuiverAnalysisResults<TVertex>(
                mainResults,
                maximalPathRepresentativesDict.ToReadOnlyDictionary(),
                nakayamaPermutation,
                longestPathEncountered);
            return results;

            // Translates a vertex by the specified number of periods
            TVertex TranslateVertex(TVertex vertex, int translationDistance)
            {
                var (periodIndex, indexInPeriod) = vertexToPeriodCoordinatesDict[vertex];
                return periods[periodIndex + translationDistance][indexInPeriod];
            }
        }
    }
}
