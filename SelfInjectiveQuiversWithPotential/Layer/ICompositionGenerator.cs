using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Layer
{
    /// <summary>
    /// Classes implementing this interface can be used to generate compositions.
    /// </summary>
    public interface ICompositionGenerator
    {
        /// <summary>
        /// Generates compositions with the specified parameters.
        /// </summary>
        /// <param name="compositionParameters">The parameters of the compositions to generate.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> containing the compositions that were generated.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="compositionParameters"/> is
        /// <see langword="null"/>.</exception>
        IEnumerable<Composition> GenerateCompositions(CompositionParameters compositionParameters);
    }
}
