using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotential.Plane
{
    /// <summary>
    /// Classes implementing this interface represent a vertex of a graph or quiver embedded
    /// (in some informal sense) in the plane.
    /// </summary>
    public interface IVertexInPlane
    {
        /// <summary>
        /// Gets the position in the plane of the vertex.
        /// </summary>
        Point Position { get; }
    }
}
