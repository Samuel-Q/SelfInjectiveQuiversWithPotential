using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// Provides data for the <see cref="QuiverEditorView.VertexMouseDown"/> event.
    /// </summary>
    public class VertexMouseDownEventArgs : EventArgs
    {
        public int Vertex { get; private set; }

        public VertexMouseDownEventArgs(int vertex)
        {
            Vertex = vertex;
        }
    }
}
