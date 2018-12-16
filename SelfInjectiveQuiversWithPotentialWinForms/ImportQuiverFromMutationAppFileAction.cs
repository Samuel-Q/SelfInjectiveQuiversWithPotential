using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential;
using SelfInjectiveQuiversWithPotential.Plane;
using SelfInjectiveQuiversWithPotential.Data;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// This class represents the action of importing a quiver in plane from a Mutation App file.
    /// </summary>
    /// <remarks>
    /// <para>Note that the results of the action of importing a quiver in plane
    /// <em>from a file</em> depends on external state, namely the contents of the file from which
    /// the quiver is imported. This state may change between the &quot;invocations&quot; of this
    /// action in a sequence <c>do, undo, redo</c>. This is problematic if the sequence of actions
    /// contain operations on the quiver after importing it: consider the sequence
    /// <c>do import, remove vertex 10, undo remove vertex 10, undo import, redo import, remove vertex 10</c>.
    /// This sequence of actions is reasonable if the imported quiver (after the first import)
    /// contains the vertex 10. If the quiver imported when redoing the import does not contain the
    /// vertex 10 (because the file was modified), then bad things will happen.</para>
    /// <para>One fix would be to change this action not to be undoable and redoable (clearing the
    /// stacks of undoable and redoable actions). Another fix (the one that was chosen) is to read
    /// from the file only once, namely when the action is first done, and store the quiver in
    /// memory. When the action is redone, the quiver is read from memory instead of from file.</para>
    /// </remarks>
    public class ImportQuiverFromMutationAppFileAction : IUndoableRedoableEditorAction
    {
        private readonly QuiverEditorModel model;
        private readonly string path;
        private readonly QuiverInPlane<int> quiverInPlaneBeforeAction;
        private QuiverInPlane<int> quiverInPlaneAfterAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportQuiverFromMutationAppFileAction"/>
        /// class.
        /// </summary>
        /// <param name="model">The quiver-editor model.</param>
        /// <param name="path">The path of the file from which to import the quiver.</param>
        /// <param name="quiverInPlaneBeforeAction">The quiver in plane before the action.</param>
        /// <para>This constructor takes care of copying
        /// <paramref name="quiverInPlaneBeforeAction"/> to ensure that the quiver in plane before
        /// the action that is stored in this <see cref="ImportQuiverFromMutationAppFileAction"/>
        /// is not modified.</para>
        public ImportQuiverFromMutationAppFileAction(QuiverEditorModel model, string path, QuiverInPlane<int> quiverInPlaneBeforeAction)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
            this.path = path ?? throw new ArgumentNullException(nameof(path));
            this.quiverInPlaneBeforeAction = quiverInPlaneBeforeAction?.Copy() ?? throw new ArgumentNullException(nameof(quiverInPlaneBeforeAction));
        }

        public void Do()
        {
            var importer = new QuiverInPlaneFromMutationAppImporter();
            var quiverInPlaneAfterAction = importer.ImportQuiverInPlane(path);

            // Copying the quiver is important, because actions done on this very object might be
            // undone on an equal but not reference-equal object. Consider the following example:
            /*
             * 1. Import (the action as a quiverInPlaneAfterAction)
		     * 2. Actions
		     * 3. Load/import (which stores a copy of quiverInPlane as quiverInPlaneBeforeAction)
		     * 4. Undo load/import (line 3)
			        quiverInPlane is now not reference-equal to quiverInPlaneAfterAction on line 1; it is reference-equal to the copy made on line 3
		     * 5. Undo actions (line 2)
			        This step did not modify quiverInPlaneAfterAction on line 1 but rather the copy made on line 3
		     * 6. Undo import
		     * 7. Redo import
			        This loads quiverInPlaneAfterAction, which is supposed to be equal to the quiver after line 1 but is equal to the quiver after line 2 !
		     * 8. Redo actions
			        This fails (e.g., if the action on line 2 was adding a vertex, then the vertex would already exist)
             */
            this.quiverInPlaneAfterAction = quiverInPlaneAfterAction.Copy();

            model.JustLoadQuiver(quiverInPlaneAfterAction);
        }

        public void Redo()
        {
            // Observe the call to Copy() to make sure that the stored quiver in plane after the
            // action is not inadvertently modified after redoing this action.
            model.JustLoadQuiver(quiverInPlaneAfterAction.Copy());
        }

        public void Undo()
        {
            // Observe the call to Copy() to make sure that the stored quiver in plane before the
            // action is not inadvertently modified after undoing this action.
            model.JustLoadQuiver(quiverInPlaneBeforeAction.Copy());
        }

        public override string ToString()
        {
            return $"Import quiver from Mutation App file";
        }
    }
}
