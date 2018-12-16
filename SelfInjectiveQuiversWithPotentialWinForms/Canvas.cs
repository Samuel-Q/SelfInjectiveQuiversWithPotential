using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// This class represents a canvas suitable for drawing on.
    /// </summary>
    /// <remarks>
    /// <para>It seems reasonable to have the canvas be a control with the
    /// <see cref="Control.DoubleBuffered"/> property set to <see langword="true"/>. However, this
    /// seems to unwantedly clear the buffer in certain situations. To reproduce this problem
    /// (using the program in the current state as of this writing), start the program, add a
    /// vertex, and press the Alt key.</para>
    public class Canvas : Panel
    {
        public Canvas()
        {
            //DoubleBuffered = true; // This introduces the issue with buffer being cleared
        }

        // Make it possible to, e.g., select tools after clicking on the canvas (after having done an analysis or so)
        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.Focus();
            base.OnMouseDown(e);
        }
    }
}
