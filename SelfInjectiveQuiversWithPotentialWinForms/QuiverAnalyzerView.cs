using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SelfInjectiveQuiversWithPotential;
using SelfInjectiveQuiversWithPotential.Analysis;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// This class represents the view component of the quiver analyzer.
    /// </summary>
    public class QuiverAnalyzerView
    {
        private readonly QuiverAnalyzerModel model;

        private readonly GroupBox analysisResultsGroupBox;
        private readonly Label analysisMainResultLabel;
        private readonly ListView maximalPathRepresentativesListView;
        private readonly ListView nakayamaPermutationListView;
        private readonly TextBox orbitTextBox;
        private readonly TextBox longestPathEncounteredTextBox;
        private readonly Label longestPathEncounteredLengthLabel;

        public event EventHandler<EventArgs> AnalyzeButtonClicked;

        public QuiverAnalyzerView(
            QuiverAnalyzerModel model,
            Button analyzeButton,
            GroupBox analysisResultsGroupBox,
            Label analysisMainResultLabel,
            ListView maximalPathRepresentativesListView,
            ListView nakayamaPermutationListView,
            TextBox orbitTextBox,
            TextBox longestPathEncounteredTextBox,
            Label longestPathEncounteredLengthLabel)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
            if (analyzeButton is null) throw new ArgumentNullException(nameof(analyzeButton));
            this.analysisResultsGroupBox = analysisResultsGroupBox ?? throw new ArgumentNullException(nameof(analysisResultsGroupBox));
            this.analysisMainResultLabel = analysisMainResultLabel ?? throw new ArgumentNullException(nameof(analysisMainResultLabel));
            this.maximalPathRepresentativesListView = maximalPathRepresentativesListView ?? throw new ArgumentNullException(nameof(maximalPathRepresentativesListView));
            this.nakayamaPermutationListView = nakayamaPermutationListView ?? throw new ArgumentNullException(nameof(nakayamaPermutationListView));
            this.orbitTextBox = orbitTextBox ?? throw new ArgumentNullException(nameof(orbitTextBox));
            this.longestPathEncounteredTextBox = longestPathEncounteredTextBox ?? throw new ArgumentNullException(nameof(longestPathEncounteredTextBox));
            this.longestPathEncounteredLengthLabel = longestPathEncounteredLengthLabel ?? throw new ArgumentNullException(nameof(longestPathEncounteredLengthLabel));

            model.ModelCleared += Model_ModelCleared;
            model.AnalysisDone += Model_AnalysisDone;
            model.MaximalPathRepresentativesChanged += Model_MaximalPathRepresentativesChanged;
            model.OrbitChanged += Model_OrbitChanged;
            model.LongestPathEncounteredChanged += Model_LongestPathEncounteredChanged;

            analyzeButton.Click += AnalyzeButton_Click;
        }

        private void SetMainResultText(string text)
        {
            analysisMainResultLabel.Text = $"{text}";
        }

        private void SetLongestPathEncounteredLengthText(string text)
        {
            longestPathEncounteredLengthLabel.Text = $"Longest path length: {text}";
        }

        private void Model_ModelCleared(object sender, EventArgs e)
        {
            analysisResultsGroupBox.Enabled = false;
            SetMainResultText("None");
            maximalPathRepresentativesListView.Items.Clear();
            maximalPathRepresentativesListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            nakayamaPermutationListView.Items.Clear();
            orbitTextBox.Clear();
            longestPathEncounteredTextBox.Clear();
            SetLongestPathEncounteredLengthText(String.Empty);
        }

        private void Model_AnalysisDone(object sender, AnalysisDoneEventArgs<int> e)
        {
            UpdateViewAccordingToAnalysisResults(e.AnalysisResults);
        }

        private void Model_MaximalPathRepresentativesChanged(object sender, MaximalPathRepresentativesChangedEventArgs e)
        {
            UpdateMaximalPathRepresentativesDisplayed(e.MaximalPathRepresentatives);
        }

        private void Model_OrbitChanged(object sender, OrbitChangedEventArgs e)
        {
            UpdateOrbitDisplayed(e.Orbit);
        }

        private void Model_LongestPathEncounteredChanged(object sender, LongestPathEncounteredChangedEventArgs e)
        {
            UpdateLongestPathEncounteredDataDisplayed(e.LongestPathEncountered);
        }

        private void UpdateMainResultText(IQuiverInPlaneAnalysisResults<int> analysisResults)
        {
            var mainResult = analysisResults.MainResults;
            var mainResultText =
                mainResult.HasFlag(QuiverInPlaneAnalysisMainResults.QuiverHasLoops) ? "Quiver has loops" :
                mainResult.HasFlag(QuiverInPlaneAnalysisMainResults.QuiverHasAntiParallelArrows) ? "Quiver has anti-parallel arrows" :
                mainResult.HasFlag(QuiverInPlaneAnalysisMainResults.QuiverHasFaceWithInconsistentOrientation) ? "Quiver has face with inconsistent orientation" :
                mainResult.HasFlag(QuiverInPlaneAnalysisMainResults.QuiverIsNotPlane) ? "Quiver is not plane" :
                mainResult.HasFlag(QuiverInPlaneAnalysisMainResults.QPAnalysisAborted) ? "QP analysis was aborted" :
                mainResult.HasFlag(QuiverInPlaneAnalysisMainResults.QPAnalysisCancelled) ? "QP analysis was cancelled" :
                mainResult.HasFlag(QuiverInPlaneAnalysisMainResults.QPIsNotWeaklyCancellative) ? "Quiver induces a planar QP that is not weakly cancellative" :
                mainResult.HasFlag(QuiverInPlaneAnalysisMainResults.QPIsNotCancellative) ?
                    mainResult.IndicatesSelfInjectivity() ?
                    "Quiver induces a planar QP that is weakly (but not strongly) cancellative and self-injective" :
                    "Quiver induces a planar QP that is weakly (but not strongly) cancellative" :
                mainResult.IndicatesSelfInjectivity() ? "Quiver induces a (strongly) cancellative planar QP that is self-injective" :
                "Quiver induces a (strongly) cancellative planar QP that is not self-injective.";

            SetMainResultText(mainResultText);
        }

        private ListViewItem CreateListViewItemForNakayamaMapping(int sourceVertex, int targetVertex)
        {
            return new ListViewItem($"{sourceVertex} -> {targetVertex}");
        }

        private void UpdateNakayamaPermutationListView(IQuiverInPlaneAnalysisResults<int> analysisResults)
        {
            nakayamaPermutationListView.Items.Clear();
            if (analysisResults.MainResults.IndicatesSelfInjectivity())
            {
                var listViewItems = analysisResults.NakayamaPermutation.OrderBy(p => p.Key).Select(p => CreateListViewItemForNakayamaMapping(p.Key, p.Value));
                nakayamaPermutationListView.Items.AddRange(listViewItems.ToArray());
            }
        }

        private void UpdateViewAccordingToAnalysisResults(IQuiverInPlaneAnalysisResults<int> analysisResults)
        {
            analysisResultsGroupBox.Enabled = true;
            UpdateMainResultText(analysisResults);
            UpdateNakayamaPermutationListView(analysisResults);
            UpdateLongestPathEncounteredDataDisplayed(analysisResults.LongestPathEncountered);
        }

        private ListViewItem CreateListViewItemForMaximalPathRepresentative(Path<int> path)
        {
            return new ListViewItem(path.ToString());
        }

        private void UpdateMaximalPathRepresentativesDisplayed(IEnumerable<Path<int>> maximalPathRepresentatives)
        {
            if (maximalPathRepresentatives is null)
            {
                maximalPathRepresentativesListView.Items.Clear();
                return;
            }

            var listViewItems = maximalPathRepresentatives.Select(p => CreateListViewItemForMaximalPathRepresentative(p));
            maximalPathRepresentativesListView.Items.Clear();
            maximalPathRepresentativesListView.Items.AddRange(listViewItems.ToArray());

            // Specifying the column width to be -2 doesn't solve the problem of really long path representatives,
            // because auto-resized column width is seemingly capped by the list view width.
            // The following line does the trick though
            maximalPathRepresentativesListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }

        private void UpdateOrbitDisplayed(IEnumerable<int> orbit)
        {
            if (orbit is null)
            {
                orbitTextBox.Text = String.Empty;
                return;
            }

            var orbitString = String.Join(", ", orbit);
            orbitTextBox.Text = orbitString;
        }

        private void UpdateLongestPathEncounteredDataDisplayed(Path<int> longestPathEncountered)
        {
            // This occurs if the quiver-in-plane analysis was unsuccessful.
            if (longestPathEncountered is null)
            {
                longestPathEncounteredTextBox.Text = String.Empty;
                SetLongestPathEncounteredLengthText(String.Empty);
                return;
            }

            longestPathEncounteredTextBox.Text = longestPathEncountered?.ToString();
            SetLongestPathEncounteredLengthText(longestPathEncountered.Length.ToString());
        }

        private void AnalyzeButton_Click(object sender, EventArgs e)
        {
            AnalyzeButtonClicked?.Invoke(this, e);
        }
    }
}
