using System;
using System.Collections.Generic;
using System.Linq;
using NUnit;
using NUnit.Framework;
using SelfInjectiveQuiversWithPotential;

namespace SelfInjectiveQuiversWithPotentialTests
{
    [TestFixture]
    public class LinearCombinationTestFixture
    {
        private LinearCombination<char> CreateComb(params KeyValuePair<char, int>[] pairs)
        {
            var dict = pairs.ToDictionary(p => p.Key, p => p.Value);
            return new LinearCombination<char>(dict);
        }

        [Test]
        public void Constructor_ThrowsOnNullDictionary()
        {
            Assert.That(() => new LinearCombination<char>(null), Throws.ArgumentNullException);
        }

        [Test]
        public void Constructor_InitializesElementToCoefficientDictionaryWithoutZeros()
        {
            var dict = new Dictionary<char, int>
            {
                { 'a', 0 },
                { 'b', 1 },
                { 'c', 2 },
                { 'd', -0 },
                { 'e', 3 },
                { 'f', 0 },
            };
            var expectedDict = new Dictionary<char, int>
            {
                { 'b', 1 },
                { 'c', 2 },
                { 'e', 3 },
            };

            var comb = new LinearCombination<char>(dict);
            Assert.That(comb.ElementToCoefficientDictionary, Is.EqualTo(expectedDict));
        }

        [Test]
        public void Equals_OnNull()
        {
            var comb = CreateComb(new KeyValuePair<char, int>('a', 1), new KeyValuePair<char, int>('b', 2));
            Assert.That(comb.Equals(null), Is.False);
        }

        [Test]
        public void Equals_OnEmptyLinearCombinations()
        {
            var comb1 = CreateComb();
            var comb2 = CreateComb();
            Assert.That(comb1.Equals(comb2), Is.True);
        }

        [Test]
        public void Equals_TypicalPositiveCase()
        {
            var comb1 = CreateComb(new KeyValuePair<char, int>('a', 1), new KeyValuePair<char, int>('b', -1), new KeyValuePair<char, int>('c', 2));
            var comb2 = CreateComb(new KeyValuePair<char, int>('b', -1), new KeyValuePair<char, int>('c', 2), new KeyValuePair<char, int>('a', 1));
            Assert.That(comb1.Equals(comb2), Is.True);
        }

        [Test]
        public void Equals_TypicalNegativeCase()
        {
            var comb1 = CreateComb(new KeyValuePair<char, int>('a', 1), new KeyValuePair<char, int>('b', 1), new KeyValuePair<char, int>('c', 2));
            var comb2 = CreateComb(new KeyValuePair<char, int>('b', -1), new KeyValuePair<char, int>('c', 2), new KeyValuePair<char, int>('a', 1));
            Assert.That(comb1.Equals(comb2), Is.False);
        }

        [Test]
        public void EqOperator_OnNull()
        {
            LinearCombination<char> nullComb = null;
            var comb = CreateComb(new KeyValuePair<char, int>('a', 1), new KeyValuePair<char, int>('b', 2));
            Assert.That(nullComb == null, Is.True);
            Assert.That(comb == null, Is.False);
        }

        [Test]
        public void NeqOperator_OnNull()
        {
            LinearCombination<char> nullComb = null;
            var comb = CreateComb(new KeyValuePair<char, int>('a', 1), new KeyValuePair<char, int>('b', 2));
            Assert.That(nullComb != null, Is.False);
            Assert.That(comb != null, Is.True);
        }

        [Test]
        public void Scale_ByZero()
        {
            var comb1 = CreateComb(new KeyValuePair<char, int>('a', 1), new KeyValuePair<char, int>('b', -1), new KeyValuePair<char, int>('c', 2));
            var expectedComb = CreateComb();
            Assert.That(comb1.Scale(0), Is.EqualTo(expectedComb));
        }

