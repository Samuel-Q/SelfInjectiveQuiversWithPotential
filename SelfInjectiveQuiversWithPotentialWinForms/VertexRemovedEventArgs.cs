using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// Provides data for the <see cref="QuiverEditorModel.VertexRemoved"/> event.
    /// </summary>
    public class VertexRemovedEventArgs : EventArgs
    {
        public int Vertex { get; }

        public VertexRemovedEventArgs(int vertex)
        {
            Vertex = vertex;
        }
    }
}
