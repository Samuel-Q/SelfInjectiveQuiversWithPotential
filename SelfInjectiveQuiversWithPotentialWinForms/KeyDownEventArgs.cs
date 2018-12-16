using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    public class KeyDownEventArgs : EventArgs
    {
        public Keys KeyCode { get; }

        public KeyDownEventArgs(KeyEventArgs e)
        {
            if (e is null) throw new ArgumentNullException(nameof(e));
            KeyCode = e.KeyCode;
        }
    }
}
