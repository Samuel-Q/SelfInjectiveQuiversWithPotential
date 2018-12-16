using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// Provides data for the <see cref="QuiverEditorView.ExportQuiverAsMutationAppFileDialogAccepted"/> event.
    /// </summary>
    public class ExportQuiverAsMutationAppFileDialogAcceptedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the path of the file to which to export the quiver.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportQuiverAsMutationAppFileDialogAcceptedEventArgs"/> class.
        /// </summary>
        /// <param name="path">The path of the file to which to export the quiver.</param>
        public ExportQuiverAsMutationAppFileDialogAcceptedEventArgs(string path)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
        }
    }
}
