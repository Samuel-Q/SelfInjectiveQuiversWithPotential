using System;
using System.Collections.Generic;
using System.Linq;
using NUnit;
using NUnit.Framework;
using DataStructures;
using SelfInjectiveQuiversWithPotential;
using SelfInjectiveQuiversWithPotential.Recipes;
using SelfInjectiveQuiversWithPotential.Analysis;

namespace SelfInjectiveQuiversWithPotentialTests
{

    [TestFixture]
    public class QPAnalyzerTestFixture
    {
        private static QPAnalyzer CreateAnalyzer()
        {
            return new QPAnalyzer();
        }

        private static QPAnalysisSettings CreateSettings()
        {
            return new QPAnalysisSettings(detectNonCancellativity: true);
        }

        private static QPAnalysisSettings CreateSettings(int maxPathLength)
        {
            return new QPAnalysisSettings(detectNonCancellativity: true, maxPathLength);
        }

        private static KnownSelfInjectiveQPs CreateKnownSelfInjectiveQPs()
        {
            return new KnownSelfInjectiveQPs();
        }

        // Sets up variables for a call to Analyzer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt (and possibly other methods)
        private static void DoSetup(
            QuiverWithPotential<int> qp,
            out QPAnalyzer analyzer,
            out TransformationRuleTreeNode<int> ruleTree,
            out QPAnalysisSettings settings)
        {
            analyzer = new QPAnalyzer();
            var ruleCreator = new TransformationRuleTreeCreator();
            ruleTree = ruleCreator.CreateTransformationRuleTree(qp);
            settings = new QPAnalysisSettings(detectNonCancellativity: false);
        }

        [Test]
        public void ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt_TriforceCorner()
        {
            var qp = TestUtility.GetTriforceQP();
            DoSetup(qp, out var analyzer, out var ruleTree, out var settings);

            var representatives = analyzer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp, 1, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives;
            var representativeArrowPaths = representatives.Select(path => path.Arrows).ToList();
            var expectedRepresentative = new Arrow<int>[] { new Arrow<int>(1, 3), new Arrow<int>(3, 6) };
            Assert.That(representativeArrowPaths, Has.Count.EqualTo(1));
            Assert.That(representativeArrowPaths.Single(), Is.EqualTo(expectedRepresentative));
        }

        [Test]
        public void ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt_TriforceInterior()
        {
            var qp = TestUtility.GetTriforceQP();
            DoSetup(qp, out var analyzer, out var ruleTree, out var settings);

            var representatives = analyzer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp, 2, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives.ToList();
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
            var qp = TestUtility.GetTetraforceQP();
            DoSetup(qp, out var analyzer, out var ruleTree, out var settings);

            var representativePaths = analyzer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp, 1, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives.ToList();
            var expectedRepresentative = new Path<int>(1, new Arrow<int>[] { new Arrow<int>(1, 2), new Arrow<int>(2, 5), new Arrow<int>(5, 6), new Arrow<int>(6, 9) });
            Assert.That(representativePaths, Has.Count.EqualTo(1));
            Assert.That(representativePaths.Single(), Is.EqualTo(expectedRepresentative));
        }

        [Test]
        public void ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt_TetraforceSide()
        {
            var qp = TestUtility.GetTetraforceQP();
            DoSetup(qp, out var analyzer, out var ruleTree, out var settings);

            var representativePaths = analyzer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp, 2, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives.ToList();
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
            var qp = TestUtility.GetTetraforceQP();
            DoSetup(qp, out var analyzer, out var ruleTree, out var settings);

            var representativePaths = analyzer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp, 5, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives.ToList();
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
            var qp = TestUtility.GetCycleQP(n);
            DoSetup(qp, out var analyzer, out var ruleTree, out var settings);

            var actualRepresentativePaths = analyzer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp, 1, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives.ToList();

            // 1->2->...->n-1
            var expectedRepresentative = new Path<int>(1, Enumerable.Range(1, n - 2).Select(k => new Arrow<int>(k, k + 1)));

            Assert.That(actualRepresentativePaths, Has.Count.EqualTo(1));
            Assert.That(actualRepresentativePaths.Single(), Is.EqualTo(expectedRepresentative));
        }

        [Test]
        public void ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt_UnnamedQP2()
        {
            var qp = TestUtility.GetUnnamedQP2();
            DoSetup(qp, out var analyzer, out var ruleTree, out var settings);

            var representativePaths = analyzer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp, 1, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives.ToList();
            Assert.That(representativePaths.Select(p => p.EndingPoint), Has.All.EqualTo(2));

            representativePaths = analyzer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp, 5, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives.ToList();
            Assert.That(representativePaths.Select(p => p.EndingPoint), Has.All.EqualTo(6));
        }

        [Test]
        public void ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt_UnnamedQP3()
        {
            var qp = TestUtility.GetUnnamedQP3();
            DoSetup(qp, out var analyzer, out var ruleTree, out var settings);

            var representatives = analyzer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp, 5, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives.ToList();
            Assert.That(representatives.Select(p => p.EndingPoint), Has.All.EqualTo(8));

            representatives = analyzer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp, 6, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives.ToList();
            var expectedEndingPoints = new int[] { 9, 11 };
            Assert.That(representatives.Select(p => p.EndingPoint), Is.SupersetOf(expectedEndingPoints).And.SubsetOf(expectedEndingPoints));
        }

