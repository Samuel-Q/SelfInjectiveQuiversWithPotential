using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// Provides data for the <see cref="QuiverEditorModel.RedoableActionsChanged"/> event.
    /// </summary>
    public class RedoableActionsChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the most recent redoable action, or <see langword="null"/> if there are no redoable
        /// actions.
        /// </summary>
        public IUndoableRedoableEditorAction Action { get; }

        public RedoableActionsChangedEventArgs(IUndoableRedoableEditorAction action)
        {
            Action = action;
        }
    }
}
