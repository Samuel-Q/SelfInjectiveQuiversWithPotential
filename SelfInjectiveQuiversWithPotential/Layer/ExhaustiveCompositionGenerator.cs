using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Layer
{
    /// <summary>
    /// This class is used to generate <em>all</em> compositions with the given parameters.
    /// </summary>
    public class ExhaustiveCompositionGenerator : ICompositionGenerator
    {
        /// <summary>
        /// Generates all compositions with the specified parameters.
        /// </summary>
        /// <param name="compositionParameters">The parameters of the compositions to generate.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> containing the compositions that were generated.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="compositionParameters"/> is
        /// <see langword="null"/>.</exception>
        /// <remarks>
        /// <para>The compositions are generated in increasing lexicographical order.</para>
        /// </remarks>
        public IEnumerable<Composition> GenerateCompositions(CompositionParameters compositionParameters)
        {
            if (compositionParameters is null) throw new ArgumentNullException(nameof(compositionParameters));

            // If no terms, return the singleton collection of the empty composition.
            if (compositionParameters.NumTerms == 0)
            {
                return new Composition[] { new Composition() };
            }

            int numTermsLeft = compositionParameters.NumTerms;
            var previousValues = new List<int>();
            var previousValuesSum = 0;
            int targetSum = compositionParameters.Sum;
            return DoWork(numTermsLeft, previousValues, previousValuesSum, targetSum);
        }

        public IEnumerable<Composition> DoWork(
            int numTermsLeft,
            List<int> previousValues,
            int previousValuesSum,
            int targetSum)
        {
            if (numTermsLeft == 1)
            {
                int lastTerm = targetSum - previousValuesSum;
                previousValues.Add(lastTerm);
                yield return new Composition(previousValues);
                previousValues.RemoveLastElement();
                yield break;
            }

            int leastPossibleSum = previousValuesSum + numTermsLeft;
            int wiggleRoomForThisTerm = targetSum - leastPossibleSum;
            for (int currentTerm = 1; currentTerm <= 1 + wiggleRoomForThisTerm; currentTerm++)
            {
                previousValues.Add(currentTerm);
                foreach (var composition in DoWork(numTermsLeft - 1, previousValues, previousValuesSum + currentTerm, targetSum))
                    yield return composition;

                previousValues.RemoveLastElement();
            }
        }
    }
}
