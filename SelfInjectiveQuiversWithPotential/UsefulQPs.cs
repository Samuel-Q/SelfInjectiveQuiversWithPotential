using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures;

namespace SelfInjectiveQuiversWithPotential
{
    /// <summary>
    /// This class is used to construct some useful predefined QPs.
    /// </summary>
    public static class UsefulQPs
    {
        private const int DefaultFirstVertex = UsefulQuivers.DefaultFirstVertex;

        #region Cycle
        public static bool CycleParameterIsValid(int numVertices) => UsefulQuivers.CycleParameterIsValid(numVertices);

        public static string CycleParameterValidityDescription => UsefulQuivers.CycleParameterValidityDescription;

        public static QuiverWithPotential<int> GetCycleQP(int numVertices, int firstVertex = DefaultFirstVertex)
        {
            if (!CycleParameterIsValid(numVertices)) throw new ArgumentOutOfRangeException(nameof(numVertices));

            int n = numVertices;
            var cycle = new DetachedCycle<int>(Enumerable.Range(firstVertex, n).Select(k => new Arrow<int>(k, (k+1 - firstVertex).Modulo(n) + firstVertex)));
            var qp = new QuiverWithPotential<int>(new Potential<int>(cycle, +1));
            return qp;
        }
        #endregion

        #region Triangle
        public static bool TriangleParameterIsValid(int numRows) => UsefulQuivers.TriangleParameterIsValid(numRows);

        public static string TriangleParameterValidityDescription => UsefulQuivers.TriangleParameterValidityDescription;

        public static QuiverWithPotential<int> GetTriangleQP(int numRows, int firstVertex = DefaultFirstVertex)
        {
            if (!TriangleParameterIsValid(numRows)) throw new ArgumentOutOfRangeException(nameof(numRows));

            if (numRows == 1)
            {
                var quiver = UsefulQuivers.GetTriangleQuiver(numRows, firstVertex);
                return new QuiverWithPotential<int>(quiver, new Potential<int>());
            }

            var potential = new Potential<int>();

            var curRowVertices = new List<int>() { firstVertex };
            int nextVertex = firstVertex+1;
            for (int rowIndex = 1; rowIndex <= numRows - 1; rowIndex++) // 1-based row index
            {
                var nextRowVertices = Enumerable.Range(nextVertex, rowIndex + 1).ToList();
                nextVertex += rowIndex + 1;

                // (2*rowIndex - 1) triangles to add. Draw a small triangle QP to realize that
                // the cycles below are the ones to add.
                for (int indexInRow = 0; indexInRow < rowIndex; indexInRow++)
                {
                    var cycleVertices = new int[] { curRowVertices[indexInRow], nextRowVertices[indexInRow + 1], nextRowVertices[indexInRow], curRowVertices[indexInRow] };
                    potential = potential.AddCycle(new DetachedCycle<int>(cycleVertices), +1); // Positive for clockwise cycles.
                }

                for (int indexInRow = 0; indexInRow < rowIndex - 1; indexInRow++)
                {
                    var cycleVertices = new int[] { curRowVertices[indexInRow + 1], curRowVertices[indexInRow], nextRowVertices[indexInRow + 1], curRowVertices[indexInRow + 1] };
                    potential = potential.AddCycle(new DetachedCycle<int>(cycleVertices), -1); // Negative for counterclockwise cycles.
                }

                curRowVertices = nextRowVertices;
            }

            var qp = new QuiverWithPotential<int>(potential);
            return qp;
        }

        public static IEnumerable<int> GetVerticesInTriangleQPRow(int rowIndex, int firstVertex = DefaultFirstVertex)
            => UsefulQuivers.GetVerticesInTriangleQuiverRow(rowIndex, firstVertex);
        #endregion

        #region Square
        public static bool SquareParameterIsValid(int numRows) => UsefulQuivers.SquareParameterIsValid(numRows);

        public static string SquareParameterValidityDescription => UsefulQuivers.SquareParameterValidityDescription;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numRows">The number of rows of vertices.</param>
        /// <param name="firstVertex"></param>
        /// <returns></returns>
        public static QuiverWithPotential<int> GetSquareQP(int numRows, int firstVertex = DefaultFirstVertex)
        {
            if (!SquareParameterIsValid(numRows)) throw new ArgumentOutOfRangeException(nameof(numRows));

            int numVerticesInRow = numRows;
            int numVertices = numVerticesInRow * numRows;

            if (numRows == 1)
            {
                var quiver = UsefulQuivers.GetSquareQuiver(numRows, firstVertex);
                return new QuiverWithPotential<int>(quiver, new Potential<int>());
            }

            var potential = new Potential<int>();
            for (int rowIndex = 0; rowIndex < numRows - 1; rowIndex++)
            {
                var curRow = GetVerticesInSquareQPRow(numRows, rowIndex, firstVertex).ToList();
                var nextRow = GetVerticesInSquareQPRow(numRows, rowIndex + 1, firstVertex).ToList();

                for (int indexInRow = 0; indexInRow < numVerticesInRow - 1; indexInRow++)
                {
                    int sign = (rowIndex + indexInRow).Modulo(2) == 0 ? +1 : -1;
                    var cycleVertices = sign == +1 ?
                        new int[] { curRow[indexInRow], curRow[indexInRow + 1], nextRow[indexInRow + 1], nextRow[indexInRow], curRow[indexInRow] } :
                        new int[] { curRow[indexInRow + 1], curRow[indexInRow], nextRow[indexInRow], nextRow[indexInRow + 1], curRow[indexInRow + 1] };
                    potential = potential.AddCycle(new DetachedCycle<int>(cycleVertices), sign);
                }
            }

            var qp = new QuiverWithPotential<int>(potential);
            return qp;
        }

