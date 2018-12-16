using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// Provides data for the <see cref="QuiverEditorView.ArrowSelectedInListView"/> event.
    /// </summary>
    public class ArrowSelectedInListViewEventArgs : EventArgs
    {
        public Arrow<int> Arrow { get; }

        public ArrowSelectedInListViewEventArgs(Arrow<int> arrow)
        {
            Arrow = arrow;
        }
    }
}
