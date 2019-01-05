using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using DataStructures;
using SelfInjectiveQuiversWithPotential;
using SelfInjectiveQuiversWithPotential.Analysis;
using SelfInjectiveQuiversWithPotential.Recipes;

namespace SelfInjectiveQuiversWithPotentialTests
{
    /// <summary>
    /// This class provides tests for the
    /// <see cref="IMaximalNonzeroEquivalenceClassRepresentativeComputer"/> interface.
    /// </summary>
    /// <remarks>
    /// <para>Implementations of the
    /// <see cref="IMaximalNonzeroEquivalenceClassRepresentativeComputer"/> interface are tested
    /// via the subclasses of this test fixture.</para>
    /// </remarks>
    [TestFixture]
    public abstract class IMaximalNonzeroEquivalenceClassRepresentativeComputerBaseTestFixture
    {
        /// <summary>
        /// Gets an instance of the class implementing the
        /// <see cref="IMaximalNonzeroEquivalenceClassRepresentativeComputer"/> interface to test.
        /// </summary>
        /// <returns>An instance of the class implementing the
        /// <see cref="IMaximalNonzeroEquivalenceClassRepresentativeComputer"/>.</returns>
        public abstract IMaximalNonzeroEquivalenceClassRepresentativeComputer Computer { get; }

        private MaximalNonzeroEquivalenceClassRepresentativeComputationSettings GetSettings(bool detectNonCancellativity)
        {
            var cancellativityFailureDetection = detectNonCancellativity ? CancellativityTypes.Cancellativity : CancellativityTypes.None;
            return GetSettings(cancellativityFailureDetection);
        }

        private MaximalNonzeroEquivalenceClassRepresentativeComputationSettings GetSettings(CancellativityTypes cancellativityFailureDetection)
        {
            return new MaximalNonzeroEquivalenceClassRepresentativeComputationSettings(cancellativityFailureDetection);
        }

        private MaximalNonzeroEquivalenceClassRepresentativeComputationSettings GetSettings(
            CancellativityTypes cancellativityFailureDetection,
            int maxPathLength = -1,
            EarlyTerminationConditions earlyTerminationCondition = EarlyTerminationConditions.None)
        {
            return new MaximalNonzeroEquivalenceClassRepresentativeComputationSettings(cancellativityFailureDetection, maxPathLength, earlyTerminationCondition);
        }

        private QPAnalysisSettings GetQPAnalysisSettings(
            bool detectNonCancellativity = false,
            int maxPathLength = -1,
            EarlyTerminationConditions earlyTerminationCondition = EarlyTerminationConditions.None)
        {
            var cancellativityFailureDetection = detectNonCancellativity ? CancellativityTypes.Cancellativity : CancellativityTypes.None;
            return new QPAnalysisSettings(cancellativityFailureDetection, maxPathLength, earlyTerminationCondition);
        }

        private void DoSetup(QuiverWithPotential<int> qp, out TransformationRuleTreeNode<int> ruleTree)
        {
            var creator = new TransformationRuleTreeCreator();
            ruleTree = creator.CreateTransformationRuleTree(qp);
        }

        [Test]
        public void ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt_TriforceCorner()
        {
            var settings = GetSettings(detectNonCancellativity: false);
            var qp = UsefulQPs.GetTriangleQP(3);
            DoSetup(qp, out var ruleTree);

            var representatives = Computer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp.Quiver, 1, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives;
            var representativeArrowPaths = representatives.Select(path => path.Arrows).ToList();
            var expectedRepresentative = new Arrow<int>[] { new Arrow<int>(1, 3), new Arrow<int>(3, 6) };
            Assert.That(representativeArrowPaths, Has.Count.EqualTo(1));
            Assert.That(representativeArrowPaths.Single(), Is.EqualTo(expectedRepresentative));
        }

