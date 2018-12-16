using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// Provides data for the <see cref="QuiverEditorModel.PathDeselected"/> event.
    /// </summary>
    public class PathDeselectedEventArgs
    {
        /// <summary>
        /// Gets the path that was deselected.
        /// </summary>
        public Path<int> Path { get; }

        public PathDeselectedEventArgs(Path<int> path)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
        }
    }
}
