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
    class AnalysisResultsFactoryTestFixture
    {
        [Test]
        public void CreateQuiverInPlaneAnalysisResults_QPExtractionResult_ThrowsOnSuccessfulQPExtraction()
        {
            Assert.That(() => AnalysisResultsFactory.CreateQuiverInPlaneAnalysisResults<int>(QPExtractionResult.Success), Throws.ArgumentException);
        }

        [Test]
        public void CreateQuiverInPlaneAnalysisResults_QPExtractionResult_WorksOnUnsuccessfulQPExtraction()
        {
            var qpExtractionResult = QPExtractionResult.QuiverHasLoops;
            var results = AnalysisResultsFactory.CreateQuiverInPlaneAnalysisResults<int>(qpExtractionResult);
            Assert.That(results.MainResults, Is.EqualTo(QuiverInPlaneAnalysisMainResults.QuiverHasLoops));
            Assert.That(results.MaximalPathRepresentatives, Is.Null);
            Assert.That(results.NakayamaPermutation, Is.Null);

            qpExtractionResult = QPExtractionResult.QuiverHasAntiParallelArrows;
            results = AnalysisResultsFactory.CreateQuiverInPlaneAnalysisResults<int>(qpExtractionResult);
            Assert.That(results.MainResults, Is.EqualTo(QuiverInPlaneAnalysisMainResults.QuiverHasAntiParallelArrows));
            Assert.That(results.MaximalPathRepresentatives, Is.Null);
            Assert.That(results.NakayamaPermutation, Is.Null);

            qpExtractionResult = QPExtractionResult.QuiverIsNotPlane;
            results = AnalysisResultsFactory.CreateQuiverInPlaneAnalysisResults<int>(qpExtractionResult);
            Assert.That(results.MainResults, Is.EqualTo(QuiverInPlaneAnalysisMainResults.QuiverIsNotPlane));
            Assert.That(results.MaximalPathRepresentatives, Is.Null);
            Assert.That(results.NakayamaPermutation, Is.Null);

            qpExtractionResult = QPExtractionResult.QuiverHasFaceWithInconsistentOrientation;
            results = AnalysisResultsFactory.CreateQuiverInPlaneAnalysisResults<int>(qpExtractionResult);
            Assert.That(results.MainResults, Is.EqualTo(QuiverInPlaneAnalysisMainResults.QuiverHasFaceWithInconsistentOrientation));
            Assert.That(results.MaximalPathRepresentatives, Is.Null);
            Assert.That(results.NakayamaPermutation, Is.Null);
        }

        [Test]
        public void CreateQuiverInPlaneAnalysisResults_IQPAnalysisResultsOfTVertex_SetsMainResultCorrectly()
        {
            var defaultMaximalReps = new Dictionary<int, IEnumerable<Path<int>>>();
            var defaultNakayamaPermutation = new NakayamaPermutation<int>(new Dictionary<int, int>());
            var defaultLongestPath = new Path<int>(startingPoint: 1);

            var qpAnalysisResults = CreateQPAnalysisResults(
                QPAnalysisMainResults.Success,
                defaultMaximalReps,
                defaultNakayamaPermutation,
                defaultLongestPath);

            var results = AnalysisResultsFactory.CreateQuiverInPlaneAnalysisResults(qpAnalysisResults);
            Assert.That(results.MainResults, Is.EqualTo(QuiverInPlaneAnalysisMainResults.Success));

            qpAnalysisResults = CreateQPAnalysisResults(
                QPAnalysisMainResults.Success,
                defaultMaximalReps,
                defaultNakayamaPermutation,
                defaultLongestPath);
            results = AnalysisResultsFactory.CreateQuiverInPlaneAnalysisResults(qpAnalysisResults);
            Assert.That(results.MainResults, Is.EqualTo(QuiverInPlaneAnalysisMainResults.Success));
            Assert.That(results.MainResults.IndicatesSelfInjectivity());

            qpAnalysisResults = CreateQPAnalysisResults(
                QPAnalysisMainResults.Aborted,
                null,
                null,
                defaultLongestPath);
            results = AnalysisResultsFactory.CreateQuiverInPlaneAnalysisResults(qpAnalysisResults);
            Assert.That(results.MainResults, Is.EqualTo(QuiverInPlaneAnalysisMainResults.QPAnalysisAborted));

            qpAnalysisResults = CreateQPAnalysisResults(
                QPAnalysisMainResults.Cancelled,
                null,
                null,
                defaultLongestPath);
            results = AnalysisResultsFactory.CreateQuiverInPlaneAnalysisResults(qpAnalysisResults);
            Assert.That(results.MainResults, Is.EqualTo(QuiverInPlaneAnalysisMainResults.QPAnalysisCancelled));

            qpAnalysisResults = CreateQPAnalysisResults(
                QPAnalysisMainResults.NotCancellative,
                null,
                null,
                defaultLongestPath);
            results = AnalysisResultsFactory.CreateQuiverInPlaneAnalysisResults(qpAnalysisResults);
            Assert.That(results.MainResults, Is.EqualTo(QuiverInPlaneAnalysisMainResults.QPIsNotCancellative));

            QPAnalysisResults<int> CreateQPAnalysisResults(
                QPAnalysisMainResults mainResult,
                Dictionary<int, IEnumerable<Path<int>>> maximalPathRepresentatives,
                NakayamaPermutation<int> nakayamaPermutation,
                Path<int> longestPathEncountered)
            {
                return new QPAnalysisResults<int>(mainResult, maximalPathRepresentatives, nakayamaPermutation, longestPathEncountered);
            }
        }
    }
}
