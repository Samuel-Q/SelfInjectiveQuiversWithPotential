using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// This class represents the controller component of the quiver analyzer.
    /// </summary>
    public class QuiverAnalyzerController
    {
        private readonly QuiverAnalyzerModel analyzerModel;
        private readonly QuiverAnalyzerView view;

        public QuiverAnalyzerController(QuiverAnalyzerModel model, QuiverAnalyzerView view)
        {
            analyzerModel = model ?? throw new ArgumentNullException(nameof(model));
            this.view = view ?? throw new ArgumentNullException(nameof(view));

            view.AnalyzeButtonClicked += View_AnalyzeButtonClicked;
        }

        private void View_AnalyzeButtonClicked(object sender, EventArgs e)
        {
            analyzerModel.Analyze();
        }
    }
}
