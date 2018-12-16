using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    /// <summary>
    /// This namespace contains the logic for determining whether various types of quivers are
    /// self-inejctive.
    /// </summary>
    /// <remarks>
    /// <para>There are three kinds of analyzers, each with a single implementation (as of this writing):
    /// <list type="table">
    /// <listheader>
    /// <term>Interface</term>
    /// <term>Implementation</term>
    /// <term>Description</term>
    /// </listheader>
    /// <item>
    /// <term><see cref="ISemimonomialUnboundQuiverAnalyzer"/></term>
    /// <term><see cref="SemimonomialUnboundQuiverAnalyzer"/></term>
    /// <term>Analyzes semimonomial quivers that are not necessarily bound.
    /// Non-cancellativity is detected by default.</term>
    /// </item>
    /// <item>
    /// <term><see cref="IQPAnalyzer"/></term>
    /// <term><see cref="QPAnalyzer"/></term>
    /// <term>Analyzes QPs whose potential has coefficients -1 and +1 only, has every arrow in at
    /// most one positive cycle and in at most one negative cycle, and has every arrow appear at
    /// most once in every cycle.</term>
    /// </item>
    /// <item>
    /// <term><see cref="IQuiverInPlaneAnalyzer"/></term>
    /// <term><see cref="QuiverInPlaneAnalyzer"/></term>
    /// <term>Analyzes quivers embedded in the plane.</term>
    /// </item>
    /// </list>
    /// Logic is heavily reused in that <see cref="QuiverInPlaneAnalyzer"/> uses
    /// <see cref="QPAnalyzer"/> to analyze the QP induced by the plane-embedded quiver and in that
    /// <see cref="QPAnalyzer"/> uses <see cref="SemimonomialUnboundQuiverAnalyzer"/> to analyze
    /// the semimonomial unbound quiver induced by the QP.
    /// </para>
    /// <para>The &quot;real work&quot; is done by the classes implementing the
    /// <see cref="IMaximalNonzeroEquivalenceClassRepresentativeComputer"/> and
    /// <see cref="IEquivalenceClassComputer"/> interfaces.</para>
    /// </remarks>
    public static class NamespaceDoc
    {
    }
}
