using System;
using System.Collections.Generic;
using NUnit.Framework;
using SelfInjectiveQuiversWithPotential;

namespace SelfInjectiveQuiversWithPotentialTests
{
    [TestFixture]
    public class PathTestFixture
    {
        public Path<int> CreatePath(int startingPoint, params Arrow<int>[] arrows)
        {
            return new Path<int>(startingPoint, arrows);
        }

        [Test]
        public void Constructor_ThrowsOnStartingPointNull()
        {
            Assert.That(() => new Path<string>(null, new Arrow<string>[] { new Arrow<string>("a", "b"), new Arrow<string>("b", "c") }), Throws.ArgumentNullException);
        }

        [Test]
        public void Constructor_ThrowsOnArrowsNull()
        {
            Assert.That(() => new Path<int>(1, null), Throws.ArgumentNullException);
        }

        [Test]
        public void Constructor_ThrowsOnBadStartingPointButAdjacentArrows()
        {
            int startingPoint = 1;
            var arrows = new Arrow<int>[] { new Arrow<int>(5, 6), new Arrow<int>(6, 7), new Arrow<int>(7, 8) };
            Assert.That(() => new Path<int>(startingPoint, arrows), Throws.ArgumentException);
        }

        [Test]
        public void Constructor_ThrowsOnGoodStartingPointButNonadjacentArrows()
        {
            int startingPoint = 1;
            var arrows = new Arrow<int>[] { new Arrow<int>(1, 2), new Arrow<int>(2, 3), new Arrow<int>(4, 5) };
            Assert.That(() => new Path<int>(startingPoint, arrows), Throws.ArgumentException);
        }

        // Regression test
        [Test]
        public void ConstructorWithVertexCollection_WorksOnEmptyPath()
        {
            int startingPoint = 1;
            var vertices = new int[] { startingPoint };
            Assert.That(() => new Path<int>(vertices), Throws.Nothing);
        }

        [Test]
        public void StartingPoint_OnEmptyPath()
        {
            int startingPoint = 2;
            var arrows = new Arrow<int>[0];
            var path = new Path<int>(startingPoint, arrows);
            Assert.That(path.StartingPoint, Is.EqualTo(startingPoint));
        }

        [Test]
        public void StartingPoint_TypicalCase()
        {
            int startingPoint = 3;
            var arrows = new Arrow<int>[] { new Arrow<int>(3, 4), new Arrow<int>(4, 1), new Arrow<int>(1, 2) };
            var path = new Path<int>(startingPoint, arrows);
            Assert.That(path.StartingPoint, Is.EqualTo(startingPoint));
        }

        [Test]
        public void EndingPoint_OnEmptyPath()
        {
            int startingPoint = 2;
            var arrows = new Arrow<int>[0];
            var path = new Path<int>(startingPoint, arrows);
            Assert.That(path.StartingPoint, Is.EqualTo(startingPoint));
        }

        [Test]
        public void EndingPoint_TypicalCase()
        {
            int startingPoint = 3;
            var arrows = new Arrow<int>[] { new Arrow<int>(3, 4), new Arrow<int>(4, 1), new Arrow<int>(1, 2) };
            var path = new Path<int>(startingPoint, arrows);
            Assert.That(path.EndingPoint, Is.EqualTo(2));
        }

        [Test]
        public void Equals_NotEqualToNull()
        {
            var path = new Path<int>(0, new Arrow<int>[0]);
            Assert.That(path.Equals(null), Is.False);
        }

        [Test]
        public void Equals_EmptyPathsAtSamePointAreEqual()
        {
            var path1 = new Path<int>(1, new Arrow<int>[0]);
            var path2 = new Path<int>(1, new Arrow<int>[0]);
            Assert.That(path1.Equals(path2), Is.True);
        }

        [Test]
        public void Equals_EmptyPathsAtDifferentPointsAreNotEqual()
        {
            var path1 = new Path<int>(1, new Arrow<int>[0]);
            var path2 = new Path<int>(2, new Arrow<int>[0]);
            Assert.That(path1.Equals(path2), Is.False);
        }

        [Test]
        public void Equals_TypicalPositiveCase()
        {
            var path1 = new Path<int>(3, new Arrow<int>[] { new Arrow<int>(3, 5), new Arrow<int>(5, 7), new Arrow<int>(7, 9), new Arrow<int>(9, 1) });
            var path2 = new Path<int>(3, new Arrow<int>[] { new Arrow<int>(3, 5), new Arrow<int>(5, 7), new Arrow<int>(7, 9), new Arrow<int>(9, 1) });
            Assert.That(path1.Equals(path2), Is.True);
        }