        [Test]
        public void ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt_UnnamedQP4()
        {
            var qp = TestUtility.GetUnnamedQP4();
            DoSetup(qp, out var analyzer, out var ruleTree, out var settings);

            var representativePaths = analyzer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp, 5, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives.ToList();
            Assert.That(representativePaths.Select(p => p.EndingPoint), Has.All.EqualTo(4));

            representativePaths = analyzer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp, 6, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives.ToList();
            Assert.That(representativePaths.Select(p => p.EndingPoint), Has.All.EqualTo(11));

            // Starting point 1 (interior) would also be good, but computing its equivalence class by hand was a bit painful
        }

        [Test]
        public void ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt_UnnamedFamily2_ForMEqualTo3NEqualTo4()
        {
            var qp = TestUtility.GetUnnamedFamily2QP(3, 4);
            DoSetup(qp, out var analyzer, out var ruleTree, out var settings);

            var representativePaths = analyzer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp, 0, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives.ToList();
            var actualEndingPoints = new HashSet<int>(representativePaths.Select(x => x.EndingPoint));
            var expectedEndingPoints = new int[] { 4, 5 };
            Assert.That(actualEndingPoints, Is.EquivalentTo(expectedEndingPoints));

            representativePaths = analyzer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp, 5, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives.ToList();
            actualEndingPoints = new HashSet<int>(representativePaths.Select(x => x.EndingPoint));
            expectedEndingPoints = new int[] { 6 };
            Assert.That(actualEndingPoints, Is.EquivalentTo(expectedEndingPoints));

            representativePaths = analyzer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp, 8, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives.ToList();
            actualEndingPoints = new HashSet<int>(representativePaths.Select(x => x.EndingPoint));
            expectedEndingPoints = new int[] { 4 };
            Assert.That(actualEndingPoints, Is.EquivalentTo(expectedEndingPoints));
        }

        [Test]
        public void ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt_Cobweb5()
        {
            const int NumInitialCycles = 5;
            var instructions = new IPotentialRecipeInstruction[]
            {
                new PeriodicAtomicGlueInstruction(new AtomicGlueInstruction(0, 1, 4), 5),
                new PeriodicAtomicGlueInstruction(new AtomicGlueInstruction(2, 2, 3), 5),
            };

            var executor = new RecipeExecutor();
            var qp = executor.ExecuteRecipe(new PotentialRecipe(instructions), NumInitialCycles);
            DoSetup(qp, out var analyzer, out var ruleTree, out var settings);
            var representativePaths = analyzer.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(qp, 0, ruleTree, settings).MaximalNonzeroEquivalenceClassRepresentatives.ToList();
            Assert.That(representativePaths, Has.Count.EqualTo(1));
            Assert.That(representativePaths.Single().EndingPoint, Is.EqualTo(2));

            //var analyzer2 = new Analyzer<int>(qp);
            //var maximalClasses2 = analyzer2.ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt(0);
        }

        private void AssertIsSelfInjectiveWithCorrectNakayamaPermutation<TVertex>(SelfInjectiveQP<TVertex> selfInjectiveQP)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            var analyzer = CreateAnalyzer();
            var settings = new QPAnalysisSettings(detectNonCancellativity: true);
            var results = analyzer.Analyze(selfInjectiveQP.QP, settings);

            Assert.That(results.MainResult.HasFlag(QPAnalysisMainResult.Success));
            Assert.That(results.MainResult.HasFlag(QPAnalysisMainResult.SelfInjective));

            Assert.That(selfInjectiveQP.NakayamaPermutation.Equals(results.NakayamaPermutation));
        }

