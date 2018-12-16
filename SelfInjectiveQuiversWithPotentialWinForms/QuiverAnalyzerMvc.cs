using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SelfInjectiveQuiversWithPotential.Plane;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// This class gathers all the components (model, view, and controller) for the quiver analyzer.
    /// </summary>
    public class QuiverAnalyzerMvc
    {
        public QuiverAnalyzerModel Model { get; }
        public QuiverAnalyzerView View { get; }
        public QuiverAnalyzerController Controller { get; }

        public QuiverAnalyzerMvc(
            QuiverEditorModel editorModel,
            Button analyzeButton,
            GroupBox analysisResultsGroupBox,
            Label analysisMainResultLabel,
            ListView maximalPathRepresentativesListView,
            ListView nakayamaPermutationListView,
            TextBox orbitTextBox,
            TextBox longestPathEncounteredTextBox,
            Label longestPathEncounteredLengthLabel)
        {
            Model = new QuiverAnalyzerModel(editorModel);
            View = new QuiverAnalyzerView(
                Model,
                analyzeButton,
                analysisResultsGroupBox,
                analysisMainResultLabel,
                maximalPathRepresentativesListView,
                nakayamaPermutationListView,
                orbitTextBox,
                longestPathEncounteredTextBox,
                longestPathEncounteredLengthLabel);
            Controller = new QuiverAnalyzerController(Model, View);
        }
    }
}
