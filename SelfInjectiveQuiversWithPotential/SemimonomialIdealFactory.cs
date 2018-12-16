using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential
{
    /// <summary>
    /// This class is used to create instances of the <see cref="SemimonomialIdeal{TVertex}"/> class.
    /// </summary>
    /// <remarks>The justification for this class is that instead of requiring either
    /// <see cref="SemimonomialIdeal{TVertex}"/> to know about the <see cref="Potential{TVertex}"/>
    /// class (e.g., with a <c>SemimonomialIdeal.CreateFromPotential</c> method) or vice versa
    /// (e.g., with a <c>Potential.CreateSemimonomialIdeal</c> method), we put the logic involving
    /// both classes into its own class.</remarks>
    public static class SemimonomialIdealFactory
    {
        /// <summary>
        /// Creates a new instance of the <see cref="SemimonomialIdeal{TVertex}"/> class that
        /// represents the Jacobian ideal of the given potential (in some unspecified quiver
        /// containing all the arrows in the potential).
        /// </summary>
        /// <param name="potential">The potential whose semimonomial Jacobian ideal to create.</param>
        /// <returns>The semimonomial Jacobian ideal of <paramref name="potential"/>.</returns>
        /// <exception cref="NotSupportedException"><paramref name="potential"/> has a cycle with
        /// coefficient not equal to either of -1 and +1,
        /// or some arrow occurs multiple times in a single cycle of <paramref name="potential"/>.</exception>
        /// <exception cref="ArgumentException">For some arrow in <paramref name="potential"/> and
        /// sign, the arrow is contained in more than one cycle of that sign.</exception>
        /// <remarks>
        /// <para>The preconditions on <paramref name="potential"/> as of this writing is that the
        /// the scalars are <c>-1</c> or <c>+1</c>, every arrow occurs in at most one cycle per
        /// sign, and every arrow occurs at most once per cycle.</para>
        /// <para>The reasoning behind the differentiation between <see cref="NotSupportedException"/>
        /// and <see cref="ArgumentException"/> is that <see cref="NotSupportedException"/> is
        /// thrown when things might work out in theory (e.g., all scalars could be -2 or +2
        /// instead of -1 or +1, or an arrow could appear more than once in a cycle if the cycle is
        /// a suitable power of some other cycle (and stars align if the arrow is also contained in
        /// a cycle of the opposite sign)), while <see cref="ArgumentException"/> is thrown when
        /// things do not work out even in theory (i.e., semimonomiality fails to hold). That is,
        /// <see cref="NotSupportedException"/> is thrown in cases that might be &quot;fixed&quot;
        /// in the future.</para>
        /// </remarks>
        public static SemimonomialIdeal<TVertex> CreateSemimonomialIdealFromPotential<TVertex>(Potential<TVertex> potential)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            // Validation
            var cycleCoefficients = new HashSet<int>(potential.LinearCombinationOfCycles.ElementToCoefficientDictionary.Values);
            if (!cycleCoefficients.IsSubsetOf(new int[] { -1, +1 }))
            {
                throw new NotSupportedException("Only potentials with all coefficients equal to +1 or -1 are supported.");
            }

            if (potential.Cycles.Any(cycle => cycle.Arrows.HasDuplicate()))
            {
                throw new NotSupportedException("Potentials with an arrow occurring more than once in a cycle are not supported.");
            }

            var signedCycles = potential.LinearCombinationOfCycles.ElementToCoefficientDictionary;
            var signedArrows = signedCycles.SelectMany(pair =>
            {
                var cycle = pair.Key;
                var sign = pair.Value;
                return cycle.Arrows.Select(arrow => (arrow, sign));
            });

            if (signedArrows.TryGetDuplicate(out var duplicate))
            {
                throw new ArgumentException($"The potential has a signed arrow {duplicate} occurring in more than one cycle.", nameof(potential));
            }

            var monomialGenerators = new List<Path<TVertex>>();
            var nonMonomialGenerators = new List<DifferenceOfPaths<TVertex>>();

            var arrows = potential.Cycles.SelectMany(cycle => cycle.Arrows).WithoutDuplicates();
            foreach (var arrow in arrows)
            {
                // Guaranteed by the validation to have only terms with coefficients +1 or -1
                // Also guaranteed to have at most 2 terms
                var linCombOfPaths = potential.DifferentiateCyclically(arrow);

                var numTerms = linCombOfPaths.NumberOfTerms;
                if (numTerms == 1)
                {
                    monomialGenerators.Add(linCombOfPaths.Elements.Single());
                }
                else if (numTerms == 2)
                {
                    var paths = linCombOfPaths.Elements.ToList();
                    var differenceOfPaths = new DifferenceOfPaths<TVertex>(paths[0], paths[1]);
                    nonMonomialGenerators.Add(differenceOfPaths);
                }
                else System.Diagnostics.Debug.Fail($"{numTerms} terms in the cyclic derivative with respect to {arrow}. Expected at most two terms.");
            }

            var semimonomialIdeal = new SemimonomialIdeal<TVertex>(monomialGenerators, nonMonomialGenerators);
            return semimonomialIdeal;
        }
    }
}