        [Test]
        public void ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt_TriforceInterior()
        {
            var settings = GetSettings(detectNonCancellativity: false);
            var qp = UsefulQPs.GetTriangleQP(3);
            DoSetup(qp, out var ruleTree);

            var representatives = Computer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp.Quiver, 2, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives.ToList();
            var representativeArrowPaths = representatives.Select(path => path.Arrows);
            var expectedMaximalEquivalenceClasses = new ISet<IEnumerable<Arrow<int>>>[]
            {
                new HashSet<IEnumerable<Arrow<int>>>
                {
                    new Arrow<int>[] { new Arrow<int>(2, 1), new Arrow<int>(1, 3) },
                    new Arrow<int>[] { new Arrow<int>(2, 5), new Arrow<int>(5, 3) }
                },
            };

            Assert.That(representatives, Has.Count.EqualTo(expectedMaximalEquivalenceClasses.Length));
            foreach (var representative in representativeArrowPaths)
            {
                Assert.That(expectedMaximalEquivalenceClasses, Has.One.Contains(representative));
            }
        }

        [Test]
        public void ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt_TetraforceCorner()
        {
            var settings = GetSettings(detectNonCancellativity: false);
            var qp = UsefulQPs.GetSquareQP(3);
            DoSetup(qp, out var ruleTree);

            var representativePaths = Computer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp.Quiver, 1, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives.ToList();
            var expectedRepresentative = new Path<int>(1, new Arrow<int>[] { new Arrow<int>(1, 2), new Arrow<int>(2, 5), new Arrow<int>(5, 6), new Arrow<int>(6, 9) });
            Assert.That(representativePaths, Has.Count.EqualTo(1));
            Assert.That(representativePaths.Single(), Is.EqualTo(expectedRepresentative));
        }

        [Test]
        public void ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt_TetraforceSide()
        {
            var settings = GetSettings(detectNonCancellativity: false);
            var qp = UsefulQPs.GetSquareQP(3);
            DoSetup(qp, out var ruleTree);

            var representativePaths = Computer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp.Quiver, 2, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives.ToList();
            var expectedMaximalEquivalenceClasses = new ISet<Path<int>>[]
            {
                new HashSet<Path<int>>
                {
                    new Path<int>(2, new Arrow<int>[] { new Arrow<int>(2, 5), new Arrow<int>(5, 4), new Arrow<int>(4, 7), new Arrow<int>(7, 8) }),
                    new Path<int>(2, new Arrow<int>[] { new Arrow<int>(2, 5), new Arrow<int>(5, 6), new Arrow<int>(6, 9), new Arrow<int>(9, 8) }),
                },
            };

            Assert.That(representativePaths, Has.Count.EqualTo(expectedMaximalEquivalenceClasses.Length));
            foreach (var representative in representativePaths)
            {
                Assert.That(expectedMaximalEquivalenceClasses, Has.One.Contains(representative));
            }
        }

        [Test]
        public void ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt_TetraforceCenter()
        {
            var settings = GetSettings(detectNonCancellativity: false);
            var qp = UsefulQPs.GetSquareQP(3);
            DoSetup(qp, out var ruleTree);

            var representativePaths = Computer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp.Quiver, 5, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives.ToList();
            var expectedMaximalEquivalenceClasses = new ISet<Path<int>>[]
            {
                new HashSet<Path<int>>
                {
                    new Path<int>(5, new Arrow<int>[] { new Arrow<int>(5, 4), new Arrow<int>(4, 1), new Arrow<int>(1, 2), new Arrow<int>(2, 5) }),
                    new Path<int>(5, new Arrow<int>[] { new Arrow<int>(5, 6), new Arrow<int>(6, 3), new Arrow<int>(3, 2), new Arrow<int>(2, 5) }),
                    new Path<int>(5, new Arrow<int>[] { new Arrow<int>(5, 4), new Arrow<int>(4, 7), new Arrow<int>(7, 8), new Arrow<int>(8, 5) }),
                    new Path<int>(5, new Arrow<int>[] { new Arrow<int>(5, 6), new Arrow<int>(6, 9), new Arrow<int>(9, 8), new Arrow<int>(8, 5) }),
                },
            };

            Assert.That(representativePaths, Has.Count.EqualTo(expectedMaximalEquivalenceClasses.Length));
            foreach (var representative in representativePaths)
            {
                Assert.That(expectedMaximalEquivalenceClasses, Has.One.Contains(representative));
            }
        }

        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        public void ComputeMaximalNonzeroEquivalenceClassRepresentativeStartingAt_Cycle(int n)
        {
            var settings = GetSettings(detectNonCancellativity: false);
            var qp = UsefulQPs.GetCycleQP(n);
            DoSetup(qp, out var ruleTree);

            var actualRepresentativePaths = Computer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp.Quiver, 1, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives.ToList();

            // 1->2->...->n-1
            var expectedRepresentative = new Path<int>(1, Enumerable.Range(1, n - 2).Select(k => new Arrow<int>(k, k + 1)));

            Assert.That(actualRepresentativePaths, Has.Count.EqualTo(1));
            Assert.That(actualRepresentativePaths.Single(), Is.EqualTo(expectedRepresentative));
        }

