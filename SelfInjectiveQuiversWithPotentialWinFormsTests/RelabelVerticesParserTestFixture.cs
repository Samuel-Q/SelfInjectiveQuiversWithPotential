using System;
using System.Collections.Generic;
using System.Linq;
using NUnit;
using NUnit.Framework;
using SelfInjectiveQuiversWithPotentialWinForms;

namespace SelfInjectiveQuiversWithPotentialWinFormsTests
{
    [TestFixture]
    public class RelabelVerticesParserTestFixture
    {
        private RelabelVerticesParser parser;

        private string CreateValidRelabelingString(int numElements)
        {
            var values = Enumerable.Range(0, numElements);
            return string.Join(" ", values);
        }

        [SetUp]
        public void SetUp()
        {
            parser = new RelabelVerticesParser();
        }

        [Test]
        public void TryCreateRelabelingMap_ThrowsArgumentNullException()
        {
            var validRelabelingString = CreateValidRelabelingString(0);
            Assert.That(() => parser.TryCreateRelabelingMap(null, null, out _, out _), Throws.ArgumentNullException);
            Assert.That(() => parser.TryCreateRelabelingMap(null, validRelabelingString, out _, out _), Throws.ArgumentNullException);
            Assert.That(() => parser.TryCreateRelabelingMap(validRelabelingString, null, out _, out _), Throws.ArgumentNullException);
        }

        [TestCase("x123", 1)]
        [TestCase("1x23", 1)]
        [TestCase("12x3", 1)]
        [TestCase("123x", 1)]
        [TestCase("x 123", 2)]
        [TestCase("123 x", 2)]
        [TestCase("123 x 321", 3)]
        [TestCase("123.0", 1)]
        [TestCase("123.", 1)]
        [TestCase(".123", 1)]
        [TestCase("123,0", 1)]
        [TestCase("123,", 1)]
        [TestCase(",123", 1)]
        public void TryCreateRelabelingMap_IsUnsuccessful_WhenVerticesStringContainsNonInteger(
            string verticesString,
            int numVertices)
        {
            var validRelabelingString = CreateValidRelabelingString(numVertices);

            bool success = parser.TryCreateRelabelingMap(verticesString, validRelabelingString, out var relabelingMap, out string errorMessage);
            Assert.That(success, Is.False);
            Assert.That(relabelingMap, Is.Null);
            Assert.That(errorMessage, Is.Not.Null);

            success = parser.TryCreateRelabelingMap(validRelabelingString, verticesString, out relabelingMap, out errorMessage);
            Assert.That(success, Is.False);
            Assert.That(relabelingMap, Is.Null);
            Assert.That(errorMessage, Is.Not.Null);
        }

        [TestCase("-2147483649", 1)]
        [TestCase("2147483648", 1)]
        public void TryCreateRelabelingMap_IsUnsuccessful_WhenVerticesStringContainsTooSmallOrLargeInteger(
            string verticesString,
            int numVertices)
        {
            var validRelabelingString = CreateValidRelabelingString(numVertices);

            bool success = parser.TryCreateRelabelingMap(verticesString, validRelabelingString, out var relabelingMap, out string errorMessage);
            Assert.That(success, Is.False);
            Assert.That(relabelingMap, Is.Null);
            Assert.That(errorMessage, Is.Not.Null);

            success = parser.TryCreateRelabelingMap(validRelabelingString, verticesString, out relabelingMap, out errorMessage);
            Assert.That(success, Is.False);
            Assert.That(relabelingMap, Is.Null);
            Assert.That(errorMessage, Is.Not.Null);
        }

