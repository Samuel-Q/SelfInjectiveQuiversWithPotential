using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential.Analysis;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// Provides data for the <see cref="QuiverAnalyzerModel.AnalysisDone"/> event.
    /// </summary>
    public class AnalysisDoneEventArgs<TVertex> : EventArgs where TVertex : IEquatable<TVertex>, IComparable<TVertex>
    {
        public IQuiverInPlaneAnalysisResults<TVertex> AnalysisResults { get; }

        public AnalysisDoneEventArgs(IQuiverInPlaneAnalysisResults<TVertex> analysisResults)
        {
            AnalysisResults = analysisResults ?? throw new ArgumentNullException(nameof(analysisResults));
        }
    }
}