        public static IEnumerable<int> GetVerticesInSquareQPRow(int numRows, int rowIndex, int firstVertex = DefaultFirstVertex)
            => UsefulQuivers.GetVerticesInSquareQuiverRow(numRows, rowIndex, firstVertex);
        #endregion

        #region Cobweb
        public static bool CobwebParameterIsValid(int numVerticesInCenterPolygon) => UsefulQuivers.CobwebParameterIsValid(numVerticesInCenterPolygon);

        public static string CobwebParameterValidityDescription => UsefulQuivers.CobwebParameterValidityDescription;

        public static int GetNumberOfLayersInCobwebQP(int numVerticesInCenterPolygon)
            => UsefulQuivers.GetNumberOfLayersInCobwebQuiver(numVerticesInCenterPolygon);

        public static QuiverWithPotential<int> GetCobwebQP(int numVerticesInCenterPolygon, int firstVertex = DefaultFirstVertex)
        {
            if (!CobwebParameterIsValid(numVerticesInCenterPolygon)) throw new ArgumentOutOfRangeException(nameof(numVerticesInCenterPolygon));

            int numLayers = (numVerticesInCenterPolygon - 1) / 2;
            int numVerticesInFullLayer = 2 * numVerticesInCenterPolygon;

            var potential = new Potential<int>();

            // Center polygon
            potential = potential.AddCycle(new DetachedCycle<int>(Enumerable.Range(firstVertex, numVerticesInCenterPolygon).AppendElement(firstVertex)), +1);

            if (numLayers > 1)
            {
                // First layer (the  squares and triangles between the first and second layers)
                var curLayer = GetLayerVertices(0);
                var nextLayer = GetLayerVertices(1);
                for (int indexInLayer = 0; indexInLayer < numVerticesInCenterPolygon; indexInLayer++)
                {
                    var triangleVertices = new int[] { curLayer[indexInLayer], nextLayer[2 * indexInLayer - 1], nextLayer[2 * indexInLayer], curLayer[indexInLayer] };
                    potential = potential.AddCycle(new DetachedCycle<int>(triangleVertices), +1);

                    var squareVertices = new int[] { curLayer[indexInLayer], curLayer[indexInLayer + 1], nextLayer[2 * indexInLayer + 1], nextLayer[2 * indexInLayer], curLayer[indexInLayer] };
                    potential = potential.AddCycle(new DetachedCycle<int>(squareVertices), -1);
                }

                // Remaining layers (only squares)
                for (int layerIndex = 1; layerIndex < numLayers - 1; layerIndex++) // 0-based layer index
                {
                    curLayer = GetLayerVertices(layerIndex);
                    nextLayer = GetLayerVertices(layerIndex + 1);
                    for (int indexInLayer = 0; indexInLayer < numVerticesInFullLayer; indexInLayer++)
                    {
                        int sign = (layerIndex + indexInLayer).Modulo(2) == 1 ? +1 : -1;

                        var squareVertices = sign == +1 ?
                            new int[] { curLayer[indexInLayer + 1], curLayer[indexInLayer], nextLayer[indexInLayer], nextLayer[indexInLayer + 1], curLayer[indexInLayer + 1] } :
                            new int[] { curLayer[indexInLayer], curLayer[indexInLayer + 1], nextLayer[indexInLayer + 1], nextLayer[indexInLayer], curLayer[indexInLayer] };
                        potential = potential.AddCycle(new DetachedCycle<int>(squareVertices), sign);
                    }
                }
            }

            var qp = new QuiverWithPotential<int>(potential);
            return qp;

            CircularList<int> GetLayerVertices(int layerIndex) => new CircularList<int>(GetVerticesInCobwebQPLayer(numVerticesInCenterPolygon, layerIndex, firstVertex));
        }

        public static IEnumerable<int> GetVerticesInCobwebQPLayer(int numVerticesInCenterPolygon, int layerIndex, int firstVertex = DefaultFirstVertex)
            => UsefulQuivers.GetVerticesInCobwebQuiverLayer(numVerticesInCenterPolygon, layerIndex, firstVertex);
        #endregion

        #region Odd flower
        public static bool OddFlowerParameterIsValid(int numVerticesInCenterPolygon) => UsefulQuivers.OddFlowerParameterIsValid(numVerticesInCenterPolygon);

        public static string OddFlowerParameterValidityDescription => UsefulQuivers.OddFlowerParameterValidityDescription;

        public static int GetNumberOfLayersInOddFlowerQP(int numVerticesInCenterPolygon)
            => UsefulQuivers.GetNumberOfLayersInOddFlowerQuiver(numVerticesInCenterPolygon);

