using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using SelfInjectiveQuiversWithPotential;
using SelfInjectiveQuiversWithPotential.Plane;
using SelfInjectiveQuiversWithPotential.Layer;

namespace SelfInjectiveQuiversWithPotentialTests
{
    [TestFixture]
    public class InteractiveLayeredQuiverGeneratorTestFixture
    {
        private InteractiveLayeredQuiverGenerator generator;

        [SetUp]
        public void SetUp()
        {
            generator = CreateGenerator();
        }

        static InteractiveLayeredQuiverGenerator CreateGenerator()
        {
            return new InteractiveLayeredQuiverGenerator();
        }

        static Potential<int> CreatePotential(IReadOnlyDictionary<DetachedCycle<int>, int> cycleToCoefficientDictionary)
        {
            return new Potential<int>(cycleToCoefficientDictionary);
        }

        static Potential<int> CreatePotential(DetachedCycle<int> cycle, int coefficient)
        {
            return new Potential<int>(cycle, coefficient);
        }

        static DetachedCycle<int> CreateDetachedCycle(IEnumerable<Arrow<int>> arrows)
        {
            return new DetachedCycle<int>(arrows);
        }

        static DetachedCycle<int> CreateDetachedCycle(params int[] vertices)
        {
            return new DetachedCycle<int>(vertices);
        }

        static DetachedCycle<int> CreateDetachedCycle(IEnumerable<int> vertices)
        {
            return new DetachedCycle<int>(vertices);
        }

        static LayerType CreateLayerType(params int[] parameterValues)
        {
            return new LayerType(parameterValues);
        }

        static Composition CreateComposition(params int[] terms)
        {
            return new Composition(terms);
        }

        static Composition CreateComposition(IEnumerable<int> terms)
        {
            return new Composition(terms);
        }

        static CompositionParameters CreateCompositionParameters(int sum, int numTerms)
        {
            return new CompositionParameters(sum, numTerms);
        }

        [Test]
        public void StartGeneration_ThrowsArgumentNullException()
        {
            Assert.That(() => generator.StartGeneration(null), Throws.ArgumentNullException);
        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(10)]
        public void StartGeneration_ReturnsNull_IfSingleLayer(int layerSize)
        {
            var layerType = CreateLayerType(layerSize);
            Assert.That(generator.StartGeneration(layerType), Is.EqualTo(null));
        }

        [Test]
        public void StartGenerationFromBase_ThrowsGeneratorException_IfTooFewExplicitArrowPairs()
        {
            // The cobweb quiver has 5 implicit arrow pairs for the boundary layer
            var quiverInPlane = UsefulQuiversInPlane.GetCobwebQuiverInPlane(5, 50, firstVertex: 1);
            var potential = UsefulQPs.GetCobwebQP(5).Potential;
            var boundaryLayer = Enumerable.Range(6, 10);
            var layerType = CreateLayerType(10, 4, 20); // 4 < 5 explicit arrow pairs

            Assert.That(
                () => generator.StartGenerationFromBase(quiverInPlane, potential, boundaryLayer, layerType, nextVertex: 16),
                Throws.InstanceOf<GeneratorException>());
        }

        [Test]
        public void TryStartGenerationFromBase_ReturnsFalse_IfTooFewExplicitArrowPairs()
        {
            // The cobweb quiver has 5 implicit arrow pairs for the boundary layer
            var quiverInPlane = UsefulQuiversInPlane.GetCobwebQuiverInPlane(5, 50, firstVertex: 1);
            var potential = UsefulQPs.GetCobwebQP(5).Potential;
            var boundaryLayer = Enumerable.Range(6, 10);
            var layerType = CreateLayerType(10, 4, 20); // 4 < 5 explicit arrow pairs

            var result = generator.TryStartGenerationFromBase(quiverInPlane, potential, boundaryLayer, layerType, nextVertex: 16, out var nextCompositionParameters);
            Assert.That(result, Is.False);
            Assert.That(nextCompositionParameters, Is.Null);
        }

        [Test]
        public void SupplyComposition_Throws_ArgumentNullException()
        {
            var layerType = CreateLayerType(5, 5, 10);
            generator.StartGeneration(layerType);
            Assert.That(() => generator.SupplyComposition(null), Throws.ArgumentNullException);
        }

