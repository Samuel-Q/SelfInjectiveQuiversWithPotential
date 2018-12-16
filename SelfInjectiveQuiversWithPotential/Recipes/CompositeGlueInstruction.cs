using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Recipes
{
    /// <summary>
    /// This class represents an instruction composed of <see cref="AtomicGlueInstruction"/>s.
    /// </summary>
    /// <remarks>
    /// <para>The point of this class is that <see cref="AtomicGlueInstruction.Index"/> of the
    /// constituent atomic instructions all refer to the boundary before the
    /// <see cref="CompositeGlueInstruction"/> is executed. Compare this to sequentially executing
    /// the <see cref="AtomicGlueInstruction"/>s, where the first <see cref="AtomicGlueInstruction"/>
    /// may update the boundary and hence update the indices of the remaining boundary arrows from
    /// before the execution.</para></remarks>
    public class CompositeGlueInstruction : IPotentialRecipeInstruction
    {
        private List<AtomicGlueInstruction> atomicInstructions;

        public CompositeGlueInstruction(IEnumerable<AtomicGlueInstruction> atomicInstructions)
        {
            this.atomicInstructions = atomicInstructions?.ToList() ?? throw new ArgumentNullException(nameof(atomicInstructions));
        }

        public RecipeExecutorState Execute(RecipeExecutorState state)
        {
            InternalExecute(state, out state, true);
            return state;
        }

        public bool TryExecute(RecipeExecutorState stateBefore, out RecipeExecutorState stateAfter)
        {
            return InternalExecute(stateBefore, out stateAfter, false);
        }

        private bool InternalExecute(RecipeExecutorState stateBefore, out RecipeExecutorState stateAfter, bool shouldThrow)
        {
            stateAfter = null;

            // Normalize the index of every instruction for the current boundary
            var instructions = atomicInstructions.Select(ai => new AtomicGlueInstruction(ai.Index.Modulo(stateBefore.Boundary.Count), ai.Count, ai.CycleLength)).ToList();
            instructions.Sort((i1, i2) =>
            {
                int cmp = i1.Index.CompareTo(i2.Index);
                if (cmp != 0) return cmp;
                else return i1.Count.CompareTo(i2.Count);
            });

            if (!InstructionsHaveDisjointSupport())
            {
                if (shouldThrow) throw new PotentialRecipeExecutionException("The atomic instructions do not have disjoint support.");
                else return false;
            }

            // Execute the instructions from last to first in order to mostly avoid having to update the indices of the instructions
            instructions.Reverse();

            // If the support of last instruction wraps around, its execution will affect the indices of the previous instructions
            // This can be solved either (1) by shifting the indices of all the previous instructions accordingly or
            // (2) by rotating the boundary and shifting the indices of *all* instructions accordingly (including the last)
            // (not that no inverse rotation is necessary).
            // The former approach is used below
            if (instructions.Count > 0)
            {
                var lastInstruction = instructions.First(); // Last instruction with the original order ("increasing")
                int numWrappedAroundArrows = Math.Max(0, (lastInstruction.Index + lastInstruction.Count) - stateBefore.Boundary.Count);
                if (numWrappedAroundArrows > 0)
                {
                    instructions = instructions.Select((instr, index) => index == 0 ? instr : new AtomicGlueInstruction(instr.Index - numWrappedAroundArrows, instr.Count, instr.CycleLength)).ToList();
                }
            }

            var state = stateBefore;
            // Might be better to sort the list descendingly from the beginning, but reversing it here is easier
            foreach (var instruction in instructions)
            {
                if (shouldThrow) state = instruction.Execute(state);
                else if (!instruction.TryExecute(state, out state)) return false;
            }

            stateAfter = state;
            return true;
            
            bool InstructionsHaveDisjointSupport()
            {
                int previousInstructionEnd = 0; // exclusive
                foreach (var instruction in instructions)
                {
                    if (instruction.Index < previousInstructionEnd)
                    {
                        return false;
                    }

                    previousInstructionEnd = instruction.Index + instruction.Count;
                }

                return true;
            }
        }
    }
}
