using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures;

namespace SelfInjectiveQuiversWithPotential
{
    // TODO: Add spurious examples as well (from Pasquali mostly/only?)
    // This class is pretty outdated (no even flowers, pointed flowers, or spurious examples).

    /// <summary>
    /// This class exposes some known self-injective planar QPs.
    /// </summary>
    /// <remarks>This class is useful for testing as well as for the GUI (letting the user select
    /// known self-injective QPs).</remarks>
    public class KnownSelfInjectiveQPs
    {
        private const int DefaultFirstVertex = UsefulQuivers.DefaultFirstVertex;

        /// <summary>
        /// Gets the self-injective cycle QPs.
        /// </summary>
        public IEnumerable<SelfInjectiveQP<int>> Cycles
        {
            get
            {
                for (int numVertices = 3; ; ++numVertices) yield return GetSelfInjectiveCycleQP(numVertices);
            }
        }

        /// <summary>
        /// Gets the self-injective triangle QPs.
        /// </summary>
        public IEnumerable<SelfInjectiveQP<int>> Triangles
        {
            get
            {
                for (int numRows = 2; ; ++numRows) yield return GetSelfInjectiveTriangleQP(numRows);
            }
        }

        /// <summary>
        /// Gets the self-injective square QPs.
        /// </summary>
        public IEnumerable<SelfInjectiveQP<int>> Squares
        {
            get
            {
                for (int numRows = 1; ; ++numRows) yield return GetSelfInjectiveSquareQP(numRows);
            }
        }

        /// <summary>
        /// Gets the self-injective cobweb QPs.
        /// </summary>
        public IEnumerable<SelfInjectiveQP<int>> Cobwebs
        {
            get
            {
                for (int numVerticesInCenterPolygon = 3; ; numVerticesInCenterPolygon += 2) yield return GetSelfInjectiveCobwebQP(numVerticesInCenterPolygon);
            }
        }

        /// <summary>
        /// Gets the self-injective odd flower QPs.
        /// </summary>
        /// <remarks>It is <em>assumed</em> that <em>all</em> odd flower QPs are self-injective.</remarks>
        public IEnumerable<SelfInjectiveQP<int>> OddFlowers
        {
            get
            {
                for (int numVerticesInCenterPolygon = 3; ; numVerticesInCenterPolygon += 2) yield return GetSelfInjectiveOddFlowerQP(numVerticesInCenterPolygon);
            }
        }

        public SelfInjectiveQP<int> GetSelfInjectiveCycleQP(int numVertices, int firstVertex = DefaultFirstVertex)
        {
            if (numVertices < 3) throw new ArgumentOutOfRangeException(nameof(numVertices));

            var qp = UsefulQPs.GetCycleQP(numVertices, firstVertex);

            int n = numVertices;
            var dict = Enumerable.Range(firstVertex, n).ToDictionary(k => k, k => (k - 2 - firstVertex).Modulo(n) + firstVertex);
            var nakayamaPermutation = new NakayamaPermutation<int>(dict);
            var selfInjectiveQp = new SelfInjectiveQP<int>(qp, nakayamaPermutation);
            return selfInjectiveQp;
        }

        public SelfInjectiveQP<int> GetSelfInjectiveTriangleQP(int numRows, int firstVertex = DefaultFirstVertex)
        {
            if (numRows < 2) throw new ArgumentOutOfRangeException(nameof(numRows));

            var qp = UsefulQPs.GetTriangleQP(numRows);
            var potential = new Potential<int>();

            // Rotation "once" clockwise to get the Nakayama permutation
            var nakayamaPermutation = new Dictionary<int, int>();
            // Start with the right-most "column" (1, 3, 6, 10, ...), which is mapped to the bottom row,
            // and go left to (2, 5, 9, ...), which is mapped to the second last row, and so on.
            var columnVertices = Enumerable.Range(1, numRows).Select(k => Utility.TriangularNumber(k));
            for (int rowIndex = numRows; rowIndex >= 1; rowIndex--)
            {
                var rowVertices = Enumerable.Range(Utility.TriangularNumber(rowIndex - 1) + 1, rowIndex).Reverse();
                var inputOutputPairs = columnVertices.Zip(rowVertices, (x, y) => (x, y));
                foreach (var (x, y) in inputOutputPairs)
                {
                    nakayamaPermutation[x] = y;
                }

                columnVertices = columnVertices.Select(x => x - 1).Skip(1);
            }

            return new SelfInjectiveQP<int>(qp, nakayamaPermutation); 
        }

        private bool SquareParameterIsValid(int numRows) => numRows >= 1;

