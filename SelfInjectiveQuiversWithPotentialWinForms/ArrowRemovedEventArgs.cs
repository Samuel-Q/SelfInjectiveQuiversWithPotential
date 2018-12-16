using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// Provides data for the <see cref="QuiverEditorModel.ArrowRemoved"/> event args.
    /// </summary>
    public class ArrowRemovedEventArgs : EventArgs
    {
        public Arrow<int> Arrow { get; }

        public ArrowRemovedEventArgs(Arrow<int> arrow)
        {
            Arrow = arrow ?? throw new ArgumentNullException(nameof(arrow));
        }
    }
}
