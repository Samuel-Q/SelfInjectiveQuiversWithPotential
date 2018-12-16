using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SelfInjectiveQuiversWithPotential.Plane;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// This class gathers all the components (model, view, and controller) for the quiver editor.
    /// </summary>
    public class QuiverEditorMvc
    {
        public QuiverEditorModel Model { get; }
        public QuiverEditorView View { get; }
        public QuiverEditorController Controller { get; }

        public QuiverEditorMvc(
            Form parent,
            Canvas canvas,
            Label centerOfCanvasLabel,
            Label mousePointerOnCanvasLocationLabel,
            ListView vertexListView,
            Label vertexCountLabel,
            ListView arrowListView,
            Label arrowCountLabel,
            IReadOnlyDictionary<QuiverEditorTool, Button> toolButtons,
            IReadOnlyDictionary<QuiverEditorTool, IEnumerable<Control>> toolSettingsControlsDictionary,
            NumericUpDown vertexToAddNud,
            ToolStripMenuItem undoMenuItem,
            ToolStripMenuItem redoMenuItem,
            ToolStripMenuItem relabelVerticesMenuItem,
            ToolStripMenuItem rotateVerticesMenuItem,
            IReadOnlyDictionary<PredefinedQuiver, ToolStripMenuItem> predefinedQuiverMenuItems,
            ToolStripMenuItem importFromMutationAppFileToolStripMenuItem,
            OpenFileDialog importFromMutationAppFileOpenFileDialog,
            ToolStripMenuItem exportAsMutationAppFileMenuItem,
            SaveFileDialog exportAsMutationAppFileSaveFileDialog)
        {
            Model = new QuiverEditorModel(new QuiverInPlane<int>());
            View = new QuiverEditorView(
                Model,
                parent,
                canvas,
                centerOfCanvasLabel,
                mousePointerOnCanvasLocationLabel,
                vertexListView,
                vertexCountLabel,
                arrowListView,
                arrowCountLabel,
                toolButtons,
                toolSettingsControlsDictionary,
                vertexToAddNud,
                undoMenuItem,
                redoMenuItem,
                relabelVerticesMenuItem,
                rotateVerticesMenuItem,
                predefinedQuiverMenuItems,
                importFromMutationAppFileToolStripMenuItem,
                importFromMutationAppFileOpenFileDialog,
                exportAsMutationAppFileMenuItem,
                exportAsMutationAppFileSaveFileDialog);
            Controller = new QuiverEditorController(Model, View);
        }
    }
}