        private void AssertAreSelfInjectiveWithCorrectNakayamaPermutation<TVertex>(IEnumerable<SelfInjectiveQP<TVertex>> selfInjectiveQPs)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            foreach (var selfInjectiveQP in selfInjectiveQPs)
            {
                AssertIsSelfInjectiveWithCorrectNakayamaPermutation(selfInjectiveQP);
            }
        }

        [Test]
        public void Analyze_IndicatesThatCyclesAreSelfInjectiveWithCorrectNakayamaPermutation()
        {
            var ksiqps = CreateKnownSelfInjectiveQPs();
            AssertAreSelfInjectiveWithCorrectNakayamaPermutation(ksiqps.Cycles.TakeWhile(siQp => siQp.QP.Quiver.Vertices.Count < 30));
        }

        [Test]
        public void Analyze_IndicatesThatTrianglesAreSelfInjectiveWithCorrectNakayamaPermutation()
        {
            var ksiqps = CreateKnownSelfInjectiveQPs();
            AssertAreSelfInjectiveWithCorrectNakayamaPermutation(ksiqps.Triangles.TakeWhile(siQp => siQp.QP.Quiver.Vertices.Count < 30));
        }

        [Test]
        public void Analyze_IndicatesThatSquaresAreSelfInjectiveWithCorrectNakayamaPermutation()
        {
            var ksiqps = CreateKnownSelfInjectiveQPs();
            AssertAreSelfInjectiveWithCorrectNakayamaPermutation(ksiqps.Squares.TakeWhile(siQp => siQp.QP.Quiver.Vertices.Count < 30));
        }

        [Test]
        public void Analyze_IndicatesThatCobwebsAreSelfInjectiveWithCorrectNakayamaPermutation()
        {
            var ksiqps = CreateKnownSelfInjectiveQPs();
            AssertAreSelfInjectiveWithCorrectNakayamaPermutation(ksiqps.Cobwebs.TakeWhile(siQp => siQp.QP.Quiver.Vertices.Count < 30));
        }

        [Test]
        public void Analyze_IndicatesThatOddFlowersAreSelfInjectiveWithCorrectNakayamaPermutation()
        {
            var ksiqps = CreateKnownSelfInjectiveQPs();
            AssertAreSelfInjectiveWithCorrectNakayamaPermutation(ksiqps.OddFlowers.TakeWhile(siQp => siQp.QP.Quiver.Vertices.Count <= 35));
        }

        [Test]
        public void Analyze_IndicatesNotCancellative_ForNotCancellativeQP()
        {
            var potential = new Potential<int>(new Dictionary<DetachedCycle<int>, int>
            {
                { new DetachedCycle<int>(1, 2, 4, 5, 1), +1},
                { new DetachedCycle<int>(1, 3, 4, 5, 1), -1}
            });
            var qp = new QuiverWithPotential<int>(potential);
            var analyzer = CreateAnalyzer();
            var settings = CreateSettings();
            var results = analyzer.Analyze(qp, settings);

            Assert.That(
                results.MainResult.HasFlag(QPAnalysisMainResult.NotCancellative),
                $"The main result was {results.MainResult}, which does not indicate that the QP fails to be cancellative.");
        }

        // Regression test
        [Test]
        public void Analyze_DoesNotTryToUnionTheSameClasses()
        {
            var potential = new Potential<int>(new Dictionary<DetachedCycle<int>, int>
            {
                { new DetachedCycle<int>(0, 1, 2, 3, 0), +1 },
                { new DetachedCycle<int>(0, 4, 3, 0), -1 },
                { new DetachedCycle<int>(2, 3, 5, 2), -1 },
                { new DetachedCycle<int>(1, 2, 6, 1), -1 },
                { new DetachedCycle<int>(0, 1, 7, 0), -1 },
                { new DetachedCycle<int>(0, 4, 8, 0), +1 },
                { new DetachedCycle<int>(3, 5, 9, 3), +1 },
                { new DetachedCycle<int>(2, 6, 10, 2), +1 },
                { new DetachedCycle<int>(1, 7, 11, 1), +1 },
                { new DetachedCycle<int>(0, 12, 4, 8, 0), -1 },
                { new DetachedCycle<int>(3, 13, 5, 9, 3), -1 },
                { new DetachedCycle<int>(2, 14, 6, 10, 2), -1 },
                { new DetachedCycle<int>(1, 15, 7, 11, 1), -1 }
            });
            var qp = new QuiverWithPotential<int>(potential);
            var analyzer = CreateAnalyzer();
            var settings = CreateSettings(maxPathLength: 20);
            Assert.That(() => analyzer.Analyze(qp, settings), Throws.Nothing);
        }

        // Regression test
        [Test]
        public void Analyze_DoesNotTryToUnionTheSameClasses_2()
        {
            var potential = new Potential<int>(new Dictionary<DetachedCycle<int>, int>
            {
                { new DetachedCycle<int>(0, 1, 2, 3, 4, 0), +1 },
                { new DetachedCycle<int>(0, 5, 4, 0), -1 },
                { new DetachedCycle<int>(3, 4, 6, 3), -1 },
                { new DetachedCycle<int>(2, 3, 7, 2), -1 },
                { new DetachedCycle<int>(1, 2, 8, 1), -1 },
                { new DetachedCycle<int>(0, 1, 9, 0), -1 },

                { new DetachedCycle<int>(0, 5, 10, 0), +1 },
                { new DetachedCycle<int>(4, 6, 11, 4), +1 },
                { new DetachedCycle<int>(3, 7, 12, 3), +1 },
                { new DetachedCycle<int>(2, 8, 13, 2), +1 },
                { new DetachedCycle<int>(1, 9, 14, 1), +1 },

                { new DetachedCycle<int>(0, 15, 5, 10, 0), -1 },
                { new DetachedCycle<int>(4, 16, 6, 11, 4), -1 },
                { new DetachedCycle<int>(3, 17, 7, 12, 3), -1 },
                { new DetachedCycle<int>(2, 18, 8, 13, 2), -1 },
                { new DetachedCycle<int>(1, 19, 9, 14, 1), -1 }
            });
            var qp = new QuiverWithPotential<int>(potential);
            var analyzer = CreateAnalyzer();
            var settings = CreateSettings(maxPathLength: 20);
            Assert.That(() => analyzer.Analyze(qp, settings), Throws.Nothing);
        }
    }
}
