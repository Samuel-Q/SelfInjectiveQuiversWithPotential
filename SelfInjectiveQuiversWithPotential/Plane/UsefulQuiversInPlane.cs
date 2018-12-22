using System;
using System.Linq;
using System.Collections.Generic;

namespace SelfInjectiveQuiversWithPotential.Plane
{
    /// <summary>
    /// This class is used to construct some useful predefined quivers in the plane.
    /// </summary>
    public static class UsefulQuiversInPlane
    {
        private const int DefaultFirstVertex = UsefulQuivers.DefaultFirstVertex;

        #region Cycle
        public static bool CycleParameterIsValid(int numVertices) => UsefulQuivers.CycleParameterIsValid(numVertices);

        public static string CycleParameterValidityDescription { get => UsefulQuivers.CycleParameterValidityDescription; }

        public static QuiverInPlane<int> GetCycleQuiverInPlane(int numVertices, int radius, int firstVertex = DefaultFirstVertex)
        {
            if (!CycleParameterIsValid(numVertices)) throw new ArgumentOutOfRangeException(nameof(numVertices));

            var quiver = UsefulQuivers.GetCycleQuiver(numVertices, firstVertex);
            var angle = 2 * Math.PI / numVertices;
            var vertexPositions = quiver.Vertices.ToDictionary(
                k => k,
                k => new Point((int)(radius * Math.Cos(k * angle)), (int)(radius * Math.Sin(k * angle))));

            return new QuiverInPlane<int>(quiver, vertexPositions);
        }
        #endregion

        #region Triangle
        public static bool TriangleParameterIsValid(int numRows) => UsefulQuivers.TriangleParameterIsValid(numRows);

        public static string TriangleParameterValidityDescription { get => UsefulQuivers.TriangleParameterValidityDescription; }

        public static QuiverInPlane<int> GetTriangleQuiverInPlane(int numRows, int radius, int firstVertex = DefaultFirstVertex)
        {
            if (!TriangleParameterIsValid(numRows)) throw new ArgumentOutOfRangeException(nameof(numRows));

            var quiver = UsefulQuivers.GetTriangleQuiver(numRows, firstVertex);

            if (numRows == 1) // Avoid division by zero
            {
                var vertexPositions1 = new Dictionary<int, Point>() { { firstVertex, Point.Origin } };
                return new QuiverInPlane<int>(quiver, vertexPositions1);
            }

            // Relationship between various lengths in an equilateral triangle (everything immediate from 30-60-90 reasoning):
            //   If the side length is 2, then
            //     * the height of the triangle is sqrt(3)
            //     * the radius of the triangle (distance from center to vertex) is 2/sqrt(3)
            //   So if the radius is 1, then the height is 3/2
            double triangleWidth = radius * Math.Sqrt(3);
            double triangleHeight = radius * 3.0 / 2;

            double horizontalStepLength = triangleWidth / (numRows - 1);
            double verticalStepLength = triangleHeight / (numRows - 1);

            var vertexPositions = new Dictionary<int, Point>();
            int vertex = firstVertex;
            double posY = radius;
            for (int rowIndex = 0; rowIndex < numRows; rowIndex++)
            {
                double posX = -rowIndex * horizontalStepLength / 2;
                int numVerticesInRow = rowIndex + 1;
                for (int indexInRow = 0; indexInRow < numVerticesInRow; indexInRow++)
                {
                    vertexPositions[vertex++] = new Point(posX, posY);
                    posX += horizontalStepLength;
                }

                posY -= verticalStepLength;
            }

            return new QuiverInPlane<int>(quiver, vertexPositions);
        }

        public static IEnumerable<int> GetVerticesInTriangleQuiverInPlaneRow(int rowIndex, int firstVertex = DefaultFirstVertex)
            => UsefulQuivers.GetVerticesInTriangleQuiverRow(rowIndex, firstVertex);
        #endregion

        #region Square
        public static bool SquareParameterIsValid(int numRows) => UsefulQuivers.SquareParameterIsValid(numRows);