        [Test]
        public void Equals_TypicalNegativeCase()
        {
            var path1 = new Path<int>(3, new Arrow<int>[] { new Arrow<int>(3, 5), new Arrow<int>(5, 7), new Arrow<int>(7, 9), new Arrow<int>(9, 1) });
            var path2 = new Path<int>(3, new Arrow<int>[] { new Arrow<int>(3, 5), new Arrow<int>(5, 6), new Arrow<int>(6, 9), new Arrow<int>(9, 1) });
            Assert.That(path1.Equals(path2), Is.False);
        }

        [Test]
        public void EqOperator_WithNull()
        {
            Path<int> nullPath = null;
            var path = new Path<int>(1, new Arrow<int>[] { new Arrow<int>(1, 2), new Arrow<int>(2, 3), new Arrow<int>(3, 1), new Arrow<int>(1, 2) });

            Assert.That(nullPath == null, Is.True);
            Assert.That(nullPath == path, Is.False);
            Assert.That(path == null, Is.False);
        }

        [Test]
        public void NeqOperator_WithNull()
        {
            Path<int> nullPath = null;
            var path = new Path<int>(1, new Arrow<int>[] { new Arrow<int>(1, 2), new Arrow<int>(2, 3), new Arrow<int>(3, 1), new Arrow<int>(1, 2) });

            Assert.That(nullPath != null, Is.False);
            Assert.That(nullPath != path, Is.True);
            Assert.That(path != null, Is.True);
        }

