using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Recipes
{
    /// <summary>
    /// Defines the orientations of the arrows on the boundary.
    /// </summary>
    /// <remarks>During generation of a potential, the orientation of an arrow on the boundary is
    /// essentially the sign of its only cycle in the potential.</remarks>
    public enum BoundaryArrowOrientation
    {
        Left = -1,
        Right = +1
    }

    public static class BoundaryArrowOrientationExtensions
    {
        internal static BoundaryArrowOrientation Reverse(this BoundaryArrowOrientation @this)
        {
            return (BoundaryArrowOrientation)(-(int)@this);
        }
    }
}
