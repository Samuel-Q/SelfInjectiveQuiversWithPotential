using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures;
using SelfInjectiveQuiversWithPotential.Plane;

namespace SelfInjectiveQuiversWithPotential.Layer
{
    /// <summary>
    /// This class is used to generate layer quivers &quot;interactively&quot;.
    /// </summary>
    /// <remarks>
    /// <para>By &quot;interactively&quot;, we mean that the quiver generation is carried out one
    /// step at a time, where in each step the generator asks for one composition with specified
    /// parameters and the consumer gives the generator one such composition.</para>
    /// <para>To generate a layer quiver interactively, first call
    /// <see cref="StartGeneration(LayerType)"/>, specifying the layer type; then repeatedly call
    /// <see cref="SupplyCompositionForDistributingExplicitArrowPairsToVertices(Composition)"/> and
    /// <see cref="SupplyCompositionForDistributingArrowsToPolygons(Composition)"/>
    /// (alternating between the two), to specify the vertical arrows, until
    /// <see cref="SupplyCompositionForDistributingArrowsToPolygons(Composition)"/> returns
    /// <see langword="null"/>; and finally call <see cref="EndGeneration"/> to get the layer
    /// quiver that was generated.</para>
    /// <para>For ease of use, the <see cref="SupplyComposition(Composition)"/> method may be used
    /// to supply both kinds of compositions.</para>
    /// </remarks>
    public class InteractiveLayerQuiverGenerator
    {
        private const int DefaultFirstVertex = 1;

        // TODO: The generation started and completed members can mostly be replaced by checking nextCompositionParameters for nullity

        /// <summary>
        /// Indicates whether the generation has been started, with a call to
        /// <see cref="StartGeneration(LayerType, int)"/>.
        /// </summary>
        private bool generationIsStarted = false;

        /// <summary>
        /// Indicates whether the generation is completed.
        /// </summary>
        private bool GenerationIsCompleted {
            get
            {
                if (!generationIsStarted) return false;
                return NumCompositionsSupplied == 2 * layerType.NumLayers - 2;
            }
        } 

        /// <summary>
        /// The layer type of the layer quiver that is currently being generated.
        /// </summary>
        /// <remarks>
        /// <para>If the generation is from a base, the layer type describes the original boundary
        /// layer and the outer layers.</para>
        /// <para>This value is non-<see langword="null"/> whenever <see cref="generationIsStarted"/>
        /// is <see langword="true"/>.</para>
        /// </remarks>
        private LayerType layerType;

        /// <summary>
        /// Gets the number of compositions supplied.
        /// </summary>
        private int NumCompositionsSupplied { get => compositionsSupplied.Count; }

        /// <summary>
        /// The compositions supplied.
        /// </summary>
        /// <remarks><para>Useful for debugging.</para></remarks>
        private List<Composition> compositionsSupplied;

        /// <summary>
        /// The expected composition parameters of the next composition supplied, or
        /// <see langword="null"/> if no composition is expected.
        /// </summary>
        private CompositionParameters expectedCompositionParameters => expectedCompositionParametersStack.Peek();

        /// <summary>
        /// A stack containing the expected composition parameters. The parameters on the top of the
        /// stack are the expected parameters of the composition to be supplied next, while the
        /// other parameters are previously expected composition parameters (used when compositions
        /// are unsupplied).
        /// </summary>
        private Stack<CompositionParameters> expectedCompositionParametersStack;

        /// <summary>
        /// The layers of vertices of the layer quiver.
        /// </summary>
        /// <remarks>
        /// <para>If the layer quiver is generated from a base, only the boundary layer of the base
        /// and the outer layers are recorded in this list.</para>
        /// </remarks>
        private List<CircularList<int>> layersOfVertices;

        private ISet<int> verticesWithImplicitUpArrows;
        private ISet<int> verticesWithImplicitDownArrows;

        // Maps a vertex to the number of explicit arrow pairs that have been assigned to it
        private Dictionary<int, int> explicitArrowPairCounts;

        /// <summary>
        /// A list of arrows added after each &quot;m in k&quot; composition supplied.
        /// </summary>
        /// <remarks>
        /// <para>Keep track of these to make undoing suppletions of compositions a breeze.</para>
        /// </remarks>
        private List<List<Arrow<int>>> arrowsAddedList;

        /// <summary>
        /// A list of the linear combinations of detached cycles (represented as dictionaries)
        /// added to the potential after each &quot;m in k&quot; composition supplied.
        /// </summary>
        /// <remarks>
        /// <para>Keep track of these to make undoing suppletions of compositions a breeze.</para>
        /// </remarks>
        private List<Dictionary<DetachedCycle<int>, int>> linCombsAddedList;

        /// <summary>
        /// The quiver in plane as generated so far.
        /// </summary>
        /// <remarks>
        /// <para>This value is non-<see langword="null"/> whenever <see cref="generationIsStarted"/>
        /// is <see langword="true"/>.</para>
        /// </remarks>
        private QuiverInPlane<int> quiverInPlane;

        /// <summary>
        /// The potential of the QP induced by the quiver in plane.
        /// </summary>
        /// <remarks>
        /// <para>In general, the quiver in plane needs to be drawn with curved arrows to be plane.</para>
        /// </remarks>
        private Potential<int> potential;

        /// <summary>
        /// Initializes the entire quiver in plane for generation, given the base.
        /// </summary>
        /// <param name="quiverInPlane"></param>
        /// <param name="boundaryLayer"></param>
        /// <param name="layerType"></param>
        /// <param name="nextVertex"></param>
        /// <param name="layersOfVertices"></param>
        private void InitializeQuiverInPlane(
            QuiverInPlane<int> quiverInPlane,
            IEnumerable<int> boundaryLayer,
            LayerType layerType,
            int nextVertex,
            out QuiverInPlane<int> quiverInPlaneOut,
            out List<CircularList<int>> layersOfVertices)
        {
            const int RadiusIncrement = 100; // The increment in the radius between one layer and the previous
            double radiusOfBase = quiverInPlane.Vertices.Select(vertex => quiverInPlane.GetVertexPosition(vertex).Radius)
                                                    .Append(0)
                                                    .Min();
            int startRadius = Convert.ToInt32(radiusOfBase) + RadiusIncrement;

            quiverInPlaneOut = quiverInPlane.Copy();
            layersOfVertices = new List<CircularList<int>> { new CircularList<int>(boundaryLayer) };

            int firstVertexInLayer = nextVertex;

            // Add positioned vertices to every new layer
            for (int layerIndex = 1; layerIndex < layerType.NumLayers; layerIndex++)
            {
                var layerSize = layerType.LayerSizes[layerIndex];
                var radius = startRadius + layerIndex * RadiusIncrement;

                var layerVertices = Enumerable.Range(firstVertexInLayer, layerSize).ToCircularList();
                layersOfVertices.Add(layerVertices);

                var vertexPositions = new Dictionary<int, Point>();
                foreach (var vertex in layerVertices)
                {
                    double angle = 2 * Math.PI * (vertex - firstVertexInLayer) / layerVertices.Count();
                    vertexPositions[vertex] = new Point(radius * Math.Cos(angle), radius * Math.Sin(angle));
                }

                foreach (var vertex in layerVertices)
                    quiverInPlaneOut.AddVertex(vertex, vertexPositions[vertex]);

                firstVertexInLayer += layerSize;
            }
        }

        private void CreatePotential(int centerPolygonSize, int firstVertex)
        {
            var centerPolygonArrows = Enumerable.Range(0, centerPolygonSize)
                                                .Select(k => new Arrow<int>(firstVertex + k, firstVertex + (k + 1).Modulo(centerPolygonSize)));
            potential = new Potential<int>(new DetachedCycle<int>(centerPolygonArrows), +1);
        }

        /// <summary>
        /// Starts the generation of a layer quiver.
        /// </summary>
        /// <param name="layerType">The layer type of the layer quiver.</param>
        /// <param name="firstVertex">The first vertex in the layer quiver.</param>
        /// <returns>The parameters of the composition to pass in the first call to
        /// <see cref="SupplyComposition(Composition)"/>, or <see langword="null"/> if the
        /// composition is already completed when this method returns.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="layerType"/> is
        /// <see langword="null"/>.</exception>
        /// <remarks>
        /// <para>Calling this method is equivalent to calling
        /// <see cref="StartGenerationFromBase(QuiverInPlane{int}, Potential{int}, IEnumerable{int}, LayerType, int)"/>
        /// with either a point or a cycle as base.</para>
        /// </remarks>
        public CompositionParameters StartGeneration(LayerType layerType, int firstVertex = DefaultFirstVertex)
        {
            this.layerType = layerType ?? throw new ArgumentNullException(nameof(layerType));

            int firstLayerSize = layerType.LayerSizes[0];
            if (firstLayerSize == 1) return StartGenerationFromPoint(layerType, firstVertex);
            else return StartGenerationFromCycle(layerType, firstVertex);

        }