        [TestCase("", "1")]
        [TestCase(" ", "1")]
        [TestCase("1", "1 2")]
        [TestCase(" 1", "1 2")]
        [TestCase("1 ", "1 2")]
        [TestCase(" 1 ", "1 2")]
        public void TryCreateRelabelingMap_IsUnsuccessful_WhenVerticesStringsContainDifferentNumberOfIntegers(
            string verticesString1,
            string verticesString2)
        {
            bool success = parser.TryCreateRelabelingMap(verticesString1, verticesString2, out var relabelingMap, out string errorMessage);
            Assert.That(success, Is.False);
            Assert.That(relabelingMap, Is.Null);
            Assert.That(errorMessage, Is.Not.Null);

            success = parser.TryCreateRelabelingMap(verticesString2, verticesString1, out relabelingMap, out errorMessage);
            Assert.That(success, Is.False);
            Assert.That(relabelingMap, Is.Null);
            Assert.That(errorMessage, Is.Not.Null);
        }

        [TestCase("1 1", 2)]
        [TestCase("1 1 2", 3)]
        [TestCase("1 2 1", 3)]
        [TestCase("1   1   2", 3)]
        public void TryCreateRelabelingMap_IsUnsuccessful_WhenVerticesStringContainsDuplicate(
            string verticesString,
            int numVertices)
        {
            var validRelabelingString = CreateValidRelabelingString(numVertices);

            bool success = parser.TryCreateRelabelingMap(verticesString, validRelabelingString, out var relabelingMap, out string errorMessage);
            Assert.That(success, Is.False);
            Assert.That(relabelingMap, Is.Null);
            Assert.That(errorMessage, Is.Not.Null);

            success = parser.TryCreateRelabelingMap(validRelabelingString, verticesString, out relabelingMap, out errorMessage);
            Assert.That(success, Is.False);
            Assert.That(relabelingMap, Is.Null);
            Assert.That(errorMessage, Is.Not.Null);
        }

        static IEnumerable<TestCaseData> TryCreateRelabelingMap_IsSuccessful_WhenVerticesAreSeparatedByMoreThanOneWhitespaceCharacter_TestCaseSource()
        {
            var expectedRelabelingMap = new Dictionary<int, int>
            {
                { 123, 1 },
                { 456, 2 },
            };
            yield return new TestCaseData(
                "123  456",
                "1 2",
                expectedRelabelingMap);
            yield return new TestCaseData(
                "123   456",
                "1 2",
                expectedRelabelingMap);
            yield return new TestCaseData(
                "123\t\t456",
                "1 2",
                expectedRelabelingMap);
            yield return new TestCaseData(
                "123\t\t\t456",
                "1 2",
                expectedRelabelingMap);
            yield return new TestCaseData(
                "123 456",
                "1  2",
                expectedRelabelingMap);
            yield return new TestCaseData(
                "123 456",
                "1   2",
                expectedRelabelingMap);
            yield return new TestCaseData(
                "123 456",
                "1\t\t2",
                expectedRelabelingMap);
            yield return new TestCaseData(
                "123 456",
                "1\t\t\t2",
                expectedRelabelingMap);
        }

        [TestCaseSource(nameof(TryCreateRelabelingMap_IsSuccessful_WhenVerticesAreSeparatedByMoreThanOneWhitespaceCharacter_TestCaseSource))]
        public void TryCreateRelabelingMap_IsSuccessful_WhenVerticesAreSeparatedByMoreThanOneWhitespaceCharacter(
            string oldVerticesString,
            string newVerticesString,
            Dictionary<int, int> expectedRelabelingMap)
        {
            bool success = parser.TryCreateRelabelingMap(oldVerticesString, newVerticesString, out var relabelingMap, out string errorMessage);
            Assert.That(success, Is.True);
            Assert.That(relabelingMap, Is.EqualTo(expectedRelabelingMap));
            Assert.That(errorMessage, Is.Null);
        }

        static IEnumerable<TestCaseData> TryCreateRelabelingMap_IsSuccessful_EvenWhenVerticesAreVerySmallOrLarge_TestCaseSource()
        {
            yield return new TestCaseData(
                "-2147483648",
                "0",
                new Dictionary<int, int>
                {
                    { -2147483648, 0 }
                });
            yield return new TestCaseData(
                "2147483647",
                "0",
                new Dictionary<int, int>
                {
                    { 2147483647, 0 }
                });
            yield return new TestCaseData(
                "+2147483647",
                "0",
                new Dictionary<int, int>
                {
                    { 2147483647, 0 }
                });
            yield return new TestCaseData(
                "0",
                "-2147483648",
                new Dictionary<int, int>
                {
                    { 0, -2147483648 }
                });
            yield return new TestCaseData(
                "0",
                "2147483647",
                new Dictionary<int, int>
                {
                    { 0, 2147483647 }
                });
            yield return new TestCaseData(
                "0",
                "+2147483647",
                new Dictionary<int, int>
                {
                    { 0, 2147483647 }
                });
        }

