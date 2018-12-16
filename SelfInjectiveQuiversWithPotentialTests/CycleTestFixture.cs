using System;
using System.Collections.Generic;
using System.Linq;
using NUnit;
using NUnit.Framework;
using SelfInjectiveQuiversWithPotential;

namespace SelfInjectiveQuiversWithPotentialTests
{
    [TestFixture]
    public class CycleTestFixture
    {
        public Path<int> CreatePath(int startingPoint, params Arrow<int>[] arrows)
        {
            return new Path<int>(startingPoint, arrows);
        }

        public DetachedCycle<int> CreateCycle(int startingPoint, params Arrow<int>[] arrows)
        {
            return new DetachedCycle<int>(new Path<int>(startingPoint, arrows));
        }

        [Test]
        public void Constructor_ThrowsOnNullPath()
        {
            Assert.That(() => new DetachedCycle<int>(null as Path<int>), Throws.ArgumentNullException);
        }

        [Test]
        public void Constructor_ThrowsOnNullArrows()
        {
            Assert.That(() => new DetachedCycle<int>(null as Arrow<int>[]), Throws.ArgumentNullException);
        }

        [Test]
        public void Constructor_ThrowsOnNonclosedPath()
        {
            var path = CreatePath(1, new Arrow<int>(1, 2), new Arrow<int>(2, 3), new Arrow<int>(3, 4));
            Assert.That(() => new DetachedCycle<int>(path), Throws.ArgumentException);
        }

        [Test]
        public void Constructor_DoesNotThrowOnEmptyPath()
        {
            var path = CreatePath(1);
            Assert.That(() => new DetachedCycle<int>(path), Throws.Nothing);
        }

        [Test]
        public void CanonicalPath_TypicalCase()
        {
            var cycle = CreateCycle(3, new Arrow<int>(3, 2), new Arrow<int>(2, 1), new Arrow<int>(1, 4), new Arrow<int>(4, 3));
            var expectedPath = CreatePath(1, new Arrow<int>(1, 4), new Arrow<int>(4, 3), new Arrow<int>(3, 2), new Arrow<int>(2, 1));
            Assert.That(cycle.CanonicalPath, Is.EqualTo(expectedPath));
        }

        [Test]
        public void CanonicalPath_WhenSeveralCyclicPermutationsWork()
        {
            var cycle = CreateCycle(3, new Arrow<int>(3, 4), new Arrow<int>(4, 1), new Arrow<int>(1, 3), new Arrow<int>(3, 4), new Arrow<int>(4, 1), new Arrow<int>(1, 3));
            var expectedPath = CreatePath(1, new Arrow<int>(1, 3), new Arrow<int>(3, 4), new Arrow<int>(4, 1), new Arrow<int>(1, 3), new Arrow<int>(3, 4), new Arrow<int>(4, 1));
            Assert.That(cycle.CanonicalPath, Is.EqualTo(expectedPath));
        }

        [Test]
        public void CanonicalPath_WhenASingleCyclicPermutationWorksButTheLeastVertexAppearsMoreThanOnce()
        {
            var cycle = CreateCycle(3, new Arrow<int>(3, 4), new Arrow<int>(4, 1), new Arrow<int>(1, 3), new Arrow<int>(3, 4), new Arrow<int>(4, 1), new Arrow<int>(1, 2), new Arrow<int>(2, 3));
            var expectedPath = CreatePath(1, new Arrow<int>(1, 2), new Arrow<int>(2, 3), new Arrow<int>(3, 4), new Arrow<int>(4, 1), new Arrow<int>(1, 3), new Arrow<int>(3, 4), new Arrow<int>(4, 1));
            Assert.That(cycle.CanonicalPath, Is.EqualTo(expectedPath));
        }

        [Test]
        public void Equals_OnNull()
        {
            var cycle = CreateCycle(1, new Arrow<int>(1, 2), new Arrow<int>(2, 3), new Arrow<int>(3, 1));
            Assert.That(cycle.Equals(null), Is.False);
        }

        [Test]
        public void Equals_OnEmptyCycle()
        {
            var cycle1 = CreateCycle(1);
            var cycle2 = CreateCycle(1);
            Assert.That(cycle1.Equals(cycle2), Is.True);
        }

        [Test]
        public void Equals_TypicalCase()
        {
            var cycle1 = CreateCycle(1, new Arrow<int>(1, 2), new Arrow<int>(2, 3), new Arrow<int>(3, 4), new Arrow<int>(4, 1));
            var cycle2 = CreateCycle(3, new Arrow<int>(3, 4), new Arrow<int>(4, 1), new Arrow<int>(1, 2), new Arrow<int>(2, 3));
            Assert.That(cycle1.Equals(cycle2), Is.True);
        }

        [Test]
        public void EqOperator_OnNull()
        {
            DetachedCycle<int> nullCycle = null;
            var cycle = CreateCycle(1, new Arrow<int>(1, 2), new Arrow<int>(2, 3), new Arrow<int>(3, 1));
            Assert.That(nullCycle == null, Is.True);
            Assert.That(cycle == null, Is.False);
        }

        [Test]
        public void EqOperator_OnEmptyCycle()
        {
            var cycle1 = CreateCycle(1);
            var cycle2 = CreateCycle(1);
            Assert.That(cycle1 == cycle2, Is.True);
        }

        [Test]
        public void EqOperator_TypicalCase()
        {
            var cycle1 = CreateCycle(1, new Arrow<int>(1, 2), new Arrow<int>(2, 3), new Arrow<int>(3, 4), new Arrow<int>(4, 1));
            var cycle2 = CreateCycle(3, new Arrow<int>(3, 4), new Arrow<int>(4, 1), new Arrow<int>(1, 2), new Arrow<int>(2, 3));
            Assert.That(cycle1 == cycle2, Is.True);
        }

        [Test]
        public void NeqOperator_OnNull()
        {
            DetachedCycle<int> nullCycle = null;
            var cycle = CreateCycle(1, new Arrow<int>(1, 2), new Arrow<int>(2, 3), new Arrow<int>(3, 1));
            Assert.That(nullCycle != null, Is.False);
            Assert.That(cycle != null, Is.True);
        }

        [Test]
        public void NeqOperator_OnEmptyCycle()
        {
            var cycle1 = CreateCycle(1);
            var cycle2 = CreateCycle(1);
            Assert.That(cycle1 != cycle2, Is.False);
        }

        [Test]
        public void NeqOperator_TypicalCase()
        {
            var cycle1 = CreateCycle(1, new Arrow<int>(1, 2), new Arrow<int>(2, 3), new Arrow<int>(3, 4), new Arrow<int>(4, 1));
            var cycle2 = CreateCycle(3, new Arrow<int>(3, 4), new Arrow<int>(4, 1), new Arrow<int>(1, 2), new Arrow<int>(2, 3));
            Assert.That(cycle1 != cycle2, Is.False);
        }
    }
}
