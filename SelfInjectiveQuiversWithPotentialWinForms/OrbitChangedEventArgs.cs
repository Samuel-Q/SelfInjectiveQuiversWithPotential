using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// Provides data for the <see cref="QuiverAnalyzerModel.OrbitChanged"/> event.
    /// </summary>
    public class OrbitChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the new orbit or <see langword="null"/> if there is no orbit.
        /// </summary>
        public IEnumerable<int> Orbit { get; }

        public OrbitChangedEventArgs(IEnumerable<int> orbit)
        {
            Orbit = orbit;
        }
    }
}
