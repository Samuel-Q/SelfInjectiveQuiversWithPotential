using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; // Sort of bad to have this in the controller, but it makes the View.KeyDown easier to implement
using SelfInjectiveQuiversWithPotential;
using SelfInjectiveQuiversWithPotential.Data;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// This class represents the controller component of the quiver editor.
    /// </summary>
    public class QuiverEditorController
    {
        private readonly QuiverEditorModel model;
        private readonly QuiverEditorView view;

        public QuiverEditorController(QuiverEditorModel model, QuiverEditorView view)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
            this.view = view ?? throw new ArgumentNullException(nameof(view));

            view.KeyDown += View_KeyDown;
            view.VertexListViewKeyDown += View_VertexListViewKeyDown;
            view.ArrowListViewKeyDown += View_ArrowListViewKeyDown;
            view.SelectToolKeyDown += View_SelectToolKeyDown;
            view.VertexMouseDown += View_VertexMouseDown;
            view.ArrowMouseDown += View_ArrowMouseDown;
            view.CanvasClicked += View_CanvasClicked;
            view.VertexSelectedInListView += View_VertexSelectedInListView;
            view.ArrowSelectedInListView += View_ArrowSelectedInListView;
            view.ToolButtonClicked += View_ToolButtonClicked;
            view.VertexToAddChanged += View_VertexToAddNudValueChanged;
            view.UndoMenuItemClicked += View_UndoMenuItemClicked;
            view.RedoMenuItemClicked += View_RedoMenuItemClicked;
            view.RelabelVerticesMenuItemClicked += View_RelabelVerticesMenuItemClicked;
            view.RotateVerticesMenuItemClicked += View_RotateVerticesMenuItemClicked;
            view.PredefinedQuiverDialogAccepted += View_PredefinedQuiverDialogAccepted;
            view.ImportQuiverFromMutationAppFileDialogAccepted += View_ImportQuiverFromMutationAppFileDialogAccepted;
            view.ExportQuiverAsMutationAppFileDialogAccepted += View_ExportQuiverAsMutationAppFileDialogAccepted;
        }

        private void Delete()
        {
            if (model.HasSelectedVertex) model.RemoveSelectedVertex();
            else if (model.HasSelectedArrow) model.RemoveSelectedArrow();
        }

        private void View_VertexListViewKeyDown(object sender, KeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Delete) Delete();
        }

        private void View_ArrowListViewKeyDown(object sender, KeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Delete) Delete();
        }

        private void View_KeyDown(object sender, KeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Delete) Delete();
        }

        private void View_SelectToolKeyDown(object sender, SelectToolKeyDownEventArgs e)
        {
            SelectTool(e.Tool);
        }

        private void AddArrowOrSelectVertex(int vertex)
        {
            if (model.HasSelectedVertex)
            {
                var sourceVertex = model.SelectedVertex.Value;
                var targetVertex = vertex;

                if (sourceVertex == targetVertex) return;
                if (model.quiverInPlane.ContainsArrow(sourceVertex, targetVertex)) return;
                model.AddArrow(sourceVertex, targetVertex);
                model.SelectVertex(targetVertex);
            }
            else
            {
                model.SelectVertex(vertex);
            }
        }

        private void View_VertexMouseDown(object sender, VertexMouseDownEventArgs e)
        {
            switch (model.SelectedTool)
            {
                case QuiverEditorTool.SelectMove: HandleClickWithSelectMove(); break;
                case QuiverEditorTool.AddVertex: HandleClickWithAddVertex(); break;
                case QuiverEditorTool.AddArrow: HandleClickWithAddArrow(); break;
                case QuiverEditorTool.SelectPath: HandleClickWithSelectPath(); break;
            }

            void HandleClickWithSelectMove()
            {
                SelectVertex(e.Vertex);
            }

            void HandleClickWithAddVertex()
            {
                // Do nothing
            }

            void HandleClickWithAddArrow()
            {
                AddArrowOrSelectVertex(e.Vertex);
            }

            void HandleClickWithSelectPath()
            {
                if (model.HasSelectedVertex)
                {
                    var arrow = new Arrow<int>(model.SelectedVertex.Value, e.Vertex);
                    if (model.quiverInPlane.ContainsArrow(arrow)) model.ExtendSelectedPath(arrow);
                }
                else
                {
                    model.SelectVertex(e.Vertex);
                }
            }
        }

        private void View_ArrowMouseDown(object sender, ArrowMouseDownEventArgs e)
        {
            switch (model.SelectedTool)
            {
                case QuiverEditorTool.SelectMove: HandleClickWithSelectMove(); break;
                case QuiverEditorTool.AddVertex: HandleClickWithAddVertex(); break;
                case QuiverEditorTool.AddArrow: HandleClickWithAddArrow(); break;
                case QuiverEditorTool.SelectPath: HandleClickWithSelectPath(); break;
            }

            void HandleClickWithSelectMove()
            {
                SelectArrow(e.Arrow);
            }

            void HandleClickWithAddVertex()
            {
                // Do nothing
            }

            void HandleClickWithAddArrow()
            {
                // Do nothing
            }

            void HandleClickWithSelectPath()
            {
                if (model.HasSelectedPath && model.SelectedPath.EndingPoint == e.Arrow.Source)
                {
                    model.ExtendSelectedPath(e.Arrow);
                }
            }
        }

        private void View_CanvasClicked(object sender, CanvasClickedEventArgs e)
        {
            switch (model.SelectedTool)
            {
                case QuiverEditorTool.SelectMove: HandleClickWithSelectMove(); break;
                case QuiverEditorTool.AddVertex: HandleClickWithAddVertex(); break;
                case QuiverEditorTool.AddArrow: HandleClickWithAddArrow(); break;
                case QuiverEditorTool.SelectPath: HandleClickWithSelectPath(); break;
            }

            void HandleClickWithSelectMove()
            {
                model.DeselectAll();
            }

            void HandleClickWithAddVertex()
            {
                model.TryAddNextVertex(e.Location);
            }

            void HandleClickWithAddArrow()
            {
                // Do nothing
            }

            void HandleClickWithSelectPath()
            {
                // Do nothing
            }
        }

        private void View_VertexSelectedInListView(object sender, VertexSelectedInListViewEventArgs e)
        {
            if (!e.Vertex.HasValue)
            {
                DeselectVertex();
                return;
            }

            // Because the selected index of a list view is cleared before it is changed,
            // this call will never add an arrow.
            AddArrowOrSelectVertex(e.Vertex.Value);
        }

        private void View_ArrowSelectedInListView(object sender, ArrowSelectedInListViewEventArgs e)
        {
            if (e.Arrow != null) SelectArrow(e.Arrow);
            else DeselectArrow();
        }

        private void View_ToolButtonClicked(object sender, ToolButtonClickedEventArgs e)
        {
            SelectTool(e.Tool);
        }

        private void View_VertexToAddNudValueChanged(object sender, VertexToAddChangedEventArgs e)
        {
            SetVertexToAdd(e.VertexToAdd);
        }

        private void View_UndoMenuItemClicked(object sender, EventArgs e)
        {
            UndoAction();
        }

        private void View_RedoMenuItemClicked(object sender, EventArgs e)
        {
            RedoAction();
        }

        private void View_RelabelVerticesMenuItemClicked(object sender, EventArgs e)
        {
            // Weird to have the controller open dialogs (this seems like the work of the view).
            var dialog = new RelabelVerticesDialog();
            if (dialog.ShowDialog() != DialogResult.OK) return;

            var oldLabelsString = dialog.OldVerticesString;
            var newLabelsString = dialog.NewVerticesString;

            var parser = new RelabelVerticesParser();
            bool success = parser.TryCreateRelabelingMap(oldLabelsString, newLabelsString, out var relabelingMap, out string errorMessage);
            if (!success)
            {
                view.HandleErrorMessage(errorMessage);
                return;
            }

            var result = model.RelabelVertices(relabelingMap);
            if (result != RelabelVerticesResult.Success)
            {
                switch (result)
                {
                    case RelabelVerticesResult.OldVertexNotInQuiver: errorMessage = "An old vertex not in the quiver was specified."; break;
                    case RelabelVerticesResult.NewVertexClashesWithPreExistingVertex: errorMessage = "A new vertex that clashes with a pre-existing vertex was specified."; break;
                    // The case RelabelVerticesResult.DuplicateNewVertex is already handled by the parser.
                    default: Debug.Fail("Switch over relabel vertices result failed."); break;
                }

                view.HandleErrorMessage(errorMessage);
                return;
            }
        }

        private void View_RotateVerticesMenuItemClicked(object sender, EventArgs e)
        {
            // Weird to have the controller open dialogs (this seems like the work of the view).
            var dialog = new RotateVerticesDialog();
            if (dialog.ShowDialog() != DialogResult.OK) return;

            var verticesString = dialog.VerticesString;
            var centerX = dialog.CenterX;
            var centerY = dialog.CenterY;
            var degrees = dialog.Degrees;

            var parser = new RotateVerticesParser();
            bool success = parser.TryGetRotationData(verticesString, out var vertices, out string errorMessage);
            if (!success)
            {
                view.HandleErrorMessage(errorMessage);
                return;
            }

            model.RotateVertices(vertices, centerX, centerY, degrees);
        }

        private void View_PredefinedQuiverDialogAccepted(object sender, PredefinedQuiverDialogAcceptedEventArgs e)
        {
            LoadPredefinedQuiver(e.PredefinedQuiver, e.QuiverParameter);
        }

        private void SelectTool(QuiverEditorTool tool)
        {
            if (model.SelectedTool == tool) return;
            model.SelectTool(tool);
        }

        private void SetVertexToAdd(int vertexToAdd)
        {
            model.SetVertexToAdd(vertexToAdd);
        }

        private void SelectVertex(int vertex)
        {
            model.SelectVertex(vertex);
        }

        private void DeselectVertex()
        {
            model.DeselectVertex();
        }

        private void SelectArrow(Arrow<int> arrow)
        {
            model.SelectArrow(arrow);
        }

        private void DeselectArrow()
        {
            model.DeselectArrow();
        }

        private void UndoAction()
        {
            model.UndoAction();
        }

        private void RedoAction()
        {
            model.RedoAction();
        }

        private void LoadPredefinedQuiver(PredefinedQuiver predefinedQuiver, dynamic quiverParameter)
        {
            model.LoadPredefinedQuiver(predefinedQuiver, quiverParameter);
        }

        private void View_ImportQuiverFromMutationAppFileDialogAccepted(object sender, ImportQuiverFromMutationAppFileDialogAcceptedEventArgs e)
        {
            try
            {
                model.ImportQuiverFromMutationAppFile(e.Path);
            }
            catch (ImporterException ex)
            {
                view.HandleException(ex);
            }
        }

        private void View_ExportQuiverAsMutationAppFileDialogAccepted(object sender, ExportQuiverAsMutationAppFileDialogAcceptedEventArgs e)
        {
            try
            {
                model.ExportQuiverToMutationAppFile(e.Path);
            }
            catch (ExporterException ex)
            {
                view.HandleException(ex);
            }
        }
    }
}