        [TestCaseSource(nameof(TryCreateRelabelingMap_IsSuccessful_EvenWhenVerticesAreVerySmallOrLarge_TestCaseSource))]
        public void TryCreateRelabelingMap_IsSuccessful_EvenWhenVerticesAreVerySmallOrLarge(
            string oldVerticesString,
            string newVerticesString,
            Dictionary<int, int> expectedRelabelingMap)
        {
            bool success = parser.TryCreateRelabelingMap(oldVerticesString, newVerticesString, out var relabelingMap, out string errorMessage);
            Assert.That(success, Is.True);
            Assert.That(relabelingMap, Is.EqualTo(expectedRelabelingMap));
            Assert.That(errorMessage, Is.Null);
        }

        [TestCase("", "")]
        [TestCase(" ", "")]
        [TestCase("\t", "")]
        [TestCase("\n", "")]
        [TestCase("\r\n", "")]
        public void TryCreateRelabelingMap_IsSuccessful_OnStringsOfNoVertices(
            string verticesString1,
            string verticesString2)
        {
            var expectedRelabelingMap = new Dictionary<int, int> { };
            bool success = parser.TryCreateRelabelingMap(verticesString1, verticesString2, out var relabelingMap, out string errorMessage);
            Assert.That(success, Is.True);
            Assert.That(relabelingMap, Is.EqualTo(expectedRelabelingMap));
            Assert.That(errorMessage, Is.Null);

            success = parser.TryCreateRelabelingMap(verticesString2, verticesString1, out relabelingMap, out errorMessage);
            Assert.That(success, Is.True);
            Assert.That(relabelingMap, Is.EqualTo(expectedRelabelingMap));
            Assert.That(errorMessage, Is.Null);
        }

        static IEnumerable<TestCaseData> TryCreateRelabelingMap_IsSuccessful_InTypicalCases_TestCaseSource()
        {
            yield return new TestCaseData(
                "1",
                "1",
                new Dictionary<int, int>
                {
                    { 1, 1 },
                });
            var expectedRelabelingMap = new Dictionary<int, int>
            {
                { 1, 2 }
            };
            yield return new TestCaseData("1", "2", expectedRelabelingMap);
            yield return new TestCaseData("1 ", "2", expectedRelabelingMap);
            yield return new TestCaseData(" 1", "2", expectedRelabelingMap);
            yield return new TestCaseData(" 1 ", "2", expectedRelabelingMap);

            expectedRelabelingMap = new Dictionary<int, int>
            {
                { 1, 3 },
                { 2, 4 }
            };
            yield return new TestCaseData("1 2", "3 4", expectedRelabelingMap);
            yield return new TestCaseData("1 2 ", "3 4 ", expectedRelabelingMap);
            yield return new TestCaseData(" 1 2", " 3 4", expectedRelabelingMap);
        }

        [TestCaseSource(nameof(TryCreateRelabelingMap_IsSuccessful_InTypicalCases_TestCaseSource))]
        public void TryCreateRelabelingMap_IsSuccessful_InTypicalCases(
            string oldVerticesString,
            string newVerticesString,
            Dictionary<int, int> expectedRelabelingMap)
        {
            bool success = parser.TryCreateRelabelingMap(oldVerticesString, newVerticesString, out var relabelingMap, out string errorMessage);
            Assert.That(success, Is.True);
            Assert.That(relabelingMap, Is.EqualTo(expectedRelabelingMap));
            Assert.That(errorMessage, Is.Null);
        }
    }
}
