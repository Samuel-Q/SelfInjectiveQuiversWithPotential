using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            bool nakayamaPermutationCouldExist = true;
            Path<TVertex> longestPath = null;
            foreach (var startingVertex in unboundQuiver.Quiver.Vertices)
            {
                var representativesResult = computer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(unboundQuiver.Quiver, startingVertex, transformationRuleTree, computationSettings);
                if (longestPath is null || representativesResult.LongestPathEncountered.Length > longestPath.Length)
                    longestPath = representativesResult.LongestPathEncountered;

                if (representativesResult.NonCancellativityDetected)
                {
                    return new SemimonomialUnboundQuiverAnalysisResults<TVertex>(
                        SemimonomialUnboundQuiverAnalysisMainResult.NotCancellative, null, null, longestPath);
                }
                else if (representativesResult.TooLongPathEncountered)
                {
                    return new SemimonomialUnboundQuiverAnalysisResults<TVertex>(SemimonomialUnboundQuiverAnalysisMainResult.Aborted, null, null, longestPath);
                }

                var maximalNonzeroEquivalenceClassRepresentatives = representativesResult.MaximalNonzeroEquivalenceClassRepresentatives;
                maximalPathRepresentatives[startingVertex] = maximalNonzeroEquivalenceClassRepresentatives;
                int numMaximalNonzeroPathClasses = maximalNonzeroEquivalenceClassRepresentatives.Count();
                if (numMaximalNonzeroPathClasses == 0) nakayamaPermutationCouldExist = false; // This should never happen?
                if (numMaximalNonzeroPathClasses > 1) nakayamaPermutationCouldExist = false;

                if (nakayamaPermutationCouldExist)
                {
                    var endingPoint = maximalNonzeroEquivalenceClassRepresentatives.Single().EndingPoint;
                    if (nakayamaPermutationDict.ContainsValue(endingPoint)) nakayamaPermutationCouldExist = false; // Fails to be injective
                    else nakayamaPermutationDict[startingVertex] = endingPoint;
                }
            }

            bool nakayamaPermutationExists = nakayamaPermutationCouldExist;

            var mainResult = SemimonomialUnboundQuiverAnalysisMainResult.Success;
            NakayamaPermutation<TVertex> nakayamaPermutation = null;
            if (nakayamaPermutationExists)
            {
                mainResult |= SemimonomialUnboundQuiverAnalysisMainResult.SelfInjective;
                nakayamaPermutation = new NakayamaPermutation<TVertex>(nakayamaPermutationDict);
            }

            return new SemimonomialUnboundQuiverAnalysisResults<TVertex>(mainResult, maximalPathRepresentatives, nakayamaPermutation, longestPath);
        }
    }
}
