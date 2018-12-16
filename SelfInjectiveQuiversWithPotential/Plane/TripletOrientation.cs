using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Plane
{
    /// <summary>
    /// Defines the three different kinds of orientations of triplets of points in the plane.
    /// </summary>
    /// <remarks>See <see href="https://www.geeksforgeeks.org/orientation-3-ordered-points/"/> for details.</remarks>
    public enum TripletOrientation
    {
        Counterclockwise,
        Clockwise,
        Collinear
    }
}
