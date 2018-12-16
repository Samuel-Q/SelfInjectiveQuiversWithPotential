using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = SelfInjectiveQuiversWithPotential.Plane.Point;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// Provides data for the <see cref="QuiverEditorView.CanvasClicked"/> event.
    /// </summary>
    public class CanvasClickedEventArgs : EventArgs
    {
        public Point Location { get; }

        public CanvasClickedEventArgs(Point location)
        {
            Location = location;
        }
    }
}
