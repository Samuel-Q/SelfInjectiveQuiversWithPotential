using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// Provides data for the <see cref="QuiverAnalyzerModel.EquivalentPathsChanged"/> event.
    /// </summary>
    public class EquivalentPathsChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the new equivalent paths.
        /// </summary>
        public IEnumerable<Path<int>> EquivalentPaths { get; }

        public EquivalentPathsChangedEventArgs(IEnumerable<Path<int>> equivalentPaths)
        {
            EquivalentPaths = equivalentPaths ?? throw new ArgumentNullException(nameof(equivalentPaths));
        }
    }
}
