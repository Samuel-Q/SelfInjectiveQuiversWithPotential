using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential;
using SelfInjectiveQuiversWithPotential.Data;
using SelfInjectiveQuiversWithPotential.Plane;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// This class represents the model component of the quiver editor.
    /// </summary>
    public class QuiverEditorModel
    {
        internal QuiverInPlane<int> quiverInPlane;

        private Stack<IUndoableRedoableEditorAction> actionsToUndo;

        public IUndoableRedoableEditorAction NextActionToUndo
        {
            get
            {
                if (actionsToUndo.Count == 0) return null;
                else return actionsToUndo.Peek();
            }
        }

        public bool CanUndoAction { get => NextActionToUndo != null; }

        private Stack<IUndoableRedoableEditorAction> actionsToRedo;

        public IUndoableRedoableEditorAction NextActionToRedo
        {
            get
            {
                if (actionsToRedo.Count == 0) return null;
                else return actionsToRedo.Peek();
            }
        }

        public bool CanRedoAction { get => NextActionToRedo != null; }

        /// <remarks>
        /// <para>Is <see langword="null"/> if no vertex is selected.</para>
        /// <para>In the case a path is selected, this property returns the ending point of the path.</para>
        /// </remarks>
        public int? SelectedVertex { get; private set; }

        public bool HasSelectedVertex { get => SelectedVertex.HasValue; }
        
        /// <remarks>Is <see langword="null"/> if no arrow is selected.</remarks>
        public Arrow<int> SelectedArrow { get; private set; }

        public bool HasSelectedArrow { get => SelectedArrow != null; }

        /// <remarks>
        /// <para>Is <see langword="null"/> if no path is selected.</para>
        /// <para>Selecting a vertex is equivalent to selecting a stationary path in the sense that
        /// the return value of this property is a stationary path if and only if the return value
        /// of <see cref="SelectedVertex"/> is non-null, in which case <see cref="SelectedPath"/>
        /// is of course the stationary path at <see cref="SelectedVertex"/>.</para>
        /// </remarks>
        public Path<int> SelectedPath { get; private set; }

        public bool HasSelectedPath { get => SelectedPath != null; }
        
        public QuiverEditorTool SelectedTool { get; private set; }

        public QuiverEditorToolSettings ToolSettings { get; private set; }

        public event EventHandler<VertexAddedEventArgs> VertexAdded;

        public event EventHandler<ArrowAddedEventArgs> ArrowAdded;

        public event EventHandler<VertexRemovedEventArgs> VertexRemoved;

        public event EventHandler<ArrowRemovedEventArgs> ArrowRemoved;

        public event EventHandler<VertexMovedEventArgs> VertexMoved;

        public event EventHandler<VertexSelectedEventArgs> VertexSelected;

        public event EventHandler<ArrowSelectedEventArgs> ArrowSelected;

        public event EventHandler<PathSelectedEventArgs> PathSelected;

        public event EventHandler<VertexDeselectedEventArgs> VertexDeselected;

        public event EventHandler<ArrowDeselectedEventArgs> ArrowDeselected;

        public event EventHandler<PathDeselectedEventArgs> PathDeselected;

        public event EventHandler<ToolSelectedEventArgs> ToolSelected;

        public event EventHandler<UndoableActionsChangedEventArgs> UndoableActionsChanged;
        public event EventHandler<RedoableActionsChangedEventArgs> RedoableActionsChanged;

        public event EventHandler<QuiverLoadedEventArgs> QuiverLoaded;

        public QuiverEditorModel(QuiverInPlane<int> quiverInPlane)
        {
            this.quiverInPlane = quiverInPlane;
            actionsToUndo = new Stack<IUndoableRedoableEditorAction>();
            actionsToRedo = new Stack<IUndoableRedoableEditorAction>();
            ToolSettings = new QuiverEditorToolSettings();
        }

        private void DoAction(IUndoableRedoableEditorAction action)
        {
            action.Do();
            actionsToUndo.Push(action);
            bool actionsToRedoChanged = actionsToRedo.Count > 0;
            actionsToRedo.Clear();
            UndoableActionsChanged?.Invoke(this, new UndoableActionsChangedEventArgs(action));
            if (actionsToRedoChanged) RedoableActionsChanged?.Invoke(this, new RedoableActionsChangedEventArgs(NextActionToRedo));
        }

        internal void JustAddVertex(int vertex, Point vertexPosition)
        {
            quiverInPlane.AddVertex(vertex, vertexPosition);
            VertexAdded?.Invoke(this, new VertexAddedEventArgs(vertex, vertexPosition));
        }

        public void AddVertex(int vertex, Point vertexPosition)
        {
            var action = new AddVertexAction(this, vertex, vertexPosition);
            DoAction(action);
        }

        public void AddNextVertex(Point vertexPosition)
        {
            int vertex = ToolSettings.VertexToAdd;
            int nextAvailableVertexNumber = Enumerable.Range(vertex + 1, vertex + 1 <= 0 ? Int32.MaxValue : Int32.MaxValue - (vertex + 1)).First(x => !quiverInPlane.ContainsVertex(x));
            ToolSettings.VertexToAdd = nextAvailableVertexNumber;
            AddVertex(vertex, vertexPosition);
        }

        public bool TryAddNextVertex(Point vertexPosition)
        {
            if (quiverInPlane.ContainsVertex(ToolSettings.VertexToAdd)) return false;
            AddNextVertex(vertexPosition);
            return true;
        }

        internal void JustRemoveVertex(int vertex)
        {
            if (SelectedVertex == vertex) DeselectVertex();
            quiverInPlane.RemoveVertex(vertex, out var arrowsRemoved);

            foreach (var arrow in arrowsRemoved) ArrowRemoved?.Invoke(this, new ArrowRemovedEventArgs(arrow));
            VertexRemoved?.Invoke(this, new VertexRemovedEventArgs(vertex));
        }

        internal void JustRemoveVertex(int vertex, out IEnumerable<Arrow<int>> arrowsRemoved)
        {
            if (SelectedVertex == vertex) DeselectVertex();
            quiverInPlane.RemoveVertex(vertex, out arrowsRemoved);

            foreach (var arrow in arrowsRemoved) ArrowRemoved?.Invoke(this, new ArrowRemovedEventArgs(arrow));
            VertexRemoved?.Invoke(this, new VertexRemovedEventArgs(vertex));
        }

        public void RemoveVertex(int vertex)
        {
            var arrowsToRemove = quiverInPlane.GetArrowsInvolvingVertex(vertex).ToList();
            var action = new RemoveVertexAction(this, vertex, arrowsToRemove);
            DoAction(action);
        }

        public void RemoveSelectedVertex()
        {
            if (!HasSelectedVertex) throw new ArgumentException($"No vertex is selected.");
            RemoveVertex(SelectedVertex.Value);
        }

        public void RemoveSelectedArrow()
        {
            if (!HasSelectedArrow) throw new ArgumentException($"No arrow is selected.");
            RemoveArrow(SelectedArrow);
        }

        internal void JustAddArrow(Arrow<int> arrow)
        {
            JustAddArrow(arrow.Source, arrow.Target);
        }

        internal void JustAddArrow(int sourceVertex, int targetVertex)
        {
            quiverInPlane.AddArrow(sourceVertex, targetVertex);
            ArrowAdded?.Invoke(this, new ArrowAddedEventArgs(new Arrow<int>(sourceVertex, targetVertex)));
        }

        public void AddArrow(Arrow<int> arrow)
        {
            if (arrow is null) throw new ArgumentNullException(nameof(arrow));
            AddArrow(arrow.Source, arrow.Target);
        }

        public void AddArrow(int sourceVertex, int targetVertex)
        {
            var action = new AddArrowAction(this, new Arrow<int>(sourceVertex, targetVertex));
            DoAction(action);
        }

        internal void JustRemoveArrow(Arrow<int> arrow)
        {
            JustRemoveArrow(arrow.Source, arrow.Target);
        }

        internal void JustRemoveArrow(int sourceVertex, int targetVertex)
        {
            var arrow = new Arrow<int>(sourceVertex, targetVertex);
            if (SelectedArrow == arrow) DeselectArrow();
            quiverInPlane.RemoveArrow(sourceVertex, targetVertex);
            ArrowRemoved?.Invoke(this, new ArrowRemovedEventArgs(arrow));
        }

        public void RemoveArrow(Arrow<int> arrow)
        {
            if (arrow is null) throw new ArgumentNullException(nameof(arrow));
            RemoveArrow(arrow.Source, arrow.Target);
        }

        public void RemoveArrow(int sourceVertex, int targetVertex)
        {
            var action = new RemoveArrowAction(this, new Arrow<int>(sourceVertex, targetVertex));
            DoAction(action);
        }

        public void SelectVertex(int vertex)
        {
            if (!quiverInPlane.ContainsVertex(vertex)) throw new ArgumentException($"The vertex {vertex} is not contained in the quiver.");

            if (SelectedVertex == vertex) return;

            if (HasSelectedVertex) DeselectVertex();
            if (HasSelectedArrow) DeselectArrow();
            SelectedVertex = vertex;
            SelectedPath = new Path<int>(vertex);

            VertexSelected?.Invoke(this, new VertexSelectedEventArgs(vertex));
            PathSelected?.Invoke(this, new PathSelectedEventArgs(SelectedPath));
        }

        public void DeselectVertex()
        {
            if (!HasSelectedVertex) return;
            int vertex = SelectedVertex.Value;
            SelectedVertex = null;
            var deselectedPath = SelectedPath;
            SelectedPath = null;

            VertexDeselected?.Invoke(this, new VertexDeselectedEventArgs(vertex));
            PathDeselected?.Invoke(this, new PathDeselectedEventArgs(deselectedPath));
        }

        public void SelectArrow(Arrow<int> arrow)
        {
            if (arrow is null) throw new ArgumentNullException(nameof(arrow));
            if (!quiverInPlane.ContainsVertex(arrow.Source)) throw new ArgumentException($"The source vertex {arrow.Source} is not contained in quiver.", nameof(arrow));
            if (!quiverInPlane.ContainsVertex(arrow.Target)) throw new ArgumentException($"The target vertex {arrow.Target} is not contained in the quiver.", nameof(arrow));
            if (!quiverInPlane.ContainsArrow(arrow)) throw new ArgumentException($"The arrow from {arrow} is not contained in the quiver.");

            if (SelectedArrow == arrow) return;

            if (HasSelectedVertex) DeselectVertex();
            if (HasSelectedArrow) DeselectArrow();
            SelectedArrow = arrow;
            ArrowSelected?.Invoke(this, new ArrowSelectedEventArgs(arrow));
        }

        public void SelectArrow(int sourceVertex, int targetVertex)
        {
            SelectArrow(new Arrow<int>(sourceVertex, targetVertex));
        }

        public void DeselectArrow()
        {
            if (!HasSelectedArrow) return;
            var arrow = SelectedArrow;
            SelectedArrow = null;
            ArrowDeselected?.Invoke(this, new ArrowDeselectedEventArgs(arrow));
        }

        public void ExtendSelectedPath(int vertex)
        {
            if (SelectedPath is null) throw new InvalidOperationException($"There is no selected path.");

            var arrowToExtendWith = new Arrow<int>(SelectedPath.EndingPoint, vertex);
            ExtendSelectedPath(arrowToExtendWith);
        }

        public void ExtendSelectedPath(Arrow<int> arrow)
        {
            if (arrow is null) throw new ArgumentNullException(nameof(arrow));
            if (SelectedPath is null) throw new InvalidOperationException($"There is no selected path.");
            if (!quiverInPlane.ContainsArrow(arrow)) throw new ArgumentException($"The quiver does not contain the arrow {arrow}.", nameof(arrow));
            if (arrow.Source != SelectedPath.EndingPoint) throw new ArgumentException($"The selected path {SelectedPath} does not end in the source of the arrow {arrow}.");

            SelectedVertex = arrow.Target;
            SelectedPath = SelectedPath.AppendArrow(arrow);

            VertexSelected?.Invoke(this, new VertexSelectedEventArgs(SelectedVertex.Value));
            PathSelected?.Invoke(this, new PathSelectedEventArgs(SelectedPath));
        }

        public void DeselectPath()
        {
            if (!HasSelectedPath) return;
            var deselectedPath = SelectedPath;
            SelectedPath = null;
            int deselectedVertex = deselectedPath.EndingPoint;
            SelectedVertex = null;

            VertexDeselected?.Invoke(this, new VertexDeselectedEventArgs(deselectedVertex));
            PathDeselected?.Invoke(this, new PathDeselectedEventArgs(deselectedPath));
        }

        /// <summary>
        /// Deselects vertex, arrow, and path.
        /// </summary>
        public void DeselectAll()
        {
            DeselectPath();
            DeselectArrow();
        }

        public void SelectTool(QuiverEditorTool tool)
        {
            var previousTool = SelectedTool;
            SelectedTool = tool;
            ToolSelected?.Invoke(this, new ToolSelectedEventArgs(previousTool, tool));
        }

        public void SetVertexToAdd(int vertexToAdd)
        {
            ToolSettings.VertexToAdd = vertexToAdd;
        }

        public void UndoAction()
        {
            if (!CanUndoAction) throw new InvalidOperationException($"There is no action to undo.");
            var action = actionsToUndo.Pop();
            action.Undo();

            actionsToRedo.Push(action);

            UndoableActionsChanged?.Invoke(this, new UndoableActionsChangedEventArgs(NextActionToUndo));
            RedoableActionsChanged?.Invoke(this, new RedoableActionsChangedEventArgs(action));
        }

        public void RedoAction()
        {
            if (!CanRedoAction) throw new InvalidOperationException($"There is no action to redo.");
            var action = actionsToRedo.Pop();
            action.Redo();

            actionsToUndo.Push(action);

            UndoableActionsChanged?.Invoke(this, new UndoableActionsChangedEventArgs(action));
            RedoableActionsChanged?.Invoke(this, new RedoableActionsChangedEventArgs(NextActionToRedo));
        }

        internal void JustLoadQuiver(QuiverInPlane<int> quiverInPlane)
        {
            DeselectPath();
            DeselectArrow();
            this.quiverInPlane = quiverInPlane;
            QuiverLoaded?.Invoke(this, new QuiverLoadedEventArgs(quiverInPlane));
        }

        public void LoadPredefinedQuiver(PredefinedQuiver predefinedQuiver, dynamic quiverParameter)
        {
            if (!predefinedQuiver.IsInEnum()) throw new ArgumentOutOfRangeException(nameof(predefinedQuiver));
            var action = new LoadPredefinedQuiverAction(this, predefinedQuiver, quiverParameter, quiverInPlane);
            DoAction(action);
        }

        /// <summary>
        /// Imports a quiver in plane for this model from a Mutation App file.
        /// </summary>
        /// <param name="path">The path of the file from which to import the quiver in plane.</param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/>.</exception>
        /// <exception cref="ImporterException">The quiver was not successfully imported from file.</exception>
        public void ImportQuiverFromMutationAppFile(string path)
        {
            if (path is null) throw new ArgumentNullException(nameof(path));
            var action = new ImportQuiverFromMutationAppFileAction(this, path, quiverInPlane);
            DoAction(action);
        }

        /// <summary>
        /// Exports the quiver in plane in this model to a file with the Mutation App format.
        /// </summary>
        /// <param name="path">The path of the file to which to export the quiver.</param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null"/>.</exception>
        /// <exception cref="ExporterException">The quiver was not successfully exported to file.</exception>
        public void ExportQuiverToMutationAppFile(string path)
        {
            if (path is null) throw new ArgumentNullException(nameof(path));
            var exporter = new QuiverInPlaneToMutationAppExporter();
            exporter.ExportQuiverInPlane(path, quiverInPlane);
        }

        internal void JustRelabelVertices(IReadOnlyDictionary<int, int> relabelingMap)
        {
            // Plan:
            // 1. Remove all old vertices
            // 2. Add all new vertices
            // 3. Add all arrows

            // 1. Remove all old vertices (and keep track of the necessary information)
            var newVertexPositions = new Dictionary<int, Point>();
            var allArrowsRemoved = new List<Arrow<int>>();
            foreach (var (oldVertex, newVertex) in relabelingMap)
            {
                newVertexPositions[newVertex] = quiverInPlane.GetVertexPosition(oldVertex);
                JustRemoveVertex(oldVertex, out var arrowsRemoved);
                allArrowsRemoved.AddRange(arrowsRemoved);
            }

            // 2. Add all new vertices
            foreach (var newVertex in relabelingMap.Values)
            {
                JustAddVertex(newVertex, newVertexPositions[newVertex]);
            }

            // 3. Add all arrows
            foreach (var arrow in allArrowsRemoved)
            {
                var source = relabelingMap.Keys.Contains(arrow.Source) ? relabelingMap[arrow.Source] : arrow.Source;
                var target = relabelingMap.Keys.Contains(arrow.Target) ? relabelingMap[arrow.Target] : arrow.Target;
                var arrowToAdd = new Arrow<int>(source, target);
                JustAddArrow(arrowToAdd);
            }
        }

        /// <summary>
        /// Relabels a subset of the vertices of the quiver according to the specified map.
        /// </summary>
        /// <param name="relabelingMap">A dictionary mapping old vertex labels to new vertex labels.</param>
        /// <returns>A <see cref="RelabelVerticesResult"/> indicating the result.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="relabelingMap"/> is <see langword="null"/>.</exception>
        public RelabelVerticesResult RelabelVertices(IReadOnlyDictionary<int, int> relabelingMap)
        {
            if (relabelingMap is null) throw new ArgumentNullException(nameof(relabelingMap));

            if (relabelingMap.Values.HasDuplicate()) return RelabelVerticesResult.DuplicateNewVertex;

            var oldVertices = new HashSet<int>(relabelingMap.Keys);
            if (oldVertices.Any(vertex => !quiverInPlane.ContainsVertex(vertex)))
                return RelabelVerticesResult.OldVertexNotInQuiver;

            if (relabelingMap.Values.Any(vertex => quiverInPlane.ContainsVertex(vertex) && !oldVertices.Contains(vertex)))
                return RelabelVerticesResult.NewVertexClashesWithPreExistingVertex;

            var action = new RelabelVerticesAction(this, relabelingMap);
            DoAction(action);

            return RelabelVerticesResult.Success;
        }

        private void JustMoveVertex(int vertex, Point position)
        {
            quiverInPlane.SetVertexPosition(vertex, position);
            VertexMoved?.Invoke(this, new VertexMovedEventArgs(vertex, position));
        }

        internal void JustRotateVertices(IEnumerable<int> vertices, Point center, double radians)
        {
            foreach (var vertex in vertices)
            {
                var oldPos = quiverInPlane.GetVertexPosition(vertex);
                var vectorToRotate = (X: oldPos.X - center.X, Y: oldPos.Y - center.Y);
                var rotatedVector = (X: Math.Cos(radians) * vectorToRotate.X - Math.Sin(radians) * vectorToRotate.Y, Y: Math.Sin(radians) * vectorToRotate.X + Math.Cos(radians) * vectorToRotate.Y);
                var newPos = new Point(center.X + rotatedVector.X, center.Y + rotatedVector.Y);
                JustMoveVertex(vertex, newPos);
            }
        }

        public RotateVerticesResult RotateVertices(IEnumerable<int> vertices, double centerX, double centerY, double degrees)
        {
            if (vertices is null) throw new ArgumentNullException(nameof(vertices));

            if (vertices.Any(vertex => !quiverInPlane.ContainsVertex(vertex)))
            {
                return RotateVerticesResult.VertexNotInQuiver;
            }

            var center = new Point(centerX, centerY);
            var radians = degrees * Math.PI / 180;
            var action = new RotateVerticesAction(this, vertices, center, radians);
            DoAction(action);

            return RotateVerticesResult.Success;
        }
    }
}