        private CompositionParameters StartGenerationFromCycle(LayerType layerType, int firstVertex = DefaultFirstVertex)
        {
            int firstLayerSize = layerType.LayerSizes[0];
            var firstLayerVertices = Enumerable.Range(firstVertex, firstLayerSize);
            var centerPolygonArrows = Enumerable.Range(0, firstLayerSize)
                                                .Select(k => new Arrow<int>(firstVertex + k, firstVertex + (k + 1).Modulo(firstLayerSize)));
            const int FirstLayerRadius = 100;
            var vertexPositions = Enumerable.Range(0, firstLayerSize).ToDictionary(k => firstVertex + k, k =>
            {
                double angle = 2 * Math.PI * k / firstLayerSize;
                return new Point(FirstLayerRadius * Math.Cos(angle), FirstLayerRadius * Math.Sin(angle));
            });

            var quiverInPlaneBase = new QuiverInPlane<int>(firstLayerVertices, centerPolygonArrows, vertexPositions);
            var potentialBase = new Potential<int>(new DetachedCycle<int>(centerPolygonArrows), +1);
            return StartGenerationFromBase(
                quiverInPlaneBase,
                potentialBase,
                firstLayerVertices,
                layerType,
                firstVertex + firstLayerSize);
        }

        private CompositionParameters StartGenerationFromPoint(LayerType layerType, int firstVertex = DefaultFirstVertex)
        {
            var vertices = new int[] { firstVertex };
            var arrows = new Arrow<int>[] { };
            var vertexPositions = new Dictionary<int, Point> { { firstVertex, Point.Origin } };
            var quiverInPlaneBase = new QuiverInPlane<int>(vertices, arrows, vertexPositions);
            var potentialBase = new Potential<int>();
            return StartGenerationFromBase(quiverInPlaneBase, potentialBase, vertices, layerType, firstVertex + 1);
        }

        /// <summary>
        /// Starts the generation of a layer quiver from a specified base.
        /// </summary>
        /// <param name="quiverInPlane">The quiver of the plane of the base.</param>
        /// <param name="potential">The potential of the base.</param>
        /// <param name="boundaryLayer">The vertices in the boundary.</param>
        /// <param name="layerType">The layer type of the boundary layer and the outer layers to
        /// generate.</param>
        /// <param name="nextVertex">The first new vertex in the layer quiver to generate.</param>
        /// <returns>The parameters of the composition to pass in the first call to
        /// <see cref="SupplyComposition(Composition)"/>, or <see langword="null"/> if the
        /// composition is already completed when this method returns.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="quiverInPlane"/> is <see langword="null"/>, or
        /// <paramref name="potential"/> is <see langword="null"/>, or
        /// <paramref name="boundaryLayer"/> is <see langword="null"/>, or
        /// <paramref name="layerType"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="potential"/> contains an arrow not
        /// present in <paramref name="quiverInPlane"/>, or
        /// the number of vertices in <paramref name="boundaryLayer"/> is not equal to the first
        /// layer size in <paramref name="layerType"/>, or
        /// <paramref name="boundaryLayer"/> contains a vertex not present in <paramref name="quiverInPlane"/>, or
        /// the consecutive pairs of vertices in <paramref name="boundaryLayer"/> are not all
        /// neighbors in <paramref name="quiverInPlane"/>, or
        /// <paramref name="boundaryLayer"/> contains a vertex with a surplus of more than one
        /// incoming or outgoing arrow, or
        /// the number of vertices in <paramref name="boundaryLayer"/> with a surplus of an incoming arrow
        /// and the number of vertices in <paramref name="boundaryLayer"/> with a surplus of an outgoing arrow are different.</exception>
        /// <exception cref="GeneratorException">The number of explicit arrow pairs for the next layer
        /// (to be specified by the next composition) is negative.</exception>
        /// <remarks>
        /// <para>By &quot;base&quot;, we mean a quiver in the plane that we glue layers onto. A
        /// useful example is the base consisting of a single vertex, in which case the output will
        /// be a &quot;layer quiver with center point&quot;.</para>
        /// <para><paramref name="nextVertex"/> is not validated; it is assumed that
        /// <paramref name="nextVertex"/> and all the following numbers are not vertices in the
        /// base.</para>
        /// </remarks>
        public CompositionParameters StartGenerationFromBase(
            QuiverInPlane<int> quiverInPlane,
            Potential<int> potential,
            IEnumerable<int> boundaryLayer,
            LayerType layerType,
            int nextVertex)
        {
            if (quiverInPlane is null) throw new ArgumentNullException(nameof(quiverInPlane));
            if (potential is null) throw new ArgumentNullException(nameof(potential));
            if (layerType is null) throw new ArgumentNullException(nameof(layerType));
            if (boundaryLayer is null) throw new ArgumentNullException(nameof(boundaryLayer));

            InternalStartGenerationFromBase(quiverInPlane, potential, boundaryLayer, layerType, nextVertex, out var nextCompositionParameters, shouldThrow: true);
            return nextCompositionParameters;
        }

        public bool TryStartGenerationFromBase(
            QuiverInPlane<int> quiverInPlane,
            Potential<int> potential,
            IEnumerable<int> boundaryLayer,
            LayerType layerType,
            int nextVertex,
            out CompositionParameters nextCompositionParameters)
        {
            if (quiverInPlane is null) throw new ArgumentNullException(nameof(quiverInPlane));
            if (potential is null) throw new ArgumentNullException(nameof(potential));
            if (layerType is null) throw new ArgumentNullException(nameof(layerType));
            if (boundaryLayer is null) throw new ArgumentNullException(nameof(boundaryLayer));

            return InternalStartGenerationFromBase(quiverInPlane, potential, boundaryLayer, layerType, nextVertex, out nextCompositionParameters, shouldThrow: false);
        }

