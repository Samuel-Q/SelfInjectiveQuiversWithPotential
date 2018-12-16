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
    public class ExtendedRepeatingCompositionGeneratorTestFixture
    {
        ExtendedRepeatingCompositionGenerator CreateGenerator(int numRepetitions)
        {
            return new ExtendedRepeatingCompositionGenerator(numRepetitions);
        }

        static CompositionParameters CreateCompositionParameters(int sum, int numTerms)
        {
            return new CompositionParameters(sum, numTerms);
        }

        static Composition CreateComposition(params int[] terms)
        {
            return new Composition(terms);
        }

        [TestCase(Int32.MinValue)]
        [TestCase(-10)]
        [TestCase(-1)]
        [TestCase(0)]
        public void Constructor_ThrowsArgumentOutOfRangeException(int numRepetitions)
        {
            Assert.That(() => new ExtendedRepeatingCompositionGenerator(numRepetitions), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void GenerateCompositions_ThrowsArgumentNullException()
        {
            var generator = CreateGenerator(3);
            Assert.That(() => generator.GenerateCompositions(null), Throws.ArgumentNullException);
        }

        [TestCase(2, 3, 2)]
        [TestCase(2, 5, 2)]
        [TestCase(2, 5, 4)]
        [TestCase(2, 15, 10)]
        [TestCase(3, 4, 3)]
        [TestCase(3, 5, 3)]
        [TestCase(3, 14, 9)]
        [TestCase(10, 21, 10)]
        public void GenerateCompositions_ThrowsArgumentException_IfCompositionSumIsNotMultipleOfNumRepetitions(
            int numRepetitions,
            int sum,
            int numTerms)
        {
            var generator = CreateGenerator(numRepetitions);
            var compositionParameters = CreateCompositionParameters(sum, numTerms);
            Assert.That(() => generator.GenerateCompositions(compositionParameters), Throws.ArgumentException);
        }

        [TestCase(2, 4, 3)]
        [TestCase(2, 14, 13)]
        [TestCase(3, 6, 2)]
        [TestCase(3, 15, 13)]
        [TestCase(10, 20, 9)]
        [TestCase(10, 20, 11)]
        public void GenerateCompositions_ThrowsArgumentException_IfCompositionNumTermsIsNotMultipleOfNumRepetitions(
            int numRepetitions,
            int sum,
            int numTerms)
        {
            var generator = CreateGenerator(numRepetitions);
            var compositionParameters = CreateCompositionParameters(sum, numTerms);
            Assert.That(() => generator.GenerateCompositions(compositionParameters), Throws.ArgumentException);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(10)]
        public void GenerateCompositions_Works_ForTheEmptyCompositionParameters(int numRepetitions)
        {
            var generator = CreateGenerator(numRepetitions);
            var compositionParameters = CreateCompositionParameters(0, 0);
            var compositions = generator.GenerateCompositions(compositionParameters).ToList();
            Assert.That(compositions, Has.Count.EqualTo(1));
            var composition = compositions.Single();
            var expectedComposition = CreateComposition();
            Assert.That(composition, Is.EqualTo(expectedComposition));
        }

        public static IEnumerable<TestCaseData> GenerateCompositions_Works_TestCaseSource()
        {
            int numRepetitions = 3;
            var compositionParameters = CreateCompositionParameters(sum: 9, numTerms: 6);
            IEnumerable<Composition> expectedCompositions = new Composition[]
            {
                CreateComposition(1, 2, 1, 2, 1, 2),
                CreateComposition(2, 1, 2, 1, 2, 1)
            };
            yield return new TestCaseData(new object[] { numRepetitions, compositionParameters, expectedCompositions });
        }

        [TestCaseSource(nameof(GenerateCompositions_Works_TestCaseSource))]
        public void GenerateCompositions_Works(
            int numRepetitions,
            CompositionParameters compositionParameters,
            IEnumerable<Composition> expectedCompositions)
        {
            var generator = CreateGenerator(numRepetitions);
            var compositions = generator.GenerateCompositions(compositionParameters).ToList();
            Assert.That(compositions, Is.EqualTo(expectedCompositions));
        }


        public static IEnumerable<TestCaseData> GenerateCompositions_ExtendedCase_TestCaseSource()
        {
            int numRepetitions = 3;
            int numTerms = 1;
            var compositionParameters = CreateCompositionParameters(sum: 1, numTerms);
            var expectedCompositions = new Composition[]
            {
                CreateComposition(1),
            };
            yield return new TestCaseData(new object[] { numRepetitions, compositionParameters, expectedCompositions });

            compositionParameters = CreateCompositionParameters(sum: 2, numTerms);
            expectedCompositions = new Composition[]
            {
                CreateComposition(2),
            };
            yield return new TestCaseData(new object[] { numRepetitions, compositionParameters, expectedCompositions });

            compositionParameters = CreateCompositionParameters(sum: 3, numTerms);
            expectedCompositions = new Composition[]
            {
                CreateComposition(3),
            };
            yield return new TestCaseData(new object[] { numRepetitions, compositionParameters, expectedCompositions });

            compositionParameters = CreateCompositionParameters(sum: 4, numTerms);
            expectedCompositions = new Composition[]
            {
                CreateComposition(4),
            };
            yield return new TestCaseData(new object[] { numRepetitions, compositionParameters, expectedCompositions });
        }

        [TestCaseSource(nameof(GenerateCompositions_ExtendedCase_TestCaseSource))]
        public void GenerateCompositions_ExtendedCase_Works(
            int numRepetitions,
            CompositionParameters compositionParameters,
            IEnumerable<Composition> expectedCompositions)
        {
            var generator = CreateGenerator(numRepetitions);
            var compositions = generator.GenerateCompositions(compositionParameters);
            Assert.That(compositions, Is.EqualTo(expectedCompositions));
        }
    }
}
