using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures;

namespace SelfInjectiveQuiversWithPotential.Recipes
{
    /// <summary>
    /// This class is used to generate QPs using recipes.
    /// </summary>
    public class QPGeneratorByRecipe
    {
        public IEnumerable<QuiverWithPotential<int>> GenerateQPs(int numPeriods, int initialCycleLength, int numRounds, int maxCycleLength)
        {
            if (numPeriods <= 0) throw new ArgumentOutOfRangeException(nameof(numPeriods));
            if (initialCycleLength <= 2) throw new ArgumentOutOfRangeException(nameof(initialCycleLength));
            if (numRounds < 0) throw new ArgumentOutOfRangeException(nameof(numRounds));
            if (maxCycleLength <= 2) throw new ArgumentOutOfRangeException(nameof(maxCycleLength));
            if (!initialCycleLength.IsMultipleOf(numPeriods)) throw new ArgumentException($"The length of the initial cycle ({initialCycleLength}) is not a multiple of the number of periods ({numPeriods})", nameof(initialCycleLength));

            var startCycle = Utility.MakeCycle(0, initialCycleLength);
            var startArrows = startCycle.CanonicalPath.Arrows;
            var boundaryTuples = startArrows.Select(a => (a, BoundaryArrowOrientation.Right));
            var state = new RecipeExecutorState
            {
                Potential = new Potential<int>(startCycle, +1),
                NextVertex = initialCycleLength,
                Boundary = new CircularList<(Arrow<int>, BoundaryArrowOrientation)>(boundaryTuples),
                PotentiallyProblematicArrows = new HashSet<Arrow<int>>(),
            };

            return DoWork(state, numPeriods, numRounds, maxCycleLength);
        }

        public IEnumerable<QuiverWithPotential<int>> GenerateQPsFromGivenBase(
            RecipeExecutorState baseState,
            int numPeriods,
            int numRounds,
            int maxCycleLength)
        {
            if (baseState == null) throw new ArgumentNullException(nameof(baseState));
            if (numPeriods <= 0) throw new ArgumentOutOfRangeException(nameof(numPeriods));
            if (numRounds < 0) throw new ArgumentOutOfRangeException(nameof(numRounds));
            if (maxCycleLength <= 2) throw new ArgumentOutOfRangeException(nameof(maxCycleLength));

            // TODO: Might want to validate baseState more carefully here

            return DoWork(baseState, numPeriods, numRounds, maxCycleLength);
        }

        private IEnumerable<QuiverWithPotential<int>> DoWork(RecipeExecutorState state, int numPeriods, int numRounds, int maxCycleLength)
        {
            if (numRounds == 0)
            {
                yield return new QuiverWithPotential<int>(state.Potential);
                yield break;
            }

            int numArrowsInPeriod = state.Boundary.Count / numPeriods;
            foreach (int index in Enumerable.Range(0, numArrowsInPeriod))
            {
                var firstArrowOrientation = state.Boundary[index].Orientation;
                for (int count = 1; count <= numArrowsInPeriod; count++)
                {
                    var lastArrowOrientation = state.Boundary[index + count - 1].Orientation;
                    if (firstArrowOrientation != lastArrowOrientation) break; // The path has arrows of different orientation, so try a new startIndex

                    for (int cycleLength = Math.Max(3, count+1); cycleLength <= maxCycleLength; cycleLength++)
                    {
                        var instruction = new PeriodicAtomicGlueInstruction(new AtomicGlueInstruction(index, count, cycleLength), numPeriods);
                        if (!instruction.TryExecute(state, out var newState)) continue; // Fails only on "problematic arrows"? Also on bad input with GenerateQPsFromGivenBase
                        foreach (var qp in DoWork(newState, numPeriods, numRounds - 1, maxCycleLength)) yield return qp;
                    }
                }
            }
        }
    }
}
