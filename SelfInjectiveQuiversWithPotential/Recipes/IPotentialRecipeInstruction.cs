using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Recipes
{
    /// <summary>
    /// Classes implementing this interface represent a potential recipe instruction.
    /// </summary>
    /// <seealso cref="PotentialRecipe"/>
    public interface IPotentialRecipeInstruction
    {
        /// <summary>
        /// Executes the instruction.
        /// </summary>
        /// <param name="state">The state of the recipe executor before executing this instruction.</param>
        /// <returns>The state of the recipe executor after having executed this instruction.</returns>
        RecipeExecutorState Execute(RecipeExecutorState state);

        /// <summary>
        /// Executes the instruction.
        /// </summary>
        /// <param name="stateBefore">The state of the recipe executor before executing this instruction.</param>
        /// <param name="stateAfter">Output parameter for the state of the recipe executor after having
        /// executed this instruction. If the execution fails, this value is <see langword="null"/>.</param>
        /// <returns>A boolean value indicating whether the instruction was executed successfully.</returns>
        bool TryExecute(RecipeExecutorState stateBefore, out RecipeExecutorState stateAfter);
    }
}