        private bool InternalStartGenerationFromBase(
            QuiverInPlane<int> quiverInPlane,
            Potential<int> potential,
            IEnumerable<int> boundaryLayer,
            LayerType layerType,
            int nextVertex,
            out CompositionParameters nextCompositionParameters,
            bool shouldThrow)
        {
            this.potential = potential;
            this.layerType = layerType;

            if (potential.LinearCombinationOfCycles.Elements.SelectMany(cycle => cycle.Arrows).Any(arrow => !quiverInPlane.ContainsArrow(arrow)))
                throw new ArgumentException($"The quiver in plane does not contain all the arrows in the potential.");

            if (boundaryLayer.Count() != layerType.LayerSizes[0])
                throw new ArgumentException($"The sizes of the boundary layer and the first layer are different " +
                    $"({boundaryLayer.Count()} and {layerType.LayerSizes[0]}, respectively).");

            if (boundaryLayer.Any(vertex => !quiverInPlane.ContainsVertex(vertex)))
                throw new ArgumentException($"The quiver in plane does not contain all the vertices in the boundary layer.");

            // TODO: Should probably disallow boundary layer count = 2
            if (boundaryLayer.Count() > 1)
            {
                if (boundaryLayer.Append(boundaryLayer.First()).AdjacentPairs().Any(pair =>
                {
                    var vertex1 = pair.Item1;
                    var vertex2 = pair.Item2;
                    return !quiverInPlane.ContainsArrow(vertex1, vertex2) && !quiverInPlane.ContainsArrow(vertex2, vertex1);
                }))
                {
                    throw new ArgumentException($"The consecutive vertices in the boundary layer are not all neighbors.");
                }
            }

            InitializeQuiverInPlane(quiverInPlane, boundaryLayer, layerType, nextVertex, out this.quiverInPlane, out layersOfVertices);

            explicitArrowPairCounts = new Dictionary<int, int>();
            verticesWithImplicitUpArrows = new HashSet<int>();
            verticesWithImplicitDownArrows = new HashSet<int>();

            int numVerticesWithOutgoingArrowSurplus = 0;
            int numVerticesWithIncomingArrowSurplus = 0;
            foreach (var vertex in boundaryLayer)
            {
                // outgoingArrowSurplus := outgoing - incoming for vertex
                int outgoingArrowSurplus = quiverInPlane.AdjacencyLists[vertex].Count - quiverInPlane.Vertices.Count(vertex2 => quiverInPlane.AdjacencyLists[vertex2].Contains(vertex));

                if (outgoingArrowSurplus < -1) throw new ArgumentException($"The boundary layer contains a vertex {vertex} with a surplus of more than one incoming arrow.");
                else if (outgoingArrowSurplus > 1) throw new ArgumentException($"The boundary layer contains a vertex {vertex} with a surplus of more than one outgoing arrow.");
                // Don't overthink this; the surplus should be 0 after the arrows between the boundary layer and the next
                // so surplus of -1 means that we need an outgoing arrow (an up arrow)
                else if (outgoingArrowSurplus == -1)
                {
                    verticesWithImplicitUpArrows.Add(vertex);
                    numVerticesWithIncomingArrowSurplus += 1;
                }
                else if (outgoingArrowSurplus == 1)
                {
                    verticesWithImplicitDownArrows.Add(vertex);
                    numVerticesWithOutgoingArrowSurplus += 1;
                }
            }

            if (numVerticesWithOutgoingArrowSurplus != numVerticesWithIncomingArrowSurplus)
            {
                throw new ArgumentException($"The surpluses of outgoing and incoming arrows " +
                    $"({numVerticesWithOutgoingArrowSurplus} and {numVerticesWithIncomingArrowSurplus}, respectively) " +
                    $"for the boundary layer are not the same.");
            }
            int numImplicitArrowPairs = numVerticesWithOutgoingArrowSurplus;

            arrowsAddedList = new List<List<Arrow<int>>>();
            linCombsAddedList = new List<Dictionary<DetachedCycle<int>, int>>();

            compositionsSupplied = new List<Composition>();
            expectedCompositionParametersStack = new Stack<CompositionParameters>();
            generationIsStarted = true;

            if (layerType.NumLayers == 1)
            {
                nextCompositionParameters = null;
                expectedCompositionParametersStack.Push(nextCompositionParameters);
                return true;
            }

            int numNextExplicitArrowPairs = layerType.VerticalArrowPairCounts[0] - numImplicitArrowPairs;
            if (numNextExplicitArrowPairs < 0)
            {
                generationIsStarted = false;
                if (shouldThrow)
                    throw new GeneratorException("The given composition resulted in a " +
                        "negative number of explicit arrows for the next layer.");

                nextCompositionParameters = null;
                return false;
            }

            int nextLayerSize = layerType.LayerSizes[0];
            nextCompositionParameters = new CompositionParameters(numNextExplicitArrowPairs + nextLayerSize, nextLayerSize);
            expectedCompositionParametersStack.Push(nextCompositionParameters);
            return true;
        }

        /// <summary>
        /// Supplies the generator with one composition.
        /// </summary>
        /// <param name="composition">The composition to supply the generator with.</param>
        /// <returns>The parameters of the next composition to supply to the generator,
        /// or <see langword="null"/> if the generation is completed when this method returns.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="composition"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">The generation is not started, or the
        /// generation is completed.</exception>
        /// <exception cref="ArgumentException">The parameters of the composition are not equal to
        /// the expected composition parameters.</exception>
        /// <exception cref="GeneratorException"><paramref name="composition"/> results in too
        /// large a number of polygons to which to distribute the arrows in the next layer, or
        /// <paramref name="composition"/> results in a negative number of explicit arrow pairs for
        /// the next layer.</exception>
        public CompositionParameters SupplyComposition(Composition composition)
        {
            if (composition is null) throw new ArgumentNullException(nameof(composition));
            if (!generationIsStarted) throw new InvalidOperationException("The generation is not started.");
            if (GenerationIsCompleted) throw new InvalidOperationException("The generation is completed.");
            if (!composition.Parameters.Equals(expectedCompositionParameters))
                throw new ArgumentException($"The parameters of the composition ({composition.Parameters}) " +
                    $"are not equal to the expected parameters ({expectedCompositionParameters}).");

            if (NumCompositionsSupplied.IsEven()) return SupplyCompositionForDistributingExplicitArrowPairsToVertices(composition);
            else return SupplyCompositionForDistributingArrowsToPolygons(composition);
        }

        /// <summary>
        /// Supplies the generator with one composition.
        /// </summary>
        /// <param name="composition">The composition to supply the generator with.</param>
        /// <param name="nextCompositionParameters">Output parameter for the parameters of the next
        /// composition to supply to the generator. If the generation is completed when this method
        /// returns, <see langword="null"/> is passed through the output parameter.</param>
        /// <returns><see langword="true"/> if the composition is supplied successfully;
        /// <see langword="false"/> otherwise, i.e., if the suppletion resulted in too
        /// large a number of polygons to which to distribute the arrows in the next layer or if
        /// the suppletion resulted in a negative number of explicit arrow pairs for the next layer.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="composition"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">The generation is not started, or the
        /// generation is completed.</exception>
        /// <exception cref="ArgumentException">The parameters of the composition are not equal to
        /// the expected composition parameters.</exception>
        public bool TrySupplyComposition(Composition composition, out CompositionParameters nextCompositionParameters)
        {
            if (composition is null) throw new ArgumentNullException(nameof(composition));
            if (!generationIsStarted) throw new InvalidOperationException("The generation is not started.");
            if (GenerationIsCompleted) throw new InvalidOperationException("The generation is completed.");
            if (!composition.Parameters.Equals(expectedCompositionParameters))
                throw new ArgumentException($"The parameters of the composition ({composition.Parameters}) " +
                    $"are not equal to the expected parameters ({expectedCompositionParameters}).");

            if (NumCompositionsSupplied.IsEven())
            {
                return TrySupplyCompositionForDistributingExplicitArrowPairsToVertices(composition, out nextCompositionParameters);
            }
            else
            {
                return TrySupplyCompositionForDistributingArrowsToPolygons(composition, out nextCompositionParameters);
            }
        }

