using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential
{
    /// <summary>
    /// This class represents a formal Z-linear combination of elements of any equatable type.
    /// </summary>
    /// <typeparam name="T">The type of the elements that are linearly combined.</typeparam>
    /// <remarks>
    /// <para>This class is immutable.</para>
    /// </remarks>
    public class LinearCombination<T> where T : IEquatable<T>
    {
        /// <summary>
        /// Gets a dictionary mapping elements to their coefficients.
        /// </summary>
        /// <remarks>The dictionary only contains nonzero coefficients.</remarks>
        public IReadOnlyDictionary<T, int> ElementToCoefficientDictionary { get; private set; }

        /// <summary>
        /// Gets an <see cref="IEnumerable{T}"/> of the elements in this linear combination.
        /// </summary>
        public IEnumerable<T> Elements { get { return ElementToCoefficientDictionary.Keys; } }

        /// <summary>
        /// Gets the number of terms in this linear combination.
        /// </summary>
        public int NumberOfTerms { get => ElementToCoefficientDictionary.Count; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearCombination{T}"/> class to represent
        /// the empty linear combination.
        /// </summary>
        public LinearCombination() : this(new Dictionary<T, int>()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearCombination{T}"/> class to represent
        /// a singleton linear combination.
        /// </summary>
        /// <param name="coefficient">The coefficient of the term.</param>
        /// <param name="element">The element of the term.</param>
        public LinearCombination(int coefficient, T element) : this(new Dictionary<T, int> { { element, coefficient } }) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearCombination{T}"/> class.
        /// </summary>
        /// <param name="elementToCoefficientDictionary">A dictionary mapping elements to their coefficients.</param>
        public LinearCombination(IReadOnlyDictionary<T, int> elementToCoefficientDictionary)
        {
            if (elementToCoefficientDictionary == null) throw new ArgumentNullException("elementToCoefficientDictionary");

            ElementToCoefficientDictionary = RemoveZerosFromDictionary(elementToCoefficientDictionary);
        }

        public LinearCombination<T> Scale(int scalar)
        {
            if (scalar == 0) return new LinearCombination<T>();

            var dict = new Dictionary<T, int>(ElementToCoefficientDictionary.ToDictionary(p => p.Key, p => p.Value));
            foreach (var key in ElementToCoefficientDictionary.Keys)
            {
                dict[key] *= scalar;
            }

            return new LinearCombination<T>(dict);
        }

        public LinearCombination<T> Add(LinearCombination<T> addend)
        {
            var resultDict = new Dictionary<T, int>(ElementToCoefficientDictionary.ToDictionary(p => p.Key, p => p.Value));
            foreach (var pair in addend.ElementToCoefficientDictionary)
            {
                if (!resultDict.ContainsKey(pair.Key)) resultDict[pair.Key] = pair.Value;
                else resultDict[pair.Key] += pair.Value;
            }

            resultDict = RemoveZerosFromDictionary(resultDict);

            return new LinearCombination<T>(resultDict);
        }

        public LinearCombination<T> AddSingleton(int coefficient, T addend)
        {
            var linComb = new LinearCombination<T>(new Dictionary<T, int>() { { addend, coefficient } });
            return Add(linComb);
        }

        private Dictionary<T, int> RemoveZerosFromDictionary(IReadOnlyDictionary<T, int> dict)
        {
            var resultDict = new Dictionary<T, int>();
            foreach (var pair in dict)
            {
                if (pair.Value != 0) resultDict[pair.Key] = pair.Value;
            }

            return resultDict;
        }

        public bool Equals(LinearCombination<T> otherComb)
        {
            if (otherComb is null) return false;
            return ElementToCoefficientDictionary.EqualUpToOrder(otherComb.ElementToCoefficientDictionary);
        }

        public override bool Equals(object obj)
        {
            var combObj = obj as LinearCombination<T>;
            if (combObj is null) return false; // Careful with the overloaded == operator
            return Equals(combObj);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
            // TODO: Consider caching the result
            // Remember that the order when iterating over the dictionary is undefined and
            // typically affects the return value of the usual implementation of GetHashCode()!
        }

        public static bool operator ==(LinearCombination<T> comb1, LinearCombination<T> comb2)
        {
            if (comb1 is null) return comb2 is null;
            return comb1.Equals(comb2);
        }

        public static bool operator !=(LinearCombination<T> comb1, LinearCombination<T> comb2) => !(comb1 == comb2);

        public override string ToString()
        {
            var builder = new StringBuilder();
            bool first = true;
            foreach (var pair in ElementToCoefficientDictionary)
            {
                var cycle = pair.Key;
                var coefficient = pair.Value;
                if (first)
                {
                    builder.AppendFormat("{0}*{1}", coefficient, cycle);
                    first = false;
                }
                else
                {
                    builder.AppendFormat(" {0} {1}*{2}", coefficient >= 0 ? "+" : "-", Math.Abs(coefficient), cycle);
                }
            }

            return builder.ToString();
        }
    }
}
