using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// Provides data for the <see cref="QuiverEditorModel.ArrowDeselected"/> event.
    /// </summary>
    public class ArrowDeselectedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the arrow that was deselected.
        /// </summary>
        public Arrow<int> Arrow { get; }

        public ArrowDeselectedEventArgs(Arrow<int> arrow)
        {
            Arrow = arrow ?? throw new ArgumentNullException(nameof(arrow));
        }
    }
}
