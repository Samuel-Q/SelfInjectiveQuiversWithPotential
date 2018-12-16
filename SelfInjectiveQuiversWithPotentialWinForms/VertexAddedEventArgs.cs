using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential.Plane;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// Provides data for the <see cref="QuiverEditorModel.VertexAdded"/> event.
    /// </summary>
    public class VertexAddedEventArgs : EventArgs
    {
        public int Vertex { get; }

        public Point VertexPosition { get; }

        public VertexAddedEventArgs(int vertex, Point vertexPosition)
        {
            Vertex = vertex;
            VertexPosition = vertexPosition;
        }
    }
}
