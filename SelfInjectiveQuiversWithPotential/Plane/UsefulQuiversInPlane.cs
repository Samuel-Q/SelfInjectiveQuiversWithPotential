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

        #region Flower
        public static bool FlowerParameterIsValid(int numVerticesInCenterPolygon) => UsefulQuivers.FlowerParameterIsValid(numVerticesInCenterPolygon);

        public static string FlowerParameterValidityDescription { get => UsefulQuivers.FlowerParameterValidityDescription; }

        public static int GetNumberOfLayersInFlowerQuiverInPlane(int numVerticesInCenterPolygon)
            => UsefulQuivers.GetNumberOfLayersInFlowerQuiver(numVerticesInCenterPolygon);

        public static QuiverInPlane<int> GetFlowerQuiverInPlane(int numVerticesInCenterPolygon, int innermostRadius, int firstVertex = DefaultFirstVertex)
        {
            if (!FlowerParameterIsValid(numVerticesInCenterPolygon)) throw new ArgumentOutOfRangeException(nameof(numVerticesInCenterPolygon));

            var quiver = UsefulQuivers.GetFlowerQuiver(numVerticesInCenterPolygon, firstVertex);

            int numLayers = UsefulQuivers.GetNumberOfLayersInFlowerQuiver(numVerticesInCenterPolygon);

            var vertexPositions = new Dictionary<int, Point>();
            for (int layerIndex = 0; layerIndex < numLayers; layerIndex++)
            {
                var radius = (layerIndex + 1) * innermostRadius;
                var layer = UsefulQuivers.GetVerticesInFlowerQuiverLayer(numVerticesInCenterPolygon, layerIndex, firstVertex);
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

        public static IEnumerable<int> GetVerticesInFlowerQuiverInPlaneLayer(int numVerticesInCenterPolygon, int layerIndex, int firstVertex = DefaultFirstVertex)
            => UsefulQuivers.GetVerticesInFlowerQuiverLayer(numVerticesInCenterPolygon, layerIndex, firstVertex);
        #endregion

        #region Even flower
        public static bool EvenFlowerParameterIsValid(int numVerticesInCenterPolygon) => UsefulQuivers.EvenFlowerParameterIsValid(numVerticesInCenterPolygon);

        public static string EvenFlowerParameterValidityDescription { get => UsefulQuivers.EvenFlowerParameterValidityDescription; }

        public static int GetNumberOfLayersInEvenFlowerQuiverInPlane(int numVerticesInCenterPolygon)
            => UsefulQuivers.GetNumberOfLayersInEvenFlowerQuiver(numVerticesInCenterPolygon);

        public static QuiverInPlane<int> GetEvenFlowerQuiverInPlane(int numVerticesInCenterPolygon, int innermostRadius, int firstVertex = DefaultFirstVertex)
        {
            if (!EvenFlowerParameterIsValid(numVerticesInCenterPolygon)) throw new ArgumentOutOfRangeException(nameof(numVerticesInCenterPolygon));

            var quiver = UsefulQuivers.GetEvenFlowerQuiver(numVerticesInCenterPolygon, firstVertex);

            int numLayers = UsefulQuivers.GetNumberOfLayersInEvenFlowerQuiver(numVerticesInCenterPolygon);

            var vertexPositions = new Dictionary<int, Point>();
            for (int layerIndex = 0; layerIndex < numLayers; layerIndex++)
            {
                var radius = (layerIndex + 1) * innermostRadius;
                var layer = UsefulQuivers.GetVerticesInEvenFlowerQuiverLayer(numVerticesInCenterPolygon, layerIndex, firstVertex);
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

        public static IEnumerable<int> GetVerticesInEvenFlowerQuiverInPlaneLayer(int numVerticesInCenterPolygon, int layerIndex, int firstVertex = DefaultFirstVertex)
            => UsefulQuivers.GetVerticesInEvenFlowerQuiverLayer(numVerticesInCenterPolygon, layerIndex, firstVertex);
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
