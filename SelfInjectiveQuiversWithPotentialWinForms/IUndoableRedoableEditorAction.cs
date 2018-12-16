using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// Classes implementing this interface represent an action in the editor that can be
    /// undone (Ctrl+Z) and redone (Ctrl+Y).
    /// </summary>
    public interface IUndoableRedoableEditorAction
    {
        void Do();

        void Undo();

        void Redo();
    }
}
