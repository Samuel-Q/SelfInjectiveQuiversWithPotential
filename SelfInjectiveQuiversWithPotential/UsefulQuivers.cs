using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential
{
    /// <summary>
    /// This class is used to construct some useful predefined quivers.
    /// </summary>
    public static class UsefulQuivers
    {
        internal const int DefaultFirstVertex = 1;

        #region Cycle
        public static bool CycleParameterIsValid(int numVertices) => numVertices >= 3;

        public static string CycleParameterValidityDescription => "The number of vertices must be at least 3.";

        public static Quiver<int> GetCycleQuiver(int numVertices, int firstVertex = DefaultFirstVertex)
        {
            if (!CycleParameterIsValid(numVertices)) throw new ArgumentOutOfRangeException(nameof(numVertices));

            // Sort of backwards to construct the entire QP only to return just the quiver
            // But this reduces duplicated logic
            var qp = UsefulQPs.GetCycleQP(numVertices, firstVertex);
            return qp.Quiver;
        }
        #endregion

        #region Triangle
        public static bool TriangleParameterIsValid(int numRows) => numRows >= 1;

        public static string TriangleParameterValidityDescription => "The number of rows must be at least 1.";

        public static Quiver<int> GetTriangleQuiver(int numRows, int firstVertex = DefaultFirstVertex)
        {
            if (!TriangleParameterIsValid(numRows)) throw new ArgumentOutOfRangeException(nameof(numRows));

            if (numRows == 1)
            {
                var vertices = new int[] { firstVertex };
                var arrows = new Arrow<int>[] { };
                return new Quiver<int>(vertices, arrows);
            }

            // Sort of backwards to construct the entire QP only to return just the quiver
            // But this reduces duplicated logic
            var qp = UsefulQPs.GetTriangleQP(numRows, firstVertex);
            return qp.Quiver;
        }

        /// <summary>
        /// Gets the vertices in the specified row of a triangle quiver.
        /// </summary>
        /// <param name="rowIndex">The 0-based row of the triangle.</param>
        /// <returns>The vertices in the <paramref name="rowIndex"/>th row of the triangle.</returns>
        public static IEnumerable<int> GetVerticesInTriangleQuiverRow(int rowIndex, int firstVertex = DefaultFirstVertex)
        {
            if (rowIndex < 0) throw new ArgumentOutOfRangeException(nameof(rowIndex));

            int firstVertexOfRow = Utility.TriangularNumber(rowIndex) + firstVertex;
            int numVertices = rowIndex + 1;
            return Enumerable.Range(firstVertexOfRow, numVertices);
        }
        #endregion

        #region Square
        public static bool SquareParameterIsValid(int numRows) => numRows >= 1;

        public static string SquareParameterValidityDescription => "The number of rows must be at least 1.";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numRows">The number of rows of vertices.</param>
        /// <param name="firstVertex"></param>
        /// <returns></returns>
        public static Quiver<int> GetSquareQuiver(int numRows, int firstVertex = DefaultFirstVertex)
        {
            if (!SquareParameterIsValid(numRows)) throw new ArgumentOutOfRangeException(nameof(numRows));

            if (numRows == 1) return new Quiver<int>(vertices: new int[] { firstVertex }, arrows: new Arrow<int>[0]);

            // Sort of backwards to construct the entire QP only to return just the quiver
            // But this reduces duplicated logic
            var qp = UsefulQPs.GetSquareQP(numRows, firstVertex);
            return qp.Quiver;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numRows">The number of rows of vertices.</param>
        /// <param name="rowIndex">The 0-based row of the square.</param>
        /// <param name="firstVertex"></param>
        /// <returns></returns>
        public static IEnumerable<int> GetVerticesInSquareQuiverRow(int numRows, int rowIndex, int firstVertex = DefaultFirstVertex)
        {
            if (!SquareParameterIsValid(numRows)) throw new ArgumentOutOfRangeException(nameof(numRows));
            if (rowIndex < 0 || rowIndex >= numRows) throw new ArgumentOutOfRangeException(nameof(rowIndex));

            int numVerticesPerRow = numRows;
            int firstVertexOfRow = rowIndex * numVerticesPerRow + firstVertex;
            int numVertices = numVerticesPerRow;
            return Enumerable.Range(firstVertexOfRow, numVertices);
        }
        #endregion

        #region Cobweb
        public static bool CobwebParameterIsValid(int numVerticesInCenterPolygon)
        {
            return numVerticesInCenterPolygon >= 3 && numVerticesInCenterPolygon.Modulo(2) == 1;
        }

        public static string CobwebParameterValidityDescription
            => "The number of vertices in the center polygon must be an odd number greater than or equal to 3.";

        public static int GetNumberOfLayersInCobwebQuiver(int numVerticesInCenterPolygon)
        {
            if (!CobwebParameterIsValid(numVerticesInCenterPolygon)) throw new ArgumentOutOfRangeException(nameof(numVerticesInCenterPolygon));
            return (numVerticesInCenterPolygon - 1) / 2;
        }

        public static Quiver<int> GetCobwebQuiver(int numVerticesInCenterPolygon, int firstVertex = DefaultFirstVertex)
        {
            if (!CobwebParameterIsValid(numVerticesInCenterPolygon)) throw new ArgumentOutOfRangeException(nameof(numVerticesInCenterPolygon));

            // Sort of backwards to construct the entire QP only to return just the quiver
            // But this reduces duplicated logic
            var qp = UsefulQPs.GetCobwebQP(numVerticesInCenterPolygon, firstVertex);
            return qp.Quiver;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numVerticesInCenterPolygon"></param>
        /// <param name="layerIndex">0-based layer index.</param>
        /// <param name="firstVertex"></param>
        /// <returns></returns>
        public static IEnumerable<int> GetVerticesInCobwebQuiverLayer(int numVerticesInCenterPolygon, int layerIndex, int firstVertex = DefaultFirstVertex)
        {
            if (!CobwebParameterIsValid(numVerticesInCenterPolygon)) throw new ArgumentOutOfRangeException(nameof(numVerticesInCenterPolygon));
            int numLayers = GetNumberOfLayersInCobwebQuiver(numVerticesInCenterPolygon);
            if (layerIndex < 0 || layerIndex >= numLayers) throw new ArgumentOutOfRangeException(nameof(layerIndex));

            int numVerticesInFullLayer = 2 * numVerticesInCenterPolygon;
            if (layerIndex == 0) return Enumerable.Range(firstVertex, numVerticesInCenterPolygon);

            int startVertex = layerIndex * numVerticesInFullLayer - numVerticesInCenterPolygon + firstVertex;
            return Enumerable.Range(startVertex, numVerticesInFullLayer);
        }
        #endregion

        #region Odd flower
        public static bool OddFlowerParameterIsValid(int numVerticesInCenterPolygon)
        {
            return numVerticesInCenterPolygon >= 3 && numVerticesInCenterPolygon.Modulo(2) == 1;
        }

        public static string OddFlowerParameterValidityDescription
            => "The number of vertices in the center polygon must be an odd number greater than or equal to 3.";

        public static int GetNumberOfLayersInOddFlowerQuiver(int numVerticesInCenterPolygon)
        {
            if (!OddFlowerParameterIsValid(numVerticesInCenterPolygon)) throw new ArgumentOutOfRangeException(nameof(numVerticesInCenterPolygon));
            return (numVerticesInCenterPolygon + 1) / 2;
        }

        public static Quiver<int> GetOddFlowerQuiver(int numVerticesInCenterPolygon, int firstVertex = DefaultFirstVertex)
        {
            if (!OddFlowerParameterIsValid(numVerticesInCenterPolygon)) throw new ArgumentOutOfRangeException(nameof(numVerticesInCenterPolygon));

            // Sort of backwards to construct the entire QP only to return just the quiver
            // But this reduces duplicated logic
            var qp = UsefulQPs.GetOddFlowerQP(numVerticesInCenterPolygon, firstVertex);
            return qp.Quiver;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numVerticesInCenterPolygon"></param>
        /// <param name="layerIndex">The 0-based layer index.</param>
        /// <param name="firstVertex"></param>
        /// <returns></returns>
        public static IEnumerable<int> GetVerticesInOddFlowerQuiverLayer(int numVerticesInCenterPolygon, int layerIndex, int firstVertex = DefaultFirstVertex)
        {
            if (!OddFlowerParameterIsValid(numVerticesInCenterPolygon)) throw new ArgumentOutOfRangeException(nameof(numVerticesInCenterPolygon));
            int numLayers = GetNumberOfLayersInOddFlowerQuiver(numVerticesInCenterPolygon);
            if (layerIndex < 0 || layerIndex >= numLayers) throw new ArgumentOutOfRangeException(nameof(layerIndex));

            int numVerticesInFullInnerLayer = 2 * numVerticesInCenterPolygon;
            int numVerticesInOuterLayer = 4 * numVerticesInCenterPolygon;

            if (layerIndex == 0) return Enumerable.Range(firstVertex, numVerticesInCenterPolygon);
            int startVertex = layerIndex * numVerticesInFullInnerLayer - numVerticesInCenterPolygon + firstVertex;
            int numVertices = layerIndex < numLayers - 1 ? numVerticesInFullInnerLayer : numVerticesInOuterLayer;
            return Enumerable.Range(startVertex, numVertices);
        }
        #endregion

        #region Even flower, type 1
        public static bool EvenFlowerType1ParameterIsValid(int numVerticesInCenterPolygon)
        {
            return numVerticesInCenterPolygon >= 4 && numVerticesInCenterPolygon.Modulo(2) == 0;
        }

        public static string EvenFlowerType1ParameterValidityDescription
            => "The number of vertices in the center polygon must be an even number greater than or equal to 4.";

        public static int GetNumberOfLayersInEvenFlowerType1Quiver(int numVerticesInCenterPolygon)
        {
            if (!EvenFlowerType1ParameterIsValid(numVerticesInCenterPolygon)) throw new ArgumentOutOfRangeException(nameof(numVerticesInCenterPolygon));
            return numVerticesInCenterPolygon / 2;
        }

        public static Quiver<int> GetEvenFlowerType1Quiver(int numVerticesInCenterPolygon, int firstVertex = DefaultFirstVertex)
        {
            if (!EvenFlowerType1ParameterIsValid(numVerticesInCenterPolygon)) throw new ArgumentOutOfRangeException(nameof(numVerticesInCenterPolygon));

            // Sort of backwards to construct the entire QP only to return just the quiver
            // But this reduces duplicated logic
            var qp = UsefulQPs.GetEvenFlowerType1QP(numVerticesInCenterPolygon, firstVertex);
            return qp.Quiver;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numVerticesInCenterPolygon"></param>
        /// <param name="layerIndex">The 0-based layer index.</param>
        /// <param name="firstVertex"></param>
        /// <returns></returns>
        public static IEnumerable<int> GetVerticesInEvenFlowerType1QuiverLayer(int numVerticesInCenterPolygon, int layerIndex, int firstVertex = DefaultFirstVertex)
        {
            if (!EvenFlowerType1ParameterIsValid(numVerticesInCenterPolygon)) throw new ArgumentOutOfRangeException(nameof(numVerticesInCenterPolygon));
            int numLayers = GetNumberOfLayersInEvenFlowerType1Quiver(numVerticesInCenterPolygon);
            if (layerIndex < 0 || layerIndex >= numLayers) throw new ArgumentOutOfRangeException(nameof(layerIndex));

            int numVerticesInFullInnerLayer = 2 * numVerticesInCenterPolygon;
            int numVerticesInOuterLayer = 3 * numVerticesInCenterPolygon;

            if (layerIndex == 0) return Enumerable.Range(firstVertex, numVerticesInCenterPolygon);
            int startVertex = layerIndex * numVerticesInFullInnerLayer - numVerticesInCenterPolygon + firstVertex;
            int numVertices = layerIndex < numLayers - 1 ? numVerticesInFullInnerLayer : numVerticesInOuterLayer;
            return Enumerable.Range(startVertex, numVertices);
        }
        #endregion

        #region Even flower, type 2
        public static bool EvenFlowerType2ParameterIsValid(int numVerticesInCenterPolygon)
        {
            return numVerticesInCenterPolygon >= 4 && numVerticesInCenterPolygon.Modulo(2) == 0;
        }

        public static string EvenFlowerType2ParameterValidityDescription
            => "The number of vertices in the center polygon must be an even number greater than or equal to 4.";

        public static int GetNumberOfLayersInEvenFlowerType2Quiver(int numVerticesInCenterPolygon)
        {
            if (!EvenFlowerType2ParameterIsValid(numVerticesInCenterPolygon)) throw new ArgumentOutOfRangeException(nameof(numVerticesInCenterPolygon));
            return numVerticesInCenterPolygon / 2;
        }

        public static Quiver<int> GetEvenFlowerType2Quiver(int numVerticesInCenterPolygon, int firstVertex = DefaultFirstVertex)
        {
            if (!EvenFlowerType2ParameterIsValid(numVerticesInCenterPolygon)) throw new ArgumentOutOfRangeException(nameof(numVerticesInCenterPolygon));

            // Sort of backwards to construct the entire QP only to return just the quiver
            // But this reduces duplicated logic
            var qp = UsefulQPs.GetEvenFlowerType2QP(numVerticesInCenterPolygon, firstVertex);
            return qp.Quiver;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numVerticesInCenterPolygon"></param>
        /// <param name="layerIndex">The 0-based layer index.</param>
        /// <param name="firstVertex"></param>
        /// <returns></returns>
        public static IEnumerable<int> GetVerticesInEvenFlowerType2QuiverLayer(int numVerticesInCenterPolygon, int layerIndex, int firstVertex = DefaultFirstVertex)
        {
            if (!EvenFlowerType2ParameterIsValid(numVerticesInCenterPolygon)) throw new ArgumentOutOfRangeException(nameof(numVerticesInCenterPolygon));
            int numLayers = GetNumberOfLayersInEvenFlowerType2Quiver(numVerticesInCenterPolygon);
            if (layerIndex < 0 || layerIndex >= numLayers) throw new ArgumentOutOfRangeException(nameof(layerIndex));

            int numVerticesInFullInnerLayer = 2 * numVerticesInCenterPolygon;
            int numVerticesInOuterLayer = 3 * numVerticesInCenterPolygon;

            if (layerIndex == 0) return Enumerable.Range(firstVertex, numVerticesInCenterPolygon);
            int startVertex = layerIndex * numVerticesInFullInnerLayer - numVerticesInCenterPolygon + firstVertex;
            int numVertices = layerIndex < numLayers - 1 ? numVerticesInFullInnerLayer : numVerticesInOuterLayer;
            return Enumerable.Range(startVertex, numVertices);
        }
        #endregion

        #region Pointed flower
        public static bool PointedFlowerParameterIsValid(int numPeriods)
        {
            return numPeriods >= 3 && numPeriods.Modulo(2) == 1;
        }

        public static string PointedFlowerParameterValidityDescription
            => "The number of periods must be an odd number greater than or equal to 3.";

        public static int GetNumberOfLayersInPointedFlowerQuiver(int numPeriods)
        {
            if (!PointedFlowerParameterIsValid(numPeriods)) throw new ArgumentOutOfRangeException(nameof(numPeriods));
            return (numPeriods + 1) / 2;
        }

        public static Quiver<int> GetPointedFlowerQuiver(int numPeriods, int firstVertex = DefaultFirstVertex)
        {
            if (!PointedFlowerParameterIsValid(numPeriods)) throw new ArgumentOutOfRangeException(nameof(numPeriods));

            // Sort of backwards to construct the entire QP only to return just the quiver
            // But this reduces duplicated logic
            var qp = UsefulQPs.GetPointedFlowerQP(numPeriods, firstVertex);
            return qp.Quiver;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numPeriods"></param>
        /// <param name="layerIndex">The 0-based layer index.</param>
        /// <param name="firstVertex"></param>
        /// <returns></returns>
        public static IEnumerable<int> GetVerticesInPointedFlowerQuiverLayer(int numPeriods, int layerIndex, int firstVertex = DefaultFirstVertex)
        {
            if (!PointedFlowerParameterIsValid(numPeriods)) throw new ArgumentOutOfRangeException(nameof(numPeriods));
            int numLayers = GetNumberOfLayersInPointedFlowerQuiver(numPeriods);
            if (layerIndex < 0 || layerIndex >= numLayers) throw new ArgumentOutOfRangeException(nameof(layerIndex));

            int numVerticesInFullInnerLayer = 2 * numPeriods;
            int numVerticesInOuterLayer = 3 * numPeriods;

            if (layerIndex == 0) return new int[] { firstVertex };
            int startVertex = 1 + (layerIndex-1) * (numVerticesInFullInnerLayer) + firstVertex;
            int numVertices = layerIndex < numLayers - 1 ? numVerticesInFullInnerLayer : numVerticesInOuterLayer;
            return Enumerable.Range(startVertex, numVertices);
        }
        #endregion

        #region Generalized cobweb
        public static bool GeneralizedCobwebParameterIsValid(int numVerticesInCenterPolygon, int numLayers)
        {
            return numVerticesInCenterPolygon >= 3 && numLayers >= 1;
        }

        public static string GeneralizedCobwebParameterValidityDescription
            => "The number of vertices in the center polygon must be at least 3 " +
                "and the number of layers must be at least 1.";

        public static Quiver<int> GetGeneralizedCobwebQuiver(int numVerticesInCenterPolygon, int numLayers, int firstVertex = DefaultFirstVertex)
        {
            if (!GeneralizedCobwebParameterIsValid(numVerticesInCenterPolygon, numLayers)) throw new ArgumentOutOfRangeException(nameof(numVerticesInCenterPolygon));

            // Sort of backwards to construct the entire QP only to return just the quiver,
            // but this reduces duplicated logic.
            var qp = UsefulQPs.GetGeneralizedCobwebQP(numVerticesInCenterPolygon, numLayers, firstVertex);
            return qp.Quiver;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numVerticesInCenterPolygon"></param>
        /// <param name="numLayers"></param>
        /// <param name="layerIndex">0-based layer index.</param>
        /// <param name="firstVertex"></param>
        /// <returns></returns>
        public static IEnumerable<int> GetVerticesInGeneralizedCobwebQuiverLayer(int numVerticesInCenterPolygon, int numLayers, int layerIndex, int firstVertex = DefaultFirstVertex)
        {
            if (!GeneralizedCobwebParameterIsValid(numVerticesInCenterPolygon, numLayers)) throw new ArgumentOutOfRangeException(nameof(numVerticesInCenterPolygon));
            if (layerIndex < 0 || layerIndex >= numLayers) throw new ArgumentOutOfRangeException(nameof(layerIndex));

            int numVerticesInFullLayer = 2 * numVerticesInCenterPolygon;
            if (layerIndex == 0) return Enumerable.Range(firstVertex, numVerticesInCenterPolygon);

            int startVertex = layerIndex * numVerticesInFullLayer - numVerticesInCenterPolygon + firstVertex;
            return Enumerable.Range(startVertex, numVerticesInFullLayer);
        }
        #endregion
    }
}
