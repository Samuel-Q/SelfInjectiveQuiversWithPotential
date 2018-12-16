using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Plane
{
    public enum QPExtractionResult
    {
        Success,
        QuiverHasLoops,
        QuiverHasAntiParallelArrows,
        QuiverIsNotPlane,

        /// <summary>
        /// The quiver has a face whose bounding arrows do not form a directed cycle.
        /// </summary>
        QuiverHasFaceWithInconsistentOrientation
    }
}
