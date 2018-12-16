using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;

namespace SelfInjectiveQuiversWithPotentialTests
{
    using SelfInjectiveQuiversWithPotential;

    [TestFixture]
    public class QPIsomorphismCheckerTestFixture
    {
        [Test]
        public void AreIsomorphic_SmallQP_DifferentSign()
        {
            var potential1 = new Potential<int>(new DetachedCycle<int>(new Path<int>(1, 2, 3, 1)), +1);
            var potential2 = new Potential<int>(new DetachedCycle<int>(new Path<int>(1, 2, 3, 1)), -1);
            var qp1 = new QuiverWithPotential<int>(potential1);
            var qp2 = new QuiverWithPotential<int>(potential2);
            var checker = new QPIsomorphismChecker();
            Assert.That(checker.AreIsomorphic(qp1, qp2), Is.False);
        }

        [Test]
        public void AreIsomorphic_SmallQP1()
        {
            var potential1 = new Potential<int>(new DetachedCycle<int>(new Path<int>(1, 2, 3, 1)), +1);
            var potential2 = new Potential<int>(new DetachedCycle<int>(new Path<int>(3, 2, 1, 3)), +1);
            var qp1 = new QuiverWithPotential<int>(potential1);
            var qp2 = new QuiverWithPotential<int>(potential2);
            var checker = new QPIsomorphismChecker();
            Assert.That(checker.AreIsomorphic(qp1, qp2), Is.True);
        }

        [Test]
        public void AreIsomorphic_SmallQP2()
        {
            var potential1 = new Potential<int>(new DetachedCycle<int>(new Path<int>(1, 2, 3, 1)), +1)
                                      .AddCycle(new DetachedCycle<int>(new Path<int>(4, 5, 6, 4)), +1);
            var potential2 = new Potential<int>(new DetachedCycle<int>(new Path<int>(3, 2, 1, 3)), +1);
            var qp1 = new QuiverWithPotential<int>(potential1);
            var qp2 = new QuiverWithPotential<int>(potential2);
            var checker = new QPIsomorphismChecker();
            Assert.That(checker.AreIsomorphic(qp1, qp2), Is.False);
        }

        [Test]
        public void AreIsomorphic_SmallQP3()
        {
            var potential1 = new Potential<int>(new DetachedCycle<int>(new Path<int>(1, 2, 3, 1)), +1)
                                      .AddCycle(new DetachedCycle<int>(new Path<int>(4, 5, 6, 4)), +1);
            var potential2 = new Potential<int>(new DetachedCycle<int>(new Path<int>(3, 2, 1, 3)), +1);
            var qp1 = new QuiverWithPotential<int>(potential1);
            var quiver2 = qp1.Quiver;
            var qp2 = new QuiverWithPotential<int>(quiver2, potential2);
            var checker = new QPIsomorphismChecker();
            Assert.That(checker.AreIsomorphic(qp1, qp2), Is.False);
        }

        [Test]
        public void AreIsomorphic_SmallQP4()
        {
            var potential1 = new Potential<int>(new DetachedCycle<int>(new Path<int>(1, 2, 3, 1)), +1)
                                      .AddCycle(new DetachedCycle<int>(new Path<int>(4, 5, 6, 4)), +1);
            var potential2 = new Potential<int>(new DetachedCycle<int>(new Path<int>(3, 2, 1, 3)), +1);
            var qp1 = new QuiverWithPotential<int>(potential1);
            var quiver2 = qp1.Quiver;
            var qp2 = new QuiverWithPotential<int>(quiver2, potential2);
            var checker = new QPIsomorphismChecker();
            Assert.That(checker.AreIsomorphic(qp2, qp1), Is.False);
        }

        [Test]
        public void AreIsomorphic_OnTriangleQPs()
        {
            var potential1 = new Potential<char>(new DetachedCycle<char>(new Path<char>('B', 'C', 'E', 'B')), +1)
                                       .AddCycle(new DetachedCycle<char>(new Path<char>('A', 'B', 'C', 'A')), -1)
                                       .AddCycle(new DetachedCycle<char>(new Path<char>('B', 'D', 'E', 'B')), -1)
                                       .AddCycle(new DetachedCycle<char>(new Path<char>('C', 'E', 'F', 'C')), -1);

            var potential2 = new Potential<char>(new DetachedCycle<char>(new Path<char>('B', 'C', 'F', 'B')), +1)
                                       .AddCycle(new DetachedCycle<char>(new Path<char>('C', 'F', 'E', 'C')), -1)
                                       .AddCycle(new DetachedCycle<char>(new Path<char>('B', 'C', 'D', 'B')), -1)
                                       .AddCycle(new DetachedCycle<char>(new Path<char>('A', 'F', 'B', 'A')), -1);

            var qp1 = new QuiverWithPotential<char>(potential1);
            var qp2 = new QuiverWithPotential<char>(potential2);
            var checker = new QPIsomorphismChecker();
            Assert.That(checker.AreIsomorphic(qp1, qp2), Is.True);
        }

