using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures;

namespace SelfInjectiveQuiversWithPotential.Recipes
{
    /// <summary>
    /// This class represents an instruction to rotate the boundary to the left.
    /// </summary>
    public class RotateLeftInstruction : IPotentialRecipeInstruction
    {
        /// <summary>
        /// Gets the number of arrows by which to rotate.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RotateLeftInstruction"/> class.
        /// </summary>
        /// <param name="count">The number of arrows by which to rotate.</param>
        public RotateLeftInstruction(int count)
        {
            Count = count;
        }

        /// <inheritdoc/>
        public RecipeExecutorState Execute(RecipeExecutorState state)
        {
            InternalExecute(state, out state, true);
            return state;
        }

        /// <inheritdoc/>
        public bool TryExecute(RecipeExecutorState stateBefore, out RecipeExecutorState stateAfter)
        {
            return InternalExecute(stateBefore, out stateAfter, false);
        }

        private bool InternalExecute(RecipeExecutorState stateBefore, out RecipeExecutorState stateAfter, bool shouldThrow)
        {
            var newBoundary = new CircularList<(Arrow<int>, BoundaryArrowOrientation)>(stateBefore.Boundary);
            stateAfter = new RecipeExecutorState
            {
                Potential = stateBefore.Potential,
                NextVertex = stateBefore.NextVertex,
                Boundary = newBoundary,
            };

            return true;
        }
    }
}
