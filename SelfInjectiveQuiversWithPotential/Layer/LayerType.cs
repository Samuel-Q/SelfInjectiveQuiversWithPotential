using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Layer
{
    /// <summary>
    /// This class represents the &quot;layer type&quot; of a layered quiver.
    /// </summary>
    /// <remarks>
    /// <para>The layer type of a layered quiver is a finite sequence
    /// <c>m_1, k_1, ..., m_L</c>
    /// of <c>2L-1</c> terms, where
    /// <list type="bullet">
    /// <item><description>
    /// <c>m_i</c> is the number of vertices in layer <c>i</c> (zero-based index), and
    /// </description></item>
    /// <item><description>
    /// <c>k_i</c> is the total number of pairs of vertical arrows between layer <c>i</c> and <c>i+1</c>.
    /// </description></item>
    /// </list>
    /// </remarks>
    public class LayerType
    {
        /// <summary>
        /// Gets the number of layers.
        /// </summary>
        public int NumLayers { get => LayerSizes.Count; }

        /// <summary>
        /// Gets the layer sizes.
        /// </summary>
        public IReadOnlyList<int> LayerSizes { get; }

        /// <summary>
        /// Gets the numbers of pairs of vertical arrows between the adjacent layers.
        /// </summary>
        /// <remarks>
        /// <para>Specifically, the <c>i</c>th value is the number of pairs of arrows between layer
        /// <c>i</c> and <c>i+1</c> (everything zero-based or everything one-based).</para>
        /// </remarks>
        public IReadOnlyList<int> VerticalArrowPairCounts { get; }

        /// <summary>
        /// Gets all the parameter values of this layer type in &quot;interleaved order&quot;,
        /// i.e., <c>m_1, k_1, m_2, k_2, ..., m_L</c>.
        /// </summary>
        public IReadOnlyList<int> ZippedParameters
        {
            get
            {
                return LayerSizes.Zip(VerticalArrowPairCounts, (layerSize, pairCount) => new int[] { layerSize, pairCount })
                                 .SelectMany(pair => pair)
                                 .Append(LayerSizes.Last())
                                 .ToList();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayerType"/> class.
        /// </summary>
        /// <param name="parameterValues">An <see cref="IEnumerable{T}"/> containing the parameter
        /// values in an &quot;interleaved order&quot;, i.e., <c>m_1, k_1, m_2, k_2, ..., m_L</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="parameterValues"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="parameterValues"/> has an even
        /// number of elements, or
        /// <paramref name="parameterValues"/> has the first <c>m</c> value (the very first
        /// parameter) strictly less than <c>3</c> and not equal to <c>1</c>, or
        /// <paramref name="parameterValues"/> has a non-first <c>m</c> value (at even zero-based indices)
        /// that is strictly less than <c>3</c>, or
        /// <paramref name="parameterValues"/> has a <c>k</c> value (at odd zero-based indices)
        /// that is non-positive.</exception>
        public LayerType(IEnumerable<int> parameterValues)
        {
            if (parameterValues is null) throw new ArgumentNullException(nameof(parameterValues));
            if (parameterValues.Count().IsEven())
                throw new ArgumentException($"The number of parameter values {parameterValues.Count()} is even.", nameof(parameterValues));

            LayerSizes = parameterValues.Where((value, index) => index.IsEven()).ToList();
            VerticalArrowPairCounts = parameterValues.Where((value, index) => index.IsOdd()).ToList();

            if (LayerSizes.First() < 3 && LayerSizes.First() != 1)
                throw new ArgumentException($"The first layer size is {LayerSizes.First()}, which is strictly less than 3 and not equal to 1.");

            if (LayerSizes.Skip(1).Any(layerSize => layerSize < 3))
                throw new ArgumentException($"At least one of the layer sizes is strictly less than 3.", nameof(parameterValues));

            if (VerticalArrowPairCounts.Any(numArrowPairs => numArrowPairs <= 0))
                throw new ArgumentException($"At least one of the vertical arrow pair counts is non-positive.", nameof(parameterValues));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayerType"/> class.
        /// </summary>
        /// <param name="parameterValues">An array containing the parameter values in an
        /// &quot;interleaved order&quot; i.e., <c>m_1, k_1, m_2, k_2, ..., m_L</c>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="parameterValues"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="parameterValues"/> has an even
        /// number of elements, or
        /// <paramref name="parameterValues"/> has an <c>m</c> value (at even zero-based indices)
        /// that is strictly less than <c>3</c>, or
        /// <paramref name="parameterValues"/> has a <c>k</c> value (at odd zero-based indices)
        /// that is non-positive.</exception>
        public LayerType(params int[] parameterValues) : this((IEnumerable<int>)(parameterValues))
        { }

        public override bool Equals(object obj)
        {
            return obj is LayerType type &&
                   EqualityComparer<IReadOnlyList<int>>.Default.Equals(LayerSizes, type.LayerSizes) &&
                   EqualityComparer<IReadOnlyList<int>>.Default.Equals(VerticalArrowPairCounts, type.VerticalArrowPairCounts);
        }

        public override int GetHashCode()
        {
            var hashCode = -1155690444;
            hashCode = hashCode * -1521134295 + EqualityComparer<IReadOnlyList<int>>.Default.GetHashCode(LayerSizes);
            hashCode = hashCode * -1521134295 + EqualityComparer<IReadOnlyList<int>>.Default.GetHashCode(VerticalArrowPairCounts);
            return hashCode;
        }

        public override string ToString()
        {
            // NUnit seems to go bonkers (silently ignore tests with the TestCaseSource attribute)
            // by strings returned by this method. Return the empty string to make the tests run.
            // Also do the same for the CompositionParameters class.

            var parameterValues = LayerSizes.Zip(VerticalArrowPairCounts, (size, count) => (size, count))
                                            .SelectMany(pair => new int[] { pair.size, pair.count })
                                            .Append(LayerSizes.Last());
            var unparenthesizedString = String.Join(", ", parameterValues);
            return $"({unparenthesizedString})";
        }
    }
}
