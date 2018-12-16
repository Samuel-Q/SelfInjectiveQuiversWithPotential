using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <remarks>
    /// <para>The code was obtained at <see href="https://stackoverflow.com/questions/2743174/how-do-i-make-a-picturebox-selectable"/></para>
    /// </remarks>
    public class SelectablePictureBox : PictureBox
    {
        public SelectablePictureBox()
        {
            SetStyle(ControlStyles.Selectable, true);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            this.Focus();
            base.OnMouseDown(e);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (this.Focused)
            {
                ClientRectangle.Inflate(-2, -2);
                ControlPaint.DrawFocusRectangle(pe.Graphics, ClientRectangle);
            }
        }
    }
}
