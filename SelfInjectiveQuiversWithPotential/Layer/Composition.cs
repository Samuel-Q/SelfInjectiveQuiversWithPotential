using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Layer
{
    /// <summary>
    /// This class represents a composition (in combinatorics), i.e., decomposition (!) of a
    /// natural number into an <em>ordered</em> sum of positive integers.
    /// </summary>
    public class Composition
    {
        /// <summary>
        /// Gets the number that is written as a sum.
        /// </summary>
        public int Sum { get; }

        /// <summary>
        /// Gets the number of terms in the sum.
        /// </summary>
        public int NumTerms { get => Terms.Count; }

        /// <summary>
        /// Gets the terms of the composition.
        /// </summary>
        public IReadOnlyList<int> Terms { get; }

        /// <summary>
        /// Gets the parameters of the composition.
        /// </summary>
        public CompositionParameters Parameters => new CompositionParameters(Sum, NumTerms);

        /// <summary>
        /// Initializes a new instance of the <see cref="Composition"/> class.
        /// </summary>
        /// <param name="terms">The terms of the composition.</param>
        /// <exception cref="ArgumentNullException"><paramref name="terms"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="terms"/> contains a non-positive
        /// integer.</exception>
        public Composition(IEnumerable<int> terms)
        {
            Terms = terms?.ToList() ?? throw new ArgumentNullException(nameof(terms));
            if (terms.Any(term => term <= 0)) throw new ArgumentException("At least one of the terms is non-positive.", nameof(terms));
            Sum = Terms.Sum();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Composition"/> class.
        /// </summary>
        /// <param name="terms">The terms of the composition.</param>
        /// <exception cref="ArgumentNullException"><paramref name="terms"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="terms"/> contains a non-positive
        /// integer.</exception>
        public Composition(params int[] terms) : this((IEnumerable<int>)terms)
        { }

        public override bool Equals(object obj)
        {
            return obj is Composition composition && Terms.SequenceEqual(composition.Terms);
        }

        public override int GetHashCode()
        {
            return -1073114708 + EqualityComparer<IReadOnlyList<int>>.Default.GetHashCode(Terms);
        }

        public override string ToString()
        {
            if (NumTerms == 0) return "0";

            var formalSum = String.Join("+", Terms);
            return $"{Sum} = {formalSum}";
        }
    }
}
