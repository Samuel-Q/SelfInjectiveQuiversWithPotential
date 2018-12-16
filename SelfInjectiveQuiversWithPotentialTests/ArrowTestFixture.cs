using System;
using System.Collections.Generic;
using System.Linq;
using NUnit;
using NUnit.Framework;
using SelfInjectiveQuiversWithPotential;

namespace SelfInjectiveQuiversWithPotentialTests
{
    [TestFixture]
    public class ArrowTestFixture
    {
        [Test]
        public void Constructor_ThrowsOnSourceNull()
        {
            Assert.That(() => new Arrow<string>(null, "abc"), Throws.ArgumentNullException);
        }

        [Test]
        public void Constructor_ThrowsOnTargetNull()
        {
            Assert.That(() => new Arrow<string>("abc", null), Throws.ArgumentNullException);
        }

        [Test]
        public void Equals_SeveralCases()
        {
            var arrow1 = new Arrow<int>(1, 2);
            var arrow2 = new Arrow<int>(2, 1);
            var arrow3 = new Arrow<int>(1, 3);
            var arrow4 = new Arrow<int>(0, 2);
            var arrow5 = new Arrow<int>(1, 2);

            Assert.That(arrow1.Equals(arrow1), Is.True);
            Assert.That(arrow1.Equals(arrow2), Is.False);
            Assert.That(arrow1.Equals(arrow3), Is.False);
            Assert.That(arrow1.Equals(arrow4), Is.False);
            Assert.That(arrow1.Equals(arrow5), Is.True);
            Assert.That(arrow1.Equals(null), Is.False);
        }

        [Test]
        public void EqOperator_SeveralCases()
        {
            var arrow1 = new Arrow<int>(1, 2);
            var arrow2 = new Arrow<int>(2, 1);
            var arrow3 = new Arrow<int>(1, 3);
            var arrow4 = new Arrow<int>(0, 2);
            var arrow5 = new Arrow<int>(1, 2);
            Arrow<int> nullarrow = null;

            Assert.That(arrow1 == arrow1, Is.True);
            Assert.That(arrow1 == arrow2, Is.False);
            Assert.That(arrow1 == arrow3, Is.False);
            Assert.That(arrow1 == arrow4, Is.False);
            Assert.That(arrow1 == arrow5, Is.True);
            Assert.That(arrow1 == null, Is.False);
            Assert.That(nullarrow == null, Is.True);
        }

        [Test]
        public void NeqOperator_SeveralCases()
        {
            var arrow1 = new Arrow<int>(1, 2);
            var arrow2 = new Arrow<int>(2, 1);
            var arrow3 = new Arrow<int>(1, 3);
            var arrow4 = new Arrow<int>(0, 2);
            var arrow5 = new Arrow<int>(1, 2);
            Arrow<int> nullarrow = null;

            Assert.That(arrow1 != arrow1, Is.False);
            Assert.That(arrow1 != arrow2, Is.True);
            Assert.That(arrow1 != arrow3, Is.True);
            Assert.That(arrow1 != arrow4, Is.True);
            Assert.That(arrow1 != arrow5, Is.False);
            Assert.That(nullarrow != null, Is.False);
        }
    }
}
