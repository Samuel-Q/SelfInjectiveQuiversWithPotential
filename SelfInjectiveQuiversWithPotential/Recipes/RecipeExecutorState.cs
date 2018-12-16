using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures;

namespace SelfInjectiveQuiversWithPotential.Recipes
{
    /// <summary>
    /// This class contains the state for the <see cref="RecipeExecutor"/> while generating
    /// a potential.
    /// </summary>
    public class RecipeExecutorState
    {
        /// <summary>
        /// Gets or sets the potential generated so far.
        /// </summary>
        public Potential<int> Potential { get; set; }

        /// <summary>
        /// Gets or sets the value of the next vertex.
        /// </summary>
        /// <remarks>As an implementation detail, this is just the current vertex count for the
        /// potential.</remarks>
        public int NextVertex { get; set; }

        /// <summary>
        /// Gets or sets the boundary, consisting of arrows with an orientation (left or right).
        /// </summary>
        public CircularList<(Arrow<int> Arrow, BoundaryArrowOrientation Orientation)> Boundary { get; set; }

        /// <summary>
        /// Gets or sets the set of potentially problematic arrows, which are arrows between
        /// two nonadjacent vertices on the boundary.
        /// </summary>
        /// <remarks>
        /// <para>If there is a path of length l > 1 on the boundary from one boundary vertex to
        /// another and the vertices are neighbors, then consuming said path to glue on an
        /// (l+1)-cycle is bad (a previous cycle is cancelled). This property is used to avoid such
        /// problems.</para>
        /// <para>This set is not &quot;cleaned up&quot; and may thus contain arrows that were
        /// potentially problematic <em>at some point in time</em> but not anymore.</para></remarks>
        public ISet<Arrow<int>> PotentiallyProblematicArrows { get; set; }
    }
}
