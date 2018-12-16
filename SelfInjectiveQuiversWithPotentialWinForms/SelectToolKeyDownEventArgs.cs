using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// Provides data for the <see cref="QuiverEditorView.SelectToolKeyDown"/> event.
    /// </summary>
    public class SelectToolKeyDownEventArgs
    {
        public QuiverEditorTool Tool { get; }

        public SelectToolKeyDownEventArgs(QuiverEditorTool tool)
        {
            Tool = tool;
        }
    }
}
