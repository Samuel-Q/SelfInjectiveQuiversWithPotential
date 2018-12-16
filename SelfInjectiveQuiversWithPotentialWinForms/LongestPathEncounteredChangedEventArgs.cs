using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelfInjectiveQuiversWithPotential;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// Provides data for the <see cref="QuiverAnalyzerModelLongestPathEncounteredChanged"/> event.
    /// </summary>
    public class LongestPathEncounteredChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the new longest path encountered, which could possibly be <see langword="null"/>.
        /// </summary>
        public Path<int> LongestPathEncountered { get; }

        public LongestPathEncounteredChangedEventArgs(Path<int> longestPathEncountered)
        {
            LongestPathEncountered = longestPathEncountered;
        }
    }
}
