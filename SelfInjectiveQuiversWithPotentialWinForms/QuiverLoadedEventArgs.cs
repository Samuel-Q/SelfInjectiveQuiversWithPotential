using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential.Plane;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// Provides data for the <see cref="QuiverEditorModel.QuiverLoaded"/> event.
    /// </summary>
    public class QuiverLoadedEventArgs
    {
        /// <summary>
        /// Gets the quiver in plane that was loaded.
        /// </summary>
        public QuiverInPlane<int> QuiverInPlane { get; }

        public QuiverLoadedEventArgs(QuiverInPlane<int> quiverInPlane)
        {
            QuiverInPlane = quiverInPlane ?? throw new ArgumentNullException(nameof(quiverInPlane));
        }
    }
}