        public static QuiverWithPotential<int> GetOddFlowerQP(int numVerticesInCenterPolygon, int firstVertex = DefaultFirstVertex)
        {
            if (!OddFlowerParameterIsValid(numVerticesInCenterPolygon)) throw new ArgumentOutOfRangeException(nameof(numVerticesInCenterPolygon));

            int numLayers = UsefulQuivers.GetNumberOfLayersInOddFlowerQuiver(numVerticesInCenterPolygon);
            int numVerticesInFullInnerLayer = 2 * numVerticesInCenterPolygon;
            int numVerticesInOuterLayer = 4 * numVerticesInCenterPolygon;

            var potential = new Potential<int>();

            // Center polygon
            potential = potential.AddCycle(new DetachedCycle<int>(Enumerable.Range(firstVertex, numVerticesInCenterPolygon).AppendElement(firstVertex)), +1);

            // Full inner layers
            if (numLayers > 2)
            {
                // First layer (the squares and triangles between the first and second layers)
                var curLayer = GetLayerVertices(0);
                var nextLayer = GetLayerVertices(1);
                for (int indexInLayer = 0; indexInLayer < numVerticesInCenterPolygon; indexInLayer++)
                {
                    var triangleVertices = new int[] { curLayer[indexInLayer], nextLayer[2 * indexInLayer - 1], nextLayer[2 * indexInLayer], curLayer[indexInLayer] };
                    potential = potential.AddCycle(new DetachedCycle<int>(triangleVertices), +1);

                    var squareVertices = new int[] { curLayer[indexInLayer], curLayer[indexInLayer + 1], nextLayer[2 * indexInLayer + 1], nextLayer[2 * indexInLayer], curLayer[indexInLayer] };
                    potential = potential.AddCycle(new DetachedCycle<int>(squareVertices), -1);
                }

                // Remaining layers (only squares)
                for (int layerIndex = 1; layerIndex < numLayers - 2; layerIndex++) // 0-based layer index
                {
                    curLayer = GetLayerVertices(layerIndex);
                    nextLayer = GetLayerVertices(layerIndex + 1);
                    for (int indexInLayer = 0; indexInLayer < numVerticesInFullInnerLayer; indexInLayer++)
                    {
                        int sign = (layerIndex + indexInLayer).Modulo(2) == 1 ? +1 : -1;

                        var squareVertices = sign == +1 ?
                            new int[] { curLayer[indexInLayer + 1], curLayer[indexInLayer], nextLayer[indexInLayer], nextLayer[indexInLayer + 1], curLayer[indexInLayer + 1] } :
                            new int[] { curLayer[indexInLayer], curLayer[indexInLayer + 1], nextLayer[indexInLayer + 1], nextLayer[indexInLayer], curLayer[indexInLayer] };
                        potential = potential.AddCycle(new DetachedCycle<int>(squareVertices), sign);
                    }
                }
            }

            // Outer layer
            {
                int layerIndex = numLayers - 2;
                var curLayer = GetLayerVertices(layerIndex);
                var nextLayer = GetLayerVertices(layerIndex + 1);
                if (numLayers == 2)
                {
                    // Add pentagons and squares in this degenerate case
                    for (int indexInLayer = 0; indexInLayer < curLayer.Count; indexInLayer++)
                    {
                        var squareVertices = new int[] { curLayer[indexInLayer], nextLayer[4 * indexInLayer - 2], nextLayer[4 * indexInLayer - 1], nextLayer[4 * indexInLayer], curLayer[indexInLayer] };
                        potential = potential.AddCycle(new DetachedCycle<int>(squareVertices), +1);

                        var pentagonVertices = new int[] { curLayer[indexInLayer], curLayer[indexInLayer + 1], nextLayer[4 * indexInLayer + 2], nextLayer[4 * indexInLayer + 1], nextLayer[4 * indexInLayer], curLayer[indexInLayer] };
                        potential = potential.AddCycle(new DetachedCycle<int>(pentagonVertices), -1);
                    }
                }
                else
                {
                    // Add pentagons in the non-degenerate case
                    for (int indexInLayer = 0; indexInLayer < curLayer.Count; indexInLayer++)
                    {
                        int sign = (layerIndex + indexInLayer).Modulo(2) == 1 ? +1 : -1;
                        var cycleVertices = sign == +1 ?
                            new int[] { curLayer[indexInLayer + 1], curLayer[indexInLayer], nextLayer[2 * indexInLayer], nextLayer[2 * indexInLayer + 1], nextLayer[2 * indexInLayer + 2], curLayer[indexInLayer + 1] } :
                            new int[] { curLayer[indexInLayer], curLayer[indexInLayer + 1], nextLayer[2 * indexInLayer + 2], nextLayer[2 * indexInLayer + 1], nextLayer[2 * indexInLayer], curLayer[indexInLayer] };
                        potential = potential.AddCycle(new DetachedCycle<int>(cycleVertices), sign);
                    }
                }
            }

            var qp = new QuiverWithPotential<int>(potential);
            return qp;

            CircularList<int> GetLayerVertices(int layerIndex)
                => new CircularList<int>(GetVerticesInOddFlowerQPLayer(numVerticesInCenterPolygon, layerIndex, firstVertex));
        }

        public static IEnumerable<int> GetVerticesInOddFlowerQPLayer(int numVerticesInCenterPolygon, int layerIndex, int firstVertex = DefaultFirstVertex)
            => UsefulQuivers.GetVerticesInOddFlowerQuiverLayer(numVerticesInCenterPolygon, layerIndex, firstVertex);

        public static IEnumerable<IEnumerable<int>> GetPeriodsOfOddFlowerQP(int numVerticesInCenterPolygon, int firstVertex = DefaultFirstVertex)
            => UsefulQuivers.GetPeriodsOfOddFlowerQuiver(numVerticesInCenterPolygon, firstVertex);
        #endregion

