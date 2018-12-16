using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential.Plane;

namespace SelfInjectiveQuiversWithPotential.Layer
{
    public class LayeredQuiverGenerator
    {
        /// <summary>
        /// The first vertex of the quivers to generate.
        /// </summary>
        public int firstVertex;

        private const int DefaultFirstVertex = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="LayeredQuiverGenerator"/> class.
        /// </summary>
        public LayeredQuiverGenerator() : this(DefaultFirstVertex)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayeredQuiverGenerator"/> class to generate
        /// quivers with vertices ranging from a specified number.
        /// </summary>
        /// <param name="firstVertex">The first vertex of the quiver.</param>
        public LayeredQuiverGenerator(int firstVertex)
        {
            this.firstVertex = firstVertex;
        }

        /// <summary>
        /// Generates all layered quivers of the specified layer type using the specified composition
        /// generator.
        /// </summary>
        /// <param name="layerType">The layer type of the layered quivers to generate.</param>
        /// <param name="firstVertex">The first vertex of the quivers to generate.</param>
        /// <param name="compositionGenerator">The composition generator whose compositions to use.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"><paramref name="layerType"/> is
        /// <see langword="null"/>, or <paramref name="compositionGenerator"/> is
        /// <see langword="null"/>.</exception>
        public IEnumerable<InteractiveLayeredQuiverGeneratorOutput> GenerateForFixedLayerType(
            LayerType layerType,
            ICompositionGenerator compositionGenerator)
        {
            if (layerType is null) throw new ArgumentNullException(nameof(layerType));
            if (compositionGenerator is null) throw new ArgumentNullException(nameof(compositionGenerator));

            var interactiveGenerator = new InteractiveLayeredQuiverGenerator();
            var nextCompositionParameters = interactiveGenerator.StartGeneration(layerType, firstVertex);
            return DoWork(interactiveGenerator, nextCompositionParameters, compositionGenerator);
        }

        public IEnumerable<InteractiveLayeredQuiverGeneratorOutput> GenerateFromBaseForFixedLayerType(
            QuiverInPlane<int> quiverInPlane,
            Potential<int> potential,
            IEnumerable<int> boundaryLayer,
            LayerType layerType,
            ICompositionGenerator compositionGenerator,
            int nextVertex)
        {
            if (quiverInPlane is null) throw new ArgumentNullException(nameof(quiverInPlane));
            if (potential is null) throw new ArgumentNullException(nameof(potential));
            if (boundaryLayer is null) throw new ArgumentNullException(nameof(boundaryLayer));
            if (layerType is null) throw new ArgumentNullException(nameof(layerType));
            if (compositionGenerator is null) throw new ArgumentNullException(nameof(compositionGenerator));

            var interactiveGenerator = new InteractiveLayeredQuiverGenerator();
            if (!interactiveGenerator.TryStartGenerationFromBase(quiverInPlane, potential, boundaryLayer, layerType, nextVertex, out var nextCompositionParameters))
                return new InteractiveLayeredQuiverGeneratorOutput[0];

            return DoWork(interactiveGenerator, nextCompositionParameters, compositionGenerator);
        }

        private IEnumerable<InteractiveLayeredQuiverGeneratorOutput> DoWork(
            InteractiveLayeredQuiverGenerator interactiveQuiverGenerator,
            CompositionParameters nextCompositionParameters,
            ICompositionGenerator compositionGenerator)
        {
            if (nextCompositionParameters is null)
            {
                yield return interactiveQuiverGenerator.EndGeneration();
                yield break;
            }

            foreach (var composition in compositionGenerator.GenerateCompositions(nextCompositionParameters))
            {
                if (!interactiveQuiverGenerator.TrySupplyComposition(composition, out nextCompositionParameters)) continue;

                foreach (var output in DoWork(interactiveQuiverGenerator, nextCompositionParameters, compositionGenerator))
                    yield return output;

                interactiveQuiverGenerator.UnsupplyLastComposition();
            }
        }
    }
}
