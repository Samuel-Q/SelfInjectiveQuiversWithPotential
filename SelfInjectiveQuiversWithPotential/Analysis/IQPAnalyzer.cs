using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    /// <summary>
    /// Classes implementing this interface represent an analyzer with which to analyze QPs
    /// (for self-injectivity and related notions).
    /// </summary>
    /// <remarks>
    /// <para>The counterpart for semimonomial unbound quivers is
    /// <see cref="ISemimonomialUnboundQuiverAnalyzer"/> and the counterpart for quivers in plane
    /// is <see cref="IQuiverInPlaneAnalyzer"/>.</para>
    /// </remarks>
    public interface IQPAnalyzer
    {
        IQPAnalysisResults<TVertex> Analyze<TVertex>(QuiverWithPotential<TVertex> qp, QPAnalysisSettings settings)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>;
    }
}
