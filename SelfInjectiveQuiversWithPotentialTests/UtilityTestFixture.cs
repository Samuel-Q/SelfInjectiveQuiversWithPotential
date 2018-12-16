using System;
using System.Collections.Generic;
using System.Linq;
using NUnit;
using NUnit.Framework;
using DataStructures;
using SelfInjectiveQuiversWithPotential;

namespace SelfInjectiveQuiversWithPotentialTests
{
    [TestFixture]
    public class UtilityTestFixture
    {
        [Test]
        public void EnumerateCombinationsWithRepetitions_For_NZero_KZero()
        {
            var coll = new List<int> { };
            var expected = new List<List<int>> { new List<int> { } }; // Only the empty combination
            Assert.That(Utility.EnumerateCombinationsWithRepetition(coll, 0), Is.EqualTo(expected));
        }

        [Test]
        public void EnumerateCombinationsWithRepetitions_For_NZero_KNonzero()
        {
            var coll = new List<int> { };
            var expected = new List<List<int>> { }; // No combinations
            Assert.That(Utility.EnumerateCombinationsWithRepetition(coll, 1), Is.EqualTo(expected));
            Assert.That(Utility.EnumerateCombinationsWithRepetition(coll, 10), Is.EqualTo(expected));
        }

        [Test]
        public void EnumerateCombinationsWithRepetitions_For_NNonzero_KZero()
        {
            var coll = new List<int> { 1 };
            var expected = new List<List<int>> { new List<int> { } }; // Only the empty combination
            Assert.That(Utility.EnumerateCombinationsWithRepetition(coll, 0), Is.EqualTo(expected));

            coll = new List<int> { 1, 2, 3, 4, 5, 6 };
            expected = new List<List<int>> { new List<int> { } }; // Only the empty combination
            Assert.That(Utility.EnumerateCombinationsWithRepetition(coll, 0), Is.EqualTo(expected));
        }

        [TestCase(1, 1)]
        [TestCase(1, 10)]
        [TestCase(2, 10)]
        [TestCase(5, 5)]
        [TestCase(5, 3)]
        public void EnumerateCombinationsWithRepetitions_GivesCorrectCount(int n, int k)
        {
            var coll = Enumerable.Range(1, n).ToList();
            int expectedCount = Utility.BinomialCoefficient(n + k - 1, k);
            Assert.That(Utility.EnumerateCombinationsWithRepetition(coll, k).Count(), Is.EqualTo(expectedCount));
        }

        [TestCase(1, 1)]
        [TestCase(1, 10)]
        [TestCase(2, 10)]
        [TestCase(5, 5)]
        [TestCase(5, 3)]
        public void EnumeratePermutationsWithRepetitions_GivesCorrectCount(int n, int k)
        {
            var coll = Enumerable.Range(1, n).ToList();
            int expectedCount = Utility.Power(n, k);
            Assert.That(Utility.EnumeratePermutationsWithRepetition(coll, k).Count(), Is.EqualTo(expectedCount));
        }
    }
}