        /// <summary>
        /// Supplies the generator with a &quot;<c>k</c> in <c>m</c>&quot; composition, distributing
        /// the explicit arrow pairs between a layer and the next to the vertices in lower layer.
        /// </summary>
        /// <param name="composition">The composition specifying the number of explicit arrow pairs
        /// that every vertex in the lower layer gets.</param>
        /// <param name="nextCompositionParameters">Output parameter for the parameters of the
        /// composition to pass in the next call to
        /// <see cref="SupplyCompositionForDistributingArrowsToPolygons(Composition)"/>,
        /// <see cref="TrySupplyCompositionForDistributingArrowsToPolygons(Composition, out CompositionParameters)"/>,
        /// or <see cref="SupplyComposition(Composition)"/>.</param>
        /// <param name="shouldThrow">Indicates whether a <see cref="GeneratorException"/> should
        /// be thrown instead of returning <see langword="false"/> when an error occurs.</param>
        /// <returns><see langword="true"/> if the composition was successfully supplied to the
        /// generator; <see langword="false"/> otherwise, i.e., if the suppletion resulted in too
        /// large a number of polygons to which to distribute the arrows in the next layer (to be
        /// specified by the next composition).</returns>
        /// <remarks>
        /// <para><paramref name="shouldThrow"/> indicates only whether the method should throw
        /// <see cref="GeneratorException"/>s; other exceptions are thrown regardless.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="composition"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">The generation is not started, or the
        /// generation is completed, or the number of compositions supplied is odd.</exception>
        /// <exception cref="ArgumentException">The parameters of the composition are not equal to
        /// the expected composition parameters.</exception>
        /// <exception cref="GeneratorException"><paramref name="shouldThrow"/> is
        /// <see langword="true"/> and <paramref name="composition"/> results in too large a number
        /// of polygons to which to distribute the arrows in the next layer (to be specified by the
        /// next composition).</exception>
        private bool InternalSupplyCompositionForDistributingExplicitArrowPairsToVertices(
            Composition composition,
            out CompositionParameters nextCompositionParameters,
            bool shouldThrow)
        {
            if (composition is null) throw new ArgumentNullException(nameof(composition));
            if (!generationIsStarted) throw new InvalidOperationException("The generation is not started.");
            if (GenerationIsCompleted) throw new InvalidOperationException("The generation is completed.");
            if (NumCompositionsSupplied.IsOdd()) throw new InvalidOperationException($"Expected a call to " +
                $"{nameof(SupplyCompositionForDistributingArrowsToPolygons)}, not " +
                $"{nameof(SupplyCompositionForDistributingExplicitArrowPairsToVertices)}.");
            if (!composition.Parameters.Equals(expectedCompositionParameters))
                throw new ArgumentException($"The parameters of the composition ({composition.Parameters}) " +
                    $"are not equal to the expected parameters ({expectedCompositionParameters}).");

            int curParameterIndex = NumCompositionsSupplied / 2; // zero-based
            int curLayerIndex = curParameterIndex;
            var curLayer = layersOfVertices[curParameterIndex];

            // Every vertex can have zero explicit arrow pairs, so decode by subtracting 1 everywhere
            var explicitArrowPairCountsEnumerable = composition.Terms.Select(term => term - 1);
            int numZerosAllowed = 0; // The "r value" r_i
            foreach (var (vertex, explicitArrowPairCount) in curLayer.Zip(explicitArrowPairCountsEnumerable, (vertex, count) => (vertex, count)))
            {
                explicitArrowPairCounts[vertex] = explicitArrowPairCount;

                // Every vertex with at least one vertical arrow (explicit or implicit) contributes by 1 to the r value,
                // as long as there are at least two vertices in the layer and at least two arrow pairs in total I think
                if (layerType.LayerSizes[curParameterIndex] >= 2 &&
                    layerType.VerticalArrowPairCounts[curParameterIndex] >= 2 &&
                    (explicitArrowPairCount > 0 || verticesWithImplicitUpArrows.Contains(vertex) || verticesWithImplicitDownArrows.Contains(vertex)))
                {
                    numZerosAllowed += 1;
                }
            }

            compositionsSupplied.Add(composition);

            // The next "m in k" composition should be (m_(i+1) + r_i in 2*k_i)
            int sum = layerType.LayerSizes[curParameterIndex + 1] + numZerosAllowed;
            int numTerms = 2 * layerType.VerticalArrowPairCounts[curParameterIndex];
            if (sum < numTerms)
            {
                InternalUnsupplyLastComposition(popCompositionParametersStack: false);

                if (shouldThrow) throw new GeneratorException("The given composition resulted in " +
                    "too large a number of polygons to which to distribute the arrows.");

                nextCompositionParameters = null;
                return false;
            }

            nextCompositionParameters = new CompositionParameters(sum, numTerms);
            expectedCompositionParametersStack.Push(nextCompositionParameters);
            return true;
        }

        /// <summary>
        /// Supplies the generator with a &quot;<c>k</c> in <c>m</c>&quot; composition, distributing
        /// the explicit arrow pairs between a layer and the next to the vertices in lower layer.
        /// </summary>
        /// <param name="composition">The composition specifying the number of explicit arrow pairs
        /// that every vertex in the lower layer gets.</param>
        /// <returns>The parameters of the composition to pass in the next call to
        /// <see cref="SupplyCompositionForDistributingArrowsToPolygons(Composition)"/>,
        /// <see cref="TrySupplyCompositionForDistributingArrowsToPolygons(Composition, out CompositionParameters)"/>,
        /// or <see cref="SupplyComposition(Composition)"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="composition"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">The generation is not started, or the
        /// generation is completed, or the number of compositions supplied is odd (indicating that
        /// <see cref="SupplyCompositionForDistributingArrowsToPolygons(Composition)"/> should be
        /// called instead).</exception>
        /// <exception cref="GeneratorException"><paramref name="composition"/> results in too
        /// large a number of polygons to which to distribute the arrows in the next layer (to be
        /// specified by the next composition).</exception>
        public CompositionParameters SupplyCompositionForDistributingExplicitArrowPairsToVertices(Composition composition)
        {
            InternalSupplyCompositionForDistributingExplicitArrowPairsToVertices(
                composition,
                out var nextCompositionParameters,
                shouldThrow: true);

            return nextCompositionParameters;
        }

        /// <summary>
        /// Supplies the generator with a &quot;<c>k</c> in <c>m</c>&quot; composition, distributing
        /// the explicit arrow pairs between a layer and the next to the vertices in lower layer.
        /// </summary>
        /// <param name="composition">The composition specifying the number of explicit arrow pairs
        /// that every vertex in the lower layer gets.</param>
        /// <param name="nextCompositionParameters">Output parameter for the parameters of the
        /// composition to pass in the next call to
        /// <see cref="SupplyCompositionForDistributingArrowsToPolygons(Composition)"/>,
        /// <see cref="TrySupplyCompositionForDistributingArrowsToPolygons(Composition, out CompositionParameters)"/>,
        /// or <see cref="SupplyComposition(Composition)"/>.</param>
        /// <returns><see langword="true"/> if the composition was successfully supplied to the
        /// generator; <see langword="false"/> otherwise, i.e., if the suppletion resulted in too
        /// large a number of polygons to which to distribute the arrows in the next layer (to be
        /// specified by the next composition).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="composition"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">The generation is not started, or the
        /// generation is completed, or the number of compositions supplied is odd (indicating that
        /// <see cref="TrySupplyCompositionForDistributingArrowsToPolygons(Composition)"/> should be
        /// called instead).</exception>
        public bool TrySupplyCompositionForDistributingExplicitArrowPairsToVertices(
            Composition composition,
            out CompositionParameters nextCompositionParameters)
        {
            return InternalSupplyCompositionForDistributingExplicitArrowPairsToVertices(
                composition,
                out nextCompositionParameters,
                shouldThrow: false);
        }

