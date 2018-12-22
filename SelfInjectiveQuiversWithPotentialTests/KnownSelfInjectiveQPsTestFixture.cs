using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SelfInjectiveQuiversWithPotential;
using SelfInjectiveQuiversWithPotential.Analysis;

namespace SelfInjectiveQuiversWithPotentialTests
{
    // The current tests belong in QPAnalyzerTestFixture or so imo.

    [TestFixture]
    public class KnownSelfInjectiveQPsTestFixture
    {
        private void AssertIsSelfInjectiveWithCorrectNakayamaPermutation<TVertex>(SelfInjectiveQP<TVertex> selfInjectiveQP)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            var analyzer = new QPAnalyzer();
            var result = analyzer.Analyze(selfInjectiveQP.QP, new QPAnalysisSettings(detectNonCancellativity: true));
            Assert.That(result.MainResult.HasFlag(QPAnalysisMainResult.SelfInjective));
            Assert.That(selfInjectiveQP.NakayamaPermutation.Equals(result.NakayamaPermutation));
        }

        private void AssertAreSelfInjectiveWithCorrectNakayamaPermutation<TVertex>(IEnumerable<SelfInjectiveQP<TVertex>> selfInjectiveQPs)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            foreach (var selfInjectiveQP in selfInjectiveQPs)
            {
                AssertIsSelfInjectiveWithCorrectNakayamaPermutation(selfInjectiveQP);
            }
        }

        private KnownSelfInjectiveQPs CreateKnownSelfInjectiveQPs()
        {
            return new KnownSelfInjectiveQPs();
        }

        [Test]
        public void Cycles_AreSelfInjectiveWithCorrectNakayamaPermutation()
        {
            var ksiqps = CreateKnownSelfInjectiveQPs();
            AssertAreSelfInjectiveWithCorrectNakayamaPermutation(ksiqps.Cycles.TakeWhile(siQp => siQp.QP.Quiver.Vertices.Count < 30));
        }

        [Test]
        public void Triangles_AreSelfInjectiveWithCorrectNakayamaPermutation()
        {
            var ksiqps = CreateKnownSelfInjectiveQPs();
            AssertAreSelfInjectiveWithCorrectNakayamaPermutation(ksiqps.Triangles.TakeWhile(siQp => siQp.QP.Quiver.Vertices.Count < 30));
        }

        [Test]
        public void Squares_AreSelfInjectiveWithCorrectNakayamaPermutation()
        {
            var ksiqps = CreateKnownSelfInjectiveQPs();
            AssertAreSelfInjectiveWithCorrectNakayamaPermutation(ksiqps.Squares.TakeWhile(siQp => siQp.QP.Quiver.Vertices.Count < 30));
        }

        [Test]
        public void Cobwebs_AreSelfInjectiveWithCorrectNakayamaPermutation()
        {
            var ksiqps = CreateKnownSelfInjectiveQPs();
            AssertAreSelfInjectiveWithCorrectNakayamaPermutation(ksiqps.Cobwebs.TakeWhile(siQp => siQp.QP.Quiver.Vertices.Count < 30));
        }

        [Test]
        public void OddFlowers_AreSelfInjectiveWithCorrectNakayamaPermutation()
        {
            var ksiqps = CreateKnownSelfInjectiveQPs();
            AssertAreSelfInjectiveWithCorrectNakayamaPermutation(ksiqps.OddFlowers.TakeWhile(siQp => siQp.QP.Quiver.Vertices.Count <= 35));
        }
    }
}
