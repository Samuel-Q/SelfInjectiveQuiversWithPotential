using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    public class AddArrowAction : IUndoableRedoableEditorAction
    {
        private readonly QuiverEditorModel model;
        private readonly Arrow<int> arrow;

        public AddArrowAction(QuiverEditorModel model, Arrow<int> arrow)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
            this.arrow = arrow ?? throw new ArgumentNullException(nameof(arrow));
        }

        public void Do()
        {
            model.JustAddArrow(arrow);
        }

        public void Undo()
        {
            model.JustRemoveArrow(arrow);
        }

        public void Redo()
        {
            Do();
        }

        public override string ToString()
        {
            return $"Add arrow {arrow}";
        }
    }
}
