using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// Provides data for the <see cref="QuiverEditorModel.UndoableActionsChanged"/> event.
    /// </summary>
    public class UndoableActionsChangedEventArgs
    {
        /// <summary>
        /// Gets the most recent undoable action, or <see langword="null"/> if there are no undoable
        /// actions.
        /// </summary>
        public IUndoableRedoableEditorAction Action { get; }

        public UndoableActionsChangedEventArgs(IUndoableRedoableEditorAction action)
        {
            Action = action;
        }
    }
}
