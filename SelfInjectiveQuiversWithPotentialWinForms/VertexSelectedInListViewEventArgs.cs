using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// Provides data for the <see cref="QuiverEditorView.VertexSelectedInListView"/> event.
    /// </summary>
    public class VertexSelectedInListViewEventArgs : EventArgs
    {
        public int? Vertex { get; }

        public VertexSelectedInListViewEventArgs(int? vertex)
        {
            Vertex = vertex;
        }
    }
}
