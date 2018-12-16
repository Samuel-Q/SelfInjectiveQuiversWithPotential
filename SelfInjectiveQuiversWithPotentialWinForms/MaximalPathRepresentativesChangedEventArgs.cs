using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// Provides data for the <see cref="QuiverAnalyzerModel.MaximalPathRepresentativesChanged"/> event.
    /// </summary>
    public class MaximalPathRepresentativesChangedEventArgs
    {
        /// <summary>
        /// Gets the maximal path representatives suitable for display, or <see langword="null"/>
        /// if it is suitable not to display any representatives.
        /// </summary>
        public IEnumerable<Path<int>> MaximalPathRepresentatives { get; }

        public MaximalPathRepresentativesChangedEventArgs(IEnumerable<Path<int>> maximalPathRepresentatives)
        {
            MaximalPathRepresentatives = maximalPathRepresentatives;
        }
    }
}