        public static string SquareParameterValidityDescription { get => UsefulQuivers.SquareParameterValidityDescription; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numRows">The number of rows of vertices.</param>
        /// <param name="width"></param>
        /// <param name="firstVertex"></param>
        /// <returns></returns>
        public static QuiverInPlane<int> GetSquareQuiverInPlane(int numRows, int width, int firstVertex = DefaultFirstVertex)
        {
            if (!SquareParameterIsValid(numRows)) throw new ArgumentOutOfRangeException(nameof(numRows));

            var quiver = UsefulQuivers.GetSquareQuiver(numRows, firstVertex);

            if (numRows == 1) // Avoid division by zero
            {
                var vertexPositions1 = new Dictionary<int, Point>() { { firstVertex, Point.Origin } };
                return new QuiverInPlane<int>(quiver, vertexPositions1);
            }

            int numVerticesInRow = numRows;
            double stepLength = (double)width / (numRows-1);

            var vertexPositions = new Dictionary<int, Point>();
            int vertex = firstVertex;
            double startPosX = -(numVerticesInRow-1) * stepLength / 2;
            double posY = (numRows-1) * stepLength / 2;
            for (int rowIndex = 0; rowIndex < numRows; rowIndex++)
            {
                double posX = startPosX;
                for (int indexInRow = 0; indexInRow < numVerticesInRow; indexInRow++)
                {
                    vertexPositions[vertex++] = new Point(posX, posY);
                    posX += stepLength;
                }

                posY -= stepLength;
            }

            return new QuiverInPlane<int>(quiver, vertexPositions);
        }

        public static IEnumerable<int> GetVerticesInSquareQuiverInPlaneRow(int numRows, int rowIndex, int firstVertex = DefaultFirstVertex)
            => UsefulQuivers.GetVerticesInSquareQuiverRow(numRows, rowIndex, firstVertex);
        #endregion

        #region Cobweb
        public static bool CobwebParameterIsValid(int numVerticesInCenterPolygon) => UsefulQuivers.CobwebParameterIsValid(numVerticesInCenterPolygon);

        public static string CobwebParameterValidityDescription { get => UsefulQuivers.CobwebParameterValidityDescription; }

        public static int GetNumberOfLayersInCobwebQuiverInPlane(int numVerticesInCenterPolygon)
            => UsefulQuivers.GetNumberOfLayersInCobwebQuiver(numVerticesInCenterPolygon);

        public static QuiverInPlane<int> GetCobwebQuiverInPlane(int numVerticesInCenterPolygon, int innermostRadius, int firstVertex = DefaultFirstVertex)
        {
            if (!CobwebParameterIsValid(numVerticesInCenterPolygon)) throw new ArgumentOutOfRangeException(nameof(numVerticesInCenterPolygon));

            var quiver = UsefulQuivers.GetCobwebQuiver(numVerticesInCenterPolygon, firstVertex);

            int numLayers = UsefulQuivers.GetNumberOfLayersInCobwebQuiver(numVerticesInCenterPolygon);

            var vertexPositions = new Dictionary<int, Point>();
            for (int layerIndex = 0; layerIndex < numLayers; layerIndex++)
            {
                var radius = (layerIndex + 1) * innermostRadius;
                var layer = UsefulQuivers.GetVerticesInCobwebQuiverLayer(numVerticesInCenterPolygon, layerIndex, firstVertex);
                double angle = 2 * Math.PI / layer.Count();
                foreach (var (vertex, indexInLayer) in layer.EnumerateWithIndex())
                {
                    vertexPositions[vertex] = layerIndex == 0 ?
                        new Point(radius * Math.Cos(indexInLayer * angle), radius * Math.Sin(indexInLayer * angle)) :
                        new Point(radius * Math.Cos((indexInLayer+0.5) * angle), radius * Math.Sin((indexInLayer+0.5) * angle));
                }
            }

            return new QuiverInPlane<int>(quiver, vertexPositions);
        }

        public static IEnumerable<int> GetVerticesInCobwebQuiverInPlaneLayer(int numVerticesInCenterPolygon, int layerIndex, int firstVertex = DefaultFirstVertex)
            => UsefulQuivers.GetVerticesInCobwebQuiverLayer(numVerticesInCenterPolygon, layerIndex, firstVertex);
        #endregion

        #region Odd flower
        public static bool OddFlowerParameterIsValid(int numVerticesInCenterPolygon) => UsefulQuivers.OddFlowerParameterIsValid(numVerticesInCenterPolygon);

        public static string OddFlowerParameterValidityDescription { get => UsefulQuivers.OddFlowerParameterValidityDescription; }

        public static int GetNumberOfLayersInOddFlowerQuiverInPlane(int numVerticesInCenterPolygon)
            => UsefulQuivers.GetNumberOfLayersInOddFlowerQuiver(numVerticesInCenterPolygon);

        public static QuiverInPlane<int> GetOddFlowerQuiverInPlane(int numVerticesInCenterPolygon, int innermostRadius, int firstVertex = DefaultFirstVertex)
        {
            if (!OddFlowerParameterIsValid(numVerticesInCenterPolygon)) throw new ArgumentOutOfRangeException(nameof(numVerticesInCenterPolygon));

            var quiver = UsefulQuivers.GetOddFlowerQuiver(numVerticesInCenterPolygon, firstVertex);

            int numLayers = UsefulQuivers.GetNumberOfLayersInOddFlowerQuiver(numVerticesInCenterPolygon);

            var vertexPositions = new Dictionary<int, Point>();
            for (int layerIndex = 0; layerIndex < numLayers; layerIndex++)
            {
                var radius = (layerIndex + 1) * innermostRadius;
                var layer = UsefulQuivers.GetVerticesInOddFlowerQuiverLayer(numVerticesInCenterPolygon, layerIndex, firstVertex);
                double angle = 2 * Math.PI / layer.Count();
                foreach (var (vertex, indexInLayer) in layer.EnumerateWithIndex())
                {
                    vertexPositions[vertex] =
                        layerIndex == 0 ? new Point(radius * Math.Cos(indexInLayer * angle), radius * Math.Sin(indexInLayer * angle)) :
                        layerIndex < numLayers - 1 ? new Point(radius * Math.Cos((indexInLayer + 0.5) * angle), radius * Math.Sin((indexInLayer + 0.5) * angle)) :
                        new Point(radius * Math.Cos((indexInLayer + 1) * angle), radius * Math.Sin((indexInLayer + 1) * angle));
                }
            }

            return new QuiverInPlane<int>(quiver, vertexPositions);
        }

        public static IEnumerable<int> GetVerticesInOddFlowerQuiverInPlaneLayer(int numVerticesInCenterPolygon, int layerIndex, int firstVertex = DefaultFirstVertex)
            => UsefulQuivers.GetVerticesInOddFlowerQuiverLayer(numVerticesInCenterPolygon, layerIndex, firstVertex);
        #endregion

        #region Even flower, type 1
        public static bool EvenFlowerType1ParameterIsValid(int numVerticesInCenterPolygon) => UsefulQuivers.EvenFlowerType1ParameterIsValid(numVerticesInCenterPolygon);

        public static string EvenFlowerType1ParameterValidityDescription { get => UsefulQuivers.EvenFlowerType1ParameterValidityDescription; }

        public static int GetNumberOfLayersInEvenFlowerType1QuiverInPlane(int numVerticesInCenterPolygon)
            => UsefulQuivers.GetNumberOfLayersInEvenFlowerType1Quiver(numVerticesInCenterPolygon);

        public static QuiverInPlane<int> GetEvenFlowerType1QuiverInPlane(int numVerticesInCenterPolygon, int innermostRadius, int firstVertex = DefaultFirstVertex)
        {
            if (!EvenFlowerType1ParameterIsValid(numVerticesInCenterPolygon)) throw new ArgumentOutOfRangeException(nameof(numVerticesInCenterPolygon));

            var quiver = UsefulQuivers.GetEvenFlowerType1Quiver(numVerticesInCenterPolygon, firstVertex);

            int numLayers = UsefulQuivers.GetNumberOfLayersInEvenFlowerType1Quiver(numVerticesInCenterPolygon);

            var vertexPositions = new Dictionary<int, Point>();
            for (int layerIndex = 0; layerIndex < numLayers; layerIndex++)
            {
                var radius = (layerIndex + 1) * innermostRadius;
                var layer = UsefulQuivers.GetVerticesInEvenFlowerType1QuiverLayer(numVerticesInCenterPolygon, layerIndex, firstVertex);
                double angle = 2 * Math.PI / layer.Count();
                foreach (var (vertex, indexInLayer) in layer.EnumerateWithIndex())
                {
                    vertexPositions[vertex] =
                        layerIndex == 0 ? new Point(radius * Math.Cos(indexInLayer * angle), radius * Math.Sin(indexInLayer * angle)) :
                        layerIndex < numLayers - 1 ? new Point(radius * Math.Cos((indexInLayer + 0.5) * angle), radius * Math.Sin((indexInLayer + 0.5) * angle)) :
                        new Point(radius * Math.Cos((indexInLayer + 0.5) * angle), radius * Math.Sin((indexInLayer + 0.5) * angle));
                }
            }

            return new QuiverInPlane<int>(quiver, vertexPositions);
        }

        public static IEnumerable<int> GetVerticesInEvenFlowerType1QuiverInPlaneLayer(int numVerticesInCenterPolygon, int layerIndex, int firstVertex = DefaultFirstVertex)
            => UsefulQuivers.GetVerticesInEvenFlowerType1QuiverLayer(numVerticesInCenterPolygon, layerIndex, firstVertex);
        #endregion

        #region Even flower, type 2
        public static bool EvenFlowerType2ParameterIsValid(int numVerticesInCenterPolygon) => UsefulQuivers.EvenFlowerType2ParameterIsValid(numVerticesInCenterPolygon);

        public static string EvenFlowerType2ParameterValidityDescription { get => UsefulQuivers.EvenFlowerType2ParameterValidityDescription; }

        public static int GetNumberOfLayersInEvenFlowerType2QuiverInPlane(int numVerticesInCenterPolygon)
            => UsefulQuivers.GetNumberOfLayersInEvenFlowerType2Quiver(numVerticesInCenterPolygon);

        public static QuiverInPlane<int> GetEvenFlowerType2QuiverInPlane(int numVerticesInCenterPolygon, int innermostRadius, int firstVertex = DefaultFirstVertex)
        {
            if (!EvenFlowerType2ParameterIsValid(numVerticesInCenterPolygon)) throw new ArgumentOutOfRangeException(nameof(numVerticesInCenterPolygon));

            var quiver = UsefulQuivers.GetEvenFlowerType2Quiver(numVerticesInCenterPolygon, firstVertex);

            int numLayers = UsefulQuivers.GetNumberOfLayersInEvenFlowerType2Quiver(numVerticesInCenterPolygon);

            var vertexPositions = new Dictionary<int, Point>();
            for (int layerIndex = 0; layerIndex < numLayers; layerIndex++)
            {
                var radius = (layerIndex + 1) * innermostRadius;
                var layer = UsefulQuivers.GetVerticesInEvenFlowerType2QuiverLayer(numVerticesInCenterPolygon, layerIndex, firstVertex);
                double angle = 2 * Math.PI / layer.Count();
                foreach (var (vertex, indexInLayer) in layer.EnumerateWithIndex())
                {
                    double angleMultiplier =
                        layerIndex == 0 ? indexInLayer :
                        layerIndex < numLayers - 1 ? (indexInLayer + 0.5) :
                        indexInLayer + 1;

                    vertexPositions[vertex] = new Point(radius * Math.Cos(angleMultiplier * angle), radius * Math.Sin(angleMultiplier * angle));
                }
            }

            return new QuiverInPlane<int>(quiver, vertexPositions);
        }

        public static IEnumerable<int> GetVerticesInEvenFlowerType2QuiverInPlaneLayer(int numVerticesInCenterPolygon, int layerIndex, int firstVertex = DefaultFirstVertex)
            => UsefulQuivers.GetVerticesInEvenFlowerType2QuiverLayer(numVerticesInCenterPolygon, layerIndex, firstVertex);
        #endregion

        #region Pointed flower
        public static bool PointedFlowerParameterIsValid(int numPeriods) => UsefulQuivers.PointedFlowerParameterIsValid(numPeriods);

        public static string PointedFlowerParameterValidityDescription { get => UsefulQuivers.PointedFlowerParameterValidityDescription; }

        public static int GetNumberOfLayersInPointedFlowerQuiverInPlane(int numPeriods)
            => UsefulQuivers.GetNumberOfLayersInPointedFlowerQuiver(numPeriods);

        public static QuiverInPlane<int> GetPointedFlowerQuiverInPlane(int numPeriods, int innermostRadius, int firstVertex = DefaultFirstVertex)
        {
            if (!PointedFlowerParameterIsValid(numPeriods)) throw new ArgumentOutOfRangeException(nameof(numPeriods));

            var quiver = UsefulQuivers.GetPointedFlowerQuiver(numPeriods, firstVertex);

            int numLayers = UsefulQuivers.GetNumberOfLayersInPointedFlowerQuiver(numPeriods);

            var vertexPositions = new Dictionary<int, Point>();
            for (int layerIndex = 0; layerIndex < numLayers; layerIndex++)
            {
                var radius = (layerIndex) * innermostRadius;
                var layer = UsefulQuivers.GetVerticesInPointedFlowerQuiverLayer(numPeriods, layerIndex, firstVertex);
                double angle = 2 * Math.PI / layer.Count();
                foreach (var (vertex, indexInLayer) in layer.EnumerateWithIndex())
                {
                    vertexPositions[vertex] =
                        layerIndex == 0 ? new Point(radius * Math.Cos(indexInLayer * angle), radius * Math.Sin(indexInLayer * angle)) :
                        layerIndex < numLayers - 1 ? new Point(radius * Math.Cos((indexInLayer + 0.5) * angle), radius * Math.Sin((indexInLayer + 0.5) * angle)) :
                        new Point(radius * Math.Cos((indexInLayer + 0.5) * angle), radius * Math.Sin((indexInLayer + 0.5) * angle));
                }
            }

            return new QuiverInPlane<int>(quiver, vertexPositions);
        }

        public static IEnumerable<int> GetVerticesInPointedFlowerQuiverInPlaneLayer(int numPeriods, int layerIndex, int firstVertex = DefaultFirstVertex)
            => UsefulQuivers.GetVerticesInPointedFlowerQuiverLayer(numPeriods, layerIndex, firstVertex);
        #endregion

        #region Generalized cobweb
        public static bool GeneralizedCobwebParameterIsValid(int numVerticesInCenterPolygon, int numLayers)
            => UsefulQuivers.GeneralizedCobwebParameterIsValid(numVerticesInCenterPolygon, numLayers);

        public static string GeneralizedCobwebParameterValidityDescription => UsefulQuivers.GeneralizedCobwebParameterValidityDescription;

        public static QuiverInPlane<int> GetGeneralizedCobwebQuiverInPlane(int numVerticesInCenterPolygon, int numLayers, int innermostRadius, int firstVertex = DefaultFirstVertex)
        {
            if (!GeneralizedCobwebParameterIsValid(numVerticesInCenterPolygon, numLayers))
            {
                throw new ArgumentOutOfRangeException();
            }

            var quiver = UsefulQuivers.GetGeneralizedCobwebQuiver(numVerticesInCenterPolygon, numLayers, firstVertex);

            var vertexPositions = new Dictionary<int, Point>();
            for (int layerIndex = 0; layerIndex < numLayers; layerIndex++)
            {
                var radius = (layerIndex + 1) * innermostRadius;
                var layer = UsefulQuivers.GetVerticesInGeneralizedCobwebQuiverLayer(numVerticesInCenterPolygon, numLayers, layerIndex, firstVertex);
                double angle = 2 * Math.PI / layer.Count();
                foreach (var (vertex, indexInLayer) in layer.EnumerateWithIndex())
                {
                    vertexPositions[vertex] = layerIndex == 0 ?
                        new Point(radius * Math.Cos(indexInLayer * angle), radius * Math.Sin(indexInLayer * angle)) :
                        new Point(radius * Math.Cos((indexInLayer + 0.5) * angle), radius * Math.Sin((indexInLayer + 0.5) * angle));
                }
            }

            return new QuiverInPlane<int>(quiver, vertexPositions);
        }

        public static IEnumerable<int> GetVerticesInGeneralizedCobwebQuiverInPlaneLayer(int numVerticesInCenterPolygon, int numLayers, int layerIndex, int firstVertex = DefaultFirstVertex)
            => UsefulQuivers.GetVerticesInGeneralizedCobwebQuiverLayer(numVerticesInCenterPolygon, numLayers, layerIndex, firstVertex);
        #endregion
    }
}