        [Test]
        public void ExtractSubpath_ThrowsOnNegativeIndex()
        {
            var path = CreatePath(1, new Arrow<int>(1, 2), new Arrow<int>(2, 4));
            Assert.That(() => path.ExtractSubpath(-1, 1), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ExtractSubpath_ThrowsOnNegativeCount()
        {
            var path = CreatePath(1, new Arrow<int>(1, 2), new Arrow<int>(2, 4));
            Assert.That(() => path.ExtractSubpath(1, -1), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ExtractSubpath_ThrowsOnIndexingPastTheEnd()
        {
            var path = CreatePath(1, new Arrow<int>(1, 2), new Arrow<int>(2, 4), new Arrow<int>(4, 5));
            Assert.That(() => path.ExtractSubpath(1, 2), Throws.Nothing);
            Assert.That(() => path.ExtractSubpath(1, 3), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ExtractSubpath_OnEmptyPath()
        {
            var path = CreatePath(1);
            var subpath = path.ExtractSubpath(0, 0);
            Assert.That(subpath, Is.EqualTo(path));
        }

        [Test]
        public void ExtractSubpath_ExtractEmptyPathAtBeginning()
        {
            var path = CreatePath(1, new Arrow<int>(1, 2), new Arrow<int>(2, 3));
            var subpath = path.ExtractSubpath(0, 0);
            Assert.That(subpath, Is.EqualTo(CreatePath(1)));
        }

        [Test]
        public void ExtractSubpath_ExtractEmptyPathInMiddle()
        {
            var path = CreatePath(1, new Arrow<int>(1, 2), new Arrow<int>(2, 3));
            var subpath = path.ExtractSubpath(1, 0);
            Assert.That(subpath, Is.EqualTo(CreatePath(2)));
        }

        [Test]
        public void ExtractSubpath_ExtractEmptyPathAtEnd()
        {
            var path = CreatePath(1, new Arrow<int>(1, 2), new Arrow<int>(2, 3));
            var subpath = path.ExtractSubpath(2, 0);
            Assert.That(subpath, Is.EqualTo(CreatePath(3)));
        }

        [Test]
        public void ExtractSubpath_TypicalCase()
        {
            var path = CreatePath(0, new Arrow<int>(0, 1), new Arrow<int>(1, 2), new Arrow<int>(2, 3), new Arrow<int>(3, 4));
            var subpath = path.ExtractSubpath(1, 2);
            var expectedPath = CreatePath(1, new Arrow<int>(1, 2), new Arrow<int>(2, 3));
            Assert.That(subpath, Is.EqualTo(expectedPath));
        }

        [Test]
        public void AppendArrow_ThrowsOnNullArrow()
        {
            var path = CreatePath(0, new Arrow<int>(0, 1), new Arrow<int>(1, 2), new Arrow<int>(2, 3));
            Assert.That(() => path.AppendArrow(null), Throws.ArgumentNullException);
        }

        [Test]
        public void AppendArrow_ThrowsOnNonadjacentArrow()
        {
            var path = CreatePath(0, new Arrow<int>(0, 1), new Arrow<int>(1, 2), new Arrow<int>(2, 3));
            Assert.That(() => path.AppendArrow(new Arrow<int>(4, 5)), Throws.ArgumentException);
        }

        [Test]
        public void AppendArrow_OnEmptyPath()
        {
            var path = CreatePath(1);
            var expectedPath = CreatePath(1, new Arrow<int>(1, 2));
            Assert.That(path.AppendArrow(new Arrow<int>(1, 2)), Is.EqualTo(expectedPath));
        }

        [Test]
        public void AppendArrow_TypicalCase()
        {
            var path = CreatePath(1, new Arrow<int>(1, 1), new Arrow<int>(1, 2), new Arrow<int>(2, 2));
            var expectedPath = CreatePath(1, new Arrow<int>(1, 1), new Arrow<int>(1, 2), new Arrow<int>(2, 2), new Arrow<int>(2, 3));
            Assert.That(path.AppendArrow(new Arrow<int>(2, 3)), Is.EqualTo(expectedPath));
        }

        [Test]
        public void ReplaceSubpath_ThrowsOnNull()
        {
            var path = CreatePath(1, new Arrow<int>(1, 1), new Arrow<int>(1, 2), new Arrow<int>(2, 2));
            Assert.That(() => path.ReplaceSubpath(1, 1, null), Throws.ArgumentNullException);
        }

        [Test]
        public void ReplaceSubpath_ThrowsOnNegativeIndex()
        {
            var path = CreatePath(1, new Arrow<int>(1, 2), new Arrow<int>(2, 4));
            var newSubpath = CreatePath(0, new Arrow<int>(0, 5), new Arrow<int>(5, 1));
            Assert.That(() => path.ReplaceSubpath(-1, 1, newSubpath), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ReplaceSubpath_ThrowsOnNegativeCount()
        {
            var path = CreatePath(1, new Arrow<int>(1, 2), new Arrow<int>(2, 4));
            var newSubpath = CreatePath(2, new Arrow<int>(2, 0), new Arrow<int>(0, 1));
            Assert.That(() => path.ReplaceSubpath(1, -1, newSubpath), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ReplaceSubpath_ThrowsOnIndexingPastTheEnd()
        {
            var path = CreatePath(1, new Arrow<int>(1, 2), new Arrow<int>(2, 4), new Arrow<int>(4, 5));
            var newSubpath1 = CreatePath(2, new Arrow<int>(2, 3), new Arrow<int>(3, 4), new Arrow<int>(4, 5));
            var newSubpath2 = CreatePath(2, new Arrow<int>(2, 3), new Arrow<int>(3, 4), new Arrow<int>(4, 5), new Arrow<int>(5, 7));
            Assert.That(() => path.ReplaceSubpath(1, 2, newSubpath1), Throws.Nothing);
            Assert.That(() => path.ReplaceSubpath(1, 3, newSubpath2), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ReplaceSubpath_ThrowsOnMismatchingStartingPoints()
        {
            var path = CreatePath(1, new Arrow<int>(1, 2), new Arrow<int>(2, 3), new Arrow<int>(3, 4), new Arrow<int>(4, 5));
            var newSubpath = CreatePath(6, new Arrow<int>(6, 7), new Arrow<int>(7, 3));
            Assert.That(() => path.ReplaceSubpath(1, 1, newSubpath), Throws.ArgumentException);
        }

        [Test]
        public void ReplaceSubpath_ThrowsOnMismatchingEndingPoints()
        {
            var path = CreatePath(1, new Arrow<int>(1, 2), new Arrow<int>(2, 3), new Arrow<int>(3, 4), new Arrow<int>(4, 5));
            var newSubpath = CreatePath(2, new Arrow<int>(2, 7), new Arrow<int>(7, 6));
            Assert.That(() => path.ReplaceSubpath(1, 1, newSubpath), Throws.ArgumentException);
        }

        [Test]
        public void ReplaceSubpath_ReplaceEmptyPath()
        {
            var path = CreatePath(1, new Arrow<int>(1, 2), new Arrow<int>(2, 3), new Arrow<int>(3, 4), new Arrow<int>(4, 5));
            var newSubpath = CreatePath(2, new Arrow<int>(2, 7), new Arrow<int>(7, 2));
            var expectedPath = CreatePath(1, new Arrow<int>(1, 2), new Arrow<int>(2, 7), new Arrow<int>(7, 2), new Arrow<int>(2, 3), new Arrow<int>(3, 4), new Arrow<int>(4, 5));
            Assert.That(path.ReplaceSubpath(1, 0, newSubpath), Is.EqualTo(expectedPath));
        }

        [Test]
        public void ReplaceSubpath_ReplaceByEmptyPath()
        {
            var path = CreatePath(1, new Arrow<int>(1, 2), new Arrow<int>(2, 3), new Arrow<int>(3, 2), new Arrow<int>(2, 5));
            var newSubpath = CreatePath(2);
            var expectedPath = CreatePath(1, new Arrow<int>(1, 2), new Arrow<int>(2, 5));
            Assert.That(path.ReplaceSubpath(1, 2, newSubpath), Is.EqualTo(expectedPath));
        }
    }
}
