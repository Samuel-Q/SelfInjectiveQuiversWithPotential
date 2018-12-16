using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using SelfInjectiveQuiversWithPotential.Layer;

namespace SelfInjectiveQuiversWithPotentialTests
{
    [TestFixture]
    public class ExhaustiveCompositionGeneratorTestFixture
    {
        ExhaustiveCompositionGenerator generator;

        [OneTimeSetUp]
        public void SetUp()
        {
            generator = new ExhaustiveCompositionGenerator();
        }

        CompositionParameters CreateCompositionParameters(int sum, int numTerms)
        {
            return new CompositionParameters(sum, numTerms);
        }

        Composition CreateComposition(params int[] terms)
        {
            return new Composition(terms);
        }

        [Test]
        public void GenerateCompositions_ThrowsArgumentNullException()
        {
            Assert.That(() => generator.GenerateCompositions(null), Throws.ArgumentNullException);
        }

        [Test]
        public void GenerateCompositions_Works_ForEmptyComposition()
        {
            var parameters = CreateCompositionParameters(0, 0);
            var compositions = generator.GenerateCompositions(parameters).ToList();
            Assert.That(compositions, Has.Count.EqualTo(1));
            var composition = compositions.Single();
            Assert.That(composition, Is.EqualTo(CreateComposition()));
        }

        [Test]
        public void GenerateCompositions_Works()
        {
            var parameters = CreateCompositionParameters(5, 3);
            var expectedCompositions = new Composition[]
            {
                CreateComposition(1, 1, 3),
                CreateComposition(1, 2, 2),
                CreateComposition(1, 3, 1),
                CreateComposition(2, 1, 2),
                CreateComposition(2, 2, 1),
                CreateComposition(3, 1, 1),
            };
            var actualCompositions = generator.GenerateCompositions(parameters);
            Assert.That(actualCompositions, Is.EqualTo(expectedCompositions));
        }
    }
}
