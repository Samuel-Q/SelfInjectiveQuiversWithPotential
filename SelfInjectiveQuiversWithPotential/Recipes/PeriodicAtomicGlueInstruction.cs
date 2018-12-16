using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Recipes
{
    /// <summary>
    /// This class represents an instruction to glue a single cycle onto the potential periodically.
    /// </summary>
    /// <remarks>The arrows of the boundary that are in the resulting cycles are said to be
    /// &quot;consumed&quot; by the instruction.</remarks>
    [DebuggerDisplay("(Index, Count, CycleLength) = ({Index}, {Count}, {CycleLength})")]
    public class PeriodicAtomicGlueInstruction : IPotentialRecipeInstruction
    {
        /// <summary>
        /// Gets the atomic instruction describing the cycle to glue on periodically.
        /// </summary>
        public AtomicGlueInstruction AtomicInstruction { get; private set; }

        /// <summary>
        /// Gets the number of periods to for the gluing (essentially the number of times that
        /// <see cref="AtomicInstruction"/> is to be executed).
        /// </summary>
        public int NumPeriods { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PeriodicAtomicGlueInstruction"/> class.
        /// </summary>
        /// <param name="instruction">An atomic instruction to &quot;periodize&quot;</param>
        /// <param name="numPeriods">The number of periods.</param>
        public PeriodicAtomicGlueInstruction(AtomicGlueInstruction instruction, int numPeriods)
        {
            AtomicInstruction = instruction ?? throw new ArgumentNullException(nameof(instruction));
            NumPeriods = numPeriods > 0 ? numPeriods : throw new ArgumentOutOfRangeException(nameof(numPeriods));
        }

        /// <inheritdoc/>
        public RecipeExecutorState Execute(RecipeExecutorState state)
        {
            InternalExecute(state, out var newState, true);
            return newState;
        }

        /// <inheritdoc/>
        public bool TryExecute(RecipeExecutorState stateBefore, out RecipeExecutorState stateAfter)
        {
            return InternalExecute(stateBefore, out stateAfter, false);
        }

        /// <remarks>Like <see cref="TryExecute(RecipeExecutorState, out RecipeExecutorState)"/>
        /// but with a boolean parameter for throwing.</remarks>
        private bool InternalExecute(RecipeExecutorState state, out RecipeExecutorState stateAfter, bool shouldThrow)
        {
            int boundaryLength = state.Boundary.Count;
            if (!boundaryLength.IsMultipleOf(NumPeriods))
            {
                if (shouldThrow) throw new PotentialRecipeExecutionException($"The length of the boundary ({boundaryLength}) is not a multiple of the number of periods ({NumPeriods}).");
                stateAfter = null;
                return false;
            }

            int periodLength = boundaryLength / NumPeriods;
            var atomicInstructions = Enumerable.Range(0, NumPeriods).Select(k => new AtomicGlueInstruction(k * periodLength + AtomicInstruction.Index, AtomicInstruction.Count, AtomicInstruction.CycleLength));
            var compositeInstruction = new CompositeGlueInstruction(atomicInstructions);

            if (shouldThrow) state = compositeInstruction.Execute(state);
            else if (!compositeInstruction.TryExecute(state, out state))
            {
                stateAfter = null;
                return false;
            }

            stateAfter = state;
            return true;
        }
    }
}
