using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SelfInjectiveQuiversWithPotential;
using SelfInjectiveQuiversWithPotential.Plane;
using Point = SelfInjectiveQuiversWithPotential.Plane.Point;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// This class represents the view component of the quiver editor.
    /// </summary>
    public class QuiverEditorView : IDisposable
    {
        private readonly QuiverEditorModel model;

        /// <summary>
        /// Used to detect canvas clicks and drags.
        /// </summary>
        private bool canvasPressed;

        /// <summary>
        /// Used to handle canvas drags.
        /// </summary>
        private System.Drawing.Point lastCanvasPosition;

        /// <summary>
        /// Used to distinguish between canvas-dragging and canvas-clicking.
        /// </summary>
        private bool cursorHasMovedSinceMouseDown;

        /// <summary>
        /// The &quot;quiver point&quot; corresponding to the center of the canvas as displayed in
        /// the view.
        /// </summary>
        private Point centerOfCanvas;

        private Point CenterOfCanvas
        {
            get => centerOfCanvas;
            set
            {
                centerOfCanvas = value;
                centerOfCanvasLabel.Text = $"Center: {centerOfCanvas}";
            }
        }

        /// <summary>
        /// Sorted list of vertices synchronized with the vertex list view.
        /// </summary>
        private SortedList<int, int> vertices;

        /// <summary>
        /// Sorted list of vertices synchronized with the arrow list view.
        /// </summary>
        private SortedList<Arrow<int>, Arrow<int>> arrows;

        // For drawing
        private const int VertexRadius = 10;
        private const int VertexRadiusSquare = VertexRadius * VertexRadius;
        private readonly Color canvasBackgroundColor = Color.White;
        private readonly Color defaultVertexColor;
        private readonly Color selectedVertexColor;
        private readonly Pen vertexBoundaryPen;
        private readonly SolidBrush defaultVertexBackgroundBrush;
        private readonly SolidBrush selectedVertexBackgroundBrush;
        private readonly SolidBrush vertexTextBrush;
        private readonly Pen defaultArrowPen;
        private readonly Pen selectedArrowPen;
        private readonly BufferedGraphics inMemoryGraphics;

        // Controls
        private readonly Canvas canvas;
        private readonly Label centerOfCanvasLabel;
        private readonly Label mousePointerOnCanvasLocationLabel;
        private readonly ListView vertexListView;
        private readonly Label vertexCountLabel;
        private readonly Label arrowCountLabel;
        private readonly ListView arrowListView;
        private readonly IReadOnlyDictionary<QuiverEditorTool, Button> toolButtons;
        private readonly IReadOnlyDictionary<QuiverEditorTool, IEnumerable<Control>> toolSettingsControlsDictionary;
        private readonly NumericUpDown vertexToAddNud;
        private readonly ToolStripMenuItem undoMenuItem;
        private readonly ToolStripMenuItem redoMenuItem;
        private readonly OpenFileDialog importFromMutationAppFileOpenFileDialog;
        private readonly SaveFileDialog exportAsMutationAppFileSaveFileDialog;

        /// <summary>
        /// A boolean used to avoid infinite loops arising when the user interacts with one control
        /// updating the model in such a way that another control (changes to which also updates
        /// the model) needs to be updated, e.g., when selecting a vertex by clicking in the canvas
        /// which updates the selection in the vertex or arrow list view, which in turn
        /// <em>would</em> update the model.
        /// </summary>
        /// <remarks>It is currently not used, because no infinite loops seem to arise.</remarks>
        private bool suppressEventsFromFiring;

        // Events
        // Sort of bad to have the name include implementations details (e.g., ToolButtonClicked indicating that
        // the tools are necessarily selected using a button), but it works for now
        // Also, for KeyDown, maybe filter out the relevant key downs in the view and have the event data be
        // a custom enum indicating the 'action'.
        public event EventHandler<KeyDownEventArgs> KeyDown;
        public event EventHandler<KeyDownEventArgs> VertexListViewKeyDown;
        public event EventHandler<KeyDownEventArgs> ArrowListViewKeyDown;

        /// <summary>
        /// Occurs when a key for selecting a tool is pressed down.
        /// </summary>
        public event EventHandler<SelectToolKeyDownEventArgs> SelectToolKeyDown;

        /// <summary>
        /// Occurs when the mouse pointer is over a vertex and a mouse button is pressed.
        /// </summary>
        public event EventHandler<VertexMouseDownEventArgs> VertexMouseDown;

        /// <summary>
        /// Occurs when the mouse pointer is over an arrow and a mouse button is pressed.
        /// </summary>
        public event EventHandler<ArrowMouseDownEventArgs> ArrowMouseDown;

        /// <summary>
        /// Occurs when the canvas (not a vertex or an arrow) is clicked.
        /// </summary>
        /// <remarks>
        /// <para>More precisely, this event is fired when the a mouse pointer is over the canvas
        /// (but not a vertex or arrow), a mouse button is pressed, and a mouse button is released
        /// without the mouse pointer moving between the press and the release.</para>
        /// </remarks>
        public event EventHandler<CanvasClickedEventArgs> CanvasClicked;

        public event EventHandler<VertexSelectedInListViewEventArgs> VertexSelectedInListView;
        public event EventHandler<ArrowSelectedInListViewEventArgs> ArrowSelectedInListView;
        public event EventHandler<ToolButtonClickedEventArgs> ToolButtonClicked;
        public event EventHandler<VertexToAddChangedEventArgs> VertexToAddChanged;
        public event EventHandler UndoMenuItemClicked;
        public event EventHandler RedoMenuItemClicked;
        public event EventHandler RelabelVerticesMenuItemClicked;
        public event EventHandler RotateVerticesMenuItemClicked;
        public event EventHandler<PredefinedQuiverDialogAcceptedEventArgs> PredefinedQuiverDialogAccepted;
        public event EventHandler<ImportQuiverFromMutationAppFileDialogAcceptedEventArgs> ImportQuiverFromMutationAppFileDialogAccepted;
        public event EventHandler<ExportQuiverAsMutationAppFileDialogAcceptedEventArgs> ExportQuiverAsMutationAppFileDialogAccepted;

        public QuiverEditorView(
            QuiverEditorModel model,
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
            ToolStripMenuItem importFromMutationAppFileMenuItem,
            OpenFileDialog importFromMutationAppFileOpenFileDialog,
            ToolStripMenuItem exportAsMutationAppFileMenuItem,
            SaveFileDialog exportAsMutationAppFileSaveFileDialog)
        {
            if (parent is null) throw new ArgumentNullException(nameof(parent));
            this.model = model ?? throw new ArgumentNullException(nameof(model));
            this.canvas = canvas ?? throw new ArgumentNullException(nameof(canvas));
            this.vertexListView = vertexListView ?? throw new ArgumentNullException(nameof(vertexListView));
            this.vertexCountLabel = vertexCountLabel ?? throw new ArgumentNullException(nameof(vertexCountLabel));
            this.arrowListView = arrowListView ?? throw new ArgumentNullException(nameof(arrowListView));
            this.arrowCountLabel = arrowCountLabel ?? throw new ArgumentNullException(nameof(arrowCountLabel));
            this.toolButtons = toolButtons ?? throw new ArgumentNullException(nameof(toolButtons));
            if (!toolButtons.HasAllEnumMembersAsKeys())
                throw new ArgumentException($"The keys collection of the tool button dictionary does not coincide with the {typeof(QuiverEditorTool)} enum.");
            this.toolSettingsControlsDictionary = toolSettingsControlsDictionary ?? throw new ArgumentNullException(nameof(toolSettingsControlsDictionary));
            if (!toolSettingsControlsDictionary.HasAllEnumMembersAsKeys())
                throw new ArgumentException($"The keys collection of the tool settings control dictionary does not coincide with the {typeof(QuiverEditorTool)} enum.");
            if (predefinedQuiverMenuItems is null) throw new ArgumentNullException(nameof(predefinedQuiverMenuItems));
            if (!predefinedQuiverMenuItems.HasAllEnumMembersAsKeys())
                throw new ArgumentException($"The keys collection of the predefined quiver menu items dictionary does not coincide with the {typeof(PredefinedQuiver)} enum.");
            if (exportAsMutationAppFileMenuItem is null) throw new ArgumentNullException(nameof(exportAsMutationAppFileMenuItem));

            this.vertexToAddNud = vertexToAddNud ?? throw new ArgumentNullException(nameof(vertexToAddNud));
            this.undoMenuItem = undoMenuItem ?? throw new ArgumentNullException(nameof(undoMenuItem));
            this.redoMenuItem = redoMenuItem ?? throw new ArgumentNullException(nameof(redoMenuItem));
            if (relabelVerticesMenuItem is null) throw new ArgumentNullException(nameof(relabelVerticesMenuItem));
            if (rotateVerticesMenuItem is null) throw new ArgumentNullException(nameof(rotateVerticesMenuItem));

            this.importFromMutationAppFileOpenFileDialog = importFromMutationAppFileOpenFileDialog ?? throw new ArgumentNullException(nameof(importFromMutationAppFileOpenFileDialog));
            this.exportAsMutationAppFileSaveFileDialog = exportAsMutationAppFileSaveFileDialog ?? throw new ArgumentNullException(nameof(exportAsMutationAppFileSaveFileDialog));

            this.centerOfCanvasLabel = centerOfCanvasLabel ?? throw new ArgumentNullException(nameof(centerOfCanvasLabel));
            this.mousePointerOnCanvasLocationLabel = mousePointerOnCanvasLocationLabel ?? throw new ArgumentNullException(nameof(mousePointerOnCanvasLocationLabel));

            CenterOfCanvas = Point.Origin;
            vertices = new SortedList<int, int>();
            arrows = new SortedList<Arrow<int>, Arrow<int>>();

            suppressEventsFromFiring = false;

            canvas.PreviewKeyDown += View_PreviewKeyDown;

            this.canvas = canvas;
            canvas.Paint += Canvas_Paint;
            canvas.MouseMove += Canvas_MouseMove;
            canvas.MouseDown += Canvas_MouseDown;
            canvas.MouseUp += Canvas_MouseUp;

            vertexListView.SelectedIndexChanged += VertexListView_SelectedIndexChanged;
            vertexListView.KeyDown += VertexListView_KeyDown;
            arrowListView.SelectedIndexChanged += ArrowListView_SelectedIndexChanged;
            arrowListView.KeyDown += ArrowListView_KeyDown;

            foreach (var (tool, button) in toolButtons)
            {
                button.Click += (object sender, EventArgs e) => ToolButton_Click(sender, e, tool);
                button.KeyDown += View_KeyDown;
            }

            MakeTextBold(toolButtons[model.SelectedTool]);

            this.vertexToAddNud = vertexToAddNud;
            vertexToAddNud.ValueChanged += VertexToAddNud_ValueChanged;

            undoMenuItem.Click += UndoMenuItem_Click;
            redoMenuItem.Click += RedoMenuItem_Click;
            relabelVerticesMenuItem.Click += RelabelVerticesMenuItem_Click;
            rotateVerticesMenuItem.Click += RotateVerticesMenuItem_Click;

            model.ArrowAdded += Model_ArrowAdded;
            model.ArrowDeselected += Model_ArrowDeselected;
            model.ArrowRemoved += Model_ArrowRemoved;
            model.ArrowSelected += Model_ArrowSelected;
            model.VertexAdded += Model_VertexAdded;
            model.VertexDeselected += Model_VertexDeselected;
            model.VertexMoved += Model_VertexMoved;
            model.VertexRemoved += Model_VertexRemoved;
            model.VertexSelected += Model_VertexSelected;
            model.PathSelected += Model_PathSelected;
            model.PathDeselected += Model_PathDeselected;
            model.ToolSelected += Model_ToolSelected;

            model.ToolSettings.VertexToAddChanged += ToolSettings_VertexToAddChanged;

            model.UndoableActionsChanged += Model_UndoableActionsChanged;
            model.RedoableActionsChanged += Model_RedoableActionsChanged;

            model.QuiverLoaded += Model_QuiverLoaded;

            foreach (var (predefinedQuiver, menuItem) in predefinedQuiverMenuItems)
            {
                menuItem.Click += (object sender, EventArgs e) => PredefinedQuiverMenuItem_Click(sender, e, predefinedQuiver);
            }

            importFromMutationAppFileMenuItem.Click += ImportFromMutationAppFileMenuItem_Click;
            exportAsMutationAppFileMenuItem.Click += ExportAsMutationAppFileMenuItem_Click;

            defaultVertexColor = Color.AliceBlue;
            selectedVertexColor = Color.Red;
            vertexBoundaryPen = new Pen(Color.Black);
            defaultVertexBackgroundBrush = new SolidBrush(defaultVertexColor);
            selectedVertexBackgroundBrush = new SolidBrush(selectedVertexColor);
            vertexTextBrush = new SolidBrush(Color.Black);
            defaultArrowPen = new Pen(Color.Black);
            selectedArrowPen = new Pen(Color.Red);

            // For drawing smoothly
            inMemoryGraphics = BufferedGraphicsManager.Current.Allocate(canvas.CreateGraphics(), new Rectangle(0, 0, canvas.Size.Width, canvas.Size.Height));
            inMemoryGraphics.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            UpdateViewForInitialization();
        }

        /// <summary>
        /// Updates the view according to the model for initialization purposes.
        /// </summary>
        /// <remarks>This is used to make sure that the initial view agrees with the initial model.
        /// Changes to the model are updated using the appropriate event handlers.</remarks>
        private void UpdateViewForInitialization()
        {
            foreach (var tool in Utility.GetEnumValues<QuiverEditorTool>())
            {
                UpdateToolSettingsControlsVisibility(tool, tool == model.SelectedTool);
            }

            UpdateUndoMenuItem(model.CanUndoAction, model.NextActionToUndo);
            UpdateRedoMenuItem(model.CanRedoAction, model.NextActionToRedo);
        }

        private void UpdateViewForQuiverLoaded(QuiverInPlane<int> quiverInPlane)
        {
            vertices = new SortedList<int, int>(quiverInPlane.Vertices.ToDictionary(vertex => vertex, vertex => vertex));
            var vertexListViewItems = vertices.Keys.Select(vertex => CreateVertexListViewItem(vertex));
            vertexListView.Items.Clear();
            vertexListView.Items.AddRange(vertexListViewItems.ToArray());
            UpdateVertexCountLabel();

            arrows = new SortedList<Arrow<int>, Arrow<int>>(quiverInPlane.GetArrows().ToDictionary(arrow => arrow, arrow => arrow));
            var arrowListViewItems = arrows.Keys.Select(arrow => CreateArrowListViewItem(arrow));
            arrowListView.Items.Clear();
            arrowListView.Items.AddRange(arrowListViewItems.ToArray());
            UpdateArrowCountLabel();

            DrawQuiver();
        }

        private void UpdateUndoMenuItem(bool enabled, IUndoableRedoableEditorAction action)
        {
            undoMenuItem.Enabled = enabled;
            undoMenuItem.Text = enabled ? $"Undo '{action}'" : "Undo";
        }

        private void UpdateRedoMenuItem(bool enabled, IUndoableRedoableEditorAction action)
        {
            redoMenuItem.Enabled = enabled;
            redoMenuItem.Text = enabled ? $"Redo '{action}'" : "Redo";
        }

        private void View_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            var ke = new KeyEventArgs(e.KeyData);
            View_KeyDown(this, ke);
        }

        private void View_KeyDown(object sender, KeyEventArgs e)
        {
            KeyDown?.Invoke(this, new KeyDownEventArgs(e));

            if (Keys.D1 <= e.KeyCode && e.KeyCode <= Keys.D4)
            {
                var tool = (QuiverEditorTool)(e.KeyCode - Keys.D1);
                SelectToolKeyDown?.Invoke(this, new SelectToolKeyDownEventArgs(tool));
            }
        }

        private void VertexListView_KeyDown(object sender, KeyEventArgs e)
        {
            VertexListViewKeyDown?.Invoke(this, new KeyDownEventArgs(e));
        }

        private void ArrowListView_KeyDown(object sender, KeyEventArgs e)
        {
            ArrowListViewKeyDown?.Invoke(this, new KeyDownEventArgs(e));
        }

        private void MakeTextBold(Control control)
        {
            var oldFont = control.Font;
            control.Font = new Font(oldFont.Name, oldFont.Size, oldFont.Style | FontStyle.Bold);
        }

        private void MakeTextNonBold(Control control)
        {
            var oldFont = control.Font;
            control.Font = new Font(oldFont.Name, oldFont.Size, oldFont.Style & ~FontStyle.Bold);
        }

        private void UpdateToolSettingsControlsVisibility(QuiverEditorTool tool, bool visible)
        {
            foreach (var control in toolSettingsControlsDictionary[tool]) control.Visible = visible;
        }

        private ListViewItem CreateVertexListViewItem(int vertex)
        {
            return new ListViewItem(vertex.ToString());
        }

        private void UpdateVertexCountLabel()
        {
            vertexCountLabel.Text = $"Vertex count: {vertexListView.Items.Count}";
        }

        private void InsertVertexIntoVertexList(int vertex)
        {
            vertices.Add(vertex, vertex);
            int index = vertices.IndexOfKey(vertex);
            vertexListView.Items.Insert(index, CreateVertexListViewItem(vertex));
            UpdateVertexCountLabel();
        }

        private void RemoveVertexFromVertexList(int vertex)
        {
            int index = vertices.IndexOfKey(vertex);
            vertices.Remove(vertex);
            vertexListView.Items.RemoveAt(index);
            UpdateVertexCountLabel();
        }

        private ListViewItem CreateArrowListViewItem(Arrow<int> arrow)
        {
            return new ListViewItem(arrow.ToString());
        }

        private void UpdateArrowCountLabel()
        {
            arrowCountLabel.Text = $"Arrow count: {arrowListView.Items.Count}";
        }

        private void InsertArrowIntoArrowList(Arrow<int> arrow)
        {
            arrows.Add(arrow, arrow);
            int index = arrows.IndexOfKey(arrow);
            arrowListView.Items.Insert(index, CreateArrowListViewItem(arrow));
            UpdateArrowCountLabel();
        }

        private void RemoveArrowFromArrowList(Arrow<int> arrow)
        {
            int index = arrows.IndexOfKey(arrow);
            arrows.Remove(arrow);
            arrowListView.Items.RemoveAt(index);
            UpdateArrowCountLabel();
        }

        private void SelectVertexInListView(int vertex)
        {
            int index = vertices.IndexOfKey(vertex);
            vertexListView.Items[index].Selected = true;
        }

        private void DeselectVertexInListView(int vertex)
        {
            int index = vertices.IndexOfKey(vertex);
            vertexListView.Items[index].Selected = false;
        }

        private void SelectArrowInListView(Arrow<int> arrow)
        {
            int index = arrows.IndexOfKey(arrow);
            arrowListView.Items[index].Selected = true;
        }

        private void DeselectArrowInListView(Arrow<int> arrow)
        {
            int index = arrows.IndexOfKey(arrow);
            arrowListView.Items[index].Selected = false;
        }

        private void Model_ToolSelected(object sender, ToolSelectedEventArgs e)
        {
            MakeTextNonBold(toolButtons[e.PreviousTool]);
            MakeTextBold(toolButtons[e.NewTool]);
            toolButtons[e.NewTool].Focus();

            UpdateToolSettingsControlsVisibility(e.PreviousTool, false);
            UpdateToolSettingsControlsVisibility(e.NewTool, true);
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            DrawQuiver();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            // Update label displaying the mouse pointer position
            var quiverPoint = DrawingPointToQuiverPoint(e.Location);
            mousePointerOnCanvasLocationLabel.Text = $"Pointer: {quiverPoint}";

            // Do canvas dragging stuff
            if (!canvasPressed) return;

            cursorHasMovedSinceMouseDown = true;

            var drawingDelta = e.Location.Minus(lastCanvasPosition);
            var quiverDelta = DrawingDeltaToQuiverDelta(drawingDelta);
            CenterOfCanvas = CenterOfCanvas - quiverDelta;

            lastCanvasPosition = e.Location;

            DrawQuiver();
        }

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            var quiverPoint = DrawingPointToQuiverPoint(e.Location);
            if (TryGetVertex(quiverPoint, out var vertex))
            {
                VertexMouseDown?.Invoke(this, new VertexMouseDownEventArgs(vertex));
            }
            else if (TryGetArrow(quiverPoint, out var arrow))
            {
                ArrowMouseDown?.Invoke(this, new ArrowMouseDownEventArgs(arrow));
            }
            else
            {
                canvasPressed = true;
                lastCanvasPosition = e.Location;
                cursorHasMovedSinceMouseDown = false;
            }
        }

        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (canvasPressed && !cursorHasMovedSinceMouseDown)
            {
                var quiverPoint = DrawingPointToQuiverPoint(e.Location);
                CanvasClicked?.Invoke(this, new CanvasClickedEventArgs(quiverPoint));
            }

            canvasPressed = false;
        }

        private void VertexListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = vertexListView.SelectedIndices.Count == 0 ? -1 : vertexListView.SelectedIndices[0];
            int? vertex = index == -1 ? default(int?) : vertices.Values[index];
            if (!suppressEventsFromFiring) VertexSelectedInListView?.Invoke(this, new VertexSelectedInListViewEventArgs(vertex));
        }

        private void ArrowListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = arrowListView.SelectedIndices.Count == 0 ? -1 : arrowListView.SelectedIndices[0];
            Arrow<int> arrow = index == -1 ? null : arrows.Values[index];
            ArrowSelectedInListView?.Invoke(this, new ArrowSelectedInListViewEventArgs(arrow));
        }

        private void ToolButton_Click(object sender, EventArgs e, QuiverEditorTool tool)
        {
            ToolButtonClicked?.Invoke(this, new ToolButtonClickedEventArgs(tool));
        }

        private void ToolSettings_VertexToAddChanged(object sender, VertexToAddChangedEventArgs e)
        {
            vertexToAddNud.Value = e.VertexToAdd;
        }

        private void VertexToAddNud_ValueChanged(object sender, EventArgs e)
        {
            VertexToAddChanged?.Invoke(this, new VertexToAddChangedEventArgs((int)vertexToAddNud.Value));
        }

        private void UndoMenuItem_Click(object sender, EventArgs e)
        {
            UndoMenuItemClicked?.Invoke(this, e);
        }

        private void RedoMenuItem_Click(object sender, EventArgs e)
        {
            RedoMenuItemClicked?.Invoke(this, e);
        }

        private void RelabelVerticesMenuItem_Click(object sender, EventArgs e)
        {
            RelabelVerticesMenuItemClicked?.Invoke(this, e);
        }

        private void RotateVerticesMenuItem_Click(object sender, EventArgs e)
        {
            RotateVerticesMenuItemClicked?.Invoke(this, e);
        }

        private void ExecuteWithoutFiringEvents(Action action)
        {
            suppressEventsFromFiring = true;
            action();
            suppressEventsFromFiring = false;
        }

        private void Model_VertexSelected(object sender, VertexSelectedEventArgs e)
        {
            ExecuteWithoutFiringEvents(() =>
            {
                SelectVertexInListView(e.Vertex);
                DrawQuiver();
            });
        }

        private void Model_VertexDeselected(object sender, VertexDeselectedEventArgs e)
        {
            ExecuteWithoutFiringEvents(() =>
            {
                DeselectVertexInListView(e.Vertex);
                DrawQuiver();
            });
        }

        private void Model_VertexMoved(object sender, VertexMovedEventArgs e)
        {
            DrawQuiver();
        }

        private void Model_VertexAdded(object sender, VertexAddedEventArgs e)
        {
            InsertVertexIntoVertexList(e.Vertex);
            DrawQuiver();
        }

        private void Model_VertexRemoved(object sender, VertexRemovedEventArgs e)
        {
            RemoveVertexFromVertexList(e.Vertex);
            DrawQuiver();
        }

        private void Model_ArrowSelected(object sender, ArrowSelectedEventArgs e)
        {
            SelectArrowInListView(e.Arrow);
            DrawQuiver();
        }

        private void Model_ArrowDeselected(object sender, ArrowDeselectedEventArgs e)
        {
            DeselectArrowInListView(e.Arrow);
            DrawQuiver();
        }

        private void Model_ArrowAdded(object sender, ArrowAddedEventArgs e)
        {
            InsertArrowIntoArrowList(e.Arrow);
            DrawQuiver();
        }

        private void Model_ArrowRemoved(object sender, ArrowRemovedEventArgs e)
        {
            RemoveArrowFromArrowList(e.Arrow);
            DrawQuiver();
        }

        private void Model_PathSelected(object sender, PathSelectedEventArgs e)
        {
            DrawQuiver(); // Sort of superfluous, because the quiver will be drawn in Model_VertexSelected or so
        }

        private void Model_PathDeselected(object sender, PathDeselectedEventArgs e)
        {
            DrawQuiver(); // Sort of superfluous, because the quiver will be drawn in Model_VertexDeselected or so
        }

        private void Model_UndoableActionsChanged(object sender, UndoableActionsChangedEventArgs e)
        {
            UpdateUndoMenuItem(e.Action != null, e.Action);
        }

        private void Model_RedoableActionsChanged(object sender, RedoableActionsChangedEventArgs e)
        {
            UpdateRedoMenuItem(e.Action != null, e.Action);
        }

        private void Model_QuiverLoaded(object sender, QuiverLoadedEventArgs e)
        {
            UpdateViewForQuiverLoaded(e.QuiverInPlane);
        }

        private void PredefinedQuiverMenuItem_Click(object sender, EventArgs e, PredefinedQuiver predefinedQuiver)
        {
            CustomDialog dialog;
            Func<dynamic> getQuiverParameter;
            Func<dynamic, bool> validateParameter;
            string parameterValidityDescription;

            switch (predefinedQuiver)
            {
                case PredefinedQuiver.Cycle:
                    var cycleDialog = new PredefinedCycleDialog();
                    dialog = cycleDialog;
                    getQuiverParameter = () => cycleDialog.CycleLength;
                    validateParameter = param => UsefulQuiversInPlane.CycleParameterIsValid(param);
                    parameterValidityDescription = UsefulQuiversInPlane.CycleParameterValidityDescription;
                    break;
                case PredefinedQuiver.Triangle:
                    var triangleDialog = new PredefinedTriangleDialog();
                    dialog = triangleDialog;
                    getQuiverParameter = () => triangleDialog.NumRows;
                    validateParameter = param => UsefulQuiversInPlane.TriangleParameterIsValid(param);
                    parameterValidityDescription = UsefulQuiversInPlane.TriangleParameterValidityDescription;
                    break;
                case PredefinedQuiver.Square:
                    var squareDialog = new PredefinedSquareDialog();
                    dialog = squareDialog;
                    getQuiverParameter = () => squareDialog.NumRows;
                    validateParameter = param => UsefulQuiversInPlane.SquareParameterIsValid(param);
                    parameterValidityDescription = UsefulQuiversInPlane.SquareParameterValidityDescription;
                    break;
                case PredefinedQuiver.Cobweb:
                    var cobwebDialog = new PredefinedCobwebDialog();
                    dialog = cobwebDialog;
                    getQuiverParameter = () => cobwebDialog.NumVerticesInCenterPolygon;
                    validateParameter = param => UsefulQuiversInPlane.CobwebParameterIsValid(param);
                    parameterValidityDescription = UsefulQuiversInPlane.CobwebParameterValidityDescription;
                    break;
                case PredefinedQuiver.OddFlower:
                    var oddFlowerDialog = new PredefinedOddFlowerDialog();
                    dialog = oddFlowerDialog;
                    getQuiverParameter = () => oddFlowerDialog.NumVerticesInCenterPolygon;
                    validateParameter = param => UsefulQuiversInPlane.OddFlowerParameterIsValid(param);
                    parameterValidityDescription = UsefulQuiversInPlane.OddFlowerParameterValidityDescription;
                    break;
                case PredefinedQuiver.EvenFlowerType1:
                    var evenFlowerType1Dialog = new PredefinedEvenFlowerType1Dialog();
                    dialog = evenFlowerType1Dialog;
                    getQuiverParameter = () => evenFlowerType1Dialog.NumVerticesInCenterPolygon;
                    validateParameter = param => UsefulQuiversInPlane.EvenFlowerType1ParameterIsValid(param);
                    parameterValidityDescription = UsefulQuiversInPlane.EvenFlowerType1ParameterValidityDescription;
                    break;
                case PredefinedQuiver.EvenFlowerType2:
                    var evenFlowerType2Dialog = new PredefinedEvenFlowerType2Dialog();
                    dialog = evenFlowerType2Dialog;
                    getQuiverParameter = () => evenFlowerType2Dialog.NumVerticesInCenterPolygon;
                    validateParameter = param => UsefulQuiversInPlane.EvenFlowerType2ParameterIsValid(param);
                    parameterValidityDescription = UsefulQuiversInPlane.EvenFlowerType2ParameterValidityDescription;
                    break;
                case PredefinedQuiver.PointedFlower:
                    var pointedFlowerDialog = new PredefinedPointedFlowerDialog();
                    dialog = pointedFlowerDialog;
                    getQuiverParameter = () => pointedFlowerDialog.NumPeriods;
                    validateParameter = param => UsefulQuiversInPlane.PointedFlowerParameterIsValid(param);
                    parameterValidityDescription = UsefulQuiversInPlane.PointedFlowerParameterValidityDescription;
                    break;
                case PredefinedQuiver.GeneralizedCobweb:
                    var generalizedCobwebDialog = new PredefinedGeneralizedCobwebDialog();
                    dialog = generalizedCobwebDialog;
                    getQuiverParameter = () => (generalizedCobwebDialog.NumVerticesInCenterPolygon, generalizedCobwebDialog.NumLayers);
                    validateParameter = param =>
                    {
                        var (numVerticesInCenterPolygon, numLayers) = ((int, int))param;
                        return UsefulQuiversInPlane.GeneralizedCobwebParameterIsValid(numVerticesInCenterPolygon, numLayers);
                    };
                    parameterValidityDescription = UsefulQuiversInPlane.GeneralizedCobwebParameterValidityDescription;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(predefinedQuiver));
            }

            if (dialog.ShowDialog() != DialogResult.OK) return;

            var quiverParameter = getQuiverParameter();
            if (!validateParameter(quiverParameter))
            {
                MessageBox.Show(
                    parameterValidityDescription,
                    "Invalid parameter(s)",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            PredefinedQuiverDialogAccepted?.Invoke(this, new PredefinedQuiverDialogAcceptedEventArgs(predefinedQuiver, quiverParameter));
        }

        private void ImportFromMutationAppFileMenuItem_Click(object sender, EventArgs e)
        {
            if (importFromMutationAppFileOpenFileDialog.ShowDialog() != DialogResult.OK) return;
            var path = importFromMutationAppFileOpenFileDialog.FileName;

            ImportQuiverFromMutationAppFileDialogAccepted?.Invoke(this, new ImportQuiverFromMutationAppFileDialogAcceptedEventArgs(path));
        }

        private void ExportAsMutationAppFileMenuItem_Click(object sender, EventArgs e)
        {
            if (exportAsMutationAppFileSaveFileDialog.ShowDialog() != DialogResult.OK) return;
            var path = exportAsMutationAppFileSaveFileDialog.FileName;

            ExportQuiverAsMutationAppFileDialogAccepted?.Invoke(this, new ExportQuiverAsMutationAppFileDialogAcceptedEventArgs(path));
        }

        private System.Drawing.Point QuiverPointToDrawingPoint(Point point)
        {
            // Translate according to centerOfCanvas
            point = point - CenterOfCanvas;

            // Negation to go from math axes to computer science axes (drawing point with respect to center of canvas)
            var drawingPoint = new System.Drawing.Point(point.X, -point.Y);

            // Drawing point with respect to the upper left corner of the canvas
            drawingPoint = new System.Drawing.Point(drawingPoint.X + canvas.Width / 2, drawingPoint.Y + canvas.Height / 2);

            return drawingPoint;
        }

        private Point DrawingPointToQuiverPoint(System.Drawing.Point point)
        {
            // Drawing point with respect to the center of the canvas
            point = new System.Drawing.Point(point.X - canvas.Width / 2, point.Y - canvas.Height / 2);

            // Negation to go from computer science axes to math axes
            var quiverPoint = new Point(point.X, -point.Y);

            // Translate according to centerOfCanvas
            quiverPoint = quiverPoint + CenterOfCanvas;

            return quiverPoint;
        }

        /// <summary>
        /// Converts a difference of drawing points to a difference of quiver points, with both
        /// differences represented as points.
        /// </summary>
        /// <param name="drawingDelta"></param>
        /// <returns></returns>
        private Point DrawingDeltaToQuiverDelta(System.Drawing.Point drawingDelta)
        {
            return new Point(drawingDelta.X, -drawingDelta.Y);
        }

        private void DrawQuiver()
        {
            var graphics = inMemoryGraphics.Graphics;

            // Clear everything
            graphics.Clear(canvasBackgroundColor);

            // Draw vertices
            var vertexFont = canvas.Font;
            var vertexTextFormat = new StringFormat()
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Center,
            };
            
            foreach (var vertex in model.quiverInPlane.Vertices.Where(v => model.HasSelectedPath ? !model.SelectedPath.Vertices.Contains(v) : true))
            {
                DrawVertex(vertex, vertexBoundaryPen, defaultVertexBackgroundBrush);
            }

            if (model.HasSelectedPath)
            {
                foreach (var vertex in model.SelectedPath.Vertices.WithoutDuplicates())
                {
                    DrawVertex(vertex, vertexBoundaryPen, selectedVertexBackgroundBrush);
                }
            }

            // Draw arrows
            foreach (var arrow in model.quiverInPlane.GetArrows().Where(a => a != model.SelectedArrow && (model.HasSelectedPath ? !model.SelectedPath.Arrows.Contains(a) : true)))
            {
                DrawArrow(arrow, defaultArrowPen);
            }

            if (model.HasSelectedArrow)
            {
                var selectedArrow = model.SelectedArrow;
                DrawArrow(selectedArrow, selectedArrowPen);
            }
            else if (model.HasSelectedPath)
            {
                foreach (var arrow in model.SelectedPath.Arrows.WithoutDuplicates())
                {
                    DrawArrow(arrow, selectedArrowPen);
                }
            }

            inMemoryGraphics.Render();

            void DrawVertex(int vertex, Pen boundaryPen, Brush backgroundBrush)
            {
                var vertexPos = model.quiverInPlane.GetVertexPosition(vertex);
                var drawingVertexPos = QuiverPointToDrawingPoint(vertexPos);
                graphics.FillEllipse(backgroundBrush, drawingVertexPos.X - VertexRadius, drawingVertexPos.Y - VertexRadius, 2 * VertexRadius, 2 * VertexRadius);
                graphics.DrawArc(boundaryPen, drawingVertexPos.X - VertexRadius, drawingVertexPos.Y - VertexRadius, VertexRadius*2, VertexRadius*2, 0, 360);
                graphics.DrawString(vertex.ToString(), vertexFont, vertexTextBrush, new PointF(drawingVertexPos.X, drawingVertexPos.Y), vertexTextFormat);
            }

            void DrawArrow(Arrow<int> arrow, Pen pen)
            {
                var sourcePos = model.quiverInPlane.GetVertexPosition(arrow.Source);
                var targetPos = model.quiverInPlane.GetVertexPosition(arrow.Target);
                var sourceDrawingPosCenter = QuiverPointToDrawingPoint(sourcePos); // Center of the vertex
                var targetDrawingPosCenter = QuiverPointToDrawingPoint(targetPos); // Center of the vertex

                // Don't draw arrows between vertices too close to each other
                if (sourceDrawingPosCenter.DistanceSquared(targetDrawingPosCenter) <= 4*VertexRadiusSquare) return; // 4*VertexRadiusSquare = (2*VertexRadius)^2 appx. MinDistSquared

                (double X, double Y) vector = (targetDrawingPosCenter.X - sourceDrawingPosCenter.X, targetDrawingPosCenter.Y - sourceDrawingPosCenter.Y);
                var sourceDrawingOffset = vector.ScaleOriginBasedVectorToNorm(VertexRadius);
                var sourceDrawingPos = new System.Drawing.Point(sourceDrawingPosCenter.X + (int)sourceDrawingOffset.X, sourceDrawingPosCenter.Y + (int)sourceDrawingOffset.Y);

                (double X, double Y) targetDrawingOffset = (-sourceDrawingOffset.X, -sourceDrawingOffset.Y);
                var targetDrawingPos = new System.Drawing.Point(targetDrawingPosCenter.X + (int)targetDrawingOffset.X, targetDrawingPosCenter.Y + (int)targetDrawingOffset.Y);

                graphics.DrawArrow(pen, sourceDrawingPos.X, sourceDrawingPos.Y, targetDrawingPos.X, targetDrawingPos.Y);
            }
        }

        /// <summary>
        /// Gets a vertex containing a point in the canvas.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="vertex">Output parameter for the vertex. The returned value is
        /// <c>default</c> if no vertex containing the point exists.</param>
        /// <returns><see langword="true"/> if there is a vertex containing the point;
        /// <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para><paramref name="point"/> is a <see cref="SelfInjectiveQuiversWithPotential.Plane.Point"/>,
        /// not a <see cref="System.Drawing.Point"/> and hence represents a &quot;quiver point&quot;,
        /// not a &quot;drawing point&quot;.</para>
        /// </remarks>
        public bool TryGetVertex(Point point, out int vertex)
        {
            foreach (var vert in model.quiverInPlane.Vertices)
            {
                var vertexPos = model.quiverInPlane.GetVertexPosition(vert);
                if (vertexPos.SquareDistanceTo(point) < VertexRadiusSquare)
                {
                    vertex = vert;
                    return true;
                }
            }

            vertex = default;
            return false;
        }

        /// <summary>
        /// Gets an arrow containing a point in the canvas.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <param name="arrow">Output parameter for the arrow. The returned value is
        /// <see langword="null"/> if no arrow containing the point exists.</param>
        /// <returns><see langword="true"/> if there is an arrow containing the point;
        /// <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// <para><paramref name="point"/> is a <see cref="SelfInjectiveQuiversWithPotential.Plane.Point"/>,
        /// not a <see cref="System.Drawing.Point"/> and hence represents a &quot;quiver point&quot;,
        /// not a &quot;drawing point&quot;.</para>
        /// </remarks>
        public bool TryGetArrow(Point point, out Arrow<int> arrow)
        {
            const double MaxDistanceForClick = 3;
            var drawingClickPoint = QuiverPointToDrawingPoint(point);

            foreach (var arr in model.quiverInPlane.GetArrows())
            {
                var (arrowSourcePoint, arrowTargetPoint) = model.quiverInPlane.GetArrowEndpointPositions(arr);
                var drawingArrowSourcePoint = QuiverPointToDrawingPoint(arrowSourcePoint);
                var drawingArrowTargetPoint = QuiverPointToDrawingPoint(arrowTargetPoint);
                if (drawingClickPoint.DistanceToLineSegment(drawingArrowSourcePoint, drawingArrowTargetPoint) <= MaxDistanceForClick)
                {
                    arrow = arr;
                    return true;
                }
            }

            arrow = null;
            return false;
        }

        public void HandleException(Exception ex)
        {
            var exceptions = ex.GetInnerExceptions();
            var builder = new StringBuilder();
            foreach (var exception in exceptions)
            {
                builder.AppendLine(exception.Message);
            }

            var message = builder.ToString();

            MessageBox.Show(
                message,
                "Error occurred",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        public void HandleErrorMessage(string errorMessage)
        {
            MessageBox.Show(
                errorMessage,
                "Error occurred",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    defaultVertexBackgroundBrush.Dispose();
                    vertexBoundaryPen.Dispose();
                    selectedVertexBackgroundBrush.Dispose();
                    vertexTextBrush.Dispose();
                    defaultArrowPen.Dispose();
                    selectedArrowPen.Dispose();
                    inMemoryGraphics.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
