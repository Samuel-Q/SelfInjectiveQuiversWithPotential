using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// Provides data for the <see cref="QuiverEditorModel.ToolSelected"/> event.
    /// </summary>
    public class ToolSelectedEventArgs : EventArgs
    {
        public QuiverEditorTool PreviousTool { get; }

        public QuiverEditorTool NewTool { get; }

        public ToolSelectedEventArgs(QuiverEditorTool previousTool, QuiverEditorTool newTool)
        {
            PreviousTool = previousTool;
            NewTool = newTool;
        }
    }
}
