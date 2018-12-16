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
    public class CompositionParametersTestFixture
    {
        public CompositionParameters CreateCompositionParameters(int sum, int numTerms)
        {
            return new CompositionParameters(sum, numTerms);
        }

        [TestCase(123, -1)]
        [TestCase(123, -10)]
        [TestCase(123, Int32.MinValue)]
        public void Constructor_ThrowsArgumentException_OnNegativeNumberOfTerms(int sum, int numTerms)
        {
            Assert.That(() => new CompositionParameters(sum, numTerms), Throws.ArgumentException);
        }

        [TestCase(0, 1)]
        [TestCase(1, 2)]
        [TestCase(1, 10)]
        [TestCase(2, 3)]
        [TestCase(2, 10)]
        [TestCase(100, 101)]
        public void Constructor_ThrowsArgumentException_OnSumLessThanNumberOfTerms(int sum, int numTerms)
        {
            Assert.That(() => new CompositionParameters(sum, numTerms), Throws.ArgumentException);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(10)]
        public void Constructor_ThrowsArgumentException_OnStrictlyPositiveSumWithNoTerms(int sum)
        {
            Assert.That(() => new CompositionParameters(sum, numTerms: 0), Throws.ArgumentException);
        }

        [TestCase(0, 0)]
        [TestCase(1, 1)]
        [TestCase(5, 1)]
        [TestCase(5, 2)]
        [TestCase(5, 3)]
        [TestCase(5, 4)]
        [TestCase(5, 5)]
        public void Constructor_Works(int sum, int numTerms)
        {
            var compositionParameters = CreateCompositionParameters(sum, numTerms);
            Assert.That(compositionParameters.Sum, Is.EqualTo(sum));
            Assert.That(compositionParameters.NumTerms, Is.EqualTo(numTerms));
        }
    }
}
