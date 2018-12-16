using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// Provides data for the <see cref="QuiverEditorModel.VertexDeselected"/> event.
    /// </summary>
    public class VertexDeselectedEventArgs : EventArgs
    {
        public int Vertex { get; }

        public VertexDeselectedEventArgs(int vertex)
        {
            Vertex = vertex;
        }
    }
}
