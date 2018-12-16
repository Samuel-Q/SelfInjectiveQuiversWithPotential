using System;
using System.Collections.Generic;
using System.Linq;
using NUnit;
using NUnit.Framework;
using SelfInjectiveQuiversWithPotential;
using SelfInjectiveQuiversWithPotential.Recipes;

namespace SelfInjectiveQuiversWithPotentialTests
{
    [TestFixture]
    public class RecipeExecutorTestFixture
    {
#region Regression tests
        [Test]
        public void AddingArrowBetweenNeighboringBoundaryVertices_Throws()
        {
            var instructions = new IPotentialRecipeInstruction[]
            {
                new AtomicGlueInstruction(0, 1, 3), // Boundary is now <- <- -> ... ->
                new AtomicGlueInstruction(0, 2, 3), // Try to consume only the <- arrows
            };
            var recipe = new PotentialRecipe(instructions);
            var executor = new RecipeExecutor();
            Assert.That(() => executor.ExecuteRecipe(recipe, 10), Throws.InstanceOf<PotentialRecipeExecutionException>());
        }
#endregion
    }
}
