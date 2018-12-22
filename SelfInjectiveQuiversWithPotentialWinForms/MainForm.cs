using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    public partial class MainForm : Form
    {
        private QuiverEditorMvc editorMvc;
        private QuiverAnalyzerMvc analyzerMvc;

        public MainForm()
        {
            InitializeComponent();

            var toolButtons = new Dictionary<QuiverEditorTool, Button>
            {
                { QuiverEditorTool.SelectMove, btnSelectMove },
                { QuiverEditorTool.AddVertex, btnAddVertex },
                { QuiverEditorTool.AddArrow, btnAddArrow },
                { QuiverEditorTool.SelectPath, btnSelectPath }
            };

            var toolSettingsControlsDictionary = new Dictionary<QuiverEditorTool, IEnumerable<Control>>
            {
                { QuiverEditorTool.SelectMove, new Control[] { } },
                { QuiverEditorTool.AddVertex, new Control[] { lblVertexToAdd, nudVertexToAdd } },
                { QuiverEditorTool.AddArrow, new Control[] { } },
                { QuiverEditorTool.SelectPath, new Control[] { } },
            };

            var predefinedQuiverMenuItems = new Dictionary<PredefinedQuiver, ToolStripMenuItem>()
            {
                { PredefinedQuiver.Cycle, cycleToolStripMenuItem },
                { PredefinedQuiver.Triangle, triangleToolStripMenuItem },
                { PredefinedQuiver.Square, squareToolStripMenuItem },
                { PredefinedQuiver.Cobweb, cobwebToolStripMenuItem },
                { PredefinedQuiver.OddFlower, oddFlowerToolStripMenuItem },
                { PredefinedQuiver.EvenFlower, evenFlowerToolStripMenuItem },
                { PredefinedQuiver.PointedFlower, pointedFlowerToolStripMenuItem },
                { PredefinedQuiver.GeneralizedCobweb, generalizedCobwebToolStripMenuItem}
            };

            editorMvc = new QuiverEditorMvc(
                this,
                canvas,
                lblCenterOfCanvas,
                lblMousePointerOnCanvasLocation,
                lstVertices,
                lblVertexCount,
                lstArrows,
                lblArrowCount,
                toolButtons,
                toolSettingsControlsDictionary,
                nudVertexToAdd,
                undoToolStripMenuItem,
                redoToolStripMenuItem,
                relabelVerticesToolStripMenuItem,
                rotateVerticesToolStripMenuItem,
                predefinedQuiverMenuItems,
                importFromMutationAppFileToolStripMenuItem,
                importFromMutationAppFileOpenFileDialog,
                exportAsMutationAppFileToolStripMenuItem,
                exportAsMutationAppFileSaveFileDialog);

            analyzerMvc = new QuiverAnalyzerMvc(
                editorMvc.Model,
                btnAnalyze,
                grpAnalysisResults,
                lblAnalysisMainResult,
                lstMaximalPathRepresentatives,
                lstNakayamaPermutation,
                txtOrbit,
                txtLongestPathEncountered,
                lblLongestPathLength);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