        [Test]
        public void Scale_ByOne()
        {
            var comb1 = CreateComb(new KeyValuePair<char, int>('a', 1), new KeyValuePair<char, int>('b', -1), new KeyValuePair<char, int>('c', 2));
            var expectedComb = CreateComb(new KeyValuePair<char, int>('a', 1), new KeyValuePair<char, int>('b', -1), new KeyValuePair<char, int>('c', 2));
            Assert.That(comb1.Scale(1), Is.EqualTo(expectedComb));
        }

        [Test]
        public void Scale_TypicalCase()
        {
            var comb1 = CreateComb(new KeyValuePair<char, int>('a', 1), new KeyValuePair<char, int>('b', -1), new KeyValuePair<char, int>('c', 2));
            var expectedComb = CreateComb(new KeyValuePair<char, int>('a', 3), new KeyValuePair<char, int>('b', -3), new KeyValuePair<char, int>('c', 6));
            Assert.That(comb1.Scale(3), Is.EqualTo(expectedComb));
        }

        [Test]
        public void Add_AddendZero()
        {
            var comb1 = CreateComb(new KeyValuePair<char, int>('a', 1), new KeyValuePair<char, int>('b', -1), new KeyValuePair<char, int>('c', 2));
            var comb2 = CreateComb();
            Assert.That(comb1.Add(comb2), Is.EqualTo(comb1));
        }

        [Test]
        public void Add_AugendZero()
        {
            var comb1 = CreateComb();
            var comb2 = CreateComb(new KeyValuePair<char, int>('a', 1), new KeyValuePair<char, int>('b', -1), new KeyValuePair<char, int>('c', 2));
            Assert.That(comb1.Add(comb2), Is.EqualTo(comb2));
        }

        [Test]
        public void Add_MutuallyDisjointSupport()
        {
            var comb1 = CreateComb(new KeyValuePair<char, int>('a', 1), new KeyValuePair<char, int>('b', -1), new KeyValuePair<char, int>('c', 2));
            var comb2 = CreateComb(new KeyValuePair<char, int>('d', 1), new KeyValuePair<char, int>('e', -1), new KeyValuePair<char, int>('f', 2));
            var expectedComb = CreateComb(new KeyValuePair<char, int>('a', 1), new KeyValuePair<char, int>('b', -1), new KeyValuePair<char, int>('c', 2), new KeyValuePair<char, int>('d', 1), new KeyValuePair<char, int>('e', -1), new KeyValuePair<char, int>('f', 2));
            Assert.That(comb1.Add(comb2), Is.EqualTo(expectedComb));
        }

        [Test]
        public void Add_OverlappingSupport()
        {
            var comb1 = CreateComb(new KeyValuePair<char, int>('a', 1), new KeyValuePair<char, int>('b', -1), new KeyValuePair<char, int>('c', 2));
            var comb2 = CreateComb(new KeyValuePair<char, int>('b', 2), new KeyValuePair<char, int>('c', -1), new KeyValuePair<char, int>('d', 2));
            var expectedComb = CreateComb(new KeyValuePair<char, int>('a', 1), new KeyValuePair<char, int>('b', 1), new KeyValuePair<char, int>('c', 1), new KeyValuePair<char, int>('d', 2));
            Assert.That(comb1.Add(comb2), Is.EqualTo(expectedComb));
        }

        [Test]
        public void Add_OverlappingSupportWithZeroCoefficientInResult()
        {
            var comb1 = CreateComb(new KeyValuePair<char, int>('a', 1), new KeyValuePair<char, int>('b', -1), new KeyValuePair<char, int>('c', 2));
            var comb2 = CreateComb(new KeyValuePair<char, int>('b', 1), new KeyValuePair<char, int>('c', -1), new KeyValuePair<char, int>('d', 2));
            var expectedComb = CreateComb(new KeyValuePair<char, int>('a', 1), new KeyValuePair<char, int>('c', 1), new KeyValuePair<char, int>('d', 2));
            Assert.That(comb1.Add(comb2), Is.EqualTo(expectedComb));
        }
    }
}
