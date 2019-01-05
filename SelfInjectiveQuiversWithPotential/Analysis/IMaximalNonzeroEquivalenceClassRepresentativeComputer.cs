using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Analysis
{
    /// <summary>
    /// Classes implementing this interface can be used to compute the maximal nonzero equivalence
    /// classes of paths (or representatives of these equivalence classes, to be precise) with a
    /// given starting vertex in a semimonomial unbound quiver.
    /// </summary>
    /// <remarks>
    /// <para>The mathematical significance of this interface is as follows.
    /// In a cancellative semimonomial bound quiver, the maximal nonzero equivalence
    /// classes of paths with a given starting vertex <c>i</c> form a basis of the socle of the
    /// indecomposable projective module corresponding to <c>i</c>. From this basis, one can easily
    /// read off whether the bound quiver has a Nakayama permutation and hence whether the bound
    /// quiver algebra is self-injective. Thus, one can think of the classes implementing this
    /// interface as doing the real work needed to determine self-injectivity.</para></remarks>
    public interface IMaximalNonzeroEquivalenceClassRepresentativeComputer
    {
        /// <summary>
        /// Gets a boolean value indicating whether the computer has support for detecting
        /// non-cancellativity.
        /// </summary>
        bool SupportsNonCancellativityDetection { get; }

        /// <summary>
        /// Gets a boolean value indicating whether the computer has support for detecting
        /// failure of weak cancellativity.
        /// </summary>
        bool SupportsNonWeakCancellativityDetection { get; }

        /// <summary>
        /// Gets a boolean value indicating whether the computer has support for dealing with
        /// non-admissibility.
        /// </summary>
        /// <remarks>
        /// <para>&quot;Dealing with non-admissibility&quot; for now means aborting the computation
        /// when a too long path is encountered.</para>
        /// </remarks>
        bool SupportsNonAdmissibilityHandling { get; }

        /// <summary>
        /// Computes a transversal of the maximal nonzero equivalence classes of paths starting at
        /// a specified vertex in a semimonomial unbound quiver.
        /// </summary>
        /// <typeparam name="TVertex">The type of the vertices in the quiver.</typeparam>
        /// <param name="quiver">The quiver of the semimonomial unbound quiver.</param>
        /// <param name="startingVertex">The starting vertex of the paths.</param>
        /// <param name="transformationRuleTree">A <see cref="TransformationRuleTreeNode{TVertex}"/>
        /// representing the transformation rules induced by the ideal of the semimonomial unbound
        /// quiver.</param>
        /// <param name="settings">Settings for the computation.</param>
        /// <returns></returns>
        /// <remarks>
        /// <para>Because this method will presumably be called many times for a single
        /// semimonomial unbound quiver, the method is specified to take a quiver and a
        /// collection of transformation rules (<paramref name="quiver"/> and
        /// <paramref name="transformationRuleTree"/>) instead of a
        /// <see cref="SemimonomialUnboundQuiver{TVertex}"/>. This design decision alleviates
        /// implementers of the <see cref="IMaximalNonzeroEquivalenceClassRepresentativeComputer"/>
        /// interface from the burden of extracting transformation rules for the semimonomial
        /// unbound quiver while also making more sense performance-wise.</para>
        /// <para></para></remarks>
        MaximalNonzeroEquivalenceClassRepresentativesResults<TVertex> ComputeMaximalNonzeroEquivalenceClassRepresentativesStartingAt<TVertex>(
            Quiver<TVertex> quiver,
            TVertex startingVertex,
            TransformationRuleTreeNode<TVertex> transformationRuleTree,
            MaximalNonzeroEquivalenceClassRepresentativeComputationSettings settings)
            where TVertex : IEquatable<TVertex>, IComparable<TVertex>;
    }
}