        [Test]
        public void ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt_UnnamedQP2()
        {
            var settings = GetSettings(detectNonCancellativity: false);
            var qp = TestUtility.GetUnnamedQP2();
            DoSetup(qp, out var ruleTree);

            var representativePaths = Computer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp.Quiver, 1, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives.ToList();
            Assert.That(representativePaths.Select(p => p.EndingPoint), Has.All.EqualTo(2));

            representativePaths = Computer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp.Quiver, 5, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives.ToList();
            Assert.That(representativePaths.Select(p => p.EndingPoint), Has.All.EqualTo(6));
        }

        [Test]
        public void ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt_UnnamedQP3()
        {
            var settings = GetSettings(detectNonCancellativity: false);
            var qp = TestUtility.GetUnnamedQP3();
            DoSetup(qp, out var ruleTree);

            var representatives = Computer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp.Quiver, 5, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives.ToList();
            Assert.That(representatives.Select(p => p.EndingPoint), Has.All.EqualTo(8));

            representatives = Computer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp.Quiver, 6, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives.ToList();
            var expectedEndingPoints = new int[] { 9, 11 };
            Assert.That(representatives.Select(p => p.EndingPoint), Is.SupersetOf(expectedEndingPoints).And.SubsetOf(expectedEndingPoints));
        }

        [Test]
        public void ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt_UnnamedQP4()
        {
            var settings = GetSettings(detectNonCancellativity: false);
            var qp = TestUtility.GetUnnamedQP4();
            DoSetup(qp, out var ruleTree);

            var representativePaths = Computer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp.Quiver, 5, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives.ToList();
            Assert.That(representativePaths.Select(p => p.EndingPoint), Has.All.EqualTo(4));

            representativePaths = Computer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp.Quiver, 6, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives.ToList();
            Assert.That(representativePaths.Select(p => p.EndingPoint), Has.All.EqualTo(11));

            // Starting point 1 (interior) would also be good, but computing its equivalence class by hand was a bit painful
        }

        [Test]
        public void ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt_UnnamedFamily2_ForMEqualTo3NEqualTo4()
        {
            var settings = GetSettings(detectNonCancellativity: false);
            var qp = TestUtility.GetUnnamedFamily2QP(3, 4);
            DoSetup(qp, out var ruleTree);

            var representativePaths = Computer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp.Quiver, 0, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives.ToList();
            var actualEndingPoints = new HashSet<int>(representativePaths.Select(x => x.EndingPoint));
            var expectedEndingPoints = new int[] { 4, 5 };
            Assert.That(actualEndingPoints, Is.EquivalentTo(expectedEndingPoints));

            representativePaths = Computer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp.Quiver, 5, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives.ToList();
            actualEndingPoints = new HashSet<int>(representativePaths.Select(x => x.EndingPoint));
            expectedEndingPoints = new int[] { 6 };
            Assert.That(actualEndingPoints, Is.EquivalentTo(expectedEndingPoints));

            representativePaths = Computer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp.Quiver, 8, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives.ToList();
            actualEndingPoints = new HashSet<int>(representativePaths.Select(x => x.EndingPoint));
            expectedEndingPoints = new int[] { 4 };
            Assert.That(actualEndingPoints, Is.EquivalentTo(expectedEndingPoints));
        }

