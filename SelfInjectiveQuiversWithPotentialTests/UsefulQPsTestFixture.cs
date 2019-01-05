using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using SelfInjectiveQuiversWithPotential;
using SelfInjectiveQuiversWithPotential.Analysis;

namespace SelfInjectiveQuiversWithPotentialTests
{
    // The tests for the GetVertices... and GetPeriods... have been copy-pasted from UsefulQuiversTestFixture
    [TestFixture]
    public class UsefulQPsTestFixture
    {
        private void AssertIsSelfInjective<TVertex>(QuiverWithPotential<TVertex> qp)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            var analyzer = new QPAnalyzer();
            var result = analyzer.Analyze(qp, new QPAnalysisSettings(CancellativityTypes.Cancellativity));
            Assert.That(result.MainResult.IndicatesSelfInjectivity());
        }

        private void AssertAreSelfInjective<TVertex>(IEnumerable<QuiverWithPotential<TVertex>> qps)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            foreach (var qp in qps)
            {
                AssertIsSelfInjective(qp);
            }
        }

        #region Triangle
        [TestCase(-1)]
        [TestCase(-2)]
        [TestCase(-10)]
        public void GetVerticesInTriangleQPRow_ThrowsOnBadRowIndex(int rowIndex)
        {
            Assert.That(() => UsefulQPs.GetVerticesInTriangleQPRow(rowIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(0, 0, 0)]
        [TestCase(0, 1, 1)]
        [TestCase(0, 10, 10)]
        [TestCase(0, -1, -1)]
        [TestCase(0, -10, -10)]
        [TestCase(1, 0, 1, 2)]
        [TestCase(1, 1, 2, 3)]
        [TestCase(1, 10, 11, 12)]
        [TestCase(1, -1, 0, 1)]
        [TestCase(1, -10, -9, -8)]
        [TestCase(3, 0, 6, 7, 8, 9)]
        [TestCase(3, 1, 7, 8, 9, 10)]
        [TestCase(3, 10, 16, 17, 18, 19)]
        [TestCase(3, -1, 5, 6, 7, 8)]
        [TestCase(3, -10, -4, -3, -2, -1)]
        public void GetVerticesInTriangleQPRow_Works(int rowIndex, int firstVertex, params int[] expectedVertices)
        {
            var actualVertices = UsefulQPs.GetVerticesInTriangleQPRow(rowIndex, firstVertex);
            Assert.That(actualVertices, Is.EqualTo(expectedVertices));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-10)]
        public void GetVerticesInSquareQuiverRow_ThrowsOnBadNumberOfSquaresInRow(int numRows)
        {
            int rowIndex = 0;
            Assert.That(() => UsefulQuivers.GetVerticesInSquareQuiverRow(numRows, rowIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }
        #endregion

        #region Square
        [TestCase(1, 1)]
        [TestCase(1, 10)]
        [TestCase(1, -1)]
        [TestCase(1, -10)]
        [TestCase(2, 2)]
        [TestCase(2, 10)]
        [TestCase(2, -1)]
        [TestCase(2, -10)]
        [TestCase(6, 6)]
        [TestCase(6, 10)]
        [TestCase(6, -1)]
        [TestCase(6, -10)]
        public void GetVerticesInSquareQPRow_ThrowsOnBadRowIndex(int numRows, int rowIndex)
        {
            Assert.That(() => UsefulQPs.GetVerticesInSquareQPRow(numRows, rowIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(1, 0, 0, new int[] { 0 })]
        [TestCase(1, 0, 1, new int[] { 1 })]
        [TestCase(1, 0, 10, new int[] { 10 })]
        [TestCase(1, 0, -1, new int[] { -1 })]
        [TestCase(1, 0, -10, new int[] { -10 })]
        [TestCase(6, 0, 0, new int[] { 0, 1, 2, 3, 4, 5 })]
        [TestCase(6, 0, 1, new int[] { 1, 2, 3, 4, 5, 6 })]
        [TestCase(6, 0, 10, new int[] { 10, 11, 12, 13, 14, 15 })]
        [TestCase(6, 0, -1, new int[] { -1, 0, 1, 2, 3, 4 })]
        [TestCase(6, 0, -10, new int[] { -10, -9, -8, -7, -6, -5 })]
        [TestCase(6, 2, 0, new int[] { 12, 13, 14, 15, 16, 17 })]
        [TestCase(6, 2, 1, new int[] { 13, 14, 15, 16, 17, 18 })]
        [TestCase(6, 2, 10, new int[] { 22, 23, 24, 25, 26, 27 })]
        [TestCase(6, 2, -1, new int[] { 11, 12, 13, 14, 15, 16 })]
        [TestCase(6, 2, -10, new int[] { 2, 3, 4, 5, 6, 7 })]
        [TestCase(6, 5, 0, new int[] { 30, 31, 32, 33, 34, 35 })]
        [TestCase(6, 5, 1, new int[] { 31, 32, 33, 34, 35, 36 })]
        [TestCase(6, 5, 10, new int[] { 40, 41, 42, 43, 44, 45 })]
        [TestCase(6, 5, -1, new int[] { 29, 30, 31, 32, 33, 34 })]
        [TestCase(6, 5, -10, new int[] { 20, 21, 22, 23, 24, 25 })]
        public void GetVerticesInSquareQPRow_Works(int numRows, int rowIndex, int firstVertex, params int[] expectedVertices)
        {
            var actualVertices = UsefulQPs.GetVerticesInSquareQPRow(numRows, rowIndex, firstVertex);
            Assert.That(actualVertices, Is.EqualTo(expectedVertices));
        }
        #endregion

        #region Cobweb
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(-6)]
        [TestCase(2)]
        [TestCase(4)]
        [TestCase(6)]
        [TestCase(20)]
        public void GetNumberOfLayersInCobwebQP_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            Assert.That(() => UsefulQPs.GetNumberOfLayersInCobwebQP(numVerticesInCenterPolygon), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(3, 1)]
        [TestCase(5, 2)]
        [TestCase(7, 3)]
        [TestCase(9, 4)]
        public void GetNumberOfLayersInCobwebQP_Works(int numVerticesInCenterPolygon, int expectedNumLayers)
        {
            Assert.That(UsefulQPs.GetNumberOfLayersInCobwebQP(numVerticesInCenterPolygon), Is.EqualTo(expectedNumLayers));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(-6)]
        [TestCase(2)]
        [TestCase(4)]
        [TestCase(6)]
        [TestCase(20)]
        public void GetVerticesInCobwebQPLayer_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            int layerIndex = 0;
            Assert.That(() => UsefulQPs.GetVerticesInCobwebQPLayer(numVerticesInCenterPolygon, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(-1)]
        [TestCase(-2)]
        [TestCase(-5)]
        [TestCase(Int32.MinValue)]
        public void GetVerticesInCobwebQPLayer_ThrowsOnBadLayerIndex(int layerIndex)
        {
            int numVerticesInCenterPolygon = 5;
            Assert.That(() => UsefulQPs.GetVerticesInCobwebQPLayer(numVerticesInCenterPolygon, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(3, 0, 0, new int[] { 0, 1, 2 })]
        [TestCase(3, 0, 1, new int[] { 1, 2, 3 })]
        [TestCase(3, 0, 10, new int[] { 10, 11, 12 })]
        [TestCase(3, 0, -1, new int[] { -1, 0, 1 })]
        [TestCase(3, 0, -10, new int[] { -10, -9, -8 })]
        [TestCase(7, 0, 0, new int[] { 0, 1, 2, 3, 4, 5, 6 })]
        [TestCase(7, 0, 1, new int[] { 1, 2, 3, 4, 5, 6, 7 })]
        [TestCase(7, 0, 10, new int[] { 10, 11, 12, 13, 14, 15, 16 })]
        [TestCase(7, 0, -1, new int[] { -1, 0, 1, 2, 3, 4, 5 })]
        [TestCase(7, 0, -10, new int[] { -10, -9, -8, -7, -6, -5, -4 })]
        [TestCase(7, 1, 0, new int[] { 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 })]
        [TestCase(7, 1, 1, new int[] { 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21 })]
        [TestCase(7, 1, 10, new int[] { 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 })]
        [TestCase(7, 1, -1, new int[] { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 })]
        [TestCase(7, 1, -10, new int[] { -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
        [TestCase(7, 2, 0, new int[] { 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34 })]
        [TestCase(7, 2, 1, new int[] { 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35 })]
        [TestCase(7, 2, 10, new int[] { 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44 })]
        [TestCase(7, 2, -1, new int[] { 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 })]
        [TestCase(7, 2, -10, new int[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 })]
        public void GetVerticesInCobwebQPLayer_Works(int numVerticesInCenterPolygon, int layerIndex, int firstVertex, params int[] expectedVertices)
        {
            var actualVertices = UsefulQPs.GetVerticesInCobwebQPLayer(numVerticesInCenterPolygon, layerIndex, firstVertex);
            Assert.That(actualVertices, Is.EqualTo(expectedVertices));
        }
        #endregion

        #region Odd flower
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(-6)]
        [TestCase(2)]
        [TestCase(4)]
        [TestCase(6)]
        [TestCase(20)]
        public void GetNumberOfLayersInOddFlowerQP_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            Assert.That(() => UsefulQPs.GetNumberOfLayersInOddFlowerQP(numVerticesInCenterPolygon), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(3, 2)]
        [TestCase(5, 3)]
        [TestCase(7, 4)]
        [TestCase(9, 5)]
        public void GetNumberOfLayersInOddFlowerQP_Works(int numVerticesInCenterPolygon, int expectedNumLayers)
        {
            Assert.That(UsefulQPs.GetNumberOfLayersInOddFlowerQP(numVerticesInCenterPolygon), Is.EqualTo(expectedNumLayers));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(-6)]
        [TestCase(2)]
        [TestCase(4)]
        [TestCase(6)]
        [TestCase(20)]
        public void GetVerticesInOddFlowerQPLayer_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            int layerIndex = 0;
            Assert.That(() => UsefulQPs.GetVerticesInOddFlowerQPLayer(numVerticesInCenterPolygon, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(-1)]
        [TestCase(-2)]
        [TestCase(-5)]
        [TestCase(Int32.MinValue)]
        public void GetVerticesInOddFlowerQPLayer_ThrowsOnBadLayerIndex(int layerIndex)
        {
            int numVerticesInCenterPolygon = 5;
            Assert.That(() => UsefulQPs.GetVerticesInOddFlowerQPLayer(numVerticesInCenterPolygon, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(3, 0, 0, new int[] { 0, 1, 2 })]
        [TestCase(3, 0, 1, new int[] { 1, 2, 3 })]
        [TestCase(3, 0, 10, new int[] { 10, 11, 12 })]
        [TestCase(3, 0, -1, new int[] { -1, 0, 1 })]
        [TestCase(3, 0, -10, new int[] { -10, -9, -8 })]
        [TestCase(3, 1, 0, new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 })]
        [TestCase(3, 1, 1, new int[] { 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 })]
        [TestCase(3, 1, 10, new int[] { 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 })]
        [TestCase(3, 1, -1, new int[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 })]
        [TestCase(3, 1, -10, new int[] { -7, -6, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4 })]
        [TestCase(5, 0, 0, new int[] { 0, 1, 2, 3, 4 })]
        [TestCase(5, 0, 1, new int[] { 1, 2, 3, 4, 5 })]
        [TestCase(5, 0, 10, new int[] { 10, 11, 12, 13, 14 })]
        [TestCase(5, 0, -1, new int[] { -1, 0, 1, 2, 3 })]
        [TestCase(5, 0, -10, new int[] { -10, -9, -8, -7, -6 })]
        [TestCase(5, 1, 0, new int[] { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 })]
        [TestCase(5, 1, 1, new int[] { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 })]
        [TestCase(5, 1, 10, new int[] { 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 })]
        [TestCase(5, 1, -1, new int[] { 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 })]
        [TestCase(5, 1, -10, new int[] { -5, -4, -3, -2, -1, 0, 1, 2, 3, 4 })]
        [TestCase(5, 2, 0, new int[] { 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34 })]
        [TestCase(5, 2, 1, new int[] { 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35 })]
        [TestCase(5, 2, 10, new int[] { 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44 })]
        [TestCase(5, 2, -1, new int[] { 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 })]
        [TestCase(5, 2, -10, new int[] { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 })]
        [TestCase(7, 0, 0, new int[] { 0, 1, 2, 3, 4, 5, 6 })]
        [TestCase(7, 0, 1, new int[] { 1, 2, 3, 4, 5, 6, 7 })]
        [TestCase(7, 0, 10, new int[] { 10, 11, 12, 13, 14, 15, 16 })]
        [TestCase(7, 0, -1, new int[] { -1, 0, 1, 2, 3, 4, 5 })]
        [TestCase(7, 0, -10, new int[] { -10, -9, -8, -7, -6, -5, -4 })]
        [TestCase(7, 1, 0, new int[] { 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 })]
        [TestCase(7, 1, 1, new int[] { 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21 })]
        [TestCase(7, 1, 10, new int[] { 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 })]
        [TestCase(7, 1, -1, new int[] { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 })]
        [TestCase(7, 1, -10, new int[] { -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
        [TestCase(7, 2, 0, new int[] { 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34 })]
        [TestCase(7, 2, 1, new int[] { 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35 })]
        [TestCase(7, 2, 10, new int[] { 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44 })]
        [TestCase(7, 2, -1, new int[] { 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 })]
        [TestCase(7, 2, -10, new int[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 })]
        public void GetVerticesInOddFlowerQPLayer_Works(int numVerticesInCenterPolygon, int layerIndex, int firstVertex, params int[] expectedVertices)
        {
            var actualVertices = UsefulQPs.GetVerticesInOddFlowerQPLayer(numVerticesInCenterPolygon, layerIndex, firstVertex);
            Assert.That(actualVertices, Is.EqualTo(expectedVertices));
        }

        [TestCase(0, 0)]
        [TestCase(-1, 0)]
        [TestCase(-5, 0)]
        [TestCase(-6, 0)]
        [TestCase(1, 0)]
        [TestCase(2, 0)]
        [TestCase(4, 0)]
        [TestCase(6, 0)]
        [TestCase(20, 0)]
        [TestCase(0, -1)]
        [TestCase(-1, -1)]
        [TestCase(-5, -1)]
        [TestCase(-6, -1)]
        [TestCase(1, -1)]
        [TestCase(2, -1)]
        [TestCase(4, -1)]
        [TestCase(6, -1)]
        [TestCase(20, -1)]
        [TestCase(0, 1)]
        [TestCase(-1, 1)]
        [TestCase(-5, 1)]
        [TestCase(-6, 1)]
        [TestCase(1, 1)]
        [TestCase(2, 1)]
        [TestCase(4, 1)]
        [TestCase(6, 1)]
        [TestCase(20, 1)]
        public void GetPeriodsOfOddFlowerQP_ThrowsArgumentOutOfRangeException_OnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon, int firstVertex)
        {
            Assert.That(() => UsefulQPs.GetPeriodsOfOddFlowerQP(numVerticesInCenterPolygon, firstVertex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        static IEnumerable<TestCaseData> GetPeriodsOfOddFlowerQP_Works_TestCaseSource()
        {
            var firstVertices = new int[] { -1, 0, 1 };

            int numVerticesInCenterPolygon = 3;
            var zeroBasedPeriods = new int[][]
            {
                new int[] { 0, 3, 4, 5, 6 },
                new int[] { 1, 7, 8, 9, 10 },
                new int[] { 2, 11, 12, 13, 14 }
            };
            foreach (int firstVertex in firstVertices)
            {
                var expectedPeriods = zeroBasedPeriods.Select(period => period.Select(vertex => vertex + firstVertex));
                yield return new TestCaseData(numVerticesInCenterPolygon, firstVertex, expectedPeriods);
            }

            numVerticesInCenterPolygon = 5;
            zeroBasedPeriods = new int[][]
            {
                new int[] { 0, 5, 6, 15, 16, 17, 18 },
                new int[] { 1, 7, 8, 19, 20, 21, 22 },
                new int[] { 2, 9, 10, 23, 24, 25, 26 },
                new int[] { 3, 11, 12, 27, 28, 29, 30 },
                new int[] { 4, 13, 14, 31, 32, 33, 34 }
            };
            foreach (int firstVertex in firstVertices)
            {
                var expectedPeriods = zeroBasedPeriods.Select(period => period.Select(vertex => vertex + firstVertex));
                yield return new TestCaseData(numVerticesInCenterPolygon, firstVertex, expectedPeriods);
            }
        }

        [TestCaseSource(nameof(GetPeriodsOfOddFlowerQP_Works_TestCaseSource))]
        public void GetPeriodsOfOddFlowerQP_Works(int numVerticesInCenterPolygon, int firstVertex, IEnumerable<IEnumerable<int>> expectedPeriods)
        {
            var actualPeriods = UsefulQPs.GetPeriodsOfOddFlowerQP(numVerticesInCenterPolygon, firstVertex);
            Assert.That(actualPeriods, Is.EqualTo(expectedPeriods));
        }
        #endregion

        #region Even flower, type 1
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(-6)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(5)]
        [TestCase(19)]
        public void GetNumberOfLayersInEvenFlowerType1QP_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            Assert.That(() => UsefulQPs.GetNumberOfLayersInEvenFlowerType1QP(numVerticesInCenterPolygon), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(4, 2)]
        [TestCase(6, 3)]
        [TestCase(8, 4)]
        [TestCase(10, 5)]
        public void GetNumberOfLayersInEvenFlowerType1QP_Works(int numVerticesInCenterPolygon, int expectedNumLayers)
        {
            Assert.That(UsefulQPs.GetNumberOfLayersInEvenFlowerType1QP(numVerticesInCenterPolygon), Is.EqualTo(expectedNumLayers));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(-6)]
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(5)]
        [TestCase(19)]
        public void GetVerticesInEvenFlowerType1QPLayer_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            int layerIndex = 0;
            Assert.That(() => UsefulQPs.GetVerticesInEvenFlowerType1QPLayer(numVerticesInCenterPolygon, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(-1)]
        [TestCase(-2)]
        [TestCase(-5)]
        [TestCase(Int32.MinValue)]
        public void GetVerticesInEvenFlowerType1QPLayer_ThrowsOnBadLayerIndex(int layerIndex)
        {
            int numVerticesInCenterPolygon = 6;
            Assert.That(() => UsefulQPs.GetVerticesInEvenFlowerType1QPLayer(numVerticesInCenterPolygon, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(4, 0, 0, new int[] { 0, 1, 2, 3 })]
        [TestCase(4, 0, 1, new int[] { 1, 2, 3, 4 })]
        [TestCase(4, 0, 10, new int[] { 10, 11, 12, 13 })]
        [TestCase(4, 0, -1, new int[] { -1, 0, 1, 2 })]
        [TestCase(4, 0, -10, new int[] { -10, -9, -8, -7 })]
        [TestCase(4, 1, 0, new int[] { 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 })]
        [TestCase(4, 1, 1, new int[] { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 })]
        [TestCase(4, 1, 10, new int[] { 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 })]
        [TestCase(4, 1, -1, new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 })]
        [TestCase(4, 1, -10, new int[] { -6, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5 })]
        [TestCase(6, 0, 0, new int[] { 0, 1, 2, 3, 4, 5 })]
        [TestCase(6, 0, 1, new int[] { 1, 2, 3, 4, 5, 6 })]
        [TestCase(6, 0, 10, new int[] { 10, 11, 12, 13, 14, 15 })]
        [TestCase(6, 0, -1, new int[] { -1, 0, 1, 2, 3, 4 })]
        [TestCase(6, 0, -10, new int[] { -10, -9, -8, -7, -6, -5 })]
        [TestCase(6, 1, 0, new int[] { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 })]
        [TestCase(6, 1, 1, new int[] { 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 })]
        [TestCase(6, 1, 10, new int[] { 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27 })]
        [TestCase(6, 1, -1, new int[] { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 })]
        [TestCase(6, 1, -10, new int[] { -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7 })]
        [TestCase(6, 2, 0, new int[] { 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35 })]
        [TestCase(6, 2, 1, new int[] { 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36 })]
        [TestCase(6, 2, 10, new int[] { 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45 })]
        [TestCase(6, 2, -1, new int[] { 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34 })]
        [TestCase(6, 2, -10, new int[] { 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 })]
        [TestCase(8, 0, 0, new int[] { 0, 1, 2, 3, 4, 5, 6, 7 })]
        [TestCase(8, 0, 1, new int[] { 1, 2, 3, 4, 5, 6, 7, 8 })]
        [TestCase(8, 0, 10, new int[] { 10, 11, 12, 13, 14, 15, 16, 17 })]
        [TestCase(8, 0, -1, new int[] { -1, 0, 1, 2, 3, 4, 5, 6 })]
        [TestCase(8, 0, -10, new int[] { -10, -9, -8, -7, -6, -5, -4, -3 })]
        [TestCase(8, 1, 0, new int[] { 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 })]
        [TestCase(8, 1, 1, new int[] { 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 })]
        [TestCase(8, 1, 10, new int[] { 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 })]
        [TestCase(8, 1, -1, new int[] { 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22 })]
        [TestCase(8, 1, -10, new int[] { -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 })]
        [TestCase(8, 2, 0, new int[] { 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39 })]
        [TestCase(8, 2, 1, new int[] { 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40 })]
        [TestCase(8, 2, 10, new int[] { 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49 })]
        [TestCase(8, 2, -1, new int[] { 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38 })]
        [TestCase(8, 2, -10, new int[] { 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 })]
        public void GetVerticesInEvenFlowerType1QPLayer_Works(int numVerticesInCenterPolygon, int layerIndex, int firstVertex, params int[] expectedVertices)
        {
            var actualVertices = UsefulQPs.GetVerticesInEvenFlowerType1QPLayer(numVerticesInCenterPolygon, layerIndex, firstVertex);
            Assert.That(actualVertices, Is.EqualTo(expectedVertices));
        }

        [TestCase(0, 0)]
        [TestCase(-1, 0)]
        [TestCase(-5, 0)]
        [TestCase(-6, 0)]
        [TestCase(1, 0)]
        [TestCase(2, 0)]
        [TestCase(3, 0)]
        [TestCase(5, 0)]
        [TestCase(19, 0)]
        [TestCase(0, -1)]
        [TestCase(-1, -1)]
        [TestCase(-5, -1)]
        [TestCase(-6, -1)]
        [TestCase(1, -1)]
        [TestCase(2, -1)]
        [TestCase(3, -1)]
        [TestCase(5, -1)]
        [TestCase(19, -1)]
        [TestCase(0, 1)]
        [TestCase(-1, 1)]
        [TestCase(-5, 1)]
        [TestCase(-6, 1)]
        [TestCase(1, 1)]
        [TestCase(2, 1)]
        [TestCase(3, 1)]
        [TestCase(5, 1)]
        [TestCase(19, 1)]
        public void GetPeriodsOfEvenFlowerType1QP_ThrowsArgumentOutOfRangeException_OnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon, int firstVertex)
        {
            Assert.That(() => UsefulQPs.GetPeriodsOfEvenFlowerType1QP(numVerticesInCenterPolygon, firstVertex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        static IEnumerable<TestCaseData> GetPeriodsOfEvenFlowerType1QP_Works_TestCaseSource()
        {
            var firstVertices = new int[] { -1, 0, 1 };

            int numVerticesInCenterPolygon = 4;
            var zeroBasedPeriods = new int[][]
            {
                new int[] { 0, 4, 5, 6 },
                new int[] { 1, 7, 8, 9 },
                new int[] { 2, 10, 11, 12 },
                new int[] { 3, 13, 14, 15 }
            };
            foreach (int firstVertex in firstVertices)
            {
                var expectedPeriods = zeroBasedPeriods.Select(period => period.Select(vertex => vertex + firstVertex));
                yield return new TestCaseData(numVerticesInCenterPolygon, firstVertex, expectedPeriods);
            }

            numVerticesInCenterPolygon = 6;
            zeroBasedPeriods = new int[][]
            {
                new int[] { 0, 6, 7, 18, 19, 20 },
                new int[] { 1, 8, 9, 21, 22, 23 },
                new int[] { 2, 10, 11, 24, 25, 26 },
                new int[] { 3, 12, 13, 27, 28, 29 },
                new int[] { 4, 14, 15, 30, 31, 32 },
                new int[] { 5, 16, 17, 33, 34, 35 }
            };
            foreach (int firstVertex in firstVertices)
            {
                var expectedPeriods = zeroBasedPeriods.Select(period => period.Select(vertex => vertex + firstVertex));
                yield return new TestCaseData(numVerticesInCenterPolygon, firstVertex, expectedPeriods);
            }
        }

        [TestCaseSource(nameof(GetPeriodsOfEvenFlowerType1QP_Works_TestCaseSource))]
        public void GetPeriodsOfEvenFlowerType1QP_Works(int numVerticesInCenterPolygon, int firstVertex, IEnumerable<IEnumerable<int>> expectedPeriods)
        {
            var actualPeriods = UsefulQPs.GetPeriodsOfEvenFlowerType1QP(numVerticesInCenterPolygon, firstVertex);
            Assert.That(actualPeriods, Is.EqualTo(expectedPeriods));
        }
        #endregion

        #region Even flower, type 2
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(-6)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(5)]
        [TestCase(19)]
        public void GetNumberOfLayersInEvenFlowerType2QP_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            Assert.That(() => UsefulQPs.GetNumberOfLayersInEvenFlowerType2QP(numVerticesInCenterPolygon), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(4, 2)]
        [TestCase(6, 3)]
        [TestCase(8, 4)]
        [TestCase(10, 5)]
        public void GetNumberOfLayersInEvenFlowerType2QP_Works(int numVerticesInCenterPolygon, int expectedNumLayers)
        {
            Assert.That(UsefulQPs.GetNumberOfLayersInEvenFlowerType2QP(numVerticesInCenterPolygon), Is.EqualTo(expectedNumLayers));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(-6)]
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(5)]
        [TestCase(19)]
        public void GetVerticesInEvenFlowerType2QPLayer_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            int layerIndex = 0;
            Assert.That(() => UsefulQPs.GetVerticesInEvenFlowerType2QPLayer(numVerticesInCenterPolygon, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(-1)]
        [TestCase(-2)]
        [TestCase(-5)]
        [TestCase(Int32.MinValue)]
        public void GetVerticesInEvenFlowerType2QPLayer_ThrowsOnBadLayerIndex(int layerIndex)
        {
            int numVerticesInCenterPolygon = 6;
            Assert.That(() => UsefulQPs.GetVerticesInEvenFlowerType2QPLayer(numVerticesInCenterPolygon, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(4, 0, 0, new int[] { 0, 1, 2, 3 })]
        [TestCase(4, 0, 1, new int[] { 1, 2, 3, 4 })]
        [TestCase(4, 0, 10, new int[] { 10, 11, 12, 13 })]
        [TestCase(4, 0, -1, new int[] { -1, 0, 1, 2 })]
        [TestCase(4, 0, -10, new int[] { -10, -9, -8, -7 })]
        [TestCase(4, 1, 0, new int[] { 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 })]
        [TestCase(4, 1, 1, new int[] { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 })]
        [TestCase(4, 1, 10, new int[] { 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 })]
        [TestCase(4, 1, -1, new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 })]
        [TestCase(4, 1, -10, new int[] { -6, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5 })]
        [TestCase(6, 0, 0, new int[] { 0, 1, 2, 3, 4, 5 })]
        [TestCase(6, 0, 1, new int[] { 1, 2, 3, 4, 5, 6 })]
        [TestCase(6, 0, 10, new int[] { 10, 11, 12, 13, 14, 15 })]
        [TestCase(6, 0, -1, new int[] { -1, 0, 1, 2, 3, 4 })]
        [TestCase(6, 0, -10, new int[] { -10, -9, -8, -7, -6, -5 })]
        [TestCase(6, 1, 0, new int[] { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 })]
        [TestCase(6, 1, 1, new int[] { 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 })]
        [TestCase(6, 1, 10, new int[] { 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27 })]
        [TestCase(6, 1, -1, new int[] { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 })]
        [TestCase(6, 1, -10, new int[] { -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7 })]
        [TestCase(6, 2, 0, new int[] { 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35 })]
        [TestCase(6, 2, 1, new int[] { 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36 })]
        [TestCase(6, 2, 10, new int[] { 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45 })]
        [TestCase(6, 2, -1, new int[] { 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34 })]
        [TestCase(6, 2, -10, new int[] { 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 })]
        [TestCase(8, 0, 0, new int[] { 0, 1, 2, 3, 4, 5, 6, 7 })]
        [TestCase(8, 0, 1, new int[] { 1, 2, 3, 4, 5, 6, 7, 8 })]
        [TestCase(8, 0, 10, new int[] { 10, 11, 12, 13, 14, 15, 16, 17 })]
        [TestCase(8, 0, -1, new int[] { -1, 0, 1, 2, 3, 4, 5, 6 })]
        [TestCase(8, 0, -10, new int[] { -10, -9, -8, -7, -6, -5, -4, -3 })]
        [TestCase(8, 1, 0, new int[] { 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 })]
        [TestCase(8, 1, 1, new int[] { 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 })]
        [TestCase(8, 1, 10, new int[] { 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 })]
        [TestCase(8, 1, -1, new int[] { 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22 })]
        [TestCase(8, 1, -10, new int[] { -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 })]
        [TestCase(8, 2, 0, new int[] { 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39 })]
        [TestCase(8, 2, 1, new int[] { 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40 })]
        [TestCase(8, 2, 10, new int[] { 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49 })]
        [TestCase(8, 2, -1, new int[] { 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38 })]
        [TestCase(8, 2, -10, new int[] { 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 })]
        public void GetVerticesInEvenFlowerType2QPLayer_Works(int numVerticesInCenterPolygon, int layerIndex, int firstVertex, params int[] expectedVertices)
        {
            var actualVertices = UsefulQPs.GetVerticesInEvenFlowerType2QPLayer(numVerticesInCenterPolygon, layerIndex, firstVertex);
            Assert.That(actualVertices, Is.EqualTo(expectedVertices));
        }

        [TestCase(0, 0)]
        [TestCase(-1, 0)]
        [TestCase(-5, 0)]
        [TestCase(-6, 0)]
        [TestCase(1, 0)]
        [TestCase(2, 0)]
        [TestCase(3, 0)]
        [TestCase(5, 0)]
        [TestCase(19, 0)]
        [TestCase(0, -1)]
        [TestCase(-1, -1)]
        [TestCase(-5, -1)]
        [TestCase(-6, -1)]
        [TestCase(1, -1)]
        [TestCase(2, -1)]
        [TestCase(3, -1)]
        [TestCase(5, -1)]
        [TestCase(19, -1)]
        [TestCase(0, 1)]
        [TestCase(-1, 1)]
        [TestCase(-5, 1)]
        [TestCase(-6, 1)]
        [TestCase(1, 1)]
        [TestCase(2, 1)]
        [TestCase(3, 1)]
        [TestCase(5, 1)]
        [TestCase(19, 1)]
        public void GetPeriodsOfEvenFlowerType2QP_ThrowsArgumentOutOfRangeException_OnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon, int firstVertex)
        {
            Assert.That(() => UsefulQPs.GetPeriodsOfEvenFlowerType2QP(numVerticesInCenterPolygon, firstVertex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        static IEnumerable<TestCaseData> GetPeriodsOfEvenFlowerType2QP_Works_TestCaseSource()
        {
            var firstVertices = new int[] { -1, 0, 1 };

            int numVerticesInCenterPolygon = 4;
            var zeroBasedPeriods = new int[][]
            {
                new int[] { 0, 4, 5, 6 },
                new int[] { 1, 7, 8, 9 },
                new int[] { 2, 10, 11, 12 },
                new int[] { 3, 13, 14, 15 }
            };
            foreach (int firstVertex in firstVertices)
            {
                var expectedPeriods = zeroBasedPeriods.Select(period => period.Select(vertex => vertex + firstVertex));
                yield return new TestCaseData(numVerticesInCenterPolygon, firstVertex, expectedPeriods);
            }

            numVerticesInCenterPolygon = 6;
            zeroBasedPeriods = new int[][]
            {
                new int[] { 0, 6, 7, 18, 19, 20 },
                new int[] { 1, 8, 9, 21, 22, 23 },
                new int[] { 2, 10, 11, 24, 25, 26 },
                new int[] { 3, 12, 13, 27, 28, 29 },
                new int[] { 4, 14, 15, 30, 31, 32 },
                new int[] { 5, 16, 17, 33, 34, 35 }
            };
            foreach (int firstVertex in firstVertices)
            {
                var expectedPeriods = zeroBasedPeriods.Select(period => period.Select(vertex => vertex + firstVertex));
                yield return new TestCaseData(numVerticesInCenterPolygon, firstVertex, expectedPeriods);
            }
        }

        [TestCaseSource(nameof(GetPeriodsOfEvenFlowerType2QP_Works_TestCaseSource))]
        public void GetPeriodsOfEvenFlowerType2QP_Works(int numVerticesInCenterPolygon, int firstVertex, IEnumerable<IEnumerable<int>> expectedPeriods)
        {
            var actualPeriods = UsefulQPs.GetPeriodsOfEvenFlowerType2QP(numVerticesInCenterPolygon, firstVertex);
            Assert.That(actualPeriods, Is.EqualTo(expectedPeriods));
        }
        #endregion

        #region Pointed flower
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(-6)]
        [TestCase(2)]
        [TestCase(4)]
        [TestCase(6)]
        [TestCase(20)]
        public void GetNumberOfLayersInPointedFlowerQP_ThrowsOnBadNumberOfPeriods(int numPeriods)
        {
            Assert.That(() => UsefulQPs.GetNumberOfLayersInPointedFlowerQP(numPeriods), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(3, 2)]
        [TestCase(5, 3)]
        [TestCase(7, 4)]
        [TestCase(9, 5)]
        public void GetNumberOfLayersInPointedFlowerQP_Works(int numPeriods, int expectedNumLayers)
        {
            Assert.That(UsefulQPs.GetNumberOfLayersInPointedFlowerQP(numPeriods), Is.EqualTo(expectedNumLayers));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(-6)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        [TestCase(6)]
        [TestCase(20)]
        public void GetVerticesInPointedFlowerQPLayer_ThrowsOnBadNumberOfPeriods(int numPeriods)
        {
            int layerIndex = 0;
            Assert.That(() => UsefulQPs.GetVerticesInPointedFlowerQPLayer(numPeriods, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(3, Int32.MinValue)]
        [TestCase(3, -2)]
        [TestCase(3, -1)]
        [TestCase(3, 2)]
        [TestCase(3, 3)]
        [TestCase(3, Int32.MaxValue)]
        [TestCase(5, Int32.MinValue)]
        [TestCase(5, -2)]
        [TestCase(5, -1)]
        [TestCase(5, 3)]
        [TestCase(5, 4)]
        [TestCase(5, Int32.MaxValue)]
        public void GetVerticesInPointedFlowerQPLayer_ThrowsOnBadLayerIndex(int numPeriods, int layerIndex)
        {
            Assert.That(() => UsefulQPs.GetVerticesInPointedFlowerQPLayer(numPeriods, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(3, 0, 0, new int[] { 0 })]
        [TestCase(3, 0, 1, new int[] { 1 })]
        [TestCase(3, 0, 10, new int[] { 10 })]
        [TestCase(3, 0, -1, new int[] { -1 })]
        [TestCase(3, 0, -10, new int[] { -10 })]
        [TestCase(3, 1, 0, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 })]
        [TestCase(3, 1, 1, new int[] { 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
        [TestCase(3, 1, 10, new int[] { 11, 12, 13, 14, 15, 16, 17, 18, 19 })]
        [TestCase(3, 1, -1, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 })]
        [TestCase(3, 1, -10, new int[] { -9, -8, -7, -6, -5, -4, -3, -2, -1 })]
        [TestCase(5, 0, 0, new int[] { 0 })]
        [TestCase(5, 0, 1, new int[] { 1 })]
        [TestCase(5, 0, 10, new int[] { 10 })]
        [TestCase(5, 0, -1, new int[] { -1 })]
        [TestCase(5, 0, -10, new int[] { -10 })]
        [TestCase(5, 1, 0, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
        [TestCase(5, 1, 1, new int[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 })]
        [TestCase(5, 1, 10, new int[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 })]
        [TestCase(5, 1, -1, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 })]
        [TestCase(5, 1, -10, new int[] { -9, -8, -7, -6, -5, -4, -3, -2, -1, 0 })]
        [TestCase(5, 2, 0, new int[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 })]
        [TestCase(5, 2, 1, new int[] { 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26 })]
        [TestCase(5, 2, 10, new int[] { 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35 })]
        [TestCase(5, 2, -1, new int[] { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 })]
        [TestCase(5, 2, -10, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 })]
        [TestCase(7, 0, 0, new int[] { 0 })]
        [TestCase(7, 0, 1, new int[] { 1 })]
        [TestCase(7, 0, 10, new int[] { 10 })]
        [TestCase(7, 0, -1, new int[] { -1 })]
        [TestCase(7, 0, -10, new int[] { -10 })]
        [TestCase(7, 1, 0, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 })]
        [TestCase(7, 1, 1, new int[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 })]
        [TestCase(7, 1, 10, new int[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 })]
        [TestCase(7, 1, -1, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 })]
        [TestCase(7, 1, -10, new int[] { -9, -8, -7, -6, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4 })]
        [TestCase(7, 2, 0, new int[] { 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28 })]
        [TestCase(7, 2, 1, new int[] { 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 })]
        [TestCase(7, 2, 10, new int[] { 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38 })]
        [TestCase(7, 2, -1, new int[] { 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27 })]
        [TestCase(7, 2, -10, new int[] { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18 })]
        [TestCase(7, 3, 0, new int[] { 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49 })]
        [TestCase(7, 3, 1, new int[] { 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50 })]
        [TestCase(7, 3, 10, new int[] { 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59 })]
        [TestCase(7, 3, -1, new int[] { 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48 })]
        [TestCase(7, 3, -10, new int[] { 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39 })]
        public void GetVerticesInPointedFlowerQPLayer_Works(int numPeriods, int layerIndex, int firstVertex, params int[] expectedVertices)
        {
            var actualVertices = UsefulQPs.GetVerticesInPointedFlowerQPLayer(numPeriods, layerIndex, firstVertex);
            Assert.That(actualVertices, Is.EqualTo(expectedVertices));
        }

        [TestCase(0, 0)]
        [TestCase(-1, 0)]
        [TestCase(-5, 0)]
        [TestCase(-6, 0)]
        [TestCase(1, 0)]
        [TestCase(2, 0)]
        [TestCase(4, 0)]
        [TestCase(6, 0)]
        [TestCase(20, 0)]
        [TestCase(0, -1)]
        [TestCase(-1, -1)]
        [TestCase(-5, -1)]
        [TestCase(-6, -1)]
        [TestCase(1, -1)]
        [TestCase(2, -1)]
        [TestCase(4, -1)]
        [TestCase(6, -1)]
        [TestCase(20, -1)]
        [TestCase(0, 1)]
        [TestCase(-1, 1)]
        [TestCase(-5, 1)]
        [TestCase(-6, 1)]
        [TestCase(1, 1)]
        [TestCase(2, 1)]
        [TestCase(4, 1)]
        [TestCase(6, 1)]
        [TestCase(20, 1)]
        public void GetPeriodsOfPointedFlowerQP_ThrowsArgumentOutOfRangeException_OnBadNumberOfVerticesInCenterPolygon(int numPeriods, int firstVertex)
        {
            Assert.That(() => UsefulQPs.GetPeriodsOfPointedFlowerQPWithoutFixedPoint(numPeriods, firstVertex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        static IEnumerable<TestCaseData> GetPeriodsOfPointedFlowerQP_Works_TestCaseSource()
        {
            var firstVertices = new int[] { -1, 0, 1 };

            int numPeriods = 3;
            var zeroBasedPeriods = new int[][]
            {
                new int[] { 1, 2, 3 },
                new int[] { 4, 5, 6 },
                new int[] { 7, 8, 9 },
            };
            foreach (int firstVertex in firstVertices)
            {
                var expectedPeriods = zeroBasedPeriods.Select(period => period.Select(vertex => vertex + firstVertex));
                yield return new TestCaseData(numPeriods, firstVertex, expectedPeriods);
            }

            numPeriods = 5;
            zeroBasedPeriods = new int[][]
            {
                new int[] { 1, 2, 11, 12, 13 },
                new int[] { 3, 4, 14, 15, 16 },
                new int[] { 5, 6, 17, 18, 19 },
                new int[] { 7, 8, 20, 21, 22 },
                new int[] { 9, 10, 23, 24, 25 }
            };
            foreach (int firstVertex in firstVertices)
            {
                var expectedPeriods = zeroBasedPeriods.Select(period => period.Select(vertex => vertex + firstVertex));
                yield return new TestCaseData(numPeriods, firstVertex, expectedPeriods);
            }
        }

        [TestCaseSource(nameof(GetPeriodsOfPointedFlowerQP_Works_TestCaseSource))]
        public void GetPeriodsOfPointedFlowerQP_Works(int numPeriods, int firstVertex, IEnumerable<IEnumerable<int>> expectedPeriods)
        {
            var actualPeriods = UsefulQPs.GetPeriodsOfPointedFlowerQPWithoutFixedPoint(numPeriods, firstVertex);
            Assert.That(actualPeriods, Is.EqualTo(expectedPeriods));
        }
        #endregion

        #region Generalized cobweb
        [TestCase(2)]
        [TestCase(1)]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(Int32.MinValue)]
        public void GetVerticesInGeneralizedCobwebQPLayer_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            int numLayers = 2;
            int layerIndex = 0;
            Assert.That(() => UsefulQPs.GetVerticesInGeneralizedCobwebQPLayer(numVerticesInCenterPolygon, numLayers, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(Int32.MinValue)]
        public void GetVerticesInGeneralizedCobwebQPLayer_ThrowsOnBadNumberOfLayers(int numLayers)
        {
            int numVerticesInCenterPolygon = 5;
            int layerIndex = 0;
            Assert.That(() => UsefulQPs.GetVerticesInGeneralizedCobwebQPLayer(numVerticesInCenterPolygon, numLayers, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(-1)]
        [TestCase(-2)]
        [TestCase(-5)]
        [TestCase(Int32.MinValue)]
        public void GetVerticesInGeneralizedCobwebQPLayer_ThrowsOnBadLayerIndex(int layerIndex)
        {
            int numVerticesInCenterPolygon = 5;
            int numLayers = 2;
            Assert.That(() => UsefulQPs.GetVerticesInGeneralizedCobwebQPLayer(numVerticesInCenterPolygon, numLayers, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }
        #endregion
        // Not so clean to test self-injectivity, but it should capture most errors

        [Test]
        public void GetCycleQP_GivesSelfInjectiveQP()
        {
            var qps = Enumerable.Range(3, 28).Select(k => UsefulQPs.GetCycleQP(k));
            AssertAreSelfInjective(qps);
        }

        [Test]
        public void Triangles_AreSelfInjective()
        {
            var qps = Utility.InfiniteRange(2).Select(k => UsefulQPs.GetTriangleQP(k)).TakeWhile(qp => qp.Quiver.Vertices.Count < 20);
            AssertAreSelfInjective(qps);
        }

        [Test]
        public void Squares_AreSelfInjective()
        {
            var qps = Utility.InfiniteRange(1).Select(k => UsefulQPs.GetSquareQP(k)).TakeWhile(qp => qp.Quiver.Vertices.Count < 30);
            AssertAreSelfInjective(qps);
        }

        [Test]
        public void Cobwebs_AreSelfInjective()
        {
            var qps = Utility.InfiniteRange(5, 2).Select(k => UsefulQPs.GetCobwebQP(k)).TakeWhile(qp => qp.Quiver.Vertices.Count < 30);
            AssertAreSelfInjective(qps);
        }

        [Test]
        public void OddFlowers_AreSelfInjective()
        {
            var qps = Utility.InfiniteRange(3, 2).Select(k => UsefulQPs.GetOddFlowerQP(k)).TakeWhile(qp => qp.Quiver.Vertices.Count <= 35);
            AssertAreSelfInjective(qps);
        }

        [Test]
        public void EvenFlowersOfType1_AreSelfInjective()
        {
            var qps = Utility.InfiniteRange(4, 2).Select(k => UsefulQPs.GetEvenFlowerType1QP(k)).TakeWhile(qp => qp.Quiver.Vertices.Count <= 36);
            AssertAreSelfInjective(qps);
        }

        [Test]
        public void EvenFlowersOfType2_AreSelfInjective()
        {
            var qps = Utility.InfiniteRange(4, 2).Select(k => UsefulQPs.GetEvenFlowerType2QP(k)).TakeWhile(qp => qp.Quiver.Vertices.Count <= 36);
            AssertAreSelfInjective(qps);
        }

        [Test]
        public void PointedFlowers_AreSelfInjective()
        {
            var qps = Utility.InfiniteRange(3, 2).Select(k => UsefulQPs.GetPointedFlowerQP(k)).TakeWhile(qp => qp.Quiver.Vertices.Count <= 50);
            AssertAreSelfInjective(qps);
        }

        [Test]
        public void GetCycleQP_GivesSelfInjectiveQP_WithUnusualFirstVertex()
        {
            const int FirstVertex = -123;
            var qps = Enumerable.Range(3, 28).Select(k => UsefulQPs.GetCycleQP(k, FirstVertex));
            AssertAreSelfInjective(qps);
        }

        [Test]
        public void Triangles_AreSelfInjective_WithUnusualFirstVertex()
        {
            const int FirstVertex = -123;
            var qps = Utility.InfiniteRange(2).Select(k => UsefulQPs.GetTriangleQP(k, FirstVertex)).TakeWhile(qp => qp.Quiver.Vertices.Count < 30);
            AssertAreSelfInjective(qps);
        }

        [Test]
        public void Squares_AreSelfInjective_WithUnusualFirstVertex()
        {
            const int FirstVertex = -123;
            var qps = Utility.InfiniteRange(1).Select(k => UsefulQPs.GetSquareQP(k, FirstVertex)).TakeWhile(qp => qp.Quiver.Vertices.Count < 30);
            AssertAreSelfInjective(qps);
        }

        [Test]
        public void Cobwebs_AreSelfInjective_WithUnusualFirstVertex()
        {
            const int FirstVertex = -123;
            var qps = Utility.InfiniteRange(5, 2).Select(k => UsefulQPs.GetCobwebQP(k, FirstVertex)).TakeWhile(qp => qp.Quiver.Vertices.Count < 30);
            AssertAreSelfInjective(qps);
        }

        [Test]
        public void OddFlowers_AreSelfInjective_WithUnusualFirstVertex()
        {
            const int FirstVertex = -123;
            var qps = Utility.InfiniteRange(3, 2).Select(k => UsefulQPs.GetOddFlowerQP(k, FirstVertex)).TakeWhile(qp => qp.Quiver.Vertices.Count <= 35);
            AssertAreSelfInjective(qps);
        }

        [Test]
        public void EvenFlowersOfType1_AreSelfInjective_WithUnusualFirstVertex()
        {
            const int FirstVertex = -123;
            var qps = Utility.InfiniteRange(4, 2).Select(k => UsefulQPs.GetEvenFlowerType1QP(k, FirstVertex)).TakeWhile(qp => qp.Quiver.Vertices.Count <= 36);
            AssertAreSelfInjective(qps);
        }

        [Test]
        public void EvenFlowersOfType2_AreSelfInjective_WithUnusualFirstVertex()
        {
            const int FirstVertex = -123;
            var qps = Utility.InfiniteRange(4, 2).Select(k => UsefulQPs.GetEvenFlowerType2QP(k, FirstVertex)).TakeWhile(qp => qp.Quiver.Vertices.Count <= 36);
            AssertAreSelfInjective(qps);
        }

        [Test]
        public void PointedFlowers_AreSelfInjective_WithUnusualFirstVertex()
        {
            const int FirstVertex = -123;
            var qps = Utility.InfiniteRange(3, 2).Select(k => UsefulQPs.GetPointedFlowerQP(k, FirstVertex)).TakeWhile(qp => qp.Quiver.Vertices.Count <= 49);
            AssertAreSelfInjective(qps);
        }
    }
}
