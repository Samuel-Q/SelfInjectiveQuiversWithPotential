using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential;
using SelfInjectiveQuiversWithPotential.Plane;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    public class LoadPredefinedQuiverAction : IUndoableRedoableEditorAction
    {
        private readonly QuiverEditorModel model;
        private readonly PredefinedQuiver predefinedQuiver;
        private readonly dynamic quiverParameter;
        private readonly QuiverInPlane<int> quiverInPlaneBeforeAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadPredefinedQuiverAction"/> class.
        /// </summary>
        /// <param name="model">The quiver-editor model.</param>
        /// <param name="predefinedQuiver">The type of predefined quiver to load.</param>
        /// <param name="quiverParameter">The parameter for the predefined quiver to load.</param>
        /// <param name="quiverInPlaneBeforeAction">The quiver in plane before the action.</param>
        /// <remarks>
        /// <para>This constructor takes care of copying
        /// <paramref name="quiverInPlaneBeforeAction"/> to ensure that the quiver in plane before
        /// the action that is stored in this <see cref="LoadPredefinedQuiverAction"/> is not
        /// modified.</para>
        /// </remarks>
        public LoadPredefinedQuiverAction(QuiverEditorModel model, PredefinedQuiver predefinedQuiver, dynamic quiverParameter, QuiverInPlane<int> quiverInPlaneBeforeAction)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
            if (!predefinedQuiver.IsInEnum()) throw new ArgumentOutOfRangeException(nameof(predefinedQuiver));
            this.predefinedQuiver = predefinedQuiver;
            this.quiverParameter = quiverParameter;
            this.quiverInPlaneBeforeAction = quiverInPlaneBeforeAction?.Copy() ?? throw new ArgumentNullException(nameof(quiverInPlaneBeforeAction));
        }

        private QuiverInPlane<int> GetPredefinedCycleQuiverInPlane(dynamic quiverParameter)
        {
            int numVertices = quiverParameter;
            const int Radius = 200;
            return UsefulQuiversInPlane.GetCycleQuiverInPlane(numVertices, Radius);
        }

        private QuiverInPlane<int> GetPredefinedTriangleQuiverInPlane(dynamic quiverParameter)
        {
            int numRows = quiverParameter;
            int radius = 60 * (numRows - 1);
            return UsefulQuiversInPlane.GetTriangleQuiverInPlane(numRows, radius);
        }

        private QuiverInPlane<int> GetPredefinedSquareQuiverInPlane(dynamic quiverParameter)
        {
            int numRows = quiverParameter;
            int width = 100 * (numRows - 1);
            return UsefulQuiversInPlane.GetSquareQuiverInPlane(numRows, width);
        }

        private QuiverInPlane<int> GetPredefinedCobwebQuiverInPlane(dynamic quiverParameter)
        {
            int numVerticesInCenterPolygon = quiverParameter;
            const int InnermostRadius = 50;
            return UsefulQuiversInPlane.GetCobwebQuiverInPlane(numVerticesInCenterPolygon, InnermostRadius);
        }

        private QuiverInPlane<int> GetPredefinedOddFlowerQuiverInPlane(dynamic quiverParameter)
        {
            int numVerticesInCenterPolygon = quiverParameter;
            const int InnermostRadius = 50;
            return UsefulQuiversInPlane.GetOddFlowerQuiverInPlane(numVerticesInCenterPolygon, InnermostRadius);
        }

        private QuiverInPlane<int> GetPredefinedEvenFlowerType1QuiverInPlane(dynamic quiverParameter)
        {
            int numVerticesInCenterPolygon = quiverParameter;
            const int InnermostRadius = 50;
            return UsefulQuiversInPlane.GetEvenFlowerType1QuiverInPlane(numVerticesInCenterPolygon, InnermostRadius);
        }

        private QuiverInPlane<int> GetPredefinedEvenFlowerType2QuiverInPlane(dynamic quiverParameter)
        {
            int numVerticesInCenterPolygon = quiverParameter;
            const int InnermostRadius = 50;
            return UsefulQuiversInPlane.GetEvenFlowerType2QuiverInPlane(numVerticesInCenterPolygon, InnermostRadius);
        }

        private QuiverInPlane<int> GetPredefinedPointedFlowerQuiverInPlane(dynamic quiverParameter)
        {
            int numPeriods = quiverParameter;
            const int InnermostRadius = 50;
            return UsefulQuiversInPlane.GetPointedFlowerQuiverInPlane(numPeriods, InnermostRadius);
        }

        private QuiverInPlane<int> GetPredefinedGeneralizedCobwebQuiverInPlane(dynamic quiverParameter)
        {
            var (numVerticesInCenterPolygon, numLayers) = ((int, int))quiverParameter;
            const int InnermostRadius = 50;
            return UsefulQuiversInPlane.GetGeneralizedCobwebQuiverInPlane(numVerticesInCenterPolygon, numLayers, InnermostRadius);
        }

        public void Do()
        {
            QuiverInPlane<int> quiverInPlane;
            switch (predefinedQuiver)
            {
                case PredefinedQuiver.Cycle: quiverInPlane = GetPredefinedCycleQuiverInPlane(quiverParameter); break;
                case PredefinedQuiver.Triangle: quiverInPlane = GetPredefinedTriangleQuiverInPlane(quiverParameter); break;
                case PredefinedQuiver.Square: quiverInPlane = GetPredefinedSquareQuiverInPlane(quiverParameter); break;
                case PredefinedQuiver.Cobweb: quiverInPlane = GetPredefinedCobwebQuiverInPlane(quiverParameter); break;
                case PredefinedQuiver.OddFlower: quiverInPlane = GetPredefinedOddFlowerQuiverInPlane(quiverParameter); break;
                case PredefinedQuiver.EvenFlowerType1: quiverInPlane = GetPredefinedEvenFlowerType1QuiverInPlane(quiverParameter); break;
                case PredefinedQuiver.EvenFlowerType2: quiverInPlane = GetPredefinedEvenFlowerType2QuiverInPlane(quiverParameter); break;
                case PredefinedQuiver.PointedFlower: quiverInPlane = GetPredefinedPointedFlowerQuiverInPlane(quiverParameter); break;
                case PredefinedQuiver.GeneralizedCobweb: quiverInPlane = GetPredefinedGeneralizedCobwebQuiverInPlane(quiverParameter); break;
                default: System.Diagnostics.Debug.Fail($"Enum value was not member of enum despite making sure that it is."); return;
            }

            model.JustLoadQuiver(quiverInPlane);
        }

        public void Redo()
        {
            Do();
        }

        public void Undo()
        {
            // Observe the call to Copy() to make sure that the stored quiver in plane is not
            // inadvertently modified after undoing this action.
            model.JustLoadQuiver(quiverInPlaneBeforeAction.Copy());
        }

        public override string ToString()
        {
            return $"Load {predefinedQuiver} (parameter {quiverParameter})";
        }
    }
}
