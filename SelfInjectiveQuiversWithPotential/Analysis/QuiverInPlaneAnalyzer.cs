using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential.Plane;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    /// <summary>
    /// This class is used to analyze quivers in the plane (<see cref="QuiverInPlane{TVertex}"/>s).
    /// </summary>
    /// <remarks>This class essentially just combines the <see cref="QPExtractor"/> and the
    /// <see cref="QPAnalyzer"/> classes: use the former to extract a QP from the
    /// quiver in plane and then use the latter for analyzing the QP.</remarks>
    public class QuiverInPlaneAnalyzer : IQuiverInPlaneAnalyzer
    {
        /// <summary>
        /// Analyzes a <see cref="QuiverInPlane{TVertex}"/>.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertices in the quiver.</typeparam>
        /// <param name="quiverInPlane">The quiver in plane to analyze.</param>
        /// <returns>The analysis results.</returns>
        /// <remarks>
        /// <para>If the analysis is unsuccessful, the value of the <c>MainResult</c> property
        /// of the returned analysis results does not have the
        /// <see cref="QuiverInPlaneAnalysisMainResult.Success"/> or the
        /// <see cref="QuiverInPlaneAnalysisMainResult.QPIsSelfInjective"/> flags set and has at least
        /// one of the other flags (each of which indicates some sort of failure) set. However, in
        /// the case of multiple causes for failure (e.g., the quiver has loops and anti-parallel
        /// arrows), all the corresponding flags are not necessarily set (e.g.,
        /// <see cref="QuiverInPlaneAnalysisMainResult.QuiverHasLoops"/> is set but
        /// <see cref="QuiverInPlaneAnalysisMainResult.QuiverHasAntiParallelArrows"/> is not set,
        /// or <see cref="QuiverInPlaneAnalysisMainResult.QuiverHasAntiParallelArrows"/> is set but
        /// <see cref="QuiverInPlaneAnalysisMainResult.QuiverHasLoops"/> is not set).</para>
        /// <para>This method does not throw any exceptions (unless I've forgotten something).</para>
        /// </remarks>
        public IQuiverInPlaneAnalysisResults<TVertex> Analyze<TVertex>(
            QuiverInPlane<TVertex> quiverInPlane,
            QuiverInPlaneAnalysisSettings settings)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            if (quiverInPlane is null) throw new ArgumentNullException(nameof(quiverInPlane));

            var qpExtractor = new QPExtractor();
            var extractionResult = qpExtractor.TryExtractQP(quiverInPlane, out var qp);
            if (extractionResult != QPExtractionResult.Success)
            {
                return AnalysisResultsFactory.CreateQuiverInPlaneAnalysisResults<TVertex>(extractionResult);
            }

            var analyzer = new QPAnalyzer();
            var qpAnalyzerSettings = new QPAnalysisSettings(settings.DetectNonCancellativity, settings.MaxPathLength);
            var qpAnalysisResults = analyzer.Analyze(qp, qpAnalyzerSettings);
            var analysisResults = AnalysisResultsFactory.CreateQuiverInPlaneAnalysisResults(qpAnalysisResults);
            return analysisResults;
        }
    }
}