        #region Even flower, type 1
        public static bool EvenFlowerType1ParameterIsValid(int numVerticesInCenterPolygon) => UsefulQuivers.EvenFlowerType1ParameterIsValid(numVerticesInCenterPolygon);

        public static string EvenFlowerType1ParameterValidityDescription => UsefulQuivers.EvenFlowerType1ParameterValidityDescription;

        public static int GetNumberOfLayersInEvenFlowerType1QP(int numVerticesInCenterPolygon)
            => UsefulQuivers.GetNumberOfLayersInEvenFlowerType1Quiver(numVerticesInCenterPolygon);

        public static QuiverWithPotential<int> GetEvenFlowerType1QP(int numVerticesInCenterPolygon, int firstVertex = DefaultFirstVertex)
        {
            if (!EvenFlowerType1ParameterIsValid(numVerticesInCenterPolygon)) throw new ArgumentOutOfRangeException(nameof(numVerticesInCenterPolygon));

            int numLayers = UsefulQuivers.GetNumberOfLayersInEvenFlowerType1Quiver(numVerticesInCenterPolygon);
            int numVerticesInFullInnerLayer = 2 * numVerticesInCenterPolygon;
            int numVerticesInOuterLayer = 3 * numVerticesInCenterPolygon;

            var potential = new Potential<int>();

            // Center polygon
            potential = potential.AddCycle(new DetachedCycle<int>(Enumerable.Range(firstVertex, numVerticesInCenterPolygon).AppendElement(firstVertex)), +1);

            // Full inner layers
            if (numLayers > 2)
            {
                // First layer (the squares and triangles between the first and second layers)
                var curLayer = GetLayerVertices(0);
                var nextLayer = GetLayerVertices(1);
                for (int indexInLayer = 0; indexInLayer < numVerticesInCenterPolygon; indexInLayer++)
                {
                    var triangleVertices = new int[] { curLayer[indexInLayer], nextLayer[2 * indexInLayer - 1], nextLayer[2 * indexInLayer], curLayer[indexInLayer] };
                    potential = potential.AddCycle(new DetachedCycle<int>(triangleVertices), +1);

                    var squareVertices = new int[] { curLayer[indexInLayer], curLayer[indexInLayer + 1], nextLayer[2 * indexInLayer + 1], nextLayer[2 * indexInLayer], curLayer[indexInLayer] };
                    potential = potential.AddCycle(new DetachedCycle<int>(squareVertices), -1);
                }

                // Remaining layers (only squares)
                for (int layerIndex = 1; layerIndex < numLayers - 2; layerIndex++) // 0-based layer index
                {
                    curLayer = GetLayerVertices(layerIndex);
                    nextLayer = GetLayerVertices(layerIndex + 1);
                    for (int indexInLayer = 0; indexInLayer < numVerticesInFullInnerLayer; indexInLayer++)
                    {
                        int sign = (layerIndex + indexInLayer).Modulo(2) == 1 ? +1 : -1;

                        var squareVertices = sign == +1 ?
                            new int[] { curLayer[indexInLayer + 1], curLayer[indexInLayer], nextLayer[indexInLayer], nextLayer[indexInLayer + 1], curLayer[indexInLayer + 1] } :
                            new int[] { curLayer[indexInLayer], curLayer[indexInLayer + 1], nextLayer[indexInLayer + 1], nextLayer[indexInLayer], curLayer[indexInLayer] };
                        potential = potential.AddCycle(new DetachedCycle<int>(squareVertices), sign);
                    }
                }
            }

            // Outer layer
            {
                int layerIndex = numLayers - 2;
                var curLayer = GetLayerVertices(layerIndex);
                var nextLayer = GetLayerVertices(layerIndex + 1);
                if (numLayers == 2)
                {
                    // Add pentagons and triangle in this degenerate case
                    for (int indexInLayer = 0; indexInLayer < curLayer.Count; indexInLayer++)
                    {
                        var triangleVertices = new int[] { curLayer[indexInLayer], nextLayer[3 * indexInLayer - 1], nextLayer[3 * indexInLayer], curLayer[indexInLayer] };
                        potential = potential.AddCycle(new DetachedCycle<int>(triangleVertices), +1);

                        var pentagonVertices = new int[] { curLayer[indexInLayer], curLayer[indexInLayer + 1], nextLayer[3 * indexInLayer + 2], nextLayer[3 * indexInLayer + 1], nextLayer[3 * indexInLayer], curLayer[indexInLayer] };
                        potential = potential.AddCycle(new DetachedCycle<int>(pentagonVertices), -1);
                    }
                }
                else
                {
                    // Add pentagons and squares in the non-degenerate case
                    for (int indexInCenterLayer = 0; indexInCenterLayer < numVerticesInCenterPolygon; indexInCenterLayer++)
                    {
                        // Add pentagon
                        int indexInPenultimateLayer = 2 * indexInCenterLayer;
                        int indexInUltimateLayer = 3 * indexInCenterLayer;
                        int pentagonSign = layerIndex.Modulo(2) == 1 ? +1 : -1; // Remember that layerIndex is the index of the *penultimate* layer
                        var pentagonVertices = pentagonSign == +1 ?
                            new int[] { curLayer[indexInPenultimateLayer + 1], curLayer[indexInPenultimateLayer], nextLayer[indexInUltimateLayer], nextLayer[indexInUltimateLayer + 1], nextLayer[indexInUltimateLayer + 2], curLayer[indexInPenultimateLayer + 1] } :
                            new int[] { curLayer[indexInPenultimateLayer], curLayer[indexInPenultimateLayer + 1], nextLayer[indexInUltimateLayer + 2], nextLayer[indexInUltimateLayer + 1], nextLayer[indexInUltimateLayer], curLayer[indexInPenultimateLayer] };
                        potential = potential.AddCycle(new DetachedCycle<int>(pentagonVertices), pentagonSign);

                        // Add square
                        int squareSign = -pentagonSign;
                        indexInPenultimateLayer += 1;
                        indexInUltimateLayer += 2;
                        var squareVertices = squareSign == +1 ?
                            new int[] { curLayer[indexInPenultimateLayer + 1], curLayer[indexInPenultimateLayer], nextLayer[indexInUltimateLayer], nextLayer[indexInUltimateLayer + 1], curLayer[indexInPenultimateLayer + 1] } :
                            new int[] { curLayer[indexInPenultimateLayer], curLayer[indexInPenultimateLayer + 1], nextLayer[indexInUltimateLayer + 1], nextLayer[indexInUltimateLayer], curLayer[indexInPenultimateLayer] };
                        potential = potential.AddCycle(new DetachedCycle<int>(squareVertices), squareSign);
                    }
                }
            }

            var qp = new QuiverWithPotential<int>(potential);
            return qp;

            CircularList<int> GetLayerVertices(int layerIndex)
                => new CircularList<int>(GetVerticesInEvenFlowerType1QPLayer(numVerticesInCenterPolygon, layerIndex, firstVertex));
        }

