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
    class CompositeGlueInstructionTestFixture
    {
        private QPAnalysisSettings GetSettings(bool detectNonCancellativity)
        {
            var cancellativityFailureDetection = detectNonCancellativity ? CancellativityTypes.Cancellativity : CancellativityTypes.None;
            return new QPAnalysisSettings(cancellativityFailureDetection);
        }

        [Test]
        public void Constructor_ThrowsArgumentNullException_OnNullInstructions()
        {
            Assert.That(() => new CompositeGlueInstruction(null), Throws.ArgumentNullException);
        }

        #region Not really unit tests
        /// These are not really unit tests (certainly not of <see cref="CompositeGlueInstruction"/>),
        /// but they are good to have <em>somewhere</em>. Let's put them here, because they really test
        /// <see cref="CompositeGlueInstruction"/> more than <see cref="RecipeExecutor"/>.
        [Test]
        public void Execute_Throws_WhenBoundaryArrowsHaveDifferentOrientation()
        {
            var gen = new RecipeExecutor();
            var instructions = new IPotentialRecipeInstruction[]
            {
                new AtomicGlueInstruction(0, 1, 4),
                new CompositeGlueInstruction(new AtomicGlueInstruction[] { new AtomicGlueInstruction(0, 4, 10) })
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
                new CompositeGlueInstruction(new AtomicGlueInstruction[] { new AtomicGlueInstruction(0, 4, 10) })
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
                new CompositeGlueInstruction(new AtomicGlueInstruction[]
                {
                    new AtomicGlueInstruction(0, 1, 4),
                    new AtomicGlueInstruction(1, 1, 4),
                    new AtomicGlueInstruction(2, 1, 4),
                    new AtomicGlueInstruction(3, 1, 4),
                    new AtomicGlueInstruction(4, 1, 4),
                }),
                new CompositeGlueInstruction(new AtomicGlueInstruction[]
                {
                    new AtomicGlueInstruction(2,  2, 3),
                    new AtomicGlueInstruction(5,  2, 3),
                    new AtomicGlueInstruction(8,  2, 3),
                    new AtomicGlueInstruction(11, 2, 3),
                    new AtomicGlueInstruction(14, 2, 3),
                }),
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
