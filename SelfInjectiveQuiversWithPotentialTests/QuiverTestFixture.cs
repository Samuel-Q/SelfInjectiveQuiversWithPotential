using System;
using System.Collections.Generic;
using NUnit.Framework;
using SelfInjectiveQuiversWithPotential;

namespace SelfInjectiveQuiversWithPotentialTests
{
    [TestFixture]
    public class QuiverTestFixture
    {
        [Test]

        public void Constructor_ThrowsNullExceptions()
        {
            Assert.That(() => new Quiver<int>(null, new Arrow<int>[0]), Throws.ArgumentNullException);
            Assert.That(() => new Quiver<int>(new int[] { 1, 2, 3 }, null), Throws.ArgumentNullException);
        }

        [Test]
        public void Constructor_ThrowsOnDuplicateVertices()
        {
            var vertices = new int[] { 1, 2, 3, 4, 3 };
            var arrows = new Arrow<int>[] { };
            Assert.That(() => new Quiver<int>(vertices, arrows), Throws.ArgumentException);
        }

        [Test]
        public void Constructor_ThrowsOnDuplicateArrows()
        {
            var vertices = new int[] { 1, 2, 3, 4, 5 };
            var arrows = new Arrow<int>[] { new Arrow<int>(1, 2), new Arrow<int>(2, 3), new Arrow<int>(1, 2) };
            Assert.That(() => new Quiver<int>(vertices, arrows), Throws.ArgumentException);
        }

        [Test]
        public void Constructor_ThrowsOnArrowWithEndpointNotInVertexCollection()
        {
            var vertices = new int[] { 1, 2 };
            var arrows = new Arrow<int>[] { new Arrow<int>(1, 2), new Arrow<int>(2, 3) };
            Assert.That(() => new Quiver<int>(vertices, arrows), Throws.ArgumentException);
        }

        [Test]
        public void Vertices_TypicalCase()
        {
            var vertices = new int[] { 1, 2, 3, 4, 5 };
            var arrows = new Arrow<int>[] { new Arrow<int>(1, 2), new Arrow<int>(2, 3), new Arrow<int>(3, 1), new Arrow<int>(3, 2), new Arrow<int>(3, 3), new Arrow<int>(3, 4), new Arrow<int>(3, 5) };
            var quiver = new Quiver<int>(vertices, arrows);
            Assert.That(quiver.Vertices, Is.EquivalentTo(vertices));
        }

        [Test]
        public void Arrows_TypicalCase()
        {
            var vertices = new int[] { 1, 2, 3, 4, 5 };
            var arrows = new Arrow<int>[] { new Arrow<int>(1, 2), new Arrow<int>(2, 3), new Arrow<int>(3, 1), new Arrow<int>(3, 2), new Arrow<int>(3, 3), new Arrow<int>(3, 4), new Arrow<int>(3, 5) };
            var quiver = new Quiver<int>(vertices, arrows);
            var expected = new Dictionary<int, List<int>>
            {
                { 1, new List<int> { 2 } },
                { 2, new List<int> { 3 } },
                { 3, new List<int> { 1, 2, 3, 4, 5 } },
                { 4, new List<int> { } },
                { 5, new List<int> { } }
            };
            Assert.That(quiver.AdjacencyLists, Is.EquivalentTo(expected));
        }
    }
}
