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
    public class UsefulQuiversInPlaneTestFixture
    {
        const int DefaultRadius = 100; // For cycles, triangles, cobwebs, and flowers
        const int DefaultWidth = 100; // For squares

        #region Cycle
        [TestCase(2)]
        [TestCase(1)]
        [TestCase(0)]
        [TestCase(-1)]
        public void GetCycleQuiverInPlane_ThrowsOnBadNumberOfVertices(int numVertices)
        {
            Assert.That(() => UsefulQuiversInPlane.GetCycleQuiverInPlane(numVertices, DefaultRadius), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        public void GetCycleQuiverInPlane_QuiverHasCorrectVertices(int numVertices, int firstVertex)
        {
            var expectedVertices = Enumerable.Range(firstVertex, numVertices);

            var quiver = UsefulQuiversInPlane.GetCycleQuiverInPlane(numVertices, DefaultRadius, firstVertex);
            CollectionAssert.AreEquivalent(expectedVertices, quiver.Vertices);
        }

        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(10)]
        public void GetCycleQuiverInPlane_QuiverHasCorrectNumberOfArrows(int numVertices)
        {
            int expectedNumArrows = numVertices;

            var quiver = UsefulQuiversInPlane.GetCycleQuiverInPlane(numVertices, DefaultRadius);
            Assert.That(quiver.GetArrows().Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuiversInPlane.GetCycleQuiverInPlane(numVertices, DefaultRadius, 0);
            Assert.That(quiver.GetArrows().Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuiversInPlane.GetCycleQuiverInPlane(numVertices, DefaultRadius, -6);
            Assert.That(quiver.GetArrows().Count, Is.EqualTo(expectedNumArrows));
        }
        #endregion

        #region Triangle
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-10)]
        public void GetTriangleQuiver_ThrowsOnBadNumberOfRows(int numRows)
        {
            Assert.That(() => UsefulQuiversInPlane.GetTriangleQuiverInPlane(numRows, DefaultRadius), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        public void GetTriangleQuiverInPlane_QuiverHasCorrectVertices(int numRows, int firstVertex)
        {
            var expectedVertices = Enumerable.Range(firstVertex, Utility.TriangularNumber(numRows));
            var quiver = UsefulQuiversInPlane.GetTriangleQuiverInPlane(numRows, DefaultRadius, firstVertex);
            CollectionAssert.AreEquivalent(expectedVertices, quiver.Vertices);
        }

        [TestCase(1, 0)]
        [TestCase(2, 3)]
        [TestCase(3, 9)]
        public void GetTriangleQuiverInPlane_QuiverHasCorrectNumberOfArrows(int numRows, int expectedNumArrows)
        {
            var quiver = UsefulQuiversInPlane.GetTriangleQuiverInPlane(numRows, DefaultRadius);
            Assert.That(quiver.GetArrows().Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuiversInPlane.GetTriangleQuiverInPlane(numRows, DefaultRadius, 0);
            Assert.That(quiver.GetArrows().Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuiversInPlane.GetTriangleQuiverInPlane(numRows, DefaultRadius, -6);
            Assert.That(quiver.GetArrows().Count, Is.EqualTo(expectedNumArrows));
        }

        [TestCase(-1)]
        [TestCase(-2)]
        [TestCase(-10)]
        public void GetVerticesInTriangleQuiverInPlaneRow_ThrowsOnBadRowIndex(int rowIndex)
        {
            Assert.That(() => UsefulQuiversInPlane.GetVerticesInTriangleQuiverInPlaneRow(rowIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        public void GetVerticesInTriangleQuiverInPlaneRow_Works(int rowIndex, int firstVertex, params int[] expectedVertices)
        {
            var actualVertices = UsefulQuiversInPlane.GetVerticesInTriangleQuiverInPlaneRow(rowIndex, firstVertex);
            Assert.That(actualVertices, Is.EqualTo(expectedVertices));
        }
        #endregion

        #region Square
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-10)]
        public void GetSquareQuiverInPlane_ThrowsOnBadNumberOfSquaresInRow(int numRows)
        {
            const int Width = 50;
            Assert.That(() => UsefulQuiversInPlane.GetSquareQuiverInPlane(numRows, Width), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        public void GetSquareQuiverInPlane_QuiverHasCorrectVertices(int numRows, int firstVertex)
        {
            int expectedNumVertices = numRows * numRows;
            var expectedVertices = Enumerable.Range(firstVertex, expectedNumVertices);
            var quiver = UsefulQuiversInPlane.GetSquareQuiverInPlane(numRows, DefaultWidth, firstVertex);
            CollectionAssert.AreEquivalent(expectedVertices, quiver.Vertices);
        }

        [TestCase(1, 0)]
        [TestCase(2, 4)]
        [TestCase(3, 12)] // 3*2 horizontal arrows and 2*3 vertical arrows
        public void GetSquareQuiverInPlane_QuiverHasCorrectNumberOfArrows(int numRows, int expectedNumArrows)
        {
            var quiver = UsefulQuiversInPlane.GetSquareQuiverInPlane(numRows, DefaultWidth);
            Assert.That(quiver.GetArrows().Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuiversInPlane.GetSquareQuiverInPlane(numRows, DefaultWidth, 0);
            Assert.That(quiver.GetArrows().Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuiversInPlane.GetSquareQuiverInPlane(numRows, DefaultWidth, -6);
            Assert.That(quiver.GetArrows().Count, Is.EqualTo(expectedNumArrows));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-10)]
        public void GetVerticesInSquareQuiverInPlaneRow_ThrowsOnBadNumberOfSquaresInRow(int numRows)
        {
            int rowIndex = 0;
            Assert.That(() => UsefulQuiversInPlane.GetVerticesInSquareQuiverInPlaneRow(numRows, rowIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        public void GetVerticesInSquareQuiverInPlaneRow_ThrowsOnRowIndex(int numRows, int rowIndex)
        {
            Assert.That(() => UsefulQuiversInPlane.GetVerticesInSquareQuiverInPlaneRow(numRows, rowIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        public void GetVerticesInSquareQuiverInPlaneRow_Works(int numRows, int rowIndex, int firstVertex, params int[] expectedVertices)
        {
            var actualVertices = UsefulQuiversInPlane.GetVerticesInSquareQuiverInPlaneRow(numRows, rowIndex, firstVertex);
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
        public void GetNumberOfLayersInCobwebQuiverInPlane_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            Assert.That(() => UsefulQuiversInPlane.GetNumberOfLayersInCobwebQuiverInPlane(numVerticesInCenterPolygon), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(3, 1)]
        [TestCase(5, 2)]
        [TestCase(7, 3)]
        [TestCase(9, 4)]
        public void GetNumberOfLayersInCobwebQuiverInPlane_Works(int numVerticesInCenterPolygon, int expectedNumLayers)
        {
            Assert.That(UsefulQuiversInPlane.GetNumberOfLayersInCobwebQuiverInPlane(numVerticesInCenterPolygon), Is.EqualTo(expectedNumLayers));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(-6)]
        [TestCase(2)]
        [TestCase(4)]
        [TestCase(6)]
        [TestCase(20)]
        public void GetCobwebQuiverInPlane_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            Assert.That(() => UsefulQuiversInPlane.GetCobwebQuiverInPlane(numVerticesInCenterPolygon, DefaultRadius), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        public void GetCobwebQuiverInPlane_QuiverHasCorrectVertices(int numVerticesInCenterPolygon, int firstVertex)
        {
            int expectedNumLayers = (numVerticesInCenterPolygon - 1) / 2; // One small layer and the rest full
            int expectedNumVerticesInFullLayer = 2 * numVerticesInCenterPolygon;
            int expectedNumVertices = (expectedNumLayers - 1) * expectedNumVerticesInFullLayer + numVerticesInCenterPolygon;
            var expectedVertices = Enumerable.Range(firstVertex, expectedNumVertices);
            var quiver = UsefulQuiversInPlane.GetCobwebQuiverInPlane(numVerticesInCenterPolygon, DefaultRadius, firstVertex);
            CollectionAssert.AreEquivalent(expectedVertices, quiver.Vertices);
        }

        [TestCase(3, 3)]
        [TestCase(5, 25)] // 5+10+10: 5 in the center polygon, 10 "vertical" arrows, and 10 horizontal in the outer layer
        public void GetCobwebQuiverInPlane_QuiverHasCorrectNumberOfArrows(int numVerticesInCenterPolygon, int expectedNumArrows)
        {
            var quiver = UsefulQuiversInPlane.GetCobwebQuiverInPlane(numVerticesInCenterPolygon, DefaultRadius);
            Assert.That(quiver.GetArrows().Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuiversInPlane.GetCobwebQuiverInPlane(numVerticesInCenterPolygon, DefaultRadius, 0);
            Assert.That(quiver.GetArrows().Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuiversInPlane.GetCobwebQuiverInPlane(numVerticesInCenterPolygon, DefaultRadius, -6);
            Assert.That(quiver.GetArrows().Count, Is.EqualTo(expectedNumArrows));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(-6)]
        [TestCase(2)]
        [TestCase(4)]
        [TestCase(6)]
        [TestCase(20)]
        public void GetVerticesInCobwebQuiverInPlaneLayer_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            int layerIndex = 0;
            Assert.That(() => UsefulQuiversInPlane.GetVerticesInCobwebQuiverInPlaneLayer(numVerticesInCenterPolygon, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        public void GetVerticesInCobwebQuiverInPlaneLayer_Works(int numVerticesInCenterPolygon, int layerIndex, int firstVertex, params int[] expectedVertices)
        {
            var actualVertices = UsefulQuiversInPlane.GetVerticesInCobwebQuiverInPlaneLayer(numVerticesInCenterPolygon, layerIndex, firstVertex);
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
        public void GetNumberOfLayersInOddFlowerQuiverInPlane_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            Assert.That(() => UsefulQuiversInPlane.GetNumberOfLayersInOddFlowerQuiverInPlane(numVerticesInCenterPolygon), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(3, 2)]
        [TestCase(5, 3)]
        [TestCase(7, 4)]
        [TestCase(9, 5)]
        public void GetNumberOfLayersInOddFlowerQuiverInPlane_Works(int numVerticesInCenterPolygon, int expectedNumLayers)
        {
            Assert.That(UsefulQuiversInPlane.GetNumberOfLayersInOddFlowerQuiverInPlane(numVerticesInCenterPolygon), Is.EqualTo(expectedNumLayers));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(-6)]
        [TestCase(2)]
        [TestCase(4)]
        [TestCase(6)]
        [TestCase(20)]
        public void GetOddFlowerQuiverInPlane_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            Assert.That(() => UsefulQuiversInPlane.GetOddFlowerQuiverInPlane(numVerticesInCenterPolygon, DefaultRadius), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        public void GetOddFlowerQuiverInPlane_QuiverHasCorrectVertices(int numVerticesInCenterPolygon, int firstVertex)
        {
            int expectedNumLayers = (numVerticesInCenterPolygon + 1) / 2; // One small layer, one outer layer of twice the usual size, and the rest normal full layer
            int expectedNumVerticesInNormalFullLayer = 2 * numVerticesInCenterPolygon;
            int expectedNumVerticesInOuterLayer = 4 * numVerticesInCenterPolygon;
            int expectedNumVertices = numVerticesInCenterPolygon + (expectedNumLayers - 2) * expectedNumVerticesInNormalFullLayer + expectedNumVerticesInOuterLayer;
            var expectedVertices = Enumerable.Range(firstVertex, expectedNumVertices);
            var quiver = UsefulQuiversInPlane.GetOddFlowerQuiverInPlane(numVerticesInCenterPolygon, DefaultRadius, firstVertex);
            CollectionAssert.AreEquivalent(expectedVertices, quiver.Vertices);
        }

        [TestCase(3, 21)] // 3+6+12: 3 in the center polygon, 6 "vertical" arrows, and 12 horizontal arrows in the outer layer
        [TestCase(5, 55)] // 25+10+20: 25 in the cobweb, 10 vertical arrows to/from the outer layer, and 20 horizontal arrows in the outer layer
        public void GetOddFlowerQuiverInPlane_QuiverHasCorrectNumberOfArrows(int numVerticesInCenterPolygon, int expectedNumArrows)
        {
            var quiver = UsefulQuiversInPlane.GetOddFlowerQuiverInPlane(numVerticesInCenterPolygon, DefaultRadius);
            Assert.That(quiver.GetArrows().Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuiversInPlane.GetOddFlowerQuiverInPlane(numVerticesInCenterPolygon, DefaultRadius, 0);
            Assert.That(quiver.GetArrows().Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuiversInPlane.GetOddFlowerQuiverInPlane(numVerticesInCenterPolygon, DefaultRadius, -6);
            Assert.That(quiver.GetArrows().Count, Is.EqualTo(expectedNumArrows));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(-6)]
        [TestCase(2)]
        [TestCase(4)]
        [TestCase(6)]
        [TestCase(20)]
        public void GetVerticesInOddFlowerQuiverInPlaneLayer_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            int layerIndex = 0;
            Assert.That(() => UsefulQuiversInPlane.GetVerticesInOddFlowerQuiverInPlaneLayer(numVerticesInCenterPolygon, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        public void GetVerticesInOddFlowerQuiverInPlaneLayer_Works(int numVerticesInCenterPolygon, int layerIndex, int firstVertex, params int[] expectedVertices)
        {
            var actualVertices = UsefulQuiversInPlane.GetVerticesInOddFlowerQuiverInPlaneLayer(numVerticesInCenterPolygon, layerIndex, firstVertex);
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
        public void GetPeriodsOfOddFlowerQuiverInPlane_ThrowsArgumentOutOfRangeException_OnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon, int firstVertex)
        {
            Assert.That(() => UsefulQuiversInPlane.GetPeriodsOfOddFlowerQuiverInPlane(numVerticesInCenterPolygon, firstVertex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        static IEnumerable<TestCaseData> GetPeriodsOfOddFlowerQuiverInPlane_Works_TestCaseSource()
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

        [TestCaseSource(nameof(GetPeriodsOfOddFlowerQuiverInPlane_Works_TestCaseSource))]
        public void GetPeriodsOfOddFlowerQuiverInPlane_Works(int numVerticesInCenterPolygon, int firstVertex, IEnumerable<IEnumerable<int>> expectedPeriods)
        {
            var actualPeriods = UsefulQuiversInPlane.GetPeriodsOfOddFlowerQuiverInPlane(numVerticesInCenterPolygon, firstVertex);
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
        public void GetNumberOfLayersInEvenFlowerType1QuiverInPlane_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            Assert.That(() => UsefulQuiversInPlane.GetNumberOfLayersInEvenFlowerType1QuiverInPlane(numVerticesInCenterPolygon), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(4, 2)]
        [TestCase(6, 3)]
        [TestCase(8, 4)]
        [TestCase(10, 5)]
        public void GetNumberOfLayersInEvenFlowerType1QuiverInPlane_Works(int numVerticesInCenterPolygon, int expectedNumLayers)
        {
            Assert.That(UsefulQuiversInPlane.GetNumberOfLayersInEvenFlowerType1QuiverInPlane(numVerticesInCenterPolygon), Is.EqualTo(expectedNumLayers));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(-6)]
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(5)]
        [TestCase(19)]
        public void GetEvenFlowerType1QuiverInPlane_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            Assert.That(() => UsefulQuiversInPlane.GetEvenFlowerType1QuiverInPlane(numVerticesInCenterPolygon, DefaultRadius), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        public void GetEvenFlowerType1QuiverInPlane_QuiverHasCorrectVertices(int numVerticesInCenterPolygon, int firstVertex)
        {
            int expectedNumLayers = numVerticesInCenterPolygon / 2; // One small layer, one outer layer of three times the center layer size, and the rest normal full layer
            int expectedNumVerticesInNormalFullLayer = 2 * numVerticesInCenterPolygon;
            int expectedNumVerticesInOuterLayer = 3 * numVerticesInCenterPolygon;
            int expectedNumVertices = numVerticesInCenterPolygon + (expectedNumLayers - 2) * expectedNumVerticesInNormalFullLayer + expectedNumVerticesInOuterLayer;
            var expectedVertices = Enumerable.Range(firstVertex, expectedNumVertices);
            var quiver = UsefulQuiversInPlane.GetEvenFlowerType1QuiverInPlane(numVerticesInCenterPolygon, DefaultRadius, firstVertex);
            CollectionAssert.AreEquivalent(expectedVertices, quiver.Vertices);
        }

        [TestCase(4, 24)] // 4+8+12: 4 in the center polygon, 8 "vertical" arrows, and 12 horizontal arrows in the outer layer                   (24 = 3*8  here, and 21 = 3*7  for Flower(3))
        [TestCase(6, 60)] // 30+12+18: 30 in the cobweb, 12 vertical arrows to/from the outer layer, and 18 horizontal arrows in the outer layer (60 = 5*12 here, abd 55 = 5*11 for Flower(5))
        public void GetEvenFlowerType1QuiverInPlane_QuiverHasCorrectNumberOfArrows(int numVerticesInCenterPolygon, int expectedNumArrows)
        {
            var quiver = UsefulQuiversInPlane.GetEvenFlowerType1QuiverInPlane(numVerticesInCenterPolygon, DefaultRadius);
            Assert.That(quiver.GetArrows().Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuiversInPlane.GetEvenFlowerType1QuiverInPlane(numVerticesInCenterPolygon, DefaultRadius, 0);
            Assert.That(quiver.GetArrows().Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuiversInPlane.GetEvenFlowerType1QuiverInPlane(numVerticesInCenterPolygon, DefaultRadius, -6);
            Assert.That(quiver.GetArrows().Count, Is.EqualTo(expectedNumArrows));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(-6)]
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(5)]
        [TestCase(19)]
        public void GetVerticesInEvenFlowerType1QuiverInPlaneLayer_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            int layerIndex = 0;
            Assert.That(() => UsefulQuiversInPlane.GetVerticesInEvenFlowerType1QuiverInPlaneLayer(numVerticesInCenterPolygon, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        public void GetVerticesInEvenFlowerType1QuiverInPlaneLayer_Works(int numVerticesInCenterPolygon, int layerIndex, int firstVertex, params int[] expectedVertices)
        {
            var actualVertices = UsefulQuiversInPlane.GetVerticesInEvenFlowerType1QuiverInPlaneLayer(numVerticesInCenterPolygon, layerIndex, firstVertex);
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
        public void GetPeriodsOfEvenFlowerType1QuiverInPlane_ThrowsArgumentOutOfRangeException_OnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon, int firstVertex)
        {
            Assert.That(() => UsefulQuiversInPlane.GetPeriodsOfEvenFlowerType1QuiverInPlane(numVerticesInCenterPolygon, firstVertex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        static IEnumerable<TestCaseData> GetPeriodsOfEvenFlowerType1QuiverInPlane_Works_TestCaseSource()
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

        [TestCaseSource(nameof(GetPeriodsOfEvenFlowerType1QuiverInPlane_Works_TestCaseSource))]
        public void GetPeriodsOfEvenFlowerType1QuiverInPlane_Works(int numVerticesInCenterPolygon, int firstVertex, IEnumerable<IEnumerable<int>> expectedPeriods)
        {
            var actualPeriods = UsefulQuiversInPlane.GetPeriodsOfEvenFlowerType1QuiverInPlane(numVerticesInCenterPolygon, firstVertex);
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
        public void GetNumberOfLayersInEvenFlowerType2QuiverInPlane_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            Assert.That(() => UsefulQuiversInPlane.GetNumberOfLayersInEvenFlowerType2QuiverInPlane(numVerticesInCenterPolygon), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(4, 2)]
        [TestCase(6, 3)]
        [TestCase(8, 4)]
        [TestCase(10, 5)]
        public void GetNumberOfLayersInEvenFlowerType2QuiverInPlane_Works(int numVerticesInCenterPolygon, int expectedNumLayers)
        {
            Assert.That(UsefulQuiversInPlane.GetNumberOfLayersInEvenFlowerType2QuiverInPlane(numVerticesInCenterPolygon), Is.EqualTo(expectedNumLayers));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(-6)]
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(5)]
        [TestCase(19)]
        public void GetEvenFlowerType2QuiverInPlane_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            Assert.That(() => UsefulQuiversInPlane.GetEvenFlowerType2QuiverInPlane(numVerticesInCenterPolygon, DefaultRadius), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        public void GetEvenFlowerType2QuiverInPlane_QuiverHasCorrectVertices(int numVerticesInCenterPolygon, int firstVertex)
        {
            int expectedNumLayers = numVerticesInCenterPolygon / 2; // One small layer, one outer layer of three times the center layer size, and the rest normal full layer
            int expectedNumVerticesInNormalFullLayer = 2 * numVerticesInCenterPolygon;
            int expectedNumVerticesInOuterLayer = 3 * numVerticesInCenterPolygon;
            int expectedNumVertices = numVerticesInCenterPolygon + (expectedNumLayers - 2) * expectedNumVerticesInNormalFullLayer + expectedNumVerticesInOuterLayer;
            var expectedVertices = Enumerable.Range(firstVertex, expectedNumVertices);
            var quiver = UsefulQuiversInPlane.GetEvenFlowerType2QuiverInPlane(numVerticesInCenterPolygon, DefaultRadius, firstVertex);
            CollectionAssert.AreEquivalent(expectedVertices, quiver.Vertices);
        }

        [TestCase(4, 24)] // 4+8+12: 4 in the center polygon, 8 "vertical" arrows, and 12 horizontal arrows in the outer layer                   (24 = 3*8  here, and 21 = 3*7  for Flower(3))
        [TestCase(6, 60)] // 30+12+18: 30 in the cobweb, 12 vertical arrows to/from the outer layer, and 18 horizontal arrows in the outer layer (60 = 5*12 here, abd 55 = 5*11 for Flower(5))
        public void GetEvenFlowerType2QuiverInPlane_QuiverHasCorrectNumberOfArrows(int numVerticesInCenterPolygon, int expectedNumArrows)
        {
            var quiver = UsefulQuiversInPlane.GetEvenFlowerType2QuiverInPlane(numVerticesInCenterPolygon, DefaultRadius);
            Assert.That(quiver.GetArrows().Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuiversInPlane.GetEvenFlowerType2QuiverInPlane(numVerticesInCenterPolygon, DefaultRadius, 0);
            Assert.That(quiver.GetArrows().Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuiversInPlane.GetEvenFlowerType2QuiverInPlane(numVerticesInCenterPolygon, DefaultRadius, -6);
            Assert.That(quiver.GetArrows().Count, Is.EqualTo(expectedNumArrows));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(-6)]
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(5)]
        [TestCase(19)]
        public void GetVerticesInEvenFlowerType2QuiverInPlaneLayer_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            int layerIndex = 0;
            Assert.That(() => UsefulQuiversInPlane.GetVerticesInEvenFlowerType2QuiverInPlaneLayer(numVerticesInCenterPolygon, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        public void GetVerticesInEvenFlowerType2QuiverInPlaneLayer_Works(int numVerticesInCenterPolygon, int layerIndex, int firstVertex, params int[] expectedVertices)
        {
            var actualVertices = UsefulQuiversInPlane.GetVerticesInEvenFlowerType2QuiverInPlaneLayer(numVerticesInCenterPolygon, layerIndex, firstVertex);
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
        public void GetPeriodsOfEvenFlowerType2QuiverInPlane_ThrowsArgumentOutOfRangeException_OnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon, int firstVertex)
        {
            Assert.That(() => UsefulQuiversInPlane.GetPeriodsOfEvenFlowerType2QuiverInPlane(numVerticesInCenterPolygon, firstVertex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        static IEnumerable<TestCaseData> GetPeriodsOfEvenFlowerType2QuiverInPlane_Works_TestCaseSource()
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

        [TestCaseSource(nameof(GetPeriodsOfEvenFlowerType2QuiverInPlane_Works_TestCaseSource))]
        public void GetPeriodsOfEvenFlowerType2QuiverInPlane_Works(int numVerticesInCenterPolygon, int firstVertex, IEnumerable<IEnumerable<int>> expectedPeriods)
        {
            var actualPeriods = UsefulQuiversInPlane.GetPeriodsOfEvenFlowerType2QuiverInPlane(numVerticesInCenterPolygon, firstVertex);
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
        public void GetNumberOfLayersInPointedFlowerQuiverInPlane_ThrowsOnBadNumberOfPeriods(int numPeriods)
        {
            Assert.That(() => UsefulQuiversInPlane.GetNumberOfLayersInPointedFlowerQuiverInPlane(numPeriods), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(3, 2)]
        [TestCase(5, 3)]
        [TestCase(7, 4)]
        [TestCase(9, 5)]
        public void GetNumberOfLayersInPointedFlowerQuiverInPlane_Works(int numPeriods, int expectedNumLayers)
        {
            Assert.That(UsefulQuiversInPlane.GetNumberOfLayersInPointedFlowerQuiverInPlane(numPeriods), Is.EqualTo(expectedNumLayers));
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
        public void GetPointedFlowerQuiverInPlane_ThrowsOnBadNumberOfPeriods(int numPeriods)
        {
            Assert.That(() => UsefulQuiversInPlane.GetPointedFlowerQuiverInPlane(numPeriods, DefaultRadius), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        public void GetPointedFlowerQuiverInPlane_QuiverHasCorrectVertices(int numPeriods, int firstVertex)
        {
            int expectedNumVertices = numPeriods * numPeriods + 1;
            var expectedVertices = Enumerable.Range(firstVertex, expectedNumVertices);
            var quiverInPlane = UsefulQuiversInPlane.GetPointedFlowerQuiverInPlane(numPeriods, DefaultRadius, firstVertex);
            CollectionAssert.AreEquivalent(expectedVertices, quiverInPlane.Vertices);
        }

        [TestCase(3, 15)] // 6+9 = 12:  6 vertical arrows and 9 in the second layer.
        [TestCase(5, 45)] // 10+10+10+15 = 45: 10 vertical arrows between layer 1,2; 10 horizontal arrows in layer 2;
                          // 10 vertical arrows between layer 2,3; 15 arrows in the outer layer.
        public void GetPointedFlowerQuiverInPlane_QuiverHasCorrectNumberOfArrows(int numPeriods, int expectedNumArrows)
        {
            var quiverInPlane = UsefulQuiversInPlane.GetPointedFlowerQuiverInPlane(numPeriods, DefaultRadius);
            Assert.That(quiverInPlane.GetArrows().Count, Is.EqualTo(expectedNumArrows));
            quiverInPlane = UsefulQuiversInPlane.GetPointedFlowerQuiverInPlane(numPeriods, DefaultRadius, 0);
            Assert.That(quiverInPlane.GetArrows().Count, Is.EqualTo(expectedNumArrows));
            quiverInPlane = UsefulQuiversInPlane.GetPointedFlowerQuiverInPlane(numPeriods, DefaultRadius, -6);
            Assert.That(quiverInPlane.GetArrows().Count, Is.EqualTo(expectedNumArrows));
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
        public void GetVerticesInPointedFlowerQuiverInPlaneLayer_ThrowsOnBadNumberOfPeriods(int numPeriods)
        {
            int layerIndex = 0;
            Assert.That(() => UsefulQuiversInPlane.GetVerticesInPointedFlowerQuiverInPlaneLayer(numPeriods, DefaultRadius, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        public void GetVerticesInPointedFlowerQuiverInPlaneLayer_ThrowsOnBadLayerIndex(int numPeriods, int layerIndex)
        {
            Assert.That(() => UsefulQuiversInPlane.GetVerticesInPointedFlowerQuiverInPlaneLayer(numPeriods, DefaultRadius, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        public void GetVerticesInPointedFlowerQuiverInPlaneLayer_Works(int numPeriods, int layerIndex, int firstVertex, params int[] expectedVertices)
        {
            var actualVertices = UsefulQuiversInPlane.GetVerticesInPointedFlowerQuiverInPlaneLayer(numPeriods, layerIndex, firstVertex);
            Assert.That(actualVertices, Is.EqualTo(expectedVertices));
        }

        // Regression test
        [TestCase(3, -1)]
        [TestCase(3, 0)]
        [TestCase(3, 1)]
        [TestCase(5, -1)]
        [TestCase(5, 0)]
        [TestCase(5, 1)]
        [TestCase(7, -1)]
        [TestCase(7, 0)]
        [TestCase(7, 1)]
        public void PointedFlowerQuiver_HasFirstVertexAtTheOrigin(int numPeriods, int firstVertex)
        {
            var quiverInPlane = UsefulQuiversInPlane.GetPointedFlowerQuiverInPlane(numPeriods, DefaultRadius, firstVertex);
            var firstVertexPosition = quiverInPlane.GetVertexPosition(firstVertex);
            Assert.That(firstVertexPosition, Is.EqualTo(Point.Origin));
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
        public void GetPeriodsOfPointedFlowerQuiverInPlane_ThrowsArgumentOutOfRangeException_OnBadNumberOfVerticesInCenterPolygon(int numPeriods, int firstVertex)
        {
            Assert.That(() => UsefulQuiversInPlane.GetPeriodsOfPointedFlowerQuiverInPlaneWithoutFixedPoint(numPeriods, firstVertex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        static IEnumerable<TestCaseData> GetPeriodsOfPointedFlowerQuiverInPlane_Works_TestCaseSource()
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

        [TestCaseSource(nameof(GetPeriodsOfPointedFlowerQuiverInPlane_Works_TestCaseSource))]
        public void GetPeriodsOfPointedFlowerQuiverInPlane_Works(int numPeriods, int firstVertex, IEnumerable<IEnumerable<int>> expectedPeriods)
        {
            var actualPeriods = UsefulQuiversInPlane.GetPeriodsOfPointedFlowerQuiverInPlaneWithoutFixedPoint(numPeriods, firstVertex);
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
        public void GetGeneralizedCobwebQuiverInPlane_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            var numsLayers = new int[] { 1, 2, 5 };
            foreach (var numLayers in numsLayers)
            {
                Assert.That(() => UsefulQuiversInPlane.GetGeneralizedCobwebQuiverInPlane(numVerticesInCenterPolygon, numLayers, DefaultRadius), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
                Assert.That(() => UsefulQuiversInPlane.GetGeneralizedCobwebQuiverInPlane(numVerticesInCenterPolygon, numLayers, DefaultRadius), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        public void GetGeneralizedCobwebQuiverInPlane_QuiverHasCorrectVertices(int numVerticesInCenterPolygon, int numLayers, int firstVertex)
        {
            int expectedNumVerticesInFullLayer = 2 * numVerticesInCenterPolygon;
            int expectedNumVertices = (numLayers - 1) * expectedNumVerticesInFullLayer + numVerticesInCenterPolygon;
            var expectedVertices = Enumerable.Range(firstVertex, expectedNumVertices);
            var quiver = UsefulQuiversInPlane.GetGeneralizedCobwebQuiverInPlane(numVerticesInCenterPolygon, numLayers, DefaultRadius, firstVertex);
            CollectionAssert.AreEquivalent(expectedVertices, quiver.Vertices);
        }

        [TestCase(3, 1, 3)]   // Just a cycle
        [TestCase(4, 1, 4)]   // Just a cycle
        [TestCase(10, 1, 10)] // Just a cycle
        [TestCase(3, 2, 15)]  // 3+6+6 = 15:   3 in the center polygon, 6 vertical arrows, and 10 in the outer layer
        [TestCase(5, 2, 25)]  // 5+10+10 = 25: 5 in the center polygon, 10 vertical arrows, and 10 in the outer layer
        public void GetGeneralizedCobwebQuiverInPlane_QuiverHasCorrectNumberOfArrows(int numVerticesInCenterPolygon, int numLayers, int expectedNumArrows)
        {
            var quiver = UsefulQuiversInPlane.GetGeneralizedCobwebQuiverInPlane(numVerticesInCenterPolygon, numLayers, DefaultRadius).GetUnderlyingQuiver();
            Assert.That(quiver.Arrows.Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuiversInPlane.GetGeneralizedCobwebQuiverInPlane(numVerticesInCenterPolygon, numLayers, DefaultRadius, 0).GetUnderlyingQuiver();
            Assert.That(quiver.Arrows.Count, Is.EqualTo(expectedNumArrows));
            quiver = UsefulQuiversInPlane.GetGeneralizedCobwebQuiverInPlane(numVerticesInCenterPolygon, numLayers, DefaultRadius, -6).GetUnderlyingQuiver();
            Assert.That(quiver.Arrows.Count, Is.EqualTo(expectedNumArrows));
        }

        [TestCase(3)]
        [TestCase(5)]
        [TestCase(7)]
        [TestCase(9)]
        public void GetGeneralizedCobwebQuiverInPlane_CoincidesWithCobwebQuiverInPlane(int numVerticesInCenterPolygon)
        {
            int numLayers = (numVerticesInCenterPolygon - 1) / 2;
            var actualQuiver = UsefulQuiversInPlane.GetGeneralizedCobwebQuiverInPlane(numVerticesInCenterPolygon, numLayers, DefaultRadius);
            var expectedQuiver = UsefulQuiversInPlane.GetCobwebQuiverInPlane(numVerticesInCenterPolygon, DefaultRadius);
            Assert.That(actualQuiver, Is.EqualTo(expectedQuiver));
        }

        [TestCase(2)]
        [TestCase(1)]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(Int32.MinValue)]
        public void GetVerticesInGeneralizedCobwebQuiverInPlaneLayer_ThrowsOnBadNumberOfVerticesInCenterPolygon(int numVerticesInCenterPolygon)
        {
            int numLayers = 2;
            int layerIndex = 0;
            Assert.That(() => UsefulQuiversInPlane.GetVerticesInGeneralizedCobwebQuiverInPlaneLayer(numVerticesInCenterPolygon, numLayers, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        [TestCase(Int32.MinValue)]
        public void GetVerticesInGeneralizedCobwebQuiverInPlaneLayer_ThrowsOnBadNumberOfLayers(int numLayers)
        {
            int numVerticesInCenterPolygon = 5;
            int layerIndex = 0;
            Assert.That(() => UsefulQuiversInPlane.GetVerticesInGeneralizedCobwebQuiverInPlaneLayer(numVerticesInCenterPolygon, numLayers, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(-1)]
        [TestCase(-2)]
        [TestCase(-5)]
        [TestCase(Int32.MinValue)]
        public void GetVerticesInGeneralizedCobwebQuiverInPlaneLayer_ThrowsOnBadLayerIndex(int layerIndex)
        {
            int numVerticesInCenterPolygon = 5;
            int numLayers = 2;
            Assert.That(() => UsefulQuiversInPlane.GetVerticesInGeneralizedCobwebQuiverInPlaneLayer(numVerticesInCenterPolygon, numLayers, layerIndex), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        public void GetVerticesInGeneralizedCobwebQuiverInPlaneLayer_Works(int numVerticesInCenterPolygon, int numLayers, int layerIndex, int firstVertex, params int[] expectedVertices)
        {
            var actualVertices = UsefulQuiversInPlane.GetVerticesInGeneralizedCobwebQuiverInPlaneLayer(numVerticesInCenterPolygon, numLayers, layerIndex, firstVertex);
            Assert.That(actualVertices, Is.EqualTo(expectedVertices));
        }
        #endregion
    }
}
