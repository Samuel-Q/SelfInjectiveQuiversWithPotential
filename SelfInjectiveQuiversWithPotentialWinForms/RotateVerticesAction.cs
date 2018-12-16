using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    using SelfInjectiveQuiversWithPotential.Plane;

    public class RotateVerticesAction : IUndoableRedoableEditorAction
    {
        private readonly QuiverEditorModel model;
        private readonly IEnumerable<int> vertices;
        private readonly Point center;
        private readonly double radians;

        public RotateVerticesAction(QuiverEditorModel model, IEnumerable<int> vertices, Point center, double radians)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
            this.vertices = vertices.ToList();
            this.center = center;
            this.radians = radians;
        }

        public void Do()
        {
            model.JustRotateVertices(vertices, center, radians);
        }

        public void Undo()
        {
            // Technically, this might not be a true undo, because of floating-point inaccuracies.
            model.JustRotateVertices(vertices, center, -radians);
        }

        public void Redo()
        {
            Do();
        }

        public override string ToString()
        {
            return $"Rotate vertices";
        }
    }
}
