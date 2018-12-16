using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Layer
{
    /// <summary>
    /// This class is used to generate &quot;repeating&quot; compositions with the given parameters.
    /// </summary>
    /// <remarks>
    /// <para>This class extends <see cref="RepeatingCompositionGenerator"/> to support the case
    /// that the sum and number of terms are not multiples of the number of repetitions <em>if</em>
    /// the number of terms is equal to 1. The case is handled by ignoring the repetition
    /// constraint and generate the only singleton composition with the specified sum.
    /// This is useful when generating layer quivers with a point as base.</para>
    /// </remarks>
    public class ExtendedRepeatingCompositionGenerator : ICompositionGenerator
    {
        /// <summary>
        /// The number of repetitions in the compositions that this generator generates.
        /// </summary>
        private readonly int numRepetitions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtendedRepeatingCompositionGenerator"/> class.
        /// </summary>
        /// <param name="numRepetitions">The number of repetitions in the compositions that
        /// are to be generated.</param>
        /// <remarks>
        /// <para>The generated compositions will have a number of repetitions that is a
        /// multiple of <paramref name="numRepetitions"/>, and it may be a strict multiple!</para>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="numRepetitions"/> is
        /// non-positive.</exception>
        public ExtendedRepeatingCompositionGenerator(int numRepetitions)
        {
            if (numRepetitions <= 0) throw new ArgumentOutOfRangeException(nameof(numRepetitions));
            this.numRepetitions = numRepetitions;
        }

        /// <summary>
        /// Generates all repeating compositions with the specified parameters.
        /// </summary>
        /// <param name="compositionParameters">The parameters of the compositions to generate.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> containing the compositions that were generated.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="compositionParameters"/> is
        /// <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="compositionParameters"/> does not
        /// have <see cref="CompositionParameters.Sum"/> a multiple of the number of repetitions of
        /// this generator, or <paramref name="compositionParameters"/> does not have
        /// <see cref="CompositionParameters.NumTerms"/> a multiple of the number of repetitions of
        /// this generator.</exception>
        /// <remarks>
        /// <para>The compositions are generated in increasing lexicographical order.</para>
        /// </remarks>
        public IEnumerable<Composition> GenerateCompositions(CompositionParameters compositionParameters)
        {
            if (compositionParameters is null) throw new ArgumentNullException(nameof(compositionParameters));

            if (compositionParameters.NumTerms > 1)
            {
                if (!compositionParameters.Sum.IsMultipleOf(numRepetitions))
                    throw new ArgumentException($"The sum ({compositionParameters.Sum}) is not a multiple " +
                        $"of the number of repetitions ({numRepetitions}).");
                if (!compositionParameters.NumTerms.IsMultipleOf(numRepetitions))
                    throw new ArgumentException($"The number of terms ({compositionParameters.NumTerms}) " +
                        $"is not a multiple of the number of repetitions ({numRepetitions}).");
            }

            if (compositionParameters.NumTerms == 1 && numRepetitions > 1)
                return ExtendedGenerate();
            else
                return Generate();

            IEnumerable<Composition> Generate()
            {
                var exhaustiveGenerator = new ExhaustiveCompositionGenerator();
                var parametersForNonRepeatingCompositions = new CompositionParameters(
                    compositionParameters.Sum / numRepetitions,
                    compositionParameters.NumTerms / numRepetitions);
                foreach (var nonRepeatingComposition in exhaustiveGenerator.GenerateCompositions(parametersForNonRepeatingCompositions))
                {
                    var repeatingTerms = Enumerable.Repeat(nonRepeatingComposition.Terms, numRepetitions).SelectMany(term => term);
                    var repeatingComposition = new Composition(repeatingTerms);
                    yield return repeatingComposition;
                }
            }

            IEnumerable<Composition> ExtendedGenerate()
            {
                var exhaustiveGenerator = new ExhaustiveCompositionGenerator();
                return exhaustiveGenerator.GenerateCompositions(compositionParameters);
            }
        }
    }
}
