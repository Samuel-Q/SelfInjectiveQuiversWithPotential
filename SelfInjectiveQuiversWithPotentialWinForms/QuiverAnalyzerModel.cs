using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential;
using SelfInjectiveQuiversWithPotential.Analysis;
using SelfInjectiveQuiversWithPotential.Plane;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// This class represents the model component of the quiver analyzer.
    /// </summary>
    public class QuiverAnalyzerModel
    {
        private readonly QuiverEditorModel editorModel;

        private readonly QuiverInPlaneAnalysisSettings analysisSettings;
        private IQuiverInPlaneAnalysisResults<int> analysisResults;

        public bool HasAnalysisResults { get => analysisResults != null; }

        public event EventHandler ModelCleared;
        public event EventHandler<AnalysisDoneEventArgs<int>> AnalysisDone;
        public event EventHandler<EquivalentPathsChangedEventArgs> EquivalentPathsChanged;
        public event EventHandler<MaximalPathRepresentativesChangedEventArgs> MaximalPathRepresentativesChanged;
        public event EventHandler<OrbitChangedEventArgs> OrbitChanged;
        public event EventHandler<LongestPathEncounteredChangedEventArgs> LongestPathEncounteredChanged;

        public QuiverAnalyzerModel(QuiverEditorModel editorModel)
        {
            this.editorModel = editorModel ?? throw new ArgumentNullException(nameof(editorModel));

            editorModel.VertexAdded += EditorModel_VertexAdded;
            editorModel.VertexRemoved += EditorModel_VertexRemoved;
            editorModel.ArrowAdded += EditorModel_ArrowAdded;
            editorModel.ArrowRemoved += EditorModel_ArrowRemoved;

            editorModel.VertexSelected += EditorModel_VertexSelected;
            editorModel.VertexDeselected += EditorModel_VertexDeselected;

            editorModel.QuiverLoaded += EditorModel_QuiverLoaded;

            analysisSettings = new QuiverInPlaneAnalysisSettings(CancellativityTypes.Cancellativity | CancellativityTypes.WeakCancellativity);
        }

        private void ClearAnalyzerModel()
        {
            analysisResults = null;
            ModelCleared?.Invoke(this, EventArgs.Empty);
        }

        private void EditorModel_VertexAdded(object sender, VertexAddedEventArgs e)
        {
            ClearAnalyzerModel();
        }

        private void EditorModel_VertexRemoved(object sender, VertexRemovedEventArgs e)
        {
            ClearAnalyzerModel();
        }

        private void EditorModel_ArrowAdded(object sender, ArrowAddedEventArgs e)
        {
            ClearAnalyzerModel();
        }

        private void EditorModel_ArrowRemoved(object sender, ArrowRemovedEventArgs e)
        {
            ClearAnalyzerModel();
        }

        private void EditorModel_VertexSelected(object sender, VertexSelectedEventArgs e)
        {
            UpdateMaximalPathRepresentatives(e.Vertex);
            UpdateOrbit(e.Vertex);
        }

        private void EditorModel_VertexDeselected(object sender, VertexDeselectedEventArgs e)
        {
            ClearMaximalPathRepresentatives();
            ClearOrbit();
        }

        private void EditorModel_QuiverLoaded(object sender, QuiverLoadedEventArgs e)
        {
            ClearAnalyzerModel();
        }

        // "Update" is sort of a misnomer; there is no field or property to update (as of this writing)
        private void UpdateMaximalPathRepresentatives(int vertex)
        {
            if (analysisResults?.MainResults.HasFlag(QuiverInPlaneAnalysisMainResult.Success) ?? false)
            {
                var maximalPathRepresentatives = analysisResults.MaximalPathRepresentatives[vertex];
                MaximalPathRepresentativesChanged?.Invoke(this, new MaximalPathRepresentativesChangedEventArgs(maximalPathRepresentatives));
            }
        }

        // "Clear" is sort of a misnomes; there is no field or property to clear (as of this writing)
        private void ClearMaximalPathRepresentatives()
        {
            MaximalPathRepresentativesChanged?.Invoke(this, new MaximalPathRepresentativesChangedEventArgs(null));
        }

        // "Update" is sort of a misnomer; there is no field or property to update (as of this writing)
        private void UpdateOrbit(int vertex)
        {
            if ((analysisResults?.MainResults.IndicatesSelfInjectivity()) ?? false)
            {
                var orbit = analysisResults.NakayamaPermutation.GetOrbit(vertex);
                OrbitChanged?.Invoke(this, new OrbitChangedEventArgs(orbit));
            }
        }

        private void ClearOrbit()
        {
            OrbitChanged?.Invoke(this, new OrbitChangedEventArgs(null));
        }

        public void Analyze()
        {
            var analyzer = new QuiverInPlaneAnalyzer();
            analysisResults = analyzer.Analyze(editorModel.quiverInPlane, analysisSettings);
            AnalysisDone?.Invoke(this, new AnalysisDoneEventArgs<int>(analysisResults));
            if (editorModel.HasSelectedVertex)
            {
                var vertex = editorModel.SelectedVertex.Value;
                UpdateMaximalPathRepresentatives(vertex);
                UpdateOrbit(vertex);
            }
        }
    }
}
