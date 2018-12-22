using NUnit.Framework;
using SelfInjectiveQuiversWithPotential;
using System;
using System.Linq;

namespace SelfInjectiveQuiversWithPotentialTests
{
    [TestFixture]
    public class UsefulQuiversTestFixture
    {
        #region Cycle
        [TestCase(2)]
        [TestCase(1)]
        [TestCase(0)]
        [TestCase(-1)]
        public void GetCycleQuiver_ThrowsOnBadNumberOfVertices(int numVertices)
        {
            Assert.That(() => UsefulQuivers.GetCycleQuiver(numVertices), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(3, 0)]
        [TestCase(3, 1)]
        [TestCase(3, 10)]
        [TestCase(3, -1)]
        [TestCase(3, -10)]
        [TestCase(4, 0)]
        [TestCase(4, 1)]
        [TestCase(4, 10)]
        [TestCase(4, -1)]
        [TestCase(4, -10)]
        [TestCase(10, 0)]
        [TestCase(10, 1)]
        [TestCase(10, 10)]
        [TestCase(10, -1)]
        [TestCase(10, -10)]
        public void GetCycleQuiver_QuiverHasCorrectVertices(int numVertices, int firstVertex)
        {
            var expectedVertices = Enumerable.Range(firstVertex, numVertices);

            var quiver = UsefulQuivers.GetCycleQuiver(numVertices, firstVertex);
            CollectionAssert.AreEquivalent(expectedVertices, quiver.Vertices);
        }

        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(10)]
        public void GetCycleQuiver_QuiverHasCorrectNumberOfArrows(int numVertices)
        {
            int expectedNumArrows = numVertices;

            var quiver = UsefulQuivers.GetCycleQuiver(numVertices);
            Assert.That(quiver.Arrows.Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuivers.GetCycleQuiver(numVertices, 0);
            Assert.That(quiver.Arrows.Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuivers.GetCycleQuiver(numVertices, -6);
            Assert.That(quiver.Arrows.Count, Is.EqualTo(expectedNumArrows));
        }
        #endregion

        #region Triangle
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-10)]
        public void GetTriangleQuiver_ThrowsOnBadNumberOfRows(int numRows)
        {
            Assert.That(() => UsefulQuivers.GetTriangleQuiver(numRows), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(1, 0)]
        [TestCase(1, 1)]
        [TestCase(1, 10)]
        [TestCase(1, -1)]
        [TestCase(1, -10)]
        [TestCase(2, 0)]
        [TestCase(2, 1)]
        [TestCase(2, 10)]
        [TestCase(2, -1)]
        [TestCase(2, -10)]
        [TestCase(3, 0)]
        [TestCase(3, 1)]
        [TestCase(3, 10)]
        [TestCase(3, -1)]
        [TestCase(3, -10)]
        [TestCase(10, 0)]
        [TestCase(10, 1)]
        [TestCase(10, 10)]
        [TestCase(10, -1)]
        [TestCase(10, -10)]
        public void GetTriangleQuiver_QuiverHasCorrectVertices(int numRows, int firstVertex)
        {
            var expectedVertices = Enumerable.Range(firstVertex, Utility.TriangularNumber(numRows));
            var quiver = UsefulQuivers.GetTriangleQuiver(numRows, firstVertex);
            CollectionAssert.AreEquivalent(expectedVertices, quiver.Vertices);
        }

        [TestCase(1, 0)]
        [TestCase(2, 3)]
        [TestCase(3, 9)]
        public void GetTriangleQuiver_QuiverHasCorrectNumberOfArrows(int numRows, int expectedNumArrows)
        {
            var quiver = UsefulQuivers.GetTriangleQuiver(numRows);
            Assert.That(quiver.Arrows.Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuivers.GetTriangleQuiver(numRows, 0);
            Assert.That(quiver.Arrows.Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuivers.GetTriangleQuiver(numRows, -6);
            Assert.That(quiver.Arrows.Count, Is.EqualTo(expectedNumArrows));
        }

        [TestCase(-1)]
        [TestCase(-2)]
        [TestCase(-10)]
        public void GetVerticesInTriangleQuiverRow_ThrowsOnBadRowIndex(int rowIndex)
        {
            Assert.That(() => UsefulQuivers.GetVerticesInTriangleQuiverRow(rowIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        public void GetVerticesInTriangleQuiverRow_Works(int rowIndex, int firstVertex, params int[] expectedVertices)
        {
            var actualVertices = UsefulQuivers.GetVerticesInTriangleQuiverRow(rowIndex, firstVertex);
            Assert.That(actualVertices, Is.EqualTo(expectedVertices));
        }
        #endregion

        #region Square
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-10)]
        public void GetSquareQuiver_ThrowsOnBadNumberOfRows(int numRows)
        {
            Assert.That(() => UsefulQuivers.GetSquareQuiver(numRows), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(1, 0)]
        [TestCase(1, 1)]
        [TestCase(1, 10)]
        [TestCase(1, -1)]
        [TestCase(1, -10)]
        [TestCase(2, 0)]
        [TestCase(2, 1)]
        [TestCase(2, 10)]
        [TestCase(2, -1)]
        [TestCase(2, -10)]
        [TestCase(3, 0)]
        [TestCase(3, 1)]
        [TestCase(3, 10)]
        [TestCase(3, -1)]
        [TestCase(3, -10)]
        [TestCase(6, 0)]
        [TestCase(6, 1)]
        [TestCase(6, 10)]
        [TestCase(6, -1)]
        [TestCase(6, -10)]
        public void GetSquareQuiver_QuiverHasCorrectVertices(int numRows, int firstVertex)
        {
            int expectedNumVertices = numRows * numRows;
            var expectedVertices = Enumerable.Range(firstVertex, expectedNumVertices);
            var quiver = UsefulQuivers.GetSquareQuiver(numRows, firstVertex);
            CollectionAssert.AreEquivalent(expectedVertices, quiver.Vertices);
        }

        [TestCase(1, 0)]
        [TestCase(2, 4)]
        [TestCase(3, 12)] // 3*2 horizontal arrows and 2*3 vertical arrows
        public void GetSquareQuiver_QuiverHasCorrectNumberOfArrows(int numRows, int expectedNumArrows)
        {
            var quiver = UsefulQuivers.GetSquareQuiver(numRows);
            Assert.That(quiver.Arrows.Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuivers.GetSquareQuiver(numRows, 0);
            Assert.That(quiver.Arrows.Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuivers.GetSquareQuiver(numRows, -6);
            Assert.That(quiver.Arrows.Count, Is.EqualTo(expectedNumArrows));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-10)]
        public void GetVerticesInSquareQuiverRow_ThrowsOnBadNumberOfRows(int numRows)
        {
            int rowIndex = 0;
            Assert.That(() => UsefulQuivers.GetVerticesInSquareQuiverRow(numRows, rowIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

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
        public void GetVerticesInSquareQuiverRow_ThrowsOnBadRowIndex(int numRows, int rowIndex)
        {
            Assert.That(() => UsefulQuivers.GetVerticesInSquareQuiverRow(numRows, rowIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        public void GetVerticesInSquareQuiverRow_Works(int numRows, int rowIndex, int firstVertex, params int[] expectedVertices)
        {
            var actualVertices = UsefulQuivers.GetVerticesInSquareQuiverRow(numRows, rowIndex, firstVertex);
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
        public void GetNumberOfLayersInCobwebQuiver_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            Assert.That(() => UsefulQuivers.GetNumberOfLayersInCobwebQuiver(numVerticesInCenterPolygon), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(3, 1)]
        [TestCase(5, 2)]
        [TestCase(7, 3)]
        [TestCase(9, 4)]
        public void GetNumberOfLayersInCobwebQuiver_Works(int numVerticesInCenterPolygon, int expectedNumLayers)
        {
            Assert.That(UsefulQuivers.GetNumberOfLayersInCobwebQuiver(numVerticesInCenterPolygon), Is.EqualTo(expectedNumLayers));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(-6)]
        [TestCase(2)]
        [TestCase(4)]
        [TestCase(6)]
        [TestCase(20)]
        public void GetCobwebQuiver_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            Assert.That(() => UsefulQuivers.GetCobwebQuiver(numVerticesInCenterPolygon), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(3, 0)]
        [TestCase(3, 1)]
        [TestCase(3, 10)]
        [TestCase(3, -1)]
        [TestCase(3, -10)]
        [TestCase(5, 0)]
        [TestCase(5, 1)]
        [TestCase(5, 10)]
        [TestCase(5, -1)]
        [TestCase(5, -10)]
        [TestCase(11, 0)]
        [TestCase(11, 1)]
        [TestCase(11, 10)]
        [TestCase(11, -1)]
        [TestCase(11, -10)]
        public void GetCobwebQuiver_QuiverHasCorrectVertices(int numVerticesInCenterPolygon, int firstVertex)
        {
            int expectedNumLayers = (numVerticesInCenterPolygon - 1) / 2; // One small layer and the rest full
            int expectedNumVerticesInFullLayer = 2 * numVerticesInCenterPolygon;
            int expectedNumVertices = (expectedNumLayers - 1) * expectedNumVerticesInFullLayer + numVerticesInCenterPolygon;
            var expectedVertices = Enumerable.Range(firstVertex, expectedNumVertices);
            var quiver = UsefulQuivers.GetCobwebQuiver(numVerticesInCenterPolygon, firstVertex);
            CollectionAssert.AreEquivalent(expectedVertices, quiver.Vertices);
        }

        [TestCase(3, 3)]
        [TestCase(5, 25)] // 5+10+10: 5 in the center polygon, 10 "vertical" arrows, and 10 horizontal in the outer layer
        public void GetCobwebQuiver_QuiverHasCorrectNumberOfArrows(int numVerticesInCenterPolygon, int expectedNumArrows)
        {
            var quiver = UsefulQuivers.GetCobwebQuiver(numVerticesInCenterPolygon);
            Assert.That(quiver.Arrows.Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuivers.GetCobwebQuiver(numVerticesInCenterPolygon, 0);
            Assert.That(quiver.Arrows.Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuivers.GetCobwebQuiver(numVerticesInCenterPolygon, -6);
            Assert.That(quiver.Arrows.Count, Is.EqualTo(expectedNumArrows));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(-6)]
        [TestCase(2)]
        [TestCase(4)]
        [TestCase(6)]
        [TestCase(20)]
        public void GetVerticesInCobwebQuiverLayer_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            int layerIndex = 0;
            Assert.That(() => UsefulQuivers.GetVerticesInCobwebQuiverLayer(numVerticesInCenterPolygon, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(-1)]
        [TestCase(-2)]
        [TestCase(-5)]
        [TestCase(Int32.MinValue)]
        public void GetVerticesInCobwebQuiverLayer_ThrowsOnBadLayerIndex(int layerIndex)
        {
            int numVerticesInCenterPolygon = 5;
            Assert.That(() => UsefulQuivers.GetVerticesInCobwebQuiverLayer(numVerticesInCenterPolygon, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        public void GetVerticesInCobwebQuiverLayer_Works(int numVerticesInCenterPolygon, int layerIndex, int firstVertex, params int[] expectedVertices)
        {
            var actualVertices = UsefulQuivers.GetVerticesInCobwebQuiverLayer(numVerticesInCenterPolygon, layerIndex, firstVertex);
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
        public void GetNumberOfLayersInOddFlowerQuiver_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            Assert.That(() => UsefulQuivers.GetNumberOfLayersInOddFlowerQuiver(numVerticesInCenterPolygon), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(3, 2)]
        [TestCase(5, 3)]
        [TestCase(7, 4)]
        [TestCase(9, 5)]
        public void GetNumberOfLayersInOddFlowerQuiver_Works(int numVerticesInCenterPolygon, int expectedNumLayers)
        {
            Assert.That(UsefulQuivers.GetNumberOfLayersInOddFlowerQuiver(numVerticesInCenterPolygon), Is.EqualTo(expectedNumLayers));
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
        public void GetOddFlowerQuiver_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            Assert.That(() => UsefulQuivers.GetOddFlowerQuiver(numVerticesInCenterPolygon), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(3, 0)]
        [TestCase(3, 1)]
        [TestCase(3, 10)]
        [TestCase(3, -1)]
        [TestCase(3, -10)]
        [TestCase(5, 0)]
        [TestCase(5, 1)]
        [TestCase(5, 10)]
        [TestCase(5, -1)]
        [TestCase(5, -10)]
        [TestCase(11, 0)]
        [TestCase(11, 1)]
        [TestCase(11, 10)]
        [TestCase(11, -1)]
        [TestCase(11, -10)]
        public void GetOddFlowerQuiver_QuiverHasCorrectVertices(int numVerticesInCenterPolygon, int firstVertex)
        {
            int expectedNumLayers = (numVerticesInCenterPolygon + 1) / 2; // One small layer, one outer layer of twice the usual size, and the rest normal full layer
            int expectedNumVerticesInNormalFullLayer = 2 * numVerticesInCenterPolygon;
            int expectedNumVerticesInOuterLayer = 4 * numVerticesInCenterPolygon;
            int expectedNumVertices = numVerticesInCenterPolygon + (expectedNumLayers - 2) * expectedNumVerticesInNormalFullLayer + expectedNumVerticesInOuterLayer;
            var expectedVertices = Enumerable.Range(firstVertex, expectedNumVertices);
            var quiver = UsefulQuivers.GetOddFlowerQuiver(numVerticesInCenterPolygon, firstVertex);
            CollectionAssert.AreEquivalent(expectedVertices, quiver.Vertices);
        }

        [TestCase(3, 21)] // 3+6+12: 3 in the center polygon, 6 "vertical" arrows, and 12 horizontal arrows in the outer layer
        [TestCase(5, 55)] // 25+10+20: 25 in the cobweb, 10 vertical arrows to/from the outer layer, and 20 horizontal arrows in the outer layer
        public void GetOddFlowerQuiver_QuiverHasCorrectNumberOfArrows(int numVerticesInCenterPolygon, int expectedNumArrows)
        {
            var quiver = UsefulQuivers.GetOddFlowerQuiver(numVerticesInCenterPolygon);
            Assert.That(quiver.Arrows.Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuivers.GetOddFlowerQuiver(numVerticesInCenterPolygon, 0);
            Assert.That(quiver.Arrows.Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuivers.GetOddFlowerQuiver(numVerticesInCenterPolygon, -6);
            Assert.That(quiver.Arrows.Count, Is.EqualTo(expectedNumArrows));
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
        public void GetVerticesInOddFlowerQuiverLayer_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            int layerIndex = 0;
            Assert.That(() => UsefulQuivers.GetVerticesInOddFlowerQuiverLayer(numVerticesInCenterPolygon, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        public void GetVerticesInOddFlowerQuiverLayer_ThrowsOnBadLayerIndex(int numVerticesInCenterPolygon, int layerIndex)
        {
            Assert.That(() => UsefulQuivers.GetVerticesInOddFlowerQuiverLayer(numVerticesInCenterPolygon, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        public void GetVerticesInOddFlowerQuiverLayer_Works(int numVerticesInCenterPolygon, int layerIndex, int firstVertex, params int[] expectedVertices)
        {
            var actualVertices = UsefulQuivers.GetVerticesInOddFlowerQuiverLayer(numVerticesInCenterPolygon, layerIndex, firstVertex);
            Assert.That(actualVertices, Is.EqualTo(expectedVertices));
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
        public void GetNumberOfLayersInEvenFlowerType1Quiver_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            Assert.That(() => UsefulQuivers.GetNumberOfLayersInEvenFlowerType1Quiver(numVerticesInCenterPolygon), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(4, 2)]
        [TestCase(6, 3)]
        [TestCase(8, 4)]
        [TestCase(10, 5)]
        public void GetNumberOfLayersInEvenFlowerType1Quiver_Works(int numVerticesInCenterPolygon, int expectedNumLayers)
        {
            Assert.That(UsefulQuivers.GetNumberOfLayersInEvenFlowerType1Quiver(numVerticesInCenterPolygon), Is.EqualTo(expectedNumLayers));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(-6)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(5)]
        [TestCase(19)]
        public void GetEvenFlowerType1Quiver_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            Assert.That(() => UsefulQuivers.GetEvenFlowerType1Quiver(numVerticesInCenterPolygon), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(4, 0)]
        [TestCase(4, 1)]
        [TestCase(4, 10)]
        [TestCase(4, -1)]
        [TestCase(4, -10)]
        [TestCase(6, 0)]
        [TestCase(6, 1)]
        [TestCase(6, 10)]
        [TestCase(6, -1)]
        [TestCase(6, -10)]
        [TestCase(12, 0)]
        [TestCase(12, 1)]
        [TestCase(12, 10)]
        [TestCase(12, -1)]
        [TestCase(12, -10)]
        public void GetEvenFlowerType1Quiver_QuiverHasCorrectVertices(int numVerticesInCenterPolygon, int firstVertex)
        {
            int expectedNumLayers = numVerticesInCenterPolygon / 2; // One small layer, one outer layer of three times the small layer size, and the rest normal full layer
            int expectedNumVerticesInNormalFullLayer = 2 * numVerticesInCenterPolygon;
            int expectedNumVerticesInOuterLayer = 3 * numVerticesInCenterPolygon;
            int expectedNumVertices = numVerticesInCenterPolygon + (expectedNumLayers - 2) * expectedNumVerticesInNormalFullLayer + expectedNumVerticesInOuterLayer;
            var expectedVertices = Enumerable.Range(firstVertex, expectedNumVertices);
            var quiver = UsefulQuivers.GetEvenFlowerType1Quiver(numVerticesInCenterPolygon, firstVertex);
            CollectionAssert.AreEquivalent(expectedVertices, quiver.Vertices);
        }

        [TestCase(4, 24)] // 4+8+12: 4 in the center polygon, 8 "vertical" arrows, and 12 horizontal arrows in the outer layer                   (24 = 3*8  here, and 21 = 3*7  for OddFlower(3))
        [TestCase(6, 60)] // 30+12+18: 30 in the cobweb, 12 vertical arrows to/from the outer layer, and 18 horizontal arrows in the outer layer (60 = 5*12 here, and 55 = 5*11 for OddFlower(5))
        public void GetEvenFlowerType1Quiver_QuiverHasCorrectNumberOfArrows(int numVerticesInCenterPolygon, int expectedNumArrows)
        {
            var quiver = UsefulQuivers.GetEvenFlowerType1Quiver(numVerticesInCenterPolygon);
            Assert.That(quiver.Arrows.Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuivers.GetEvenFlowerType1Quiver(numVerticesInCenterPolygon, 0);
            Assert.That(quiver.Arrows.Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuivers.GetEvenFlowerType1Quiver(numVerticesInCenterPolygon, -6);
            Assert.That(quiver.Arrows.Count, Is.EqualTo(expectedNumArrows));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(-6)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(5)]
        [TestCase(19)]
        public void GetVerticesInEvenFlowerType1QuiverLayer_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            int layerIndex = 0;
            Assert.That(() => UsefulQuivers.GetVerticesInEvenFlowerType1QuiverLayer(numVerticesInCenterPolygon, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(4, Int32.MinValue)]
        [TestCase(4, -2)]
        [TestCase(4, -1)]
        [TestCase(4, 2)]
        [TestCase(4, 3)]
        [TestCase(4, Int32.MaxValue)]
        [TestCase(6, Int32.MinValue)]
        [TestCase(6, -2)]
        [TestCase(6, -1)]
        [TestCase(6, 3)]
        [TestCase(6, 4)]
        [TestCase(6, Int32.MaxValue)]
        public void GetVerticesInEvenFlowerType1QuiverLayer_ThrowsOnBadLayerIndex(int numVerticesInCenterPolygon, int layerIndex)
        {
            Assert.That(() => UsefulQuivers.GetVerticesInEvenFlowerType1QuiverLayer(numVerticesInCenterPolygon, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        public void GetVerticesInEvenFlowerType1QuiverLayer_Works(int numVerticesInCenterPolygon, int layerIndex, int firstVertex, params int[] expectedVertices)
        {
            var actualVertices = UsefulQuivers.GetVerticesInEvenFlowerType1QuiverLayer(numVerticesInCenterPolygon, layerIndex, firstVertex);
            Assert.That(actualVertices, Is.EqualTo(expectedVertices));
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
        public void GetNumberOfLayersInPointedFlowerQuiver_ThrowsOnBadNumberOfPeriods(int numPeriods)
        {
            Assert.That(() => UsefulQuivers.GetNumberOfLayersInPointedFlowerQuiver(numPeriods), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(3, 2)]
        [TestCase(5, 3)]
        [TestCase(7, 4)]
        [TestCase(9, 5)]
        public void GetNumberOfLayersInPointedFlowerQuiver_Works(int numPeriods, int expectedNumLayers)
        {
            Assert.That(UsefulQuivers.GetNumberOfLayersInPointedFlowerQuiver(numPeriods), Is.EqualTo(expectedNumLayers));
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
        public void GetPointedFlowerQuiver_ThrowsOnBadNumberOfPeriods(int numPeriods)
        {
            Assert.That(() => UsefulQuivers.GetPointedFlowerQuiver(numPeriods), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(3, 0)]
        [TestCase(3, 1)]
        [TestCase(3, 10)]
        [TestCase(3, -1)]
        [TestCase(3, -10)]
        [TestCase(5, 0)]
        [TestCase(5, 1)]
        [TestCase(5, 10)]
        [TestCase(5, -1)]
        [TestCase(5, -10)]
        [TestCase(11, 0)]
        [TestCase(11, 1)]
        [TestCase(11, 10)]
        [TestCase(11, -1)]
        [TestCase(11, -10)]
        public void GetPointedFlowerQuiver_QuiverHasCorrectVertices(int numPeriods, int firstVertex)
        {
            int expectedNumVertices = numPeriods * numPeriods + 1;
            var expectedVertices = Enumerable.Range(firstVertex, expectedNumVertices);
            var quiver = UsefulQuivers.GetPointedFlowerQuiver(numPeriods, firstVertex);
            CollectionAssert.AreEquivalent(expectedVertices, quiver.Vertices);
        }

        [TestCase(3, 15)] // 6+9 = 12:  6 vertical arrows and 9 in the second layer.
        [TestCase(5, 45)] // 10+10+10+15 = 45: 10 vertical arrows between layer 1,2; 10 horizontal arrows in layer 2;
                          // 10 vertical arrows between layer 2,3; 15 arrows in the outer layer.
        public void GetPointedFlowerQuiver_QuiverHasCorrectNumberOfArrows(int numPeriods, int expectedNumArrows)
        {
            var quiver = UsefulQuivers.GetPointedFlowerQuiver(numPeriods);
            Assert.That(quiver.Arrows.Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuivers.GetPointedFlowerQuiver(numPeriods, 0);
            Assert.That(quiver.Arrows.Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuivers.GetPointedFlowerQuiver(numPeriods, -6);
            Assert.That(quiver.Arrows.Count, Is.EqualTo(expectedNumArrows));
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
        public void GetVerticesInPointedFlowerQuiverLayer_ThrowsOnBadNumberOfPeriods(int numPeriods)
        {
            int layerIndex = 0;
            Assert.That(() => UsefulQuivers.GetVerticesInPointedFlowerQuiverLayer(numPeriods, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        public void GetVerticesInPointedFlowerQuiverLayer_ThrowsOnBadLayerIndex(int numPeriods, int layerIndex)
        {
            Assert.That(() => UsefulQuivers.GetVerticesInPointedFlowerQuiverLayer(numPeriods, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        public void GetVerticesInPointedFlowerQuiverLayer_Works(int numPeriods, int layerIndex, int firstVertex, params int[] expectedVertices)
        {
            var actualVertices = UsefulQuivers.GetVerticesInPointedFlowerQuiverLayer(numPeriods, layerIndex, firstVertex);
            Assert.That(actualVertices, Is.EqualTo(expectedVertices));
        }
        #endregion

        #region Generalized cobweb
        [TestCase(2)]
        [TestCase(1)]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(Int32.MinValue)]
        public void GetGeneralizedCobwebQuiver_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            var numsLayers = new int[] { 1, 2, 5 };
            foreach (var numLayers in numsLayers)
            {
                Assert.That(() => UsefulQuivers.GetGeneralizedCobwebQuiver(numVerticesInCenterPolygon, numLayers), Throws.InstanceOf<ArgumentOutOfRangeException>());
            }
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(Int32.MinValue)]
        public void GetGeneralizedCobwebQuiver_ThrowsOnBadNumberOfLayers(int numLayers)
        {
            var numsVerticesInCenterPolygon = new int[] { 3, 4, 5, 6, 10 };
            foreach (var numVerticesInCenterPolygon in numsVerticesInCenterPolygon)
            {
                Assert.That(() => UsefulQuivers.GetGeneralizedCobwebQuiver(numVerticesInCenterPolygon, numLayers), Throws.InstanceOf<ArgumentOutOfRangeException>());
            }
        }

        [TestCase(3, 1, 0)]
        [TestCase(3, 1, 1)]
        [TestCase(3, 1, -10)]
        [TestCase(3, 1, 10)]
        [TestCase(3, 2, 0)]
        [TestCase(3, 2, 1)]
        [TestCase(3, 2, -10)]
        [TestCase(3, 2, 10)]
        [TestCase(3, 5, 0)]
        [TestCase(3, 5, 1)]
        [TestCase(3, 5, -10)]
        [TestCase(3, 5, 10)]
        [TestCase(4, 1, 0)]
        [TestCase(4, 1, 1)]
        [TestCase(4, 1, -10)]
        [TestCase(4, 1, 10)]
        [TestCase(4, 2, 0)]
        [TestCase(4, 2, 1)]
        [TestCase(4, 2, -10)]
        [TestCase(4, 2, 10)]
        [TestCase(4, 5, 0)]
        [TestCase(4, 5, 1)]
        [TestCase(4, 5, -10)]
        [TestCase(4, 5, 10)]
        [TestCase(8, 1, 0)]
        [TestCase(8, 1, 1)]
        [TestCase(8, 1, -10)]
        [TestCase(8, 1, 10)]
        [TestCase(8, 2, 0)]
        [TestCase(8, 2, 1)]
        [TestCase(8, 2, -10)]
        [TestCase(8, 2, 10)]
        [TestCase(8, 5, 0)]
        [TestCase(8, 5, 1)]
        [TestCase(8, 5, -10)]
        [TestCase(8, 5, 10)]
        public void GetGeneralizedCobwebQuiver_QuiverHasCorrectVertices(int numVerticesInCenterPolygon, int numLayers, int firstVertex)
        {
            int expectedNumVerticesInFullLayer = 2 * numVerticesInCenterPolygon;
            int expectedNumVertices = (numLayers - 1) * expectedNumVerticesInFullLayer + numVerticesInCenterPolygon;
            var expectedVertices = Enumerable.Range(firstVertex, expectedNumVertices);
            var quiver = UsefulQuivers.GetGeneralizedCobwebQuiver(numVerticesInCenterPolygon, numLayers, firstVertex);
            CollectionAssert.AreEquivalent(expectedVertices, quiver.Vertices);
        }

        [TestCase(3, 1, 3)]   // Just a cycle
        [TestCase(4, 1, 4)]   // Just a cycle
        [TestCase(10, 1, 10)] // Just a cycle
        [TestCase(3, 2, 15)]  // 3+6+6 = 15:   3 in the center polygon, 6 vertical arrows, and 10 in the outer layer
        [TestCase(5, 2, 25)]  // 5+10+10 = 25: 5 in the center polygon, 10 vertical arrows, and 10 in the outer layer
        public void GetGeneralizedCobwebQuiver_QuiverHasCorrectNumberOfArrows(int numVerticesInCenterPolygon, int numLayers, int expectedNumArrows)
        {
            var quiver = UsefulQuivers.GetGeneralizedCobwebQuiver(numVerticesInCenterPolygon, numLayers);
            Assert.That(quiver.Arrows.Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuivers.GetGeneralizedCobwebQuiver(numVerticesInCenterPolygon, numLayers, 0);
            Assert.That(quiver.Arrows.Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuivers.GetGeneralizedCobwebQuiver(numVerticesInCenterPolygon, numLayers, -6);
            Assert.That(quiver.Arrows.Count, Is.EqualTo(expectedNumArrows));
        }

        [TestCase(3)]
        [TestCase(5)]
        [TestCase(7)]
        [TestCase(9)]
        public void GetGeneralizedCobwebQuiver_CoincidesWithCobwebQuiver(int numVerticesInCenterPolygon)
        {
            int numLayers = (numVerticesInCenterPolygon - 1) / 2;
            var actualQuiver = UsefulQuivers.GetGeneralizedCobwebQuiver(numVerticesInCenterPolygon, numLayers);
            var expectedQuiver = UsefulQuivers.GetCobwebQuiver(numVerticesInCenterPolygon);
            Assert.That(actualQuiver, Is.EqualTo(expectedQuiver));
        }

        [TestCase(2)]
        [TestCase(1)]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(Int32.MinValue)]
        public void GetVerticesInGeneralizedCobwebQuiverLayer_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            int numLayers = 2;
            int layerIndex = 0;
            Assert.That(() => UsefulQuivers.GetVerticesInGeneralizedCobwebQuiverLayer(numVerticesInCenterPolygon, numLayers, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(Int32.MinValue)]
        public void GetVerticesInGeneralizedCobwebQuiverLayer_ThrowsOnBadNumberOfLayers(int numLayers)
        {
            int numVerticesInCenterPolygon = 5;
            int layerIndex = 0;
            Assert.That(() => UsefulQuivers.GetVerticesInGeneralizedCobwebQuiverLayer(numVerticesInCenterPolygon, numLayers, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(-1)]
        [TestCase(-2)]
        [TestCase(-5)]
        [TestCase(Int32.MinValue)]
        public void GetVerticesInGeneralizedCobwebQuiverLayer_ThrowsOnBadLayerIndex(int layerIndex)
        {
            int numVerticesInCenterPolygon = 5;
            int numLayers = 2;
            Assert.That(() => UsefulQuivers.GetVerticesInGeneralizedCobwebQuiverLayer(numVerticesInCenterPolygon, numLayers, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(3, 1, 0, 0, new int[] { 0, 1, 2 })]
        [TestCase(3, 1, 0, 1, new int[] { 1, 2, 3 })]
        [TestCase(3, 1, 0, 10, new int[] { 10, 11, 12 })]
        [TestCase(3, 1, 0, -1, new int[] { -1, 0, 1 })]
        [TestCase(3, 1, 0, -10, new int[] { -10, -9, -8 })]
        [TestCase(3, 2, 0, 0, new int[] { 0, 1, 2 })]
        [TestCase(3, 2, 0, 1, new int[] { 1, 2, 3 })]
        [TestCase(3, 2, 0, 10, new int[] { 10, 11, 12 })]
        [TestCase(3, 2, 0, -1, new int[] { -1, 0, 1 })]
        [TestCase(3, 2, 0, -10, new int[] { -10, -9, -8 })]
        [TestCase(3, 2, 1, 0, new int[] { 3, 4, 5, 6, 7, 8 })]
        [TestCase(3, 2, 1, 1, new int[] { 4, 5, 6, 7, 8, 9 })]
        [TestCase(3, 2, 1, 10, new int[] { 13, 14, 15, 16, 17, 18 })]
        [TestCase(3, 2, 1, -1, new int[] { 2, 3, 4, 5, 6, 7 })]
        [TestCase(3, 2, 1, -10, new int[] { -7, -6, -5, -4, -3, -2 })]
        [TestCase(7, 2, 0, 0, new int[] { 0, 1, 2, 3, 4, 5, 6 })]
        [TestCase(7, 2, 0, 1, new int[] { 1, 2, 3, 4, 5, 6, 7 })]
        [TestCase(7, 2, 0, 10, new int[] { 10, 11, 12, 13, 14, 15, 16 })]
        [TestCase(7, 2, 0, -1, new int[] { -1, 0, 1, 2, 3, 4, 5 })]
        [TestCase(7, 2, 0, -10, new int[] { -10, -9, -8, -7, -6, -5, -4 })]
        [TestCase(7, 2, 1, 0, new int[] { 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 })]
        [TestCase(7, 2, 1, 1, new int[] { 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21 })]
        [TestCase(7, 2, 1, 10, new int[] { 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 })]
        [TestCase(7, 2, 1, -1, new int[] { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 })]
        [TestCase(7, 2, 1, -10, new int[] { -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
        [TestCase(7, 3, 0, 0, new int[] { 0, 1, 2, 3, 4, 5, 6 })]
        [TestCase(7, 3, 0, 1, new int[] { 1, 2, 3, 4, 5, 6, 7 })]
        [TestCase(7, 3, 0, 10, new int[] { 10, 11, 12, 13, 14, 15, 16 })]
        [TestCase(7, 3, 0, -1, new int[] { -1, 0, 1, 2, 3, 4, 5 })]
        [TestCase(7, 3, 0, -10, new int[] { -10, -9, -8, -7, -6, -5, -4 })]
        [TestCase(7, 3, 1, 0, new int[] { 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 })]
        [TestCase(7, 3, 1, 1, new int[] { 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21 })]
        [TestCase(7, 3, 1, 10, new int[] { 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 })]
        [TestCase(7, 3, 1, -1, new int[] { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 })]
        [TestCase(7, 3, 1, -10, new int[] { -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
        [TestCase(7, 3, 2, 0, new int[] { 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34 })]
        [TestCase(7, 3, 2, 1, new int[] { 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35 })]
        [TestCase(7, 3, 2, 10, new int[] { 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44 })]
        [TestCase(7, 3, 2, -1, new int[] { 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33 })]
        [TestCase(7, 3, 2, -10, new int[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 })]
        public void GetVerticesInGeneralizedCobwebQuiverLayer_Works(int numVerticesInCenterPolygon, int numLayers, int layerIndex, int firstVertex, params int[] expectedVertices)
        {
            var actualVertices = UsefulQuivers.GetVerticesInGeneralizedCobwebQuiverLayer(numVerticesInCenterPolygon, numLayers, layerIndex, firstVertex);
            Assert.That(actualVertices, Is.EqualTo(expectedVertices));
        }
        #endregion
    }
}