        [Test]
        public void ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt_Cobweb5()
        {
            var settings = GetSettings(detectNonCancellativity: false);
            var qp = UsefulQPs.GetCobwebQP(5, 0);
            DoSetup(qp, out var ruleTree);
            var representativePaths = Computer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp.Quiver, 0, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives.ToList();
            Assert.That(representativePaths, Has.Count.EqualTo(1));
            Assert.That(representativePaths.Single().EndingPoint, Is.EqualTo(2));
        }

        [Test]
        public void ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt_OnClassicNonCancellativeExample_IndicatesNotCancellative()
        {
            if (!Computer.SupportsNonCancellativityDetection) return;

            var settings = GetSettings(detectNonCancellativity: true);
            var qp = UsefulQPs.GetClassicNonCancellativeQP();
            DoSetup(qp, out var ruleTree);
            var result = Computer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp.Quiver, 1, ruleTree, settings);
            Assert.That(result.CancellativityFailureDetected, Is.True);
        }

        [Test]
        public void ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt_OnClassicNonCancellativeExample_IndicatesNotWeaklyCancellative()
        {
            if (!Computer.SupportsNonWeakCancellativityDetection) return;

            var settings = GetSettings(CancellativityTypes.WeakCancellativity);
            var qp = UsefulQPs.GetClassicNonCancellativeQP();
            DoSetup(qp, out var ruleTree);
            var result = Computer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp.Quiver, 1, ruleTree, settings);
            Assert.That(result.WeakCancellativityFailureDetected, Is.True);
        }

        // This region contains tests that have not been verified by hand to be correct
        #region Uncertain tests
        private static RecipeExecutorState GetBaseStateForTwoInnermostLayersOfCobweb(int n)
        {
            var instructions = new IPotentialRecipeInstruction[]
            {
                new PeriodicAtomicGlueInstruction(new AtomicGlueInstruction(0, 1, 4), n),
                new PeriodicAtomicGlueInstruction(new AtomicGlueInstruction(2, 2, 3), n),
            };
            var recipe = new PotentialRecipe(instructions);
            var executor = new RecipeExecutor();
            var qp = executor.ExecuteRecipe(recipe, initialCycleNumArrows: n);

            var boundary = Enumerable.Range(0, 2 * n).Select(k =>
            {
                int source = 3 * n - 2 * (k / 2) - 2;
                int target = (k == 2 * n - 1) ? 3 * n - 1 : // Handle the last k specially
                             (k % 2 == 0) ? source + 1 : source - 1;
                var arrow = new Arrow<int>(source, target);
                return (arrow, k % 2 == 0 ? BoundaryArrowOrientation.Left : BoundaryArrowOrientation.Right);
            }).ToCircularList();

            return new RecipeExecutorState()
            {
                Potential = qp.Potential,
                NextVertex = qp.Quiver.Vertices.Count,
                Boundary = boundary,
                PotentiallyProblematicArrows = new HashSet<Arrow<int>>(),
            };
        }

        private static IEnumerable<QuiverWithPotential<int>> GetQPsForTest1()
        {
            int numPeriods = 3;
            int numRounds = 2;
            int maxCycleLength = 4;

            var baseState = GetBaseStateForTwoInnermostLayersOfCobweb(numPeriods);
            var gen = new QPGeneratorByRecipe();
            var qps = gen.GenerateQPsFromGivenBase(baseState, numPeriods, numRounds, maxCycleLength);

            return qps;
        }

        private static readonly int[] NonAdmissibleIndicesForTest1 = { 3, 8, 13, 37, 41, 46 };

        private static readonly int[] NonCancellativeIndicesForTest1 =
        {
            2, 3, 8, 16, 17, 20, 21, 22, 25, 26, 30, 31, 32,
            36, 37, 40, 45, 50, 51, 54, 55, 58, 59, 60, 64
        };

        /// <summary>
        /// Checks that non-cancellativity is detected for the right QPs.
        /// </summary>
        /// <remarks>
        /// <para>The <see cref="IMaximalNonzeroEquivalenceClassRepresentativeComputer"/> is tested
        /// via an instance of the <see cref="QPAnalyzer"/> class, which makes the test brittle but
        /// easy to write.</para>
        /// </remarks>
        [Test]
        public void ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt_UncertainTest1_NonCancellativityDetectionWorks()
        {
            if (!Computer.SupportsNonCancellativityDetection) return;

            var qpAnalysisSettings = GetQPAnalysisSettings(detectNonCancellativity: true);
            var qpAnalyzer = new QPAnalyzer();
            foreach (var (qp, index) in GetQPsForTest1().EnumerateWithIndex())
            {
                if (NonAdmissibleIndicesForTest1.Contains(index)) continue;

                var results = qpAnalyzer.Analyze(qp, qpAnalysisSettings, Computer);
                bool nonCancellativityDefinitelyExpected = NonCancellativeIndicesForTest1.Except(NonAdmissibleIndicesForTest1).Contains(index);
                bool eitherOfNonCancellativityAndNonAdmissibilityExpected = NonCancellativeIndicesForTest1.Intersect(NonAdmissibleIndicesForTest1).Contains(index);

                if (nonCancellativityDefinitelyExpected) Assert.That(results.MainResults.HasFlag(QPAnalysisMainResults.NotCancellative));
                else if (eitherOfNonCancellativityAndNonAdmissibilityExpected)
                {
                    Assert.That(results.MainResults.HasFlag(QPAnalysisMainResults.NotCancellative) || results.MainResults.HasFlag(QPAnalysisMainResults.Aborted));
                }
                else Assert.That(results.MainResults.HasFlag(QPAnalysisMainResults.NotCancellative), Is.False);
            }
        }

        /// <summary>
        /// Checks that non-admissibility is handled for the right QPs.
        /// </summary>
        /// <remarks>
        /// <para>The <see cref="IMaximalNonzeroEquivalenceClassRepresentativeComputer"/> is tested
        /// via an instance of the <see cref="QPAnalyzer"/> class, which makes the test brittle but
        /// easy to write.</para>
        /// </remarks>
        [Test]
        public void ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt_UncertainTest1_NonAdmissibilityHandlingWorks()
        {
            if (!Computer.SupportsNonCancellativityDetection || !Computer.SupportsNonAdmissibilityHandling) return;

            var qpAnalysisSettings = GetQPAnalysisSettings(detectNonCancellativity: true, maxPathLength: 40);
            var qpAnalyzer = new QPAnalyzer();
            foreach (var (qp, index) in GetQPsForTest1().EnumerateWithIndex())
            {
                var results = qpAnalyzer.Analyze(qp, qpAnalysisSettings, Computer);
                bool nonAdmissibilityDefinitelyExpected = NonAdmissibleIndicesForTest1.Except(NonCancellativeIndicesForTest1).Contains(index);
                bool eitherOfNonAdmissibilityAndNonCancellativityExpected = NonAdmissibleIndicesForTest1.Intersect(NonCancellativeIndicesForTest1).Contains(index);

                if (nonAdmissibilityDefinitelyExpected) Assert.That(results.MainResults.HasFlag(QPAnalysisMainResults.Aborted));
                else if (eitherOfNonAdmissibilityAndNonCancellativityExpected)
                {
                    Assert.That(results.MainResults.HasFlag(QPAnalysisMainResults.Aborted) || results.MainResults.HasFlag(QPAnalysisMainResults.NotCancellative));
                }
                else Assert.That(results.MainResults.HasFlag(QPAnalysisMainResults.Aborted), Is.False);
            }
        }
        #endregion
    }
}