        [Test]
        public void AreIsomorphic_OnIsomorphicQuiversButNonIsomorphicPotentials_DifferentGroupSignatures()
        {
            var potential1 = new Potential<char>(new DetachedCycle<char>(new Path<char>('B', 'C', 'E', 'B')), +1)
                                       .AddCycle(new DetachedCycle<char>(new Path<char>('A', 'B', 'C', 'A')), -1)
                                       .AddCycle(new DetachedCycle<char>(new Path<char>('B', 'D', 'E', 'B')), -1)
                                       .AddCycle(new DetachedCycle<char>(new Path<char>('C', 'E', 'F', 'C')), -1);

            var potential2 = new Potential<char>(new DetachedCycle<char>(new Path<char>('B', 'C', 'F', 'B')), +1);

            var qp1 = new QuiverWithPotential<char>(potential1);

            var vertices2 = new char[] { 'A', 'B', 'C', 'D', 'E', 'F' };
            var arrows2 = new Arrow<char>[]
            {
                new Arrow<char>('A', 'B'),
                new Arrow<char>('B', 'C'),
                new Arrow<char>('B', 'D'),
                new Arrow<char>('C', 'A'),
                new Arrow<char>('C', 'E'),
                new Arrow<char>('D', 'E'),
                new Arrow<char>('E', 'B'),
                new Arrow<char>('E', 'F'),
                new Arrow<char>('F', 'C'),
            };
            var quiver2 = new Quiver<char>(vertices2, arrows2);
            var qp2 = new QuiverWithPotential<char>(potential2);
            var checker = new QPIsomorphismChecker();
            Assert.That(checker.AreIsomorphic(qp1, qp2), Is.False);
        }

        [Test]
        public void AreIsomorphic_OnIsomorphicQuiversButNonIsomorphicPotentials_SameGroupSignaturesButDifferentCounts()
        {
            var potential1 = new Potential<char>(new DetachedCycle<char>(new Path<char>('B', 'C', 'E', 'B')), +1)
                                       .AddCycle(new DetachedCycle<char>(new Path<char>('A', 'B', 'C', 'A')), -1)
                                       .AddCycle(new DetachedCycle<char>(new Path<char>('B', 'D', 'E', 'B')), -1)
                                       .AddCycle(new DetachedCycle<char>(new Path<char>('C', 'E', 'F', 'C')), -1);

            var potential2 = new Potential<char>(new DetachedCycle<char>(new Path<char>('B', 'C', 'F', 'B')), +1)
                                       .AddCycle(new DetachedCycle<char>(new Path<char>('B', 'D', 'E', 'B')), -1)
                                       .AddCycle(new DetachedCycle<char>(new Path<char>('C', 'E', 'F', 'C')), -1);

            var qp1 = new QuiverWithPotential<char>(potential1);

            var vertices2 = new char[] { 'A', 'B', 'C', 'D', 'E', 'F' };
            var arrows2 = new Arrow<char>[]
            {
                new Arrow<char>('A', 'B'),
                new Arrow<char>('B', 'C'),
                new Arrow<char>('B', 'D'),
                new Arrow<char>('C', 'A'),
                new Arrow<char>('C', 'E'),
                new Arrow<char>('D', 'E'),
                new Arrow<char>('E', 'B'),
                new Arrow<char>('E', 'F'),
                new Arrow<char>('F', 'C'),
            };
            var quiver2 = new Quiver<char>(vertices2, arrows2);
            var qp2 = new QuiverWithPotential<char>(quiver2, potential2);
            var checker = new QPIsomorphismChecker();
            Assert.That(checker.AreIsomorphic(qp1, qp2), Is.False);
        }

        [Test]
        public void AreIsomorphic_OnIsomorphicQuiversButNonIsomorphicPotentials_SameGroupSignaturesAndSameCounts()
        {
            var potential1 = new Potential<char>(new DetachedCycle<char>(new Path<char>('B', 'C', 'E', 'B')), +1)
                                       .AddCycle(new DetachedCycle<char>(new Path<char>('A', 'B', 'C', 'A')), -1)
                                       .AddCycle(new DetachedCycle<char>(new Path<char>('B', 'D', 'E', 'B')), -1);

            var potential2 = new Potential<char>(new DetachedCycle<char>(new Path<char>('B', 'C', 'E', 'B')), +1)
                                       .AddCycle(new DetachedCycle<char>(new Path<char>('A', 'B', 'C', 'A')), -1)
                                       .AddCycle(new DetachedCycle<char>(new Path<char>('C', 'E', 'F', 'C')), -1);

            var qp1 = new QuiverWithPotential<char>(potential1);

            var vertices2 = new char[] { 'A', 'B', 'C', 'D', 'E', 'F' };
            var arrows2 = new Arrow<char>[]
            {
                new Arrow<char>('A', 'B'),
                new Arrow<char>('B', 'C'),
                new Arrow<char>('B', 'D'),
                new Arrow<char>('C', 'A'),
                new Arrow<char>('C', 'E'),
                new Arrow<char>('D', 'E'),
                new Arrow<char>('E', 'B'),
                new Arrow<char>('E', 'F'),
                new Arrow<char>('F', 'C'),
            };
            var quiver2 = new Quiver<char>(vertices2, arrows2);
            var qp2 = new QuiverWithPotential<char>(quiver2, potential2);
            var checker = new QPIsomorphismChecker();
            Assert.That(checker.AreIsomorphic(qp1, qp2), Is.False);
        }
    }
}