        public static IEnumerable<int> GetVerticesInEvenFlowerType1QPLayer(int numVerticesInCenterPolygon, int layerIndex, int firstVertex = DefaultFirstVertex)
            => UsefulQuivers.GetVerticesInEvenFlowerType1QuiverLayer(numVerticesInCenterPolygon, layerIndex, firstVertex);

        public static IEnumerable<IEnumerable<int>> GetPeriodsOfEvenFlowerType1QP(int numVerticesInCenterPolygon, int firstVertex = DefaultFirstVertex)
            => UsefulQuivers.GetPeriodsOfEvenFlowerType1Quiver(numVerticesInCenterPolygon, firstVertex);
        #endregion

        #region Even flower, type 2
        public static bool EvenFlowerType2ParameterIsValid(int numVerticesInCenterPolygon) => UsefulQuivers.EvenFlowerType2ParameterIsValid(numVerticesInCenterPolygon);

        public static string EvenFlowerType2ParameterValidityDescription => UsefulQuivers.EvenFlowerType2ParameterValidityDescription;

        public static int GetNumberOfLayersInEvenFlowerType2QP(int numVerticesInCenterPolygon)
            => UsefulQuivers.GetNumberOfLayersInEvenFlowerType2Quiver(numVerticesInCenterPolygon);

