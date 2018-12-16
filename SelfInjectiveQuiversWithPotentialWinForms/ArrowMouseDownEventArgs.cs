using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// Provides data for the <see cref="QuiverEditorView.ArrowMouseDown"/> event.
    /// </summary>
    public class ArrowMouseDownEventArgs : EventArgs
    {
        public Arrow<int> Arrow { get; private set; }

        public ArrowMouseDownEventArgs(Arrow<int> arrow)
        {
            Arrow = arrow ?? throw new ArgumentNullException(nameof(arrow));
        }
    }
}