        public SelfInjectiveQP<int> GetSelfInjectiveSquareQP(int numRows, int firstVertex = DefaultFirstVertex)
        {
            if (!SquareParameterIsValid(numRows)) throw new ArgumentOutOfRangeException(nameof(numRows));

            var qp = UsefulQPs.GetSquareQP(numRows);

            int numVerticesInRow = numRows;
            int numVertices = numRows * numVerticesInRow;

            // Rotation "twice" clockwise/counterclockwise to get the Nakayama permutation
            // This map happens to be just "x mapsto (n+1)-x" (labeling the vertices from 0 would be cleaner here I guess)
            var nakayamaPermutation = Enumerable.Range(1, numVertices).ToDictionary(x => x, x => numVertices+1 - x);

            return new SelfInjectiveQP<int>(qp, nakayamaPermutation);
        }

        public SelfInjectiveQP<int> GetSelfInjectiveCobwebQP(int numVerticesInCenterPolygon, int firstVertex = DefaultFirstVertex)
        {
            if (numVerticesInCenterPolygon < 3 || numVerticesInCenterPolygon.Modulo(2) == 0) throw new ArgumentOutOfRangeException(nameof(numVerticesInCenterPolygon));

            var qp = UsefulQPs.GetCobwebQP(numVerticesInCenterPolygon);

            int numLayers = (numVerticesInCenterPolygon - 1) / 2;
            int numVerticesInFullLayer = 2 * numVerticesInCenterPolygon;

            // Rotate clockwise by 2*pi / ((numVerticesInCenterPolygon-1)/2) for the Nakayama permutation?
            var nakayamaPermutation = new Dictionary<int, int>();
            for (int layerIndex = 0; layerIndex < numLayers; layerIndex++)
            {
                var layer = GetLayerVertices(layerIndex);
                for (int indexInLayer = 0; indexInLayer < layer.Count; indexInLayer++)
                {
                    int input = layer[indexInLayer];
                    int stepsToRotateBy = layerIndex == 0 ? (numVerticesInCenterPolygon - 1) / 2 : numVerticesInCenterPolygon - 1;
                    int output = layer[indexInLayer + stepsToRotateBy];
                    nakayamaPermutation[input] = output;
                }
            }

            return new SelfInjectiveQP<int>(qp, nakayamaPermutation);

            CircularList<int> GetLayerVertices(int layerIndex) // 0-based layer index
            {
                if (layerIndex == 0) return new CircularList<int>(Enumerable.Range(1, numVerticesInCenterPolygon));

                int startVertex = layerIndex * numVerticesInFullLayer - numVerticesInCenterPolygon + 1;
                return new CircularList<int>(Enumerable.Range(startVertex, numVerticesInFullLayer));
            }
        }

        /// <remarks>It is <em>assumed</em> that <em>all</em> odd flower QPs are self-injective.</remarks>
        public SelfInjectiveQP<int> GetSelfInjectiveOddFlowerQP(int numVerticesInCenterPolygon, int firstVertex = DefaultFirstVertex)
        {
            if (numVerticesInCenterPolygon < 3 || numVerticesInCenterPolygon.Modulo(2) == 0) throw new ArgumentOutOfRangeException(nameof(numVerticesInCenterPolygon));

            var qp = UsefulQPs.GetOddFlowerQP(numVerticesInCenterPolygon);

            int numLayers = (numVerticesInCenterPolygon+1) / 2;
            int numVerticesInFullInnerLayer = 2 * numVerticesInCenterPolygon;
            int numVerticesInOuterLayer = 2 * numVerticesInFullInnerLayer;

            // Rotate clockwise by 2*pi / ((numVerticesInCenterPolygon-1)/2) for the Nakayama permutation?
            var nakayamaPermutation = new Dictionary<int, int>();
            for (int layerIndex = 0; layerIndex < numLayers; layerIndex++)
            {
                var layer = GetLayerVertices(layerIndex);
                for (int indexInLayer = 0; indexInLayer < layer.Count; indexInLayer++)
                {
                    int input = layer[indexInLayer];
                    int stepsToRotateBy = (numVerticesInCenterPolygon - 1) / 2 * (layer.Count / numVerticesInCenterPolygon);
                    int output = layer[indexInLayer + stepsToRotateBy];
                    nakayamaPermutation[input] = output;
                }
            }

            return new SelfInjectiveQP<int>(qp, nakayamaPermutation);

            CircularList<int> GetLayerVertices(int layerIndex) // 0-based layer index
            {
                if (layerIndex == 0) return new CircularList<int>(Enumerable.Range(1, numVerticesInCenterPolygon));
                int startVertex = layerIndex * numVerticesInFullInnerLayer - numVerticesInCenterPolygon + 1;
                int numVertices = layerIndex < numLayers - 1 ? numVerticesInFullInnerLayer : numVerticesInOuterLayer;
                return new CircularList<int>(Enumerable.Range(startVertex, numVertices));
            }
        }
    }
}
