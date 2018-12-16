using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// Provides data for the <see cref="QuiverEditorModel.VertexSelected"/> event.
    /// </summary>
    public class VertexSelectedEventArgs : EventArgs
    {
        public int Vertex { get; }

        public VertexSelectedEventArgs(int vertex)
        {
            Vertex = vertex;
        }
    }
}