        /// <summary>
        /// Supplies the generator with an &quot;<c>m</c> in <c>k</c>&quot; composition, distributing
        /// the edges in the upper layer to the polygons between the lower and upper layer.
        /// </summary>
        /// <param name="composition">The composition specifying the number of &quot;skips&quot;
        /// and &quot;steps&quot;.</param>
        /// <param name="nextCompositionParameters">Output parameter for the parameters of the
        /// composition to pass in the next call to
        /// <see cref="SupplyCompositionForDistributingExplicitArrowPairsToVertices(Composition)"/>,
        /// <see cref="TrySupplyCompositionForDistributingArrowsToPolygons(Composition, out CompositionParameters)"/>,
        /// or <see cref="SupplyComposition(Composition)"/>. If the generation is completed when
        /// this method returns, the output parameter is assigned <see langword="null"/>.</param>
        /// <param name="shouldThrow">Indicates whether a <see cref="GeneratorException"/> should
        /// be thrown instead of returning <see langword="false"/> when an error occurs.</param>
        /// <returns><see langword="true"/> if the composition was successfully supplied to the
        /// generator; <see langword="false"/> otherwise, i.e., if the suppletion resulted in a
        /// negative number of explicit arrow pairs for the next layer.</returns>
        /// <remarks>
        /// <para><paramref name="shouldThrow"/> indicates only whether the method should throw
        /// <see cref="GeneratorException"/>s; other exceptions are thrown regardless.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"><paramref name="composition"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">The generation is not started, or the
        /// generation is completed, or the number of compositions supplied is even.</exception>
        /// <exception cref="ArgumentException">The parameters of the composition are not equal to
        /// the expected composition parameters.</exception>
        /// <exception cref="GeneratorException"><paramref name="shouldThrow"/> is
        /// <see langword="true"/> and <paramref name="composition"/> results in a negative number
        /// of explicit arrow pairs for the next layer (to be specified by the next composition).
        /// </exception>
        /// <remarks>
        /// <para>In some more detail, a composition <c>a1 + a2 + ...</c> is decoded into a
        /// composition with some zero terms allowed <c>b1 + b2 + ...</c> with the same number of
        /// terms as the &quot;encoded&quot; composition, where <c>bi = ai</c> or
        /// <c>bi = ai - 1</c>, according to some rules (that should be written down but are a
        /// little bit complicated).</para>
        /// <para>The terms of the decoded composition <c>b1 + b2 + ...</c> should be thought of as
        /// occurring in pairs <c>(b1 + b2) + (b3 + b4) + ...</c>. The pair <c>b1 + b2</c>
        /// specifies that for the first up arrow from the current layer to the next (starting from
        /// the first vertex of the layer until an up arrow, be it implicit or explicit, is
        /// encountered) (note that explicit up arrows are encountered before implicit up arrows
        /// for a given vertex: implicit up arrows are thought of as being as far
        /// &quot;to the right&quot; as possible. On the other hand, implicit down arrows are
        /// thought of as being as far &quot;to the left&quot; as possible and are hence the first
        /// arrow into a vertex), <c>b1</c> arrows in the outer layer should be consumed to the
        /// left of the target of the up arrow before returning to the inner layer and forming a
        /// polygon, and <c>b2</c> arrows in the outer layer should be consumed to the right of the
        /// target of the up arrow before returning to the inner layer and forming a polygon.</para>
        /// <para>Previously, the terms were cyclically permuted: <c>b2 + b3 + b4 + ... + b1</c>.
        /// This convention seemed less useful but lives on in my physical notepad (prior to
        /// 2018-10-29) and possibly in other comments or documentation in the codebase.</para>
        /// </remarks>
        private bool InternalSupplyCompositionForDistributingArrowsToPolygons(
            Composition composition,
            out CompositionParameters nextCompositionParameters,
            bool shouldThrow)
        {
            if (composition is null) throw new ArgumentNullException(nameof(composition));
            if (!generationIsStarted) throw new InvalidOperationException("The generation is not started.");
            if (GenerationIsCompleted) throw new InvalidOperationException("The generation is completed.");
            if (NumCompositionsSupplied.IsEven()) throw new InvalidOperationException($"Expected a call to " +
                $"{nameof(SupplyCompositionForDistributingExplicitArrowPairsToVertices)}, not " +
                $"{nameof(SupplyCompositionForDistributingArrowsToPolygons)}.");
            if (!composition.Parameters.Equals(expectedCompositionParameters))
                throw new ArgumentException($"The parameters of the composition ({composition.Parameters}) " +
                    $"are not equal to the expected parameters ({expectedCompositionParameters}).");

            int curParameterIndex = NumCompositionsSupplied / 2; // zero-based
            int curLayerIndex = curParameterIndex;
            var curLayer = layersOfVertices[curParameterIndex];
            var nextLayer = layersOfVertices[curParameterIndex + 1];

            // Associates to every vertex a value +1 for implicit up arrow, -1 for implicit down arrow,
            // or 0 for no implicit arrows
            var nextLayerImplicitUpArrowCounts = nextLayer.ToDictionary(vertex => vertex, vertex => 0);

            int indexInNextLayer = 0;
            int indexInComposition = 0;
            var arrowsToAdd = new List<Arrow<int>>();
            var linCombsToAdd = new Dictionary<DetachedCycle<int>, int>();

            // These three variables are used to record all the polygons/cycles for the potential.

            // The arrows up and to the left for the first up arrow in the layer.
            // We won't know where to go back down to the lower layer until the loop is done, so
            // store the arrows until then.
            IEnumerable<Arrow<int>> storedUpLeftArrows = null;

            // The arrows up and to the right for implicit up arrows.
            // We won't know where to go back down to the lower layer until we encounter the next
            // vertical arrow (which will be the corresponding implicit down arrow).
            IEnumerable<Arrow<int>> storedUpRightArrows = null;

            // For use with storedUpLeftArrows
            Arrow<int> lastDownArrow = null;

            // Careful with degenerate cases
            bool layerHasMoreThanOneVertexAndVerticalArrowPair =
                layerType.LayerSizes[curParameterIndex] > 1 && layerType.VerticalArrowPairCounts[curParameterIndex] > 1;

            // Store data for a down-arrow in order to handle it after the loop, if it is the first
            // arrow in the entire layer.
            var storedDownArrowData = (IndexInCurLayer: default(int), IndexInNextLayer: default(int));

            // Remark: The first vertical arrow encountered in a layer is not necessarily an up arrow.
            // I erroneously thought the contrary for a while.
            // (This necessitates the use of lastUpArrow
            // (if I've thought correctly) an up arrow (implicit or explicit).
            // In other words, an implicit down arrow is never the first arrow.
            // This is important to guarantee that storedUpRightArrows will
            // always be non-null when handling an implicit down arrow.
            for (int indexInCurLayer = 0; indexInCurLayer < curLayer.Count; indexInCurLayer++)
            {
                var curVertex = curLayer[indexInCurLayer];

                bool hasLoneDownArrow = verticesWithImplicitDownArrows.Contains(curVertex);
                bool hasLoneUpArrow = verticesWithImplicitUpArrows.Contains(curVertex);
                int numExplicitArrowPairCountsToHandle = explicitArrowPairCounts[curVertex];

                // If there was a lone up arrow and this vertex has a down arrow but no implicit
                // down arrow, then the explicit pairs are down-up pairs instead of up-down pairs.

                // Same thing can happen for the first vertex with explicit pairs encountered in
                // the layer, and then the implicit up arrow might not have been encountered yet.
                // That is, if the first implicit arrow is a down arrow and there are explicit
                // pairs before that, the explicit pairs are down-up pairs.

                // Solve this down-up issue by repairing the arrows as
                // "down, up-down, ..., up-down, up", thereby introducing two lone arrows (the
                // first down and the last up) and reducing the number of normal explicit arrow
                // pairs by 1.

                bool nextImplicitArrowIsDownArrow =
                    Enumerable.Range(indexInCurLayer + 1, curLayer.Count) // Start from the next vertex (because implicit down arrow is first)
                              .Select(index => curLayer[index])
                              .TryFirst(
                                  vertex => verticesWithImplicitDownArrows.Contains(vertex),
                                  out var vertexWithImplicitDownArrow,
                                  out int indexForImplicitDownArrow
                              ) &&
                    Enumerable.Range(indexInCurLayer, curLayer.Count) // Start from this vertex (because implicit up arrow is last)
                              .Select(index => curLayer[index])
                              .TryFirst(
                                  vertex => verticesWithImplicitUpArrows.Contains(vertex),
                                  out var vertexWithImplicitUpArrow,
                                  out int indexForImplicitUpArrow
                              ) &&
                              indexForImplicitDownArrow < indexForImplicitUpArrow;

                if (numExplicitArrowPairCountsToHandle > 0 && nextImplicitArrowIsDownArrow)
                {
                    hasLoneDownArrow = true;
                    System.Diagnostics.Debug.Assert(!hasLoneUpArrow, "Lone up arrow was unexpected.");
                    hasLoneUpArrow = true;
                    numExplicitArrowPairCountsToHandle -= 1;
                }

                if (hasLoneDownArrow)
                {
                    // If the first arrow encountered in the entire layer is a (lone) down arrow,
                    // then we do not have any stored up-right arrows to form a polygon with, so
                    // record this lone down arrow for handling at the end.
                    // For all other lone down arrows, just handle it.
                    if (storedUpRightArrows is null)
                    {
                        storedDownArrowData = (indexInCurLayer, indexInNextLayer);
                        lastDownArrow = new Arrow<int>(nextLayer[indexInNextLayer], curLayer[indexInCurLayer]);
                    }
                    else HandleLoneDownArrow(curLayer, nextLayer, indexInCurLayer, indexInNextLayer, nextLayerImplicitUpArrowCounts, arrowsToAdd, linCombsToAdd, ref storedUpRightArrows, ref lastDownArrow);
                }
                HandleExplicitArrowPairs(curLayer, nextLayer, indexInCurLayer, ref indexInNextLayer, numExplicitArrowPairCountsToHandle, composition, ref indexInComposition, nextLayerImplicitUpArrowCounts, arrowsToAdd, linCombsToAdd, ref storedUpLeftArrows, ref storedUpRightArrows, ref lastDownArrow, hasLoneDownArrow, layerHasMoreThanOneVertexAndVerticalArrowPair);
                if (hasLoneUpArrow) HandleLoneUpArrow(curLayer, nextLayer, indexInCurLayer, ref indexInNextLayer, composition, ref indexInComposition, nextLayerImplicitUpArrowCounts, arrowsToAdd, linCombsToAdd, ref storedUpLeftArrows, ref storedUpRightArrows, ref lastDownArrow, layerHasMoreThanOneVertexAndVerticalArrowPair);
            }

            // Add the left/counterclockwise cycle for the first up arrow, if the first arrow in the layer was an up arrow
            if (storedUpLeftArrows != null)
            {
                // Remark: The rightmost vertex and the leftmost vertex can coincide
                // (in which case we want the empty path in the lower layer).
                var leftmostVertex = lastDownArrow.Target;
                var rightmostVertex = storedUpLeftArrows.First().Source;
                var rightmostVertexIndex = curLayer.IndexOf(rightmostVertex); // Hacky
                var cycleVerticesInCurLayer = Utility.InfiniteRange(rightmostVertexIndex, step: -1)
                                                     .Select(index => curLayer[index])
                                                     .TakeWhile(vertex => vertex != leftmostVertex)
                                                     .Append(leftmostVertex)
                                                     .Reverse();
                var cycleArrowsInCurLayer = cycleVerticesInCurLayer.AdjacentPairs()
                                                                   .Select(pair => new Arrow<int>(pair.Item1, pair.Item2));
                var cycleArrows = storedUpLeftArrows.Append(lastDownArrow).Concat(cycleArrowsInCurLayer);
                linCombsToAdd[new DetachedCycle<int>(cycleArrows)] = -1;
            }
            // Add the right/clockwise cycle for the first down arrow, if the first arrow in the layer was a down arrow
            else
            {
                HandleLoneDownArrow(curLayer, nextLayer, storedDownArrowData.IndexInCurLayer, storedDownArrowData.IndexInNextLayer, nextLayerImplicitUpArrowCounts, arrowsToAdd, linCombsToAdd, ref storedUpRightArrows, ref lastDownArrow);
            }

            // Record which vertices in the next layer ended up with implicit up or down arrows
            // Also tally the number of implicit arrow pairs (which are not to be considered for
            // distribution among the vertices of the next layer), which is the l_i value in my
            // notes.
            int numImplicitArrowPairs = 0;
            foreach (var (vertex, implicitUpArrowCount) in nextLayerImplicitUpArrowCounts)
            {
                if (implicitUpArrowCount == 1)
                {
                    verticesWithImplicitUpArrows.Add(vertex);
                    numImplicitArrowPairs += 1;
                }
                else if (implicitUpArrowCount == -1) verticesWithImplicitDownArrows.Add(vertex);
            }

            foreach (var arrow in arrowsToAdd) quiverInPlane.AddArrow(arrow);
            arrowsAddedList.Add(arrowsToAdd);

            foreach (var (cycle, coefficient) in linCombsToAdd) potential = potential.AddCycle(cycle, coefficient);
            linCombsAddedList.Add(linCombsToAdd);

            compositionsSupplied.Add(composition);

            // Return the next composition parameters
            if (GenerationIsCompleted)
            {
                nextCompositionParameters = null;
                expectedCompositionParametersStack.Push(nextCompositionParameters);
                return true;
            }

            // The next "k in m" composition should be (k_(i+1) - l_(i+1) in m_(i+1)) as a
            // composition with zero terms allowed and (k_(i+1) - l_(i+1) + m_(i+1) in m_(i+1))
            // without zero terms allowed.
            int nextParameterIndex = curParameterIndex + 1;
            int numNextExplicitArrowPairs = layerType.VerticalArrowPairCounts[nextParameterIndex] - numImplicitArrowPairs;
            if (numNextExplicitArrowPairs < 0)
            {
                InternalUnsupplyLastComposition(popCompositionParametersStack: false);

                if (shouldThrow) throw new GeneratorException("The given composition resulted in a " +
                    "negative number of explicit arrows for the next layer.");

                nextCompositionParameters = null;
                return false;
            }

            nextCompositionParameters = new CompositionParameters(numNextExplicitArrowPairs + nextLayer.Count, nextLayer.Count);
            expectedCompositionParametersStack.Push(nextCompositionParameters);
            return true;
        }

