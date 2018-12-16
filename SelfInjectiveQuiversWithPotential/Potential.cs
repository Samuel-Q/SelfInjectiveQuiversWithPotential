using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential
{
    /// <summary>
    /// This class represents a potential, i.e., a linear combination of cycles (up to starting point)
    /// in a quiver.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertices in the quiver.</typeparam>
    /// <remarks>
    /// <para>As of this writing, this class only supports integral coefficients for the linear combination.</para>
    /// <para>This class is immutable.</para></remarks>
    public class Potential<TVertex> where TVertex : IEquatable<TVertex>, IComparable<TVertex>
    {
        /// <summary>
        /// Gets the linear combination that constitutes the potential.
        /// </summary>
        public LinearCombination<DetachedCycle<TVertex>> LinearCombinationOfCycles { get; private set; }

        /// <summary>
        /// Gets the cycles of the potential.
        /// </summary>
        public IEnumerable<DetachedCycle<TVertex>> Cycles { get { return LinearCombinationOfCycles.ElementToCoefficientDictionary.Keys; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="Potential{TVertex}"/> class to be empty.
        /// </summary>
        public Potential() : this(new LinearCombination<DetachedCycle<TVertex>>()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Potential{TVertex}"/> class to contain a
        /// single cycle.
        /// </summary>
        /// <param name="cycle">The cycle.</param>
        /// <param name="coefficient">The coefficient of the cycle.</param>
        public Potential(DetachedCycle<TVertex> cycle, int coefficient)
        {
            LinearCombinationOfCycles = new LinearCombination<DetachedCycle<TVertex>>(coefficient, cycle);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Potential{TVertex}"/> class with cycles with
        /// specified coefficients.
        /// </summary>
        /// <param name="cycleToCoefficientDictionary">A dictionary mapping the cycles to their
        /// coefficients.</param>
        public Potential(IReadOnlyDictionary<DetachedCycle<TVertex>, int> cycleToCoefficientDictionary)
            : this(new LinearCombination<DetachedCycle<TVertex>>(cycleToCoefficientDictionary ?? throw new ArgumentNullException("cycleToCoefficientDictionary")))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Potential{TVertex}"/> class as the specified
        /// linear combination of cycles.
        /// </summary>
        /// <param name="linearCombination">The linear combination of cycles.</param>
        public Potential(LinearCombination<DetachedCycle<TVertex>> linearCombination)
        {
            LinearCombinationOfCycles = linearCombination ?? throw new ArgumentNullException("linearCombination");
        }

        /// <summary>
        /// Adds a cycle to the potential.
        /// </summary>
        /// <returns>The potential with the specified cycle added.</returns>
        /// <remarks>This method does <em>not</em> modify the potential.</remarks>
        public Potential<TVertex> AddCycle(DetachedCycle<TVertex> cycle, int coefficient)
        {
            var linComb = LinearCombinationOfCycles.AddSingleton(coefficient, cycle);
            return new Potential<TVertex>(linComb);
        }

        public LinearCombination<Path<TVertex>> DifferentiateCyclically(Arrow<TVertex> arrow)
        {
            var derivativesOfIndividualCycles = LinearCombinationOfCycles.ElementToCoefficientDictionary.Select(pair =>
                {
                    var cycle = pair.Key;
                    var sign = pair.Value;
                    return cycle.DifferentiateCyclically(arrow).Scale(sign);
                });
            var derivative = derivativesOfIndividualCycles.Aggregate((linComb1, linComb2) => linComb1.Add(linComb2));
            return derivative;
        }

        public bool Equals(Potential<TVertex> otherPotential)
        {
            if (otherPotential is null) return false;
            else return LinearCombinationOfCycles.Equals(otherPotential.LinearCombinationOfCycles);
        }

        public override bool Equals(object obj)
        {
            if (obj is Potential<TVertex> otherPotential) return Equals(otherPotential);
            else return false;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return LinearCombinationOfCycles.ToString();
        }
    }
}
