using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential.Plane;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    public class AddVertexAction : IUndoableRedoableEditorAction
    {
        private readonly QuiverEditorModel model;
        private readonly int vertex;
        private readonly Point vertexPosition;

        public AddVertexAction(QuiverEditorModel model, int vertex, Point vertexPosition)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
            this.vertex = vertex;
            this.vertexPosition = vertexPosition;
        }

        public void Do()
        {
            model.JustAddVertex(vertex, vertexPosition);
        }

        public void Undo()
        {
            model.JustRemoveVertex(vertex);
        }

        public void Redo()
        {
            Do();
        }

        public override string ToString()
        {
            return $"Add vertex {vertex}";
        }
    }
}