        /// <summary>
        /// Supplies the generator with an &quot;<c>m</c> in <c>k</c>&quot; composition, distributing
        /// the edges in the upper layer to the polygons between the lower and upper layer.
        /// </summary>
        /// <param name="composition">The composition specifying the number of &quot;skips&quot;
        /// and &quot;steps&quot;.</param>
        /// <returns>The parameters of the composition to pass in the next call to
        /// <see cref="SupplyCompositionForDistributingExplicitArrowPairsToVertices(Composition)"/>,
        /// <see cref="TrySupplyCompositionForDistributingExplicitArrowPairsToVertices(Composition, out CompositionParameters)"/>,
        /// or <see cref="SupplyComposition(Composition)"/>. If the generation is completed when
        /// this method returns, <see langword="null"/> is returned.</returns>
        /// <exception cref="GeneratorException"><paramref name="composition"/> results in a
        /// negative number of explicit arrow pairs for the next layer.</exception>
        /// <remarks>
        /// <para>In some more detail, a composition <c>a1 + a2 + ...</c> is decoded into a
        /// composition with some zero terms allowed <c>b1 + b2 + ...</c> with the same number of
        /// terms as the &quot;encoded&quot; composition, where <c>bi = ai</c> or
        /// <c>bi = ai - 1</c>, according to some rules (that should be written down but are a
        /// little bit complicated).</para>
        /// <para>The terms of the decoded composition <c>b1 + b2 + ...</c> should be thought of as
        /// occurring in pairs <c>(b1 + b2) + (b3 + b4) + ...</c>. The pair <c>b1 + b2</c>
        /// specifies that for the first up arrow from the current layer to the next (starting from
        /// the first vertex of the layer until an up arrow, be it implicit or explicit, is
        /// encountered) (note that explicit up arrows are encountered before implicit up arrows
        /// for a given vertex: implicit up arrows are thought of as being as far
        /// &quot;to the right&quot; as possible. On the other hand, implicit down arrows are
        /// thought of as being as far &quot;to the left&quot; as possible and are hence the first
        /// arrow into a vertex), <c>b1</c> arrows in the outer layer should be consumed to the
        /// left of the target of the up arrow before returning to the inner layer and forming a
        /// polygon, and <c>b2</c> arrows in the outer layer should be consumed to the right of the
        /// target of the up arrow before returning to the inner layer and forming a polygon.</para>
        /// <para>Previously, the terms were cyclically permuted: <c>b2 + b3 + b4 + ... + b1</c>.
        /// This convention seemed less useful but lives on in my physical notepad (prior to
        /// 2018-10-29) and possibly in other comments or documentation in the codebase.</para>
        /// </remarks>
        public CompositionParameters SupplyCompositionForDistributingArrowsToPolygons(Composition composition)
        {
            InternalSupplyCompositionForDistributingArrowsToPolygons(
                composition,
                out var nextCompositionParameters,
                shouldThrow: true);
            return nextCompositionParameters;
        }

        /// <summary>
        /// Supplies the generator with an &quot;<c>m</c> in <c>k</c>&quot; composition, distributing
        /// the edges in the upper layer to the polygons between the lower and upper layer.
        /// </summary>
        /// <param name="composition">The composition specifying the number of &quot;skips&quot;
        /// and &quot;steps&quot;.</param>
        /// <param name="nextCompositionParameters">Output parameter for the parameters of the
        /// composition to pass in the next call to
        /// <see cref="SupplyCompositionForDistributingExplicitArrowPairsToVertices(Composition)"/>,
        /// <see cref="TrySupplyCompositionForDistributingExplicitArrowPairsToVertices(Composition, out CompositionParameters)"/>,
        /// or <see cref="SupplyComposition(Composition)"/>. If the generation is completed when
        /// this method returns, the output parameter is assigned <see langword="null"/>.</param>
        /// <returns></returns>
        /// <para>In some more detail, a composition <c>a1 + a2 + ...</c> is decoded into a
        /// composition with some zero terms allowed <c>b1 + b2 + ...</c> with the same number of
        /// terms as the &quot;encoded&quot; composition, where <c>bi = ai</c> or
        /// <c>bi = ai - 1</c>, according to some rules (that should be written down but are a
        /// little bit complicated).</para>
        /// <para>The terms of the decoded composition <c>b1 + b2 + ...</c> should be thought of as
        /// occurring in pairs <c>(b1 + b2) + (b3 + b4) + ...</c>. The pair <c>b1 + b2</c>
        /// specifies that for the first up arrow from the current layer to the next (starting from
        /// the first vertex of the layer until an up arrow, be it implicit or explicit, is
        /// encountered) (note that explicit up arrows are encountered before implicit up arrows
        /// for a given vertex: implicit up arrows are thought of as being as far
        /// &quot;to the right&quot; as possible. On the other hand, implicit down arrows are
        /// thought of as being as far &quot;to the left&quot; as possible and are hence the first
        /// arrow into a vertex), <c>b1</c> arrows in the outer layer should be consumed to the
        /// left of the target of the up arrow before returning to the inner layer and forming a
        /// polygon, and <c>b2</c> arrows in the outer layer should be consumed to the right of the
        /// target of the up arrow before returning to the inner layer and forming a polygon.</para>
        /// <para>Previously, the terms were cyclically permuted: <c>b2 + b3 + b4 + ... + b1</c>.
        /// This convention seemed less useful but lives on in my physical notepad (prior to
        /// 2018-10-29) and possibly in other comments or documentation in the codebase.</para>
        /// </remarks>
        public bool TrySupplyCompositionForDistributingArrowsToPolygons(
            Composition composition,
            out CompositionParameters nextCompositionParameters)
        {
            return InternalSupplyCompositionForDistributingArrowsToPolygons(
                composition,
                out nextCompositionParameters,
                shouldThrow: false);
        }

