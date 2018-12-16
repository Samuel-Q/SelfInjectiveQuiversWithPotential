using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Recipes
{
    /// <summary>
    /// This class represents a potential recipe (or &quot;program&quot;), which is a list of instructions
    /// (see <see cref="IPotentialRecipeInstruction"/>) specifying how to construct a potential by starting
    /// with an n-gon and gluing cycles onto it.
    /// </summary>
    public class PotentialRecipe
    {
        /// <summary>
        /// Gets the list of instructions that constitute this recipe.
        /// </summary>
        public IEnumerable<IPotentialRecipeInstruction> Instructions { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PotentialRecipe"/> class.
        /// </summary>
        /// <param name="instructions">The list of instructions.</param>
        /// <exception cref="ArgumentNullException"><paramref name="instructions"/> is <see langword="null"/>.</exception>
        public PotentialRecipe(IEnumerable<IPotentialRecipeInstruction> instructions)
        {
            Instructions = instructions ?? throw new ArgumentNullException(nameof(instructions));
        }
    }
}
