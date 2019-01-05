using System;
using System.Collections.Generic;
using System.Linq;
using NUnit;
using NUnit.Framework;
using SelfInjectiveQuiversWithPotential;
using SelfInjectiveQuiversWithPotential.Analysis;
using SelfInjectiveQuiversWithPotential.Recipes;

namespace SelfInjectiveQuiversWithPotentialTests
{
    [TestFixture]
    public class AtomicGlueInstructionTestFixture
    {
        private QPAnalysisSettings GetSettings(bool detectNonCancellativity)
        {
            var cancellativityFailureDetection = detectNonCancellativity ? CancellativityTypes.Cancellativity : CancellativityTypes.None;
            return new QPAnalysisSettings(cancellativityFailureDetection);
        }

        #region Not really unit tests
        /// These are not really unit tests (certainly not of <see cref="AtomicGlueInstruction"/>),
        /// but they are good to have <em>somewhere</em>. Let's put them here, because they really test
        /// <see cref="AtomicGlueInstruction"/> more than <see cref="RecipeExecutor"/>.
        [Test]
        public void Execute_Throws_WhenBoundaryArrowsHaveDifferentOrientation()
        {
            var gen = new RecipeExecutor();
            var instructions = new IPotentialRecipeInstruction[]
            {
                new AtomicGlueInstruction(0, 1, 4),
                new AtomicGlueInstruction(3, 1, 4),
                new AtomicGlueInstruction(6, 1, 4),
                new AtomicGlueInstruction(9, 1, 4),
                new AtomicGlueInstruction(12, 2, 4),
            };

            var recipe = new PotentialRecipe(instructions);
            Assert.That(() => gen.ExecuteRecipe(recipe, 5), Throws.InstanceOf<PotentialRecipeExecutionException>());
        }

        [Test]
        public void TryExecute_ReturnsFalse_WhenBoundaryArrowsHaveDifferentOrientation()
        {
            var gen = new RecipeExecutor();
            var instructions = new IPotentialRecipeInstruction[]
            {
                new AtomicGlueInstruction(0, 1, 4),
                new AtomicGlueInstruction(3, 1, 4),
                new AtomicGlueInstruction(6, 1, 4),
                new AtomicGlueInstruction(9, 1, 4),
                new AtomicGlueInstruction(12, 2, 4),
            };

            var recipe = new PotentialRecipe(instructions);
            bool result = gen.TryExecuteRecipe(recipe, 5, out QuiverWithPotential<int> qp);
            Assert.That(result, Is.EqualTo(false));
            Assert.That(qp, Is.EqualTo(null));
        }

        [Test]
        public void Execute_ResultsInSelfInjectiveQP_WhenShouldBeCobMinus5()
        {
            var gen = new RecipeExecutor();
            var instructions = new IPotentialRecipeInstruction[]
            {
                new AtomicGlueInstruction(0, 1, 4),
                new AtomicGlueInstruction(3, 1, 4),
                new AtomicGlueInstruction(6, 1, 4),
                new AtomicGlueInstruction(9, 1, 4),
                new AtomicGlueInstruction(12, 1, 4),
                new AtomicGlueInstruction(2, 2, 3),
                new AtomicGlueInstruction(4, 2, 3),
                new AtomicGlueInstruction(6, 2, 3),
                new AtomicGlueInstruction(8, 2, 3),
                new AtomicGlueInstruction(10, 2, 3),
            };

            var recipe = new PotentialRecipe(instructions);
            var qp = gen.ExecuteRecipe(recipe, 5);
            var analyzer = new QPAnalyzer();
            var settings = GetSettings(detectNonCancellativity: true);
            var result = analyzer.Analyze(qp, settings);
            Assert.That(result.MainResult.HasFlag(QPAnalysisMainResult.SelfInjective));
        }
        #endregion
    }
}
