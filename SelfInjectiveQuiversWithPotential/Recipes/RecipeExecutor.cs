using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures;

namespace SelfInjectiveQuiversWithPotential.Recipes
{
    /// <summary>
    /// This class is used to execute a recipe (see <see cref="PotentialRecipe"/>) and obtain a
    /// potential (or the corresponding QP).
    /// </summary>
    public class RecipeExecutor
    {
        /// <summary>
        /// Executes the specified recipe.
        /// </summary>
        /// <param name="recipe">The recipe according to which to generate the potential of the QP.</param>
        /// <param name="initialCycleNumArrows">The number of arrows in the first cycle.</param>
        /// <returns>A QP generated according to the recipe.</returns>
        /// <exception cref="PotentialRecipeExecutionException"><paramref name="recipe"/> cannot be
        /// executed starting with a cycle of <paramref name="initialCycleNumArrows"/> arrows.</exception>
        public QuiverWithPotential<int> ExecuteRecipe(PotentialRecipe recipe, int initialCycleNumArrows)
        {
            InternalExecuteRecipe(recipe, initialCycleNumArrows, out var qp, true);
            return qp;
        }

        /// <summary>
        /// Executes the specified recipe.
        /// </summary>
        /// <param name="recipe">The recipe according to which to generate the potential of the QP.</param>
        /// <param name="initialCycleNumArrows">The number of arrows in the first cycle.</param>
        /// <param name="qp">Output parameter for the QP generated according to the recipe. If the
        /// recipe is not executed successfully, the output value is <see langword="null"/>.</param>
        /// <returns>A boolean value indicating whether the recipe was executed successfully.</returns>
        public bool TryExecuteRecipe(PotentialRecipe recipe, int initialCycleNumArrows, out QuiverWithPotential<int> qp)
        {
            return InternalExecuteRecipe(recipe, initialCycleNumArrows, out qp, false);
        }

        /// <remarks>Like <see cref="TryExecuteRecipe(PotentialRecipe, int, out QuiverWithPotential{int})"/>
        /// but with a boolean parameter for throwing.</remarks>
        private bool InternalExecuteRecipe(PotentialRecipe recipe, int initialCycleNumArrows, out QuiverWithPotential<int> qp, bool shouldThrow)
        {
            if (recipe == null) throw new ArgumentNullException(nameof(recipe));
            if (initialCycleNumArrows <= 0) throw new ArgumentOutOfRangeException(nameof(initialCycleNumArrows));

            qp = null;

            var startCycle = Utility.MakeCycle(0, initialCycleNumArrows);
            var startArrows = startCycle.CanonicalPath.Arrows;
            var boundaryTuples = startArrows.Select(a => (a, BoundaryArrowOrientation.Right));
            var state = new RecipeExecutorState
            {
                Potential = new Potential<int>(startCycle, +1),
                NextVertex = initialCycleNumArrows,
                Boundary = new CircularList<(Arrow<int>, BoundaryArrowOrientation)>(boundaryTuples),
                PotentiallyProblematicArrows = new HashSet<Arrow<int>>(),
            };

            foreach (var instruction in recipe.Instructions)
            {
                if (shouldThrow) state = instruction.Execute(state);
                else if (!instruction.TryExecute(state, out state)) return false;
            }

            qp = new QuiverWithPotential<int>(state.Potential);
            return true;
        }
    }
}
