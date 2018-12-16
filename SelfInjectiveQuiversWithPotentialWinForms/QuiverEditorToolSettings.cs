using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// This class encapsulates the settings for all the quiver editor tools.
    /// </summary>
    public class QuiverEditorToolSettings
    {
        private int vertexToAdd;

        public int VertexToAdd
        {
            get => vertexToAdd;
            set
            {
                if (value == vertexToAdd) return;
                vertexToAdd = value;
                VertexToAddChanged?.Invoke(this, new VertexToAddChangedEventArgs(value));
            }
        }

        public event EventHandler<VertexToAddChangedEventArgs> VertexToAddChanged;
    }
}
