using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// Provides data for the <see cref="QuiverEditorView.ImportQuiverFromMutationAppFileDialogAccepted"/> event.
    /// </summary>
    public class ImportQuiverFromMutationAppFileDialogAcceptedEventArgs
    {
        /// <summary>
        /// Gets the path of the file from which to import the quiver.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportQuiverFromMutationAppFileDialogAcceptedEventArgs"/> class.
        /// </summary>
        /// <param name="path">The path of the file from which to import the quiver.</param>
        public ImportQuiverFromMutationAppFileDialogAcceptedEventArgs(string path)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
        }
    }
}
