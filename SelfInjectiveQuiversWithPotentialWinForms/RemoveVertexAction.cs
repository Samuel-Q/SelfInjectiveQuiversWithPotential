using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential;
using SelfInjectiveQuiversWithPotential.Plane;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    public class RemoveVertexAction : IUndoableRedoableEditorAction
    {
        private readonly QuiverEditorModel model;
        private readonly int vertexToRemove;
        private readonly Point vertexPosition;
        private readonly IEnumerable<Arrow<int>> arrowsToRemove;

        public RemoveVertexAction(QuiverEditorModel model, int vertex, IEnumerable<Arrow<int>> arrowsToRemove)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
            this.vertexToRemove = vertex;
            this.vertexPosition = model.quiverInPlane.GetVertexPosition(vertex);
            this.arrowsToRemove = arrowsToRemove ?? throw new ArgumentNullException(nameof(arrowsToRemove));
        }

        public void Do()
        {
            foreach (var arrow in arrowsToRemove) model.JustRemoveArrow(arrow);
            model.JustRemoveVertex(vertexToRemove);
        }

        public void Undo()
        {
            model.JustAddVertex(vertexToRemove, vertexPosition);
            foreach (var arrow in arrowsToRemove) model.JustAddArrow(arrow);
        }

        public void Redo()
        {
            Do();
        }

        public override string ToString()
        {
            return $"Remove vertex {vertexToRemove} {(arrowsToRemove.Count() > 0 ? " (and arrows)" : "" )}";
        }
    }
}
