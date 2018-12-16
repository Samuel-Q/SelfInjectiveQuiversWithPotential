using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// Provides data for the <see cref="QuiverEditorView.PredefinedQuiverDialogAccepted"/> event.
    /// </summary>
    public class PredefinedQuiverDialogAcceptedEventArgs : EventArgs
    {
        public PredefinedQuiver PredefinedQuiver { get; }

        public dynamic QuiverParameter { get; }

        public PredefinedQuiverDialogAcceptedEventArgs(PredefinedQuiver predefinedQuiver, dynamic quiverParameter)
        {
            PredefinedQuiver = predefinedQuiver;
            QuiverParameter = quiverParameter;
        }
    }
}
