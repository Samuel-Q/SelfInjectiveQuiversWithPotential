using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    /// <summary>
    /// Classes implementing this interface represent an analyzer with which to analyze
    /// semimonomial unbound quivers (for self-injectivity and related notions).
    /// </summary>
    /// <remarks>The counterpart for QPs is <see cref="IQPAnalyzer"/>, and the counterpart for
    /// quivers in plane is <see cref="IQuiverInPlaneAnalyzer"/>.</remarks>
    public interface ISemimonomialUnboundQuiverAnalyzer
    {
        ISemimonomialUnboundQuiverAnalysisResults<TVertex> Analyze<TVertex>(SemimonomialUnboundQuiver<TVertex> quiver, SemimonomialUnboundQuiverAnalysisSettings settings)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>;
    }
}
