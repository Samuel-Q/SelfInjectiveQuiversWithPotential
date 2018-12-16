using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;
using SelfInjectiveQuiversWithPotential;
using SelfInjectiveQuiversWithPotential.Plane;
using SelfInjectiveQuiversWithPotential.Analysis;

namespace SelfInjectiveQuiversWithPotentialTests
{
    [TestFixture]
    public class QuiverInPlaneAnalyzerTestFixture
    {
        public (QuiverInPlaneAnalyzer, QuiverInPlaneAnalysisSettings) CreateAnalyzerWithSettings()
        {
            return (new QuiverInPlaneAnalyzer(), new QuiverInPlaneAnalysisSettings(detectNonCancellativity: true));
        }

        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(10)]
        public void Analyze_WorksOnCounterclockwiseCycle(int cycleLength)
        {
            const double Radius = 1000;
            var vertices = Enumerable.Range(0, cycleLength).ToList();
            var arrows = vertices.Select(k => new Arrow<int>(k, (k+1).Modulo(cycleLength)));
            double baseAngle = 2 * Math.PI / cycleLength;
            var vertexPositions = vertices.ToDictionary(k => k, k => new Point((int)(Radius * Math.Cos(k * baseAngle)), (int)Math.Round(Radius * Math.Sin(k * baseAngle))));
            var quiverInPlane = new QuiverInPlane<int>(vertices, arrows, vertexPositions);

            var (analyzer, settings) = CreateAnalyzerWithSettings();

            var results = analyzer.Analyze(quiverInPlane, settings);
            Assert.That(results.MainResult, Is.EqualTo(QuiverInPlaneAnalysisMainResult.Success | QuiverInPlaneAnalysisMainResult.QPIsSelfInjective));
            var expectedMaximalPathRepresentatives = vertices.ToDictionary(
                k => k,
                k => new Path<int>[] { new Path<int>(Enumerable.Range(k, vertices.Count - 1).Select(l => l.Modulo(vertices.Count))) });
            Assert.That(results.MaximalPathRepresentatives, Is.EqualTo(expectedMaximalPathRepresentatives));

            var expectedNakayamaPermutation = vertices.ToDictionary(k => k, k => (k - 2).Modulo(vertices.Count));
            Assert.That(results.NakayamaPermutation.UnderlyingDictionary, Is.EqualTo(expectedNakayamaPermutation));
        }

        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(10)]
        public void Analyze_WorksOnClockwiseCycle(int cycleLength)
        {
            const double Radius = 1000;
            var vertices = Enumerable.Range(0, cycleLength).ToList();
            var arrows = vertices.Select(k => new Arrow<int>(k, (k + 1).Modulo(cycleLength)));
            double baseAngle = 2 * Math.PI / cycleLength;
            var vertexPositions = vertices.ToDictionary(k => k, k => new Point((int)(Radius * Math.Cos(-k * baseAngle)), (int)Math.Round(Radius * Math.Sin(-k * baseAngle))));
            var quiverInPlane = new QuiverInPlane<int>(vertices, arrows, vertexPositions);

            var (analyzer, settings) = CreateAnalyzerWithSettings();

            var results = analyzer.Analyze(quiverInPlane, settings);
            Assert.That(results.MainResult, Is.EqualTo(QuiverInPlaneAnalysisMainResult.Success | QuiverInPlaneAnalysisMainResult.QPIsSelfInjective));
            var expectedMaximalPathRepresentatives = vertices.ToDictionary(
                k => k,
                k => new Path<int>[] { new Path<int>(Enumerable.Range(k, vertices.Count - 1).Select(l => l.Modulo(vertices.Count))) });
            Assert.That(results.MaximalPathRepresentatives.ToList(), Is.EqualTo(expectedMaximalPathRepresentatives));

            var expectedNakayamaPermutation = vertices.ToDictionary(k => k, k => (k - 2).Modulo(vertices.Count));
            Assert.That(results.NakayamaPermutation.UnderlyingDictionary, Is.EqualTo(expectedNakayamaPermutation));
        }
    }
}
