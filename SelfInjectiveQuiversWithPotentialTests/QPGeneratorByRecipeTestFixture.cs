using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential;
using SelfInjectiveQuiversWithPotential.Recipes;
using NUnit;
using NUnit.Framework;

namespace SelfInjectiveQuiversWithPotentialTests
{
    [TestFixture]
    public class QPGeneratorByRecipeTestFixture
    {
        [Test]
        public void GenerateQPs_GeneratesTheRightNumberOfQPs()
        {
            var gen = new QPGeneratorByRecipe();
            var qps = gen.GenerateQPs(3, 3, 1, 3);
            Assert.That(qps.Count(), Is.EqualTo(1));
        }
    }
}