        private int AddUpLeftAndRightArrows(
            CircularList<int> curLayer,
            CircularList<int> nextLayer,
            int indexInCurLayer,
            ref int indexInNextLayerRef,
            Composition composition,
            int indexInComposition,
            Dictionary<int, int> nextLayerImplicitUpArrowCounts,
            List<Arrow<int>> arrowsToAdd,
            Dictionary<DetachedCycle<int>, int> linCombsToAdd,
            ref IEnumerable<Arrow<int>> storedUpLeftArrows,
            ref IEnumerable<Arrow<int>> storedUpRightArrows,
            ref Arrow<int> lastDownArrow,
            bool layerHasMoreThanOneVertexAndVerticalArrowPair,
            bool vertexHasNoEarlierDownArrow,
            bool isLone)
        {
            var curVertex = curLayer[indexInCurLayer];

            // Zero allowed for an up arrow (implicit or explicit) if the vertex has no earlier
            // down arrow, the layer has mroe than one vertex, and the layer has more than one vertical arrow pair,
            // in which case the composition term needs to be "decoded".
            int numLeft = composition.Terms[indexInComposition];
            if (layerHasMoreThanOneVertexAndVerticalArrowPair && vertexHasNoEarlierDownArrow)
            {
                numLeft -= 1;
            }

            // Zero allowed for implicit up arrows, in which case the composition term needs
            // to be decoded.
            int numRight = composition.Terms[indexInComposition + 1];
            if (isLone) numRight -= 1;

            indexInNextLayerRef += numLeft;

            // Up arrow
            var upArrow = new Arrow<int>(curVertex, nextLayer[indexInNextLayerRef]);
            arrowsToAdd.Add(upArrow);
            nextLayerImplicitUpArrowCounts[nextLayer[indexInNextLayerRef]] -= 1;

            // For use in lambdas, where ref parameters cannot be used! (Is also used a bit further down)
            int indexInNextLayer = indexInNextLayerRef;
            // Arrows going to the left
            var leftArrows = Enumerable.Range(0, numLeft).Select(k => new Arrow<int>(
                nextLayer[indexInNextLayer - k],
                nextLayer[indexInNextLayer - (k + 1)]));
            arrowsToAdd.AddRange(leftArrows);

            // Potential stuff
            if (lastDownArrow is null)
            {
                // If this was the first up arrow, we have no stored down arrow for the left cycle/polygon
                // so store the up and left arrows until the very last down arrow of the layer has been encountered
                // ToList(), because leftArrows is defined lazily
                storedUpLeftArrows = leftArrows.Prepend(upArrow).ToList();
            }
            else
            {
                // Add the left/counterclockwise cycle
                int leftmostVertex = lastDownArrow.Target;
                var cycleVerticesInCurLayer = Utility.InfiniteRange(indexInCurLayer, step: -1)
                                              .Select(index => curLayer[index])
                                              .TakeWhile(vertex => vertex != leftmostVertex)
                                              .Append(leftmostVertex)
                                              .Reverse();
                var cycleArrowsInCurLayer = cycleVerticesInCurLayer.AdjacentPairs()
                                                            .Select(pair => new Arrow<int>(pair.Item1, pair.Item2));
                var cycleArrows = new Arrow<int>[] { upArrow }.Concat(leftArrows).Append(lastDownArrow).Concat(cycleArrowsInCurLayer);
                linCombsToAdd[new DetachedCycle<int>(cycleArrows)] = -1;
            }

            // Arrows going to the right
            var rightArrows = Enumerable.Range(0, numRight).Select(k => new Arrow<int>(
                nextLayer[indexInNextLayer + k],
                nextLayer[indexInNextLayer + k + 1]));
            arrowsToAdd.AddRange(rightArrows);
            if (isLone)
            {
                // If the up arrow is implicit, store the arrows and add the
                // right /clockwise cycle when handling the corresponding
                // implicit down arrow instead
                // ToList(), because rightArrows is defined lazily
                storedUpRightArrows = rightArrows.Prepend(upArrow).ToList();
            }
            else
            {
                // Add the right/clockwise cycle
                var downArrow = new Arrow<int>(nextLayer[indexInNextLayerRef + numRight], curVertex);
                var cycleArrows = new Arrow<int>[] { upArrow }.Concat(rightArrows)
                                                              .Append(downArrow);
                linCombsToAdd[new DetachedCycle<int>(cycleArrows)] = +1;
            }

            return numRight;
        }

        private void HandleLoneDownArrow(
            CircularList<int> curLayer,
            CircularList<int> nextLayer,
            int indexInCurLayer,
            int indexInNextLayer,
            Dictionary<int, int> nextLayerImplicitUpArrowCounts,
            List<Arrow<int>> arrowsToAdd,
            Dictionary<DetachedCycle<int>, int> linCombsToAdd,
            ref IEnumerable<Arrow<int>> storedUpRightArrows,
            ref Arrow<int> lastDownArrow)
        {
            int curVertex = curLayer[indexInCurLayer];

            // Add the arrow going down (and keep note of the implicit up arrow for the next layer)
            // Add the right/clockwise cycle to the potential (use storedUpRightArrow to figure out
            // the arrows of the cycle).
            var nextLayerVertex = nextLayer[indexInNextLayer];
            var downArrow = new Arrow<int>(nextLayerVertex, curVertex);
            arrowsToAdd.Add(downArrow);
            lastDownArrow = downArrow;
            nextLayerImplicitUpArrowCounts[nextLayerVertex] += 1;

            var sourceVertex = storedUpRightArrows.First().Source;
            var curLayerVertices = Utility.InfiniteRange(indexInCurLayer, step: -1)
                                            .Select(index => curLayer[index])
                                            .TakeWhile(vertex => vertex != sourceVertex)
                                            .Append(sourceVertex);
            var leftArrowsInCurLayer = curLayerVertices.AdjacentPairs().Select(pair => new Arrow<int>(pair.Item1, pair.Item2));
            var cycleArrows = storedUpRightArrows.Append(downArrow).Concat(leftArrowsInCurLayer);
            storedUpRightArrows = null;
            linCombsToAdd[new DetachedCycle<int>(cycleArrows)] = +1;
        }

        private void HandleExplicitArrowPairs(
            CircularList<int> curLayer,
            CircularList<int> nextLayer,
            int indexInCurLayer,
            ref int indexInNextLayer,
            int numExplicitArrowPairsToHandle,
            Composition composition,
            ref int indexInComposition,
            Dictionary<int, int> nextLayerImplicitUpArrowCounts,
            List<Arrow<int>> arrowsToAdd,
            Dictionary<DetachedCycle<int>, int> linCombsToAdd,
            ref IEnumerable<Arrow<int>> storedUpLeftArrows,
            ref IEnumerable<Arrow<int>> storedUpRightArrows,
            ref Arrow<int> lastDownArrow,
            bool vertexHasLoneDownArrow,
            bool layerHasMoreThanOneVertexAndVerticalArrowPair)
        {
            int curVertex = curLayer[indexInCurLayer];
            foreach (var explicitArrowPairIndex in Enumerable.Range(0, numExplicitArrowPairsToHandle))
            {
                // Add up, left, and right arrows (and keep note of the implicit down arrow in the next layer)
                int numRight = AddUpLeftAndRightArrows(
                    curLayer,
                    nextLayer,
                    indexInCurLayer,
                    ref indexInNextLayer,
                    composition,
                    indexInComposition,
                    nextLayerImplicitUpArrowCounts,
                    arrowsToAdd,
                    linCombsToAdd,
                    ref storedUpLeftArrows,
                    ref storedUpRightArrows,
                    ref lastDownArrow,
                    layerHasMoreThanOneVertexAndVerticalArrowPair,
                    vertexHasNoEarlierDownArrow: explicitArrowPairIndex == 0 && !vertexHasLoneDownArrow,
                    isLone: false);

                // Add down arrow (and keep note of the implicit up arrow in the next layer)
                var downArrow = new Arrow<int>(nextLayer[indexInNextLayer + numRight], curVertex);
                arrowsToAdd.Add(downArrow);
                nextLayerImplicitUpArrowCounts[nextLayer[indexInNextLayer + numRight]] += 1;
                lastDownArrow = downArrow;

                indexInNextLayer += numRight;
                indexInComposition += 2;
            }
        }