        [Test]
        public void SupplyComposition_ThrowsInvalidOperationException_IfGenerationIsNotStarted()
        {
            var composition = CreateComposition(2, 2, 2);

            Assert.That(() => generator.SupplyComposition(composition), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void SupplyComposition_ThrowsInvalidOperationException_IfGenerationIsCompleted()
        {
            var layerType = CreateLayerType(3);
            generator.StartGeneration(layerType);
            var composition = CreateComposition(2, 2, 2);

            Assert.That(() => generator.SupplyComposition(composition), Throws.InstanceOf<InvalidOperationException>());
        }

        static IEnumerable<TestCaseData> SupplyComposition_ThrowsArgumentException_IfCompositionHasBadParameters_TestCaseSource()
        {
            var testCaseData = new TestCaseData(
                CreateLayerType(5, 5, 10), // Cobweb(5) layer type
                new Composition[]
                {
                    CreateComposition(1, 1, 1, 1, 1), // Expecting CompositionParameters (Sum = 10, NumTerms = 5)
                });
            yield return testCaseData;
            yield return new TestCaseData(
                CreateLayerType(5, 5, 10), // Cobweb(5) layer type
                new Composition[]
                {
                    CreateComposition(2, 2, 2, 2, 2), // One pair for each vertex
                    CreateComposition(1, 1, 1, 1, 1, 1, 1, 1, 1, 1) // Expecting (Sum = 15, NumTerms = 10); correct number of terms
                });
            yield return new TestCaseData(
                CreateLayerType(5, 5, 10), // Cobweb(5) layer type
                new Composition[]
                {
                    CreateComposition(2, 2, 2, 2, 2), // One pair for each vertex
                    CreateComposition(3, 3, 3, 3, 3) // Expecting (Sum = 15, NumTerms = 10); correct sum
                });
        }

        [TestCaseSource(nameof(SupplyComposition_ThrowsArgumentException_IfCompositionHasBadParameters_TestCaseSource))]
        public void SupplyComposition_ThrowsArgumentException_IfCompositionHasBadParameters(
            LayerType layerType,
            IEnumerable<Composition> compositions)
        {
            generator.StartGeneration(layerType);
            foreach (var composition in compositions.SkipLast(1)) generator.SupplyComposition(composition);
            Assert.That(() => generator.SupplyComposition(compositions.Last()), Throws.ArgumentException);
        }

        // See my physical notepad (2018-11-15) for drawings
        [Test]
        public void SupplyComposition_DoesNotMessUpInternalState_OnGeneratorException()
        {
            var layerType = CreateLayerType(4, 4, 8, 2, 16);
            generator.StartGeneration(layerType);
            generator.SupplyComposition(CreateComposition(2, 2, 2, 2));
            var nextComposition = CreateComposition(Utility.RepeatMany(4, 2, 1));
            for (int i = 0; i < 10; i++)
            {
                Assert.That(() => generator.SupplyComposition(nextComposition), Throws.InstanceOf<GeneratorException>());
            }

            nextComposition = CreateComposition(Utility.RepeatMany(4, 1, 2));
            Assert.That(() => generator.SupplyComposition(nextComposition), Throws.Nothing);
        }

        static IEnumerable<TestCaseData> UnsupplyComposition_UnsupplyingAndResupplyingAllCompositionsWorks_TestCaseSource()
        {
            yield return new TestCaseData( // Cobweb(5)
                CreateLayerType(5, 5, 10),
                new Composition[]
                {
                    CreateComposition(2, 2, 2, 2, 2),
                    CreateComposition(2, 1, 2, 1, 2, 1, 2, 1, 2, 1)
                });
        }

        [TestCaseSource(nameof(UnsupplyComposition_UnsupplyingAndResupplyingAllCompositionsWorks_TestCaseSource))]
        public void UnsupplyComposition_UnsupplyingAndResupplyingAllCompositions_DoesNotCrash(
            LayerType layerType,
            IEnumerable<Composition> compositions)
        {
            generator.StartGeneration(layerType);
            foreach (var composition in compositions) generator.SupplyComposition(composition);
            foreach (var _ in compositions) generator.UnsupplyLastComposition();
            foreach (var composition in compositions) generator.SupplyComposition(composition);
        }

        [Test]
        public void EndGeneration_ThrowsInvalidOperationException_IfGenerationIsNotStarted()
        {
            Assert.That(() => generator.EndGeneration(), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void EndGeneration_ThrowsInvalidOperationException_IfGenerationIsNotCompleted()
        {
            var layerType = CreateLayerType(5, 5, 10);
            generator.StartGeneration(layerType);

            Assert.That(() => generator.EndGeneration(), Throws.InstanceOf<InvalidOperationException>());

            generator = CreateGenerator();
            layerType = CreateLayerType(5, 5, 10);
            generator.StartGeneration(layerType);
            generator.SupplyComposition(CreateComposition(2, 2, 2, 2, 2));

            Assert.That(() => generator.EndGeneration(), Throws.InstanceOf<InvalidOperationException>());
        }

        [TestCase(3)]
        [TestCase(4)]
        [TestCase(10)]
        public void QuiverGenerationWorks_ForCycle(int numVertices)
        {
            var layerType = CreateLayerType(numVertices);
            int firstVertex = 0;

            Assert.That(generator.StartGeneration(layerType, firstVertex), Is.EqualTo(null));

            var quiverInPlane = generator.EndGeneration().QuiverInPlane;
            var actualQuiver = quiverInPlane.GetUnderlyingQuiver();
            var expectedVertices = Enumerable.Range(firstVertex, numVertices);
            var expectedArrows = expectedVertices.Select(k => new Arrow<int>(k, (k+1).Modulo(numVertices)));
            CollectionAssert.AreEquivalent(expectedVertices, actualQuiver.Vertices);
            CollectionAssert.AreEquivalent(expectedArrows, actualQuiver.Arrows);
        }

        [TestCase(3)]
        [TestCase(4)]
        [TestCase(10)]
        public void QPGenerationWorks_ForCycle(int numVertices)
        {
            var layerType = CreateLayerType(numVertices);
            int firstVertex = 0;

            Assert.That(generator.StartGeneration(layerType, firstVertex), Is.EqualTo(null));

            var qp = generator.EndGeneration().QP;
            var actualQuiver = qp.Quiver;
            var expectedVertices = Enumerable.Range(firstVertex, numVertices);
            var cycleArrows = expectedVertices.Select(k => new Arrow<int>(k, (k + 1).Modulo(numVertices)));
            var expectedArrows = cycleArrows;
            CollectionAssert.AreEquivalent(expectedVertices, actualQuiver.Vertices);
            CollectionAssert.AreEquivalent(expectedArrows, actualQuiver.Arrows);

            var actualPotential = qp.Potential;
            var expectedPotential = CreatePotential(CreateDetachedCycle(cycleArrows), +1);
            Assert.That(actualPotential, Is.EqualTo(expectedPotential));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(1)]
        public void QuiverGenerationWorks_ForCobweb5(int firstVertex)
        {
            // 5 vertices in the inner layer
            // 5 pairs of vertical arrows (one for each vertex)
            // 10 vertices in the outer layer
            var layerType = CreateLayerType(5, 5, 10);

            // Composition of 5 in 5 non-negative terms (equivalent to 10 in 5 positive terms)
            var expectedCompositionParameters = CreateCompositionParameters(10, 5);
            Assert.That(generator.StartGeneration(layerType, firstVertex), Is.EqualTo(expectedCompositionParameters));

            // 1+1+1+1+1 as a composition into non-negative terms
            var composition = CreateComposition(2, 2, 2, 2, 2);

            // 10 arrows in the upper layer to be distributed to 10 = 2*5 = 2*k1 polygons
            // Every vertex in the lower layer has at least one vertical arrow:
            //    One explicit pair and no implicit pairs
            // so r1 = 5, and the modified composition into positive terms is
            //    (m2+r1 in 2*k1) = (10+5 in 2*5) = (15 in 10)
            expectedCompositionParameters = CreateCompositionParameters(15, 10);
            Assert.That(generator.SupplyComposition(composition), Is.EqualTo(expectedCompositionParameters));

            // 1*10 as a composition into terms where zero is sometimes allowed
            // Every "skip" can be zero, so increase the corresponding terms from 1 to 2
            //composition = CreateComposition(1, 2, 1, 2, 1, 2, 1, 2, 1, 2);
            composition = CreateComposition(2, 1, 2, 1, 2, 1, 2, 1, 2, 1);
            expectedCompositionParameters = null;
            Assert.That(generator.SupplyComposition(composition), Is.EqualTo(expectedCompositionParameters));

            var quiverInPlane = generator.EndGeneration().QuiverInPlane;
            var actualQuiver = quiverInPlane.GetUnderlyingQuiver();
            var expectedVertices = Enumerable.Range(firstVertex, 15);
            IEnumerable<Arrow<int>> expectedArrows = new Arrow<int>[]
            {
                // First layer
                new Arrow<int>(0, 1), new Arrow<int>(1, 2), new Arrow<int>(2, 3), new Arrow<int>(3, 4), new Arrow<int>(4, 0),

                // Up-down polygons
                new Arrow<int>(0, 6), new Arrow<int>(6, 7), new Arrow<int>(7, 0),
                new Arrow<int>(1, 8), new Arrow<int>(8, 9), new Arrow<int>(9, 1),
                new Arrow<int>(2, 10), new Arrow<int>(10, 11), new Arrow<int>(11, 2),
                new Arrow<int>(3, 12), new Arrow<int>(12, 13), new Arrow<int>(13, 3),
                new Arrow<int>(4, 14), new Arrow<int>(14, 5), new Arrow<int>(5, 4),

                // Remaining arrows in the second layer
                new Arrow<int>(6, 5), new Arrow<int>(8, 7), new Arrow<int>(10, 9), new Arrow<int>(12, 11),  new Arrow<int>(14, 13)
            };
            expectedArrows = expectedArrows.Select(a => new Arrow<int>(a.Source + firstVertex, a.Target + firstVertex));
            CollectionAssert.AreEquivalent(expectedVertices, actualQuiver.Vertices);
            CollectionAssert.AreEquivalent(expectedArrows, actualQuiver.Arrows);
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(1)]
        public void QPGenerationWorks_ForCobweb5(int firstVertex)
        {
            // 5 vertices in the inner layer
            // 5 pairs of vertical arrows (one for each vertex)
            // 10 vertices in the outer layer
            var layerType = CreateLayerType(5, 5, 10);

            // Composition of 5 in 5 non-negative terms (equivalent to 10 in 5 positive terms)
            var expectedCompositionParameters = CreateCompositionParameters(10, 5);
            Assert.That(generator.StartGeneration(layerType, firstVertex), Is.EqualTo(expectedCompositionParameters));

            // 1+1+1+1+1 as a composition into non-negative terms
            var composition = CreateComposition(2, 2, 2, 2, 2);

            // 10 arrows in the upper layer to be distributed to 10 = 2*5 = 2*k1 polygons
            // Every vertex in the lower layer has at least one vertical arrow:
            //    One explicit pair and no implicit pairs
            // so r1 = 5, and the modified composition into positive terms is
            //    (m2+r1 in 2*k1) = (10+5 in 2*5) = (15 in 10)
            expectedCompositionParameters = CreateCompositionParameters(15, 10);
            Assert.That(generator.SupplyComposition(composition), Is.EqualTo(expectedCompositionParameters));

            // 1*10 as a composition into terms where zero is sometimes allowed
            // Every "skip" can be zero, so increase the corresponding terms from 1 to 2
            //composition = CreateComposition(1, 2, 1, 2, 1, 2, 1, 2, 1, 2);
            composition = CreateComposition(2, 1, 2, 1, 2, 1, 2, 1, 2, 1);
            expectedCompositionParameters = null;
            Assert.That(generator.SupplyComposition(composition), Is.EqualTo(expectedCompositionParameters));

            var qp = generator.EndGeneration().QP;
            var actualQuiver = qp.Quiver;
            var expectedVertices = Enumerable.Range(firstVertex, 15);
            IEnumerable<Arrow<int>> expectedArrows = new Arrow<int>[]
            {
                // First layer
                new Arrow<int>(0, 1), new Arrow<int>(1, 2), new Arrow<int>(2, 3), new Arrow<int>(3, 4), new Arrow<int>(4, 0),

                // Up-down polygons
                new Arrow<int>(0, 6), new Arrow<int>(6, 7), new Arrow<int>(7, 0),
                new Arrow<int>(1, 8), new Arrow<int>(8, 9), new Arrow<int>(9, 1),
                new Arrow<int>(2, 10), new Arrow<int>(10, 11), new Arrow<int>(11, 2),
                new Arrow<int>(3, 12), new Arrow<int>(12, 13), new Arrow<int>(13, 3),
                new Arrow<int>(4, 14), new Arrow<int>(14, 5), new Arrow<int>(5, 4),

                // Remaining arrows in the second layer
                new Arrow<int>(6, 5), new Arrow<int>(8, 7), new Arrow<int>(10, 9), new Arrow<int>(12, 11),  new Arrow<int>(14, 13)
            };
            expectedArrows = expectedArrows.Select(a => new Arrow<int>(a.Source + firstVertex, a.Target + firstVertex));
            CollectionAssert.AreEquivalent(expectedVertices, actualQuiver.Vertices);
            CollectionAssert.AreEquivalent(expectedArrows, actualQuiver.Arrows);

            var actualPotential = qp.Potential;
            var linCombDict = new Dictionary<DetachedCycle<int>, int>
            {
                { CreateDetachedCycle(0, 1, 2, 3, 4, 0), +1 },
                { CreateDetachedCycle(0, 6, 7, 0), +1 },
                { CreateDetachedCycle(1, 8, 9, 1), +1 },
                { CreateDetachedCycle(2, 10, 11, 2), +1 },
                { CreateDetachedCycle(3, 12, 13, 3), +1 },
                { CreateDetachedCycle(4, 14, 5, 4), +1 },
                { CreateDetachedCycle(0, 6, 5, 4, 0), -1 },
                { CreateDetachedCycle(1, 8, 7, 0, 1), -1 },
                { CreateDetachedCycle(2, 10, 9, 1, 2), -1 },
                { CreateDetachedCycle(3, 12, 11, 2, 3), -1 },
                { CreateDetachedCycle(4, 14, 13, 3, 4), -1 },
            };
            // Shift the vertices by firstVertex
            linCombDict = linCombDict.Select(pair =>
            {
                var newCycle = CreateDetachedCycle(pair.Key.CanonicalPath.Vertices.Select(vertex => vertex + firstVertex));
                return new KeyValuePair<DetachedCycle<int>, int>(newCycle, pair.Value);
            }).ToDictionary(pair => pair.Key, pair => pair.Value);
            var expectedPotential = CreatePotential(linCombDict);

            Assert.That(actualPotential, Is.EqualTo(expectedPotential));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(1)]
        public void QuiverGenerationWorks_ForCobweb7(int firstVertex)
        {
            // 7 vertices in the inner layer, 14 in the other two layers.
            // 7 pairs of vertical arrows between each pair of consecutive layers.
            var layerType = CreateLayerType(7, 7, 14, 7, 14);

            // "k1 in m1" composition
            // Composition of k1 = 7 in m1 = 7 non-negative terms (equivalent to 14 in 7 positive terms)
            var expectedCompositionParameters = CreateCompositionParameters(14, 7);
            Assert.That(generator.StartGeneration(layerType, firstVertex), Is.EqualTo(expectedCompositionParameters));

            // 1+1+1+1+1+1+1 = 1*7 as a composition into non-negative terms
            var composition = CreateComposition(2, 2, 2, 2, 2, 2, 2);

            // "m2 in k1" composition (m2+r1 in 2*k1)
            // 14 arrows in the upper layer (second layer) to be distributed to
            // 14 = 2*7 = 2*k1 polygons (between layer 1 and 2, one-based)
            // Every vertex in the lower layer has at least one vertical arrow:
            //    One explicit pair and no implicit pairs
            // so r1 = 7, and the modified composition into positive terms is
            //    (m2+r1 in 2*k1) = (14+7 in 2*7) = (21 in 14)
            expectedCompositionParameters = CreateCompositionParameters(21, 14);
            Assert.That(generator.SupplyComposition(composition), Is.EqualTo(expectedCompositionParameters));

            // 1*10 as a composition into terms where zero is sometimes allowed
            // Every "skip" can be zero, so increase the corresponding terms from 1 to 2
            composition = CreateComposition(2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1);

            // "k2 in m2" composition (k2-l2 in m2 with zeros allowed, so k2-l2+m2 in m2)
            // l2 = number of implicit arrows between layer 2 and 3 = 7 (only implicit arrows!)
            // So there's no choice!
            expectedCompositionParameters = CreateCompositionParameters(14, 14);
            Assert.That(generator.SupplyComposition(composition), Is.EqualTo(expectedCompositionParameters));

            // 14 = 1*14; no choice
            composition = CreateComposition(Enumerable.Repeat(1, 14));

            // "m3 in k2" composition (m3+r2 in 2*k2)
            // r2 = 14, because *every* vertex in layer 2 has a vertical arrow to/from layer 3
            // so (m3+r2 in 2*k2) = (14+14 in 14) = (28 in 14)
            expectedCompositionParameters = CreateCompositionParameters(28, 14);
            Assert.That(generator.SupplyComposition(composition), Is.EqualTo(expectedCompositionParameters));

            // Consume 1 arrow in the outer layer for every polygon. For every polygon, consuming
            // 0 arrows would have been fine (0 steps or 0 skips), so every value gets increased by 1
            // That is, 1*14 becomes 2*14
            composition = CreateComposition(Enumerable.Repeat(2, 14));

            // No more compositions
            expectedCompositionParameters = null;
            Assert.That(generator.SupplyComposition(composition), Is.EqualTo(expectedCompositionParameters));

            var quiverInPlane = generator.EndGeneration().QuiverInPlane;
            var actualQuiver = quiverInPlane.GetUnderlyingQuiver();

            // Construct the expected quiver (see my physical pad, 2018-10-29)
            var expectedVertices = Enumerable.Range(firstVertex, 35);
            var expectedArrows = new List<Arrow<int>>();

            // Layer 1
            expectedArrows.AddRange(Enumerable.Range(0, 7).Select(k => new Arrow<int>(k, (k + 1).Modulo(7))));

            // Up, left, right, down arrows for layer 1 and 2
            expectedArrows.AddRange(new Arrow<int>[]
            {
                new Arrow<int>(0, 8), new Arrow<int>(8, 7), new Arrow<int>(8, 9), new Arrow<int>(9, 0),
                new Arrow<int>(1, 10), new Arrow<int>(10, 9), new Arrow<int>(10, 11), new Arrow<int>(11, 1),
                new Arrow<int>(2, 12), new Arrow<int>(12, 11), new Arrow<int>(12, 13), new Arrow<int>(13, 2),
                new Arrow<int>(3, 14), new Arrow<int>(14, 13), new Arrow<int>(14, 15), new Arrow<int>(15, 3),
                new Arrow<int>(4, 16), new Arrow<int>(16, 15), new Arrow<int>(16, 17), new Arrow<int>(17, 4),
                new Arrow<int>(5, 18), new Arrow<int>(18, 17), new Arrow<int>(18, 19), new Arrow<int>(19, 5),
                new Arrow<int>(6, 20), new Arrow<int>(20, 19), new Arrow<int>(20, 7), new Arrow<int>(7, 6)
            });

            // Up arrows from layer 2 to 3
            expectedArrows.AddRange(new Arrow<int>[]
            {
                new Arrow<int>(7, 22),
                new Arrow<int>(9, 24),
                new Arrow<int>(11, 26),
                new Arrow<int>(13, 28),
                new Arrow<int>(15, 30),
                new Arrow<int>(17, 32),
                new Arrow<int>(19, 34)
            });

            // Down arrows from layer 3 to 2
            expectedArrows.AddRange(new Arrow<int>[]
            {
                new Arrow<int>(23, 8),
                new Arrow<int>(25, 10),
                new Arrow<int>(27, 12),
                new Arrow<int>(29, 14),
                new Arrow<int>(31, 16),
                new Arrow<int>(33, 18),
                new Arrow<int>(21, 20)
            });

            // Left arrows in layer 3
            expectedArrows.AddRange(new Arrow<int>[]
            {
                new Arrow<int>(22, 21),
                new Arrow<int>(24, 23),
                new Arrow<int>(26, 25),
                new Arrow<int>(28, 27),
                new Arrow<int>(30, 29),
                new Arrow<int>(32, 31),
                new Arrow<int>(34, 33)
            });

            // Right arrows in layer 3
            expectedArrows.AddRange(new Arrow<int>[]
            {
                new Arrow<int>(22, 23),
                new Arrow<int>(24, 25),
                new Arrow<int>(26, 27),
                new Arrow<int>(28, 29),
                new Arrow<int>(30, 31),
                new Arrow<int>(32, 33),
                new Arrow<int>(34, 21)
            });
            expectedArrows = expectedArrows.Select(a => new Arrow<int>(a.Source + firstVertex, a.Target + firstVertex)).ToList();

            CollectionAssert.AreEquivalent(expectedVertices, actualQuiver.Vertices);
            CollectionAssert.AreEquivalent(expectedArrows, actualQuiver.Arrows);
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(1)]
        public void QPGenerationWorks_ForCobweb7(int firstVertex)
        {
            // 7 vertices in the inner layer, 14 in the other two layers.
            // 7 pairs of vertical arrows between each pair of consecutive layers.
            var layerType = CreateLayerType(7, 7, 14, 7, 14);

            // "k1 in m1" composition
            // Composition of k1 = 7 in m1 = 7 non-negative terms (equivalent to 14 in 7 positive terms)
            var expectedCompositionParameters = CreateCompositionParameters(14, 7);
            Assert.That(generator.StartGeneration(layerType, firstVertex), Is.EqualTo(expectedCompositionParameters));

            // 1+1+1+1+1+1+1 = 1*7 as a composition into non-negative terms
            var composition = CreateComposition(2, 2, 2, 2, 2, 2, 2);

            // "m2 in k1" composition (m2+r1 in 2*k1)
            // 14 arrows in the upper layer (second layer) to be distributed to
            // 14 = 2*7 = 2*k1 polygons (between layer 1 and 2, one-based)
            // Every vertex in the lower layer has at least one vertical arrow:
            //    One explicit pair and no implicit pairs
            // so r1 = 7, and the modified composition into positive terms is
            //    (m2+r1 in 2*k1) = (14+7 in 2*7) = (21 in 14)
            expectedCompositionParameters = CreateCompositionParameters(21, 14);
            Assert.That(generator.SupplyComposition(composition), Is.EqualTo(expectedCompositionParameters));

            // 1*10 as a composition into terms where zero is sometimes allowed
            // Every "skip" can be zero, so increase the corresponding terms from 1 to 2
            composition = CreateComposition(2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1);

            // "k2 in m2" composition (k2-l2 in m2 with zeros allowed, so k2-l2+m2 in m2)
            // l2 = number of implicit arrows between layer 2 and 3 = 7 (only implicit arrows!)
            // So there's no choice!
            expectedCompositionParameters = CreateCompositionParameters(14, 14);
            Assert.That(generator.SupplyComposition(composition), Is.EqualTo(expectedCompositionParameters));

            // 14 = 1*14; no choice
            composition = CreateComposition(Enumerable.Repeat(1, 14));

            // "m3 in k2" composition (m3+r2 in 2*k2)
            // r2 = 14, because *every* vertex in layer 2 has a vertical arrow to/from layer 3
            // so (m3+r2 in 2*k2) = (14+14 in 14) = (28 in 14)
            expectedCompositionParameters = CreateCompositionParameters(28, 14);
            Assert.That(generator.SupplyComposition(composition), Is.EqualTo(expectedCompositionParameters));

            // Consume 1 arrow in the outer layer for every polygon. For every polygon, consuming
            // 0 arrows would have been fine (0 steps or 0 skips), so every value gets increased by 1
            // That is, 1*14 becomes 2*14
            composition = CreateComposition(Enumerable.Repeat(2, 14));

            // No more compositions
            expectedCompositionParameters = null;
            Assert.That(generator.SupplyComposition(composition), Is.EqualTo(expectedCompositionParameters));

            var qp = generator.EndGeneration().QP;
            var actualQuiver = qp.Quiver;

            // Construct the expected quiver (see my physical pad, 2018-10-29)
            var expectedVertices = Enumerable.Range(firstVertex, 35);
            var expectedArrows = new List<Arrow<int>>();

            // Layer 1
            expectedArrows.AddRange(Enumerable.Range(0, 7).Select(k => new Arrow<int>(k, (k + 1).Modulo(7))));

            // Up, left, right, down arrows for layer 1 and 2
            expectedArrows.AddRange(new Arrow<int>[]
            {
                new Arrow<int>(0, 8), new Arrow<int>(8, 7), new Arrow<int>(8, 9), new Arrow<int>(9, 0),
                new Arrow<int>(1, 10), new Arrow<int>(10, 9), new Arrow<int>(10, 11), new Arrow<int>(11, 1),
                new Arrow<int>(2, 12), new Arrow<int>(12, 11), new Arrow<int>(12, 13), new Arrow<int>(13, 2),
                new Arrow<int>(3, 14), new Arrow<int>(14, 13), new Arrow<int>(14, 15), new Arrow<int>(15, 3),
                new Arrow<int>(4, 16), new Arrow<int>(16, 15), new Arrow<int>(16, 17), new Arrow<int>(17, 4),
                new Arrow<int>(5, 18), new Arrow<int>(18, 17), new Arrow<int>(18, 19), new Arrow<int>(19, 5),
                new Arrow<int>(6, 20), new Arrow<int>(20, 19), new Arrow<int>(20, 7), new Arrow<int>(7, 6)
            });

            // Up arrows from layer 2 to 3
            expectedArrows.AddRange(new Arrow<int>[]
            {
                new Arrow<int>(7, 22),
                new Arrow<int>(9, 24),
                new Arrow<int>(11, 26),
                new Arrow<int>(13, 28),
                new Arrow<int>(15, 30),
                new Arrow<int>(17, 32),
                new Arrow<int>(19, 34)
            });

            // Down arrows from layer 3 to 2
            expectedArrows.AddRange(new Arrow<int>[]
            {
                new Arrow<int>(23, 8),
                new Arrow<int>(25, 10),
                new Arrow<int>(27, 12),
                new Arrow<int>(29, 14),
                new Arrow<int>(31, 16),
                new Arrow<int>(33, 18),
                new Arrow<int>(21, 20)
            });

            // Left arrows in layer 3
            expectedArrows.AddRange(new Arrow<int>[]
            {
                new Arrow<int>(22, 21),
                new Arrow<int>(24, 23),
                new Arrow<int>(26, 25),
                new Arrow<int>(28, 27),
                new Arrow<int>(30, 29),
                new Arrow<int>(32, 31),
                new Arrow<int>(34, 33)
            });

            // Right arrows in layer 3
            expectedArrows.AddRange(new Arrow<int>[]
            {
                new Arrow<int>(22, 23),
                new Arrow<int>(24, 25),
                new Arrow<int>(26, 27),
                new Arrow<int>(28, 29),
                new Arrow<int>(30, 31),
                new Arrow<int>(32, 33),
                new Arrow<int>(34, 21)
            });
            expectedArrows = expectedArrows.Select(a => new Arrow<int>(a.Source + firstVertex, a.Target + firstVertex)).ToList();

            CollectionAssert.AreEquivalent(expectedVertices, actualQuiver.Vertices);
            CollectionAssert.AreEquivalent(expectedArrows, actualQuiver.Arrows);

            var actualPotential = qp.Potential;
            var linCombDict = new Dictionary<DetachedCycle<int>, int>
            {
                // Layer 1
                { CreateDetachedCycle(0, 1, 2, 3, 4, 5, 6, 0), +1 },

                // Layer 1-2
                { CreateDetachedCycle(0, 8, 9, 0), +1 },
                { CreateDetachedCycle(1, 10, 11, 1), +1 },
                { CreateDetachedCycle(2, 12, 13, 2), +1 },
                { CreateDetachedCycle(3, 14, 15, 3), +1 },
                { CreateDetachedCycle(4, 16, 17, 4), +1 },
                { CreateDetachedCycle(5, 18, 19, 5), +1 },
                { CreateDetachedCycle(6, 20, 7, 6), +1 },

                { CreateDetachedCycle(0, 8, 7, 6, 0), -1 },
                { CreateDetachedCycle(1, 10, 9, 0, 1), -1 },
                { CreateDetachedCycle(2, 12, 11, 1, 2), -1 },
                { CreateDetachedCycle(3, 14, 13, 2, 3), -1 },
                { CreateDetachedCycle(4, 16, 15, 3, 4), -1 },
                { CreateDetachedCycle(5, 18, 17, 4, 5), -1 },
                { CreateDetachedCycle(6, 20, 19, 5, 6), -1 },

                // Layer 2-3
                { CreateDetachedCycle(7, 22, 23, 8, 7), +1 },
                { CreateDetachedCycle(9, 24, 25, 10, 9), +1 },
                { CreateDetachedCycle(11, 26, 27, 12, 11), +1 },
                { CreateDetachedCycle(13, 28, 29, 14, 13), +1 },
                { CreateDetachedCycle(15, 30, 31, 16, 15), +1 },
                { CreateDetachedCycle(17, 32, 33, 18, 17), +1 },
                { CreateDetachedCycle(19, 34, 21, 20, 19), +1 },

                { CreateDetachedCycle(7, 22, 21, 20, 7), -1 },
                { CreateDetachedCycle(9, 24, 23, 8, 9), -1 },
                { CreateDetachedCycle(11, 26, 25, 10, 11), -1 },
                { CreateDetachedCycle(13, 28, 27, 12, 13), -1 },
                { CreateDetachedCycle(15, 30, 29, 14, 15), -1 },
                { CreateDetachedCycle(17, 32, 31, 16, 17), -1 },
                { CreateDetachedCycle(19, 34, 33, 18, 19), -1 },
            };
            // Shift the vertices by firstVertex
            linCombDict = linCombDict.Select(pair =>
            {
                var newCycle = CreateDetachedCycle(pair.Key.CanonicalPath.Vertices.Select(vertex => vertex + firstVertex));
                return new KeyValuePair<DetachedCycle<int>, int>(newCycle, pair.Value);
            }).ToDictionary(pair => pair.Key, pair => pair.Value);
            var expectedPotential = CreatePotential(linCombDict);

            Assert.That(actualPotential, Is.EqualTo(expectedPotential));
        }

        // Messed up regression test (see my physical pad, 2018-10-31)
        [Test]
        public void TrySupplyComposition_ReturnsFalseWhenItShould()
        {
            var layerType = CreateLayerType(5, 5, 15, 10, 5);

            var nextCompositionParameters = generator.StartGeneration(layerType);
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(10));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(5));

            nextCompositionParameters = generator.SupplyComposition(CreateComposition(2, 2, 2, 2, 2));
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(20));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(10));

            nextCompositionParameters = generator.SupplyComposition(CreateComposition(Utility.RepeatMany(5, 2, 2)));
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(20));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(15));

