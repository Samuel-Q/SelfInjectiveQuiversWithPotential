using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// Provides data for the <see cref="QuiverEditorToolSettings.VertexToAddChanged"/>
    /// and the <see cref="QuiverEditorView.VertexToAddChanged"/> events.
    /// </summary>
    public class VertexToAddChangedEventArgs : EventArgs
    {
        public int VertexToAdd { get; }

        public VertexToAddChangedEventArgs(int vertexToAdd)
        {
            VertexToAdd = vertexToAdd;
        }
    }
}