        private void HandleLoneUpArrow(
            CircularList<int> curLayer,
            CircularList<int> nextLayer,
            int indexInCurLayer,
            ref int indexInNextLayer,
            Composition composition,
            ref int indexInComposition,
            Dictionary<int, int> nextLayerImplicitUpArrowCounts,
            List<Arrow<int>> arrowsToAdd,
            Dictionary<DetachedCycle<int>, int> linCombsToAdd,
            ref IEnumerable<Arrow<int>> storedUpLeftArrows,
            ref IEnumerable<Arrow<int>> storedUpRightArrows,
            ref Arrow<int> lastDownArrow,
            bool layerHasMoreThanOneVertexAndVerticalArrowPair)
        {
            var curVertex = curLayer[indexInCurLayer];

            // Add the arrow up and the arrows in the next layer and keep note of the implicit down arrow
            // (Do not add the arrow going down or keep note of the implicit up arrow)
            var numExplicitArrowPairs = explicitArrowPairCounts[curVertex];
            int numRight = AddUpLeftAndRightArrows(
                curLayer,
                nextLayer,
                indexInCurLayer,
                ref indexInNextLayer,
                composition,
                indexInComposition,
                nextLayerImplicitUpArrowCounts,
                arrowsToAdd,
                linCombsToAdd,
                ref storedUpLeftArrows,
                ref storedUpRightArrows,
                ref lastDownArrow,
                layerHasMoreThanOneVertexAndVerticalArrowPair,
                vertexHasNoEarlierDownArrow: numExplicitArrowPairs == 0, // Lone down arrow and lone up arrow without explicit pairs should be impossible, so no need to check for a lone down arrow
                isLone: true);
            indexInNextLayer += numRight;
            indexInComposition += 2;
        }

        /// <remarks>
        /// <para>Not popping the composition parameters stack is useful when unsupplying the last
        /// composition on a generator exception when almost done with a supplied composition.</para>
        /// </remarks>
        private void InternalUnsupplyLastComposition(bool popCompositionParametersStack)
        {
            if (NumCompositionsSupplied.IsOdd()) InternalUnsupplyCompositionForDistributingExplicitArrowPairsToVertices(popCompositionParametersStack);
            else InternalUnsupplyCompositionForDistributingArrowsToPolygons(popCompositionParametersStack);
        }

        /// <remarks>
        /// <para>Not popping the composition parameters stack is useful when unsupplying the last
        /// composition on a generator exception when almost done with a supplied composition.</para>
        /// </remarks>
        private void InternalUnsupplyCompositionForDistributingExplicitArrowPairsToVertices(bool popCompositionParametersStack)
        {
            // Values from explicitArrowPairCounts could be set to 0 or removed from the dictionary
            // but there is no need for it.

            compositionsSupplied.RemoveLastElement();
            if (popCompositionParametersStack) expectedCompositionParametersStack.Pop();
        }

        /// <remarks>
        /// <para>Not popping the composition parameters stack is useful when unsupplying the last
        /// composition on a generator exception when almost done with a supplied composition.</para>
        /// </remarks>
        private void InternalUnsupplyCompositionForDistributingArrowsToPolygons(bool popCompositionParametersStack)
        {
            // Undo changes to
            //   * verticesWithImplicitUpArrows
            //   * verticesWithImplicitDownArrows
            //   * arrowsAdded;
            //   * quiverInPlane
            //   * potential
            //   * compositionsSupplied
            //   * expectedCompositionParametersStack (optionally)

            compositionsSupplied.RemoveLastElement();
            if (popCompositionParametersStack) expectedCompositionParametersStack.Pop();
            var layerIndex = NumCompositionsSupplied / 2;
            var curLayer = layersOfVertices[layerIndex];
            var nextLayer = layersOfVertices[layerIndex + 1];

            verticesWithImplicitUpArrows.ExceptWith(nextLayer);
            verticesWithImplicitDownArrows.ExceptWith(nextLayer);
            var arrowsAdded = arrowsAddedList[layerIndex];
            foreach (var arrow in arrowsAdded)
            {
                quiverInPlane.RemoveArrow(arrow);
            }
            arrowsAddedList.RemoveAt(layerIndex);

            var linCombsAdded = linCombsAddedList[layerIndex];
            foreach (var (cycle, coefficient) in linCombsAdded)
            {
                potential = potential.AddCycle(cycle, -coefficient);
            }
            linCombsAddedList.RemoveAt(layerIndex);
        }

        /// <summary>
        /// Undoes the last suppletion of a composition.
        /// </summary>
        /// <exception cref="InvalidOperationException">The generation is not started, or no
        /// compositions have been supplied.</exception>
        public void UnsupplyLastComposition()
        {
            if (!generationIsStarted) throw new InvalidOperationException("The generation is not started.");
            if (NumCompositionsSupplied == 0) throw new InvalidOperationException("No compositions have been supplied.");

            InternalUnsupplyLastComposition(popCompositionParametersStack: true);
        }

        /// <summary>
        /// Undoes the last suppletion of a composition, which was a composition for distributing
        /// explicit arrow pairs to vertices.
        /// </summary>
        public void UnsupplyCompositionForDistributingExplicitArrowPairsToVertices()
        {
            if (!generationIsStarted) throw new InvalidOperationException("The generation is not started.");
            if (NumCompositionsSupplied == 0) throw new InvalidOperationException("No compositions have been supplied.");
            if (NumCompositionsSupplied.IsEven()) throw new InvalidOperationException("The last composition supplied " +
                "was a composition for distributing arrows to polygons, not explicit arrow pairs to vertices.");

            InternalUnsupplyCompositionForDistributingExplicitArrowPairsToVertices(popCompositionParametersStack: true);
        }

        /// <summary>
        /// Undoes the last suppletion of a composition, which was a composition for distributing
        /// arrows to polygons.
        /// </summary>
        public void UnsupplyCompositionForDistributingArrowsToPolygons()
        {
            if (!generationIsStarted) throw new InvalidOperationException("The generation is not started.");
            if (NumCompositionsSupplied == 0) throw new InvalidOperationException("No compositions have been supplied.");
            if (NumCompositionsSupplied.IsOdd()) throw new InvalidOperationException("The last composition supplied " +
                "was a composition for distributing explicit arrow pairs to vertices, not arrows to polygons.");

            InternalUnsupplyCompositionForDistributingArrowsToPolygons(popCompositionParametersStack: true);
        }

        /// <summary>
        /// Ends the generation of a layer quiver.
        /// </summary>
        /// <returns>A tuple whose first element is a <see cref="QuiverInPlane{TVertex}"/>, which
        /// is not necessarily embedded <em>planarly</em> (at least not if the arrows are drawn as
        /// straight line segments), and whose second element is a
        /// <see cref="QuiverWithPotential{TVertex}"/>, representing the QP induced by the quiver
        /// in plane (when drawn with curved arrows, I guess).</returns>
        /// <exception cref="InvalidOperationException">The generation is not started, or the
        /// generation is not completed.</exception>
        public InteractiveLayerQuiverGeneratorOutput EndGeneration()
        {
            if (!generationIsStarted) throw new InvalidOperationException("The generation is not started.");
            if (!GenerationIsCompleted) throw new InvalidOperationException("The generation is not completed.");

            var qp = new QuiverWithPotential<int>(potential);
            var output = new InteractiveLayerQuiverGeneratorOutput(
                layerType,
                new List<Composition>(compositionsSupplied),
                quiverInPlane.Copy(),
                qp);
            return output;
        }
    }
}
