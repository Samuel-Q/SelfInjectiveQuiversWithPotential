using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// Provides data for the <see cref="QuiverEditorModel.PathSelected"/> event.
    /// </summary>
    public class PathSelectedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the path that was selected.
        /// </summary>
        public Path<int> Path { get; }

        public PathSelectedEventArgs(Path<int> path)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
        }
    }
}