        public static QuiverWithPotential<int> GetEvenFlowerType2QP(int numVerticesInCenterPolygon, int firstVertex = DefaultFirstVertex)
        {
            if (!EvenFlowerType2ParameterIsValid(numVerticesInCenterPolygon)) throw new ArgumentOutOfRangeException(nameof(numVerticesInCenterPolygon));

            int numLayers = UsefulQuivers.GetNumberOfLayersInEvenFlowerType2Quiver(numVerticesInCenterPolygon);
            int numVerticesInFullInnerLayer = 2 * numVerticesInCenterPolygon;
            int numVerticesInOuterLayer = 3 * numVerticesInCenterPolygon;

            var potential = new Potential<int>();

            // Center polygon
            potential = potential.AddCycle(new DetachedCycle<int>(Enumerable.Range(firstVertex, numVerticesInCenterPolygon).AppendElement(firstVertex)), +1);

            // Full inner layers
            if (numLayers > 2)
            {
                // First layer (the squares and triangles between the first and second layers)
                var curLayer = GetLayerVertices(0);
                var nextLayer = GetLayerVertices(1);
                for (int indexInLayer = 0; indexInLayer < numVerticesInCenterPolygon; indexInLayer++)
                {
                    var triangleVertices = new int[] { curLayer[indexInLayer], nextLayer[2 * indexInLayer - 1], nextLayer[2 * indexInLayer], curLayer[indexInLayer] };
                    potential = potential.AddCycle(new DetachedCycle<int>(triangleVertices), +1);

                    var squareVertices = new int[] { curLayer[indexInLayer], curLayer[indexInLayer + 1], nextLayer[2 * indexInLayer + 1], nextLayer[2 * indexInLayer], curLayer[indexInLayer] };
                    potential = potential.AddCycle(new DetachedCycle<int>(squareVertices), -1);
                }

                // Remaining layers (only squares)
                for (int layerIndex = 1; layerIndex < numLayers - 2; layerIndex++) // 0-based layer index
                {
                    curLayer = GetLayerVertices(layerIndex);
                    nextLayer = GetLayerVertices(layerIndex + 1);
                    for (int indexInLayer = 0; indexInLayer < numVerticesInFullInnerLayer; indexInLayer++)
                    {
                        int sign = (layerIndex + indexInLayer).Modulo(2) == 1 ? +1 : -1;

                        var squareVertices = sign == +1 ?
                            new int[] { curLayer[indexInLayer + 1], curLayer[indexInLayer], nextLayer[indexInLayer], nextLayer[indexInLayer + 1], curLayer[indexInLayer + 1] } :
                            new int[] { curLayer[indexInLayer], curLayer[indexInLayer + 1], nextLayer[indexInLayer + 1], nextLayer[indexInLayer], curLayer[indexInLayer] };
                        potential = potential.AddCycle(new DetachedCycle<int>(squareVertices), sign);
                    }
                }
            }

            // Outer layer
            {
                int layerIndex = numLayers - 2;
                var curLayer = GetLayerVertices(layerIndex);
                var nextLayer = GetLayerVertices(layerIndex + 1);
                if (numLayers == 2)
                {
                    // Add squares and diamonds in this degenerate case
                    for (int indexInLayer = 0; indexInLayer < curLayer.Count; indexInLayer++)
                    {
                        var diamondVertices = new int[] { curLayer[indexInLayer], nextLayer[3 * indexInLayer - 2], nextLayer[3 * indexInLayer - 1], nextLayer[3 * indexInLayer], curLayer[indexInLayer] };
                        potential = potential.AddCycle(new DetachedCycle<int>(diamondVertices), +1);

                        var squareVertices = new int[] { curLayer[indexInLayer], curLayer[indexInLayer + 1], nextLayer[3 * indexInLayer + 1], nextLayer[3 * indexInLayer], curLayer[indexInLayer] };
                        potential = potential.AddCycle(new DetachedCycle<int>(squareVertices), -1);
                    }
                }
                else
                {
                    // Add squares and pentagons in the non-degenerate case
                    for (int indexInCenterLayer = 0; indexInCenterLayer < numVerticesInCenterPolygon; indexInCenterLayer++)
                    {
                        // Add square
                        int indexInPenultimateLayer = 2 * indexInCenterLayer;
                        int indexInUltimateLayer = 3 * indexInCenterLayer;
                        int squareSign = layerIndex.Modulo(2) == 1 ? +1 : -1; // Remember that layerIndex is the index of the *penultimate* layer
                        var squareVertices = squareSign == +1 ?
                            new int[] { curLayer[indexInPenultimateLayer + 1], curLayer[indexInPenultimateLayer], nextLayer[indexInUltimateLayer], nextLayer[indexInUltimateLayer + 1], curLayer[indexInPenultimateLayer + 1] } :
                            new int[] { curLayer[indexInPenultimateLayer], curLayer[indexInPenultimateLayer + 1], nextLayer[indexInUltimateLayer + 1], nextLayer[indexInUltimateLayer], curLayer[indexInPenultimateLayer] };
                        potential = potential.AddCycle(new DetachedCycle<int>(squareVertices), squareSign);

                        // Add pentagon
                        int pentagonSign = -squareSign;
                        indexInPenultimateLayer += 1;
                        indexInUltimateLayer += 1;
                        var pentagonVertices = pentagonSign == +1 ?
                            new int[] { curLayer[indexInPenultimateLayer + 1], curLayer[indexInPenultimateLayer], nextLayer[indexInUltimateLayer], nextLayer[indexInUltimateLayer + 1], nextLayer[indexInUltimateLayer + 2], curLayer[indexInPenultimateLayer + 1] } :
                            new int[] { curLayer[indexInPenultimateLayer], curLayer[indexInPenultimateLayer + 1], nextLayer[indexInUltimateLayer + 2], nextLayer[indexInUltimateLayer + 1], nextLayer[indexInUltimateLayer], curLayer[indexInPenultimateLayer] };
                        potential = potential.AddCycle(new DetachedCycle<int>(pentagonVertices), pentagonSign);
                    }
                }
            }

            var qp = new QuiverWithPotential<int>(potential);
            return qp;

            CircularList<int> GetLayerVertices(int layerIndex)
                => new CircularList<int>(GetVerticesInEvenFlowerType2QPLayer(numVerticesInCenterPolygon, layerIndex, firstVertex));
        }

        public static IEnumerable<int> GetVerticesInEvenFlowerType2QPLayer(int numVerticesInCenterPolygon, int layerIndex, int firstVertex = DefaultFirstVertex)
            => UsefulQuivers.GetVerticesInEvenFlowerType2QuiverLayer(numVerticesInCenterPolygon, layerIndex, firstVertex);

        public static IEnumerable<IEnumerable<int>> GetPeriodsOfEvenFlowerType2QP(int numVerticesInCenterPolygon, int firstVertex = DefaultFirstVertex)
            => UsefulQuivers.GetPeriodsOfEvenFlowerType2Quiver(numVerticesInCenterPolygon, firstVertex);
        #endregion

        #region Pointed flower
        public static bool PointedFlowerParameterIsValid(int numPeriods) => UsefulQuivers.PointedFlowerParameterIsValid(numPeriods);

        public static string PointedFlowerParameterValidityDescription => UsefulQuivers.PointedFlowerParameterValidityDescription;

        public static int GetNumberOfLayersInPointedFlowerQP(int numPeriods)
            => UsefulQuivers.GetNumberOfLayersInPointedFlowerQuiver(numPeriods);

