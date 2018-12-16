using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential
{
    /// <summary>
    /// This class is used to create instances of the <see cref="SemimonomialUnboundQuiver{TVertex}"/> class.
    /// </summary>
    /// <remarks>The justification for this class is that instead of requiring either
    /// <see cref="SemimonomialUnboundQuiver{TVertex}"/> to know about the
    /// <see cref="QuiverWithPotential{TVertex}"/> class (e.g., with a
    /// <c>SemimonomialUnboundQuiver.CreateFromQP</c> method) or vice versa (e.g., with a
    /// <c>Potential.CreateSemimonomialIdeal</c> method), we put the logic involving both classes
    /// into its own class.</remarks>
    public static class SemimonomialUnboundQuiverFactory
    {
        /// <summary>
        /// Creates a new instance of the <see cref="SemimonomialUnboundQuiver{TVertex}"/> class
        /// whose ideal is the Jacobian ideal of the potential of the given QP.
        /// </summary>
        /// <param name="qp">The QP whose corresponding semimonomial unbound quiver to create.</param>
        /// <returns>The semimonomial unbound quiver induced by <paramref name="qp"/>.</returns>
        /// <exception cref="NotSupportedException">The potential of <paramref name="qp"/> has a
        /// cycle with coefficient not equal to either of -1 and +1,
        /// or some arrow occurs multiple times in a single cycle of the potential of
        /// <paramref name="qp"/>.</exception>
        /// <exception cref="ArgumentException">For some arrow in the potential of
        /// <paramref name="qp"/> and sign, the arrow is contained in more than one cycle of that
        /// sign.</exception>
        /// <remarks>
        /// <para>The preconditions on the potential of <paramref name="qp"/> as of this writing is
        /// that the the scalars are <c>-1</c> or <c>+1</c>, every arrow occurs in at most one
        /// cycle per sign, and every arrow occurs at most once per cycle.</para>
        /// <para>The reasoning behind the differentiation between <see cref="NotSupportedException"/>
        /// and <see cref="ArgumentException"/> is that <see cref="NotSupportedException"/> is
        /// thrown when things might work out in theory (e.g., all scalars could be -2 or +2
        /// instead of -1 or +1, or an arrow could appear more than once in a cycle if the cycle is
        /// a suitable power of some other cycle (and stars align if the arrow is also contained in
        /// a cycle of the opposite sign)), while <see cref="ArgumentException"/> is thrown when
        /// things do not work out even in theory (i.e., semimonomiality fails to hold). That is,
        /// <see cref="NotSupportedException"/> is thrown in cases that might be &quot;fixed&quot;
        /// in the future.</para>
        /// </remarks>
        public static SemimonomialUnboundQuiver<TVertex> CreateSemimonomialUnboundQuiverFromQP<TVertex>(QuiverWithPotential<TVertex> qp)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>
        {
            var semimonomialIdeal = SemimonomialIdealFactory.CreateSemimonomialIdealFromPotential(qp.Potential);
            var semimonomialUnboundQuiver = new SemimonomialUnboundQuiver<TVertex>(qp.Quiver, semimonomialIdeal);
            return semimonomialUnboundQuiver;
        }
    }
}
