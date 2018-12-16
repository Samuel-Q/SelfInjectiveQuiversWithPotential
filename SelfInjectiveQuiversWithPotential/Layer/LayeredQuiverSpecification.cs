using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Layer
{
    /// <summary>
    /// This class represents a complete specification of a layered quiver, complete in the sense
    /// that it determines the layered quiver uniquely.
    /// </summary>
    /// <remarks>
    /// <para>The specification is the <see cref="LayerType"/> of the layered quiver and a collection
    /// of <see cref="Composition"/>s that specify the vertical arrows between each pair of layers.
    /// Exactly how the compositions specify the layer type is involved, but the compositions are
    /// of two different types (and alternate between the two types) with the compositions of the
    /// first type encoding the number of explicit arrow pairs for every vertex in a layer and
    /// the compositions of the second type encoding the number of arrows in the upper/outer
    /// layer that are consumed between each pair of vertical arrows between the two layers.</para>
    /// </remarks>
    public class LayeredQuiverSpecification
    {
        /// <summary>
        /// Gets the layer type.
        /// </summary>
        public LayerType LayerType { get; }

        /// <summary>
        /// Gets the compositions that specify the vertical arrows between each pair of layers.
        /// </summary>
        public IReadOnlyList<Composition> Compositions { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayeredQuiverSpecification"/> class.
        /// </summary>
        /// <param name="layerType">The layer type.</param>
        /// <param name="compositions">A list of compositions that specify the vertical arrows
        /// between each pair of layers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="layerType"/> is <see langword="null"/>,
        /// or <paramref name="compositions"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">The number of elements in <paramref name="compositions"/>
        /// is not <c>2 * layerType.NumLayers - 2</c>.</exception>
        /// <remarks>
        /// <para>This constructor does not fully validate the arguments, because it requires quite
        /// a bit of the logic for generating the layered quiver. Instead, the generators,
        /// <see cref="InteractiveLayeredQuiverGenerator"/> and
        /// <see cref="InteractiveLayeredQuiverGenerator"/>, take care of the detailed
        /// validation.</para>
        /// </remarks>
        public LayeredQuiverSpecification(LayerType layerType, IEnumerable<Composition> compositions)
        {
            LayerType = layerType ?? throw new ArgumentNullException(nameof(layerType));
            Compositions = compositions?.ToList() ?? throw new ArgumentNullException(nameof(compositions));

            int expectedNumCompositions = 2 * LayerType.NumLayers - 2;
            if (Compositions.Count != expectedNumCompositions)
                throw new ArgumentException($"The number of compositions {Compositions.Count} differs from the expected number ({expectedNumCompositions}).");
        }
    }
}