        public static QuiverWithPotential<int> GetPointedFlowerQP(int numPeriods, int firstVertex = DefaultFirstVertex)
        {
            if (!PointedFlowerParameterIsValid(numPeriods)) throw new ArgumentOutOfRangeException(nameof(numPeriods));

            int numLayers = UsefulQuivers.GetNumberOfLayersInPointedFlowerQuiver(numPeriods);
            int numVerticesInFullInnerLayer = 2 * numPeriods;
            int numVerticesInOuterLayer = 3 * numPeriods;

            var potential = new Potential<int>();

            if (numLayers == 2)
            {
                int centerVertex = GetLayerVertices(0).Single();
                var nextLayer = GetLayerVertices(1);
                for (int periodIndex = 0; periodIndex < numPeriods; periodIndex++)
                {
                    int indexInNextLayer = 3 * periodIndex;

                    // Square
                    var positiveCycleVertices = new int[] { centerVertex, nextLayer[indexInNextLayer], nextLayer[indexInNextLayer + 1], nextLayer[indexInNextLayer + 2], centerVertex };

                    // Triangle
                    var negativeCycleVertices = new int[] { centerVertex, nextLayer[indexInNextLayer + 3], nextLayer[indexInNextLayer + 2], centerVertex };

                    potential = potential.AddCycle(new DetachedCycle<int>(positiveCycleVertices), +1);
                    potential = potential.AddCycle(new DetachedCycle<int>(negativeCycleVertices), -1);
                }

                return new QuiverWithPotential<int>(potential);
            }

            // Polygons between first and second layer
            {
                int centerVertex = GetLayerVertices(0).Single();
                var nextLayer = GetLayerVertices(1);
                for (int periodIndex = 0; periodIndex < numPeriods; periodIndex++)
                {
                    int indexInNextLayer = 2 * periodIndex;

                    // Triangles
                    var positiveCycleVertices = new int[] { centerVertex, nextLayer[indexInNextLayer], nextLayer[indexInNextLayer + 1], centerVertex };
                    var negativeCycleVertices = new int[] { centerVertex, nextLayer[indexInNextLayer + 2], nextLayer[indexInNextLayer + 1], centerVertex };

                    potential = potential.AddCycle(new DetachedCycle<int>(positiveCycleVertices), +1);
                    potential = potential.AddCycle(new DetachedCycle<int>(negativeCycleVertices), -1);
                }
            }

            // Cobweb layers
            for (int layerIndex = 1; layerIndex < numLayers - 2; layerIndex++) // 0-based layer index
            {
                var curLayer = GetLayerVertices(layerIndex);
                var nextLayer = GetLayerVertices(layerIndex + 1);
                for (int indexInLayer = 0; indexInLayer < numVerticesInFullInnerLayer; indexInLayer++)
                {
                    int sign = (layerIndex + indexInLayer).Modulo(2) == 0 ? +1 : -1;

                    var squareVertices = sign == +1 ?
                        new int[] { curLayer[indexInLayer + 1], curLayer[indexInLayer], nextLayer[indexInLayer], nextLayer[indexInLayer + 1], curLayer[indexInLayer + 1] } :
                        new int[] { curLayer[indexInLayer], curLayer[indexInLayer + 1], nextLayer[indexInLayer + 1], nextLayer[indexInLayer], curLayer[indexInLayer] };
                    potential = potential.AddCycle(new DetachedCycle<int>(squareVertices), sign);
                }
            }

            // Outermost layer of polygons
            {
                int layerIndex = numLayers - 2;
                var curLayer = GetLayerVertices(layerIndex);
                var nextLayer = GetLayerVertices(layerIndex + 1);
                for (int indexInPeriod = 0; indexInPeriod < numPeriods; indexInPeriod++)
                {
                    // Add pentagon
                    int indexInPenultimateLayer = 2 * indexInPeriod;
                    int indexInUltimateLayer = 3 * indexInPeriod;
                    int pentagonSign = layerIndex.Modulo(2) == 0 ? +1 : -1; // Remember that layerIndex is the index of the *penultimate* layer
                    var pentagonVertices = pentagonSign == +1 ?
                        new int[] { curLayer[indexInPenultimateLayer + 1], curLayer[indexInPenultimateLayer], nextLayer[indexInUltimateLayer], nextLayer[indexInUltimateLayer + 1], nextLayer[indexInUltimateLayer + 2], curLayer[indexInPenultimateLayer + 1] } :
                        new int[] { curLayer[indexInPenultimateLayer], curLayer[indexInPenultimateLayer + 1], nextLayer[indexInUltimateLayer + 2], nextLayer[indexInUltimateLayer + 1], nextLayer[indexInUltimateLayer], curLayer[indexInPenultimateLayer] };
                    potential = potential.AddCycle(new DetachedCycle<int>(pentagonVertices), pentagonSign);

                    // Add square
                    int squareSign = -pentagonSign;
                    indexInPenultimateLayer += 1;
                    indexInUltimateLayer += 2;
                    var squareVertices = squareSign == +1 ?
                        new int[] { curLayer[indexInPenultimateLayer + 1], curLayer[indexInPenultimateLayer], nextLayer[indexInUltimateLayer], nextLayer[indexInUltimateLayer + 1], curLayer[indexInPenultimateLayer + 1] } :
                        new int[] { curLayer[indexInPenultimateLayer], curLayer[indexInPenultimateLayer + 1], nextLayer[indexInUltimateLayer + 1], nextLayer[indexInUltimateLayer], curLayer[indexInPenultimateLayer] };
                    potential = potential.AddCycle(new DetachedCycle<int>(squareVertices), squareSign);
                }
            }

            var qp = new QuiverWithPotential<int>(potential);
            return qp;

            CircularList<int> GetLayerVertices(int layerIndex)
                => new CircularList<int>(GetVerticesInPointedFlowerQPLayer(numPeriods, layerIndex, firstVertex));
        }

