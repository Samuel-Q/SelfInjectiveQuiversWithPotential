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
    public class CompositionTestFixture
    {
        public Composition CreateComposition(params int[] terms)
        {
            return new Composition(terms);
        }

        [Test]
        public void Constructor_ThrowsArgumentNullException()
        {
            Assert.That(() => new Composition(null), Throws.ArgumentNullException);
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-10)]
        [TestCase(Int32.MinValue)]
        [TestCase(0, 123)]
        [TestCase(-1, 123)]
        [TestCase(-10, 123)]
        [TestCase(Int32.MinValue, 123)]
        [TestCase(123, 0)]
        [TestCase(123, -1)]
        [TestCase(123, -10)]
        [TestCase(123, Int32.MinValue)]
        public void Constructor_ThrowsArgumentException_OnNonPositiveTerms(params int[] terms)
        {
            Assert.That(() => new Composition(terms), Throws.ArgumentException);
        }

        [TestCase(0)]
        [TestCase(1, 1)]
        [TestCase(10, 10)]
        [TestCase(30, 10, 20)]
        [TestCase(5, 1, 1, 1, 1, 1)]
        [TestCase(15, 1, 2, 3, 4, 5)]
        public void Sum_Works(int sum, params int[] terms)
        {
            var composition = CreateComposition(terms);
            Assert.That(composition.Sum, Is.EqualTo(sum));
        }

        [TestCase(0)]
        [TestCase(1, 1)]
        [TestCase(1, 10)]
        [TestCase(2, 10, 20)]
        [TestCase(5, 1, 1, 1, 1, 1)]
        [TestCase(5, 1, 2, 3, 4, 5)]
        public void NumTerms_Works(int numTerms, params int[] terms)
        {
            var composition = CreateComposition(terms);
            Assert.That(composition.NumTerms, Is.EqualTo(numTerms));
        }

        [TestCase()]
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(10, 20)]
        [TestCase(1, 1, 1, 1, 1)]
        [TestCase(1, 2, 3, 4, 5)]
        public void Terms_Works(params int[] terms)
        {
            var composition = CreateComposition(terms);
            Assert.That(composition.Terms, Is.EqualTo(terms));
        }
    }
}
