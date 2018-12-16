using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using SelfInjectiveQuiversWithPotential;
using SelfInjectiveQuiversWithPotential.Plane;

namespace SelfInjectiveQuiversWithPotentialTests
{
    [TestFixture]
    public class QPExtractorTestFixture
    {
        public QPExtractor CreateQPExtractor()
        {
            return new QPExtractor();
        }

        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(10)]
        public void TryExtractQP_WorksOnCounterclockwiseCycle(int cycleLength)
        {
            const double Radius = 1000;
            var vertices = Enumerable.Range(1, cycleLength);
            var arrows = vertices.Select(k => new Arrow<int>(k, k.Modulo(cycleLength) + 1));
            double baseAngle = 2 * Math.PI / cycleLength;
            var vertexPositions = vertices.ToDictionary(k => k, k => new Point((int)(Radius * Math.Cos(k * baseAngle)), (int)Math.Round(Radius * Math.Sin(k * baseAngle))));
            var quiverInPlane = new QuiverInPlane<int>(vertices, arrows, vertexPositions);
            var extractor = CreateQPExtractor();

            var result = extractor.TryExtractQP(quiverInPlane, out var qp);
            var expectedQuiver = new Quiver<int>(vertices, arrows);
            var expectedPotential = new Potential<int>(new DetachedCycle<int>(vertices.AppendElement(1)), -1);
            var expectedQP = new QuiverWithPotential<int>(expectedQuiver, expectedPotential);
            Assert.That(result, Is.EqualTo(QPExtractionResult.Success));
            Assert.That(qp, Is.EqualTo(expectedQP));
        }

        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(10)]
        public void TryExtractQP_WorksOnClockwiseCycle(int cycleLength)
        {
            const double Radius = 1000;
            var vertices = Enumerable.Range(1, cycleLength);
            var arrows = vertices.Select(k => new Arrow<int>(k, k.Modulo(cycleLength) + 1));
            double baseAngle = 2 * Math.PI / cycleLength;
            var vertexPositions = vertices.ToDictionary(k => k, k => new Point((int)(Radius * Math.Cos(-k * baseAngle)), (int)Math.Round(Radius * Math.Sin(-k * baseAngle))));
            var quiverInPlane = new QuiverInPlane<int>(vertices, arrows, vertexPositions);
            var extractor = CreateQPExtractor();

            var result = extractor.TryExtractQP(quiverInPlane, out var qp);
            var expectedQuiver = new Quiver<int>(vertices, arrows);
            var expectedPotential = new Potential<int>(new DetachedCycle<int>(vertices.AppendElement(1)), +1);
            var expectedQP = new QuiverWithPotential<int>(expectedQuiver, expectedPotential);
            Assert.That(result, Is.EqualTo(QPExtractionResult.Success));
            Assert.That(qp, Is.EqualTo(expectedQP));
        }

        // Regression test
        // The problem was that the face search found an already found face when restarting the
        // search on a boundary arrow:
        // Start   with 5 -> 1 and get 5 -> 1 -> 2 -> 5
        // Restart with 4 -> 5 and get 4 -> 5 -> 1 -> 2 -> 5, thus refinding the cycle!
        //
        // With the new implementation of QPExtractor, this might no longer be a useful test
        [Test]
        public void TryExtractQP_DoesNotThrow_WhenEncounteringFaceMoreThanOnce()
        {
            var vertices = new int[] { 1, 2, 3, 4, 5 };
            var arrows = new Arrow<int>[]
            {
                new Arrow<int>(1, 2),
                new Arrow<int>(2, 3),
                new Arrow<int>(3, 1),
                new Arrow<int>(1, 4),
                new Arrow<int>(4, 5),
                new Arrow<int>(5, 1),
                new Arrow<int>(2, 5)
            };

            var vertexPositions = new Dictionary<int, Point>
            {
                { 1, new Point(0, 0) },
                { 2, new Point(1, -1) },
                { 3, new Point(-1, -1) },
                { 4, new Point(-1, 1) },
                { 5, new Point(1, 1) },
            };

            var quiverInPlane = new QuiverInPlane<int>(vertices, arrows, vertexPositions);
            var extractor = CreateQPExtractor();

            Assert.That(() => extractor.TryExtractQP(quiverInPlane, out _), Throws.Nothing);
        }

        // Regression test
        // This is the test case that made me realize that the search for faces needs to be done
        // in the underlying graph, not in the directed quiver.
        [Test]
        public void TryExtractQP_ReturnsQuiverHasFaceWithInconsistentOrientation_For4CycleWithMiddleArrow()
        {
            var vertices = new int[] { 1, 2, 3, 4 };
            var arrows = vertices.Select(k => new Arrow<int>(k, (k % 4) + 1));
            arrows = arrows.AppendElement(new Arrow<int>(2, 4));
            var vertexPositions = vertices.ToDictionary(k => k, k => new Point(
                (int)(1000 * Math.Cos(k * 2 * Math.PI / vertices.Count())),
                (int)(1000 * Math.Sin(k * 2 * Math.PI / vertices.Count()))));
            var quiverInPlane = new QuiverInPlane<int>(vertices, arrows, vertexPositions);
            var extractor = CreateQPExtractor();

            Assert.That(extractor.TryExtractQP(quiverInPlane, out _), Is.EqualTo(QPExtractionResult.QuiverHasFaceWithInconsistentOrientation));
        }
    }
}