        public static IEnumerable<int> GetVerticesInPointedFlowerQPLayer(int numPeriods, int layerIndex, int firstVertex = DefaultFirstVertex)
            => UsefulQuivers.GetVerticesInPointedFlowerQuiverLayer(numPeriods, layerIndex, firstVertex);

        public static IEnumerable<IEnumerable<int>> GetPeriodsOfPointedFlowerQPWithoutFixedPoint(int numPeriods, int firstVertex = DefaultFirstVertex)
            => UsefulQuivers.GetPeriodsOfPointedFlowerQuiverWithoutFixedPoint(numPeriods, firstVertex);
        #endregion

        #region Generalized cobwebs
        public static bool GeneralizedCobwebParameterIsValid(int numVerticesInCenterPolygon, int numLayers)
            => UsefulQuivers.GeneralizedCobwebParameterIsValid(numVerticesInCenterPolygon, numLayers);

        public static string GeneralizedCobwebParameterValidityDescription => UsefulQuivers.GeneralizedCobwebParameterValidityDescription;

        public static QuiverWithPotential<int> GetGeneralizedCobwebQP(int numVerticesInCenterPolygon, int numLayers, int firstVertex = DefaultFirstVertex)
        {
            if (!GeneralizedCobwebParameterIsValid(numVerticesInCenterPolygon, numLayers))
            {
                throw new ArgumentOutOfRangeException();
            }

            int numVerticesInFullLayer = 2 * numVerticesInCenterPolygon;

            var potential = new Potential<int>();

            // Center polygon
            potential = potential.AddCycle(new DetachedCycle<int>(Enumerable.Range(firstVertex, numVerticesInCenterPolygon).AppendElement(firstVertex)), +1);

            if (numLayers > 1)
            {
                // First layer (the squares and triangles between the first and second layers)
                var curLayer = GetLayerVertices(0);
                var nextLayer = GetLayerVertices(1);
                for (int indexInLayer = 0; indexInLayer < numVerticesInCenterPolygon; indexInLayer++)
                {
                    var triangleVertices = new int[] { curLayer[indexInLayer], nextLayer[2 * indexInLayer - 1], nextLayer[2 * indexInLayer], curLayer[indexInLayer] };
                    potential = potential.AddCycle(new DetachedCycle<int>(triangleVertices), +1);

                    var squareVertices = new int[] { curLayer[indexInLayer], curLayer[indexInLayer + 1], nextLayer[2 * indexInLayer + 1], nextLayer[2 * indexInLayer], curLayer[indexInLayer] };
                    potential = potential.AddCycle(new DetachedCycle<int>(squareVertices), -1);
                }

                // Remaining layers (only squares)
                for (int layerIndex = 1; layerIndex < numLayers - 1; layerIndex++) // 0-based layer index
                {
                    curLayer = GetLayerVertices(layerIndex);
                    nextLayer = GetLayerVertices(layerIndex + 1);
                    for (int indexInLayer = 0; indexInLayer < numVerticesInFullLayer; indexInLayer++)
                    {
                        int sign = (layerIndex + indexInLayer).Modulo(2) == 1 ? +1 : -1;

                        var squareVertices = sign == +1 ?
                            new int[] { curLayer[indexInLayer + 1], curLayer[indexInLayer], nextLayer[indexInLayer], nextLayer[indexInLayer + 1], curLayer[indexInLayer + 1] } :
                            new int[] { curLayer[indexInLayer], curLayer[indexInLayer + 1], nextLayer[indexInLayer + 1], nextLayer[indexInLayer], curLayer[indexInLayer] };
                        potential = potential.AddCycle(new DetachedCycle<int>(squareVertices), sign);
                    }
                }
            }

            var qp = new QuiverWithPotential<int>(potential);
            return qp;

            CircularList<int> GetLayerVertices(int layerIndex) => new CircularList<int>(GetVerticesInGeneralizedCobwebQPLayer(numVerticesInCenterPolygon, numLayers, layerIndex, firstVertex));
        }

        public static IEnumerable<int> GetVerticesInGeneralizedCobwebQPLayer(int numVerticesInCenterPolygon, int numLayers, int layerIndex, int firstVertex = DefaultFirstVertex)
            => UsefulQuivers.GetVerticesInGeneralizedCobwebQuiverLayer(numVerticesInCenterPolygon, numLayers, layerIndex, firstVertex);
        #endregion

        #region Miscellaneous
        /// <summary>
        /// Gets the &quot;classic&quot; non-cancellative QP.
        /// </summary>
        /// <returns>The &quot;classic&quot; non-cancellative QP.</returns>
        public static QuiverWithPotential<int> GetClassicNonCancellativeQP()
        {
            var potential = new Potential<int>();
            potential = potential.AddCycle(new DetachedCycle<int>(1, 2, 4, 5, 1), -1);
            potential = potential.AddCycle(new DetachedCycle<int>(1, 3, 4, 5, 1), +1);
            var qp = new QuiverWithPotential<int>(potential);

            return qp;
        }
        #endregion
    }
}
