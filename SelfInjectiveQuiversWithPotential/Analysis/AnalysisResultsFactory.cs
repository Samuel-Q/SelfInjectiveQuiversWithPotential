using SelfInjectiveQuiversWithPotential.Plane;
using System;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    /// <summary>
    /// This class encapsulates the logic for creating instances of various types extending the
    /// <see cref="AnalysisResults{TVertex, TMainResult}"/>.
    /// </summary>
    public static class AnalysisResultsFactory
    {
        /// <summary>
        /// Creates an object of a class implementing the <see cref="IQPAnalysisResults{TVertex}"/>
        /// interface that wraps the specified
        /// <see cref="ISemimonomialUnboundQuiverAnalysisResults{TVertex}"/>.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertices in the quiver.</typeparam>
        /// <param name="suqResults">The results to wrap.</param>
        /// <returns>An instance of the <see cref=""/></returns>
        public static IQPAnalysisResults<TVertex> CreateQPAnalysisResults<TVertex>(ISemimonomialUnboundQuiverAnalysisResults<TVertex> suqResults)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            if (suqResults is null) throw new ArgumentNullException(nameof(suqResults));

            var mainResult = (QPAnalysisMainResult)suqResults.MainResult;
            return new QPAnalysisResults<TVertex>(
                mainResult,
                suqResults.MaximalPathRepresentatives,
                suqResults.NakayamaPermutation,
                suqResults.LongestPathEncountered);
        }

        /// <summary>
        /// Creates an object of a class implementing the
        /// <see cref="IQuiverInPlaneAnalysisResults{TVertex}"/> interface for the case that the QP
        /// extraction was unsuccessful.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertices in the quiver.</typeparam>
        /// <param name="qpExtractionResult">The QP extraction result.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="qpExtractionResult"/> is
        /// not a member of the <see cref="QPExtractionResult"/> enum.</exception>
        /// <exception cref="ArgumentException"><paramref name="qpExtractionResult"/> is
        /// <see cref="QPExtractionResult.Success"/>.</exception>
        public static IQuiverInPlaneAnalysisResults<TVertex> CreateQuiverInPlaneAnalysisResults<TVertex>(QPExtractionResult qpExtractionResult)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            QuiverInPlaneAnalysisMainResult mainResult;
            switch (qpExtractionResult)
            {
                case QPExtractionResult.Success: throw new ArgumentException($"The QP extraction result must indicate a failure.");
                case QPExtractionResult.QuiverHasLoops: mainResult = QuiverInPlaneAnalysisMainResult.QuiverHasLoops; break;
                case QPExtractionResult.QuiverHasAntiParallelArrows: mainResult = QuiverInPlaneAnalysisMainResult.QuiverHasAntiParallelArrows; break;
                case QPExtractionResult.QuiverIsNotPlane: mainResult = QuiverInPlaneAnalysisMainResult.QuiverIsNotPlane; break;
                case QPExtractionResult.QuiverHasFaceWithInconsistentOrientation: mainResult = QuiverInPlaneAnalysisMainResult.QuiverHasFaceWithInconsistentOrientation; break;
                default: throw new ArgumentOutOfRangeException($"The QP extraction result {qpExtractionResult} is not among the valid QP extraction results.");
            }

            return new QuiverInPlaneAnalysisResults<TVertex>(mainResult, null, null, null);
        }

        /// <summary>
        /// Creates an object of a class implementing the
        /// <see cref="IQuiverInPlaneAnalysisResults{TVertex}"/> interface for the case that the QP
        /// extraction was successful and the extracted QP was analyzed.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertices in the quiver.</typeparam>
        /// <param name="qpAnalysisResults">The results of the analysis of the QP.</param>
        public static IQuiverInPlaneAnalysisResults<TVertex> CreateQuiverInPlaneAnalysisResults<TVertex>(IQPAnalysisResults<TVertex> qpAnalysisResults)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            if (qpAnalysisResults is null) throw new ArgumentNullException(nameof(qpAnalysisResults));

            var qpMainResult = qpAnalysisResults.MainResult;
            // Shift up the non-first bits (corresponding to everything except Success) by 4 (which is
            // the number of non-success members in the QPExtractionResult enum)
            qpMainResult = (QPAnalysisMainResult)((int)(qpAnalysisResults.MainResult & ~QPAnalysisMainResult.Success) << 4) | (qpMainResult & QPAnalysisMainResult.Success);
            var mainResult = (QuiverInPlaneAnalysisMainResult)qpMainResult;

            return new QuiverInPlaneAnalysisResults<TVertex>(
                mainResult,
                qpAnalysisResults.MaximalPathRepresentatives,
                qpAnalysisResults.NakayamaPermutation,
                qpAnalysisResults.LongestPathEncountered);
        }
    }
}
