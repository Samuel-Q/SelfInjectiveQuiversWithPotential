using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfInjectiveQuiversWithPotentialWinForms
{
    /// <summary>
    /// Defines the possible results of a call to <see cref="QuiverEditorModel"/>
    /// </summary>
    public enum RelabelVerticesResult
    {
        Success,
        DuplicateNewVertex,
        OldVertexNotInQuiver,
        NewVertexClashesWithPreExistingVertex
    }
}
