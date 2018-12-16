using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential.Plane;

namespace SelfInjectiveQuiversWithPotential.Layer
{
    /// <summary>
    /// The type of the output of the <see cref="InteractiveLayerQuiverGenerator"/> class.
    /// </summary>
    /// <remarks>
    /// <para>The output essentially consists of three different representations of the layer
    /// quiver:
    /// /// <list type="bullet">
    /// <item><description>
    /// The layer type and the compositions.
    /// </description></item>
    /// <item><description>
    /// A <see cref="QuiverInPlane{TVertex}"/>.
    /// </description></item>
    /// <item><description>
    /// A <see cref="QuiverWithPotential{TVertex}"/>.
    /// </description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public class InteractiveLayerQuiverGeneratorOutput
    {
        /// <summary>
        /// Gets the layer type of the layer quiver.
        /// </summary>
        public LayerType LayerType { get; }

        /// <summary>
        /// Gets the compositions that specify the arrows of the layer quiver.
        /// </summary>
        public IEnumerable<Composition> Compositions { get; }

        /// <summary>
        /// Gets the a <see cref="QuiverInPlane{TVertex}"/> representing the layer quiver.
        /// </summary>
        public QuiverInPlane<int> QuiverInPlane { get; }

        /// <summary>
        /// Gets a <see cref="QuiverWithPotential{TVertex}"/> representing the layer quiver.
        /// </summary>
        public QuiverWithPotential<int> QP { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InteractiveLayerQuiverGeneratorOutput"/>
        /// class.
        /// </summary>
        /// <param name="layerType">The layer type of the layer quiver.</param>
        /// <param name="compositions">The compositions that specify the arrows of the layer quiver.</param>
        /// <param name="quiverInPlane">A <see cref="QuiverInPlane{TVertex}"/> representing the
        /// layer quiver.</param>
        /// <param name="qp">A <see cref="QuiverWithPotential{TVertex}"/> representing the layer
        /// quiver.</param>
        public InteractiveLayerQuiverGeneratorOutput(
            LayerType layerType,
            IEnumerable<Composition> compositions,
            QuiverInPlane<int> quiverInPlane,
            QuiverWithPotential<int> qp)
        {
            LayerType = layerType ?? throw new ArgumentNullException(nameof(layerType));
            Compositions = compositions ?? throw new ArgumentNullException(nameof(compositions));
            QuiverInPlane = quiverInPlane ?? throw new ArgumentNullException(nameof(quiverInPlane));
            QP = qp ?? throw new ArgumentNullException(nameof(qp));
        }

        public void Deconstruct(
            out LayerType layerType,
            out IEnumerable<Composition> compositions,
            out QuiverInPlane<int> quiverInPlane,
            out QuiverWithPotential<int> qp)
        {
            layerType = LayerType;
            compositions = Compositions;
            quiverInPlane = QuiverInPlane;
            qp = QP;
        }
    }
}
