using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Layer
{
    /// <summary>
    /// This class represents the two parameters &quot;sum&quot; and &quot;number of terms&quot;
    /// for a composition.
    /// </summary>
    /// <seealso cref="Composition"/>
    public class CompositionParameters
    {
        /// <summary>
        /// Gets the sum.
        /// </summary>
        public int Sum { get; }

        /// <summary>
        /// Gets the number of terms.
        /// </summary>
        public int NumTerms { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositionParameters"/> class.
        /// </summary>
        /// <param name="sum">The sum.</param>
        /// <param name="numTerms">The number of terms.</param>
        /// <exception cref="ArgumentException"><paramref name="numTerms"/> is negative, or
        /// <paramref name="sum"/> is strictly less than <paramref name="numTerms"/>, or
        /// <paramref name="numTerms"/> is zero and <paramref name="sum"/> is strictly positive.</exception>
        /// <remarks>
        /// <para>It seems debatable whether <paramref name="numTerms"/> equal to zero and
        /// <paramref name="sum"/> strictly positive should be a disallowed pair of parameters.
        /// Instead, it could be an allowed pair of parameters with zero compositions.</para>
        /// </remarks>
        public CompositionParameters(int sum, int numTerms)
        {
            if (numTerms < 0) throw new ArgumentException($"The number of terms ({numTerms}) is negative.", nameof(numTerms));
            if (sum < numTerms) throw new ArgumentException($"The sum ({sum}) is less than the number of terms ({numTerms}).");
            if (numTerms == 0 && sum > 0) throw new ArgumentException($"The number of terms is zero but the sum ({sum}) is positive.");

            Sum = sum;
            NumTerms = numTerms;
        }

        public override bool Equals(object obj)
        {
            return obj is CompositionParameters parameters &&
                   Sum == parameters.Sum &&
                   NumTerms == parameters.NumTerms;
        }

        public override int GetHashCode()
        {
            var hashCode = 1684812574;
            hashCode = hashCode * -1521134295 + Sum.GetHashCode();
            hashCode = hashCode * -1521134295 + NumTerms.GetHashCode();
            return hashCode;
        }

        // Forget about this until NUnit is fixed (it seems to break with the DebuggerDisplay
        // attribute as well
        public override string ToString()
        {
            // NUnit seems to go bonkers if the string returned by ToString contains any of the
            // strings "in" and "with".
            // "1" and "3" is fine
            // "1 with 3_" is bad but "1 with 3x" is fine
            // Return the empty string to make the tests run.
            // Also do the same for the LayerType class.

            return $"({Sum} in {NumTerms} term{(NumTerms != 1 ? "s" : "")})";
        }
    }
}
