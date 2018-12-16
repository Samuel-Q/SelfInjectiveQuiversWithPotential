using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    public class RelabelVerticesAction : IUndoableRedoableEditorAction
    {
        private readonly QuiverEditorModel model;
        private readonly IReadOnlyDictionary<int, int> relabelingMap;

        public RelabelVerticesAction(QuiverEditorModel model, IReadOnlyDictionary<int, int> relabelingMap)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
            if (relabelingMap is null) throw new ArgumentNullException(nameof(relabelingMap));
            this.relabelingMap = relabelingMap.ToDictionary(pair => pair.Key, pair => pair.Value); // Copy the relabeling map
        }

        public void Do()
        {
            model.JustRelabelVertices(relabelingMap);
        }

        public void Undo()
        {
            model.JustRelabelVertices(relabelingMap.Inverse());
        }

        public void Redo()
        {
            Do();
        }

        public override string ToString()
        {
            return $"Relabel vertices";
        }
    }
}