            var success = generator.TrySupplyComposition(CreateComposition(Utility.RepeatMany(5, 1, 2, 1)), out nextCompositionParameters);
            Assert.That(success, Is.False);
            Assert.That(nextCompositionParameters, Is.Null);
        }

        // Regression test (see my physical pad, 2018-10-31)
        // This tests that the generator works when an implicit up arrow does not end with an
        // implicit down arrow
        [Test]
        public void SupplyComposition_DoesNotCrash_WhenImplicitUpArrowHasExplicitDownArrow()
        {
            var layerType = CreateLayerType(5, 5, 15, 10, 5);

            var nextCompositionParameters = generator.StartGeneration(layerType);
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(10));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(5));

            nextCompositionParameters = generator.SupplyComposition(CreateComposition(2, 2, 2, 2, 2));
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(20));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(10));

            nextCompositionParameters = generator.SupplyComposition(CreateComposition(Utility.RepeatMany(5, 3, 1)));
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(20));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(15));

            nextCompositionParameters = generator.SupplyComposition(CreateComposition(Utility.RepeatMany(5, 1, 2, 1)));
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(20));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(20));

            nextCompositionParameters = generator.SupplyComposition(CreateComposition(Utility.RepeatMany(5, 1, 1, 1, 1)));
            Assert.That(nextCompositionParameters, Is.Null);
        }

        // Regression test
        // This test has to do with implicit arrows not ending with an explicit arrow or so.
        [Test]
        public void SupplyComposition_DoesNotCrashAndDoesAsExpected()
        {
            var layerType = CreateLayerType(5, 5, 15, 10, 5);

            var nextCompositionParameters = generator.StartGeneration(layerType);
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(10));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(5));

            nextCompositionParameters = generator.SupplyComposition(CreateComposition(2, 2, 2, 2, 2));
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(20));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(10));

            nextCompositionParameters = generator.SupplyComposition(CreateComposition(Utility.RepeatMany(5, 2, 2)));
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(20));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(15));

            nextCompositionParameters = generator.SupplyComposition(CreateComposition(Utility.RepeatMany(5, 1, 1, 2)));
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(20));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(20));

            nextCompositionParameters = generator.SupplyComposition(CreateComposition(Utility.RepeatMany(5, 1, 1, 1, 1)));
            Assert.That(nextCompositionParameters, Is.Null);
        }

        // Regression test
        // Test for the bug on four layers discovered 2018-11-01.
        [Test]
        public void SupplyComposition_DoesNotCrashAndDoesAsExpected_OnFourLayers()
        {
            var layerType = CreateLayerType(4, 4, 8, 8, 8, 4, 4);

            var nextCompositionParameters = generator.StartGeneration(layerType);
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(8));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(4));

            nextCompositionParameters = generator.SupplyComposition(CreateComposition(Utility.RepeatMany(4, 2)));
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(12));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(8));

            nextCompositionParameters = generator.SupplyComposition(CreateComposition(Utility.RepeatMany(4, 2, 1)));
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(12));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(8));

            nextCompositionParameters = generator.SupplyComposition(CreateComposition(Utility.RepeatMany(4, 2, 1)));
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(16));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(16));

            nextCompositionParameters = generator.SupplyComposition(CreateComposition(Utility.RepeatMany(4, 1, 1, 1, 1)));
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(8));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(8));

            nextCompositionParameters = generator.SupplyComposition(CreateComposition(Utility.RepeatMany(4, 1, 1)));
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(12));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(8));

            nextCompositionParameters = generator.SupplyComposition(CreateComposition(Utility.RepeatMany(4, 1, 2)));
            Assert.That(nextCompositionParameters, Is.Null);
        }

        // Regression test
        // Test for the bug on four layers discovered 2018-11-04.
        // Checks that the swapping of up-down pairs to down-up pairs can occur even at the start
        // of the layer (just after an implicit up arrow)
        [Test]
        public void QuiverGeneration_GivesQPSatisfyingTheSufficientConditionForHavingASemimonomialJacobianAlgebra()
        {
            var layerType = CreateLayerType(4, 4, 8, 8, 12, 8, 4);

            var nextCompositionParameters = generator.StartGeneration(layerType);
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(8));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(4));

            // Distribute pairs
            nextCompositionParameters = generator.SupplyComposition(CreateComposition(Utility.RepeatMany(4, 2)));
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(12));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(8));

            nextCompositionParameters = generator.SupplyComposition(CreateComposition(Utility.RepeatMany(4, 2, 1)));
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(12));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(8));

            // Distribute pairs
            nextCompositionParameters = generator.SupplyComposition(CreateComposition(Utility.RepeatMany(4, 2, 1)));
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(20));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(16));

            nextCompositionParameters = generator.SupplyComposition(CreateComposition(Utility.RepeatMany(4, 2, 1, 1, 1)));
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(16));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(12));

            // Distribute pairs
            nextCompositionParameters = generator.SupplyComposition(CreateComposition(Utility.RepeatMany(4, 2, 1, 1)));
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(16));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(16));

            nextCompositionParameters = generator.SupplyComposition(CreateComposition(Utility.RepeatMany(4, 1, 1, 1, 1)));
            Assert.That(nextCompositionParameters, Is.Null);

            var qp = generator.EndGeneration().QP;
            Assert.That(() => SemimonomialUnboundQuiverFactory.CreateSemimonomialUnboundQuiverFromQP(qp), Throws.Nothing);
        }

        // Regression test
        // Test for the bug introduced when fixing the first bug of 2018-11-04.
        // Seems to have been just an off-by-one error (too eager to turn up-down pairs into down-up pairs)
        [Test]
        public void QuiverGeneration_DoesNotCrashAndDoesAsExpected()
        {
            var layerType = CreateLayerType(4, 4, 4, 4, 8, 8, 8);

            var nextCompositionParameters = generator.StartGeneration(layerType);
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(8));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(4));

            // Distribute pairs
            nextCompositionParameters = generator.SupplyComposition(CreateComposition(Utility.RepeatMany(4, 2)));
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(8));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(8));

            nextCompositionParameters = generator.SupplyComposition(CreateComposition(Utility.RepeatMany(4, 1, 1)));
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(8));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(4));

            // Distribute pairs
            nextCompositionParameters = generator.SupplyComposition(CreateComposition(Utility.RepeatMany(4, 2)));
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(12));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(8));

            nextCompositionParameters = generator.SupplyComposition(CreateComposition(Utility.RepeatMany(4, 2, 1)));
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(12));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(8));

            // Distribute pairs
            nextCompositionParameters = generator.SupplyComposition(CreateComposition(Utility.RepeatMany(4, 1, 2)));
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(16));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(16));

            nextCompositionParameters = generator.SupplyComposition(CreateComposition(Utility.RepeatMany(4, 1, 1, 1, 1)));
            Assert.That(nextCompositionParameters, Is.Null);
        }

        [Test]
        public void QuiverGeneration_FromPoint_Works()
        {
            var layerType = CreateLayerType(1, 3, 6);

            var nextCompositionParameters = generator.StartGeneration(layerType);
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(4));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(1));

            // Distribute arrow pairs
            nextCompositionParameters = generator.SupplyComposition(CreateComposition(4));
            Assert.That(nextCompositionParameters.Sum, Is.EqualTo(6));
            Assert.That(nextCompositionParameters.NumTerms, Is.EqualTo(6));

            // Distribute arrows to polygons
            nextCompositionParameters = generator.SupplyComposition(CreateComposition(1, 1, 1, 1, 1, 1));
            Assert.That(nextCompositionParameters, Is.Null);

            var output = generator.EndGeneration();
            var actualPotential = output.QP.Potential;
            var expectedPotential = CreatePotential(new Dictionary<DetachedCycle<int>, int>
            {
                { CreateDetachedCycle(1,3,2,1), -1 },
                { CreateDetachedCycle(1,3,4,1), +1 },
                { CreateDetachedCycle(1,5,4,1), -1 },
                { CreateDetachedCycle(1,5,6,1), +1 },
                { CreateDetachedCycle(1,7,6,1), -1 },
                { CreateDetachedCycle(1,7,2,1), +1 },
            });
            Assert.That(actualPotential, Is.EqualTo(expectedPotential));
        }

        [Test]
        public void QuiverGeneration_WithStartGenerationFromBase_GeneratesOddFlower5FromCobweb5Correctly()
        {
            var quiverInPlane = UsefulQuiversInPlane.GetCobwebQuiverInPlane(5, 50, firstVertex: 1);
            var qp = UsefulQPs.GetCobwebQP(5, firstVertex: 1);
            var potential = qp.Potential;
            var boundaryLayer = UsefulQPs.GetVerticesInCobwebQPLayer(5, 1, firstVertex: 1);
            var layerType = CreateLayerType(10, 5, 20); // 5 arrow pairs, all implicit

            generator.StartGenerationFromBase(quiverInPlane, potential, boundaryLayer, layerType, nextVertex: qp.Quiver.Vertices.Count + 1);
            generator.SupplyComposition(CreateComposition(Utility.RepeatMany(5, 1, 1))); // No choice for the 5 arrow pairs
            generator.SupplyComposition(CreateComposition(Utility.RepeatMany(5, 3, 3))); // 2 steps left and right (both of which can be 0)

            var output = generator.EndGeneration();
            qp = output.QP;
            potential = qp.Potential;
            var expectedPotential = CreatePotential(new Dictionary<DetachedCycle<int>, int>
            {
                { CreateDetachedCycle(1, 2, 3, 4, 5, 1), +1 },

                { CreateDetachedCycle(1, 2, 7, 6, 1), -1 },
                { CreateDetachedCycle(2, 3, 9, 8, 2), -1 },
                { CreateDetachedCycle(3, 4, 11, 10, 3), -1 },
                { CreateDetachedCycle(4, 5, 13, 12, 4), -1 },
                { CreateDetachedCycle(5, 1, 15, 14, 5), -1 },

                { CreateDetachedCycle(1, 15, 6, 1), +1 },
                { CreateDetachedCycle(2, 7, 8, 2), +1 },
                { CreateDetachedCycle(3, 9, 10, 3), +1 },
                { CreateDetachedCycle(4, 11, 12, 4), +1 },
                { CreateDetachedCycle(5, 13, 14, 5), +1 },

                { CreateDetachedCycle(6, 18, 17, 16, 15, 6), -1},
                { CreateDetachedCycle(8, 22, 21, 20, 7, 8), -1 },
                { CreateDetachedCycle(10, 26, 25, 24, 9, 10), -1 },
                { CreateDetachedCycle(12, 30, 29, 28, 11, 12), -1 },
                { CreateDetachedCycle(14, 34, 33, 32, 13, 14), -1 },

                { CreateDetachedCycle(6, 18, 19, 20, 7, 6), +1 },
                { CreateDetachedCycle(8, 22, 23, 24, 9, 8), +1 },
                { CreateDetachedCycle(10, 26, 27, 28, 11, 10), +1 },
                { CreateDetachedCycle(12, 30, 31, 32, 13, 12), +1 },
                { CreateDetachedCycle(14, 34, 35, 16, 15, 14), +1 },
            });

            Assert.That(potential, Is.EqualTo(expectedPotential));
        }
    }
}
