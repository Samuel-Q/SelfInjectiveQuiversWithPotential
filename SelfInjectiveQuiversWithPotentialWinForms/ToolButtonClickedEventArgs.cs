using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// Provides data for the <see cref="QuiverEditorView.ToolButtonClicked"/> event.
    /// </summary>
    public class ToolButtonClickedEventArgs : EventArgs
    {
        public QuiverEditorTool Tool { get; }

        public ToolButtonClickedEventArgs(QuiverEditorTool tool)
        {
            Tool = tool;
        }
    }
}
