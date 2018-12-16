using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential.Plane;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    /// <summary>
    /// Classes implementing this interface represent an analyzer with which to analyze quivers in
    /// the plane.
    /// </summary>
    /// <remarks>
    /// <para>The counterpart for semimonomial unbound quivers is
    /// <see cref="ISemimonomialUnboundQuiverAnalyzer"/> and the counterpart for QPs is
    /// <see cref="IQPAnalyzer"/>.</para>
    /// </remarks>
    public interface IQuiverInPlaneAnalyzer
    {
        IQuiverInPlaneAnalysisResults<TVertex> Analyze<TVertex>(QuiverInPlane<TVertex> quiverInPlane, QuiverInPlaneAnalysisSettings settings)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>;
    }
}
