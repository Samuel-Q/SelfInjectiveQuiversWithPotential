using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    using SelfInjectiveQuiversWithPotential.Plane;

    /// <summary>
    /// Provides data for the <see cref="QuiverEditorModel.VertexMoved"/> event.
    /// </summary>
    public class VertexMovedEventArgs : EventArgs
    {
        public int Vertex { get; }

        public Point Point { get; }

        public VertexMovedEventArgs(int vertex, Point point)
        {
            Vertex = vertex;
            Point = point;
        }
    }
}
